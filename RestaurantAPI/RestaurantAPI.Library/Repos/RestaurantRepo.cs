using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Library.Repos
{
    public class RestaurantRepo : IRestaurantRepo
    {
        private readonly Project2DBContext _db;

        public RestaurantRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //parameterless contstructor to enable moq-ing
        public RestaurantRepo() { }

        /// <summary>
        /// Primary method for retriving all restaurants from the database. By default includes no info from junciton tables.
        /// </summary>
        /// <returns>Returns an IQueryable containing all restaurants in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<Restaurant> GetRestaurants()
        {
            return _db.Restaurant.AsNoTracking();
        }

        /// <summary>
        /// Overload for retrieving all restaurants from the database.  Use parameter to distinguish whether information from junciton tables is required or not.
        /// </summary>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Returns an IQueryable containing all restaurants in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<Restaurant> GetRestaurants(bool includeAll)
        {
            if (includeAll)
                return _db.Restaurant.AsNoTracking().Include(m => m.Blacklist).Include(m => m.Favorite).Include(m => m.RestaurantKeywordJunction);
            return _db.Restaurant.AsNoTracking();
        }

        /// <summary>
        /// Checks whether DB conatins a restaurant with the given ID
        /// </summary>
        /// <param name="Id">ID to search DB for</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public bool DBContainsRestaurant(string Id)
        {
            if (Id == null)
                return false;
            return GetRestaurants(false).Any(t => t.Id.Equals(Id));
        }

        
        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public Restaurant GetRestaurantByID(string Id)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return GetRestaurants().First(t => t.Id.Equals(Id));
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// Overload which includes all junction table info depending on bool parameter
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Restaurant object with specified ID</returns>
        public Restaurant GetRestaurantByID(string Id, bool includeAll)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return GetRestaurants(includeAll).First(t => t.Id.Equals(Id));
        }
        
        /// <summary>
        /// Adds the given Restaurant object to the database
        /// Throws an exception if the Id is already set to some value, as SQL is set to generate a new ID.
        /// Throws an exception if a restraunt already exists in the DB with that given name and location.
        /// </summary>
        /// <param name="r"></param>
        public void AddRestaurant(Restaurant r)
        {
            if (DBContainsRestaurant(r.Id))
                throw new DbUpdateException("Invalid ID. ID must be unique to add a Restaurant to DB", new NotSupportedException());
            if(r.Name == null)
                throw new DbUpdateException("Invalid Name. Restaurant name must be non-null", new ArgumentNullException("r.Name"));
            if (r.Lat == null)
                throw new DbUpdateException("Invalid location. Longitude and Latitude must be non-null", new ArgumentNullException("r.Lat"));
            if (r.Lon == null)
                throw new DbUpdateException("Invalid location. Longitude and Latitude must be non-null", new ArgumentNullException("r.Lon"));
            _db.Restaurant.Add(r);
        }

        /// <summary>
        /// Given a list of Restaurants, adds all to DB that are not already in it.
        /// Will also register the given keywords in the RestaurantKeywordJunctionTable for each Restaurant
        /// Assumes All keywords already exist in DB from registering QueryKeywordJunction before this
        /// </summary>
        /// <param name="rList">list of restaurants</param>
        /// <param name="keywords">list of keywords (pass empty list if none)</param>
        public void AddNewRestaurants(List<Restaurant> rList, List<string> keywords)
        {
            if (rList == null)
                throw new DbUpdateException("Restaurant list cannot be null.", new ArgumentNullException("rList"));

            foreach (Restaurant r in rList)
            {
                r.Owner = null;  //making null since owner will be added in later, not when restaurant is first added
                if (!DBContainsRestaurant(r.Id))
                    AddRestaurant(r);

                if (keywords != null)
                {
                    foreach (string k in keywords)
                    {
                        if (!_db.RestaurantKeywordJunction.Any(t => t.RestaurantId.Equals(r.Id)&&t.Word.Equals(k)))
                            _db.RestaurantKeywordJunction.Add(new RestaurantKeywordJunction() { RestaurantId = r.Id, Word = k.ToLower() });
                    }
                }
            }
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }




        //Async versions of above methods as needed:

        /// <summary>
        /// Checks whether DB conatins a restaurant with the given ID
        /// </summary>
        /// <param name="Id">ID to search DB for</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public async Task<bool> DBContainsRestaurantAsync(string Id)
        {
            if (Id == null)
                return false;
            return await GetRestaurants(false).AnyAsync(t => t.Id.Equals(Id));
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public async Task<Restaurant> GetRestaurantByIDAsync(string Id)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return await GetRestaurants().FirstAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// Overload which includes all junction table info depending on bool parameter
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Restaurant object with specified ID</returns>
        public async Task<Restaurant> GetRestaurantByIDAsync(string Id, bool includeAll)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return await GetRestaurants(includeAll).FirstAsync(t => t.Id.Equals(Id));
        }

        /// <summary>
        /// Adds the given Restaurant object to the database
        /// Throws an exception if the Id is already set to some value, as SQL is set to generate a new ID.
        /// Throws an exception if a restraunt already exists in the DB with that given name and location.
        /// </summary>
        /// <param name="r"></param>
        public async Task AddRestaurantAsync(Restaurant r)
        {
            var contains = await DBContainsRestaurantAsync(r.Id);
            if (contains)
                throw new DbUpdateException("Invalid ID. ID must be unique to add a Restaurant to DB", new NotSupportedException());
            if (r.Name == null)
                throw new DbUpdateException("Invalid Name. Restaurant name must be non-null", new ArgumentNullException("r.Name"));
            if (r.Lat == null)
                throw new DbUpdateException("Invalid location. Longitude and Latitude must be non-null", new ArgumentNullException("r.Lat"));
            if (r.Lon == null)
                throw new DbUpdateException("Invalid location. Longitude and Latitude must be non-null", new ArgumentNullException("r.Lon"));
            _db.Restaurant.Add(r);
        }

        /// <summary>
        /// Given a list of Restaurants, adds all to DB that are not already in it.
        /// Will also register the given keywords in the RestaurantKeywordJunctionTable for each Restaurant
        /// Assumes All keywords already exist in DB from registering QueryKeywordJunction before this
        /// </summary>
        /// <param name="rList">list of restaurants</param>
        /// <param name="keywords">list of keywords (pass empty list if none)</param>
        public async Task AddNewRestaurantsAsync(List<Restaurant> rList, List<string> keywords)
        {
            if (rList == null)
                throw new DbUpdateException("Restaurant list cannot be null.", new ArgumentNullException("rList"));

            foreach (Restaurant r in rList)
            {
                r.Owner = null;  //making null since owner will be added in later, not when restaurant is first added
                bool contains = await DBContainsRestaurantAsync(r.Id);
                if (!contains)
                    AddRestaurant(r);

                if (keywords != null)
                {
                    foreach (string k in keywords)
                    {
                        contains = await _db.RestaurantKeywordJunction.AnyAsync(t => t.RestaurantId.Equals(r.Id) && t.Word.Equals(k));
                        if (!contains)
                            _db.RestaurantKeywordJunction.Add(new RestaurantKeywordJunction() { RestaurantId = r.Id, Word = k.ToLower() });
                    }
                }
            }
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}

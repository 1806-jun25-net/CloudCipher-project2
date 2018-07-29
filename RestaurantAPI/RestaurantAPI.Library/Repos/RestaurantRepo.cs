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
        public bool DBContainsRestaurant(int Id)
        {
            return GetRestaurants(false).Any(t => t.Id == Id);
        }

        /// <summary>
        /// Checks whether DB conatins a restaurant as identified by the given name and location
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public bool DBContainsRestaurant(string name, string location)
        {
            if (name == null || location == null)
                return false;
            return GetRestaurants(false).Any(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public Restaurant GetRestaurantByID(int Id)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return GetRestaurants().First(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// Overload which includes all junction table info depending on bool parameter
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Restaurant object with specified ID</returns>
        public Restaurant GetRestaurantByID(int Id, bool includeAll)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return GetRestaurants(includeAll).First(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns the Restaurant object.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>the Restaurant object specified</returns>
        public Restaurant GetRestaurantByNameAndLocation(string name, string location)
        {
            if (name == null || location == null || !DBContainsRestaurant(name, location))
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return GetRestaurants().First(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns the Restaurant object.
        /// Overload which includes all junction table info depending on bool parameter
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>the Restaurant object specified</returns>
        public Restaurant GetRestaurantByNameAndLocation(string name, string location, bool includeAll)
        {
            if (name == null || location == null || !DBContainsRestaurant(name, location))
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return GetRestaurants(includeAll).First(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns its ID number.
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>ID number of specified Restaurant</returns>
        public int GetRestaurantIDByNameAndLocation(string name, string location)
        {
            if (name == null || location==null || !DBContainsRestaurant(name, location))
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return GetRestaurants().First(t => t.Name.Equals(name) && t.Location.Equals(location)).Id;
        }

        /// <summary>
        /// Adds the given Restaurant object to the database
        /// Throws an exception if the Id is already set to some value, as SQL is set to generate a new ID.
        /// Throws an exception if a restraunt already exists in the DB with that given name and location.
        /// </summary>
        /// <param name="r"></param>
        public void AddRestaurant(Restaurant r)
        {
            if (DBContainsRestaurant(r.Id) || r.Id > 0)
                throw new DbUpdateException("Invalid ID. ID should not be set prior to adding a new Restaurant to the database.  Identity constraint does that for you.", new NotSupportedException());
            if (DBContainsRestaurant(r.Name, r.Location))
                throw new DbUpdateException("Restraunt with that Name and location already exists in the DB.  Cannot add another.", new NotSupportedException());

            _db.Restaurant.Add(r);
        }

        /// <summary>
        /// Given a list of Restaurants, adds all to DB that are not already in it.
        /// Will also register the given keywords in the RestaurantKeywordJunctionTable for each Restaurant
        /// </summary>
        /// <param name="rList">list of restaurants</param>
        /// <param name="keywords">list of keywords (pass empty list if none)</param>
        public void AddNewRestaurants(List<Restaurant> rList, List<string> keywords)
        {
            if (rList == null)
                throw new DbUpdateException("Restaurant list cannot be null.", new ArgumentNullException());

            foreach (Restaurant r in rList)
            {
                if (keywords != null)
                {
                    foreach (string k in keywords)
                    {
                        try
                        {
                            _db.RestaurantKeywordJunction.Add(new RestaurantKeywordJunction() { RestaurantId = r.Id, Word = k });
                        }
                        catch { }
                    }
                }
                try
                {
                    AddRestaurant(r);
                }
                catch {}
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
        public async Task<bool> DBContainsRestaurantAsync(int Id)
        {
            return await GetRestaurants(false).AnyAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Checks whether DB conatins a restaurant as identified by the given name and location
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public async Task<bool> DBContainsRestaurantAsync(string name, string location)
        {
            if (name == null || location == null)
                return false;
            return await GetRestaurants(false).AnyAsync(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public async Task<Restaurant> GetRestaurantByIDAsync(int Id)
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
        public async Task<Restaurant> GetRestaurantByIDAsync(int Id, bool includeAll)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return await GetRestaurants(includeAll).FirstAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns the Restaurant object.
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>the Restaurant object specified</returns>
        public async Task<Restaurant> GetRestaurantByNameAndLocationAsync(string name, string location)
        {
            var contains = await DBContainsRestaurantAsync(name, location);
            if (!contains)
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return await GetRestaurants().FirstAsync(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns the Restaurant object.
        /// Overload which includes all junction table info depending on bool parameter
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>the Restaurant object specified</returns>
        public async Task<Restaurant> GetRestaurantByNameAndLocationAsync(string name, string location, bool includeAll)
        {
            var contains = await DBContainsRestaurantAsync(name, location);
            if (!contains)
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return await GetRestaurants(includeAll).FirstAsync(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns its ID number.
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>ID number of specified Restaurant</returns>
        public async Task<int> GetRestaurantIDByNameAndLocationAsync(string name, string location)
        {
            var contains = await DBContainsRestaurantAsync(name, location);
            if (!contains)
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return await GetRestaurants().Where(t => t.Name.Equals(name) && t.Location.Equals(location)).Select(t => t.Id).FirstAsync();
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

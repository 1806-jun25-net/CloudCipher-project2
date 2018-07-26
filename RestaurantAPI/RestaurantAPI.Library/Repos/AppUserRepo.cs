using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Library.Repos
{
    public class AppUserRepo
    {
        private readonly Project2DBContext _db;

        public AppUserRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //Read methods: will retrieve data from DB but not make changes

        /// <summary>
        /// Primary method for retriving all users from the database.  By default only returns basic values and none of the data from junction tables.
        /// </summary>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public async Task<List<AppUser>> GetUsers()
        {
            return await _db.AppUser.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Overload of method for retriving all users from the database.  Use parameter to distinguish whether information from junciton tables is required or not.
        /// </summary>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public async Task<List<AppUser>> GetUsers(bool includeAll)
        {
            if (includeAll)
                return await _db.AppUser.AsNoTracking().Include(m => m.Blacklist).ThenInclude(k => k.Restaurant).Include(m => m.Favorite).ThenInclude(k => k.Restaurant).Include(m => m.Query).ThenInclude(k => k.QueryKeywordJunction).Include(m => m.Restaurant).ToListAsync();
            return await _db.AppUser.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Overload for selecting which specific junciton table information is requred for list of users
        /// </summary>
        /// <param name="includeBlacklist">Whether to include the list of blacklisted restaurants for that user</param>
        /// <param name="includeFavorites">Whether to include the list of favorited restaurants for that user</param>
        /// <param name="includeQueries">Whether to include the list of queries for that user</param>
        /// <param name="includeOwnedRestaurants">Whether to include the list of restaurants registered as owned by that user</param>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public async Task<List<AppUser>> GetUsers(bool includeBlacklist, bool includeFavorites, bool includeQueries, bool includeOwnedRestaurants)
        {
            IQueryable<AppUser> result = _db.AppUser.AsNoTracking();
            if (includeBlacklist)
                result.Include(m => m.Blacklist).ThenInclude(k=>k.Restaurant);
            if (includeFavorites)
                result.Include(m => m.Favorite).ThenInclude(k => k.Restaurant);
            if (includeQueries)
                result.Include(m => m.Query).ThenInclude(k => k.QueryKeywordJunction);
            if (includeOwnedRestaurants)
                result.Include(m => m.Restaurant);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <returns>AppUser object with specified username</returns>
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            return await _db.AppUser.AsNoTracking().FirstAsync(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  Includes junction table data depending on bool parameter
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>AppUser object with specified username</returns>
        public async Task<AppUser> GetUserByUsername(string username, bool includeAll)
        {
            if (!includeAll)
                return await GetUserByUsernameAsync(username);
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
                return await _db.AppUser.AsNoTracking().FirstAsync(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  Overload version to include only the lists you specify
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <param name="includeBlacklist">Whether to include the list of blacklisted restaurants for that user</param>
        /// <param name="includeFavorites">Whether to include the list of favorited restaurants for that user</param>
        /// <param name="includeQueries">Whether to include the list of queries for that user</param>
        /// <param name="includeOwnedRestaurants">Whether to include the list of restaurants registered as owned by that user</param>
        /// <returns>AppUser object with specified username</returns>
        public async Task<AppUser> GetUserByUsername(string username, bool includeBlacklist, bool includeFavorites, bool includeQueries, bool includeOwnedRestaurants)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            IQueryable<AppUser> result = _db.AppUser.AsNoTracking();
            if (includeBlacklist)
                result.Include(m => m.Blacklist).ThenInclude(k => k.Restaurant);
            if (includeFavorites)
                result.Include(m => m.Favorite).ThenInclude(k => k.Restaurant);
            if (includeQueries)
                result.Include(m => m.Query).ThenInclude(k => k.QueryKeywordJunction);
            if (includeOwnedRestaurants)
                result.Include(m => m.Restaurant);
            return await result.FirstAsync(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Determines whether a user with the given username exists in the DB
        /// </summary>
        /// <param name="username">name of user to retrieve blacklist for</param>
        /// <returns>collection containing all restaurants listed in the given user's blacklist</returns>
        public async Task<bool> DBContainsUsername(string username)
        {
            if (username==null)
                throw new ArgumentNullException();
            return await _db.AppUser.AsNoTracking().AnyAsync(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their blacklist
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up blacklist for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetBlacklistForUser(string username)
        {
            var userTask= GetUserByUsername(username, true, false, false, false);
            userTask.Wait();
            return userTask.Result.Blacklist.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their favorites
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up favorites for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetFavoritesForUser(string username)
        {
            var userTask = GetUserByUsername(username, false, true, false, false);
            userTask.Wait();
            return userTask.Result.Favorite.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of queries made by the given user
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up queries for</param>
        /// <returns>IEnumerable of query objects</returns>
        public IEnumerable<Query> GetQueriesForUser(string username)
        {
            var userTask = GetUserByUsername(username, false, false, true, false);
            userTask.Wait();
            return userTask.Result.Query;
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants a user is listed as owning
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up owned restaurants for</param>
        /// <returns>IEnumerable of restaurant objects</returns>
        public IEnumerable<Restaurant> GetOwnedRestaurantsForUser(string username)
        {
            var userTask = GetUserByUsername(username, false, false, false, true);
            userTask.Wait();
            return userTask.Result.Restaurant;
        }


        // Create methods:  add new data to the DB

        /// <summary>
        /// Adds a new AppUser object to the DB
        /// Throws an exception if that username is already in use to avoid violating PK constraint.
        /// Must still call Save() after all DB updates are finished.
        /// </summary>
        /// <param name="u">AppUser object to be added to DB</param>
        public void AddUser(AppUser u)
        {
            var taskResult = DBContainsUsername(u.Username);
            taskResult.Wait();
            if (taskResult.Result)
                throw new DbUpdateException("That username is already in the database.  Usernames must be unique to add a new user", new NotSupportedException());
            _db.Add(u);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is blaclisting</param>
        /// <param name="r">Restraunt object to be blacklisted</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(u.Username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Blacklist bl = new Blacklist() { Username = u.Username, RestaurantId = r.Id };
            _db.Add(bl);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantName">string containing restaurant's name</param>
        /// <param name="restaurantLocation">string containing restaurant's location</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = r.Id };
            _db.Add(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(string username, int restaurantId, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            if (!rRepo.DBContainsRestaurant(restaurantId))
                throw new NotSupportedException($"Restaurant ID '{restaurantId}' not found.");
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = restaurantId };
            _db.Add(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is Favoriting</param>
        /// <param name="r">Restraunt object to be Favorited</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(u.Username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Favorite fav = new Favorite() { Username = u.Username, RestaurantId = r.Id };
            _db.Add(fav);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantName">string containing restaurant's name</param>
        /// <param name="restaurantLocation">string containing restaurant's location</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            Favorite fav = new Favorite() { Username = username, RestaurantId = r.Id };
            _db.Add(fav);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(string username, int restaurantId, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsername(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            if (!rRepo.DBContainsRestaurant(restaurantId))
                throw new NotSupportedException($"Restaurant ID '{restaurantId}' not found.");
            Favorite fav = new Favorite() { Username = username, RestaurantId = restaurantId };
            _db.Add(fav);
        }


        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

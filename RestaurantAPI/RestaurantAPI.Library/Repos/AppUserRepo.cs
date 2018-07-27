﻿using Microsoft.EntityFrameworkCore;
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

        //ORIGINAL NON ASYNC VERSIONS

        /// <summary>
        /// Primary method for retriving all users from the database.  By default only returns basic values and none of the data from junction tables.
        /// Per Nick, since it returns an IQueryable it does not need an Async version.
        /// </summary>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<AppUser> GetUsers()
        {
            return _db.AppUser.AsNoTracking();
        }

        /// <summary>
        /// Overload of method for retriving all users from the database.  Use parameter to distinguish whether information from junciton tables should be included or not.
        /// </summary>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<AppUser> GetUsers(bool includeAll)
        {
            if (includeAll)
                return _db.AppUser.AsNoTracking().Include(m => m.Blacklist).ThenInclude(k => k.Restaurant).Include(m => m.Favorite).ThenInclude(k => k.Restaurant).Include(m => m.Query).ThenInclude(k => k.QueryKeywordJunction).Include(m => m.Restaurant);
            return _db.AppUser.AsNoTracking();
        }


        /// <summary>
        /// Determines whether a user with the given username exists in the DB
        /// </summary>
        /// <param name="username">name of user to retrieve blacklist for</param>
        /// <returns>collection containing all restaurants listed in the given user's blacklist</returns>
        public bool DBContainsUsername(string username)
        {
            return GetUsers().Any(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  
        /// By default will not include any list/Juntion table data.  If junction table data is required, use overload below
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <returns>AppUser object with specified username</returns>
        public AppUser GetUserByUsername(string username)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            return GetUsers().First(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  Includes junction table data depending on bool parameter.
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>AppUser object with specified username</returns>
        public AppUser GetUserByUsername(string username, bool includeAll)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            return GetUsers(includeAll).First(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their blacklist.
        /// Throws an exception if username not found.
        /// </summary>
        /// <param name="username">Username to look up blacklist for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetBlacklistForUser(string username)
        {
            return GetUserByUsername(username, true).Blacklist.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their favorites
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up favorites for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetFavoritesForUser(string username)
        {
            return GetUserByUsername(username, true).Favorite.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of queries made by the given user
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up queries for</param>
        /// <returns>IEnumerable of query objects</returns>
        public IEnumerable<Query> GetQueriesForUser(string username)
        {
            return GetUserByUsername(username, true).Query;
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants a user is listed as owning
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up owned restaurants for</param>
        /// <returns>IEnumerable of restaurant objects</returns>
        public IEnumerable<Restaurant> GetOwnedRestaurantsForUser(string username)
        {
            return GetUserByUsername(username, true).Restaurant;
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
            if (DBContainsUsername(u.Username))
                throw new DbUpdateException("That username is already in the database.  Usernames must be unique to add a new user", new NotSupportedException());
            _db.Add(u);
        }
        
        
        //Add Methods- store new data in DB

        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Blacklist already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is blaclisting</param>
        /// <param name="r">Restraunt object to be blacklisted</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(u.Username))
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Blacklist bl = new Blacklist() { Username = u.Username, RestaurantId = r.Id };
            _db.Add(bl);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Blacklist already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantName">string containing restaurant's name</param>
        /// <param name="restaurantLocation">string containing restaurant's location</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = r.Id };
            _db.Add(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Blacklist already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToBlacklist(string username, int restaurantId, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            if (!rRepo.DBContainsRestaurant(restaurantId))
                throw new NotSupportedException($"Restaurant ID '{restaurantId}' not found.");
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = restaurantId };
            _db.Add(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Favorite list already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is Favoriting</param>
        /// <param name="r">Restraunt object to be Favorited</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(u.Username))
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Favorite fav = new Favorite() { Username = u.Username, RestaurantId = r.Id };
            _db.Add(fav);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Favorite list already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantName">string containing restaurant's name</param>
        /// <param name="restaurantLocation">string containing restaurant's location</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            Favorite fav = new Favorite() { Username = username, RestaurantId = r.Id };
            _db.Add(fav);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Also throws exception if Favorite list already exists for that user
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public void AddRestaurantToFavorites(string username, int restaurantId, RestaurantRepo rRepo)
        {
            if (!DBContainsUsername(username))
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









        //ATTEMPT AT ASCYNC VERSIONS


        //Read methods: will retrieve data from DB but not make changes


        //Per Nick, don't need async versions of the GetUsers methods or others that return IQueryable

        /// <summary>
        /// Determines whether a user with the given username exists in the DB
        /// </summary>
        /// <param name="username">name of user to retrieve blacklist for</param>
        /// <returns>collection containing all restaurants listed in the given user's blacklist</returns>
        public async Task<bool> DBContainsUsernameAsync(string username)
        {
            if (username == null)
                throw new ArgumentNullException();
            return await _db.AppUser.AsNoTracking().AnyAsync(t => t.Username.Equals(username));
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
            var task = DBContainsUsernameAsync(username);
            task.Wait();
            if (!task.Result)
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
        public async Task<AppUser> GetUserByUsernameAsync(string username, bool includeAll)
        {
            if (!includeAll)
                return await GetUserByUsernameAsync(username);
            var task = DBContainsUsernameAsync(username);
            task.Wait();
            if (!task.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
                return await _db.AppUser.AsNoTracking().FirstAsync(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their blacklist
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up blacklist for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetBlacklistForUserAsync(string username)
        {
            var userTask= GetUserByUsernameAsync(username, true);
            userTask.Wait();
            return userTask.Result.Blacklist.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants the given user has added to their favorites
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up favorites for</param>
        /// <returns>IEnumerable of restraunt objects</returns>
        public IEnumerable<Restaurant> GetFavoritesForUserAsync(string username)
        {
            var userTask = GetUserByUsernameAsync(username, true);
            userTask.Wait();
            return userTask.Result.Favorite.Select(b => b.Restaurant);
        }

        /// <summary>
        /// Returns a list(IEnumerable) of queries made by the given user
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up queries for</param>
        /// <returns>IEnumerable of query objects</returns>
        public IEnumerable<Query> GetQueriesForUserAsync(string username)
        {
            var userTask = GetUserByUsernameAsync(username, true);
            userTask.Wait();
            return userTask.Result.Query;
        }

        /// <summary>
        /// Returns a list(IEnumerable) of restaurants a user is listed as owning
        /// Throws an exception if username not found
        /// </summary>
        /// <param name="username">Username to look up owned restaurants for</param>
        /// <returns>IEnumerable of restaurant objects</returns>
        public IEnumerable<Restaurant> GetOwnedRestaurantsForUserAsync(string username)
        {
            var userTask = GetUserByUsernameAsync(username, true);
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
        public async Task AddUserAsync(AppUser u)
        {
            var taskResult = DBContainsUsernameAsync(u.Username);
            taskResult.Wait();
            if (taskResult.Result)
                throw new DbUpdateException("That username is already in the database.  Usernames must be unique to add a new user", new NotSupportedException());
            await _db.AddAsync(u);
        }

        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is blaclisting</param>
        /// <param name="r">Restraunt object to be blacklisted</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public async Task AddRestaurantToBlacklistAsync(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(u.Username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Blacklist bl = new Blacklist() { Username = u.Username, RestaurantId = r.Id };
            await _db.AddAsync(bl);
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
        public async Task AddRestaurantToBlacklistAsync(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            //TODO:  change to async once it exists
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = r.Id };
            await _db.AddAsync(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's blacklist
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public async Task AddRestaurantToBlacklistAsync(string username, int restaurantId, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            if (!rRepo.DBContainsRestaurant(restaurantId))
                throw new NotSupportedException($"Restaurant ID '{restaurantId}' not found.");
            Blacklist bl = new Blacklist() { Username = username, RestaurantId = restaurantId };
            await _db.AddAsync(bl);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="u">User object who is Favoriting</param>
        /// <param name="r">Restraunt object to be Favorited</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public async Task AddRestaurantToFavoritesAsync(AppUser u, Restaurant r, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(u.Username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{u.Username}' not found.");
            if (!rRepo.DBContainsRestaurant(r.Id))
                throw new NotSupportedException($"Restaurant ID '{r.Id}' not found.");
            Favorite fav = new Favorite() { Username = u.Username, RestaurantId = r.Id };
            await _db.AddAsync(fav);
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
        public async Task AddRestaurantToFavoritesAsync(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            Restaurant r = rRepo.GetRestaurantByNameAndLocation(restaurantName, restaurantLocation);
            Favorite fav = new Favorite() { Username = username, RestaurantId = r.Id };
            await _db.AddAsync(fav);
        }


        /// <summary>
        /// Adds the specified restaurant to the specified user's Favorites
        /// Throws an exception if specified user or restraunt is not found in DB
        /// Must still call Save() after to persist changes to DB
        /// </summary>
        /// <param name="username">string containing user's username</param>
        /// <param name="restaurantId">int containing restaurant's ID number</param>
        /// <param name="rRepo">RestaurantRepo object, required for validation to ensure the given restraunt exists in our DB</param>
        public async Task AddRestaurantToFavoritesAsync(string username, int restaurantId, RestaurantRepo rRepo)
        {
            var taskResult = DBContainsUsernameAsync(username);
            taskResult.Wait();
            if (!taskResult.Result)
                throw new NotSupportedException($"Username '{username}' not found.");
            if (!rRepo.DBContainsRestaurant(restaurantId))
                throw new NotSupportedException($"Restaurant ID '{restaurantId}' not found.");
            Favorite fav = new Favorite() { Username = username, RestaurantId = restaurantId };
            await _db.AddAsync(fav);
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

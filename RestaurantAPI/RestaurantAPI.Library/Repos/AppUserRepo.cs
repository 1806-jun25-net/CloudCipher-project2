using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Library.Repos
{
    class AppUserRepo
    {
        private readonly Project2DBContext _db;

        public AppUserRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Primary method for retriving all users from the database.  Use parameter to distinguish whether information from junciton tables is required or not.
        /// </summary>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<AppUser> GetUsers(bool includeAll)
        {
            if (includeAll)
                return _db.AppUser.AsNoTracking().Include(m => m.Blacklist).Include(m => m.Favorite).Include(m => m.Query).Include(m => m.Restaurant);
            return _db.AppUser.AsNoTracking();
        }

        /// <summary>
        /// Overload for selecting which specific junciton table information is requred for list of users
        /// </summary>
        /// <param name="includeBlacklist">Whether to include the list of blacklisted restaurants for that user</param>
        /// <param name="includeFavorites">Whether to include the list of favorited restaurants for that user</param>
        /// <param name="includeQueries">Whether to include the list of queries for that user</param>
        /// <param name="includeOwnedRestaurants">Whether to include the list of restaurants registered as owned by that user</param>
        /// <returns>Returns an IQueryable containing all users in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<AppUser> GetUsers(bool includeBlacklist, bool includeFavorites, bool includeQueries, bool includeOwnedRestaurants)
        {
            IQueryable<AppUser> result = _db.AppUser.AsNoTracking();
            if (includeBlacklist)
                result.Include(m => m.Blacklist);
            if (includeFavorites)
                result.Include(m => m.Favorite);
            if (includeQueries)
                result.Include(m => m.Query);
            if (includeOwnedRestaurants)
                result.Include(m => m.Restaurant);
            return result;
        }

        /// <summary>
        /// Given a username, returns the AppUser object with that username from the DB.  By default will include all list data
        /// Throws an exception if username not found in DB
        /// </summary>
        /// <param name="username">Username to look up in database</param>
        /// <returns>AppUser object with specified useranme</returns>
        public AppUser GetUserByUsername(string username)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            return GetUsers(true).First(t => t.Username.Equals(username));
        }

        /// <summary>
        /// Determines whether a user with the given username exists in the DB
        /// </summary>
        /// <param name="username">name of user to search for</param>
        /// <returns>true if user is found in DB, false otherwise</returns>
        public bool DBContainsUsername(string username)
        {
            return GetUsers(false).Any(t => t.Username.Equals(username));
        }

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

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

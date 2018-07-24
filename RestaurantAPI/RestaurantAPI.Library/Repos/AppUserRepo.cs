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

        public IQueryable<AppUser> GetUsers(bool includeAll)
        {
            if (includeAll)
                return _db.AppUser.AsNoTracking().Include(m => m.Blacklist).Include(m => m.Favorite).Include(m => m.Query).Include(m => m.Restaurant);
            return _db.AppUser.AsNoTracking();
        }

        public IQueryable<AppUser> GetUsers(bool includeBlacklist, bool includeFavorites, bool includeQueries, bool includeOwnedRestaurants)
        {
            IQueryable<AppUser> result = _db.AppUser.AsNoTracking();
            if (includeBlacklist)
                result.Include(m => m.Blacklist);
            if (includeBlacklist)
                result.Include(m => m.Favorite);
            if (includeBlacklist)
                result.Include(m => m.Query);
            if (includeBlacklist)
                result.Include(m => m.Restaurant);
            return result;
        }

        public AppUser GetUserByUsername(string username)
        {
            if (!DBContainsUsername(username))
                throw new NotSupportedException($"Username '{username}' not found.");
            return GetUsers(true).First(t => t.Username.Equals(username));
        }

        public bool DBContainsUsername(string username)
        {
            return GetUsers(false).Any(t => t.Username.Equals(username));
        }

        public void AddUser(AppUser u)
        {
            if (DBContainsUsername(u.Username))
                throw new DbUpdateException("That username is already in the database.  Usernames must be unique to add a new user", new NotSupportedException());
            _db.Add(u);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

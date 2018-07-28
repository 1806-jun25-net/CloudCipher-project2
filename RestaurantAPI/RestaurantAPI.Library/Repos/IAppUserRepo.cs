using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IAppUserRepo
    {
        void AddRestaurantToBlacklist(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void AddRestaurantToBlacklist(string username, int restaurantId, RestaurantRepo rRepo);
        void AddRestaurantToBlacklist(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo);
        void AddRestaurantToFavorites(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void AddRestaurantToFavorites(string username, int restaurantId, RestaurantRepo rRepo);
        void AddRestaurantToFavorites(string username, string restaurantName, string restaurantLocation, RestaurantRepo rRepo);
        void AddUser(AppUser u);
        bool DBContainsUsername(string username);
        Task<bool> DBContainsUsernameAsync(string username);
        IEnumerable<Restaurant> GetBlacklistForUser(string username);
        Task<IEnumerable<Restaurant>> GetBlacklistForUserAsync(string username);
        IEnumerable<Restaurant> GetFavoritesForUser(string username);
        Task<IEnumerable<Restaurant>> GetFavoritesForUserAsync(string username);
        IEnumerable<Restaurant> GetOwnedRestaurantsForUser(string username);
        Task<IEnumerable<Restaurant>> GetOwnedRestaurantsForUserAsync(string username);
        IEnumerable<Query> GetQueriesForUser(string username);
        Task<IEnumerable<Query>> GetQueriesForUserAsync(string username);
        AppUser GetUserByUsername(string username);
        AppUser GetUserByUsername(string username, bool includeAll);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByUsernameAsync(string username, bool includeAll);
        IQueryable<AppUser> GetUsers();
        IQueryable<AppUser> GetUsers(bool includeAll);
        void Save();
        Task<int> SaveAsync();
    }
}
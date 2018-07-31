using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IAppUserRepo
    {
        void AddRestaurantToBlacklist(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void AddRestaurantToBlacklist(string username, string restaurantId, RestaurantRepo rRepo);
        Task AddRestaurantToBlacklistAsync(AppUser u, Restaurant r, RestaurantRepo rRepo);
        Task AddRestaurantToBlacklistAsync(string username, string restaurantId, RestaurantRepo rRepo);
        void AddRestaurantToFavorites(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void AddRestaurantToFavorites(string username, string restaurantId, RestaurantRepo rRepo);
        Task AddRestaurantToFavoritesAsync(AppUser u, Restaurant r, RestaurantRepo rRepo);
        Task AddRestaurantToFavoritesAsync(string username, string restaurantId, RestaurantRepo rRepo);
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
        void RemoveRestaurantFromBlacklist(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void RemoveRestaurantFromBlacklist(string username, string restaurantId, RestaurantRepo rRepo);
        Task RemoveRestaurantFromBlacklistAsync(AppUser u, Restaurant r, RestaurantRepo rRepo);
        Task RemoveRestaurantFromBlacklistAsync(string username, string restaurantId, RestaurantRepo rRepo);
        void RemoveRestaurantFromFavorites(AppUser u, Restaurant r, RestaurantRepo rRepo);
        void RemoveRestaurantFromFavorites(string username, string restaurantId, RestaurantRepo rRepo);
        Task RemoveRestaurantFromFavoritesAsync(AppUser u, Restaurant r, RestaurantRepo rRepo);
        Task RemoveRestaurantFromFavoritesAsync(string username, string restaurantId, RestaurantRepo rRepo);
        void Save();
        Task SaveAsync();
        void UpdateUser(AppUser u);
    }
}
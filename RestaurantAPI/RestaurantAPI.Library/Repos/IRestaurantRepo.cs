using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IRestaurantRepo
    {
        void AddNewRestaurants(List<Restaurant> rList, List<string> keywords);
        Task AddNewRestaurantsAsync(List<Restaurant> rList, List<string> keywords);
        void AddRestaurant(Restaurant r);
        Task AddRestaurantAsync(Restaurant r);
        bool DBContainsRestaurant(string Id);
        Task<bool> DBContainsRestaurantAsync(string Id);
        Restaurant GetRestaurantByID(string Id);
        Restaurant GetRestaurantByID(string Id, bool includeAll);
        Task<Restaurant> GetRestaurantByIDAsync(string Id);
        Task<Restaurant> GetRestaurantByIDAsync(string Id, bool includeAll);
        IQueryable<Restaurant> GetRestaurants();
        IQueryable<Restaurant> GetRestaurants(bool includeAll);
        void Save();
        Task<int> SaveAsync();
    }
}
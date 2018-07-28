using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IRestaurantRepo
    {
        void AddRestaurant(Restaurant r);
        bool DBContainsRestaurant(int Id);
        bool DBContainsRestaurant(string name, string location);
        Task<bool> DBContainsRestaurantAsync(int Id);
        Task<bool> DBContainsRestaurantAsync(string name, string location);
        Restaurant GetRestaurantByID(int Id);
        Restaurant GetRestaurantByID(int Id, bool includeAll);
        Task<Restaurant> GetRestaurantByIDAsync(int Id);
        Task<Restaurant> GetRestaurantByIDAsync(int Id, bool includeAll);
        Restaurant GetRestaurantByNameAndLocation(string name, string location);
        Restaurant GetRestaurantByNameAndLocation(string name, string location, bool includeAll);
        Task<Restaurant> GetRestaurantByNameAndLocationAsync(string name, string location);
        Task<Restaurant> GetRestaurantByNameAndLocationAsync(string name, string location, bool includeAll);
        int GetRestaurantIDByNameAndLocation(string name, string location);
        Task<int> GetRestaurantIDByNameAndLocationAsync(string name, string location);
        IQueryable<Restaurant> GetRestaurants();
        IQueryable<Restaurant> GetRestaurants(bool includeAll);
        void Save();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IQueryRepo
    {
        void AddQuery(Query u, KeywordRepo kRepo);
        Task AddQueryAsync(Query u, KeywordRepo kRepo);
        void AddQueryRestaurantJunction(int queryId, List<Restaurant> restaurants, RestaurantRepo rRepo);
        bool DBContainsQuery(int Id);
        Task<bool> DBContainsQueryAsync(int Id);
        IQueryable<Query> GetQueries();
        Query GetQueryByID(int Id);
        Task<Query> GetQueryByIDAsync(int Id);
        List<Restaurant> GetRestaurantsInQuery(int Id);
        void Save();
        Task<int> SaveAsync();
    }
}
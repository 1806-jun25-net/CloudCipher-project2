using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IQueryRepo
    {
        void AddQuery(Query u);
        void AddQueryKeywordJunction(int queryId, List<string> keywords, KeywordRepo kRepo);
        Task AddQueryKeywordJunctionAsync(int queryId, List<string> keywords, KeywordRepo kRepo);
        void AddQueryRestaurantJunction(int queryId, List<Restaurant> restaurants, RestaurantRepo rRepo);
        Task AddQueryRestaurantJunctionAsync(int queryId, List<Restaurant> restaurants, RestaurantRepo rRepo);
        bool DBContainsQuery(int Id);
        Task<bool> DBContainsQueryAsync(int Id);
        IQueryable<Query> GetQueries();
        Query GetQueryByID(int Id);
        Task<Query> GetQueryByIDAsync(int Id);
        List<Restaurant> GetRestaurantsInQuery(int Id);
        Task<List<Restaurant>> GetRestaurantsInQueryAsync(int Id);
        void Save();
        Task<int> SaveAsync();
    }
}
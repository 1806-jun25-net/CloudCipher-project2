using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IQueryRepo
    {
        void AddQuery(Query u, KeywordRepo kRepo);
        Task AddQueryAsync(Query u, KeywordRepo kRepo);
        bool DBContainsQuery(int Id);
        Task<bool> DBContainsQueryAsync(int Id);
        IQueryable<Query> GetQueries();
        Query GetQueryByID(int Id);
        Task<Query> GetQueryByIDAsync(int Id);
        void Save();
        Task<int> SaveAsync();
    }
}
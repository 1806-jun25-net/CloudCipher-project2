using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Data;

namespace RestaurantAPI.Library.Repos
{
    public interface IKeywordRepo
    {
        void AddKeyword(Keyword kw);
        void AddKeyword(string kw);
        Task AddKeywordAsync(Keyword kw);
        Task AddKeywordAsync(string kw);
        bool DBContainsKeyword(Keyword kw);
        bool DBContainsKeyword(string kw);
        Task<bool> DBContainsKeywordAsync(Keyword kw);
        Task<bool> DBContainsKeywordAsync(string kw);
        IQueryable<Keyword> GetKeywords();
        void Save();
        Task SaveAsync();
    }
}
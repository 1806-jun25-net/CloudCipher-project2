using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Library.Repos
{
    public class QueryRepo : IQueryRepo
    {
        private readonly Project2DBContext _db;

        public QueryRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //parameterless contstructor to enable moq-ing
        public QueryRepo() { }

        /// <summary>
        /// Default method for retriving all Queries from the DB.
        /// </summary>
        /// <returns>Returns an IQueryable containing all Query objects in the database.  Use ToList() on the result to make it a usable list. </returns>
        public IQueryable<Query> GetQueries()
        {
            return _db.Query.AsNoTracking().Include(m =>m.QueryKeywordJunction).Include(m=>m.QueryRestaurantJunction).ThenInclude(n=>n.Restaurant);
        }

        /// <summary>
        /// Checks whether DB conatins a query with the given ID
        /// </summary>
        /// <param name="Id">ID to search DB for</param>
        /// <returns>true if Query is found matching given ID, false otherwise</returns>
        public bool DBContainsQuery(int Id)
        {
            return GetQueries().Any(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Query ID, returns the Query object with that ID from the DB.
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Query object with specified ID</returns>
        public Query GetQueryByID(int Id)
        {
            if (!DBContainsQuery(Id))
                throw new NotSupportedException($"Query ID '{Id}' not found.");
            return GetQueries().First(t => t.Id == Id);
        }

        /// <summary>
        /// Adds given Query object to the DB.  
        /// Also ensures keywords exist Keyword table and add them to QueryKeywordJunction.
        /// Throws an excpetion if the Id is already set to some value, as SQL is set to generate a new ID
        /// Must call Save() afterwards to persist changes to DB.
        /// </summary>
        /// <param name="u">Query to add to DB</param>
        /// /// <param name="kRepo">KeywordRepo so keyword can be added to DB if needed</param>
        public void AddQuery(Query u, KeywordRepo kRepo)
        {
            if (DBContainsQuery(u.Id) || u.Id > 0)
                throw new DbUpdateException("Invalid ID. ID should not be set prior to adding a new query to the database.  Identity constraint does that for you.", new NotSupportedException());
            _db.Add(u);
            foreach (var kwj in u.QueryKeywordJunction)
            {
                if (!kRepo.DBContainsKeyword(kwj.Word))
                    kRepo.AddKeyword(new Keyword() { Word = kwj.Word });
                _db.Add(kwj);
            }  
        }

        public void AddQueryRestaurantJunction(int queryId, List<Restaurant> restaurants, RestaurantRepo rRepo)
        {
            if (!DBContainsQuery(queryId))
                throw new DbUpdateException($"Query Id {queryId} not recognized.", new NotSupportedException());
            foreach (Restaurant r in restaurants)
            {
                if (!rRepo.DBContainsRestaurant(r.Id))
                    throw new DbUpdateException($"Restaurant Id {r.Id} not recognized.", new NotSupportedException());
                _db.QueryRestaurantJunction.Add(new QueryRestaurantJunction() { QueryId = queryId, RestaurantId = r.Id});
            }
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }


        //Async versions of applicable methods:
        
        /// <summary>
        /// Checks whether DB conatins a Query with the given ID
        /// </summary>
        /// <param name="Id">ID to search DB for</param>
        /// <returns>true if Query is found matching given ID, false otherwise</returns>
        public async Task<bool> DBContainsQueryAsync(int Id)
        {
            return await GetQueries().AnyAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Query ID, returns the Query object with that ID from the DB.
        /// Will include QueryKeywordJunction data.
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public async Task<Query> GetQueryByIDAsync(int Id)
        {
            if (!DBContainsQuery(Id))
                throw new NotSupportedException($"Query ID '{Id}' not found.");
            return await GetQueries().FirstAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Adds given Query object to the DB.  
        /// Also ensures keywords exist in both QueryKeywordJunction and Keyword tables.
        /// Throws an excpetion if the Id is already set to some value, as SQL is set to generate a new ID
        /// Must call Save() afterwards to persist changes to DB.
        /// </summary>
        /// <param name="u">Query to add to DB</param>
        /// /// <param name="kRepo">KeywordRepo so keyword can be added to DB if needed</param>
        public async Task AddQueryAsync(Query u, KeywordRepo kRepo)
        {
            bool contains = await DBContainsQueryAsync(u.Id);
            if ( contains || u.Id > 0)
                throw new DbUpdateException("Invalid ID. ID should not be set prior to adding a new query to the database.  Identity constraint does that for you.", new NotSupportedException());
            _db.Add(u);
            foreach (var kwj in u.QueryKeywordJunction)
            {
                contains = await kRepo.DBContainsKeywordAsync(kwj.Word);
                if (!contains)
                    kRepo.AddKeyword(new Keyword() { Word = kwj.Word });
                _db.Add(kwj);
            }
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}

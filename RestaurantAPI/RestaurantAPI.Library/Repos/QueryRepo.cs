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
            return _db.Query.AsNoTracking()
                .Include(m =>m.QueryKeywordJunction)
                .Include(m=>m.QueryRestaurantJunction).ThenInclude(n=>n.Restaurant);
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
        /// Given a Query Id, returns the list of restaurants that were returned for that query as found in the QueryRestaurantJunction table
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">query Id to lookup</param>
        /// <returns>list of Restaurant objects</returns>
        public List<Restaurant> GetRestaurantsInQuery(int Id)
        {
            Query q = GetQueryByID(Id);
            return q.QueryRestaurantJunction.Select(n => n.Restaurant).ToList();
        }

        /// <summary>
        /// Adds given Query object to the DB.  
        /// Must call Save() afterwards to persist changes to DB and learn the Id it has been assigned by the DB.
        /// </summary>
        /// <param name="u">Query to add to DB</param>
        public void AddQuery(Query u)
        {
            _db.Add(u);
        }

        /// <summary>
        /// Given a queryId and list of keywords (strings), adds entries to the QueryKeywordJunction table.
        /// Throws an exception if QueryId not found for whatever reason.
        /// </summary>
        /// <param name="queryId">int Id of the query</param>
        /// <param name="keywords">list of string of keywords</param>
        /// <param name="kRepo">KeywordRepo</param>
        public void AddQueryKeywordJunction(int queryId, List<string> keywords, KeywordRepo kRepo)
        {
            if (!DBContainsQuery(queryId))
                throw new DbUpdateException($"Query Id {queryId} not recognized.", new NotSupportedException());
            foreach (var kw in keywords)
            {
                if (!kRepo.DBContainsKeyword(kw))
                    kRepo.AddKeyword(new Keyword() { Word = kw });
                _db.Add(new QueryKeywordJunction() { QueryId = queryId, Word = kw });
            }  
        }

        /// <summary>
        /// Given a queryId and list of restaurants, adds entries to the QueryRestaurantJunction table.
        /// Throws an exception if QueryId not found for whatever reason.
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="restaurants"></param>
        /// <param name="rRepo"></param>
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
            var contains = await DBContainsQueryAsync(Id);
            if (!contains)
                throw new NotSupportedException($"Query ID '{Id}' not found.");
            return await GetQueries().FirstAsync(t => t.Id == Id);
        }

        /// <summary>
        /// Given a Query Id, returns the list of restaurants that were returned for that query as found in the QueryRestaurantJunction table
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">query Id to lookup</param>
        /// <returns>list of Restaurant objects</returns>
        public async Task<List<Restaurant>> GetRestaurantsInQueryAsync(int Id)
        {
            Query q = await GetQueryByIDAsync(Id);
            return q.QueryRestaurantJunction.Select(n => n.Restaurant).ToList();
        }

        /// <summary>
        /// Given a queryId and list of keywords (strings), adds entries to the QueryKeywordJunction table.
        /// Throws an exception if QueryId not found for whatever reason.
        /// </summary>
        /// <param name="queryId">int Id of the query</param>
        /// <param name="keywords">list of string of keywords</param>
        /// <param name="kRepo">KeywordRepo</param>
        public async Task AddQueryKeywordJunctionAsync(int queryId, List<string> keywords, KeywordRepo kRepo)
        {
            var contains = await DBContainsQueryAsync(queryId);
            if (!contains)
                throw new DbUpdateException($"Query Id {queryId} not recognized.", new NotSupportedException());
            foreach (var kw in keywords)
            {
                contains = await kRepo.DBContainsKeywordAsync(kw);
                if (!contains)
                    kRepo.AddKeyword(new Keyword() { Word = kw });
                _db.Add(new QueryKeywordJunction() { QueryId = queryId, Word = kw });
            }
        }

        public async Task AddQueryRestaurantJunctionAsync(int queryId, List<Restaurant> restaurants, RestaurantRepo rRepo)
        {
            var contains = await DBContainsQueryAsync(queryId);
            if (!contains)
                throw new DbUpdateException($"Query Id {queryId} not recognized.", new NotSupportedException());
            foreach (Restaurant r in restaurants)
            {
                contains = await rRepo.DBContainsRestaurantAsync(r.Id);
                if (!contains)
                    throw new DbUpdateException($"Restaurant Id {r.Id} not recognized.", new NotSupportedException());
                _db.QueryRestaurantJunction.Add(new QueryRestaurantJunction() { QueryId = queryId, RestaurantId = r.Id });
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

using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Library.Repos
{
    class QueryRepo
    {
        private readonly Project2DBContext _db;

        public QueryRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        /// <summary>
        /// Default method for retriving all Queries from the DB.
        /// </summary>
        /// <returns>Returns an IQueryable containing all Query objects in the database.  Use ToList() on the result to make it a usable list. </returns>
        public IQueryable<Query> GetQueries()
        {
            return _db.Query.AsNoTracking().Include(m =>m.QueryKeywordJunction);
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
                throw new NotSupportedException($"Query ID '{Id}' not found."); ;
            return GetQueries().First(t => t.Id == Id);
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
        /// Adds given Query object to the DB.
        /// Throws an excpetion if the Id is already set to some value, as SQL is set to generate a new ID
        /// Must call Save() afterwards to persist changes to DB.
        /// </summary>
        /// <param name="u">Query to add to DB</param>
        public void AddQuery(Query u)
        {
            if (DBContainsQuery(u.Id) || u.Id > 0)
                throw new DbUpdateException("Invalid ID. ID should not be set prior to adding a new query to the database.  Identity constraint does that for you.", new NotSupportedException());
            _db.Add(u);
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Library.Repos
{
    public class KeywordRepo
    {
        private readonly Project2DBContext _db;

        public KeywordRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Default method for retriving all queried Keywords from the DB.
        /// </summary>
        /// <returns>Returns an IQueryable containing all Keyword objects in the database.  Use ToList() on the result to make it a usable list.  Use keyword.Word to access the actual string.</returns>
        public IQueryable<Keyword> GetKeywords()
        {
            return _db.Keyword.AsNoTracking();
        }

        /// <summary>
        /// Checks whether the given Keyword already exists in the DB or not.  Overload which takes a Keyword object.
        /// </summary>
        /// <param name="kw">Keyword object who's .Word is to be checked for existence in the DB</param>
        /// <returns>true if Keyword found in DB, falase otherwise</returns>
        public bool DBContainsKeyword(Keyword kw)
        {
            return GetKeywords().Any(t => t.Word.Equals(kw.Word));
        }

        /// <summary>
        /// Checks whether the given Keyword already exists in the DB or not.  Overload which takes a string.
        /// </summary>
        /// <param name="kw">string to be checked for existence in the DB as a Keyword</param>
        /// <returns>true if keyword string found in DB, falase otherwise</returns>
        public bool DBContainsKeyword(string kw)
        {
            return GetKeywords().Any(t => t.Word.Equals(kw));
        }

        /// <summary>
        /// Adds the given Keyword object to the DB.  Overload which takes the Keyword object.
        /// Throws an exception if the Keyword already exists in the DB to avoid violating PK constraint.
        /// Still need to call Save() afterwards to keep changes.
        /// </summary>
        /// <param name="u">Keyword object to add to DB</param>
        public void AddKeyword(Keyword u)
        {
            if (DBContainsKeyword(u))
                throw new DbUpdateException("That keyword is already in the database.  Keywords must be unique.", new NotSupportedException());
            _db.Add(u);
        }

        /// <summary>
        /// Adds the given keyword string to the DB.  Overload which takes a string of keyword to be added.
        /// Throws an exception if the given string has a Keyword already in the DB to avoid violating PK constraint.
        /// Still need to call Save() afterwards to keep changes.
        /// </summary>
        /// <param name="u">keyword string to add to DB</param>
        public void AddKeyword(string u)
        {
            if (DBContainsKeyword(u))
                throw new DbUpdateException("That keyword is already in the database.  Keywords must be unique.", new NotSupportedException());
            _db.Add(new Keyword() { Word = u } );
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

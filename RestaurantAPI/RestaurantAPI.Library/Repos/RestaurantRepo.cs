﻿using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantAPI.Library.Repos
{
    public class RestaurantRepo
    {
        private readonly Project2DBContext _db;

        public RestaurantRepo(Project2DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Primary method for retriving all restaurants from the database. By default includes no info from junciton tables.
        /// </summary>
        /// <returns>Returns an IQueryable containing all restaurants in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<Restaurant> GetRestaurants()
        {
            return _db.Restaurant.AsNoTracking();
        }

        /// <summary>
        /// Overload for retrieving all restaurants from the database.  Use parameter to distinguish whether information from junciton tables is required or not.
        /// </summary>
        /// <param name="includeAll">Whether to include the information from junction tables or not</param>
        /// <returns>Returns an IQueryable containing all restaurants in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<Restaurant> GetRestaurants(bool includeAll)
        {
            if (includeAll)
                return _db.Restaurant.AsNoTracking().Include(m => m.Blacklist).Include(m => m.Favorite).Include(m => m.RestaurantKeywordJunction);
            return _db.Restaurant.AsNoTracking();
        }

        /// <summary>
        /// Overload for retrieving all restaurants from the database.  Use parameters to distinguish whether information from junciton tables is required or not.
        /// </summary>
        /// <param name="includeBlacklist">Whether to include the list of users blacklisting this restaurant</param>
        /// <param name="includeFavorites">Whether to include the list of users favoriting this restaurant</param>
        /// <param name="includeKeywords">Whether to include the list of keywords pertaining to this restaurant</param>
        /// <returns>Returns an IQueryable containing all restaurants in the database.  Use ToList() on the result to make it a usable list.</returns>
        public IQueryable<Restaurant> GetRestaurants(bool includeBlacklist, bool includeFavorites, bool includeQueries, bool includeKeywords)
        {
            IQueryable<Restaurant> result = _db.Restaurant.AsNoTracking();
            if (includeBlacklist)
                result.Include(m => m.Blacklist);
            if (includeFavorites)
                result.Include(m => m.Favorite);
            if (includeKeywords)
                result.Include(m => m.RestaurantKeywordJunction);
            return result;
        }

        /// <summary>
        /// Given a Restaurant ID, returns the Restaurant object with that ID from the DB.
        /// Throws an exception if ID not found in DB
        /// </summary>
        /// <param name="Id">ID to look up in database</param>
        /// <returns>Restaurant object with specified ID</returns>
        public Restaurant GetRestaurantByID(int Id)
        {
            if (!DBContainsRestaurant(Id))
                throw new NotSupportedException($"Restaurant ID '{Id}' not found.");
            return GetRestaurants(true).First(t => t.Id == Id);
        }

        /// <summary>
        /// Checks whether DB conatins a restaurant with the given ID
        /// </summary>
        /// <param name="Id">ID to search DB for</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public bool DBContainsRestaurant(int Id)
        {
            return GetRestaurants(false).Any(t => t.Id == Id);
        }


        /// <summary>
        /// Checks whether DB conatins a restaurant as identified by the given name and location
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>true if Restaurant is found matching given ID, false otherwise</returns>
        public bool DBContainsRestaurant(string name, string location)
        {
            return GetRestaurants(false).Any(t => t.Name.Equals(name) && t.Location.Equals(location));
        }
        //TODO:  Add argument null exceptions to Add or search

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns its ID number.
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>ID number of specified Restaurant</returns>
        public int GetRestaurantIDByNameAndLocation(string name, string location)
        {
            if (!DBContainsRestaurant(name, location))
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return GetRestaurants(false).First(t => t.Name.Equals(name) && t.Location.Equals(location)).Id;
        }

        /// <summary>
        /// Given a Restaurant's name and location, looks it up in the DB and returns the Restaurant object.
        /// Throws an exception if name and location do not match any restaurants in DB.
        /// </summary>
        /// <param name="name">Restaurant name</param>
        /// <param name="location">Restaurant location</param>
        /// <returns>the Restaurant object specified</returns>
        public Restaurant GetRestaurantByNameAndLocation(string name, string location)
        {
            if (!DBContainsRestaurant(name, location))
                throw new NotSupportedException($"Restaurant name '{name}' at location '{location}' not found.");
            return GetRestaurants(true).First(t => t.Name.Equals(name) && t.Location.Equals(location));
        }

        /// <summary>
        /// Adds the given Restaurant object to the database
        /// Throws an exception if the Id is already set to some value, as SQL is set to generate a new ID.
        /// Throws an exception if a restraunt already exists in the DB with that given name and location.
        /// </summary>
        /// <param name="r"></param>
        public void AddRestaurant(Restaurant r)
        {
            if (DBContainsRestaurant(r.Id) || r.Id > 0)
                throw new DbUpdateException("Invalid ID. ID should not be set prior to adding a new Restaurant to the database.  Identity constraint does that for you.", new NotSupportedException());
            if (DBContainsRestaurant(r.Name, r.Location))
                throw new DbUpdateException("Restraunt with that Name and location already exists in the DB.  Cannot add another.", new NotSupportedException());

            _db.Add(r);
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.API.Models;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryRestaurantAnalyticsController : ControllerBase
    {

        // GET: api/QueryRestaurantAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many times given restaurant appeared in a query result, sorted by descending.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of RestaurantModel</returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<RestaurantModel>>> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/QueryRestaurantAnalytics/5
        /// <summary>
        /// Returns a list of Restaurants wrapped w/ frequencies.
        /// Returns an error code if user not found in DB.
        /// Frequencies represent how many times given restaurant appeared in a query result for a specific user, sorted by descending.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>List of FrequencyWrapper of RestaurantModel<</returns>
        [HttpGet("{id}", Name = "GetQueryRestaurantAnalytics")]
        public ActionResult<List<FrequencyWrapper<RestaurantModel>>> Get(string username)
        {
            throw new NotImplementedException();
        }

    }
}

using System;
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
    public class FavoritesAnalyticsController : ControllerBase
    {
        // GET: api/FavoritesAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many users have given restaurant in their favorites.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of RestaurantModel </returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<RestaurantModel>>> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/FavoritesAnalytics/5
        /// <summary>
        /// Return a list of usernames of users who have favorited a specific restaurant.
        /// Available to all users (public like facebook likes?) (or admin only?)
        /// </summary>
        /// <param name="id">restaurant Id</param>
        /// <returns>List of strings representing usernames</returns>
        [HttpGet("{id}", Name = "GetFavoritesAnalytics")]
        public ActionResult<List<string>> Get(string id)
        {
            throw new NotImplementedException();
        }

    }
}

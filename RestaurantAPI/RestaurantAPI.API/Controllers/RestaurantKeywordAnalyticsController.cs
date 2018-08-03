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
    public class RestaurantKeywordAnalyticsController : ControllerBase
    {

        // GET: api/RestaurantKeywordAnalytic
        /// <summary>
        /// Return a list of all Keywords wrapped w/ frequencies.
        /// Frequencies represent how many restaurants in DB match that keyword.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of string </returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<string>>> Get()
        {
            //throw new NotImplementedException();
            return new List<FrequencyWrapper<string>>() { new FrequencyWrapper<string> { Frequency = 1, Obj = "it works!" } };
        }

        // GET: api/RestaurantKeywordAnalytics/5
        /// <summary>
        /// Given a restaurantId, return all keywords associated with that restaurant.
        /// Returns an error code if restaurant not found in DB.
        /// Accessible to all users.
        /// </summary>
        /// <param name="restaurantId">Keyword to get restaurant matches for</param>
        /// <returns>List of restaurants </returns>
        [HttpGet("{restaurantId}", Name = "GetKeywordsForRestaurant")]
        public ActionResult<List<RestaurantModel>> Get(string restaurantId)
        {
            throw new NotImplementedException();
        }
        
    }
}

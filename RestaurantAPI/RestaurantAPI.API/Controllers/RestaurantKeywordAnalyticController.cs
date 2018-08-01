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
    public class RestaurantKeywordAnalyticController : ControllerBase
    {

        // GET: api/RestaurantKeywordAnalytic
        /// <summary>
        /// Return a list of all Keywords wrapped w/ frequencies.
        /// Frequencies represent how many restaurants matched that keyword in a search.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of string </returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<string>>> Get()
        {
            //throw new NotImplementedException();
            return new List<FrequencyWrapper<string>>() { new FrequencyWrapper<string> { Frequency = 1, Obj = "it works!" } };
        }

        // GET: api/RestaurantKeywordAnalytic/5
        /// <summary>
        /// Return a list of Restaurants that have ever matched the given keyword.
        /// Returns an error code if keyword not found in DB.
        /// Accessible to all users.
        /// </summary>
        /// <param name="keyword">Keyword to get restaurant matches for</param>
        /// <returns>List of restaurants </returns>
        [HttpGet("{id}", Name = "GetRestaurantKeywordAnalytic")]
        public ActionResult<List<RestaurantModel>> Get(string keyword)
        {
            throw new NotImplementedException();
        }
        
    }
}

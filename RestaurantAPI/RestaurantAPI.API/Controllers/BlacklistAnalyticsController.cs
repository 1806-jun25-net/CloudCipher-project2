using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistAnalyticsController : ControllerBase
    {
        // GET: api/BlacklistAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many users have given restaurant in their blacklist.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of RestaurantModel </returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/BlacklistAnalytics/5
        /// <summary>
        /// Return a list of usernames of users who have blacklisted a specific restaurant.
        /// Available to all users (public like facebook likes?) (or admin only?)
        /// </summary>
        /// <param name="id">restaurant Id</param>
        /// <returns>List of strings representing usernames</returns>
        [HttpGet("{id}", Name = "GetBlacklistAnalytics")]
        public ActionResult<List<string>> Get(int id)
        {
            throw new NotImplementedException();
        }

    }
}

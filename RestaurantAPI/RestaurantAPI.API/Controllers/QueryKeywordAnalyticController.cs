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
    public class QueryKeywordAnalyticController : ControllerBase
    {

        // GET: api/KeywordQueryAnalytic
        /// <summary>
        /// Return a list of all Keywords wrapped w/ frequencies.
        /// Frequencies represent how many queries that keyword has appeared in, sorted by descending.
        /// Available to All users
        /// </summary>
        /// <returns>List of FrequencyWrapper of string </returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<string>>> Get()
        {
            //Test code to see if it is possible to send the generic type FrequencyWrapperModel<string>
            // Spoilers - it is! :D

            //throw new NotImplementedException();
            return new List<FrequencyWrapper<string>>() { new FrequencyWrapper<string>() { Frequency = 1, Obj = "it works!" } };
        }

        //
        //takes
        // GET: api/KeywordQueryAnalytic/5
        /// <summary>
        /// Return a list of all keywords wrapped w/ frequencies.
        /// Filters to only include queries placed by the given username.
        /// Frequencies represent how many queries that keyword has appeared in, sorted by descending.
        /// Returns an error code if user not found in DB.
        /// Only available for user matching username, or admin.
        /// </summary>
        /// <param name="username">name of user to look up keyword frequency for</param>
        /// <returns>List of FrequencyWrapperModel<string></returns>
        [HttpGet("{id}", Name = "GetKeywordQueryAnalytic")]
        public string Get(string username)
        {
            throw new NotImplementedException();
        }
        
    }
}

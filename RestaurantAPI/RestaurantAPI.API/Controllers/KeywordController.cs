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
    public class KeywordController : ControllerBase
    {
        // GET: api/Keyword
        /// <summary>
        /// Gets a list of all keywords in the database
        /// </summary>
        /// <returns>IEnumerable of strings</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/Keyword/5
        /// <summary>
        /// Given a keyword, returns all restaurants associated to that keyword
        /// </summary>
        /// <param name="keyword">The keyword to match restaurants for</param>
        /// <returns>IEmumerable of RestaurantModels</returns>
        [HttpGet("{keyword}", Name = "GetRestaurantsForKeyword")]
        public ActionResult<IEnumerable<RestaurantModel>> Get(int keyword)
        {
            throw new NotImplementedException();
            //return new List<RestaurantModel>();
        }

        
    }
}

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
    public class FavoritesController : ControllerBase
    {
        //Return list of all favorited Restaurants for a given user
        // GET: api/Favorites
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Unused
        // GET: api/Favorites/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        //Given a restaurant id as a parameter, add the restaurant to the current user's favorites
        // POST: api/Favorites
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        //Unused
        // PUT: api/Favorites/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //Given a restaurant id as a parameter, remove the restaurant from the current user's favorites
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

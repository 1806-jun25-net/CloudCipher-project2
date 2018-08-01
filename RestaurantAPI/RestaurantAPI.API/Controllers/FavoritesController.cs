using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        public FavoritesController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
        {
            Arepo = AppRepo;
            Krepo = KeyRepo;
            Qrepo = QRepo;
            Rrepo = RestRepo;
        }

        public IAppUserRepo Arepo { get; set; }
        public IKeywordRepo Krepo { get; set; }
        public IQueryRepo Qrepo { get; set; }
        public IRestaurantRepo Rrepo { get; set; }


        //Return list of all favorited Restaurants for a given user
        // GET: api/Favorites
        [HttpGet]
        public IEnumerable<string> Get()
        {
            <List><Restaurant>

            return new string[] { "value1", "value2" };
        }

        //Unused
        // GET: api/Favorites/5
        [HttpGet("{id}", Name = "GetFavorites")]
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

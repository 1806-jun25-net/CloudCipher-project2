using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.API.Models;
using RestaurantAPI.Library;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        public BlacklistController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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
        // GET: api/Blacklist
        [HttpGet]
        public ActionResult<List<RestaurantModel>> Get()
        {
            Arepo.GetFavoritesForUser(User.Identity.Name);

            return Mapper.Map(Arepo.GetFavoritesForUser(User.Identity.Name)).ToList();

        }

        //Given a restaurantID, returns bool of whether restaurant is in user's blacklist
        //TODO: this
        // GET: api/Blacklist/5
        [HttpGet("{id}", Name = "GetBlacklist")]
        public string Get(int id)
        {
            return "value";
        }

        //Given a restaurant id as a parameter, add the restaurant to the current user's favorites
        // POST: api/Blacklist
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] string value)
        {

            try
            {
                Arepo.AddRestaurantToBlacklist(User.Identity.Name, value, (RestaurantRepo)Rrepo);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Rrepo.Save();

            return CreatedAtRoute("GetBlacklist", new { Id = value }, value);
        }
        

        //Given a restaurant id as a parameter, remove the restaurant from the current user's favorites
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete([FromBody]string value)
        {

            try
            {
                Arepo.RemoveRestaurantFromBlacklist(User.Identity.Name, value, (RestaurantRepo)Rrepo);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Rrepo.Save();

            return CreatedAtRoute("RemoveBlacklist", new { Id = value }, value);
        }
    }
}

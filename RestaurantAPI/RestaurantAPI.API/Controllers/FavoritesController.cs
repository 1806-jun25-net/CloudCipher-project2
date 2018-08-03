using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Library;
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
        public ActionResult<List<RestaurantModel>> Get()
        {
            Arepo.GetFavoritesForUser(User.Identity.Name);

            return Mapper.Map(Arepo.GetFavoritesForUser(User.Identity.Name)).ToList();

        }

        //Given a restaurantID, returns bool of whether restaurant is in user's favorites
        //TODO: this
        // GET: api/Favorites/5
        [HttpGet("{id}", Name = "GetFavorites")]
        public async Task<ActionResult<bool>> GetAsync(string rId)
        {
            if (!(await Arepo.DBContainsUsernameAsync(User.Identity.Name)))
                return StatusCode(StatusCodes.Status401Unauthorized);
            //Since this method is authorized by Identity, it will automatically handle returning 401 if user isn't logged in.
            return (await Arepo.GetFavoritesForUserAsync(User.Identity.Name)).Any(n => n.Id.Equals(rId));
        }

        //Given a restaurant id as a parameter, add the restaurant to the current user's favorites
        // POST: api/Favorites
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] string value)
        {
         
            try
            {
                Arepo.AddRestaurantToFavorites(User.Identity.Name, value, (RestaurantRepo)Rrepo);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Rrepo.Save();

            return CreatedAtRoute("Getfavorite", new { Id = value }, value);
        }

        
        //Given a restaurant id as a parameter, remove the restaurant from the current user's favorites
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete([FromBody]string value)
        {

            try
            {
                Arepo.RemoveRestaurantFromFavorites(User.Identity.Name, value, (RestaurantRepo)Rrepo);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Rrepo.Save();

            return CreatedAtRoute("Removefavorite", new { Id = value }, value);
        }
    }
}

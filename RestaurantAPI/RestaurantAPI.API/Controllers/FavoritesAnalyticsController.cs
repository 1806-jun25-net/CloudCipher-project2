using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.API.Models;
using RestaurantAPI.Library;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesAnalyticsController : ControllerBase
    {
        public FavoritesAnalyticsController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        // GET: api/FavoritesAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many users have given restaurant in their favorites.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of RestaurantModel </returns>
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<RestaurantModel>>> Get()
        {
            try
            {
                return Rrepo.GetRestaurants(true).Select(r => new FrequencyWrapper<RestaurantModel>()
                {
                    Obj = Mapper.Map(r),
                    Frequency = r.Favorite.Count()
                }).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/FavoritesAnalytics/5
        /// <summary>
        /// Return a list of usernames of users who have favorited a specific restaurant.
        /// Available to all users (public like facebook likes?) (or admin only?)
        /// </summary>
        /// <param name="id">restaurant Id</param>
        /// <returns>List of strings representing usernames</returns>
        [ProducesResponseType(500)]
        [HttpGet("{rId}", Name = "GetFavoritesAnalytics")]
        public ActionResult<List<string>> Get(string rId)
        {
            try
            {
                return Arepo.GetUsers(true).Where(a => a.Favorite.Any(f => f.RestaurantId.Equals(rId))).Select(a => a.Username).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}

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
    public class QueryRestaurantAnalyticsController : ControllerBase
    {
        public QueryRestaurantAnalyticsController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        // GET: api/QueryRestaurantAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many times given restaurant appeared in a query result, sorted by descending.
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of RestaurantModel</returns>
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<RestaurantModel>>> Get()
        {
            return Rrepo.GetRestaurants(true).Select(r => new FrequencyWrapper<RestaurantModel>()
            {
                Obj = Mapper.Map(r),
                Frequency = r.QueryRestaurantJunction.Count()
            }).ToList();
        }

        // GET: api/QueryRestaurantAnalytics/5
        /// <summary>
        /// Returns a list of Restaurants wrapped w/ frequencies.
        /// Returns an error code if user not found in DB.
        /// Frequencies represent how many times given restaurant appeared in a query result for a specific user, sorted by descending.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>List of FrequencyWrapper of RestaurantModel<</returns>
        [ProducesResponseType(400)]
        [HttpGet("{username}", Name = "GetQueryRestaurantAnalytics")]
        public async Task<ActionResult<List<FrequencyWrapper<RestaurantModel>>>> GetAsync(string username)
        {
            if (!(await Arepo.DBContainsUsernameAsync(username)))
                return StatusCode(StatusCodes.Status400BadRequest);

            return Rrepo.GetRestaurants(true).Select(r => new FrequencyWrapper<RestaurantModel>()
            {
                Obj = Mapper.Map(r),
                Frequency = r.QueryRestaurantJunction.Where(q => q.Query.Username.Equals(username)).Count()
            }).ToList();
        }

    }
}

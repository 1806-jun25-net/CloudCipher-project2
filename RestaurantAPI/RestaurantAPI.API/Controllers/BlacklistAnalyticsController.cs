using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using RestaurantAPI.API.Models;
using RestaurantAPI.Library;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistAnalyticsController : ControllerBase
    {
        public BlacklistAnalyticsController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/BlacklistAnalytics
        /// <summary>
        /// Return a list of all Restaurants wrapped w/ frequencies.
        /// Frequencies represent how many users have given restaurant in their blacklist, sorted by descending.
        /// Available to all users.
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
                    Frequency = r.Blacklist.Count()
                }).OrderByDescending(k => k.Frequency).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/BlacklistAnalytics/5
        /// <summary>
        /// Return a list of usernames of users who have blacklisted a specific restaurant.
        /// Available to all users.
        /// Should be sorted alphabetically by default...
        /// </summary>
        /// <param name="id">restaurant Id</param>
        /// <returns>List of strings representing usernames</returns>
        [ProducesResponseType(500)]
        [HttpGet("{rId}", Name = "GetBlacklistAnalytics")]
        public ActionResult<List<string>> Get(string rId)
        {
            try
            {
                return Arepo.GetUsers(true).Where(a => a.Blacklist.Any(f => f.RestaurantId.Equals(rId))).Select(a => a.Username).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}

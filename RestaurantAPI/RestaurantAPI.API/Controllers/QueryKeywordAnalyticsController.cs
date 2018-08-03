using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using RestaurantAPI.API.Models;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryKeywordAnalyticsController : ControllerBase
    {
        public QueryKeywordAnalyticsController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        // GET: api/QueryKeywordAnalytics
        /// <summary>
        /// Return a list of all Keywords wrapped w/ frequencies.
        /// Frequencies represent how many queries that keyword has appeared in, sorted by descending.
        /// Available to All users
        /// </summary>
        /// <returns>List of FrequencyWrapper of string </returns>
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<string>>> Get()
        {
            try
            {
                return Krepo.GetKeywords().Select(k => new FrequencyWrapper<string>()
                {
                    Obj = k.Word,
                    Frequency = Krepo.GetQueryKeywordJunction().Count(qkj => qkj.Word.Equals(k.Word))
                }).OrderByDescending(k => k.Frequency).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //
        //takes
        // GET: api/QueryKeywordAnalytics/5
        /// <summary>
        /// Return a list of all keywords wrapped w/ frequencies.
        /// Filters to only include queries placed by the given username.
        /// Frequencies represent how many queries that keyword has appeared in, sorted by descending.
        /// Returns an error code if user not found in DB.
        /// Only available for user matching username, or admin.
        /// </summary>
        /// <param name="username">name of user to look up keyword frequency for</param>
        /// <returns>List of FrequencyWrapperModel<string></returns>
        [Authorize]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [HttpGet("{username}", Name = "GetKeywordQueryAnalytics")]
        public ActionResult<List<FrequencyWrapper<string>>> Get(string username)
        {
            //[Authorize] handles returning 401 if noone is logged in.
            if (!(User.Identity.Name.Equals(username) || User.IsInRole("admin")))
            {
                return StatusCode(403);//Forbidden
            }
            try
            {
                return Krepo.GetKeywords().Select(k => new FrequencyWrapper<string>()
                {
                    Obj = k.Word,
                    Frequency = Krepo.GetQueryKeywordJunction().Where(s => s.Query.Username.Equals(User.Identity.Name) && s.Word.Equals(k.Word)).Count()
                }).OrderByDescending(k => k.Frequency).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
    }
}

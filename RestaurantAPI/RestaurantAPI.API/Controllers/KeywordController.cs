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
    public class KeywordController : ControllerBase
    {
        public KeywordController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        // GET: api/Keyword
        /// <summary>
        /// Gets a list of all keywords in the database
        /// </summary>
        /// <returns>IEnumerable of strings</returns>
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                return Krepo.GetKeywords().Select(k => k.Word).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Keyword/5
        /// <summary>
        /// Given a keyword, returns all restaurants associated to that keyword
        /// </summary>
        /// <param name="keyword">The keyword to match restaurants for</param>
        /// <returns>IEmumerable of RestaurantModels</returns>
        [ProducesResponseType(500)]
        [HttpGet("{keyword}", Name = "GetRestaurantsForKeyword")]
        public ActionResult<IEnumerable<RestaurantModel>> Get(string keyword)
        {
            try
            {
                return Rrepo.GetRestaurants(true).Where(k => k.RestaurantKeywordJunction.Any(rkj => rkj.Word.Equals(keyword))).Select(k => Mapper.Map(k)).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
    }
}

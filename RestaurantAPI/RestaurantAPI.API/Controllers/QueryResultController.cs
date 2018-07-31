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
    public class QueryResultController : ControllerBase
    {
        public QueryResultController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        /// <summary>
        /// Get all queries placed by a username, or all queries in DB if user is an admin.
        /// </summary>
        /// <returns>list of QueryModels</returns>
        // GET: api/Query
        [Authorize]
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<List<QueryResult>> Get()
        {
            if (User == null)
                return StatusCode(401);//unauthorized, in case User is null for some reason like the tests.
            var queryList = Qrepo.GetQueries();
            if (queryList == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!User.IsInRole("admin"))
                queryList = queryList.Where(q => q.Username.Equals(User.Identity.Name));

            return queryList.Select(m => new QueryResult() { QueryObject = Mapper.Map(m), Restaurants = Mapper.Map(Qrepo.GetRestaurantsInQuery(m.Id)).ToList() } ).ToList();
        }

        /// <summary>
        /// Returns a specific query based on its Id only if it matches the current user or they're admin.
        /// </summary>
        /// <param name="id">queryId to look up</param>
        /// <returns>A QueryModel object matching the given Id</returns>
        // GET: api/Query/5
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<QueryResult>> GetAsync(int id)
        {
            Query q;
            try
            {
                q = await Qrepo.GetQueryByIDAsync(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            if (!(User.Identity.Name.Equals(q.Username) || User.IsInRole("admin")))
            {
                return StatusCode(403);//Forbidden
            }
            return new QueryResult() { QueryObject = Mapper.Map(q), Restaurants = Mapper.Map( await Qrepo.GetRestaurantsInQueryAsync(q.Id)).ToList() };
        }

        /// <summary>
        /// Used to add new QueryResults to DB.
        /// 1. Adds the query to the DB
        /// 2. Adds any new restaurants to the DB that don't already exist, and register new keyword associations to each restaurant
        /// 3. Adds data to QueryRestaurantJunction table
        /// Query Id should be 0 for new queries.
        /// **Not idempotent - will assign a new id to the query each time and add a new query to the DB each time it is called.
        /// </summary>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        // POST: api/Query
        [HttpPost]
        public async Task<ActionResult<List<RestaurantModel>>> PostAsync([FromBody] QueryResult queryResult)
        {
            //Add query to DB
            Query q = Mapper.Map(queryResult.QueryObject);
            List<Restaurant> restaurants = Mapper.Map(queryResult.Restaurants).ToList();
            try
            {
                await Qrepo.AddQueryAsync(q, (KeywordRepo)Krepo);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                //Add any new restaurants to DB, and register any new keywords to existing restaurants
                await Rrepo.AddNewRestaurantsAsync(restaurants, queryResult.QueryObject.Keywords);
                //Add query+restaurants to junction table
                await Qrepo.AddQueryRestaurantJunctionAsync(q.Id, restaurants, (RestaurantRepo)Rrepo);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("Get", new { id = q.Id }, queryResult);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class QueryController : ControllerBase
    {
        public QueryController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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
        [HttpGet]
        public ActionResult<List<QueryModel>> Get()
        {
            if (User == null)
                return StatusCode(401);//unauthorized, in case User is null for some reason like the tests.
            var queryList = Qrepo.GetQueries();
            List<QueryModel> queryModelList;
            if (queryList == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (User.IsInRole("admin"))
                queryModelList = Mapper.Map(queryList).ToList();
            else
                queryModelList = Mapper.Map(queryList.Where(q => q.Username.Equals(User.Identity.Name))).ToList();

            return queryModelList.ToList();
        }

        /// <summary>
        /// Returns a specific query based on its Id only if it matches the current user or they're admin.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A QueryModel object matching the given Id</returns>
        // GET: api/Query/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<QueryModel> Get(int id)
        {
            Query q;
            try
            {
                q = Qrepo.GetQueryByID(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            if (!(User.Identity.Name.Equals(q.Username) || User.IsInRole("admin")))
            {
                return StatusCode(403);//Forbidden
            }
            return Mapper.Map(q);
        }

        // POST: api/Query
        [HttpPost]
        public ActionResult<List<RestaurantModel>> Post([FromBody] QueryResult queryResult)
        {
            //Add query to DB
            Query q = Mapper.Map(queryResult.QueryObject);
            List<Restaurant> restaurants = Mapper.Map(queryResult.Restaurants).ToList();
            try
            {
                Qrepo.AddQuery(q, (KeywordRepo)Krepo);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            //Add any new restaurants to DB, and register any new keywords to existing restaurants

            try
            {
                Rrepo.AddNewRestaurants(restaurants, queryResult.QueryObject.Keywords);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //Add query+restaurants to junction table

            try
            {
                Qrepo.AddQueryRestaurantJunction(q.Id, restaurants, (RestaurantRepo)Rrepo);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //return StatusCode(StatusCodes.Status201Created);
            return CreatedAtRoute("Get", new { id = q.Id }, queryResult);
        }

        /*  No need for update or delete methods for queries
         *  
        // PUT: api/Query/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        *
        */
    }
}

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


        // GET: api/Query
        [HttpGet]
        public ActionResult<List<QueryModel>> Get()
        {
            var queryList = Qrepo.GetQueries();
            if (queryList == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var queryModelList = Mapper.Map(queryList);

            return queryModelList.ToList();
        }

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

            return Mapper.Map(q);
        }

        // POST: api/Query
        [HttpPost]
        public ActionResult<List<RestaurantModel>> Post([FromBody] QueryModel queryM)
        {
            //Add query to DB
            try
            {
                Qrepo.AddQuery(Mapper.Map(queryM), (KeywordRepo)Krepo);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            //Execute request of Google Search API to get some list of RestaurantModels back
            //TODO:  connect this controller with Google Search API functionality
            List<RestaurantModel> queryResults = new List<RestaurantModel>();
            //Add any new restaurants to DB, and register any new keywords to existing restaurants

            try
            {
                Rrepo.AddNewRestaurants(Mapper.Map(queryResults).ToList(), queryM.Keywords);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return queryResults;
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

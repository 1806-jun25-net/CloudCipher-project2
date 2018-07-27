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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : Controller
    {
        public RestaurantController(AppUserRepo AppRepo, KeywordRepo KeyRepo, QueryRepo QRepo, RestaurantRepo RestRepo)
        {
            Arepo = AppRepo;
            Krepo = KeyRepo;
            Qrepo = QRepo;
            Rrepo = RestRepo;
        }

        public AppUserRepo Arepo { get; set; }
        public KeywordRepo Krepo { get; set; }
        public QueryRepo Qrepo { get; set; }
        public RestaurantRepo Rrepo { get; set; }


        // GET: api/<controller>
        [HttpGet]
        public ActionResult <IEnumerable<RestaurantModel>> Get()
        {
            Rrepo.GetRestaurants();

            return Mapper.Map(Rrepo.GetRestaurants().ToList());
            
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetRestaurant")]
        public ActionResult<RestaurantModel> Get(int id)
        {
            Restaurant grabVariable;
            try
            {
                grabVariable = Rrepo.GetRestaurantByID(id);
            }
            catch(Exception g)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Mapper.Map(grabVariable);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Create([FromBody]RestaurantModel value)
        {
            Restaurant createVariable;

            createVariable = Mapper.Map(value);

            try
            {
                Rrepo.AddRestaurant(createVariable);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Rrepo.Save();

            return CreatedAtRoute("GetRestaurant", new { Id = value.Id }, value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

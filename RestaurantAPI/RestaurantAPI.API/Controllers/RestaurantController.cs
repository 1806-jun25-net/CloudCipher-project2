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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantAPI.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : Controller
    {
        public RestaurantController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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


        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public ActionResult<List<RestaurantModel>> Get()
        {
            Rrepo.GetRestaurants();

            return Mapper.Map(Rrepo.GetRestaurants().ToList()).ToList();
            
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetRestaurant")]
        [Authorize]
        public ActionResult<RestaurantModel> Get(int id)
        {
            Restaurant grabVariable;
            try
            {
                grabVariable = Rrepo.GetRestaurantByID(id);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Mapper.Map(grabVariable);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
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
        [Authorize(Roles = "admin")]
        public void Delete(int id)
        {
        }
    }
}

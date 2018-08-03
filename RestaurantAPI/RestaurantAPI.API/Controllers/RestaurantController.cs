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
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<List<RestaurantModel>> Get()
        {
            try
            {
                return Mapper.Map(Rrepo.GetRestaurants(true)).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ProducesResponseType(400)]
        // GET api/<controller>/5
        [HttpGet("{id}", Name = "GetRestaurant")]
        public async Task<ActionResult<RestaurantModel>> GetAsync(string id)
        {
            Restaurant r;
            try
            {
                r = await Rrepo.GetRestaurantByIDAsync(id, true);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Mapper.Map(r);
        }

        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody]RestaurantModel value)
        {
            Restaurant createVariable;

            createVariable = Mapper.Map(value);

            try
            {
                await Rrepo.AddRestaurantAsync(createVariable);
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                await Rrepo.SaveAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtRoute("GetRestaurant", new { Id = value.Id }, value);
        }

        //TODO: Implement this method or delete
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        //TODO: Implement this method or delete
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public void Delete(int id)
        {
        }
    }
}

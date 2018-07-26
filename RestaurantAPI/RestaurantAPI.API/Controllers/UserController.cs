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
    public class UserController : Controller
    {
        public UserController(AppUserRepo AppRepo, KeywordRepo KeyRepo, QueryRepo QRepo, RestaurantRepo RestRepo)
        {
           Arepo = AppRepo;
           Krepo = KeyRepo;
           Qrepo  = QRepo;
           Rrepo = RestRepo; 
        }

        public AppUserRepo Arepo { get; set; }
        public KeywordRepo Krepo { get; set; }
        public QueryRepo Qrepo { get; set; }
        public RestaurantRepo Rrepo { get; set; }

        
        // GET: api/<controller>
        [HttpGet]
        public ActionResult <IEnumerable<string>> Get()
        {
            Arepo.GetUsers();
            //  Krepo.GetKeywords();
            //  Qrepo.GetQueries();
            //  Rrepo.GetRestaurants();

            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // GET api/<controller>/5
        /*
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */    

        // POST api/<controller>
        [HttpPost]
        public IActionResult Create([FromBody]UserModel value)
        {
            AppUser createVariable;

            createVariable = Mapper.Map(value);

            try
            {
                Arepo.AddUser(createVariable);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Arepo.Save();

            return CreatedAtRoute("GetUser", new { username = value.Username }, value);
           

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

        [HttpGet("{username}", Name = "GetUser")]
        [Route("api/User")]
        public ActionResult<UserModel> GetByUsername(string username)
        {
            AppUser userVariable;
            try
            {
                userVariable = Arepo.GetUserByUsername(username);
            }

            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Mapper.Map(userVariable);

            
        }

        



    }
}

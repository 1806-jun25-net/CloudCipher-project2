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
        [Authorize(Roles = "admin")]//checking if you are in some role, to access something
        public ActionResult <List<UserModel>> Get()
        {
            var userlist = Arepo.GetUsers();
            if (userlist == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // return StatusCode(StatusCodes.Status501NotImplemented);

            //SET THIS RETURN MAINLY FOR TESTING CONNECTION TO MVC
            return Mapper.Map(userlist).ToList();
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
        [AllowAnonymous]
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
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<UserModel> GetByUsername(string username)
        {
            if(User.Identity.Name != username && User.IsInRole("admin") != true)
            {
                return StatusCode(403);//Forbidden
            }
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

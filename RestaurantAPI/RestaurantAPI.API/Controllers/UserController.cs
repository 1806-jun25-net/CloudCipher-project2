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
        public UserController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
        {
           Arepo = AppRepo;
           Krepo = KeyRepo;
           Qrepo  = QRepo;
           Rrepo = RestRepo;
        }

        public IAppUserRepo Arepo { get; set; }
        public IKeywordRepo Krepo { get; set; }
        public IQueryRepo Qrepo { get; set; }
        public IRestaurantRepo Rrepo { get; set; }

        
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

            return Mapper.Map(userlist).ToList();
        }  

        // POST api/<controller>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody]UserModel value)
        {
            AppUser createVariable;

            createVariable = Mapper.Map(value);

            try
            {
                await Arepo.AddUserAsync(createVariable);
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            await Arepo.SaveAsync();

            return CreatedAtRoute("GetUser", new { username = value.Username }, value);
           

        }

        // PUT api/<controller>/5
        [HttpPut("{username}", Name = "UpdateUser")]
        public async Task<IActionResult> PutAsync(string username, [FromBody]UserModel value)
        {
            AppUser updateVariable;
            if (!username.Equals(value.Username))
                return StatusCode(StatusCodes.Status400BadRequest);
            if (!(User.Identity.Name.Equals(username) || User.IsInRole("admin")))
            {
                return StatusCode(403);//Forbidden
            }
            updateVariable = Mapper.Map(value);

            try
            {
                Arepo.UpdateUser(updateVariable);
                await Arepo.SaveAsync();
            }

            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            await Arepo.SaveAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }
        
        [HttpGet("{username}", Name = "GetUser")]
        [Route("api/User")]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserModel>> GetByUsernameAsync(string username)
        {
            if (User == null)
                return StatusCode(401);//unauthorized, in case User is null for some reason like the tests.

            if(!(User.Identity.Name.Equals(username) || User.IsInRole("admin")) )
            {
                return StatusCode(403);//Forbidden
            }
            AppUser userVariable;
            try
            {
                userVariable = await Arepo.GetUserByUsernameAsync(username);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return Mapper.Map(userVariable);

            
        }

        



    }
}

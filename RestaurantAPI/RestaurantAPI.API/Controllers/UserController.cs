﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Library.Repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
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
        public IEnumerable<string> Get()
        {
            Arepo.GetUsers();
          //  Krepo.GetKeywords();
          //  Qrepo.GetQueries();
          //  Rrepo.GetRestaurants();

            getbyusername UserModel()
            {
                 


            }


            return Mapper.Map(  );
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
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

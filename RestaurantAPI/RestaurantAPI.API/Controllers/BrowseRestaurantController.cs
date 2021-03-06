﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using RestaurantAPI.API.Models;
using RestaurantAPI.Library;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowseRestaurantController : ControllerBase
    {
        public BrowseRestaurantController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/BrowseRestaurant
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        // GET: api/BrowseRestaurant/5
        [HttpGet("{search}", Name = "SearchTerm")]
        public ActionResult<List<RestaurantModel>> Get(string search)
        {
            Rrepo.GetRestaurants(true);
            string [] stringarray = search.Split(new Char[]{' '});
            List<List<RestaurantModel>> Listoflists = new List<List<RestaurantModel>>();
            
            foreach (var word in stringarray)
            {
               
                try
                {
                    Listoflists.Add(Rrepo.GetRestaurants(true).Where(k => k.RestaurantKeywordJunction.Any(rkj => rkj.Word.Equals(word))).Select(k => Mapper.Map(k)).ToList());
                    Listoflists.Add(Rrepo.GetRestaurants(true).Where(k => k.Name.Contains(word)).Select(k => Mapper.Map(k)).ToList());
                }
                catch (DbUpdateException ex)
                {
                    logger.Error(ex, ex.ToString());
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            List<RestaurantModel> results = new List<RestaurantModel>();

            foreach (var list in Listoflists)
            {
                
                foreach (var restaurant in list)
                {
                    //output each restaurant from each list that matches what was searched
                    if (User.Identity.IsAuthenticated)
                    {
                       if (!Arepo.GetBlacklistForUser(User.Identity.Name).Any(c => c.Id.Equals(restaurant.Id)) && !results.Contains(restaurant))
                       {
                            results.Add(restaurant);
                       }

                    }
                    else
                    {
                        if (!results.Contains(restaurant))
                        {
                            results.Add(restaurant);
                        }
                    }

                }

            }

            return results;
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantKeywordAnalyticsController : ControllerBase
    {
        public RestaurantKeywordAnalyticsController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        // GET: api/RestaurantKeywordAnalytic
        /// <summary>
        /// Return a list of all Keywords wrapped w/ frequencies.
        /// Frequencies represent how many restaurants in DB match that keyword.
        /// Sorted by most frequent to least
        /// Available to all users
        /// </summary>
        /// <returns>List of FrequencyWrapper of string </returns>
        [ProducesResponseType(500)]
        [HttpGet]
        public ActionResult<List<FrequencyWrapper<string>>> Get()
        {
            try
            { 
                return Krepo.GetKeywords().Select(k => new FrequencyWrapper<string>()
                {
                    Obj = k.Word,
                    Frequency = Krepo.GetRestaurantKeywordJunction().Count(w => w.Word.Equals(k.Word))
                }).OrderByDescending(k => k.Frequency).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/RestaurantKeywordAnalytics/5
        /// <summary>
        /// Given a restaurantId, return all keywords associated with that restaurant.
        /// Returns an error code if restaurant not found in DB.
        /// Accessible to all users.
        /// </summary>
        /// <param name="restaurantId">Keyword to get restaurant matches for</param>
        /// <returns>List of restaurants </returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("{restaurantId}", Name = "GetKeywordsForRestaurant")]
        public async Task<ActionResult<List<string>>> GetAsync(string restaurantId)
        {
            Restaurant r;
            try
            {
                r = await Rrepo.GetRestaurantByIDAsync(restaurantId, true);
            }
            catch (Exception e)
            {
                //if requested restaurantID not in DB
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                return r.RestaurantKeywordJunction.Select(n => n.Word).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
    }
}

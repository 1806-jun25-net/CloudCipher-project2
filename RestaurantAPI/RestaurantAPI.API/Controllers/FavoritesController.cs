﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using RestaurantAPI.API.Models;
using RestaurantAPI.Data;
using RestaurantAPI.Library;
using RestaurantAPI.Library.Repos;

namespace RestaurantAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        public FavoritesController(IAppUserRepo AppRepo, IKeywordRepo KeyRepo, IQueryRepo QRepo, IRestaurantRepo RestRepo)
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

        //Return list of all favorited Restaurants for a given user
        // GET: api/Favorites
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<RestaurantModel>>> GetAsync()
        {
            //Since this method is authorized by Identity, it will automatically handle returning 401 if user isn't logged in.
            try
            {
                return Mapper.Map(await Arepo.GetFavoritesForUserAsync(User.Identity.Name)).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Given a restaurantID, returns bool of whether restaurant is in user's favorites
        // GET: api/Favorites/5
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize]
        [HttpGet("{rId}", Name = "GetFavorites")]
        public async Task<ActionResult<bool>> GetAsync(string rId)
        {
            //Since this method is authorized by Identity, it will automatically handle returning 401 if user isn't logged in.
            try
            {
                return (await Arepo.GetFavoritesForUserAsync(User.Identity.Name)).Any(n => n.Id.Equals(rId));
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Given a restaurant id as a parameter, add the restaurant to the current user's favorites
        // POST: api/Favorites
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] StringModel sm)
        {
            try
            {
                await Arepo.AddRestaurantToFavoritesAsync(User.Identity.Name, sm.Value, (RestaurantRepo)Rrepo);
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                await Rrepo.SaveAsync();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }


        //Given a restaurant id as a parameter, remove the restaurant from the current user's favorites
        // DELETE: api/ApiWithActions/5
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize]
        [HttpDelete("{value}")]
        public async Task<IActionResult> DeleteAsync(string value)
        {
            try
            {
                await Arepo.RemoveRestaurantFromFavoritesAsync(User.Identity.Name, value, (RestaurantRepo)Rrepo);
            }

            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                await Rrepo.SaveAsync();
            }
            catch (Exception e)
            {
                logger.Error(e, e.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}

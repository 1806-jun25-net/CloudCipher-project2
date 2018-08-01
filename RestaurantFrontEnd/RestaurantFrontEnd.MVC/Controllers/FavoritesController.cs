﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantFrontEnd.Library.API_Models;

namespace RestaurantFrontEnd.MVC.Controllers
{
    public class FavoritesController : AServiceController
    {

        public FavoritesController(HttpClient httpClient) : base(httpClient)
        {

        }

        // GET: Favorites
        public async Task<ActionResult> Index([FromQuery] string search = "")
        {
            var request = CreateRequestService(HttpMethod.Get, "api/user");

            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            List<Restaurant> user = JsonConvert.DeserializeObject<List<Restaurant>>(jsonString);

            return View(@"..\User\Index", user);

             
        }

        // GET: Favorites/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Favorites/Create
        public async Task<ActionResult> Create(string id)
        {

            try
            {
                string jsonString = JsonConvert.SerializeObject(id);
                //var uri = apiserviceuri + "user";
                var request = CreateRequestService(HttpMethod.Post, "api/favorites");

                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                TempData["Message"] = "Added to Favorites"; 
            }
            catch
            {
                TempData["Message"] = "Sorry, something went wrong";
            }
            return Redirect(Url.Action("Details", "Restaurant") + "/" + id);
        }

        // POST: Favorites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Favorites/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Favorites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Favorites/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            
            if (id == null)
                return View("Error");
            var request = CreateRequestService(HttpMethod.Get, $"api/favorites/{id}");

            try
            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                

                return View(@"..\restaurant\Details", id);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", ex);
            }

           
        }

        // POST: Favorites/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            return View();
        }
    }
    
}
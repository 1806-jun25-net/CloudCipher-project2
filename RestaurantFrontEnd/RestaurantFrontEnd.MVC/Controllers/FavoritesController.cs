using System;
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
        public async Task<ActionResult> Index()
        {
            var request = CreateRequestService(HttpMethod.Get, "api/favorites");

            var response = await HttpClient.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            List<Restaurant> user = JsonConvert.DeserializeObject<List<Restaurant>>(jsonString);

            return View(@"..\Favorites\view_faves", user);

             
        }

        //// GET: Favorites/Details/5
        //public ActionResult Details(int id)
        //{
        //    //var request = CreateRequestService(HttpMethod., "api/favorites");
        //    return View();
        //}

        // GET: Favorites/Create
        public async Task<string> Create(string id)//ADD REST TO FAVES 
        {

            stringmodel favemodel = new stringmodel() {
                value = id,
            };


            //check if rest already in faves before trying to add
            try
            {
                string jsonString = JsonConvert.SerializeObject(id);
                var request = CreateRequestService(HttpMethod.Get, "api/favorites/"+id);
                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await HttpClient.SendAsync(request);


                

                if (!response.IsSuccessStatusCode)
                {
                    ViewData["addedfave"] = "Error in check faves response";
                    return (string)ViewData["addedfave"];
                }
                string jsonString2 = await response.Content.ReadAsStringAsync();
                bool user = JsonConvert.DeserializeObject<bool>(jsonString2);
                TempData.Add("restfavecheck", user);
            }
            catch
            {
                TempData["Message"] = "Sorry, something went wrong";
            }
           

           //if new rest fave,then add
            if((bool)TempData["restfavecheck"]==false)
            {
                try
                {
                    string jsonString = JsonConvert.SerializeObject(favemodel);
                    //var uri = apiserviceuri + "user";
                    var request = CreateRequestService(HttpMethod.Post, "api/favorites");

                    request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                    var response = await HttpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewData["errorfave"] = "Error in adding fave to db";
                        return (string)ViewData["errorfave"];
                    }
                    ViewData["Message"] = "Added to Favorites";
                    
                }
                catch
                {
                    TempData["Message"] = "Sorry, something went wrong";
                }
                ViewData["addedfave"] ="added to faves";
                return (string)ViewData["addedfave"]; ;//EVENTUALLY CHANGE TO A SIMPLE ALERT/NOTIFICATION

            }
            else
            {
                TempData["alreadyfave"] = "restaurant already in your faves";
                return (string)TempData["alreadyfave"];
            }
        }

        //// POST: Favorites/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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
            var request = CreateRequestService(HttpMethod.Delete, $"api/favorites/{id}");


            try

            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                

                return RedirectToAction(nameof(Index));
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
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
    public class BlacklistController : AServiceController
    {

        public BlacklistController(HttpClient httpClient) : base(httpClient)
        {

        }


        // GET: Blacklist
        public async Task<ActionResult> Index()
        {
            var request = CreateRequestService(HttpMethod.Get, "api/blacklist");

            var response = await HttpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            List<Restaurant> user = JsonConvert.DeserializeObject<List<Restaurant>>(jsonString);

            return View(@"..\Blacklist\view_blacklist", user);


        }

        public async Task<string> LoginCheck()
        {
            var request = CreateRequestService(HttpMethod.Get, "api/blacklist");

            var response = await HttpClient.SendAsync(request);
            

            if (!response.IsSuccessStatusCode)
            {
                TempData["Username"] = null;
                return "logged out";
            }
            return (string)TempData.Peek("Username");
        }

        // GET: Blacklist/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Blacklist/Create
        public async Task<string> Create(string id)
        {
            stringmodel blistmodel = new stringmodel()
            {
                value = id,
            };
            //check if rest already in faves before trying to add
            try
            {
                string jsonString = JsonConvert.SerializeObject(id);
                var request = CreateRequestService(HttpMethod.Get, "api/blacklist/" + id);
                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

              

                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["addedblacklist"] = "Error in check blacklist response";
                    return (string)ViewData["addedblacklist"];
                }
                string jsonString2 = await response.Content.ReadAsStringAsync();
                bool user = JsonConvert.DeserializeObject<bool>(jsonString2);
                TempData.Add("restblistcheck", user);
            }
            catch
            {
                TempData["Message"] = "Sorry, something went wrong";
            }


            //if new rest fave,then add
            if ((bool)TempData["restblistcheck"] == false)
            {
                try
                {
                    string jsonString = JsonConvert.SerializeObject(blistmodel);
                    //var uri = apiserviceuri + "user";
                    var request = CreateRequestService(HttpMethod.Post, "api/blacklist");

                    request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                    var response = await HttpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewData["errorfave"] = "Error in adding blaclisted restaurant to db";
                        return (string)ViewData["errorblacklist"];
                    }
                    ViewData["Message"] = "Added to Favorites";

                }
                catch
                {
                    TempData["Message"] = "Sorry, something went wrong";
                }
                ViewData["addedblacklist"] = "blacklisted";
                return (string)ViewData["addedblacklist"]; ;//EVENTUALLY CHANGE TO A SIMPLE ALERT/NOTIFICATION

            }
            else
            {
                TempData["alreadyblacklisted"] = "restaurant already blacklisted";
                return (string)TempData["alreadyblacklisted"];
            }
        }

        // POST: Blacklist/Create
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

        // GET: Blacklist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Blacklist/Edit/5
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

        // GET: Blacklist/Delete/5
        public async Task<ActionResult> Delete(string id)
        {


            if (id == null)
                return View("Error");
            var request = CreateRequestService(HttpMethod.Delete, $"api/blacklist/{id}");

            

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

        // POST: Blacklist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantFrontEnd.MVC.Controllers
{
    public class RestaurantController : AServiceController
    {

        public RestaurantController(HttpClient httpClient):base(httpClient)
        {

        }
        // GET: Restaurant
        public ActionResult Index([FromQuery] string search="")
        {
            var request = CreateRequestService(HttpMethod.Post, "api/Restaurant");
            //change to Query once you have authorization set up

            if (search==null || search == "")
            {
                ///request = CreateRequestService(HttpMethod.Get, "api/restaurant")
                //SAVE ABOVE FOR LATER//WIll be sending this request to Query
                //controller in the form of a list
                return View(@"..\Restaurant\Index");
            }
            else
            {
                
                //ISCHECKED FOR CHECKBOXES FEATURE(RESEARCH)
                string queryString = search;
                var queryArray = queryString.Split("+");
                //string firstElem = array.First();
                string queried = string.Join("&", queryArray);
                ViewData["query"] = queried;
                return View(@"..\Restaurant\Index");
            }
            
          

        }

        // GET: Restaurant/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
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

        // GET: Restaurant/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Restaurant/Edit/5
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

        // GET: Restaurant/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Restaurant/Delete/5
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
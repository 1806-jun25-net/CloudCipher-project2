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
    public class RestaurantController : AServiceController
    {

        public RestaurantController(HttpClient httpClient):base(httpClient)
        {

        }
        // GET: Restaurant
        public async Task<ActionResult> Index([FromQuery] string search = "")
        {

            //blacklist check
            var request1 = CreateRequestService(HttpMethod.Get, "api/blacklist/");


          

            var response1 = await HttpClient.SendAsync(request1);
            if (!response1.IsSuccessStatusCode)
            {
                return View("Error");
            }
            string jsonString1 = await response1.Content.ReadAsStringAsync();
            List<Restaurant> user1 = JsonConvert.DeserializeObject<List<Restaurant>>(jsonString1);
            ViewBag.blist = user1;
            List<string> blistIdString = new List<string>();
            foreach (var n in user1)
            {
                blistIdString.Add(n.Id);
            }
            string[] newblist = blistIdString.ToArray();
            TempData["blacklistcheck1"] = newblist;
            ////////
     
            //change to Query once you have authorization set up

            if (search == null || search == "")
            {

                string cookieValue = Request.Cookies[s_CookieName];

                //RETURNING SUGGESTIONS FOR
                if (cookieValue != null)
                {
                    if (TempData.Peek("Username") != null)
                    {
                       
                        //QueryResult Check
                        var request = CreateRequestService(HttpMethod.Get, "api/QueryResult/");
                        
                        try
                        {
                            var response = await HttpClient.SendAsync(request);

                            if (!response.IsSuccessStatusCode)
                            {
                                return View("Error");
                            }

                            string jsonString = await response.Content.ReadAsStringAsync();
                            List<QueryResult> user = JsonConvert.DeserializeObject<List<QueryResult>>(jsonString);
                            
                            if (user.Count()== 0)
                            {
                                return View(@"..\Restaurant\Index");
                            }
                            else
                            {
                                return View(@"..\Restaurant\Index", user);
                            }

                        }
                        catch (HttpRequestException ex)
                        {
                            return View("Error");
                        };
                    }
                    else
                    {
                        return View(@"..\Restaurant\Index");
                    }
                }
                else
                {
                    return View(@"..\Restaurant\Index");
                }
                

                ///////////
            }
            else
            {

                //ISCHECKED FOR CHECKBOXES FEATURE(RESEARCH)
                string queryString = search;
                var queryArray = queryString.Split(" ");
                //string firstElem = array.First();
                string queried = string.Join("&", queryArray);
                ViewData["query"] = queried;

                return View(@"..\Restaurant\Index");
            }



        }
        [HttpPost]
        
        public async Task<string>  GetQueryResults(string[] queryobj,
            List<Restaurant> restobj,string[] keyobj)
        {
            
            List<string>kw = new List<string>();
            foreach(var n in keyobj)
            {
                string character = "";
                foreach (var l in n)
                {
                    if(char.IsLetter(l))
                    character +=l;
                }

                kw.Add(character);
            }
            foreach(var k in restobj)
            {
                k.Keywords = kw;
                
            }


            QueryResult QR = new QueryResult
            {
                QueryObject = new Query
                {
                    Id = 0,
                    Username = (string)TempData.Peek("Username"),
                    Lat = (string)queryobj[0],
                    Lon = (string)queryobj[1],
                    Radius = Convert.ToInt32(queryobj[2]),
                    Keywords = kw,
                    QueryTime = DateTime.Now.Date

                },
                Restaurants = restobj




            };
            //QR;
            try
            {
                string result;
                string jsonString = JsonConvert.SerializeObject(QR);
                var request = CreateRequestService(HttpMethod.Post, "api/queryresult");

                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    result = "error";
                    return result;
                }

                result = restobj[0].Name + queryobj[0] + keyobj[0];
                return result;
            }
            catch
            {
                string result = "error";
                return result;
            }


        }

   
        

        //////////////

        // GET: Restaurant/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
                return View("Error");
            var request = CreateRequestService(HttpMethod.Get, $"api/restaurant/{id}");

           
            try
            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                Restaurant restaurant = JsonConvert.DeserializeObject<Restaurant>(jsonString);

                return View(@"..\Restaurant\Details", restaurant);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", ex);
            }
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

        public async Task<ActionResult> BrowseRestaurants([FromQuery] string search = "")
        {
            HttpRequestMessage request;

           
            if (string.IsNullOrEmpty(search))
            {
                request = CreateRequestService(HttpMethod.Get, "api/restaurant");
            }

            else
            {
                request = CreateRequestService(HttpMethod.Get, "api/Keyword/"+search);
            }


            try
            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                List<Restaurant> restaurant = JsonConvert.DeserializeObject<List<Restaurant>>(jsonString);

                return View(@"..\Restarant\Index", restaurant);
            }

            catch (HttpRequestException ex)
            {
                return View("Error", ex);
            }



        }
    }
}
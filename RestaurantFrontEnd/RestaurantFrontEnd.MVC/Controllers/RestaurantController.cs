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
            //var requestg = CreateRequestService(HttpMethod.Get, "api/Restaurant");
          
            //change to Query once you have authorization set up

            if (search == null || search == "")
            {
                string cookieValue = Request.Cookies[s_CookieName];

                //RETURNING SUGGESTIONS FOR
                if (cookieValue != null)
                {
                    if (TempData.Peek("Username") != null)
                    {

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

                            //IEnumerable<Restaurant> GetUserRests()
                            //{

                            //    // IEnumerable<Restaurant> user_rests = new List<Restaurant>();
                            //    List<Restaurant> user_rests = new List<Restaurant>();
                            //    foreach (var m in user)
                            //    {
                            //        if (m.QueryObject.Username == (string)TempData.Peek("Username"))
                            //        {
                            //            foreach (var l in m.Restaurants)
                            //            {
                            //                user_rests.Add(l);
                            //            }
                            //        }
                            //    }
                            //    IEnumerable<Restaurant> userRests = user_rests;
                            //    return userRests;
                            //}
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
                /////request = CreateRequestService(HttpMethod.Get, "api/restaurant")
                ////SAVE ABOVE FOR LATER//WIll be sending this request to Query
                ////controller in the form of a list

                //DONT FORGET THIS: IF NEW TO APP THIS SHOULD BE RETURNED
                //MAYBE ABOVE SUGGESTIONS PROCESS CAN STILL FUNCITON AS LONG AS IT DOESNT RETURN
                //ERROR PAGE IF NO RESULTS RETURNED...SHOULD CONTINUE ON TO EMPTY VIEW

                //return View(@"..\Restaurant\Index");

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
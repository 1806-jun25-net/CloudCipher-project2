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
    public class UserController : AServiceController
    {
        //public HttpClient HttpClient {get;set;}
        //private readonly static string apiserviceuri= "http://localhost:58756/api/";
        
        public UserController(HttpClient httpClient) :base(httpClient)
        {
           
        }
        
        // GET: User/Users
        //feel free to modify the if statements if you have a simpler
        //solution( switch statement?)
        public async Task<ActionResult> Index([FromQuery] string search = "")
        {
            var request = CreateRequestService(HttpMethod.Get, "api/user");

            //var uri = apiserviceuri + "user/";
            if(search != null && search != "")
            {
                //var search_uri = apiserviceuri + "user/" + search;
                request = CreateRequestService(HttpMethod.Get, "api/user/"+search);

                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if(!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }

                    string jsonString = await response.Content.ReadAsStringAsync();

                    User user = JsonConvert.DeserializeObject<User>(jsonString);

                    //View required an iterable as model param.Open for Mods
                    IEnumerable<User>GetUserInfo()
                    {
                        IEnumerable<User> users = new List<User> { user };
                        return users;
                    }

                    return View(@"..\User\Index",GetUserInfo());
                }
                catch(HttpRequestException ex)
                {
                    return View("Error", ex);
                }
            }

            else
            {
                

                try
                {
                    var response = await HttpClient.SendAsync(request);

                    if(!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }

                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<User> user = JsonConvert.DeserializeObject<List<User>>(jsonString);

                    return View(@"..\User\Index",user);
                }
                catch(HttpRequestException ex)
                {
                    return View("Error", ex);
                }
            }
           
        }

        
        // GET: User/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string username)
        {
            if (username == null)
                username = (string)TempData.Peek("Username");
            var request = CreateRequestService(HttpMethod.Get, $"api/user/{username}");
            
            try
            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(jsonString);

                return View(@"..\User\Details", user);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", ex);
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View(@"..\User\Create");
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            if(!ModelState.IsValid)
            {
                return View(@"..\User\Create",user);
            }

            try
            {
                string jsonString = JsonConvert.SerializeObject(user);
                //var uri = apiserviceuri + "user";
                var request = CreateRequestService(HttpMethod.Post, "api/user");

                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                

                var response = await HttpClient.SendAsync(request);

                if(!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                
                return Redirect(Url.Action("Index","User") +"?search="+user.Username);
            }
            catch
            {
                return View(@"..\User\Create");
            }
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string username)
        {
            var request = CreateRequestService(HttpMethod.Get, $"api/user/{username}");
            try
            {
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(jsonString);

                return View(@"..\User\Edit", user);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", ex);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(@"..\User\Edit\"+$"{user.Username}", user);
            }

            try
            {
                string jsonString = JsonConvert.SerializeObject(user);
                //var uri = apiserviceuri + "user";
                var request = CreateRequestService(HttpMethod.Put, "api/user/"+user.Username);

                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return Redirect(Url.Action("Details", "User") + "?Username=" + user.Username);
            }
            catch
            {
                return View(@"..\User\Edit", user.Username);
            }
        }


        /*
         * Don't need functionality for deleting users
        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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

    */
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantFrontEnd.Library.API_Models;

namespace RestaurantFrontEnd.MVC.Controllers
{
    public class AccountController : AServiceController
    {
        // how do we know what username and what roles we have for subsequent requests?
        // could make every API model include that info, so we know. (e.g. model base class)

        // could make an API endpoint to just tell me that info, and then I call that endpoint
        // every time I need to know who I am in MVC, and put that info into e.g. ViewData.
        // that it would be a good candidate for a custom action filter.

        // especially since if i want to put something dynamic in the layout / navbar for my user,
        // then that would be every request.

        public AccountController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: Account/Register
        public ViewResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<ActionResult> Register(LoginUser loginUser)
        {
            if(!ModelState.IsValid || !loginUser.Password.Equals(loginUser.Password2))
            {
                return View(loginUser);
            }
            Login account = new Login()
            {
                Username = loginUser.Username,
                Password = loginUser.Password
            };
            User user = new User()
            {
                Username = loginUser.Username,
                FirstName = loginUser.FirstName,
                LastName = loginUser.LastName,
                Email = loginUser.Email
            };
            //Add Login to identity DB
            HttpRequestMessage apiRequest = CreateRequestService(HttpMethod.Post, "api/Account/Register", account);

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);

            }
            catch
            {
                return View(loginUser);
            }

            if(!apiResponse.IsSuccessStatusCode)
            {
                return View(loginUser);
            }
            
            //Add User to restaurant DB
            apiRequest = CreateRequestService(HttpMethod.Post, "api/User", user);
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);

            }
            catch
            {
                return View(loginUser);
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return View(loginUser);
            }

            PassCookiesToClient(apiResponse);

            return RedirectToAction("Login");
        }

        //GET: Account/Login
        public ViewResult Login()
        {
            return View();//Might need to use redirecttoaction
        }

        //POST: Account/Login
        [HttpPost]
        public async Task<ActionResult> Login(Login account)
        {
            HttpRequestMessage apiRequest = CreateRequestService(HttpMethod.Post, "api/Account/Login", account);

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch(AggregateException)
            {
                return View("Error");
            }

            if(!apiResponse.IsSuccessStatusCode)
            {
                if(apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return View("AccessDenied");
                }
                return View("Error");
            }

            PassCookiesToClient(apiResponse);
            TempData.Add("Username", account.Username);

            if (account.Username == "admin.2")
            {
                return RedirectToAction("Index", "User");
                //return Redirect(Url.Action("Index", "User") + "?search=" + account.Username);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //GET: Accout/Logout
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return View("Error");

            }
            HttpRequestMessage apiRequest = CreateRequestService(HttpMethod.Post, "api/Account/Logout");

            HttpResponseMessage apiResponse;

            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);

            }
            catch(AggregateException)
            {
                return View("Error");
            }

            if(!apiResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);
            TempData.Remove("Username");
            return RedirectToAction("Index", "Home");
        }

        private bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if(apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                string authValue = values.FirstOrDefault(x => x.StartsWith(s_CookieName));
                if(authValue != null)
                {
                    Response.Headers.Add("Set-Cookie", authValue);
                    return true;
                }
            }
            return false;
        }
    }
}
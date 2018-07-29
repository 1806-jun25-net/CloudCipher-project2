using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RestaurantFrontEnd.MVC.Controllers
{
    public abstract class AServiceController : Controller
    {
        private static readonly Uri s_serviceUri = new Uri("http://localhost:58756/");
        protected static readonly string s_CookieName = "Project2Auth";

        protected HttpClient HttpClient { get; }

        public AServiceController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpRequestMessage CreateRequestService(HttpMethod method,string uri, object body = null)
        {
            var apiRequest = new HttpRequestMessage(method,new Uri(s_serviceUri, uri));

            if(body != null)
            {
                string jsonString = JsonConvert.SerializeObject(body);
                apiRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            string cookieValue = Request.Cookies[s_CookieName];

            if(cookieValue != null)
            {
                apiRequest.Headers.Add("Cookie",new CookieHeaderValue(s_CookieName,cookieValue).ToString());
            }
            return apiRequest;
        }
    }
}
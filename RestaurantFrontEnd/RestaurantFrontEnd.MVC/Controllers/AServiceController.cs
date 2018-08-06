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
        private static readonly Uri s_LocalServiceUri = new Uri("http://localhost:58756/");
        private static readonly Uri s_AzureServiceUri = new Uri("https://cloudcipher-restrauntrecommendations.azurewebsites.net/");
        protected static readonly string s_CookieName = "Project2Auth"; 
        protected static readonly bool useAzureApi = true; //Use this to switch between Azure and local Api

        protected HttpClient HttpClient { get; }

        public AServiceController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpRequestMessage CreateRequestService(HttpMethod method,string uri, object body = null)
        {
            HttpRequestMessage apiRequest;
            if (useAzureApi)  //use AzureApi
            {
                apiRequest = new HttpRequestMessage(method, new Uri(s_AzureServiceUri, uri));
            }
            else  //use localhostApi
            {
                apiRequest = new HttpRequestMessage(method, new Uri(s_LocalServiceUri, uri));
            }

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
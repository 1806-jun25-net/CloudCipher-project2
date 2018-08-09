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
        private static readonly Uri s_LocalServiceUri = new Uri("http://localhost:58756/"); //1
        private static readonly Uri s_AzureServiceUri = new Uri("https://cloudcipher-restrauntrecommendations.azurewebsites.net/"); //2
        private static readonly Uri s_DockerUri = new Uri("https://api/"); //3
        protected static readonly string s_CookieName = "Project2Auth"; 
        protected static readonly int connectionToUse = 3; //Use this to switch between Azure and local Api

        protected HttpClient HttpClient { get; }

        public AServiceController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpRequestMessage CreateRequestService(HttpMethod method,string uri, object body = null)
        {
            HttpRequestMessage apiRequest;
            switch (connectionToUse)
            {
                case 1:
                    apiRequest = new HttpRequestMessage(method, new Uri(s_LocalServiceUri, uri));
                    break;
                case 2:
                    apiRequest = new HttpRequestMessage(method, new Uri(s_AzureServiceUri, uri));
                    break;
                case 3:
                    apiRequest = new HttpRequestMessage(method, new Uri(s_DockerUri, uri));
                    break;
                default:
                    apiRequest = new HttpRequestMessage(method, new Uri(s_LocalServiceUri, uri));
                    break;
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
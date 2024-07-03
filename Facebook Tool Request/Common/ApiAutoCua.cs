using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook_Tool_Request.Helpers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.Net;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium;

namespace Facebook_Tool_Request.Common
{
    internal class ApiAutoCua
    {
        public static string apiUrl = "http://localhost:5000/api/";

        public static bool addClone(string data)
        {
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "","", 0);
                string json = requestXNet.RequestPost(apiUrl + "clone/add", data, "application/json");
                JObject jObject = JObject.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in addClone method: {ex.Message}");
                return false;
            }
        }
        private static string RequestPost(string url, string data)
        {
            string text = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                StringContent c = new StringContent((string)(object)data, (Encoding)(object)data, "application/json");
                Task<string> task = Task.Run(() => PostURI(new Uri(url), c));
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Request Post");
                return "";
            }
        }
        private static async Task<string> PostURI(Uri u, HttpContent c)
        {
            string response = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(u, c);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        public static string RequestGet(string url)
        {
            string text = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Task<string> task = Task.Run(() => GetURI(new Uri(url)));
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Request get");
                return "";
            }
        }
        private static async Task<string> GetURI(Uri u)
        {
            string response = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Drawing;
using System.IO;
using System.Threading;
using Facebook_Tool_Request.Helpers;
using System.Text.RegularExpressions;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Facebook_Tool_Request.Common
{
    internal class WebCaptcha
    {
        public static Bitmap GetElementScreenShot(ChromeDriver driver, IWebElement element)
        {
            Screenshot sc = ((ITakesScreenshot)driver).GetScreenshot();
            var img = Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            return img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat);
        }

        string BitmapToBase64(Bitmap image)
        {
            using (var ms = new MemoryStream())
            {
                using (var bitmap = new Bitmap(image))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                }
            }
        }

        public static string createTaskOmocaptcha(string ApiKey, string base64)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://omocaptcha.com/api/createJob");
            request.Content = new StringContent("{\"api_token\": \"" + ApiKey + "\",\"data\": {\"type_job_id\": \"30\",\"image_base64\": \"" + base64 + "\"}}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(responseBody);
            string error = json["error"].ToString();
            if (error == "true")
            {
                return "";
            }
            string job_id = json["job_id"].ToString();
            return job_id;
        }

        public static string getPhoneSimFast(string apiKey = "f74b53b3edc6f37e2d823b2f27bc744e", string appId = "1")
        {
            string phone = "";
            string id = "";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://fastsim.online/api/sim/buy?key="+apiKey+"&app=" + appId);
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            JObject json = JObject.Parse(responseBody);
            string success = json["success"].ToString();
            if (success == "True")
            {
                id = json["data"]["id"].ToString();
                phone = json["data"]["number"].ToString();
            }

            return id + "|" + phone;
        }

        public static string getSmsOtpFastSim(string idOrder, string apiKey = "f74b53b3edc6f37e2d823b2f27bc744e")
        {
            string otp = null;
            string status = "";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://fastsim.online/api/sim/check?key=" + apiKey + "&id=" + idOrder);
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            JObject json = JObject.Parse(responseBody);
            string success = json["success"].ToString();
            if (success == "True")
            {
                status = json["data"]["status"].ToString();

                if(status == "1")
                {
                    otp = json["data"]["code"].ToString();
                }
            }

            return otp;
        }

        public static string getResultOmocaptcha(string ApiKey, string jobId)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://omocaptcha.com/api/getJobResult");
            request.Content = new StringContent("{\"api_token\": \"" + ApiKey + "\",\"job_id\": " + jobId + "}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(responseBody);
            string error = json["error"].ToString();
            if (error == "true")
            {
                return "";
            }
            string result = json["result"].ToString();
            return result;
        }

        public static string createTaskNopecha(string ApiKey, string base64)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.nopecha.com/");
            request.Content = new StringContent("{\"key\": \"" + ApiKey + "\", \"type\": \"textcaptcha\", \"image_data\": [\"" + base64 + "\"]}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            string data = Regex.Match(responseBody, "\"data\":\"(.*?)\"").Groups[1].Value;
            bool checknull = string.IsNullOrEmpty(data);
            if (checknull)
            {
                return "";
            }
            return data;
        }
        public static string getResultNopecha(string ApiKey, string jobId)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.nopecha.com?key=" + ApiKey + "&id=" + jobId);
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            string captcha = Regex.Match(responseBody, "\"data\":[[]\"(.*?)\"[]]").Groups[1].Value;

            return captcha;
        }

        public static string checkAmountOmocaptcha(string ApiKey)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://omocaptcha.com/api/getBalance");
            request.Content = new StringContent("{\"api_token\": \"" + ApiKey + "\"}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(responseBody);
            string error = json["error"].ToString();
            if (error == "true")
            {
                return "error|Hết tiền hoặc key sai!";
            }
            string balance = json["balance"].ToString();

            return "success|" + balance;
        }
    }
}

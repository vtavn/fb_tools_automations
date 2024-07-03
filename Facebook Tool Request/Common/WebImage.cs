using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Facebook_Tool_Request.Common
{
    public class WebImage
    {
        public RestClient Client { set; get; }
        private RestClientOptions options;
        private RestRequest request;
        public string datapost { set; get; }
        public string ResponseURI { set; get; }

        public WebImage()
        {
            datapost = "";
            Client = new RestClient();
        }

        public Task<string> Get(string url, string filename, string referer = null)
        {
            var restk = Task.Run(async () =>
            {
                request = new RestRequest(url);
                request.AddHeader("accept", "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8");
                request.AddHeader("sec-fetch-site", "same-site");
                request.AddHeader("sec-fetch-dest", "image");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "no-cors");
                if(!string.IsNullOrEmpty(referer)) request.AddHeader("referer", referer);
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.Method = Method.Get;
                var res = await Client.ExecuteAsync(request);
                try
                {
                    if (res.ResponseUri != null)
                        ResponseURI = res.ResponseUri.OriginalString;
                }
                catch { }
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    return res?.Content;
                }
                return res?.Content;
            });
            return restk;
        }
        public async Task<string> DownloadImageFormUrl(string url, string uid, string fileName, string referer = null)
        {
            var request = new RestRequest(url);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Host", "api.unrealperson.com");
            request.AddHeader("Origin", "https://www.unrealperson.com");
            request.AddHeader("Referer", "https://www.unrealperson.com/");
            request.Method = Method.Get;

            var res = await Client.ExecuteAsync(request);

            if (res.StatusCode == HttpStatusCode.OK && res.RawBytes != null && res.RawBytes.Length > 0)
            {
                string directoryPath = Path.Combine("backup", uid, "image");
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, $"{fileName}");

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                    {
                        await stream.WriteAsync(res.RawBytes, 0, res.RawBytes.Length);
                    }

                    return filePath;
                }
                catch
                {
                    return "";
                }
            }

            return "";
        }

        public static void ChangeMD5OfFile(string filePath)
        {
            Random random = new Random();
            int num = random.Next(2, 7);
            byte[] extraByte = new byte[num];
            for (int j = 0; j < num; j++)
            {
                extraByte[j] = (byte)0;
            }
            long fileSize = new FileInfo(filePath).Length;


            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
            {
                fileStream.Write(extraByte, 0, extraByte.Length);
            }
            int bufferSize = fileSize > 1048576L ? 1048576 : 4096;
            using (MD5 md = MD5.Create())
            {
                using (FileStream fileStream2 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
                {
                    BitConverter.ToString(md.ComputeHash(fileStream2)).Replace("-", "");
                }
            }
        }

        public static async Task<string> dowloadThispersondoesnotexist(string uid, string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("image/jpeg"));
                client.MaxResponseContentBufferSize = 256000000;
                client.DefaultRequestHeaders.ConnectionClose = true;
                string requestUrl = @"https://thispersondoesnotexist.com";

                string directoryPath = Path.Combine("backup", uid, "image");
                Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, $"{fileName}.jpg");

                try
                {
                    using (Stream streamFromServer = await client.GetStreamAsync(requestUrl))
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            streamFromServer.CopyTo(fs);
                        }
                    }
                    return Path.GetFullPath(filePath);
                }
                catch (Exception e)
                {
                    return string.Empty;
                }
            }
        }

        public static async Task<string> downloadUnrealperson(string uid)
        {
            var options = new RestClientOptions("https://api.unrealperson.com")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/person", Method.Get);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Host", "api.unrealperson.com");
            request.AddHeader("Origin", "https://www.unrealperson.com");
            request.AddHeader("Referer", "https://www.unrealperson.com/");
            RestResponse response = await client.ExecuteAsync(request);
            JObject jObject = JObject.Parse(response.Content);
            string imgUrl = "";
            string fileImg = "";
            if (jObject["code"].ToString() == "200")
            {
                imgUrl = jObject["image_url"].ToString();
                string directoryPath = Path.Combine("backup", uid, "image");
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, $"{imgUrl}");

                string urlDown = "/image?name=" + imgUrl + "&type=tpdne";
                var imageRequest = new RestRequest(urlDown, Method.Get);
                imageRequest.AddHeader("Accept", "application/json, text/plain, */*");
                imageRequest.AddHeader("Host", "api.unrealperson.com");
                imageRequest.AddHeader("Origin", "https://www.unrealperson.com");
                imageRequest.AddHeader("Referer", "https://www.unrealperson.com/");
                var imageResponse = await client.ExecuteAsync(imageRequest);

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await stream.WriteAsync(imageResponse.RawBytes, 0, imageResponse.RawBytes.Length);
                }
                fileImg = filePath;
            }
            else
            {
                fileImg = "";
            }

            return Path.GetFullPath(fileImg); ;
        }


    }
}

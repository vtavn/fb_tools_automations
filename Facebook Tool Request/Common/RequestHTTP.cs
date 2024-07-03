using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Common
{
    public class RequestHTTP
    {
        private RestClient Client;
        private RestRequest request;
        private RestClientOptions options;

        public string UserAgent { set; get; }
        public string Cookie { set; get; }
        public string ProxyUrl { set; get; }
        public RequestHTTP(string cookie = "", string proxy = "", string useragent = "") 
        { 
        
        }
    }
}

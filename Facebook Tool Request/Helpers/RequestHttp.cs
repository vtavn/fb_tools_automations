using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HttpRequest;

namespace Facebook_Tool_Request.Helpers
{
    public class RequestHttp
    {
        public RequestHttp(string cookie = "", string userAgent = "", string proxy = "", int typeProxy = 0)
        {
            bool flag = userAgent == "";
            if (flag)
            {
                this.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";
            }
            else
            {
                this.UserAgent = userAgent;
            }
            this.request = new RequestHTTP();
            this.request.SetSSL(SecurityProtocolType.Tls12);
            this.request.SetKeepAlive(true);
            this.request.SetDefaultHeaders(new string[]
            {
                "content-type: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                "user-agent: " + this.UserAgent
            });
            bool flag2 = cookie != "";
            if (flag2)
            {
                this.AddCookie(cookie);
            }
            this.Proxy = proxy;
        }

        public string RequestGet(string url)
        {
            bool flag2 = this.Proxy != "";
            string result;
            if (flag2)
            {
                bool flag3 = this.Proxy.Contains(":");
                if (flag3)
                {
                    result = this.request.Request("GET", url, null, null, true, new WebProxy(this.Proxy.Split(new char[]
                    {
                        ':'
                    })[0], Convert.ToInt32(this.Proxy.Split(new char[]
                    {
                        ':'
                    })[1])), 60000).ToString();
                }
                else
                {
                    result = this.request.Request("GET", url, null, null, true, new WebProxy("127.0.0.1", Convert.ToInt32(this.Proxy)), 60000).ToString();
                }
            }
            else
            {
                result = this.request.Request("GET", url, null, null, true, null, 60000).ToString();
            }
            return result;
        }

        public string RequestPost(string url, string data = "")
        {
            bool flag = this.Proxy != "";
            string result;
            if (flag)
            {
                bool flag2 = this.Proxy.Contains(":");
                if (flag2)
                {
                    result = this.request.Request("POST", url, null, Encoding.ASCII.GetBytes(data), true, new WebProxy(this.Proxy.Split(new char[]
                    {
                        ':'
                    })[0], Convert.ToInt32(this.Proxy.Split(new char[]
                    {
                        ':'
                    })[1])), 60000).ToString();
                }
                else
                {
                    result = this.request.Request("POST", url, null, Encoding.ASCII.GetBytes(data), true, new WebProxy("127.0.0.1", Convert.ToInt32(this.Proxy)), 60000).ToString();
                }
            }
            else
            {
                result = this.request.Request("POST", url, null, Encoding.ASCII.GetBytes(data), true, null, 60000).ToString();
            }
            return result;
        }

        public void AddCookie(string cookie)
        {
            string[] array = cookie.Split(new char[]
            {
                ';'
            });
            string text = "";
            foreach (string text2 in array)
            {
                string[] array3 = text2.Split(new char[]
                {
                    '='
                });
                bool flag = array3.Count<string>() > 1;
                if (flag)
                {
                    try
                    {
                        text = string.Concat(new string[]
                        {
                            text,
                            array3[0],
                            "=",
                            array3[1],
                            ";"
                        });
                    }
                    catch
                    {
                    }
                }
            }
            this.request.SetDefaultHeaders(new string[]
            {
                "content-type: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8;charset=UTF-8",
                "user-agent: " + this.UserAgent,
                "cookie: " + text
            });
        }

        public string GetCookie()
        {
            return this.request.GetCookiesString();
        }

        public RequestHTTP request;

        private string UserAgent;

        private string Proxy;
    }
}

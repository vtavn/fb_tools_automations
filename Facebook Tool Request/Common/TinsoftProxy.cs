using Facebook_Tool_Request.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Common
{
    internal class TinsoftProxy
    {
        public string api_key { get; set; }

        public string proxy { get; set; }

        public string ip { get; set; }

        public int port { get; set; }

        public int timeout { get; set; }

        public int next_change { get; set; }

        public int location { get; set; }

        private object k1 = new object();

        private object k = new object();

        public string errorCode = "";

        private string svUrl = "http://proxy.tinsoftsv.com";

        private int lastRequest = 0;

        public bool canChangeIP = true;

        public int dangSuDung = 0;

        public int daSuDung = 0;

        public int limit_theads_use = 3;

        public TinsoftProxy(string api_key, int limit_theads_use, int location = 0)
        {
            this.api_key = api_key;
            this.proxy = "";
            this.ip = "";
            this.port = 0;
            this.timeout = 0;
            this.next_change = 0;
            this.location = location;
            this.limit_theads_use = limit_theads_use;
            this.dangSuDung = 0;
            this.daSuDung = 0;
            this.canChangeIP = true;
        }

        public string TryToGetMyIP()
        {
            object obj = this.k1;
            string result;
            lock (obj)
            {
                bool flag2 = this.dangSuDung == 0;
                if (flag2)
                {
                    bool flag3 = this.daSuDung > 0 && this.daSuDung < this.limit_theads_use;
                    if (flag3)
                    {
                        bool flag4 = this.GetTimeOut() < 30;
                        if (flag4)
                        {
                            bool flag5 = this.ChangeProxy();
                            if (!flag5)
                            {
                                return "0";
                            }
                        }
                    }
                    else
                    {
                        bool flag6 = this.ChangeProxy();
                        if (!flag6)
                        {
                            return "0";
                        }
                    }
                }
                else
                {
                    bool flag7 = this.daSuDung < this.limit_theads_use;
                    if (!flag7)
                    {
                        return "2";
                    }
                    bool flag8 = this.GetTimeOut() < 30;
                    if (flag8)
                    {
                        bool flag9 = this.ChangeProxy();
                        if (!flag9)
                        {
                            return "0";
                        }
                    }
                }
                this.daSuDung++;
                this.dangSuDung++;
                result = "1";
            }
            return result;
        }

        public void DecrementDangSuDung()
        {
            object obj = this.k;
            lock (obj)
            {
                this.dangSuDung--;
                bool flag2 = this.dangSuDung == 0 && this.daSuDung == this.limit_theads_use;
                if (flag2)
                {
                    this.daSuDung = 0;
                }
            }
        }

        public bool ChangeProxy()
        {
            object obj = this.k;
            bool result;
            lock (obj)
            {
                bool flag2 = this.checkLastRequest();
                if (flag2)
                {
                    this.errorCode = "";
                    this.next_change = 0;
                    this.proxy = "";
                    this.ip = "";
                    this.port = 0;
                    this.timeout = 0;
                    string svcontent = this.getSVContent(string.Concat(new object[]
                    {
                        this.svUrl,
                        "/api/changeProxy.php?key=",
                        this.api_key,
                        "&location=",
                        this.location
                    }));
                    bool flag3 = svcontent != "";
                    if (flag3)
                    {
                        try
                        {
                            JObject jobject = JObject.Parse(svcontent);
                            bool flag4 = bool.Parse(jobject["success"].ToString());
                            if (flag4)
                            {
                                this.proxy = jobject["proxy"].ToString();
                                string[] array = this.proxy.Split(new char[]
                                {
                                    ':'
                                });
                                this.ip = array[0];
                                this.port = int.Parse(array[1]);
                                this.timeout = int.Parse(jobject["timeout"].ToString());
                                this.next_change = int.Parse(jobject["next_change"].ToString());
                                this.errorCode = "";
                                return true;
                            }
                            this.errorCode = jobject["description"].ToString();
                            this.next_change = int.Parse(jobject["next_change"].ToString());
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        this.errorCode = "request server timeout!";
                    }
                }
                else
                {
                    this.errorCode = "Request so fast!";
                }
                result = false;
            }
            return result;
        }

        public string GetProxy()
        {
            bool flag;
            do
            {
                flag = this.CheckStatusProxy();
            }
            while (!flag);
            return this.proxy;
        }

        public int GetTimeOut()
        {
            bool flag;
            do
            {
                flag = this.CheckStatusProxy();
            }
            while (!flag);
            return this.timeout;
        }

        public int GetNextChange()
        {
            bool flag;
            do
            {
                flag = this.CheckStatusProxy();
            }
            while (!flag);
            return this.next_change;
        }

        public bool CheckStatusProxy()
        {
            object obj = this.k;
            bool result;
            lock (obj)
            {
                this.errorCode = "";
                this.next_change = 0;
                this.proxy = "";
                this.ip = "";
                this.port = 0;
                this.timeout = 0;
                string svcontent = this.getSVContent(string.Concat(new object[]
                {
                    this.svUrl,
                    "/api/getProxy.php?key=",
                    this.api_key
                }));
                bool flag2 = svcontent != "";
                if (flag2)
                {
                    try
                    {
                        JObject jobject = JObject.Parse(svcontent);
                        bool flag3 = bool.Parse(jobject["success"].ToString());
                        if (flag3)
                        {
                            this.proxy = jobject["proxy"].ToString();
                            string[] array = this.proxy.Split(new char[]
                            {
                                ':'
                            });
                            this.ip = array[0];
                            this.port = int.Parse(array[1]);
                            this.timeout = int.Parse(jobject["timeout"].ToString());
                            this.next_change = int.Parse(jobject["next_change"].ToString());
                            this.errorCode = "";
                            return true;
                        }
                        this.errorCode = jobject["description"].ToString();
                        bool flag4 = jobject["next_change"] != null;
                        if (flag4)
                        {
                            this.next_change = int.Parse(jobject["next_change"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    this.errorCode = "request server timeout!";
                }
                result = false;
            }
            return result;
        }

        private bool checkLastRequest()
        {
            try
            {
                DateTime dateTime = new DateTime(2001, 1, 1);
                long ticks = DateTime.Now.Ticks - dateTime.Ticks;
                TimeSpan timeSpan = new TimeSpan(ticks);
                int num = (int)timeSpan.TotalSeconds;
                bool flag = num - this.lastRequest >= 10;
                if (flag)
                {
                    this.lastRequest = num;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private string getSVContent(string url)
        {
            Console.WriteLine(url);
            string text = "";
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    text = webClient.DownloadString(url);
                }
                bool flag = string.IsNullOrEmpty(text);
                if (flag)
                {
                    text = "";
                }
            }
            catch
            {
                text = "";
            }
            return text;
        }

        private static string GetSVContent(string url)
        {
            Console.WriteLine(url);
            string text = "";
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    text = webClient.DownloadString(url);
                }
                bool flag = string.IsNullOrEmpty(text);
                if (flag)
                {
                    text = "";
                }
            }
            catch
            {
                text = "";
            }
            return text;
        }

        public static bool CheckApiProxy(string apiProxy)
        {
            string svcontent = TinsoftProxy.GetSVContent("http://proxy.tinsoftsv.com/api/getKeyInfo.php?key=" + apiProxy);
            bool flag = svcontent != "";
            if (flag)
            {
                JObject jobject = JObject.Parse(svcontent);
                bool flag2 = bool.Parse(jobject["success"].ToString());
                if (flag2)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetListKey(string api_user)
        {
            List<string> list = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)", "", 0);
                string json = requestXNet.RequestGet("http://proxy.tinsoftsv.com/api/getUserKeys.php?key=" + api_user);
                JObject jobject = JObject.Parse(json);
                foreach (JToken jtoken in ((IEnumerable<JToken>)jobject["data"]))
                {
                    bool flag = Convert.ToBoolean(jtoken["success"].ToString());
                    if (flag)
                    {
                        list.Add(jtoken["key"].ToString());
                    }
                }
            }
            catch
            {
            }
            return list;
        }
    }
}

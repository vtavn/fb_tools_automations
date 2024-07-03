using HttpRequest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace cuakit
{
    internal class Helpers
    {
        public static Random rd = new Random();

        public static string GetIdPost(string Link)
        {
            //RequestHTTP requestHTTP = new RequestHTTP();
            //requestHTTP.SetSSL(SecurityProtocolType.Tls12);
            //requestHTTP.SetKeepAlive(k: true);
            //requestHTTP.SetDefaultHeaders(new string[2] { "content-type: application/x-www-form-urlencoded", "user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36" });
            //string input = requestHTTP.Request("GET", Link);

            Meta request = new Meta();
            string input = request.Get(Link).Result;

            string value = Regex.Match(input, "\"post_id\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
            if (value == "")
            {
                value = Regex.Match(input, "\"share_fbid\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
            }else
            if (value == "")
            {
                value = Regex.Match(input, "videos/(.*?)/", RegexOptions.Singleline).Groups[1].Value;
            }
            else
            if (value == "")
            {
                value = Regex.Match(input, "\"userID\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
            }

            return value;
        }
        public static List<string> GetRandomIDs(List<string> ids, int count)
        {
            if (count > ids.Count) count = 1;

            List<string> selectedIDs = new List<string>();

            while (selectedIDs.Count < count)
            {
                int randomIndex = rd.Next(ids.Count); 
                string randomID = ids[randomIndex];
                if (!selectedIDs.Contains(randomID))
                {
                    selectedIDs.Add(randomID);
                }
            }
            return selectedIDs;
        }

        public static string checkLoginAndCheckpoint(string url)
        {
            string rs = "";
            if (url.Contains("login/identify"))
            {
                rs = "Sai Mật Khẩu.";
            }
            else if (url.Contains("828281030927956"))
            {
                rs = "CP 956.";
            }
            else if (url.Contains("1501092823525282"))
            {
                rs = "CP 282.";
            }
            return rs;
        }
        public static string loginFbRequest(string uid, string pass, string tfa, string userAgent = null, string proxy = null)
        {
            string cookie = null;
            string rs = "0|Lỗi";
            Meta request = new Meta(cookie, userAgent, proxy);
            try
            {
                while (true)
                {
                    string loadHome = request.Get("https://www.facebook.com/").Result;
                    string actionLogin = "https://www.facebook.com" + Regex.Match(loadHome, "action=\"(.*?)\"").Groups[1].Value.Trim();
                    string Jazoest = Regex.Match(loadHome.Replace("\\", string.Empty), "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value.Trim();
                    string LSD = Regex.Match(loadHome.Replace("\\", string.Empty), "LSD\",(.*?),{\"token\":\"(.*?)\"").Groups[2].Value;
                    string loginPost = request.PostLogin(actionLogin, "jazoest=" + Jazoest + "&lsd=" + LSD + "&login_source=comet_headerless_login&next=&email=" + uid + "&pass=" + pass).Result;
                    string checkUrl = cuakit.Helpers.checkLoginAndCheckpoint(request.ResponseURI);
                    if (!string.IsNullOrEmpty(checkUrl))
                    {
                        rs = "0|" + checkUrl;
                        break;
                    }

                    if (loginPost.Contains("name=\"approvals_code\"") && string.IsNullOrEmpty(checkUrl))
                    {
                        string nh = Regex.Match(loginPost, "name=\"nh\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        string jazoest = Regex.Match(loginPost, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        string fb_dtsg = Regex.Match(loginPost, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        if (string.IsNullOrEmpty(tfa))
                        {
                            //không có 2fa 
                            rs = "0|Không có 2FA";
                            break;
                        }
                        //noti & get code 2fa
                        TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                        string code = getcode.GetCode(tfa);

                        string loginSubmit = request.PostCheckPoint("https://www.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&no_fido=true&approvals_code={code}&submit%5BContinue%5D=Continue").Result;

                        if (loginSubmit.Contains("data-xui-error"))
                        {
                            rs = "0|Mã 2fa không đúng";
                            break;
                        }

                        //lưu vị trí
                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://www.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&name_action_selected=save_device&submit%5BContinue%5D=Continue").Result;
                        }

                        //save browser
                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://www.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BContinue%5D=Continue").Result;
                        }

                        //day la toi 
                        if (loginSubmit.Contains("name=\"submit[This was me]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://www.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BThis+was+me%5D=This+was+me").Result;
                        }

                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://www.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BContinue%5D=Continue").Result;
                        }

                        string checkUrl2 = cuakit.Helpers.checkLoginAndCheckpoint(request.ResponseURI);

                        if (!string.IsNullOrEmpty(checkUrl2))
                        {
                            rs = "0|" + checkUrl2;
                            break;
                        }

                        loginSubmit = request.Get("https://www.facebook.com/").Result;
                        if (loginSubmit.Contains("personal_user_id\":\"0\""))
                        {
                            rs = "0|Đăng nhập thất bại!";
                            break;
                        }
                        rs = "1|" + request.GetCookie();
                        break;
                    }
                    else
                    {
                        string loginSubmit = request.Get("https://www.facebook.com/").Result;
                        if (loginSubmit.Contains("personal_user_id\":\"0\""))
                        {
                            rs = "0|Đăng nhập thất bại!";
                            break;
                        }
                        rs = "1|" + request.GetCookie();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                rs = "0|Error App";
            }

            return rs;
        }
        public static string UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            dateTime = dateTime.AddSeconds(unixTimeStamp);

            string formattedDateTime = dateTime.ToString("dd/MM/yyyy HH:mm:ss");

            return formattedDateTime;
        }
        public static int GetNowTimeStamp()
        {
            DateTime dateTime = DateTime.Now;
            double unixTimeStamp = dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            int intTimeStamp = (int)unixTimeStamp;
            return intTimeStamp;
        }
        public static int checkExpired(double timestamp1, double timestamp2)
        {
            DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime1 = dateTime1.AddSeconds(timestamp1);
            DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime2 = dateTime2.AddSeconds(timestamp2);
            if (dateTime1 > dateTime2)
            {
                return 1;
            }
            else if (dateTime1 == dateTime2)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        //email helper
        public static string getOtpHotmailApi(string email, string password)
        {
            string result = "";
            string apiGet = $"https://api.cua.monster/hotmail/read.php?mail={email}&pass={password}";
            for (int i = 0; i < 30; i++)
            {
                string text = "";
                Thread.Sleep(1);
                WebRequest webRequest = WebRequest.Create(apiGet);
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        text = streamReader.ReadToEnd();
                    }
                }
                bool flag = text.Contains("code_return");
                if (flag)
                {
                    try
                    {
                        result = Regex.Match(text, "\"code_return\":\"(\\d+)\"").Groups[1].Value.ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
                Thread.Sleep(1000);
            }
            return result;
        }

        public static string checkApiIronsim(string key)
        {
            string result = "";
            string apiGet = $"https://ironsim.com/api/user/balance?token={key}";
            string text = "";
            Thread.Sleep(1);
            WebRequest webRequest = WebRequest.Create(apiGet);
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    text = streamReader.ReadToEnd();
                }
            }
            JObject jObject = JObject.Parse(text);
            if (jObject["status_code"].ToString() == "200")
            {
                result = $"Key Hợp lệ! \n Số dư: {jObject["data"]["balance"].ToString()} vnđ.";
            }
            else
            {
                result = "Vui lòng kiểm tra lại key!";
            }

            return result;
        }

        //mail inboxes
        public static string getCodeMailInboxes(string email, string proxy = null)
        {
            string code = "";
            try
            {
                Meta request = new Meta(null, null, proxy);
                string regUser = request.PostInboxes("https://inboxes.com/api/v2/signup").Result;
                string userid = Regex.Match(regUser, "\"user_id\":\"(.*?)\"").Groups[1].Value.Trim();
                string token = Regex.Match(regUser, "\"token\":\"(.*?)\"").Groups[1].Value.Trim();
                string rftoken = Regex.Match(regUser, "\"refresh_token\":\"(.*?)\"").Groups[1].Value.Trim();
                string mailId = "";
                int countGet = 0;
                goto getMail;

            getMail:
                if (countGet >= 30)
                {
                    goto quit;
                }
                Thread.Sleep(2000);
                string getAllMail = request.GetInboxes("https://inboxes.com/api/v2/inbox/" + email, userid, token).Result;
                var responseDataObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(getAllMail);
                foreach (var message in responseDataObject["msgs"])
                {
                    string f = message["f"];
                    string s = message["s"];
                    string ph = message["ph"];

                    if (f.Contains("Microsoft") || s.Contains("Microsoft") || ph.Contains("Microsoft"))
                    {
                        mailId = message["uid"];
                        goto getCode;
                    }
                }

                if (string.IsNullOrEmpty(mailId))
                {
                    countGet++;
                    goto getMail;
                }
            getCode:
                Thread.Sleep(1000);
                string getBodyMail = request.GetInboxes("https://inboxes.com/api/v2/message/" + mailId, userid, token).Result;
                code = Regex.Match(getBodyMail, "Security code: (\\d+)").Groups[1].Value.Trim();
                if (string.IsNullOrEmpty(code))
                {
                    goto getMail;
                }
                return code;

            quit:
                code = "";
            }
            catch
            {
                code = "";
            }
            return code;
        }
    }
}

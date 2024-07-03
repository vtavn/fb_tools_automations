using RestSharp;
using cuakit;
using CuaPackage;
using Facebook_Tool_Request.core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using xNet;

namespace Facebook_Tool_Request.Helpers
{
    internal class CommonRequest
    {

        public static string CheckInfoUsingUid(string uid)
        {
            //RequestHttp requestHttp = new RequestHttp("", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0");
            //string text = "";
            //try
            //{
            //    string text2 = "";
            //    string text3 = "";
            //    string text4 = "";
            //    string text5 = "";
            //    string text6 = "";
            //    string text7 = "";
            //    string text8 = "";
            //    text = requestHttp.RequestPost("https://www.facebook.com/api/graphql/", "q=user(" + uid + "){friends{count},groups{count},id,name,gender}&access_token=6628568379|c1e620fa708a1d5696fb991c1bde5662");
            //    if (!string.IsNullOrEmpty(text))
            //    {
            //        JObject jObject = JObject.Parse(text);
            //        if (string.IsNullOrEmpty(jObject[uid].ToString()))
            //        {
            //            return "0|";
            //        }
            //        if (jObject[uid]["name"] != null)
            //        {
            //            if (jObject[uid]["friends"]["count"] != null)
            //            {
            //                text2 = jObject[uid]["friends"]["count"].ToString();
            //            }
            //            if (jObject[uid]["groups"]["count"] != null)
            //            {
            //                text3 = jObject[uid]["groups"]["count"].ToString();
            //            }
            //            if (jObject[uid]["name"] != null)
            //            {
            //                text4 = jObject[uid]["name"].ToString();
            //            }
            //            if (jObject[uid]["gender"] != null)
            //            {
            //                text5 = jObject[uid]["gender"].ToString();
            //            }
            //            if (jObject[uid]["username"] != null)
            //            {
            //                text6 = jObject[uid]["username"].ToString();
            //            }
            //            if (jObject[uid]["birthday"] != null)
            //            {
            //                text7 = jObject[uid]["birthday"].ToString();
            //            }
            //            if (jObject[uid]["email_addresses"].ToString() != "[]")
            //            {
            //                text8 = jObject[uid]["email_addresses"].ToString();
            //            }
            //            return "1|" + text6 + "|" + text4 + "|" + text5 + "|" + text7 + "|" + text2 + "|" + text3 + "|" + text8;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //}
            return "0|";
        }
        public static string CheckLiveWall(string uid)
        {
            RequestXNet requestXNet = new RequestXNet("", SetupFolder.GetUseragentIPhone(Base.rd), "", 0);
            string text = "";
            try
            {
                text = requestXNet.RequestGet("https://graph.facebook.com/" + uid + "/picture?redirect=false");
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Contains("height") && text.Contains("width"))
                    {
                        return "1|";
                    }
                    return "0|";
                }
            }
            catch
            {
            }
            return "2|";
        }
        public static string getTokenEAAB(string cookie, string useragent, string proxy, int typeProxy = 0)
        {
            string text = "";
            //try
            //{
                string value = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
                RequestXNet requestXNet = new RequestXNet(cookie, useragent, proxy, typeProxy, false);
                string url = "https://adsmanager.facebook.com/adsmanager/";
                string input = requestXNet.RequestGet(url);
                text = Regex.Match(input, "EAAB(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
            //}
            //catch
            //{
            //    if (!CheckLiveCookie(cookie, useragent, proxy, typeProxy).StartsWith("1|"))
            //    {
            //        return "-1";
            //    }
            //}
            //if (text == "" && !CheckLiveCookie(cookie, useragent, proxy, typeProxy).StartsWith("1|"))
            //{
            //    return "-1";
            //}
            return text;
        }
        public static string getTokenEAAG(string cookie, string tfa, string userAgent, string proxy, int typeProxy = 0)
        {
            try
            {
                if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|")) return "";
                string token = "";
                Meta request = new Meta(cookie, userAgent, proxy);
                if (!string.IsNullOrEmpty(tfa))
                {
                    TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                    string code = getcode.GetCode(tfa);
                    string checkAuth = request.Post("https://business.facebook.com/security/twofactor/reauth/enter/", "&approvals_code=" + code + "&save_device=false&hash").Result;
                    if (!checkAuth.Contains("\"codeConfirmed\":true")) return "";
                }
                string getHtml = request.Get("https://business.facebook.com/content_management/").Result;
                var res = Regex.Matches(getHtml, "\"accessToken\":\"(.*?)\"");
                foreach (Match node in res)
                {
                    token = node.Groups[1].Value;
                    if (!string.IsNullOrEmpty(token)) break;
                }
                return token;
            }catch {
                return "";
            }
        }

        public static string GetTokenEAAGNo2Fa(string cookie, string userAgent, string proxy, int typeProxy)
        {
            string text = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "", proxy, typeProxy);
                string input = requestXNet.RequestGet("https://business.facebook.com/content_management/");
                text = Regex.Match(input, "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
            }
            catch
            {
                if (!CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                {
                    return "-1";
                }
            }
            if (text == "" && !CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
            {
                return "-1";
            }
            return text;
        }
        public static string CheckLiveCookie(string cookie, string userAgent, string proxy, int typeProxy)
        {
            string result = "0|0";
            string value = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
            try
            {
                Meta request = new Meta(cookie, userAgent, proxy);
                if (value != "")
                {
                    string text = request.Get("https://www.facebook.com/me").Result;

                    if (Regex.Match(text, "\"USER_ID\":\"(.*?)\"").Groups[1].Value.Trim() == value.Trim() && Regex.Match(text, "\"personal_user_id\":\"(.*?)\"").Groups[1].Value.Trim() == value.Trim())
                    {
                        result = "1|1";
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public static bool CheckLiveToken(string cookie, string token, string useragent, string proxy, int typeProxy = 0)
        {
            bool result = false;
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, useragent, proxy, typeProxy);
                string text = requestXNet.RequestGet("https://graph.facebook.com/me?access_token=" + token);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public static string GraphFacebook(string cookie, string token, string useragent, string proxy, int typeProxy = 0)
        {
            RequestXNet requestXNet = new RequestXNet(cookie, useragent, proxy, typeProxy);
            string rs = "";
            try
            {
                rs = requestXNet.RequestGet("https://graph.facebook.com/me?access_token=" + token);
                if (!string.IsNullOrEmpty(rs))
                {
                    return rs;
                }
            }
            catch
            {
                rs = "Error";
            }
            return rs;
        }
        public static string getInfoByUid(string uid)
        {
            RequestHttp requestHttp = new RequestHttp("", "");
            string rs = "";
            string name = "none";
            try
            {
                rs = requestHttp.RequestPost("https://www.facebook.com/api/graphql", "q=node(" + uid + "){name}");
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (string.IsNullOrEmpty(jObject[uid].ToString()))
                    {
                        return "none";
                    }
                    if (jObject[uid]["name"] != null)
                    {
                        name = jObject[uid]["name"].ToString();
                        return name;
                    }
                }
            }
            catch
            {

            }
            return "";
        }
        public static string getTokenPage(string token, string cookie, string proxy, int typeProxy)
        {
            string tokenPage = "";
            string rs = "";
            List<string> list = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                rs = requestXNet.RequestGet("https://graph.facebook.com/me/accounts?access_token=" + token);
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return "";
                    }
                    JToken jPages = jObject["data"];
                    if (jPages != null && jPages.HasValues)
                    {
                        foreach (JToken pageToken in jPages)
                        {
                            string item = pageToken["access_token"].ToString();
                            list.Add(item);
                        }
                        Random random = new Random();
                        int randomIndex = random.Next(0, list.Count);
                        tokenPage = list[randomIndex];
                    }
                }
            }
            catch
            {
            }
            return tokenPage;
        }

        public static List<string> getTokenPageLimit(string token, string cookie, int limit = 1, string proxy = "", int typeProxy = 0)
        {
            string rs = "";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                rs = requestXNet.RequestGet("https://graph.facebook.com/me/accounts?access_token=" + token);
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return list2;
                    }
                    JToken jPages = jObject["data"];
                    if (jPages != null && jPages.HasValues)
                    {
                        foreach (JToken pageToken in jPages)
                        {
                            string item = pageToken["id"].ToString() + "|" + pageToken["access_token"].ToString();
                            list.Add(item);
                        }
                        Random rand = new Random();
                        list2 = list.OrderBy(x => rand.Next()).Take(limit).ToList();
                    }
                }
            }
            catch
            {
            }
            return list2;
        }

        public static string getCountPage(string token, string cookie, string proxy, int typeProxy)
        {
            string countPage = "0";
            string rs = "";
            List<string> list = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                rs = requestXNet.RequestGet("https://graph.facebook.com/me/accounts?access_token=" + token);
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return "";
                    }
                    countPage = jObject["data"].Count().ToString();
                }
            }
            catch
            {
            }
            return countPage;
        }

        public static string getTokenPageAndFindUid(string uidPage, string token, string cookie, string proxy, int typeProxy)
        {
            string tokenPage = "";
            string rs = "";
            List<string> list = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                rs = requestXNet.RequestGet("https://graph.facebook.com/me/accounts?access_token=" + token);
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return "";
                    }
                    JToken jPages = jObject["data"];
                    if (jPages != null && jPages.HasValues)
                    {
                        foreach (JToken item in jPages)
                        {
                            string id = item["id"].ToString();

                            if (id == uidPage)
                            {
                                tokenPage = item["access_token"].ToString();
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return tokenPage;
        }

        public static string getPostNew(string uid, string token, string cookie, string proxy, int typeProxy, int nudPostUid)
        {
            string idPost = null;
            string rs = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                rs = requestXNet.RequestGet($"https://graph.facebook.com/{uid}/posts?fields=id,actions,from&access_token={token}&limit={nudPostUid}");
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);

                    if (jObject.ContainsKey("error"))
                    {
                        return "";
                    }

                    JToken jData = jObject["data"];
                    if (jData != null && jData.HasValues)
                    {
                        foreach (JToken item in jData)
                        {
                            JToken actions = item["actions"];

                            if (actions != null && actions.HasValues)
                            {
                                foreach (JToken action in actions)
                                {
                                    string name = action["name"].ToString();
                                    if (name == "Comment")
                                    {
                                        idPost = item["id"].ToString();
                                        break;
                                    }
                                }
                            }
                            if (idPost != null)
                            {
                                break;
                            }
                        }
                    }

                }
            }
            catch
            {
            }
            return idPost;
        }

        public static bool sendCommentByTokenCookie(string idPost, string content, string image, bool ckbAnh, string token, string cookie, string proxy, int typeProxy)
        {
            string rs = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                if (ckbAnh)
                {
                    rs = requestXNet.RequestGet($"https://graph.facebook.com/{idPost}/comments?message={content}&attachment_url={image}&method=post&access_token={token}");
                }
                else
                {
                    rs = requestXNet.RequestGet($"https://graph.facebook.com/{idPost}/comments?message={content}&method=post&access_token={token}");
                }
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static string UploadImgToServer(string pathImg)
        {
            string apiUpload = "https://api.cua.bio/api/imgur.php";
            string apiUpload2 = "https://api.cua.monster/up.php";
            string apiUpload3 = "https://up.vutienanh.net/imgur.php";

            string urlImg = "https://api.cua.monster/uploads/c5540533f3828969331adaddc500396b.jpg";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    byte[] imageBytes = File.ReadAllBytes(pathImg); 
                    form.Add(new ByteArrayContent(imageBytes), "image", "image.jpg");

                    HttpResponseMessage response = client.PostAsync(apiUpload, form).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc và xử lý phản hồi JSON
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        JObject jObject = JObject.Parse(responseBody);

                        if (jObject["status"].ToString() == "success")
                        {
                            urlImg = jObject["image_url"].ToString();
                        }
                    }
                    else
                    {
                        Helpers.Common.ExportError(null, "Yêu cầu tải lên ảnh không thành công. Mã trạng thái: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, "Yêu cầu tải lên ảnh không thành công. Mã trạng thái: " + ex.Message);
            }

            return urlImg;
        }
        public static bool sendLikeByTokenCookie(string idPost, string token, string cookie, string proxy, int typeProxy)
        {
            string rs = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36", proxy, typeProxy);
                    rs = requestXNet.RequestGet($"https://graph.facebook.com/{idPost}/comments?message=&method=post&access_token={token}");
                if (!string.IsNullOrEmpty(rs))
                {
                    JObject jObject = JObject.Parse(rs);
                    if (jObject.ContainsKey("error"))
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static string checkLicense(string user, string pass, string idthietbi)
        {
            string rs = "";
            string msg = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string data = "user="+ user +"&pass="+ pass +"&idthietbi="+idthietbi;
                rs = requestXNet.RequestPost("https://api.cua.monster/license/check.php", data);
                JObject jObject = JObject.Parse(rs);

                if (jObject["status"].ToString() == "True")
                {
                    msg = "1|" + jObject["msg"].ToString() + "|" + jObject["fullname"].ToString() + "|" + jObject["idthietbi"].ToString() + "|" + jObject["han"].ToString() + "|" + jObject["buy_at"].ToString() + "|" + jObject["admin"].ToString();
                }
                else
                {
                    msg = "0|"+jObject["msg"].ToString();
                }

            }
            catch (Exception ex)
            {
                msg = "0|Lỗi hệ thống. Liên hệ để được hỗ trợ";
            }

            return msg;
        }

        public static bool updateInfoPageByToken(string cookie, string token, string proxy, string userAgent, int typeProxy, string pageUid, string linkAvt, string linkCover, string phoneNumber, string website)
        {
            try
            {
                //đổi ảnh đại diện
                RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                string rqUpdateAvt = requestXNet.RequestPost("https://graph.facebook.com/" + pageUid + "/picture?picture=" + linkAvt + "&access_token=" + token);
                Common.DelayTime(5);
                //up ảnh và lấy id cover
                string rqPostPhoto = requestXNet.RequestPost("https://graph.facebook.com/" + pageUid + "/photos?caption="+ pageUid +" - "+ Common.CreateRandomStringNumber(5) + "&url=" + linkCover + "&access_token=" + token);
                string idCover = "";
                JObject jObCover = JObject.Parse(rqPostPhoto);
                if (!jObCover.ContainsKey("error"))
                {
                    idCover = jObCover["id"].ToString();
                }
                Common.DelayTime(5);
                //đổi info page 
                string phonePage = "+84" + phoneNumber.Substring(1);
                string rqChangeInfo = requestXNet.RequestPost("https://graph.facebook.com/" + pageUid + "?access_token=" + token + "&cover=" + idCover + "&phone=" + phonePage + "&website=" + website);

                return true;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool postNewPost(string cookie, string token, string proxy, string userAgent, int typeProxy, string pageUid, string linkImgPost, string caption, int type)
        {
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                if (type == 1)
                {
                    string rqPostPhoto = requestXNet.RequestPost("https://graph.facebook.com/" + pageUid + "/photos?caption=" + caption + "&url=" + linkImgPost + "&access_token=" + token);
                    JObject jObPostPhoto = JObject.Parse(rqPostPhoto);
                    if (!jObPostPhoto.ContainsKey("error"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string rqPostFeed = requestXNet.RequestPost("https://graph.facebook.com/" + pageUid + "/feed?access_token=" + token + "&message=" + caption);
                    JObject jPostFeed = JObject.Parse(rqPostFeed);
                    if (!jPostFeed.ContainsKey("error"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            catch
            {
                return false;
            }
        }
        public static string getFbdtsg(string cookie, string proxy = "")
        {
            string rs = "";
            string fbDtsg = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "", proxy, 0);
                rs = requestXNet.RequestGet("https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed");
                if (!string.IsNullOrEmpty(rs))
                {
                    fbDtsg = Regex.Match(rs, "fb_dtsg\\\\\" value=\\\\\"(.*?)\\\\\" ").Groups[1].Value;
                }
            }
            catch
            {

            }
            return fbDtsg;
        }

        public static string regPageRequest(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string namePage, string descPage)
        {
            string rs = "";
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    cookie = requestXNet.GetCookie();
                    string category = Helpers.Common.randomPageCategories();
                    string variables = "{\"input\":{\"bio\":\"" + descPage + "\",\"categories\":"+category+",\"creation_source\":\"comet\",\"name\":\"" + namePage + "\",\"page_referrer\":\"launch_point\",\"actor_id\":\"" + uid + "\",\"client_mutation_id\":\"2\"}}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=5903223909690825";
                    rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    JObject jObject = JObject.Parse(rs);
                    string uixd = jObject["data"]["additional_profile_plus_create"]["page"]["id"].ToString();
                    if (jObject["data"]["additional_profile_plus_create"]["page"]["id"].ToString() != null || jObject["data"]["additional_profile_plus_create"]["page"]["id"].ToString() != "")
                    {
                        string newCoookie = loadBeforeRegPage(pageSource, cookie, proxy, userAgent, typeProxy);
                        msg = "1|" + jObject["data"]["additional_profile_plus_create"]["page"]["id"].ToString() + "|" + jObject["data"]["additional_profile_plus_create"]["additional_profile"]["id"].ToString() + "|"+ newCoookie;
                    }
                    else
                    {
                        msg = "0|" + jObject["errors"]["message"].ToString() + " - "+ jObject["errors"]["description"].ToString();
                    }
                }
            }
            catch
            {
                msg = "0|Giới hạn tạo page.";
            }

            return msg;
        }

        public static string loadBeforeRegPage(string pageSource, string cookie, string proxy, string userAgent, int typeProxy)
        {
            string rs = "";
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    cookie = requestXNet.GetCookie();
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={{\"assetsOrder\":[\"LAST_USED_TIME_DESC\"],\"limit\":500,\"routeName\":\"HOME\",\"supportedAssetTypes\":[\"PAGE\"],\"supportedAssetTypesForCurrentRoute\":[\"PAGE\"]}}&doc_id=4052496901540649";
                    rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    JObject jObject = JObject.Parse(rs);

                    string can_create_business = jObject["data"]["viewer"]["can_create_business"].ToString();
                    if (!string.IsNullOrEmpty(can_create_business) && can_create_business == "True")
                    {
                        msg = cookie;
                    }
                    else
                    {
                        msg = "";
                    }
                }
            }
            catch
            {
                msg = "";
            }

            return msg;
        }

        public static bool updateInfoPageRequest(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string uidPage, string txtAddress, string txtZipcode, string txtEmail, string txtPhoneNumber, string txtWebsite)
        {
            bool status = false;
            string address = getLocationRegPage(pageSource, cookie, proxy, userAgent, typeProxy, txtAddress);
            if (!string.IsNullOrEmpty(address))
            {
                cookie = address.Split('|')[2];
                string updateProcess = updateInfoPageRequestProcess(pageSource, cookie, proxy, userAgent, typeProxy, uidPage, address, txtZipcode, txtEmail, txtPhoneNumber, txtWebsite);
                if(updateProcess.ToString() == "True")
                {
                    status = true;
                }
            }
            return status;
;
        }
        public static string getLocationRegPage(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string address)
        {
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"params\":{\"caller\":\"PROFILE_ABOUT\",\"country_filter\":null,\"geocode_fallback\":false,\"integration_strategy\":\"STRING_MATCH\",\"page_category\":[\"CITY\",\"SUBCITY\"],\"provider\":\"HERE_THRIFT\",\"query\":\"" + address + "\",\"radius\":null,\"result_ordering\":\"INTERLEAVE\",\"search_type\":\"CITY_TYPEAHEAD\"},\"max_results\":10,\"photo_width\":50,\"photo_height\":50}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=4186966154761327";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string countAdd = jObject["data"]["city_street_search"]["street_results"]["edges"].Count().ToString();
                    if (countAdd != "0")
                    {
                        msg = jObject["data"]["city_street_search"]["street_results"]["edges"][0]["node"]["city"]["id"].ToString() + "|" + jObject["data"]["city_street_search"]["street_results"]["edges"][0]["node"]["title"].ToString() + "|" + cookie;
                    }
                    else
                    {
                        msg = "103934556308311|Hải Dương|" + cookie;
                    }
                }
            }
            catch
            {
                msg = "103934556308311|Hải Dương|" + cookie;
            }

            return msg;
        }
        public static string updateInfoPageRequestProcess(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string pageId, string address, string zipcode, string email, string phone, string website)
        {
            string msg = "";

            string[] addr = address.Split('|');
            string addrId = addr[0];
            string addrName = addr[1];
            phone = "84" + phone.Substring(1);

            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy, true, true);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"input\":{\"additional_profile_plus_id\":\"" + pageId + "\",\"creation_source\":\"comet\",\"address\":\"" + addrName + "\",\"city_id\":\"" + addrId + "\",\"zipcode\":\"" + zipcode + "\",\"email\":\"" + email + "\",\"phone\":\"" + phone + "\",\"website\":\"" + website + "\",\"hours\":{\"hours_type\":\"ALWAYS_OPEN\"},\"actor_id\":\"" + uid + "\",\"client_mutation_id\":\"6\"}}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=5261494580582015";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data, "application/x-www-form-urlencoded; charset=utf-8");
                    JObject jObject = JObject.Parse(rs);
                    string checkUpdate = jObject["data"]["additional_profile_plus_edit"].Count().ToString();
                    if (checkUpdate != "0")
                    {
                        msg = jObject["extensions"]["is_final"].ToString();
                    }
                    else
                    {
                        msg = "False";
                    }
                }
            }
            catch
            {
                msg = "False";
            }

            return msg;
        }
        public static string uploadImgRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string pathUrl)
        {
            string rs = "";
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy, true, true);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    cookie = requestXNet.GetCookie();

                    xNet.MultipartContent data = new xNet.MultipartContent()
                    {
                        { new FileContent(pathUrl), "file", System.IO.Path.GetFileName(pathUrl)}
                    };

                    rs = requestXNet.UpLoad($"https://www.facebook.com/profile/picture/upload/?photo_source=57&profile_id={uid}&__user={uid}&__a=1&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}", data).Replace("for (;;);", "");
                    JObject jObject = JObject.Parse(rs);
                    string uixd = jObject["payload"]["fbid"].ToString();
                    if (uixd != null || uixd != "")
                    {
                        
                        msg = "1|" + uixd;
                    }
                    else
                    {
                        msg = "0|";
                    }
                }
            }
            catch
            {
                msg = "0";
            }

            return msg;
        }
        public static bool changeAvtCoverRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string uidPage, string idAvt, string idCover)
        {
            bool msg = false;
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"input\":{\"additional_profile_plus_id\":" + uidPage + ",\"creation_source\":\"comet\",\"profile_photo\":{\"existing_photo_id\":" + idAvt + "},\"cover_photo\":{\"existing_cover_photo_id\":" + idCover + ",\"focus\":{\"x\":0.5,\"y\":0.5}},\"actor_id\":" + uid + ",\"client_mutation_id\":\"14\"}}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=5261494580582015";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string updateCheck = jObject["data"]["additional_profile_plus_edit"].Count().ToString();
                    if (updateCheck != "0")
                    {
                        msg = true;
                    }
                    else
                    {
                        msg = false;
                    }
                }
            }
            catch
            {
                msg = false;
            }

            return msg;
        }
        public static string switchToUidRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string idSwitch)
        {
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"profile_id\":\"" + idSwitch + "\"}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&server_timestamps=true&doc_id=4855674131145168";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string uidCheck = jObject["data"]["profile_switcher_comet_login"]["id"].ToString();
                    if (uidCheck == idSwitch)
                    {
                        msg = uidCheck + "|" + cookie;
                    }
                    else
                    {
                        msg = "";
                    }
                }
            }
            catch
            {
                msg = "";
            }

            return msg;
        }
        public static string authInviteUidRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string password, string uidPage)
        {
            string msg = "";
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"input\":{\"password\":\"" + password + "\",\"actor_id\":\"" + uid + "\",\"client_mutation_id\":\"1\"}}";
                    string data = $"av={uidPage}&__user={uidPage}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=5048033918567225";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string checkOK = jObject["data"]["admin_management_mark_reauthed"]["reauth_is_successful"].ToString();
                    if (checkOK == "True")
                    {
                        msg = uidPage + "|" + uid + "|" + cookie;
                    }
                    else
                    {
                        msg = "";
                    }
                }
            }
            catch
            {
                msg = "";
            }
            return msg;
        }
        public static bool inviteUidToPageRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string uidPage, string uidAdminInvite)
        {
            bool msg = false;
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"input\":{\"additional_profile_id\":\"" + uidPage + "\",\"admin_id\":\"" + uidAdminInvite + "\",\"admin_visibility\":\"Unspecified\",\"grant_full_control\":true,\"actor_id\":\"" + uidPage + "\",\"client_mutation_id\":\"2\"}}";
                    string data = $"av={uidPage}&__user={uidPage}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=5399357510181394";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string checkOK = jObject["data"]["profile_plus_core_admin_invite"]["is_invite_sent"].ToString();
                    if (checkOK == "True")
                    {
                        msg = true;
                    }
                    else
                    {
                        msg = false;
                    }
                }
            }
            catch
            {
                msg = false;
            }
            return msg;
        }
       
        public static bool acceptInviteAdminRequestFb(string pageSource, string cookie, string proxy, string userAgent, int typeProxy, string uidPage)
        {
            bool msg = false;
            try
            {
                string fbdtsg = Regex.Match(pageSource, "\"DTSGInitData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value.ToString();
                string jazoest = Regex.Match(pageSource, "&jazoest=(.*?)\"").Groups[1].Value.ToString();
                string lsd = Regex.Match(pageSource, "\"LSD\\\",\\[],{\\\"token\\\":\\\"(.*?)\\\"").Groups[1].Value.ToString();
                string uid = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
                string profileAdminInvite = Regex.Match(pageSource, "\"profile_admin_invite_id\":\"(.*?)\"").Groups[1].Value.ToString();

                if (!string.IsNullOrEmpty(fbdtsg))
                {
                    RequestXNet requestXNet = new RequestXNet(cookie, userAgent, proxy, typeProxy);
                    requestXNet.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-site", "same-origin");
                    requestXNet.request.AddHeader("sec-fetch-dest", "empty");
                    requestXNet.request.AddHeader("accept-language", "en-US,en;q=0.9");
                    requestXNet.request.AddHeader("sec-fetch-mode", "cors");
                    requestXNet.request.AddHeader("referer", "https://www.facebook.com/");
                    requestXNet.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                    requestXNet.request.AddHeader("x-fb-lsd", lsd);
                    string variables = "{\"input\":{\"client_mutation_id\":\"1\",\"actor_id\":\""+ uid + "\",\"is_accept\":true,\"profile_admin_invite_id\":\""+ profileAdminInvite + "\",\"user_id\":\""+ uidPage + "\"},\"scale\":1.5}";
                    string data = $"av={uid}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={fbdtsg}&jazoest={jazoest}&lsd={lsd}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern&variables={variables}&doc_id=8184939334914113";
                    string rs = requestXNet.RequestPost("https://www.facebook.com/api/graphql/", data);
                    cookie = requestXNet.GetCookie();
                    JObject jObject = JObject.Parse(rs);
                    string uidcheckOK = jObject["data"]["accept_or_decline_profile_plus_admin_invite"]["id"].ToString();
                    if (uidcheckOK == uidPage)
                    {
                        msg = true;
                    }
                    else
                    {
                        msg = false;
                    }
                }
            }
            catch
            {
                msg = false;
            }
            return msg;
        }

        public static string getInfoWithUidNoToken(string uid)
        {
            StringBuilder result = new StringBuilder();
            RequestHttp requestHttp = new RequestHttp("", "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_2 like Mac OS X) AppleWebKit/603.2.4 (KHTML, like Gecko) Mobile/14F89");
            try
            {
                string nameRs = requestHttp.RequestPost("https://www.facebook.com/api/graphql", "q=node(" + uid + "){name}");
                JObject nameObject = JObject.Parse(nameRs);
                if (nameObject[uid]?["name"] != null)
                {
                    string name = nameObject[uid]["name"].ToString();
                    result.AppendFormat("1|{0}", name);
                }
                else
                {
                    result.Append("|");
                }

                string friendRs = requestHttp.RequestPost("https://www.facebook.com/api/graphql", "q=node(" + uid + "){friends{count}}");
                JObject friendsObject = JObject.Parse(friendRs);
                if (friendsObject[uid]?["friends"]?["count"] != null)
                {
                    string friends = friendsObject[uid]["friends"]["count"].ToString();
                    result.AppendFormat("|{0}", friends);
                }
                else
                {
                    result.Append("|");
                }
                string createdResponse = requestHttp.RequestPost("https://www.facebook.com/api/graphql", "q=node(" + uid + "){created_time}");

                JObject createdObject = JObject.Parse(createdResponse);
                if (createdObject[uid]?["created_time"] != null)
                {
                    string created = createdObject[uid]["created_time"].ToString();
                    result.AppendFormat("|{0}", Helpers.Common.ConvertTimeStampToDateTime(long.Parse(created)));
                }
                else
                {
                    result.Append("|");
                }
            }
            catch
            {
                result.Append("0|||");
            }
            return result.ToString();
        }
        public static int CheckAvatarFromUid(string uid, string token)
        {
            int result;
            try
            {
                bool flag = token != "";
                if (flag)
                {
                    string text = new RequestXNet("", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36", "", 0).RequestGet("https://graph.facebook.com/" + uid + "/picture?width=73&redirect=false&access_token=" + token);
                    bool flag2 = text.Contains(".gif") || text.Contains("143086968_2856368904622192_1959732218791162458") || text.Contains("84628273_176159830277856_972693363922829312_n");
                    if (flag2)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else
                {
                    List<bool> hash = CommonRequest.GetHash(Base.base64_anhmau.ToBitmap());
                    List<bool> hash2 = CommonRequest.GetHash(GetAvatarFromUid(uid, false));
                    bool flag3 = (double)(hash.Zip(hash2, (bool i, bool j) => i == j).Count((bool eq) => eq) / 256) == 0.0;
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch
            {
                result = 2;
            }
            return result;
        }

        private static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> list = new List<bool>();
            Bitmap bitmap = new Bitmap(bmpSource, new Size(16, 16));
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    list.Add(bitmap.GetPixel(j, i).GetBrightness() < 0.5f);
                }
            }
            return list;
        }
        public static Bitmap GetAvatarFromUid(string uid, bool isSave = false)
        {
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string url = "https://graph.facebook.com/" + uid + "/picture?height=500&access_token=6628568379%7Cc1e620fa708a1d5696fb991c1bde5662";
                byte[] bytes = requestXNet.GetBytes(url);
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));
                Bitmap bitmap = new Bitmap(memoryStream, false);
                if (isSave)
                {
                    string text = FileHelper.GetPathToCurrentFolder() + "\\avatar";
                    Common.CreateFolder(text);
                    bitmap.Save(text + "\\" + uid + ".png");
                }
                return bitmap;
            }
            catch
            {
            }
            return null;
        }

        public static string getAmountDongVanFb(string key)
        {
            string vnd = "error|0";
            try
            {
                RequestXNet requestXNet = new RequestXNet("","","",0);
                string result = requestXNet.RequestGet("https://api.dongvanfb.net/user/balance?apikey=" + key);
                JObject resultObject = JObject.Parse(result);
                if(resultObject["status"].ToString() == "True")
                {
                    vnd = "success|" + resultObject["balance"].ToString();
                }
            }
            catch
            {
            }
            return vnd;
        }

        public static List<string> GetTypeDongVanFb(string key)
        {
            List<string> list = new List<string>();
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string result = requestXNet.RequestGet("https://api.dongvanfb.net/user/account_type?apikey=" + key);
                JObject resultObject = JObject.Parse(result);
                if (resultObject["status"].ToString().ToLower() == "true")
                {
                    JArray dataArray = JArray.Parse(resultObject["data"].ToString());

                    foreach (JObject item in dataArray)
                    {
                        string id = item["id"].ToString();
                        string name = item["name"].ToString();
                        string quality = item["quality"].ToString();
                        string price = item["price"].ToString();
                        string itemString = $"{id}|{name} - Giá: {price} vnđ - Còn {quality}.";
                        //if(quality != "0")
                        //{
                        list.Add(itemString);
                        //}
                    }
                }
                else
                {
                    list.Add("|");
                }
            }
            catch
            {
            }
            return list;
        }
        public static string getMailCuaApi()
        {
            string msg = "error|Lỗi không xác định";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string result = requestXNet.RequestGet("https://api.cua.monster/hotmail/get.php");
                if (!string.IsNullOrEmpty(result))
                {
                    msg = "success|" + result.ToString();
                }
                else
                {
                    msg = "error|" + "Hết mail vui lòng thêm mail mới!";
                }
            }
            catch
            {
            }
            return msg;
        }

        public static string buyEmailDongVanFb(string key, string type)
        {
            string msg = "error|Lỗi không xác định";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string result = requestXNet.RequestGet("https://api.dongvanfb.net/user/buy?apikey=" + key + "&account_type="+ type +"&quality=1&type=null");
               // string result = requestXNet.RequestGet("https://api.cua.monster/test/dv.php");

                JObject resultObject = JObject.Parse(result);
                if (resultObject["status"].ToString() == "True")
                {
                    msg = "success|" + resultObject["data"]["list_data"][0].ToString();
                }
                else
                {
                    msg = "error|" + resultObject["message"].ToString();
                }
            }
            catch
            {
            }
            return msg;
        }
        public static string getOtpDongVanFb(string mail, string pass, string type)
        {
            string msg = "error|Lỗi không xác định";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string result = requestXNet.RequestGet("https://tools.dongvanfb.net/api/get_code?mail=" + mail + "&pass=" + pass + "&type=" + type);

                JObject resultObject = JObject.Parse(result);
                if (resultObject["status"].ToString() == "True")
                {
                    msg = "success|" + resultObject["code"].ToString();
                }
                else
                {
                    msg = "error|Không có OTP";
                }
            }
            catch
            {
            }
            return msg;
        }
        public static string checkLic2(string key, string idthietbi)
        {
            string rs = "";
            string msg = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet("", "", "", 0);
                string data = "key=" + key + "&idthietbi=" + idthietbi;
                rs = requestXNet.RequestPost("https://api.cua.bio/license/cua.php", data);
                JObject jObject = JObject.Parse(rs);

                if (jObject["status"].ToString() == "True")
                {
                    msg = "1|" + jObject["msg"].ToString() + "|" + jObject["han"].ToString() + "|" + jObject["fullname"].ToString() + "|" + jObject["active"].ToString();
                }
                else
                {
                    msg = "0|" + jObject["msg"].ToString();
                }

            }
            catch (Exception ex)
            {
                msg = "0|Lỗi hệ thống. Liên hệ để được hỗ trợ";
            }

            return msg;
        }

        public static string getFbdtsgNew(string cookie, string proxy = "")
        {
            string rs = "";
            string fbDtsg = "";
            try
            {
                RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36", proxy, 0);

                rs = requestXNet.RequestGetNew("https://www.facebook.com/v2.3/dialog/oauth?redirect_uri=fbconnect://success&scope=email,publish_actions,publish_pages,user_about_me,user_actions.books,user_actions.music,user_actions.news,user_actions.video,user_activities,user_birthday,user_education_history,user_events,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_notes,user_photos,user_questions,user_relationship_details,user_relationships,user_religion_politics,user_status,user_subscriptions,user_videos,user_website,user_work_history,friends_about_me,friends_actions.books,friends_actions.music,friends_actions.news,friends_actions.video,friends_activities,friends_birthday,friends_education_history,friends_events,friends_games_activity,friends_groups,friends_hometown,friends_interests,friends_likes,friends_location,friends_notes,friends_photos,friends_questions,friends_relationship_details,friends_relationships,friends_religion_politics,friends_status,friends_subscriptions,friends_videos,friends_website,friends_work_history,ads_management,create_event,create_note,export_stream,friends_online_presence,manage_friendlists,manage_notifications,manage_pages,photo_upload,publish_stream,read_friendlists,read_insights,read_mailbox,read_page_mailboxes,read_requests,read_stream,rsvp_event,share_item,sms,status_update,user_online_presence,video_upload,xmpp_login&response_type=token,code&client_id=356275264482347");
                if (!string.IsNullOrEmpty(rs))
                {
                    fbDtsg = Regex.Match(rs.Replace("[]", ""), "DTSGInitData\",,{\"token\":\"(.+?)\"").Groups[1].Value;
                }
            }
            catch
            {

            }
            return fbDtsg;
        }

        public static string getTokenAppNew(string cookie, string idApp, string fb_dtsg, string proxy = "")
        {
            RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36", proxy, 0);
            string rs = "";
            string token = "";
            try
            {

                rs = requestXNet.RequestPostNew("https://www.facebook.com/dialog/oauth/business/cancel/?app_id="+ idApp + "&version=v12.0&logger_id=&user_scopes[0]=user_birthday&user_scopes[1]=user_religion_politics&user_scopes[2]=user_relationships&user_scopes[3]=user_relationship_details&user_scopes[4]=user_hometown&user_scopes[5]=user_location&user_scopes[6]=user_likes&user_scopes[7]=user_education_history&user_scopes[8]=user_work_history&user_scopes[9]=user_website&user_scopes[10]=user_events&user_scopes[11]=user_photos&user_scopes[12]=user_videos&user_scopes[13]=user_friends&user_scopes[14]=user_about_me&user_scopes[15]=user_posts&user_scopes[16]=email&user_scopes[17]=manage_fundraisers&user_scopes[18]=read_custom_friendlists&user_scopes[19]=read_insights&user_scopes[20]=rsvp_event&user_scopes[21]=xmpp_login&user_scopes[22]=offline_access&user_scopes[23]=publish_video&user_scopes[24]=openid&user_scopes[25]=catalog_management&user_scopes[26]=user_messenger_contact&user_scopes[27]=gaming_user_locale&user_scopes[28]=private_computation_access&user_scopes[29]=instagram_business_basic&user_scopes[30]=user_managed_groups&user_scopes[31]=groups_show_list&user_scopes[32]=pages_manage_cta&user_scopes[33]=pages_manage_instant_articles&user_scopes[34]=pages_show_list&user_scopes[35]=pages_messaging&user_scopes[36]=pages_messaging_phone_number&user_scopes[37]=pages_messaging_subscriptions&user_scopes[38]=read_page_mailboxes&user_scopes[39]=ads_management&user_scopes[40]=ads_read&user_scopes[41]=business_management&user_scopes[42]=instagram_basic&user_scopes[43]=instagram_manage_comments&user_scopes[44]=instagram_manage_insights&user_scopes[45]=instagram_content_publish&user_scopes[46]=publish_to_groups&user_scopes[47]=groups_access_member_info&user_scopes[48]=leads_retrieval&user_scopes[49]=whatsapp_business_management&user_scopes[50]=instagram_manage_messages&user_scopes[51]=attribution_read&user_scopes[52]=page_events&user_scopes[53]=business_creative_transfer&user_scopes[54]=pages_read_engagement&user_scopes[55]=pages_manage_metadata&user_scopes[56]=pages_read_user_content&user_scopes[57]=pages_manage_ads&user_scopes[58]=pages_manage_posts&user_scopes[59]=pages_manage_engagement&user_scopes[60]=whatsapp_business_messaging&user_scopes[61]=instagram_shopping_tag_products&user_scopes[62]=read_audience_network_insights&user_scopes[63]=user_about_me&user_scopes[64]=user_actions.books&user_scopes[65]=user_actions.fitness&user_scopes[66]=user_actions.music&user_scopes[67]=user_actions.news&user_scopes[68]=user_actions.video&user_scopes[69]=user_activities&user_scopes[70]=user_education_history&user_scopes[71]=user_events&user_scopes[72]=user_friends&user_scopes[73]=user_games_activity&user_scopes[74]=user_groups&user_scopes[75]=user_hometown&user_scopes[76]=user_interests&user_scopes[77]=user_likes&user_scopes[78]=user_location&user_scopes[79]=user_managed_groups&user_scopes[80]=user_photos&user_scopes[81]=user_posts&user_scopes[82]=user_relationship_details&user_scopes[83]=user_relationships&user_scopes[84]=user_religion_politics&user_scopes[85]=user_status&user_scopes[86]=user_tagged_places&user_scopes[87]=user_videos&user_scopes[88]=user_website&user_scopes[89]=user_work_history&user_scopes[90]=email&user_scopes[91]=manage_notifications&user_scopes[92]=manage_pages&user_scopes[93]=publish_actions&user_scopes[94]=publish_pages&user_scopes[95]=read_friendlists&user_scopes[96]=read_insights&user_scopes[97]=read_page_mailboxes&user_scopes[98]=read_stream&user_scopes[99]=rsvp_event&user_scopes[100]=read_mailbox&user_scopes[101]=business_creative_management&user_scopes[102]=business_creative_insights&user_scopes[103]=business_creative_insights_share&user_scopes[104]=whitelisted_offline_access&redirect_uri=fbconnect%3A%2F%2Fsuccess&response_types[0]=token&response_types[1]=code&display=page&action=finish&return_scopes=false&return_format[0]=access_token&return_format[1]=code&tp=unspecified&sdk=&selected_business_id=&set_token_expires_in_60_days=false", "fb_dtsg=" + fb_dtsg + "");
                if (!string.IsNullOrEmpty(rs))
                {
                    token = Regex.Match(rs, "access_token=(.+?)&data_access_expiration_time").Groups[1].Value;
                }
            }
            catch
            {

            }
            return token;
        }

        public static string getTokenInstaNew(string cookie, string proxy)
        {
            RequestXNet requestXNet = new RequestXNet(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36", proxy, 0);
            string rs = "";
            string rs2 = "";
            string encrypted_post_body = "";
            string scope = "";
            string fb_dtsg = "";

            string access_token = "";
            try
            {
                rs = requestXNet.RequestGetNew2("https://www.facebook.com/dialog/oauth?redirect_uri=fbconnect%3A%2F%2Fsuccess&scope=user_birthday,user_religion_politics,user_relationships,user_relationship_details,user_hometown,user_location,user_likes,user_education_history,user_work_history,user_website,user_events,user_photos,user_videos,user_friends,user_about_me,user_posts,email,manage_fundraisers,read_custom_friendlists,read_insights,rsvp_event,xmpp_login,offline_access,publish_video,openid,catalog_management,user_messenger_contact,gaming_user_locale,private_computation_access,instagram_business_basic,user_managed_groups,groups_show_list,pages_manage_cta,pages_manage_instant_articles,pages_show_list,pages_messaging,pages_messaging_phone_number,pages_messaging_subscriptions,read_page_mailboxes,ads_management,ads_read,business_management,instagram_basic,instagram_manage_comments,instagram_manage_insights,instagram_content_publish,publish_to_groups,groups_access_member_info,leads_retrieval,whatsapp_business_management,instagram_manage_messages,attribution_read,page_events,business_creative_transfer,pages_read_engagement,pages_manage_metadata,pages_read_user_content,pages_manage_ads,pages_manage_posts,pages_manage_engagement,whatsapp_business_messaging,instagram_shopping_tag_products,read_audience_network_insights,user_about_me,user_actions.books,user_actions.fitness,user_actions.music,user_actions.news,user_actions.video,user_activities,user_education_history,user_events,user_friends,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_managed_groups,user_photos,user_posts,user_relationship_details,user_relationships,user_religion_politics,user_status,user_tagged_places,user_videos,user_website,user_work_history,email,manage_notifications,manage_pages,publish_actions,publish_pages,read_friendlists,read_insights,read_page_mailboxes,read_stream,rsvp_event,read_mailbox,business_creative_management,business_creative_insights,business_creative_insights_share,whitelisted_offline_access&response_type=token,code&client_id=124024574287414");
                rs = rs.Replace("\\\"", "\"");
                encrypted_post_body = Regex.Match(rs, "name=\"encrypted_post_body\" value=\"(.*?)\"").Groups[1].Value;
                scope = Regex.Match(rs, "name=\"scope\" value=\"(.*?)\"").Groups[1].Value;
                fb_dtsg = Regex.Match(rs, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value;

                rs2 = requestXNet.RequestPostNew2("https://www.facebook.com/v1.0/dialog/oauth/skip/submit/", "fb_dtsg="+fb_dtsg+ "&__CONFIRM__=1&scope="+ scope+ "&encrypted_post_body="+ encrypted_post_body+ "&return_format[]=access_token");
                if (!string.IsNullOrEmpty(rs2))
                {
                    access_token = Regex.Match(rs2, "access_token=(.+?)&data_access_expiration_time").Groups[1].Value;
                }
            }
            catch
            {

            }
            return access_token;
        }

        public static (string, string) RunGetAccessToken2FA(string account, string password, string code2FA, string apiKey, string secret, string oauth)
        {
            Random random = new Random();

            string deviceUuid = CuaHelpers.GetDeviceUuid();
            string deviceFdid = deviceUuid;
            string cookies = "";
            string nid = CuaHelpers.getNid();
            string _cid = "";
            string adid = CuaHelpers.GetDeviceAdId();
            password = CuaHelpers.EncodePassword(password);
            string jazoest = CuaHelpers.GetJazoest();
            var client = new RestClient("https://b-graph.facebook.com/auth/login");
            var request = new RestRequest();
            request.Method = Method.Post;

            // Set request headers
            request.AddHeader("Host", "b-graph.facebook.com");
            request.AddHeader("Authorization", $"Bearer {oauth}|{secret}");
            request.AddHeader("Priority", "u=3, i");
            request.AddHeader("X-Fb-Connection-Quality", "EXCELLENT");
            request.AddHeader("X-Fb-Sim-Hni", "311370");
            request.AddHeader("X-Fb-Net-Hni", "311360");
            request.AddHeader("X-Fb-Connection-Type", "unknown");
            request.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 9; SM-A908N Build/PQ3B.190801.002) [FBAN/FB4A;FBAV/417.0.0.33.65;FBPN/com.facebook.katana;FBLC/vi_VN;FBBV/480086274;FBCR/Viettel Telecom;FBMF/samsung;FBBD/samsung;FBDV/SM-A908N;FBSV/9;FBCA/x86:armeabi-v7a;FBDM/{density=1.5,width=1600,height=900};FB_FW/1;FBRV/0;]");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("X-Tigon-Is-Retry", "False");
            request.AddHeader("X-Fb-Session-Id", CuaHelpers.GetSessionId(nid, _cid));
            request.AddHeader("X-Fb-Device-Group", random.Next(3000, 8000).ToString());
            request.AddHeader("X-Fb-Friendly-Name", "authenticate");
            request.AddHeader("X-Fb-Request-Analytics-Tags", "unknown");
            request.AddHeader("X-Fb-Http-Engine", "Liger");
            request.AddHeader("X-Fb-Client-Ip", "True");
            request.AddHeader("X-Fb-Server-Cluster", "True");
            request.AddHeader("X-Fb-Connection-Token", CuaHelpers.GetCid(_cid));
            request.AddHeader("Connection", "close");

            // Set request parameters
            request.AddParameter("adid", adid);
            request.AddParameter("email", account);
            request.AddParameter("password", password);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("community_id", "");
            request.AddParameter("secure_family_device_id", "");
            request.AddParameter("cpl", "true");
            request.AddParameter("currently_logged_in_userid", "0");
            request.AddParameter("device_id", deviceUuid);
            request.AddParameter("fdid", deviceFdid);
            request.AddParameter("fb_api_caller_class", "AuthOperations$PasswordAuthOperation");
            request.AddParameter("fb_api_req_friendly_name", "authenticate");
            request.AddParameter("enroll_misauth", "false");
            request.AddParameter("format", "json");
            request.AddParameter("generate_analytics_claim", "1");
            request.AddParameter("generate_machine_id", "1");
            request.AddParameter("generate_session_cookies", "1");
            request.AddParameter("jazoest", jazoest);
            request.AddParameter("meta_inf_fbmeta", "NO_FILE");
            request.AddParameter("source", "login");
            request.AddParameter("try_num", "1");
            request.AddParameter("credentials_type", "password");
            request.AddParameter("error_detail_type", "button_with_disabled");
            request.AddParameter("access_token", $"{oauth}|{secret}");

            // Execute the request
            var response = client.Execute(request);

            // Parse the response content
            JToken json = JToken.Parse(response.Content.ToString());

            if (json["error"]["message"] != null && json["error"]["message"].ToString() == "Invalid username or password")
            {
                return ("invalid", "invalid");
            }
            
            if (json["error"]["message"] != null && json["error"]["message"].ToString() == "Login approvals are on. Expect an SMS shortly with a code to use for log in")
            {
                // Handle 2FA scenario
                JObject errorData = (JObject)json["error"]["error_data"];

                // Get values
                string machineId = errorData["machine_id"].ToString();
                string loginFirstFactor = errorData["login_first_factor"].ToString();
                string supportUri = errorData["support_uri"].ToString();
                string authToken = errorData["auth_token"].ToString();

                string cleandcode2FA = code2FA.Replace(" ", "").Replace("\n", "");
                string totp = Helpers.Common.GetTotp(cleandcode2FA);

                RestRequest request2 = new RestRequest();
                request2.Method = Method.Post;

                // Set request headers
                request2.AddHeader("Host", "b-graph.facebook.com");
                request2.AddHeader("Authorization", $"Bearer {oauth}|{secret}");
                request2.AddHeader("Priority", "u=3, i");
                request2.AddHeader("X-Fb-Connection-Quality", "EXCELLENT");
                request2.AddHeader("X-Fb-Sim-Hni", "311370");
                request2.AddHeader("X-Fb-Net-Hni", "311360");
                request2.AddHeader("X-Fb-Connection-Type", "unknown");
                request2.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 9; SM-A908N Build/PQ3B.190801.002) [FBAN/FB4A;FBAV/417.0.0.33.65;FBPN/com.facebook.katana;FBLC/vi_VN;FBBV/480086274;FBCR/Viettel Telecom;FBMF/samsung;FBBD/samsung;FBDV/SM-A908N;FBSV/9;FBCA/x86:armeabi-v7a;FBDM/{density=1.5,width=1600,height=900};FB_FW/1;FBRV/0;]");
                request2.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request2.AddHeader("X-Tigon-Is-Retry", "False");
                request2.AddHeader("X-Fb-Session-Id", CuaHelpers.GetSessionId(nid, _cid));
                request2.AddHeader("X-Fb-Device-Group", random.Next(3000, 8000).ToString());
                request2.AddHeader("X-Fb-Friendly-Name", "authenticate");
                request2.AddHeader("X-Fb-Request-Analytics-Tags", "unknown");
                request2.AddHeader("X-Fb-Http-Engine", "Liger");
                request2.AddHeader("X-Fb-Client-Ip", "True");
                request2.AddHeader("X-Fb-Server-Cluster", "True");
                request2.AddHeader("X-Fb-Connection-Token", CuaHelpers.GetCid(_cid));
                request2.AddHeader("Connection", "close");

                request2.AddParameter("format", "json");
                request2.AddParameter("adid", adid);
                request2.AddParameter("device_id", deviceUuid);
                request2.AddParameter("fdid", deviceFdid);
                request2.AddParameter("email", account);
                request2.AddParameter("password", totp);
                request2.AddParameter("twofactor_code", totp);
                request2.AddParameter("encrypted_msisdn", "");
                request2.AddParameter("userid", account);
                request2.AddParameter("machine_id", machineId);
                request2.AddParameter("first_factor", loginFirstFactor);
                request2.AddParameter("support_uri", supportUri);
                request2.AddParameter("auth_token", authToken);
                request2.AddParameter("credentials_type", "two_factor");
                request2.AddParameter("cpl", "true");
                request2.AddParameter("meta_inf_fbmeta", "NO_FILE");
                request2.AddParameter("currently_logged_in_userid", "0");
                request2.AddParameter("generate_analytics_claim", "1");
                request2.AddParameter("generate_machine_id", "1");
                request2.AddParameter("generate_session_cookies", "1");
                request2.AddParameter("jazoest", jazoest);
                request2.AddParameter("source", "login");
                request2.AddParameter("try_num", "1");
                request2.AddParameter("fb_api_caller_class", "AuthOperations$PasswordAuthOperation");
                request2.AddParameter("fb_api_req_friendly_name", "authenticate");
                request2.AddParameter("api_key", apiKey);
                request2.AddParameter("access_token", $"{oauth}|{secret}");

                // Execute the 2FA request2
                response = client.Execute(request2);

                JToken json2 = JToken.Parse(response.Content.ToString());

                if (json2["error"] != null && json2["error"]["message"] != null && json2["error"]["message"].ToString() == "User must verify their account on www.facebook.com")
                {
                    return ("verify", "verify");
                }

                string accessToken = json2["access_token"].ToString();

                JArray sessionCookies = JArray.Parse(json2["session_cookies"].ToString());
                List<string> cookiesList = new List<string>();
                foreach (var cookie in sessionCookies)
                {
                    JToken ck = JToken.Parse(cookie.ToString());

                    string name = ck["name"]?.ToString();
                    string value = ck["value"]?.ToString();

                    if (name != null && value != null)
                    {
                        // Add the formatted cookie to the list
                        cookiesList.Add($"{name}={value}");
                    }
                }

                cookies = string.Join("; ", cookiesList);

                return (accessToken, cookies);
            }
            else
            {
                // Handle non-2FA scenario
                string accessToken = json["access_token"].ToString();
                JArray sessionCookies = JArray.Parse(json["session_cookies"].ToString());
                List<string> cookiesList = new List<string>();
                foreach (var cookie in sessionCookies)
                {
                    JToken ck = JToken.Parse(cookie.ToString());

                    string name = ck["name"]?.ToString();
                    string value = ck["value"]?.ToString();

                    if (name != null && value != null)
                    {
                        cookiesList.Add($"{name}={value}");
                    }
                }

                cookies = string.Join("; ", cookiesList);
                return (accessToken, cookies);
            }
        }

        public static (List<string>, string) GetPage(string url, string proxy, string userAgent)
        {
            Meta request = new Meta(null, userAgent, proxy);
            //RestClient client = new RestClient();
            //client.Proxy = "";
            //client.UserAgent = "";

            List<string> list = new List<string>();
           
            string response = request.GetNew(url).Result;

            JToken responseData = JToken.Parse(response);

            string next_url = null;
            if(responseData != null && responseData["data"] != null)
            {
                list.Add(responseData["data"].ToString());

                if (responseData["paging"] != null && responseData["paging"]["next"] != null)
                {
                    next_url = responseData["paging"]["next"].ToString();
                }    
            }

            return (list, next_url);
        }

    }
}

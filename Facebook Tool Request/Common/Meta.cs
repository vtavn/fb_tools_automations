using Facebook_Tool_Request.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.core;
using System.Net;
using System.Net.Security;
using RestSharp;

namespace cuakit
{
    internal class Meta
    {
        public RestClient Client { set; get; }
        private RestClientOptions options;
        private RestRequest request;
        public string LSD { set; get; }
        public string Fb_dtsg { set; get; }
        public string Jazoest { set; get; }
        public string ResponseURI { set; get; }
        public string User { set; get; }
        public string UID { set; get; }
        public string datapost { set; get; }
        public string sessionId { set; get; }

        private string html;
        string UserAgentDefault = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36";

        public Meta(string cookies = null, string userAgent = null, string ProxyUrl = null)
        {
            datapost = "";
            User = "";
            Setoptions(userAgent);
            SetProxy(ProxyUrl);
            Client = new RestClient(options);
            SetCookie(cookies);
        }

        private void Setoptions(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) userAgent = UserAgentDefault;
            if (options == null) options = new RestClientOptions() { MaxTimeout = 60000, CookieContainer = new System.Net.CookieContainer(), UserAgent = userAgent, Encoding = Encoding.UTF8, Expect100Continue = true, MaxRedirects = 100, FollowRedirects = true, ThrowOnAnyError = false, FailOnDeserializationError = false, ThrowOnDeserializationError = false, RemoteCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; }), PreAuthenticate = true };
        }
        public string _Cookie { set; get; }
        private void SetCookie(string cookies)
        {
            if (string.IsNullOrEmpty(cookies)) cookies = "locale=vi_VN;";
            _Cookie = cookies;
            if (string.IsNullOrEmpty(UID)) UID = RegexMatch(cookies, "c_user=(\\d+)").Groups[1].Value;
            foreach (string coo in cookies.Split(new[] { ";", " " }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (coo.Contains("useragent=")) continue;
                if (coo.Contains("i_user")) continue;
                string[] temp = coo.Split('=');
                if (temp.Length == 2) Client.CookieContainer.Add(new Cookie(temp[0], temp[1], "/", "facebook.com"));
            }
        }
        public void setDatapost(string uid, string user)
        {
            datapost = $"av={user}&__user={uid}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";

        }
        public void SetProxy(string ProxyUrl)
        {
            if (!string.IsNullOrEmpty(ProxyUrl))
            {
                string[] prox = ProxyUrl.Replace(" ", "").Replace("|", ":").Split(':');
                string proxyUserName = "";
                string proxyPassword = "";
                string proxyHost = prox[0];
                string proxyPort = "";
                if (!string.IsNullOrEmpty(proxyHost) && prox.Length >= 2)
                {
                    proxyPort = prox[1];
                    var proxy = new WebProxy(proxyHost, int.Parse(proxyPort));
                    if (prox.Length > 2)
                    {
                        proxyUserName = prox[2];
                        proxyPassword = prox[3];
                        proxy.Credentials = new NetworkCredential(userName: proxyUserName, password: proxyPassword);
                    }
                    options.Proxy = proxy;
                }
            }
        }
        public string GetCookie(string url = "https://www.facebook.com")
        {
            string temp = "";
            var MyCookie = Client.CookieContainer.GetCookies(new Uri(url));
            foreach (Cookie co in MyCookie)
            {
                temp += co.Name + "=" + co.Value + "; ";
            }
            return temp;
        }
        public void ResetToken(string HTML = "")
        {
            if (!string.IsNullOrEmpty(HTML)) html = HTML;
            if (!string.IsNullOrEmpty(html))
            {
                Fb_dtsg = RegexMatch(html.Replace("\\", string.Empty), "DTSGInitialData\",(.*?),{\"token\":\"(.*?)\"").Groups[2].Value;
                Jazoest = RegexMatch(html.Replace("\\", string.Empty), "jazoest=(\\d+)").Groups[1].Value;
                if (string.IsNullOrEmpty(Jazoest)) Jazoest = RegexMatch(html.Replace("\\", string.Empty), "jazoest\",\"version\":2,\"should_randomize\":false},(\\d+)").Groups[1].Value;
                LSD = RegexMatch(html.Replace("\\", string.Empty), "LSD\",(.*?),{\"token\":\"(.*?)\"").Groups[2].Value;
                sessionId = RegexMatch(html.Replace("\\", string.Empty), "\"sessionID\":\"(.*?)\"").Groups[1].Value;
            }
        }
        public Task<string> Post(string url, string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest(url);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(datapost)) datapost = $"av={UID}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                data = datapost + data;
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                return res?.Content;
            });
            return restk;
        }
        public Task<string> PostLogin(string url, string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest(url);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                }
                data = datapost + data;
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);

                if (string.IsNullOrEmpty(LSD))
                {
                    string temp = res?.Content;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        ResetToken(temp);
                    }
                }
                try
                {
                    if (res.ResponseUri != null)
                    {
                        ResponseURI = res.ResponseUri.OriginalString;
                    }
                }
                catch { }
                return res?.Content;
            });
            return restk;
        }
        public Task<string> PostCheckPoint(string url, string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest(url);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);

                try
                {
                    if (res.ResponseUri != null)
                    {
                        ResponseURI = res.ResponseUri.OriginalString;
                    }
                }
                catch { }

                return res?.Content;
            });
            return restk;
        }
        public Task<string> PostGraphApi(string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest("https://www.facebook.com/api/graphql/");
                request.AddHeader("accept", "*/*");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(User)) User = UID;
                    if (string.IsNullOrEmpty(datapost) || User != UID) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                if (!string.IsNullOrEmpty(data)) data = data.Replace("{Var:UID}", UID).Replace("{Var:SessionId}", sessionId);
                if (!data.Contains("&__user=") && !string.IsNullOrEmpty(UID)) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }
        public void SetData(string data)
        {
            datapost = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
        }
        public Task<string> Get(string url)
        {
            var restk = Task.Run(async () =>
            {
                request = new RestRequest(url.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest));
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
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
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                if (string.IsNullOrEmpty(LSD))
                {
                    string temp = res?.Content;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        ResetToken(temp);
                    }
                }

                return res?.Content;
            });
            return restk;
        }
        public Task<string> Post(string url, string data, bool isPage = false)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(User)) User = UID;
                if (string.IsNullOrEmpty(datapost) || isPage) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                url = url.Replace("{Var:Data}", datapost).Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);

                request = new RestRequest(url);
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD)) request.AddHeader("x-fb-lsd", LSD);
                if (string.IsNullOrEmpty(data) || (!data.Contains("&__user=") && !data.Contains("fb_dtsg"))) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }
        public string PostString(string url, string data)
        {
            request = new RestRequest(url);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("referer", "https://www.facebook.com/");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            if (!string.IsNullOrEmpty(LSD)) request.AddHeader("x-fb-lsd", LSD);
            request.AddStringBody(data, "application/x-www-form-urlencoded");
            request.Method = Method.Post;
            var res = Client.ExecuteAsync(request).Result;
            html = res?.Content;
            string error = GetError(html);
            if (!string.IsNullOrEmpty(error)) return error;
            return res?.Content;
        }
        public string GetError(string ketqua)
        {
            if (string.IsNullOrEmpty(ketqua)) return "";
            string status = RegexMatch(ketqua.Replace("\\\"", "'"), "name_error\":\"(.*?)\"").Groups[1].Value;
            if (string.IsNullOrEmpty(status)) status = RegexMatch(ketqua.Replace("\\\"", "'"), "name_error\":\"(.*?)\"").Groups[1].Value;
            if (string.IsNullOrEmpty(status)) status = RegexMatch(ketqua.Replace("\\\"", "'"), "error_message\":\"(.*?)\"").Groups[1].Value;
            if (string.IsNullOrEmpty(status)) status = RegexMatch(ketqua.Replace("\\\"", "'"), "errorDescription\":\"(.*?)\"").Groups[1].Value;
            if (!string.IsNullOrEmpty(status)) status = Regex.Unescape(status);
            return status;
        }
        public Match RegexMatch(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input)) return Match.Empty;
            else return Regex.Match(input, pattern);
        }
        public string UpLoadImg(string filename)
        {
            if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
            {
                Get("https://www.facebook.com/").Wait();
            }
            if (string.IsNullOrEmpty(User)) User = UID;
            string url = "https://upload.facebook.com/ajax/react_composer/attachments/photo/upload" + $"?av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
            request = new RestRequest(url);
            request.AddHeader("accept", "*/*");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("referer", "https://www.facebook.com/");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            if (!string.IsNullOrEmpty(LSD))
            {
                request.AddHeader("x-fb-lsd", LSD);
            }
            request.AddQueryParameter("source", "8");
            request.AddQueryParameter("profile_id", User);
            request.AddQueryParameter("waterfallxapp", "comet");
            request.AddQueryParameter("upload_id", "jsc_c_gk");
            request.AddFile("farr", filename, "image/jpeg");
            request.Method = Method.Post;
            var res = Client.ExecuteAsync(request).Result;
            html = res?.Content;
            string error = GetError(html);
            if (!string.IsNullOrEmpty(error)) return error;

            return Regex.Match(html, "photoID\":\"(\\d+)").Groups[1].Value;
        }

        //post status, story
        public Task<string> PostStatusGraphApi(string attachments, string tagIds, string content, string docId)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest("https://www.facebook.com/api/graphql/");
                request.AddHeader("accept", "*/*");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                string ssGen = Guid.NewGuid().ToString();
                string data = string.Concat(new string[]
                                {
                                    "&variables={\"input\":{\"composer_entry_point\":\"inline_composer\",\"composer_source_surface\":\"timeline\",\"idempotence_token\":\""+ssGen+"_FEED\",\"source\":\"WWW\",\"attachments\":["+attachments+"],\"audience\":{\"privacy\":{\"allow\":[],\"base_state\":\"EVERYONE\",\"deny\":[],\"tag_expansion_state\":\"UNSPECIFIED\"}},\"message\":{\"ranges\":[],\"text\":\""+content+"\"},\"with_tags_ids\":["+tagIds+"],\"inline_activities\":[],\"explicit_place_id\":\"\",\"text_format_preset_id\":\"0\",\"logging\":{\"composer_session_id\":\""+ssGen+"\"},\"navigation_data\":{\"attribution_id_v2\":\"\"},\"tracking\":[null],\"actor_id\":\""+UID+"\",\"client_mutation_id\":\"2\"},\"displayCommentsFeedbackContext\":null,\"displayCommentsContextEnableComment\":null,\"displayCommentsContextIsAdPreview\":null,\"displayCommentsContextIsAggregatedShare\":null,\"displayCommentsContextIsStorySet\":null,\"feedLocation\":\"TIMELINE\",\"feedbackSource\":0,\"focusCommentID\":null,\"gridMediaWidth\":230,\"groupID\":null,\"scale\":1,\"privacySelectorRenderLocation\":\"COMET_STREAM\",\"renderLocation\":\"timeline\",\"useDefaultActor\":false,\"inviteShortLinkKey\":null,\"isFeed\":false,\"isFundraiser\":false,\"isFunFactPost\":false,\"isGroup\":false,\"isEvent\":false,\"isTimeline\":true,\"isSocialLearning\":false,\"isPageNewsFeed\":false,\"isProfileReviews\":false,\"isWorkSharedDraft\":false,\"UFI2CommentsProvider_commentsKey\":\"ProfileCometTimelineRoute\",\"hashtag\":null,\"canUserManageOffers\":false}&doc_id="+docId
                                });
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(User)) User = UID;
                    if (string.IsNullOrEmpty(datapost) || User != UID) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                if (!string.IsNullOrEmpty(data)) data = data.Replace("{Var:UID}", UID).Replace("{Var:SessionId}", sessionId);
                if (!data.Contains("&__user=") && !string.IsNullOrEmpty(UID)) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }


        //business
        public Task<string> PostGraphBusinessApi(string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://business.facebook.com/").Wait();
                }
                request = new RestRequest("https://business.facebook.com/api/graphql/");
                request.AddHeader("accept", "*/*");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://business.facebook.com/api/graphql/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(User)) User = UID;
                    if (string.IsNullOrEmpty(datapost) || User != UID) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                if (!string.IsNullOrEmpty(data)) data = data.Replace("{Var:UID}", UID);
                if (!data.Contains("&__user=") && !string.IsNullOrEmpty(UID)) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }

        public Task<string> GraphApi(string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest("https://www.facebook.com/api/graphql/");
                request.AddHeader("accept", "*/*");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(User)) User = UID;
                    if (string.IsNullOrEmpty(datapost) || User != UID) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                if (!string.IsNullOrEmpty(data)) data = data.Replace("{Var:UID}", UID);
                if (!data.Contains("&__user=") && !string.IsNullOrEmpty(UID)) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }

        public Task<string> GraphMApi(string data)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://www.facebook.com/").Wait();
                }
                request = new RestRequest("https://m.facebook.com/api/graphql/");
                request.AddHeader("accept", "*/*");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://www.facebook.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                if (!string.IsNullOrEmpty(LSD))
                {
                    request.AddHeader("x-fb-lsd", LSD);
                    if (string.IsNullOrEmpty(User)) User = UID;
                    if (string.IsNullOrEmpty(datapost) || User != UID) datapost = $"av={User}&__user={UID}&__a=1&__dyn=&__csr=&__req=&__hs=&dpr=&__ccg=&__rev=&__s=&__hsi=&__comet_req=15&fb_dtsg={Fb_dtsg}&jazoest={Jazoest}&lsd={LSD}&__spin_r=&__spin_b=&__spin_t=&fb_api_caller_class=RelayModern";
                }
                if (!string.IsNullOrEmpty(data)) data = data.Replace("{Var:UID}", UID);
                if (!data.Contains("&__user=") && !string.IsNullOrEmpty(UID)) data = datapost + "&" + data.TrimStart('&');
                else data = data.Replace("{Var:Fb_dtsg}", Fb_dtsg).Replace("{Var:LSD}", LSD).Replace("{Var:Jazoest}", Jazoest);
                request.AddStringBody(data, "application/x-www-form-urlencoded");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                return res?.Content;
            });
            return restk;
        }

        public bool isPageProfile(string id)
        {
            string html = GraphApi("&variables={\"pageID\":\"" + id + "\"}&doc_id=5710437792369510").Result;
            if (html.Contains("\"data\":{\"page\":null}"))
            {
                return true;
            }
            else return false;
        }
        public bool isIDPublish(string id)
        {
            string html = GraphApi("&q=node(" + id + "){name}").Result;
            string name = RegexMatch(html, "name\":\"(.*?\"").Groups[1].Value;
            if (!string.IsNullOrEmpty(name))
            {
                return true;
            }else return false;
        }
        public void ChangePageProfile(string profile)
        {
            if (string.IsNullOrEmpty(LSD))
            {
                Get("https://www.facebook.com").Wait();
            }
            string temphtml = GraphApi(datapost + "&variables={\"profile_id\":\"" + profile + "\"}&server_timestamps=true&doc_id=4855674131145168").Result;
            if (temphtml.Contains("{\"data\":{\"profile_switcher_comet_login\":{\"name\":\""))
            {
                temphtml = Get("https://www.facebook.com/").Result;
                ResetToken(temphtml);
            }
        }
        public void ChangeBackProfile()
        {
            if (string.IsNullOrEmpty(LSD))
            {
                Get("https://www.facebook.com").Wait();
            }
            string temphtml = GraphApi(datapost + "&variables={\"profile_id\":\"" + UID + "\"}&server_timestamps=true&doc_id=4855674131145168&fb_api_analytics_tags=[\"qpl_active_flow_ids=30605361\"]").Result;
            temphtml = Get("https://www.facebook.com/").Result;
            ResetToken(temphtml);
        }

        public string getDataShare(string linkFb)
        {
            string result = this.PostGraphApi("&variables={\"feedLocation\":\"FEED_COMPOSER\",\"focusCommentID\":null,\"goodwillCampaignId\":\"\",\"goodwillCampaignMediaIds\":[],\"goodwillContentType\":null,\"params\":{\"url\":\"" + linkFb + "\"},\"privacySelectorRenderLocation\":\"COMET_COMPOSER\",\"renderLocation\":\"composer_preview\",\"parentStoryID\":null,\"scale\":1.5,\"useDefaultActor\":false,\"shouldIncludeStoryAttachment\":false}&doc_id=5564070646981374").Result;
            string result2;
            if (string.IsNullOrEmpty(result))
            {
                result2 = "Lỗi link share";
            }
            else
            {
                string value = this.RegexMatch(result, "\"share_scrape_data\":\"(.*?)\",\"story\":").Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    result2 = "{\"link\":{\"share_scrape_data\":\"" + value + "\"}}";
                }
                else
                {
                    result2 = "Lỗi link share";
                }
            }
            return result2;
        }
        public string getIdPost(string linkFb)
        {
            string result = this.PostGraphApi("&variables={\"feedLocation\":\"FEED_COMPOSER\",\"focusCommentID\":null,\"goodwillCampaignId\":\"\",\"goodwillCampaignMediaIds\":[],\"goodwillContentType\":null,\"params\":{\"url\":\"" + linkFb + "\"},\"privacySelectorRenderLocation\":\"COMET_COMPOSER\",\"renderLocation\":\"composer_preview\",\"parentStoryID\":null,\"scale\":1.5,\"useDefaultActor\":false,\"shouldIncludeStoryAttachment\":false}&doc_id=5564070646981374").Result;
            string result2;
            if (string.IsNullOrEmpty(result))
            {
                result2 = "";
            }
            else
            {
                string value = this.RegexMatch(result, "post_id\":\"(\\d+)\\\"").Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    result2 = value;
                }
                else
                {
                    result2 = "";
                }
            }
            return result2;
        }
        public void TryDispose()
        {
            Client?.Dispose();
        }

        public Task<string> GetNew(string url)
        {
            var restk = Task.Run(async () =>
            {
                request = new RestRequest(url);
                //request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                //request.AddHeader("sec-fetch-site", "same-origin");
                //request.AddHeader("sec-fetch-dest", "empty");
                //request.AddHeader("accept-language", "en-US,en;q=0.9");
                //request.AddHeader("sec-fetch-mode", "cors");
                //request.AddHeader("referer", "https://www.facebook.com/");
                //request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
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
                html = res?.Content;
                string error = GetError(html);
                if (!string.IsNullOrEmpty(error)) return error;
                if (string.IsNullOrEmpty(LSD))
                {
                    string temp = res?.Content;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        ResetToken(temp);
                    }
                }

                return res?.Content;
            });
            return restk;
        }

        ~Meta()
        {
            Client?.Dispose();
        }

        public static string loginFacebookWithCookie(string cookie, string userAgent, string proxy, int typeProxy)
        {
            string status = "";
            if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
            {
                status = "Cookie Die";
            }
            RequestXNet requestXNetL = new RequestXNet(cookie, userAgent, proxy, typeProxy, false, true);
            requestXNetL.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            requestXNetL.request.AddHeader("sec-fetch-site", "same-origin");
            requestXNetL.request.AddHeader("sec-fetch-dest", "document");
            requestXNetL.request.AddHeader("accept-language", "en-US,en;q=0.9");
            requestXNetL.request.AddHeader("sec-fetch-mode", "navigate");
            requestXNetL.request.AddHeader("referer", "https://www.facebook.com/");
            requestXNetL.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            requestXNetL.request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            requestXNetL.request.AddHeader("Cookie", cookie);

            string login = requestXNetL.RequestGet("https://www.facebook.com/");

            if (login.Length < 0)
            {
                status = "Login Errors";
            }
            else
            {
                status = login;
            }

            return status;
        }
        public static string loginFacebookUrlWithCookie(string cookie, string userAgent, string proxy, int typeProxy, string uid)
        {
            string status = null;
            if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
            {
                status = null;
            }
            RequestXNet requestXNetL = new RequestXNet(cookie, userAgent, proxy, typeProxy, true);
            requestXNetL.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            requestXNetL.request.AddHeader("sec-fetch-site", "same-origin");
            requestXNetL.request.AddHeader("sec-fetch-dest", "document");
            requestXNetL.request.AddHeader("accept-language", "en-US,en;q=0.9");
            requestXNetL.request.AddHeader("sec-fetch-mode", "navigate");
            requestXNetL.request.AddHeader("referer", "https://www.facebook.com/");
            requestXNetL.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            requestXNetL.request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            requestXNetL.request.AddHeader("Cookie", cookie);

            string login = requestXNetL.RequestGet("https://www.facebook.com/" + uid);
            cookie = requestXNetL.GetCookie();

            if (login.Length < 0)
            {
                status = null;
            }
            else
            {
                status = login;
            }

            return status;
        }

        //Inboxes
        public Task<string> PostInboxes(string url)
        {
            var restk = Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(LSD) && _Cookie != "locale=vi_VN;")
                {
                    Get("https://inboxes.com/").Wait();
                }
                request = new RestRequest(url);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://inboxes.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.Method = Method.Post;
                var res = await Client.ExecuteAsync(request);
                return res?.Content;
            });
            return restk;
        }
        public Task<string> GetInboxes(string url, string uid, string token)
        {
            var restk = Task.Run(async () =>
            {
                request = new RestRequest(url);
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("referer", "https://inboxes.com/");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("cookie", "user_id=" + uid);
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
    }
}

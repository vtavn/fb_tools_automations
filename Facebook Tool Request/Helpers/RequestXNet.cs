using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace Facebook_Tool_Request.Helpers
{
    public class RequestXNet
    {
        public xNet.HttpRequest request;

        public RequestXNet(string cookie, string userAgent, string proxy, int typeProxy, bool allowAutoRedirect = true, bool Accept = false)
        {
            bool flag = userAgent == "";
            if (flag)
            {
                userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";
            }
            this.request = new xNet.HttpRequest
            {
                KeepAlive = true,
                AllowAutoRedirect = allowAutoRedirect,
                Cookies = new CookieDictionary(false),
                UserAgent = userAgent
            };
            if(Accept == false)
            {
                this.request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            }
            this.request.AddHeader("Accept-Language", "en-US,en;q=0.9,vi;q=0.8");

            bool flag2 = cookie != "";
            if (flag2)
            {
                this.AddCookie(cookie);
            }
            bool flag3 = proxy != "";
            if (flag3)
            {
                switch (proxy.Split(new char[]
                {
                    ':'
                }).Count<string>())
                {
                    case 1:
                        {
                            bool flag4 = typeProxy == 0;
                            if (flag4)
                            {
                                this.request.Proxy = HttpProxyClient.Parse("127.0.0.1:" + proxy);
                            }
                            else
                            {
                                this.request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:" + proxy);
                            }
                            break;
                        }
                    case 2:
                        {
                            bool flag5 = typeProxy == 0;
                            if (flag5)
                            {
                                this.request.Proxy = HttpProxyClient.Parse(proxy);
                            }
                            else
                            {
                                this.request.Proxy = Socks5ProxyClient.Parse(proxy);
                            }
                            break;
                        }
                    case 4:
                        {
                            bool flag6 = typeProxy == 0;
                            if (flag6)
                            {
                                this.request.Proxy = new HttpProxyClient(proxy.Split(new char[]
                                {
                            ':'
                                })[0], Convert.ToInt32(proxy.Split(new char[]
                                {
                            ':'
                                })[1]), proxy.Split(new char[]
                                {
                            ':'
                                })[2], proxy.Split(new char[]
                                {
                            ':'
                                })[3]);
                            }
                            else
                            {
                                this.request.Proxy = new Socks5ProxyClient(proxy.Split(new char[]
                                {
                            ':'
                                })[0], Convert.ToInt32(proxy.Split(new char[]
                                {
                            ':'
                                })[1]), proxy.Split(new char[]
                                {
                            ':'
                                })[2], proxy.Split(new char[]
                                {
                            ':'
                                })[3]);
                            }
                            break;
                        }
                }
            }
        }

        public string RequestGet(string url) => this.request.Get(url)?.ToString();
        public string RequestGet(string url, string username, string password) => this.request.Get($"{url}?user={username}&password={password}")?.ToString();
        public byte[] GetBytes(string url)
        {
            return this.request.Get(url, null).ToBytes();
        }

        public string RequestPost(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
        {
            bool flag = data == "" || contentType == "";
            string result;
            if (flag)
            {
                result = this.request.Post(url).ToString();
            }
            else
            {
                result = this.request.Post(url, data, contentType).ToString();
            }
            return result;
        }

        public string UpLoad(string url, MultipartContent data = null) => this.request.Post(url, data)?.ToString();

        //public void AddCookie(string cookie)
        //{
        //    string[] array = cookie.Split(new char[]
        //    {
        //        ';'
        //    });
        //    foreach (string text in array)
        //    {
        //        string[] array3 = text.Split(new char[]
        //        {
        //            '='
        //        });
        //        bool flag = array3.Count<string>() > 1;
        //        if (flag)
        //        {
        //            try
        //            {
        //                this.request.Cookies.Add(array3[0], array3[1]);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //}
        public void AddCookie(string cookie)
        {
            var cookieArray = cookie.Split(';')
                .Select(x => x.Trim())
                .Select(x => x.Split('='))
                .Where(x => x.Length == 2)
                .ToList();

            foreach (var cookiePair in cookieArray)
            {
                try
                {
                    this.request.Cookies.Add(cookiePair[0], cookiePair[1]);
                }
                catch
                {
                    // Handle exception
                }
            }
        }

        public string GetCookie()
        {
            return this.request.Cookies.ToString();
        }
        public string Uri() => request.Address.AbsoluteUri;

        public string RequestGetNew(string url)
        {
            this.request.AddHeader("authority", "www.facebook.com");
            this.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/jxl,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            this.request.AddHeader("accept-language", "vi,en-US;q=0.9,en;q=0.8");
            this.request.AddHeader("cache-control", "max-age=0");
            this.request.AddHeader("dnt", "1");
            this.request.AddHeader("dpr", "1.25");
            this.request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"117\", \"Not;A=Brand\";v=\"8\"");
            this.request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"117.0.5938.157\", \"Not;A=Brand\";v=\"8.0.0.0\"");
            this.request.AddHeader("sec-ch-ua-mobile", "?0");
            this.request.AddHeader("sec-ch-ua-model", "\"\"");
            this.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            this.request.AddHeader("sec-ch-ua-platform-version", "\"15.0.0\"");
            this.request.AddHeader("sec-fetch-dest", "document");
            this.request.AddHeader("sec-fetch-mode", "navigate");
            this.request.AddHeader("sec-fetch-site", "same-origin");
            this.request.AddHeader("sec-fetch-user", "?1");
            this.request.AddHeader("upgrade-insecure-requests", "1");
            this.request.AddHeader("viewport-width", "1038");

            return this.request.Get(url)?.ToString();
        }

        public string RequestPostNew(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
        {
            this.request.AddHeader("authority", "www.facebook.com");
            this.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/jxl,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            this.request.AddHeader("accept-language", "vi,en-US;q=0.9,en;q=0.8");
            this.request.AddHeader("cache-control", "max-age=0");
            this.request.AddHeader("dnt", "1");
            this.request.AddHeader("dpr", "1.25");
            this.request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"117\", \"Not;A=Brand\";v=\"8\"");
            this.request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"117.0.5938.157\", \"Not;A=Brand\";v=\"8.0.0.0\"");
            this.request.AddHeader("sec-ch-ua-mobile", "?0");
            this.request.AddHeader("sec-ch-ua-model", "\"\"");
            this.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            this.request.AddHeader("sec-ch-ua-platform-version", "\"15.0.0\"");
            this.request.AddHeader("sec-fetch-dest", "document");
            this.request.AddHeader("sec-fetch-mode", "navigate");
            this.request.AddHeader("sec-fetch-site", "same-origin");
            this.request.AddHeader("sec-fetch-user", "?1");
            this.request.AddHeader("upgrade-insecure-requests", "1");
            this.request.AddHeader("viewport-width", "1038");

            bool flag = data == "" || contentType == "";
            string result;
            if (flag)
            {
                result = this.request.Post(url).ToString();
            }
            else
            {
                result = this.request.Post(url, data, contentType).ToString();
            }
            return result;
        }

        public string RequestGetNew2(string url)
        {
            this.request.AddHeader("authority", "d.facebook.com");
            this.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/jxl,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            this.request.AddHeader("accept-language", "vi,en-US;q=0.9,en;q=0.8");
            this.request.AddHeader("cache-control", "no-cache");
            this.request.AddHeader("dnt", "1");
            this.request.AddHeader("dpr", "1.25");
            this.request.AddHeader("pragma", "no-cache");
            this.request.AddHeader("sec-ch-prefers-color-scheme", "dark");
            this.request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"117\", \"Not;A=Brand\";v=\"8\"");
            this.request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"117.0.5938.157\", \"Not;A=Brand\";v=\"8.0.0.0\"");
            this.request.AddHeader("sec-ch-ua-mobile", "?0");
            this.request.AddHeader("sec-ch-ua-model", "\"\"");
            this.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            this.request.AddHeader("sec-ch-ua-platform-version", "\"15.0.0\"");
            this.request.AddHeader("sec-fetch-dest", "document");
            this.request.AddHeader("sec-fetch-mode", "navigate");
            this.request.AddHeader("sec-fetch-site", "none");
            this.request.AddHeader("sec-fetch-user", "?1");
            this.request.AddHeader("upgrade-insecure-requests", "1");
            this.request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36");

            return this.request.Get(url)?.ToString();
        }
        public string RequestPostNew2(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
        {
            this.request.AddHeader("authority", "www.facebook.com");
            this.request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/jxl,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            this.request.AddHeader("accept-language", "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5");
            this.request.AddHeader("cache-control", "no-cache");
            this.request.AddHeader("dnt", "1");
            this.request.AddHeader("dpr", "1.25");
            this.request.AddHeader("origin", "https://www.facebook.com");
            this.request.AddHeader("pragma", "no-cache");
            this.request.AddHeader("referer", "https://www.facebook.com/dialog/oauth?redirect_uri=fbconnect%3A%2F%2Fsuccess&scope=user_birthday%2Cuser_religion_politics%2Cuser_relationships%2Cuser_relationship_details%2Cuser_hometown%2Cuser_location%2Cuser_likes%2Cuser_education_history%2Cuser_work_history%2Cuser_website%2Cuser_events%2Cuser_photos%2Cuser_videos%2Cuser_friends%2Cuser_about_me%2Cuser_posts%2Cemail%2Cmanage_fundraisers%2Cread_custom_friendlists%2Cread_insights%2Crsvp_event%2Cxmpp_login%2Coffline_access%2Cpublish_video%2Copenid%2Ccatalog_management%2Cuser_messenger_contact%2Cgaming_user_locale%2Cprivate_computation_access%2Cinstagram_business_basic%2Cuser_managed_groups%2Cgroups_show_list%2Cpages_manage_cta%2Cpages_manage_instant_articles%2Cpages_show_list%2Cpages_messaging%2Cpages_messaging_phone_number%2Cpages_messaging_subscriptions%2Cread_page_mailboxes%2Cads_management%2Cads_read%2Cbusiness_management%2Cinstagram_basic%2Cinstagram_manage_comments%2Cinstagram_manage_insights%2Cinstagram_content_publish%2Cpublish_to_groups%2Cgroups_access_member_info%2Cleads_retrieval%2Cwhatsapp_business_management%2Cinstagram_manage_messages%2Cattribution_read%2Cpage_events%2Cbusiness_creative_transfer%2Cpages_read_engagement%2Cpages_manage_metadata%2Cpages_read_user_content%2Cpages_manage_ads%2Cpages_manage_posts%2Cpages_manage_engagement%2Cwhatsapp_business_messaging%2Cinstagram_shopping_tag_products%2Cread_audience_network_insights%2Cuser_about_me%2Cuser_actions.books%2Cuser_actions.fitness%2Cuser_actions.music%2Cuser_actions.news%2Cuser_actions.video%2Cuser_activities%2Cuser_education_history%2Cuser_events%2Cuser_friends%2Cuser_games_activity%2Cuser_groups%2Cuser_hometown%2Cuser_interests%2Cuser_likes%2Cuser_location%2Cuser_managed_groups%2Cuser_photos%2Cuser_posts%2Cuser_relationship_details%2Cuser_relationships%2Cuser_religion_politics%2Cuser_status%2Cuser_tagged_places%2Cuser_videos%2Cuser_website%2Cuser_work_history%2Cemail%2Cmanage_notifications%2Cmanage_pages%2Cpublish_actions%2Cpublish_pages%2Cread_friendlists%2Cread_insights%2Cread_page_mailboxes%2Cread_stream%2Crsvp_event%2Cread_mailbox%2Cbusiness_creative_management%2Cbusiness_creative_insights%2Cbusiness_creative_insights_share%2Cwhitelisted_offline_access&response_type=token%2Ccode&client_id=124024574287414");
            this.request.AddHeader("sec-ch-prefers-color-scheme", "dark");
            this.request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"117\", \"Not; A = Brand\";v=\"8\"");
            this.request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"117.0.5938.157\", \"Not; A = Brand\";v=\"8.0.0.0\"");
            this.request.AddHeader("sec-ch-ua-mobile", "?0");
            this.request.AddHeader("sec-ch-ua-model", "\"\"");
            this.request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            this.request.AddHeader("sec-ch-ua-platform-version", "\"15.0.0\"");
            this.request.AddHeader("sec-fetch-dest", "document");
            this.request.AddHeader("sec-fetch-mode", "navigate");
            this.request.AddHeader("sec-fetch-site", "same-origin");
            this.request.AddHeader("sec-fetch-user", "?1");
            this.request.AddHeader("upgrade-insecure-requests", "1");

            bool flag = data == "" || contentType == "";
            string result;
            if (flag)
            {
                result = this.request.Post(url).ToString();
            }
            else
            {
                result = this.request.Post(url, data, contentType).ToString();
            }
            return result;
        }


    }
}

using Facebook_Tool_Request.core;
using Facebook_Tool_Request.core.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    public class CommonChrome
    {
        public static bool CheckFacebookBlocked(Chrome chrome)
        {
            return chrome.GetURL().StartsWith("https://m.facebook.com/feature_limit_notice/") || chrome.CheckExistElements(0.0, new string[]
            {
                "[href*=\"facebook.com/help/177066345680802\"]",
                "[href*=\"facebook.com/help/contact/\"]"
            }) != 0 || chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway");
        }
        public static string GetUserAgentDefault()
        {
            string text = "";
            try
            {
                JSON_Settings json_Settings = new JSON_Settings("configGeneral", false);
                Chrome chrome = new Chrome
                {
                    HideBrowser = true,
                    PathExtension = ""
                };
                bool flag = json_Settings.GetValueInt("typeBrowser", 0) != 0;

                if (flag)
                {
                    chrome.LinkToOtherBrowser = json_Settings.GetValue("txtLinkToOtherBrowser", "");
                }
                bool flag2 = chrome.Open(true);
                if (flag2)
                {
                    text = chrome.GetUseragent();
                    text = text.Replace("Headless", "");
                    chrome.Close();
                }
            }
            catch
            {
            }
            return text;
        }
        public static int CheckTypeWebFacebookFromUrl(string url)
        {
            int result = 0;
            bool flag = url.StartsWith("https://www.facebook") || url.StartsWith("https://facebook") || url.StartsWith("https://web.facebook");
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = url.StartsWith("https://m.facebook") || url.StartsWith("https://d.facebook") || url.StartsWith("https://mobile.facebook");
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = url.StartsWith("https://mbasic.facebook");
                    if (flag3)
                    {
                        result = 3;
                    }
                }
            }
            return result;
        }
        public static int CheckFacebookWebsite(Chrome chrome, string url)
        {
            bool flag = !chrome.CheckIsLive();
            int result;
            if (flag)
            {
                result = -2;
            }
            else
            {
                int num = 0;
                for (int i = 0; i < 2; i++)
                {
                    num = CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL());
                    bool flag2 = num == 0;
                    if (!flag2)
                    {
                        break;
                    }
                    chrome.GotoURL(url);
                    chrome.DelayTime(1.0);
                }
                result = num;
            }
            return result;
        }
        public static string RequestGet(Chrome chrome, string url, string website)
        {
            try
            {
                bool flag = website.Split(new char[]
                {
                    '/'
                }).Length > 2;
                if (flag)
                {
                    website = website.Replace("//", "__");
                    website = website.Split(new char[]
                    {
                        '/'
                    })[0];
                    website = website.Replace("__", "//");
                }
                bool flag2 = !chrome.GetURL().StartsWith(website);
                if (flag2)
                {
                    chrome.GotoURL(website);
                    chrome.DelayTime(1.0);
                    chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Đừng có mà nhìn! <br>CUA TOOLKIT</b>'; document.querySelector('body').style = 'text-align: center; background-color:#fff'");
                }
                return (string)chrome.ExecuteScript("async function RequestGet() { var output = ''; try { var response = await fetch('" + url + "'); if (response.ok) { var body = await response.text(); return body; } } catch {} return output; }; var c = await RequestGet(); return c;");
            }
            catch
            {
            }
            return "";
        }
        private static bool CheckStringContainKeyword(string content, List<string> lstKerword)
        {
            for (int i = 0; i < lstKerword.Count; i++)
            {
                bool flag = Regex.IsMatch(content, lstKerword[i]) || content.Contains(lstKerword[i]);
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckNoveri(Chrome chrome, string currentUrl = "", string html = "")
        {
            bool flag = currentUrl == "";
            if (flag)
            {
                currentUrl = chrome.GetURL();
            }
            List<string> lstKerword = new List<string>
            {
                "facebook.com/confirmemail.php"
            };
            bool flag2 = CommonChrome.CheckStringContainKeyword(currentUrl, lstKerword);
            bool result;
            if (flag2)
            {
                result = true;
            }
            else
            {
                List<string> list = new List<string>
                {
                    "[name=\"c\"]"
                };
                bool flag3 = chrome.CheckExistElements(0.0, list.ToArray()) > 0;
                result = flag3;
            }
            return result;
        }

        public static bool CheckCheckpoint(Chrome chrome, string currentUrl = "", string html = "")
        {
            bool flag = currentUrl == "";
            if (flag)
            {
                currentUrl = chrome.GetURL();
            }
            List<string> lstKerword = new List<string>
            {
                "facebook.com/checkpoint/828281030927956",
                "facebook.com/checkpoint/1501092823525282",
                "facebook.com/nt/screen/?params=%7B%22token",
                "facebook.com/x/checkpoint/"
            };
            bool flag2 = CommonChrome.CheckStringContainKeyword(currentUrl, lstKerword);
            bool result;
            if (flag2)
            {
                result = true;
            }
            else
            {
                bool flag3 = html == "";
                if (flag3)
                {
                    html = chrome.GetPageSource();
                }
                List<string> lstKerword2 = new List<string>
                {
                    "verification_method",
                    "submit[Yes]",
                    "send_code",
                    "/checkpoint/dyi",
                    "captcha_response",
                    "https://www.facebook.com/communitystandards/",
                    "help/121104481304395",
                    "help/166863010078512",
                    "help/117450615006715",
                    "checkpoint/1501092823525282",
                    "checkpoint/828281030927956",
                    "name=\"code_1\""
                };
                bool flag4 = CommonChrome.CheckStringContainKeyword(html, lstKerword2);
                if (flag4)
                {
                    result = true;
                }
                else
                {
                    List<string> list = new List<string>
                    {
                        "[name=\"verification_method\"]",
                        "[name=\"submit[Yes]\"]",
                        "[name=\"send_code\"]",
                        "#captcha_response",
                        "[href=\"https://www.facebook.com/communitystandards/\"]",
                        "[name=\"code_1\"]",
                        "[action=\"/login/checkpoint/\"] [name=\"contact_index\"]"
                    };
                    bool flag5 = chrome.CheckExistElements(0.0, list.ToArray()) > 0;
                    result = flag5;
                }
            }
            return result;
        }

        public static bool CheckLoginSuccess(Chrome chrome, string currentUrl = "", string html = "")
        {
            bool flag = currentUrl == "";
            if (flag)
            {
                currentUrl = chrome.GetURL();
            }
            List<string> lstKerword = new List<string>
            {
                "https://m.facebook.com/home.php"
            };
            bool flag2 = CommonChrome.CheckStringContainKeyword(currentUrl, lstKerword);
            bool result;
            if (flag2)
            {
                result = true;
            }
            else
            {
                bool flag3 = html == "";
                if (flag3)
                {
                    html = chrome.GetPageSource();
                }
                List<string> lstKerword2 = new List<string>
                {
                    "/friends/",
                    "/logout.php?button_location=settings&amp;button_name=logout"
                };
                bool flag4 = CommonChrome.CheckStringContainKeyword(html, lstKerword2);
                if (flag4)
                {
                    result = true;
                }
                else
                {
                    List<string> list = new List<string>
                    {
                        "a[href*=\"/friends/\"]",
                        "[action=\"/logout.php?button_location=settings&button_name=logout\"]"
                    };
                    bool flag5 = chrome.CheckExistElements(0.0, list.ToArray()) > 0;
                    result = flag5;
                }
            }
            return result;
        }

        internal static void CheckStatusAccount(Chrome chrome, bool isSendRequest)
        {
            try
            {
                bool flag = chrome.CheckChromeClosed();
                if (flag)
                {
                    chrome.Status = StatusChromeAccount.ChromeClosed;
                }
                else
                {
                    string text2;
                    if (isSendRequest)
                    {
                        string text = chrome.GetURL();
                        text = Regex.Match(text, "https://(.*?)facebook.com").Value + "/login";
                        text2 = CommonChrome.RequestGet(chrome, text, text);
                    }
                    else
                    {
                        text2 = chrome.GetPageSource();
                    }
                    bool flag2 = text2 == "-2";
                    if (flag2)
                    {
                        chrome.Status = StatusChromeAccount.ChromeClosed;
                    }
                    else
                    {
                        bool flag3 = Regex.IsMatch(text2, "login_form") || text2.Contains("/login/?next");
                        if (flag3)
                        {
                            bool flag4 = chrome.CheckExistElement("[href*=\"/login/?next\"]", 0.0) == 1;
                            if (flag4)
                            {
                                chrome.Click(4, "[href*=\"/login/?next\"]", 0, 0, "", 0, 1);
                            }
                            chrome.Status = StatusChromeAccount.LoginWithUserPass;
                        }
                        else
                        {
                            bool flag5 = Regex.IsMatch(text2, "login_profile_form") || Regex.IsMatch(text2, "/login/device-based/validate-p");
                            if (flag5)
                            {
                                chrome.Status = StatusChromeAccount.LoginWithSelectAccount;
                            }
                            else
                            {
                                bool flag6 = Convert.ToBoolean(chrome.ExecuteScript("var kq=false;if(document.querySelector('#mErrorView')!=null && !document.querySelector('#mErrorView').getAttribute('style').includes('display:none')) kq=true;return kq+''")) || Regex.IsMatch(text2, "href=\"https://m.facebook.com/login.php");
                                if (flag6)
                                {
                                    chrome.Status = StatusChromeAccount.LoginWithSelectAccount;
                                }
                                else
                                {
                                    bool flag7 = CommonChrome.CheckNoveri(chrome, "", "");
                                    if (flag7)
                                    {
                                        chrome.Status = StatusChromeAccount.Noveri;
                                    }
                                    else
                                    {
                                        bool flag8 = CommonChrome.CheckCheckpoint(chrome, "", "");
                                        if (flag8)
                                        {
                                            chrome.Status = StatusChromeAccount.Checkpoint;
                                        }
                                        else
                                        {
                                            bool flag9 = text2.Contains("error-information-popup-content") || text2.Contains("suggestionsSummaryList");
                                            if (flag9)
                                            {
                                                chrome.Status = StatusChromeAccount.NoInternet;
                                            }
                                            else
                                            {
                                                bool flag10 = CommonChrome.CheckLoginSuccess(chrome, "", "");
                                                if (flag10)
                                                {
                                                    chrome.Status = StatusChromeAccount.Logined;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        public static int CheckLiveCookie(Chrome chrome, string url = "https://m.facebook.com")
        {
            try
            {
                bool flag = chrome.CheckChromeClosed();
                if (flag)
                {
                    return -2;
                }
                bool flag2 = CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL()) != CommonChrome.CheckTypeWebFacebookFromUrl(url);
                if (flag2)
                {
                    chrome.GotoURL(url);
                }
                string url2 = chrome.GetURL();
                bool flag3 = url2.Contains("facebook.com/checkpoint/") || url2.Contains("facebook.com/nt/screen/?params=%7B%22token") || url2.Contains("facebook.com/x/checkpoint/");
                if (flag3)
                {
                    return 2;
                }
                int num = CommonChrome.CheckFacebookWebsite(chrome, url);
                bool flag4 = num == 2;
                if (flag4)
                {
                    bool flag5 = chrome.CheckExistElement("a[href*=\"/zero/optin/write/?action=cancel&page=dialtone_optin_page\"]", 5.0) == 1;
                    if (flag5)
                    {
                        chrome.ExecuteScript("document.querySelector('a[href*=\"/zero/optin/write/?action=cancel&page=dialtone_optin_page\"]').click()");
                        chrome.DelayTime(3.0);
                        bool flag6 = chrome.CheckExistElement("[action=\"/zero/optin/write/?action=confirm&page=reconsider_optin_dialog\"] button", 10.0) == 1;
                        if (flag6)
                        {
                            chrome.ExecuteScript("document.querySelector('[action=\"/zero/optin/write/?action=confirm&page=reconsider_optin_dialog\"] button').click()");
                            chrome.DelayTime(3.0);
                        }
                    }
                    bool flag7 = chrome.GetURL().StartsWith("https://m.facebook.com/zero/optin/write/");
                    if (flag7)
                    {
                        chrome.DelayTime(1.0);
                        chrome.ExecuteScript("document.querySelector('[action=\"/zero/optin/write/?action=confirm&page=reconsider_optin_dialog\"] button').click()");
                        chrome.DelayTime(3.0);
                    }
                    bool flag8 = chrome.GetURL().StartsWith("https://m.facebook.com/zero/policy/optin");
                    if (flag8)
                    {
                        chrome.DelayTime(1.0);
                        chrome.ExecuteScript("document.querySelector('a[data-sigil=\"touchable\"]').click()");
                        chrome.DelayTime(3.0);
                        bool flag9 = chrome.CheckExistElement("button[data-sigil=\"touchable\"]", 10.0) == 1;
                        if (flag9)
                        {
                            chrome.DelayTime(1.0);
                            chrome.ExecuteScript("document.querySelector('button[data-sigil=\"touchable\"]').click()");
                            chrome.DelayTime(3.0);
                        }
                    }
                    bool flag10 = Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('legal_consent/basic/?consent_step=1')){x[i].click();break;check='true'}} return check"));
                    if (flag10)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Common.DelayTime(2.0);
                            bool flag11 = !Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('legal_consent/basic/?consent_step=1')){x[i].click();break;check='true'}} return check"));
                            if (flag11)
                            {
                                break;
                            }
                        }
                        for (int j = 0; j < 5; j++)
                        {
                            Common.DelayTime(2.0);
                            bool flag12 = !Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('consent/basic/log')){x[i].click();break;check='true'}} return check"));
                            if (flag12)
                            {
                                break;
                            }
                        }
                        bool flag13 = chrome.CheckExistElement("[href=\"/home.php\"]", 0.0) == 1;
                        if (flag13)
                        {
                            chrome.Click(4, "[href=\"/home.php\"]", 0, 0, "", 0, 1);
                        }
                    }
                    bool flag14 = chrome.GetURL().StartsWith("https://m.facebook.com/legal_consent");
                    if (flag14)
                    {
                        chrome.ExecuteScript("document.querySelector('button').click()");
                        chrome.DelayTime(1.0);
                        chrome.ExecuteScript("document.querySelectorAll('button')[1].click()");
                        chrome.DelayTime(1.0);
                        chrome.ExecuteScript("document.querySelector('button').click()");
                        chrome.DelayTime(1.0);
                        chrome.ExecuteScript("document.querySelectorAll('button')[1].click()");
                        chrome.DelayTime(1.0);
                    }
                    bool flag15 = chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway");
                    if (flag15)
                    {
                        chrome.Click(4, "[data-sigil=\"touchable\"]", 0, 0, "", 0, 1);
                        chrome.DelayTime(1.0);
                    }
                    bool flag16 = chrome.CheckExistElement("button[value=\"OK\"]", 0.0) == 1;
                    if (flag16)
                    {
                        chrome.Click(4, "button[value=\"OK\"]", 0, 0, "", 0, 1);
                        chrome.DelayTime(1.0);
                    }
                    bool flag17 = chrome.CheckExistElement("[data-store-id=\"2\"]>span", 0.0) == 1;
                    if (flag17)
                    {
                        chrome.Click(4, "[data-store-id=\"2\"]>span", 0, 0, "", 0, 1);
                        chrome.DelayTime(1.0);
                    }
                    bool flag18 = chrome.CheckExistElement("[data-nt=\"FB:HEADER_FOOTER_VIEW\"]>div>div>div>span>span", 0.0) == 1;
                    if (flag18)
                    {
                        chrome.Click(4, "[data-nt=\"FB:HEADER_FOOTER_VIEW\"]>div>div>div>span>span", 0, 0, "", 0, 1);
                        chrome.DelayTime(3.0);
                    }
                    bool flag19 = chrome.CheckExistElement("#nux-nav-button", 0.0) == 1;
                    if (flag19)
                    {
                        bool flag20 = false;
                        for (int k = 0; k < 5; k++)
                        {
                            bool flag21 = flag20;
                            if (flag21)
                            {
                                break;
                            }
                            int num2 = chrome.CheckExistElements(3.0, new string[]
                            {
                                "#qf_skip_dialog_skip_link",
                                "#nux-nav-button"
                            });
                            int num3 = num2;
                            int num4 = num3;
                            if (num4 != 1)
                            {
                                if (num4 != 2)
                                {
                                    flag20 = true;
                                }
                                else
                                {
                                    chrome.Click(1, "nux-nav-button", 0, 0, "", 0, 1);
                                    chrome.DelayTime(2.0);
                                }
                            }
                            else
                            {
                                chrome.ExecuteScript("document.querySelector('#qf_skip_dialog_skip_link').click()");
                                chrome.DelayTime(1.0);
                            }
                        }
                    }
                }
                else
                {
                    bool flag22 = num == 1;
                    if (flag22)
                    {
                        bool flag23 = chrome.GetURL().StartsWith("https://www.facebook.com/legal_consent");
                        if (flag23)
                        {
                            for (int l = 0; l < 5; l++)
                            {
                                bool flag24 = chrome.CheckExistElement("button", 0.0) != 1;
                                if (flag24)
                                {
                                    break;
                                }
                                chrome.ExecuteScript("document.querySelector('button').click()");
                                chrome.DelayTime(1.0);
                            }
                        }
                    }
                }
                CommonChrome.CheckStatusAccount(chrome, true);
                switch (chrome.Status)
                {
                    case StatusChromeAccount.ChromeClosed:
                        return -2;
                    case StatusChromeAccount.LoginWithUserPass:
                    case StatusChromeAccount.LoginWithSelectAccount:
                        return 0;
                    case StatusChromeAccount.Checkpoint:
                        return 2;
                    case StatusChromeAccount.Logined:
                        return 1;
                    case StatusChromeAccount.NoInternet:
                        return -3;
                    case StatusChromeAccount.Noveri:
                        return 3;
                }
            }
            catch
            {
            }
            return 0;
        }
        public static int GotoLogin(Chrome chrome, int typeWeb)
        {
            bool flag = false;
            try
            {
                switch (typeWeb)
                {
                    case 1:
                        chrome.GotoURL("https://www.facebook.com/login");
                        break;
                    case 2:
                        chrome.GotoURL("https://m.facebook.com/login");
                        break;
                    case 3:
                        chrome.GotoURL("https://mbasic.facebook.com/login");
                        break;
                }
                flag = true;
                chrome.DelayTime(1.0);
            }
            catch (Exception ex)
            {
            }
            return flag ? 1 : 0;
        }

        public static string LoginFacebookUsingUidPassNew(Chrome chrome, string uid, string pass, string fa2 = "", string Url = "https://m.facebook.com", int tocDoGoVanBan = 0, bool isDontSaveBrowser = false, int timeOut = 120)
        {
            int num = 0;
            int num2 = 0;
            int tickCount = Environment.TickCount;
            try
            {
                bool flag = CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL()) != CommonChrome.CheckTypeWebFacebookFromUrl(Url);
                if (flag)
                {
                    chrome.GotoURL(Url);
                }
                string text;
                for (; ; )
                {
                    int num3 = CommonChrome.CheckFacebookWebsite(chrome, chrome.GetURL());
                    bool flag2 = chrome.CheckExistElement("[data-cookiebanner=\"accept_button\"]", 0.0) == 1;
                    if (flag2)
                    {
                        chrome.Click(4, "[data-cookiebanner=\"accept_button\"]", 0, 0, "", 0, 1);
                        bool flag3 = chrome.CheckExistElement("[name=\"pass\"]", 0.0) != 1;
                        if (flag3)
                        {
                            CommonChrome.GotoLogin(chrome, num3);
                            bool flag4 = chrome.CheckExistElement("[data-cookiebanner=\"accept_button\"]", 0.0) == 1;
                            if (flag4)
                            {
                                chrome.Click(4, "[data-cookiebanner=\"accept_button\"]", 0, 0, "", 0, 1);
                            }
                        }
                    }
                    else
                    {
                        bool flag5 = chrome.CheckExistElement("[name=\"email\"]", 0.0) == 1 && chrome.CheckExistElement("[name=\"pass\"]", 0.0) == 1 && chrome.CheckExistElement("[name=\"login\"]", 0.0) == 1;
                        bool flag6 = chrome.CheckExistElements(0.0, new string[]
                        {
                            "[ajaxify*=\"/login/device-based/async/remove/\"]",
                            "[data-sigil=\"login_profile_form\"] div[role=\"button\"]",
                            "[action=\"/login/device-based/validate-pin/\"] input[type=\"submit\"]"
                        }) > 0;
                        bool flag7 = !flag5 && !flag6;
                        if (flag7)
                        {
                            CommonChrome.GotoLogin(chrome, num3);
                            bool flag8 = chrome.CheckExistElement("[data-cookiebanner=\"accept_button\"]", 0.0) == 1;
                            if (flag8)
                            {
                                chrome.Click(4, "[data-cookiebanner=\"accept_button\"]", 0, 0, "", 0, 1);
                            }
                        }
                    }
                    bool flag9 = CommonChrome.CheckCheckpoint(chrome, "", "");
                    if (flag9)
                    {
                        break;
                    }
                    int num4 = chrome.CheckExistElements(0.0, new string[]
                    {
                        "[data-sigil=\"login_profile_form\"] div[role=\"button\"]",
                        "[action=\"/login/device-based/validate-pin/\"] input[type=\"submit\"]"
                    });
                    bool flag10 = num3 == 2 && num4 > 0;
                    if (flag10)
                    {
                        int num5 = num4;
                        int num6 = num5;
                        if (num6 != 1)
                        {
                            if (num6 == 2)
                            {
                                chrome.ExecuteScript("document.querySelector('[action=\"/login/device-based/validate-pin/\"] input[type=\"submit\"]').click()");
                            }
                        }
                        else
                        {
                            chrome.ExecuteScript("document.querySelector('[data-sigil=\"login_profile_form\"] div[role=\"button\"]').click()");
                        }
                        chrome.DelayTime(1.0);
                        num2 = chrome.CheckExistElements(5.0, new string[]
                        {
                            "[name=\"pass\"]",
                            "#approvals_code"
                        });
                        bool flag11 = num2 == 1;
                        if (flag11)
                        {
                            bool flag12 = chrome.SendKeysWithSpeed(tocDoGoVanBan, 2, "pass", pass, 0.1, true, 0.1) == 1;
                            if (flag12)
                            {
                                chrome.DelayTime(1.0);
                                bool flag13 = chrome.Click(4, chrome.GetCssSelector("button", "data-sigil", "password_login_button"), 0, 0, "", 0, 1) == 1;
                                if (flag13)
                                {
                                    chrome.DelayTime(1.0);
                                }
                            }
                        }
                    }
                    else
                    {
                        bool flag14 = chrome.CheckExistElement("[data-sigil=\"touchable\"]", 0.0) == 1 && chrome.CheckExistElement("#m_login_auto_search_form_forgot_password_button", 0.0) != 1;
                        if (flag14)
                        {
                            chrome.Click(4, "[data-sigil=\"touchable\"]", 0, 0, "", 0, 1);
                        }
                        int num7 = 1;
                        bool flag15 = false;
                        do
                        {
                            switch (num7)
                            {
                                case 1:
                                    num2 = chrome.SendKeysWithSpeed(tocDoGoVanBan, 2, "email", uid, 0.1, true, 0.1);
                                    break;
                                case 2:
                                    num2 = chrome.SendKeysWithSpeed(tocDoGoVanBan, 2, "pass", pass, 0.1, true, 0.1);
                                    break;
                                case 3:
                                    num2 = chrome.SendEnter(2, "pass");
                                    chrome.DelayTime(3.0);
                                    flag15 = true;
                                    break;
                                default:
                                    flag15 = true;
                                    break;
                            }
                            bool flag16 = num2 == -2;
                            if (flag16)
                            {
                                goto Block_24;
                            }
                            chrome.DelayTime(1.0);
                            num7++;
                        }
                        while (!flag15);
                    }
                    Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>
                    {
                        {
                            1,
                            new List<string>
                            {
                                "[name=\"login\"]",
                                "[name=\"reset_action\"]"
                            }
                        },
                        {
                            2,
                            new List<string>
                            {
                                "[name=\"approvals_code\"]"
                            }
                        },
                        {
                            3,
                            new List<string>
                            {
                                "#checkpoint_title"
                            }
                        },
                        {
                            4,
                            new List<string>
                            {
                                "#checkpointSubmitButton",
                                "#checkpointBottomBar"
                            }
                        },
                        {
                            5,
                            new List<string>
                            {
                                "#qf_skip_dialog_skip_link"
                            }
                        },
                        {
                            6,
                            new List<string>
                            {
                                "#nux-nav-button"
                            }
                        },
                        {
                            7,
                            new List<string>
                            {
                                "[name=\"n\"]"
                            }
                        },
                        {
                            8,
                            new List<string>
                            {
                                "#identify_search_text_input"
                            }
                        }
                    };
                    int num8 = 0;
                    int num9 = 0;
                    for (; ; )
                    {
                        bool flag17 = Environment.TickCount - tickCount >= timeOut * 1000;
                        if (flag17)
                        {
                            goto Block_25;
                        }
                        num2 = chrome.CheckExistElements(0.0, dic);
                        switch (num2)
                        {
                            case 1:
                                {
                                    bool flag18 = num8 == 0 && num9 == 0;
                                    if (flag18)
                                    {
                                        text = "";
                                        int num10 = CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL());
                                        int num11 = num10;
                                        if (num11 != 1)
                                        {
                                            if (num11 == 2)
                                            {
                                                text = chrome.ExecuteScript("var out='';var x=document.querySelector('#login_error');if(x!=null) out=x.innerText;return out;").ToString();
                                            }
                                        }
                                        else
                                        {
                                            text = chrome.ExecuteScript("var out='';var x=document.querySelector('#error_box');if(x!=null) out=x.innerText;return out;").ToString();
                                            text = ((text.Split(new string[]
                                            {
                                        "\r\n"
                                            }, StringSplitOptions.RemoveEmptyEntries).Count<string>() > 1) ? text.Split(new string[]
                                            {
                                        "\r\n"
                                            }, StringSplitOptions.RemoveEmptyEntries)[1] : text);
                                        }
                                        bool flag19 = text != "";
                                        if (flag19)
                                        {
                                            goto Block_32;
                                        }
                                        bool flag20 = chrome.CheckExistElement("[href=\"/login/identify/\"]", 0.0) == 1;
                                        if (flag20)
                                        {
                                            num = 4;
                                        }
                                        else
                                        {
                                            bool flag21 = chrome.CheckExistElement("#account_recovery_initiate_view_label", 0.0) == 1;
                                            if (flag21)
                                            {
                                                num = 5;
                                            }
                                            else
                                            {
                                                bool flag22 = !Convert.ToBoolean(chrome.ExecuteScript("return (document.querySelector('[name=\"email\"]').value!='' && document.querySelector('[name=\"pass\"]').value!='')+''").ToString());
                                                if (flag22)
                                                {
                                                    bool flag23 = chrome.ExecuteScript("return document.querySelector('[name=\"email\"]').value").ToString().Trim() == "";
                                                    if (flag23)
                                                    {
                                                        num = 4;
                                                    }
                                                    else
                                                    {
                                                        bool flag24 = chrome.ExecuteScript("return document.querySelector('[name=\"pass\"]').value").ToString().Trim() == "";
                                                        if (flag24)
                                                        {
                                                            num = 5;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool flag25 = chrome.ExecuteScript("return document.querySelector('[name=\"email\"]').value").ToString().Trim() == "";
                                        if (flag25)
                                        {
                                            goto Block_38;
                                        }
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    bool flag26 = fa2 == "";
                                    if (flag26)
                                    {
                                        num = 3;
                                    }
                                    else
                                    {
                                        num8++;
                                        bool flag27 = num8 > 2;
                                        if (flag27)
                                        {
                                            goto Block_40;
                                        }
                                        string totp = Common.GetTotp(fa2.Replace(" ", "").Replace("\n", "").Trim());
                                        bool flag28 = string.IsNullOrEmpty(totp);
                                        if (flag28)
                                        {
                                            goto Block_41;
                                        }
                                        chrome.SendKeysWithSpeed(tocDoGoVanBan, 2, "approvals_code", totp, 0.1, true, 0.1);
                                        chrome.DelayTime(1.0);
                                        chrome.SendEnter(2, "approvals_code");
                                        chrome.DelayTime(1.0);
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    bool flag29 = num8 > 1;
                                    if (flag29)
                                    {
                                        num = 6;
                                    }
                                    else
                                    {
                                        bool flag30 = CommonChrome.CheckCheckpoint(chrome, "", "");
                                        if (flag30)
                                        {
                                            goto Block_43;
                                        }
                                        num2 = chrome.CheckExistElements(0.0, new string[]
                                        {
                                    "button#checkpointSubmitButton",
                                    "#checkpointSubmitButton [type=\"submit\"]"
                                        });
                                        num9++;
                                        bool flag31 = num9 > 10;
                                        if (flag31)
                                        {
                                            goto Block_44;
                                        }
                                        bool flag32 = chrome.CheckExistElement("[value=\"dont_save\"]", 0.0) == 1 && isDontSaveBrowser;
                                        if (flag32)
                                        {
                                            chrome.Click(4, "[value=\"dont_save\"]", 0, 0, "", 0, 1);
                                        }
                                        bool flag33 = num2 == 1;
                                        if (flag33)
                                        {
                                            chrome.Click(4, "button#checkpointSubmitButton", 0, 0, "", 0, 1);
                                        }
                                        else
                                        {
                                            chrome.Click(4, "#checkpointSubmitButton [type=\"submit\"]", 0, 0, "", 0, 1);
                                        }
                                        chrome.DelayTime(1.0);
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    bool flag34 = num8 > 1;
                                    if (flag34)
                                    {
                                        num = 6;
                                    }
                                    else
                                    {
                                        bool flag35 = CommonChrome.CheckCheckpoint(chrome, "", "");
                                        if (flag35)
                                        {
                                            goto Block_48;
                                        }
                                        num2 = chrome.CheckExistElements(0.0, new string[]
                                        {
                                    "button#checkpointSubmitButton",
                                    "#checkpointSubmitButton [type=\"submit\"]"
                                        });
                                        num9++;
                                        bool flag36 = num9 > 10;
                                        if (flag36)
                                        {
                                            goto Block_49;
                                        }
                                        bool flag37 = chrome.CheckExistElement("[value=\"dont_save\"]", 0.0) == 1 && isDontSaveBrowser;
                                        if (flag37)
                                        {
                                            chrome.Click(4, "[value=\"dont_save\"]", 0, 0, "", 0, 1);
                                        }
                                        bool flag38 = num2 == 1;
                                        if (flag38)
                                        {
                                            chrome.Click(4, "button#checkpointSubmitButton", 0, 0, "", 0, 1);
                                        }
                                        else
                                        {
                                            chrome.Click(4, "#checkpointSubmitButton [type=\"submit\"]", 0, 0, "", 0, 1);
                                        }
                                        chrome.DelayTime(1.0);
                                    }
                                    break;
                                }
                            case 5:
                                chrome.ClickWithAction(1, "qf_skip_dialog_skip_link", 0, 0, "", 0);
                                chrome.DelayTime(2.0);
                                break;
                            case 6:
                                chrome.Click(1, "nux-nav-button", 0, 0, "", 0, 1);
                                chrome.DelayTime(2.0);
                                break;
                            case 7:
                                num = 5;
                                break;
                            case 8:
                                goto IL_152A;
                            default:
                                {
                                    bool flag39 = chrome.GetURL().Contains("facebook.com/confirm");
                                    if (flag39)
                                    {
                                        goto Block_52;
                                    }
                                    bool flag40 = CommonChrome.CheckCheckpoint(chrome, "", "");
                                    if (flag40)
                                    {
                                        goto Block_53;
                                    }
                                    bool flag41 = chrome.CheckExistElement("a[href*=\"/zero/optin/write/?action=cancel&page=dialtone_optin_page\"]", 0.0) == 1;
                                    if (flag41)
                                    {
                                        chrome.ExecuteScript("document.querySelector('a[href*=\"/zero/optin/write/?action=cancel&page=dialtone_optin_page\"]').click()");
                                        chrome.DelayTime(3.0);
                                        bool flag42 = chrome.CheckExistElement("[action=\"/zero/optin/write/?action=confirm&page=reconsider_optin_dialog\"] button", 10.0) == 1;
                                        if (flag42)
                                        {
                                            chrome.ExecuteScript("document.querySelector('[action=\"/zero/optin/write/?action=confirm&page=reconsider_optin_dialog\"] button').click()");
                                            chrome.DelayTime(3.0);
                                        }
                                    }
                                    else
                                    {
                                        bool flag43 = CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL()) == 2;
                                        if (flag43)
                                        {
                                            bool flag44 = chrome.GetURL().StartsWith("https://m.facebook.com/zero/policy/optin");
                                            if (flag44)
                                            {
                                                chrome.DelayTime(1.0);
                                                chrome.ExecuteScript("document.querySelector('a[data-sigil=\"touchable\"]').click()");
                                                chrome.DelayTime(3.0);
                                                bool flag45 = chrome.CheckExistElement("button[data-sigil=\"touchable\"]", 10.0) == 1;
                                                if (flag45)
                                                {
                                                    chrome.DelayTime(1.0);
                                                    chrome.ExecuteScript("document.querySelector('button[data-sigil=\"touchable\"]').click()");
                                                    chrome.DelayTime(3.0);
                                                }
                                            }
                                            bool flag46 = Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('legal_consent/basic/?consent_step=1')){x[i].click();break;check='true'}} return check"));
                                            if (flag46)
                                            {
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    Common.DelayTime(2.0);
                                                    bool flag47 = !Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('legal_consent/basic/?consent_step=1')){x[i].click();break;check='true'}} return check"));
                                                    if (flag47)
                                                    {
                                                        break;
                                                    }
                                                }
                                                for (int j = 0; j < 5; j++)
                                                {
                                                    Common.DelayTime(2.0);
                                                    bool flag48 = !Convert.ToBoolean(chrome.ExecuteScript("var check='false';var x=document.querySelectorAll('a');for(i=0;i<x.length;i++){if(x[i].href.includes('consent/basic/log')){x[i].click();break;check='true'}} return check"));
                                                    if (flag48)
                                                    {
                                                        break;
                                                    }
                                                }
                                                bool flag49 = chrome.CheckExistElement("[href=\"/home.php\"]", 0.0) == 1;
                                                if (flag49)
                                                {
                                                    chrome.Click(4, "[href=\"/home.php\"]", 0, 0, "", 0, 1);
                                                }
                                            }
                                            for (; ; )
                                            {
                                                bool flag50 = chrome.GetURL().StartsWith("https://m.facebook.com/legal_consent");
                                                if (!flag50)
                                                {
                                                    break;
                                                }
                                                int num12 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('button').length").ToString());
                                                bool flag51 = num12 == 3;
                                                if (flag51)
                                                {
                                                    chrome.Click(4, "button", 0, 0, "", 0, 1);
                                                }
                                                else
                                                {
                                                    bool flag52 = num12 == 4;
                                                    if (flag52)
                                                    {
                                                        chrome.Click(4, "button", 1, 0, "", 0, 1);
                                                    }
                                                }
                                                chrome.DelayTime(2.0);
                                            }
                                            bool flag53 = chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway");
                                            if (flag53)
                                            {
                                                bool flag54 = chrome.CheckExistElement("[data-nt=\"NT:IMAGE\"]", 15.0) == 1;
                                                if (flag54)
                                                {
                                                    chrome.ExecuteScript("document.querySelector('[data-nt=\"NT:IMAGE\"]').click()");
                                                    chrome.DelayTime(2.0);
                                                }
                                            }
                                            bool flag55 = chrome.CheckExistElement("button[value=\"OK\"]", 0.0) == 1;
                                            if (flag55)
                                            {
                                                chrome.Click(4, "button[value=\"OK\"]", 0, 0, "", 0, 1);
                                                chrome.DelayTime(1.0);
                                            }
                                            bool flag56 = chrome.CheckExistElement("[data-store-id=\"2\"]>span", 0.0) == 1;
                                            if (flag56)
                                            {
                                                chrome.Click(4, "[data-store-id=\"2\"]>span", 0, 0, "", 0, 1);
                                                chrome.DelayTime(1.0);
                                            }
                                            bool flag57 = chrome.CheckExistElement("[data-nt=\"FB:HEADER_FOOTER_VIEW\"]>div>div>div>span>span", 0.0) == 1;
                                            if (flag57)
                                            {
                                                chrome.Click(4, "[data-nt=\"FB:HEADER_FOOTER_VIEW\"]>div>div>div>span>span", 0, 0, "", 0, 1);
                                                chrome.DelayTime(3.0);
                                            }
                                        }
                                        else
                                        {
                                            bool flag58 = chrome.GetURL().StartsWith("https://www.facebook.com/legal_consent");
                                            if (flag58)
                                            {
                                                for (int k = 0; k < 5; k++)
                                                {
                                                    bool flag59 = chrome.CheckExistElement("button", 0.0) != 1;
                                                    if (flag59)
                                                    {
                                                        break;
                                                    }
                                                    chrome.ExecuteScript("document.querySelector('button').click()");
                                                    chrome.DelayTime(1.0);
                                                }
                                            }
                                        }
                                    }
                                    bool flag60 = CommonChrome.CheckLoginSuccess(chrome, "", "");
                                    if (flag60)
                                    {
                                        num = 1;
                                    }
                                    break;
                                }
                        }
                        if (num != 0)
                        {
                            goto IL_1E81;
                        }
                    }
                IL_152A:
                    chrome.GotoURL("https://www.facebook.com");
                    chrome.DelayTime(1.0);
                }
                num = 2;
                goto IL_1EBD;
            Block_24:
                num = -2;
            Block_25:
                goto IL_1E81;
            Block_32:
                return text;
            Block_38:
                num = 0;
                goto IL_1EBD;
            Block_40:
                num = 6;
                goto IL_1EBD;
            Block_41:
                num = 6;
                goto IL_1EBD;
            Block_43:
                num = 2;
            Block_44:
                goto IL_1EBD;
            Block_48:
                num = 2;
            Block_49:
                goto IL_1EBD;
            Block_52:
                num = 1;
                goto IL_1EBD;
            Block_53:
                num = 2;
            IL_1E81:;
            }
            catch (Exception ex)
            {
                Common.ExportError(chrome, ex, "Error Login Uid Pass");
            }
        IL_1EBD:
            return num.ToString() ?? "";
        }
        public static string LoginFacebookUsingCookie(Chrome chrome, string cookie, string URL = "https://www.facebook.com")
        {
            try
            {
                bool flag = chrome.GotoURLIfNotExist(URL) == -2;
                if (flag)
                {
                    return "-2";
                }
                bool flag2 = chrome.AddCookieIntoChrome(cookie, ".facebook.com") == -2;
                if (flag2)
                {
                    return "-2";
                }
                bool flag3 = chrome.Refresh() == -2;
                if (flag3)
                {
                    return "-2";
                }
                return CommonChrome.CheckLiveCookie(chrome, "https://m.facebook.com").ToString() ?? "";
            }
            catch
            {
            }
            return "0";
        }

        public static List<string> GetMyListUidFriend(Chrome chrome)
        {
            List<string> list = new List<string>();
            try
            {
                string tokenEAAAAZ = "";
                //string tokenEAAAAZ = CommonChrome.GetTokenEAAAAZ(chrome);
                bool flag = !chrome.GetURL().Contains("https://graph.facebook.com/");
                if (flag)
                {
                    chrome.GotoURL("https://graph.facebook.com/");
                }
                string json = (string)chrome.ExecuteScript("async function GetListUidNameFriend() { var output = ''; try { var response = await fetch('https://graph.facebook.com/me/friends?limit=5000&fields=id&access_token=" + tokenEAAAAZ + "'); if (response.ok) { var body = await response.text(); return body; } } catch {} return output; }; var c = await GetListUidNameFriend(); return c;");
                JObject jobject = JObject.Parse(json);
                bool flag2 = jobject["data"].Count<JToken>() > 0;
                if (flag2)
                {
                    for (int i = 0; i < jobject["data"].Count<JToken>(); i++)
                    {
                        string item = jobject["data"][i]["id"].ToString();
                        list.Add(item);
                    }
                }
            }
            catch
            {
            }
            return list;
        }
        public static int GoToSetting_TimelineAndTagging(Chrome chrome)
        {
            try
            {
                bool flag = chrome != null;
                if (flag)
                {
                    bool flag2 = chrome.CheckChromeClosed();
                    if (flag2)
                    {
                        return -2;
                    }
                    CommonChrome.GoToSetting(chrome);
                    string cssSelector = chrome.GetCssSelector("a", "href", "/privacy/touch/timeline_and_tagging/");
                    bool flag3 = cssSelector != "";
                    if (flag3)
                    {
                        chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('" + cssSelector + "')");
                        chrome.DelayThaoTacNho(0, null);
                        bool flag4 = chrome.Click(4, cssSelector, 0, 0, "", 0, 1) == 1;
                        if (flag4)
                        {
                            chrome.DelayThaoTacNho(0, null);
                            return 1;
                        }
                    }
                    return chrome.GotoURL("https://m.facebook.com/privacy/touch/timeline_and_tagging/");
                }
            }
            catch
            {
            }
            return 0;
        }
        public static int GoToSetting(Chrome chrome)
        {
            try
            {
                bool flag = chrome != null;
                if (flag)
                {
                    bool flag2 = chrome.CheckChromeClosed();
                    if (flag2)
                    {
                        return -2;
                    }
                    bool flag3 = chrome.CheckExistElement("[data-sigil=\"nav-popover bookmarks\"]>a", 0.0) == 1;
                    if (flag3)
                    {
                        chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('[data-sigil=\"nav-popover bookmarks\"]>a')");
                        chrome.DelayThaoTacNho(0, null);
                    }
                    int num = chrome.Click(4, "[data-sigil=\"nav-popover bookmarks\"]>a", 0, 0, "", 0, 1);
                    bool flag4 = num != 1;
                    if (flag4)
                    {
                        CommonChrome.GoToHome(chrome);
                        chrome.DelayThaoTacNho(2, null);
                        num = chrome.Click(4, "[data-sigil=\"nav-popover bookmarks\"]>a", 0, 0, "", 0, 1);
                    }
                    bool flag5 = num == 1;
                    if (flag5)
                    {
                        chrome.DelayThaoTacNho(1, null);
                        string cssSelector = chrome.GetCssSelector("a", "href", "/settings/");
                        bool flag6 = cssSelector != "";
                        if (flag6)
                        {
                            chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('" + cssSelector + "')");
                            chrome.DelayThaoTacNho(0, null);
                            bool flag7 = chrome.Click(4, cssSelector, 0, 0, "", 0, 1) == 1;
                            if (flag7)
                            {
                                chrome.DelayThaoTacNho(0, null);
                                return 1;
                            }
                        }
                    }
                    return chrome.GotoURL("https://m.facebook.com/settings/?entry_point=bookmark");
                }
            }
            catch
            {
            }
            return 0;
        }
        public static int GoToHome(Chrome chrome)
        {
            try
            {
                bool flag = chrome != null;
                if (flag)
                {
                    bool flag2 = chrome.CheckChromeClosed();
                    if (flag2)
                    {
                        return -2;
                    }
                    bool flag3 = !(chrome.GetURL() == "https://m.facebook.com/home.php") && !(chrome.GetURL() == "https://m.facebook.com");
                    if (flag3)
                    {
                        bool flag4 = chrome.CheckExistElement("#feed_jewel a", 0.0) == 1;
                        if (flag4)
                        {
                            chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('#feed_jewel a')");
                            chrome.DelayThaoTacNho(0, null);
                        }
                        bool flag5 = chrome.Click(4, "#feed_jewel a", 0, 0, "", 0, 1) != 1;
                        if (flag5)
                        {
                            chrome.GotoURL("https://m.facebook.com/home.php");
                        }
                        chrome.DelayTime(1.0);
                        bool flag6 = chrome.CheckExistElement("#nux-nav-button", 2.0) == 1;
                        if (flag6)
                        {
                            chrome.ClickWithAction(1, "nux-nav-button", 0, 0, "", 0);
                            chrome.DelayTime(2.0);
                        }
                    }
                    bool flag7 = chrome.CheckChromeClosed();
                    if (flag7)
                    {
                        return -2;
                    }
                    bool flag8 = chrome.GetURL() == "https://m.facebook.com/home.php" || chrome.GetURL() == "https://m.facebook.com/home.php?ref=wizard&_rdr" || chrome.GetURL() == "https://m.facebook.com";
                    if (flag8)
                    {
                        return 1;
                    }
                }
            }
            catch
            {
            }
            return 0;
        }
        public static string GetNameFromPost(Chrome chrome)
        {
            string text = chrome.ExecuteScript("var x='';document.querySelectorAll('[property=\"og:title\"]').length>0&&(x=document.querySelector('[property=\"og:title\"]').getAttribute('content')),''==x&&document.querySelectorAll('[data-gt] a').length>0&&(x=document.querySelector('[data-gt] a').innerText),''==x&&document.querySelectorAll('.actor').length>0&&(x=document.querySelector('.actor').innerText), x+''; return x;").ToString();
            bool flag = text == "";
            if (flag)
            {
                text = chrome.ExecuteScript("return document.title").ToString().Split(new char[]
                {
                    '-',
                    '|'
                })[0].Trim();
            }
            return text;
        }
        public static int ScrollRandom(Chrome chrome, int from = 3, int to = 5)
        {
            try
            {
                bool flag = chrome.CheckChromeClosed();
                if (flag)
                {
                    return -2;
                }
                int num = Base.rd.Next(from, to + 1);
                int num2 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString());
                bool flag2 = chrome.ScrollSmooth(Base.rd.Next(chrome.GetSizeChrome().Y / 2, chrome.GetSizeChrome().Y)) == 1;
                if (flag2)
                {
                    chrome.DelayRandom(1, 3);
                    int num3 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString());
                    bool flag3 = num2 != num3;
                    if (flag3)
                    {
                        for (int i = 0; i < num - 1; i++)
                        {
                            num2 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString());
                            bool flag4 = chrome.ScrollSmooth(((Base.rd.Next(1, 1000) % 5 != 0) ? 1 : -1) * Base.rd.Next(chrome.GetSizeChrome().Y / 2, chrome.GetSizeChrome().Y)) == -2;
                            if (flag4)
                            {
                                return -2;
                            }
                            chrome.DelayRandom(1, 3);
                            num3 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString());
                            bool flag5 = num2 != num3;
                            if (!flag5)
                            {
                                break;
                            }
                            chrome.DelayRandom(1, 2);
                        }
                    }
                    return 1;
                }
            }
            catch
            {
            }
            return 0;
        }
        public static bool IsCheckpoint(Chrome chrome)
        {
            try
            {
                bool flag = chrome.CheckExistElements(0.0, new string[]
                {
                    "#checkpointSubmitButton",
                    "#captcha_response",
                    "[name=\"verification_method\"]",
                    "#checkpointBottomBar",
                    "[href =\"https://www.facebook.com/communitystandards/\"]"
                }) > 0;
                if (flag)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        public static int CheckStatusChrome(Chrome chrome)
        {
            try
            {
                bool flag = chrome != null;
                if (flag)
                {
                    bool flag2 = chrome.CheckChromeClosed();
                    if (flag2)
                    {
                        return -2;
                    }
                    int num = chrome.CheckExistElement("[jsselect=\"suggestionsSummaryList\"]", 0.0);
                    int num2 = num;
                    int num3 = num2;
                    if (num3 == -2)
                    {
                        return -2;
                    }
                    if (num3 == 1)
                    {
                        return -3;
                    }
                    bool flag3 = CommonChrome.IsCheckpoint(chrome);
                    if (flag3)
                    {
                        return -1;
                    }
                }
            }
            catch
            {
            }
            return 0;
        }
        public static string GetWebsiteFacebook(Chrome chrome, int typeWeb)
        {
            string url = "";
            switch (typeWeb)
            {
                case 1:
                    url = "https://www.facebook.com";
                    break;
                case 2:
                    url = "https://m.facebook.com";
                    break;
                case 3:
                    url = "https://mbasic.facebook.com";
                    break;
            }
            string url2 = chrome.GetURL();
            bool flag = CommonChrome.CheckTypeWebFacebookFromUrl(url2) != typeWeb;
            if (flag)
            {
                chrome.GotoURL(url);
            }
            return Regex.Match(chrome.GetURL(), "https://(.*?)facebook.com").Value;
        }
        public static int GoToFriendSuggest(Chrome chrome)
        {
            try
            {
                bool flag = chrome != null;
                if (flag)
                {
                    bool flag2 = chrome.CheckChromeClosed();
                    if (flag2)
                    {
                        return -2;
                    }
                    bool flag3 = chrome.CheckExistElement("#requests_jewel a", 0.0) == 1;
                    if (flag3)
                    {
                        chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('#requests_jewel a')");
                        chrome.DelayThaoTacNho(0, null);
                    }
                    int num = chrome.Click(4, "#requests_jewel a", 0, 0, "", 0, 1);
                    bool flag4 = num != 1;
                    if (flag4)
                    {
                        CommonChrome.GoToHome(chrome);
                        chrome.DelayThaoTacNho(2, null);
                        num = chrome.Click(4, "#requests_jewel a", 0, 0, "", 0, 1);
                    }
                    bool flag5 = num == 1;
                    if (flag5)
                    {
                        chrome.DelayThaoTacNho(1, null);
                        bool flag6 = chrome.Click(4, "[href=\"/friends/center/suggestions/?mff_nav=1&ref=bookmarks\"]", 0, 0, "", 0, 1) == 1;
                        if (flag6)
                        {
                            chrome.DelayThaoTacNho(0, null);
                            return 1;
                        }
                    }
                    return chrome.GotoURL("https://m.facebook.com/friends/center/suggestions/?mff_nav=1&ref=bookmarks");
                }
            }
            catch
            {
            }
            return 0;
        }
        public static bool SkipNotifyWhenAddFriend(Chrome chrome)
        {
            bool result = true;
            string text = "";
            switch (chrome.CheckExistElements(2.0, new string[]
            {
                "[data-sigil=\" m-overlay-layer\"] button",
                "[data-sigil=\" m-overlay-layer\"] [value=\"OK\"]",
                "[data-sigil=\"touchable m-error-overlay-done\"]",
                "[data-sigil=\"touchable m-overlay-layer\"]",
                "[data-sigil=\"touchable m-error-overlay-cancel\"]"
            }))
            {
                case 0:
                    result = false;
                    break;
                case 1:
                    text = "[data-sigil=\" m-overlay-layer\"] button";
                    break;
                case 2:
                    text = "[data-sigil=\" m-overlay-layer\"] [value=\"OK\"]";
                    break;
                case 3:
                    text = "[data-sigil=\"touchable m-error-overlay-done\"]";
                    break;
                case 4:
                    text = "[data-sigil=\"touchable m-overlay-layer\"]";
                    break;
                case 5:
                    text = "[data-sigil=\"touchable m-error-overlay-cancel\"]";
                    break;
            }
            bool flag = text != "";
            if (flag)
            {
                chrome.ExecuteScript("document.querySelector('" + text + "').click();");
            }
            return result;
        }

        //public static string GetTokenEAAB(Chrome chrome)
        //{
        //    string result = "";
        //    try
        //    {
        //        chrome.GotoURL("https://adsmanager.facebook.com/adsmanager/");
        //        chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Cua Toolkit<br>Đang Thao Tác...</b>'; document.querySelector('body').style = 'font-size:18px; color:red;text-align: center; background-color:#fff'");
        //        chrome.DelayTime(2.0);
        //        // get token 
        //        object body = chrome.ExecuteScript(@"function getTokenEAAB() { let tokens = window.__accessToken; if (tokens) { return tokens; } else { return '';  } } return getTokenEAAB();");
        //        string token = body.ToString();
        //        result = token;
        //    }
        //    catch
        //    {
        //    }
        //    return result;
        //}

        public static string GetHostFacebook(Chrome chrome, int typeWeb = 2)
        {
            try
            {
                string url = chrome.GetURL();
                bool flag = CommonChrome.CheckTypeWebFacebookFromUrl(url) != typeWeb;
                if (flag)
                {
                    switch (typeWeb)
                    {
                        case 1:
                            chrome.GotoURL("https://www.facebook.com");
                            break;
                        case 2:
                            chrome.GotoURL("https://m.facebook.com");
                            break;
                        case 3:
                            chrome.GotoURL("https://mbasic.facebook.com");
                            break;
                    }
                }
                return Regex.Match(chrome.GetURL(), "https://(.*?)facebook.com").Value;
            }
            catch
            {
            }
            return "";
        }

        public static string RequestPost(Chrome chrome, string url, string data, string website, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                bool flag = !chrome.GetURL().StartsWith(website);
                if (flag)
                {
                    chrome.GotoURL(website);
                    chrome.DelayTime(1.0);
                    chrome.ExecuteScript("document.querySelector('body').innerHTML='CuaDev Running...'; document.querySelector('body').style = 'text-align: center; background-color:#fff'");
                }
                chrome.DelayTime(1.0);
                data = data.Replace("\n", "\\n").Replace("\"", "\\\"");
                return (string)chrome.ExecuteScript(string.Concat(new string[]
                {
                    "async function RequestPost() { var output = ''; try { var response = await fetch('",
                    url,
                    "', { method: 'POST', body: '",
                    data,
                    "', headers: { 'Content-Type': '",
                    contentType,
                    "' } }); if (response.ok) { var body = await response.text(); return body; } } catch {} return output; }; var c = await RequestPost(); return c;"
                }));
            }
            catch
            {
            }
            return "";
        }

        public static string GetInfoAccountFromUidUsingToken(Chrome chrome, string token)
        {
            bool flag = false;
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            string text6 = "";
            string text7 = "";
            try
            {
                string json = CommonChrome.RequestGet(chrome, "https://graph.facebook.com/v2.11/me?fields=name,email,gender,birthday&access_token=" + token, "https://graph.facebook.com/");
                JObject jobject = JObject.Parse(json);
                flag = true;
                text = jobject["name"].ToString();
                try
                {
                    text2 = jobject["gender"].ToString();
                }
                catch
                {
                }
                try
                {
                    text3 = jobject["birthday"].ToString();
                }
                catch
                {
                }
                try
                {
                    text5 = jobject["email"].ToString();
                }
                catch
                {
                }
            }
            catch
            {
            }
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", new object[]
            {
                flag,
                text,
                text2,
                text3,
                text4,
                text5,
                text6,
                text7
            });
        }

        public static string GetInfoAccountFromUidUsingCookie(Chrome chrome)
        {
            bool flag = false;
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            string text6 = "";
            string text7 = "";
            string text8 = "";
            string text9 = "";
            string text10 = "";
            try
            {
                string cookieFromChrome = chrome.GetCookieFromChrome("facebook");
                string value = Regex.Match(cookieFromChrome + ";", "c_user=(.*?);").Groups[1].Value;
                string text11 = CommonChrome.RequestGet(chrome, CommonChrome.GetHostFacebook(chrome, 3) + "/friends/center/friends/?mff_nav=1", CommonChrome.GetHostFacebook(chrome, 3));
                text6 = Regex.Match(text11, "bm bn\">(.*?)<").Groups[1].Value.Replace(",", "").Replace(".", "");
                bool flag2 = text6 == "";
                if (flag2)
                {
                    text6 = Regex.Match(text11, "bm\">(.*?)<").Groups[1].Value.Replace(",", "").Replace(".", "");
                }
                text6 = Regex.Match(text6, "\\d+").Value;
                text11 = CommonChrome.RequestGet(chrome, CommonChrome.GetHostFacebook(chrome, 3) + "/groups/?seemore&refid=27", CommonChrome.GetHostFacebook(chrome, 3));
                text7 = Regex.Matches(text11, "class=\"bl\"").Count.ToString();
                bool flag3 = text7 == "0";
                if (flag3)
                {
                    text7 = Regex.Matches(text11, "class=\"bm\"").Count.ToString();
                }
                try
                {
                    text11 = CommonChrome.RequestGet(chrome, CommonChrome.GetHostFacebook(chrome, 2) + "/composer/ocelot/async_loader/?publisher=feed", CommonChrome.GetHostFacebook(chrome, 2));
                    string value2 = Regex.Match(text11, "fb_dtsg\\\\\" value=\\\\\"(.*?)\\\\\"").Groups[1].Value;
                    text8 = Regex.Match(text11, "EAAA(.*?)\"").Value.TrimEnd(new char[]
                    {
                        '"',
                        '\\'
                    });
                    string data = string.Concat(new string[]
                    {
                        "av=",
                        value,
                        "&__user=",
                        value,
                        "&__a=1&__dyn=&__csr=&__req=y&__hs=18794.EXP2%3Acomet_pkg.2.1.0.0&dpr=1&__ccg=EXCELLENT&__rev=1003974565&__s=zbue97%3A5iciql%3Abxnvge&__hsi=6974199735511561326-0&__comet_req=1&fb_dtsg=",
                        value2,
                        "&jazoest=22414&lsd=&__spin_r=1003974565&__spin_b=trunk&__spin_t=1623807413&fb_api_caller_class=RelayModern&fb_api_req_friendly_name=PrivacyAccessHubYourInformationSectionQuery&variables=%7B%7D&server_timestamps=true&doc_id=3200030856767767"
                    });
                    text11 = CommonChrome.RequestPost(chrome, CommonChrome.GetHostFacebook(chrome, 1) + "/api/graphql/", data, CommonChrome.GetHostFacebook(chrome, 1) + "/api/graphql/", "application/x-www-form-urlencoded");
                    JObject jobject = JObject.Parse(text11);
                    text9 = jobject["data"]["section"]["tiles"][1]["links"][0]["non_link_content"]["metadata"].ToString();
                }
                catch
                {
                }
                string infoAccountFromUidUsingToken = CommonChrome.GetInfoAccountFromUidUsingToken(chrome, text8);
                string[] array = infoAccountFromUidUsingToken.Split(new char[]
                {
                    '|'
                });
                text = array[1];
                text2 = array[2];
                text3 = array[3];
                text5 = array[5];
                bool flag4 = text10 == "";
                if (flag4)
                {
                    text10 = "0";
                }
                bool flag5 = text6 == "";
                if (flag5)
                {
                    text6 = "0";
                }
                bool flag6 = text7 == "";
                if (flag6)
                {
                    text7 = "0";
                }
            }
            catch (Exception ex)
            {
                bool flag7 = CommonChrome.CheckLiveCookie(chrome, "https://m.facebook.com/") == 0;
                if (flag7)
                {
                    return "-1";
                }
                CommonCSharp.ExportError(null, ex.ToString());
            }
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", new object[]
            {
                flag,
                text,
                text2,
                text3,
                text4,
                text5,
                text6,
                text7,
                text8,
                text9,
                text10
            });
        }

    }
}

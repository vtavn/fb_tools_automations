using Facebook_Tool_Request.core;
using Facebook_Tool_Request.Properties;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;
    using core;
    using core.Enum;
    using Newtonsoft.Json.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class Chrome
    {
        public int IndexChrome;

        private Random rd;

        private JSON_Settings settings;

        public Process process { get; set; }

        public ChromeDriver chrome { get; set; }

        public bool HideBrowser { get; set; }

        public bool Incognito { get; set; }

        public bool DisableImage { get; set; }

        public bool DisableSound { get; set; }

        public bool AutoPlayVideo { get; set; }

        public string UserAgent { get; set; }

        public int PixelRatio { get; set; }

        public string ProfilePath { get; set; }

        public Point Size { get; set; }

        public Point Position { get; set; }

        public int TimeWaitForSearchingElement { get; set; }

        public int TimeWaitForLoadingPage { get; set; }

        public string Proxy { get; set; }

        public int TypeProxy { get; set; }

        public string App { get; set; }

        public string LinkToOtherBrowser { get; set; }

        public string PathExtension { get; set; }

        public bool IsUseEmulator { get; set; }

        public bool IsUsePortable { get; set; }

        public string PathToPortableZip { get; set; }

        public Point Size_Emulator { get; set; }

        public StatusChromeAccount Status { get; set; }

        public bool scaleChorme { get; set; }

        public Chrome()
        {
            IndexChrome = 0;
            HideBrowser = false;
            DisableImage = false;
            DisableSound = false;
            Incognito = false;
            scaleChorme = true;
            UserAgent = "";
            ProfilePath = "";
            Size = new Point(300, 300);
            Size = new Point(Size.X, Size.Y);
            Proxy = "";
            TypeProxy = 0;
            Position = new Point(Position.X, Position.Y);
            TimeWaitForSearchingElement = 0;
            TimeWaitForLoadingPage = 1;
            App = "";
            AutoPlayVideo = false;
            LinkToOtherBrowser = "";
            PathExtension = "data\\extension";
            IsUseEmulator = false;
            Size_Emulator = new Point(300, 300);
            Status = StatusChromeAccount.Empty;
            IsUsePortable = false;
            PathToPortableZip = "";
            rd = new Random();
        }

        public bool Open(bool isGetUseragentDefault = false)
        {
            this.settings = new JSON_Settings("configGeneral", false);

            bool result = false;
            try
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                chromeDriverService.DisableBuildCheck = true;
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--disable-3d-apis", "--disable-background-networking", "--disable-bundled-ppapi-flash", "--disable-client-side-phishing-detection", "--disable-default-apps", "--disable-hang-monitor", "--disable-prompt-on-repost", "--disable-sync", "--disable-webgl", "--enable-blink-features=ShadowDOMV0", "--enable-logging", "--disable-notifications", "--window-size=" + Size.X + "," + Size.Y, "--window-position=" + Position.X + "," + Position.Y, "--no-sandbox", "--disable-gpu", "--disable-dev-shm-usage", "--disable-web-security", "--disable-rtc-smoothness-algorithm", "--disable-webrtc-hw-decoding", "--disable-webrtc-hw-encoding", "--disable-webrtc-multiple-routes", "--disable-webrtc-hw-vp8-encoding", "--enforce-webrtc-ip-permission-check", "--force-webrtc-ip-handling-policy", "--ignore-certificate-errors", "--disable-infobars", "--disable-blink-features=\"BlockCredentialedSubresources\"", "--disable-popup-blocking", "disable-blink-features=BlockCredentialedSubresources", "--no-first-run", "no-default-browser-check");
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 0);
                if (DisableSound)
                {
                    chromeOptions.AddArgument("--mute-audio");
                }
                chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                chromeOptions.AddUserProfilePreference("useAutomationExtension", false);
                chromeOptions.AddExcludedArgument("enable-automation");
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);

                if (scaleChorme)
                {
                    chromeOptions.AddArgument(string.Format("--force-device-scale-factor={0}", Convert.ToDouble(this.settings.GetValue("txtScaleChorme", "0.7"))));
                    chromeOptions.AddArgument(string.Format("high-dpi-support={0}", Convert.ToDouble(this.settings.GetValue("txtScaleChorme", "0.7"))));

                }
                if (LinkToOtherBrowser != "" && File.Exists(LinkToOtherBrowser))
                {
                    chromeOptions.BinaryLocation = LinkToOtherBrowser;
                }
                if (IsUsePortable)
                {
                    if (!string.IsNullOrEmpty(ProfilePath.Trim()))
                    {
                        if (!Directory.Exists(ProfilePath))
                        {
                            ZipFile.ExtractToDirectory(PathToPortableZip, ProfilePath);
                        }
                        chromeOptions.BinaryLocation = ProfilePath + "\\App\\Chrome-bin\\chrome.exe";
                        if (!HideBrowser)
                        {
                            if (DisableImage)
                            {
                                chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
                            }
                            chromeOptions.AddArgument("--user-data-dir=" + ProfilePath + "\\Data\\profile");
                        }
                        else
                        {
                            chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
                            chromeOptions.AddArgument("--headless=new");
                        }
                    }
                }
                else if (!HideBrowser)
                {
                    if (DisableImage)
                    {
                        chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
                    }
                    if (!string.IsNullOrEmpty(ProfilePath.Trim()))
                    {
                        try
                        {
                            chromeOptions.AddArgument("--user-data-dir=" + ProfilePath);
                            File.Delete(ProfilePath + "\\Default\\Secure Preferences");
                            string json = File.ReadAllText(ProfilePath + "\\Default\\Preferences");
                            JObject jObject = JObject.Parse(json);
                            jObject["profile"]["exit_type"] = "none";
                            File.WriteAllText(ProfilePath + "\\Default\\Preferences", jObject.ToString());
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
                    chromeOptions.AddArgument("--headless");
                }
                if (Incognito)
                {
                    chromeOptions.AddArguments("--incognito");
                }
                if (!string.IsNullOrEmpty(Proxy.Trim()))
                {
                    string pxx = Proxy;
                    int prx = Proxy.Split(':').Count();
                    switch (Proxy.Split(':').Count())
                    {
                        case 1:
                            chromeOptions.AddArgument("--proxy-server= " + ((this.TypeProxy == 0) ? "" : "socks5://") + "127.0.0.1:" + this.Proxy);
                            break;
                        case 2:
                            if (TypeProxy == 0)
                            {
                                chromeOptions.AddArgument("--proxy-server= " + Proxy);
                            }
                            else
                            {
                                chromeOptions.AddArgument("--proxy-server= socks5://" + Proxy);
                            }
                            break;
                        case 4:
                            chromeOptions.AddArgument(string.Concat(new string[]
                            {
                                "--proxy-server= ",
                                (this.TypeProxy == 0) ? "" : "socks5://",
                                this.Proxy.Split(new char[]
                                {
                                    ':'
                                })[0],
                                ":",
                                this.Proxy.Split(new char[]
                                {
                                    ':'
                                })[1]
                            }));
                            //chromeOptions.AddExtension("extension\\proxy1.crx");
                            break;
                    }
                }
                if (!isGetUseragentDefault)
                {
                    chromeOptions.AddArgument("--user-agent=" + UserAgent);
                }
                
                //bool checkhidebrowser = !this.HideBrowser;
                //if (checkhidebrowser)
                //{
                //    this.GetProcess();
                //}

                bool checkpathext = this.PathExtension.Trim() != "";
                if (checkpathext)
                {
                    bool flag11 = !Directory.Exists(this.PathExtension);
                    if (flag11)
                    {
                        Directory.CreateDirectory(this.PathExtension);
                    }
                    List<string> files = Common.GetFiles(this.PathExtension);
                    bool flag12 = files.Count > 0;
                    if (flag12)
                    {
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        chromeOptions.AddExtension(files[i]);
                    }
                }

                if (!string.IsNullOrEmpty(App.Trim()))
                {
                    chromeOptions.AddArgument("--app=" + App);
                }
                if (IsUseEmulator)
                {
                    //ChromeMobileEmulationDeviceSettings deviceSettings = new ChromeMobileEmulationDeviceSettings
                    //{
                    //    EnableTouchEvents = true,
                    //    Width = Size_Emulator.X,
                    //    Height = Size_Emulator.Y,
                    //    UserAgent = UserAgent,
                    //    PixelRatio = PixelRatio
                    //};
                    //chromeOptions.EnableMobileEmulation(deviceSettings);
                }
                if (AutoPlayVideo)
                {
                    chromeOptions.AddArgument("--autoplay-policy=no-user-gesture-required");
                }
                chrome = new ChromeDriver(chromeDriverService, chromeOptions);
                chrome.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TimeWaitForSearchingElement);
                chrome.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(TimeWaitForLoadingPage);
                string p = Settings.Default.PathChrome;

                int a = 0;
                if (Settings.Default.PathChrome == "")
                {
                    chrome.Close();
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                bool flag17 = e.ToString().Contains("session timed out after");
                if (flag17)
                {
                    Common.KillProcess("chrome");
                    Common.KillProcess("chromedriver");
                }
                Chrome.ExportError(null, e, "chrome.Open()");
            }
            return result;
        }

        public string GetPageSource()
        {
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                return chrome.PageSource;
            }
            catch (Exception)
            {
            }
            return "";
        }

        public bool CheckIsLive()
        {
            return !CheckChromeClosed();
        }

        public bool CheckChromeClosed()
        {
            if (process != null)
            {
                return process.HasExited;
            }
            bool result = true;
            try
            {
                string title = chrome.Title;
                result = false;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CheckChromeClosed()");
            }
            return result;
        }

        public bool CreateShortcut(string shortcutName, string shortcutPath, string icon = "", string targetFileLocation = "\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\"")
        {
            bool result = false;
            try
            {
                Helpers.Common.CreateShortcut(shortcutName, shortcutPath, targetFileLocation, "--user-data-dir=\"" + ProfilePath + "\"", targetFileLocation.Substring(0, targetFileLocation.LastIndexOf("\\")), icon);
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CreateShortcut(" + shortcutName + "," + shortcutPath + "," + targetFileLocation + ")");
            }
            return result;
        }

        public string GetCssSelector(string querySelector, string attributeName, string attributeValue)
        {
            string result = "";
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                result = chrome.ExecuteScript("function GetSelector(el){let path=[],parent;while(parent=el.parentNode){path.unshift(`${el.tagName}:nth-child(${[].indexOf.call(parent.children, el)+1})`);el=parent}return `${path.join('>')}`.toLowerCase()}; function GetCssSelector(selector, attribute, value){var c = document.querySelectorAll(selector); for (i = 0; i < c.length; i++) { if (c[i].getAttribute(attribute)!=null && c[i].getAttribute(attribute).includes(value)) { return GetSelector(c[i])} }; return '';}; return GetCssSelector('" + querySelector + "','" + attributeName + "','" + attributeValue + "')").ToString();
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GetCssSelector(" + querySelector + "," + attributeName + "," + attributeValue + ")");
            }
            return result;
        }

        public string GetCssSelector(string querySelector)
        {
            //Discarded unreachable code: IL_00a4, IL_00ee
            string result = "";
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                result = chrome.ExecuteScript("function GetSelector(el){let path=[],parent;while(parent=el.parentNode){path.unshift(`${el.tagName}:nth-child(${[].indexOf.call(parent.children, el)+1})`);el=parent}return `${path.join('>')}`.toLowerCase()}; return GetSelector(" + querySelector + ")").ToString();
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GetCssSelector(" + querySelector + ")");
            }
            return result;
        }

        public string GetAttributeValue(string querySelector, string attributeName)
        {
            //Discarded unreachable code: IL_00ec, IL_017e
            string result = "";
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                result = chrome.ExecuteScript("return document.querySelector('" + querySelector + "').getAttribute('" + attributeName + "')").ToString();
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GetAttributeValue(" + querySelector + "," + attributeName + ")");
            }
            return result;
        }

        public int ScrollSmooth(int distance)
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int num = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString());
                chrome.ExecuteScript("window.scrollBy({ top: " + distance + ",behavior: 'smooth'});");
                DelayTime(0.1);
                if (num == Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('html').getBoundingClientRect().y+''").ToString()))
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.ScrollSmooth({distance})");
            }
            return 1;
        }

        public string GetUseragent()
        {
            string result = "";
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                result = chrome.ExecuteScript("return navigator.userAgent").ToString();
            }
            catch
            {
            }
            return result;
        }

        public int SendKeyDown(int typeAttribute, string attributeValue)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).SendKeys(Keys.ArrowDown);
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).SendKeys(Keys.ArrowDown);
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).SendKeys(Keys.ArrowDown);
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(Keys.ArrowDown);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeyDown({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeyEnd(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0, int times = 1)
        {
            //Discarded unreachable code: IL_02be, IL_0300
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            for (int i = 0; i < times; DelayTime(1.0), i++)
            {
                try
                {
                    if (subTypeAttribute == 0)
                    {
                        switch (typeAttribute)
                        {
                            case 1:
                                chrome.FindElements(By.Id(attributeValue))[index].SendKeys(Keys.End);
                                break;
                            case 2:
                                chrome.FindElements(By.Name(attributeValue))[index].SendKeys(Keys.End);
                                break;
                            case 3:
                                chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(Keys.End);
                                break;
                            case 4:
                                chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(Keys.End);
                                break;
                        }
                    }
                    else
                    {
                        switch (typeAttribute)
                        {
                            case 1:
                                chrome.FindElements(By.Id(attributeValue))[index]
                                    .FindElements(By.Id(subAttributeValue))[subIndex].SendKeys(Keys.End);
                                break;
                            case 2:
                                chrome.FindElements(By.Name(attributeValue))[index]
                                    .FindElements(By.Name(subAttributeValue))[subIndex].SendKeys(Keys.End);
                                break;
                            case 3:
                                chrome.FindElements(By.XPath(attributeValue))[index]
                                    .FindElements(By.XPath(subAttributeValue))[subIndex].SendKeys(Keys.End);
                                break;
                            case 4:
                                chrome.FindElements(By.CssSelector(attributeValue))[index]
                                    .FindElements(By.CssSelector(subAttributeValue))[subIndex].SendKeys(Keys.End);
                                break;
                        }
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    ExportError(null, ex, $"chrome.End({typeAttribute},{attributeValue})");
                    continue;
                }
                break;
            }
            return flag ? 1 : 0;
        }

        public string GetURL()
        {
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                return chrome.Url;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GetURL()");
            }
            return "";
        }

        public int GotoURL(string url)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.Navigate().GoToUrl(url);
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GotoURL(" + url + ")");
            }
            return flag ? 1 : 0;
        }

        public int GotoURLIfNotExist(string url)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (GetURL() != url)
                {
                    chrome.Navigate().GoToUrl(url);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GotoURL(" + url + ")");
            }
            return flag ? 1 : 0;
        }

        public int Refresh()
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.Navigate().Refresh();
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.Refresh()");
            }
            return flag ? 1 : 0;
        }

        public int GotoBackPage(int times = 1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                for (int i = 0; i < times; i++)
                {
                    chrome.Navigate().Back();
                    DelayTime(0.5);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GotoBackPage()");
            }
            return flag ? 1 : 0;
        }

        public int HoverElement(int typeAttribute, string attributeValue, int index, double timeHover_second)
        {
            //Discarded unreachable code: IL_01aa, IL_01f7
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                WebDriverWait webDriverWait = new WebDriverWait(chrome, TimeSpan.FromSeconds(10.0));
                switch (typeAttribute)
                {
                    case 1:
                        new Actions(chrome).MoveToElement(chrome.FindElement(By.Id(attributeValue))).Perform();
                        break;
                    case 2:
                        new Actions(chrome).MoveToElement(chrome.FindElement(By.Name(attributeValue))).Perform();
                        break;
                    case 3:
                        new Actions(chrome).MoveToElement(chrome.FindElement(By.XPath(attributeValue))).Perform();
                        break;
                    case 4:
                        new Actions(chrome).MoveToElement(chrome.FindElement(By.CssSelector(attributeValue))).Perform();
                        break;
                }
                Thread.Sleep(Convert.ToInt32(timeHover_second * 1000.0));
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.HoverElement({typeAttribute}, {attributeValue}, {timeHover_second})");
            }
            return flag ? 1 : 0;
        }

        public int HoverElement(int typeAttribute, string attributeValue, double timeHover_second)
        {
            //Discarded unreachable code: IL_017f, IL_01cc
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                WebDriverWait webDriverWait = new WebDriverWait(chrome, TimeSpan.FromSeconds(10.0));
                switch (typeAttribute)
                {
                    case 1:
                        new Actions(chrome).MoveToElement(webDriverWait.Until(c => c.FindElement(By.Id(attributeValue)))).Perform();
                        break;
                    case 2:
                        new Actions(chrome).MoveToElement(webDriverWait.Until(c => c.FindElement(By.Name(attributeValue)))).Perform();
                        break;
                    case 3:
                        new Actions(chrome).MoveToElement(webDriverWait.Until(c => c.FindElement(By.XPath(attributeValue)))).Perform();
                        break;
                    case 4:
                        new Actions(chrome).MoveToElement(webDriverWait.Until(c => c.FindElement(By.CssSelector(attributeValue)))).Perform();
                        break;
                }
                Thread.Sleep(Convert.ToInt32(timeHover_second * 1000.0));
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.HoverElement({typeAttribute}, {attributeValue}, {timeHover_second})");
            }
            return flag ? 1 : 0;
        }

        public bool MoveToElement(int typeAttribute, string attributeValue, int index)
        {
            //Discarded unreachable code: IL_015b, IL_01a6
            bool result = true;
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        new Actions(chrome).MoveToElement(chrome.FindElements(By.Id(attributeValue))[index]).Build().Perform();
                        break;
                    case 2:
                        new Actions(chrome).MoveToElement(chrome.FindElements(By.Name(attributeValue))[index]).Build().Perform();
                        break;
                    case 3:
                        new Actions(chrome).MoveToElement(chrome.FindElements(By.XPath(attributeValue))[index]).Build().Perform();
                        break;
                    case 4:
                        new Actions(chrome).MoveToElement(chrome.FindElements(By.CssSelector(attributeValue))[index]).Build().Perform();
                        break;
                }
                result = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.MoveToElement({typeAttribute},{attributeValue},{index})");
            }
            return result;
        }

        public object ExecuteScript(string script)
        {
            //Discarded unreachable code: IL_004e, IL_0098
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                return chrome.ExecuteScript(script);
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.ExecuteScript(" + script + ")");
            }
            return "";
        }

        public Image CaptureWindow(IntPtr handle)
        {
            IntPtr windowDC = User32.GetWindowDC(handle);
            User32.RECT rect = default(User32.RECT);
            User32.GetWindowRect(handle, ref rect);
            int nWidth = rect.right - rect.left;
            int nHeight = rect.bottom - rect.top;
            IntPtr intPtr = GDI32.CreateCompatibleDC(windowDC);
            IntPtr intPtr2 = GDI32.CreateCompatibleBitmap(windowDC, nWidth, nHeight);
            IntPtr hObject = GDI32.SelectObject(intPtr, intPtr2);
            GDI32.BitBlt(intPtr, 0, 0, nWidth, nHeight, windowDC, 0, 0, 13369376);
            GDI32.SelectObject(intPtr, hObject);
            GDI32.DeleteDC(intPtr);
            User32.ReleaseDC(handle, windowDC);
            Image result = Image.FromHbitmap(intPtr2);
            GDI32.DeleteObject(intPtr2);
            return result;
        }

        public Bitmap ScreenCapture(int count = 1)
        {
            Bitmap result = null;
            try
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        result = (Bitmap)CaptureWindow(process.MainWindowHandle);
                    }
                    catch (Exception ex)
                    {
                        ExportError(this, ex, "CaptureWindow");
                        Helpers.Common.DelayTime(1.0);
                        continue;
                    }
                    break;
                }
            }
            catch (Exception ex2)
            {
                ExportError(null, ex2, "AutoChrome.ScreenCapture()");
            }
            return result;
        }

        public bool CheckExistImage(string ImagePath, int timeSearchImage_Second = 0)
        {
            if (FindImage(ImagePath, timeSearchImage_Second).HasValue)
            {
                return true;
            }
            return false;
        }

        public Point? FindImage(string ImagePath, int timeSearchImage_Second = 0)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(ImagePath);
            FileInfo[] files = directoryInfo.GetFiles();
            int tickCount = Environment.TickCount;
            do
            {
                Bitmap bitmap = ScreenCapture(3);
                if (bitmap == null)
                {
                    break;
                }
                Point? point = null;
                FileInfo[] array = files;
                foreach (FileInfo fileInfo in array)
                {
                    Bitmap subBitmap = (Bitmap)Image.FromFile(fileInfo.FullName);
                    point = ImageScanOpenCV.FindOutPoint(bitmap, subBitmap);
                    if (point.HasValue)
                    {
                        int x = point.Value.X;
                        int y = point.Value.Y;
                        return new Point(x, y);
                    }
                }
                Helpers.Common.DelayTime(1.0);
            }
            while (Environment.TickCount - tickCount <= timeSearchImage_Second * 1000);
            return null;
        }

        public int Click(string ImagePath, int timeSearchImage_Second = 0)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                Point? point = FindImage(ImagePath, timeSearchImage_Second);
                if (point.HasValue)
                {
                    AutoControl.SendClickOnPosition(process.MainWindowHandle, point.Value.X, point.Value.Y);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }

        public int Click(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0, int times = 1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            for (int i = 0; i < times; DelayTime(1.0), i++)
            {
                try
                {
                    if (subTypeAttribute == 0)
                    {
                        switch (typeAttribute)
                        {
                            case 1:
                                chrome.FindElements(By.Id(attributeValue))[index].Click();
                                break;
                            case 2:
                                chrome.FindElements(By.Name(attributeValue))[index].Click();
                                break;
                            case 3:
                                chrome.FindElements(By.XPath(attributeValue))[index].Click();
                                break;
                            case 4:
                                chrome.FindElements(By.CssSelector(attributeValue))[index].Click();
                                break;
                        }
                    }
                    else
                    {
                        switch (typeAttribute)
                        {
                            case 1:
                                chrome.FindElements(By.Id(attributeValue))[index]
                                    .FindElements(By.Id(subAttributeValue))[subIndex].Click();
                                break;
                            case 2:
                                chrome.FindElements(By.Name(attributeValue))[index]
                                    .FindElements(By.Name(subAttributeValue))[subIndex].Click();
                                break;
                            case 3:
                                chrome.FindElements(By.XPath(attributeValue))[index]
                                    .FindElements(By.XPath(subAttributeValue))[subIndex].Click();
                                break;
                            case 4:
                                chrome.FindElements(By.CssSelector(attributeValue))[index]
                                    .FindElements(By.CssSelector(subAttributeValue))[subIndex].Click();
                                break;
                        }
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    ExportError(null, ex, $"chrome.Click({typeAttribute},{attributeValue})");
                    continue;
                }
                break;
            }
            return flag ? 1 : 0;
        }

        public int FindAndClick(double timeWait_Second, int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    try
                    {
                        if (subTypeAttribute == 0)
                        {
                            switch (typeAttribute)
                            {
                                case 1:
                                    chrome.FindElement(By.Id(attributeValue)).Click();
                                    break;
                                case 2:
                                    chrome.FindElement(By.Name(attributeValue)).Click();
                                    break;
                                case 3:
                                    chrome.FindElement(By.XPath(attributeValue)).Click();
                                    break;
                                case 4:
                                    chrome.FindElement(By.CssSelector(attributeValue)).Click();
                                    break;
                            }
                        }
                        else
                        {
                            switch (typeAttribute)
                            {
                                case 1:
                                    chrome.FindElements(By.Id(attributeValue))[index]
                                        .FindElements(By.Id(subAttributeValue))[subIndex].Click();
                                    break;
                                case 2:
                                    chrome.FindElements(By.Name(attributeValue))[index]
                                        .FindElements(By.Name(subAttributeValue))[subIndex].Click();
                                    break;
                                case 3:
                                    chrome.FindElements(By.XPath(attributeValue))[index]
                                        .FindElements(By.XPath(subAttributeValue))[subIndex].Click();
                                    break;
                                case 4:
                                    chrome.FindElements(By.CssSelector(attributeValue))[index]
                                        .FindElements(By.CssSelector(subAttributeValue))[subIndex].Click();
                                    break;
                            }
                        }
                        flag = true;
                        DelayTime(1.0);
                    }
                    catch (Exception)
                    {
                        goto IL_02be;
                    }
                    break;
                IL_02be:
                    if ((double)(Environment.TickCount - tickCount) >= timeWait_Second * 1000.0)
                    {
                        break;
                    }
                    DelayTime(1.0);
                }
            }
            catch (Exception ex2)
            {
                ExportError(null, ex2, $"chrome.FindAndClick({timeWait_Second},{typeAttribute},{attributeValue},{index},{subTypeAttribute},{subAttributeValue},{subIndex}");
            }
            return flag ? 1 : 0;
        }

        public int ClickWithAction(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (subTypeAttribute != 0)
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            new Actions(chrome).Click(chrome.FindElements(By.Id(attributeValue))[index]
                                .FindElements(By.Id(subAttributeValue))[subIndex]).Perform();
                            break;
                        case 2:
                            new Actions(chrome).Click(chrome.FindElements(By.Name(attributeValue))[index]
                                .FindElements(By.Name(subAttributeValue))[subIndex]).Perform();
                            break;
                        case 3:
                            new Actions(chrome).Click(chrome.FindElements(By.XPath(attributeValue))[index]
                                .FindElements(By.XPath(subAttributeValue))[subIndex]).Perform();
                            break;
                        case 4:
                            new Actions(chrome).Click(chrome.FindElements(By.CssSelector(attributeValue))[index]
                                .FindElements(By.CssSelector(subAttributeValue))[subIndex]).Perform();
                            break;
                    }
                }
                else
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            new Actions(chrome).Click(chrome.FindElements(By.Id(attributeValue))[index]).Perform();
                            break;
                        case 2:
                            new Actions(chrome).Click(chrome.FindElements(By.Name(attributeValue))[index]).Perform();
                            break;
                        case 3:
                            new Actions(chrome).Click(chrome.FindElements(By.XPath(attributeValue))[index]).Perform();
                            break;
                        case 4:
                            new Actions(chrome).Click(chrome.FindElements(By.CssSelector(attributeValue))[index]).Perform();
                            break;
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.ClickWithAction({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeys(int typeAttribute, string attributeValue, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    DelayTime(timeDelayAfterClick);
                }
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).SendKeys(content);
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).SendKeys(content);
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).SendKeys(content);
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(content);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeys(int typeAttribute, string attributeValue, int index, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    DelayTime(timeDelayAfterClick);
                }
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElements(By.Id(attributeValue))[index].SendKeys(content);
                        break;
                    case 2:
                        chrome.FindElements(By.Name(attributeValue))[index].SendKeys(content);
                        break;
                    case 3:
                        chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(content);
                        break;
                    case 4:
                        chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(content);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeys(int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    DelayTime(timeDelayAfterClick);
                }
                for (int i = 0; i < content.Length; i++)
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElement(By.Id(attributeValue)).SendKeys(content[i].ToString());
                            break;
                        case 2:
                            chrome.FindElement(By.Name(attributeValue)).SendKeys(content[i].ToString());
                            break;
                        case 3:
                            chrome.FindElement(By.XPath(attributeValue)).SendKeys(content[i].ToString());
                            break;
                        case 4:
                            chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(content[i].ToString());
                            break;
                    }
                    if (timeDelay_Second > 0.0)
                    {
                        int num = Convert.ToInt32(timeDelay_Second * 1000.0);
                        if (num < 100)
                        {
                            num = 100;
                        }
                        Thread.Sleep(Base.rd.Next(num, num + 50));
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{timeDelay_Second},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeys(int typeAttribute, string attributeValue, int index, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    DelayTime(timeDelayAfterClick);
                }
                for (int i = 0; i < content.Length; i++)
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElements(By.Id(attributeValue))[index].SendKeys(content[i].ToString());
                            break;
                        case 2:
                            chrome.FindElements(By.Name(attributeValue))[index].SendKeys(content[i].ToString());
                            break;
                        case 3:
                            chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(content[i].ToString());
                            break;
                        case 4:
                            chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(content[i].ToString());
                            break;
                    }
                    if (timeDelay_Second > 0.0)
                    {
                        int num = Convert.ToInt32(timeDelay_Second * 1000.0);
                        if (num < 100)
                        {
                            num = 100;
                        }
                        Thread.Sleep(Base.rd.Next(num, num + 50));
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{timeDelay_Second},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeys(Random rd, int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    DelayTime(timeDelayAfterClick);
                }
                int num = 0;
                int num2 = rd.Next(1, 1000) % 3;
                if (content.Length < 3)
                {
                    num2 = 2;
                }
                else
                {
                    num = rd.Next(1, content.Length * 3 / 4);
                }
                switch (num2)
                {
                    case 0:
                        {
                            string content4 = content.Substring(0, num);
                            SendKeys(typeAttribute, attributeValue, content4, Convert.ToDouble(rd.Next(10, 100)) / 1000.0);
                            DelayTime(rd.Next(1, 3));
                            int num3 = rd.Next(1, num);
                            for (int i = 0; i < num3; i++)
                            {
                                SendBackspace(typeAttribute, attributeValue);
                                DelayTime(Convert.ToDouble(rd.Next(1000, 2000)) / 10000.0);
                            }
                            string text = "";
                            switch (typeAttribute)
                            {
                                case 1:
                                    text = "#" + attributeValue;
                                    break;
                                case 2:
                                    text = "[name=\"" + attributeValue + "\"]";
                                    break;
                                case 4:
                                    text = attributeValue;
                                    break;
                            }
                            content4 = content.Substring(chrome.ExecuteScript("return document.querySelector('" + text + "').value+''").ToString().Length);
                            DelayTime(rd.Next(1, 3));
                            SendKeys(typeAttribute, attributeValue, content4, Convert.ToDouble(rd.Next(100, 300)) / 1000.0, isClick: false);
                            DelayTime(rd.Next(1, 3));
                            break;
                        }
                    case 1:
                        {
                            string content2 = content.Substring(0, num);
                            string content3 = content.Substring(num);
                            SendKeys(typeAttribute, attributeValue, content2, Convert.ToDouble(rd.Next(10, 100)) / 1000.0);
                            DelayTime(rd.Next(1, 3));
                            SendKeys(typeAttribute, attributeValue, content3, Convert.ToDouble(rd.Next(100, 300)) / 1000.0, isClick: false);
                            DelayTime(rd.Next(1, 3));
                            break;
                        }
                    case 2:
                        SendKeys(typeAttribute, attributeValue, content, Convert.ToDouble(rd.Next(100, 200)) / 1000.0);
                        DelayTime(rd.Next(1, 3));
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{timeDelay_Second},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeysv2(int typeAttribute, string attributeValue, int index, int subTypeAttribute, string subAttributeValue, int subIndex, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex);
                    DelayTime(timeDelayAfterClick);
                }
                if (subTypeAttribute == 0)
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElements(By.Id(attributeValue))[index].SendKeys(content);
                            break;
                        case 2:
                            chrome.FindElements(By.Name(attributeValue))[index].SendKeys(content);
                            break;
                        case 3:
                            chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(content);
                            break;
                        case 4:
                            chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(content);
                            break;
                    }
                }
                else
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElements(By.Id(attributeValue))[index].FindElements(By.Id(subAttributeValue))[subIndex].SendKeys(content);
                            break;
                        case 2:
                            chrome.FindElements(By.Name(attributeValue))[index].FindElements(By.Name(subAttributeValue))[subIndex].SendKeys(content);
                            break;
                        case 3:
                            chrome.FindElements(By.XPath(attributeValue))[index].FindElements(By.XPath(subAttributeValue))[subIndex].SendKeys(content);
                            break;
                        case 4:
                            chrome.FindElements(By.CssSelector(attributeValue))[index].FindElements(By.CssSelector(subAttributeValue))[subIndex].SendKeys(content);
                            break;
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendKeys({typeAttribute},{attributeValue},{content},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SendKeysWithSpeed(int tocDo, int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            int result = 0;
            switch (tocDo)
            {
                case 0:
                    result = SendKeys(Base.rd, typeAttribute, attributeValue, content, timeDelay_Second, isClick, timeDelayAfterClick);
                    break;
                case 1:
                    result = SendKeys(typeAttribute, attributeValue, content, timeDelay_Second, isClick, timeDelayAfterClick);
                    break;
                case 2:
                    result = SendKeys(typeAttribute, attributeValue, content, isClick, timeDelayAfterClick);
                    break;
            }
            return result;
        }

        public int SendKeysWithSpeedv2(int tocDo, int typeAttribute, string attributeValue, int index, int subTypeAttribute, string subAttributeValue, int subIndex, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            int result = 0;
            switch (tocDo)
            {
                case 0:
                    result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
                    break;
                case 1:
                    result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
                    break;
                case 2:
                    result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
                    break;
            }
            return result;
        }

        public int SendBackspace(int typeAttribute, string attributeValue)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).SendKeys(Keys.Backspace);
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).SendKeys(Keys.Backspace);
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).SendKeys(Keys.Backspace);
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(Keys.Backspace);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendBackspace({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public void DelayThaoTacNho(int timeAdd = 0, Random rd = null)
        {
            if (rd == null)
            {
                rd = new Random();
            }
            DelayTime(rd.Next(timeAdd + 1, timeAdd + 4));
        }

        public void DelayRandom(int timeFrom, int timeTo)
        {
            DelayTime(Base.rd.Next(timeFrom, timeTo + 1));
        }

        public int SendEnter(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (subTypeAttribute == 0)
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElements(By.Id(attributeValue))[index].SendKeys(Keys.Enter);
                            break;
                        case 2:
                            chrome.FindElements(By.Name(attributeValue))[index].SendKeys(Keys.Enter);
                            break;
                        case 3:
                            chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(Keys.Enter);
                            break;
                        case 4:
                            chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(Keys.Enter);
                            break;
                    }
                }
                else
                {
                    switch (typeAttribute)
                    {
                        case 1:
                            chrome.FindElements(By.Id(attributeValue))[index].FindElements(By.Id(subAttributeValue))[subIndex].SendKeys(Keys.Enter);
                            break;
                        case 2:
                            chrome.FindElements(By.Name(attributeValue))[index].FindElements(By.Name(subAttributeValue))[subIndex].SendKeys(Keys.Enter);
                            break;
                        case 3:
                            chrome.FindElements(By.XPath(attributeValue))[index].FindElements(By.XPath(subAttributeValue))[subIndex].SendKeys(Keys.Enter);
                            break;
                        case 4:
                            chrome.FindElements(By.CssSelector(attributeValue))[index].FindElements(By.CssSelector(subAttributeValue))[subIndex].SendKeys(Keys.Enter);
                            break;
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendEnter({typeAttribute},{attributeValue},{index})");
            }
            return flag ? 1 : 0;
        }

        public int PasteContent(int typeAttribute, string attributeValue, int index = 0, bool isClick = true, int timeDelayAfterClick = 0)
        {
            //Discarded unreachable code: IL_0200, IL_024d
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (isClick)
                {
                    Click(typeAttribute, attributeValue);
                    Thread.Sleep(Convert.ToInt32(timeDelayAfterClick * 1000));
                }
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElements(By.Id(attributeValue))[index].SendKeys(Keys.Control + "v");
                        break;
                    case 2:
                        chrome.FindElements(By.Name(attributeValue))[index].SendKeys(Keys.Control + "v");
                        break;
                    case 3:
                        chrome.FindElements(By.XPath(attributeValue))[index].SendKeys(Keys.Control + "v");
                        break;
                    case 4:
                        chrome.FindElements(By.CssSelector(attributeValue))[index].SendKeys(Keys.Control + "v");
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.PasteContent({typeAttribute},{attributeValue},{isClick})");
            }
            return flag ? 1 : 0;
        }

        public int SelectText(int typeAttribute, string attributeValue)
        {
            //Discarded unreachable code: IL_016b, IL_01ad
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).SendKeys(Keys.Control + "a");
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).SendKeys(Keys.Control + "a");
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).SendKeys(Keys.Control + "a");
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(Keys.Control + "a");
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SelectText({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public int ClearText(int typeAttribute, string attributeValue)
        {
            //Discarded unreachable code: IL_00ef, IL_0131
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).Clear();
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).Clear();
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).Clear();
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).Clear();
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.ClearText({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public int CountElement(string querySelector)
        {
            //Discarded unreachable code: IL_00b8, IL_0102
            int result = 0;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                result = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('" + querySelector.Replace("'", "\\'") + "').length+''").ToString());
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CountElement(" + querySelector + ")");
            }
            return result;
        }

        public int CheckExistElement(string querySelector, double timeWait_Second = 0.0)
        {
            bool flag = true;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                while ((string)chrome.ExecuteScript("return document.querySelectorAll('" + querySelector.Replace("'", "\\'") + "').length+''") == "0")
                {
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        flag = false;
                        break;
                    }
                    if (!CheckIsLive())
                    {
                        return -2;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                ExportError(null, ex, $"chrome.CheckExistElement({querySelector},{timeWait_Second})");
            }
            return flag ? 1 : 0;
        }

        public int CheckExistElementv2(string JSPath, double timeWait_Second = 0.0)
        {
            bool flag = true;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                while ((string)chrome.ExecuteScript("return " + JSPath + ".length+''") == "0")
                {
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        flag = false;
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                ExportError(null, ex, $"chrome.CheckExistElement({JSPath},{timeWait_Second})");
            }
            return flag ? 1 : 0;
        }

        public int WaitForSearchElement(string querySelector, int typeSearch = 0, double timeWait_Second = 0.0)
        {
            bool flag = true;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                if (typeSearch == 0)
                {
                    while ((string)chrome.ExecuteScript("return document.querySelectorAll('" + querySelector.Replace("'", "\\'") + "').length+''") == "0")
                    {
                        if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                        {
                            flag = false;
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    while ((string)chrome.ExecuteScript("return document.querySelectorAll('" + querySelector.Replace("'", "\\'") + "').length+''") != "0")
                    {
                        if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                        {
                            flag = false;
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                ExportError(null, ex, $"chrome.WaitForSearchElement({querySelector},{typeSearch},{timeWait_Second})");
            }
            return flag ? 1 : 0;
        }

        public int CheckExistElements(double timeWait_Second = 0.0, params string[] querySelectors)
        {
            int num = 0;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; }return (output + ''); "));
                    if (num != 0)
                    {
                        return num;
                    }
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    bool flag = true;
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, string.Format("chrome.CheckExistElements({0},{1})", timeWait_Second, string.Join("|", querySelectors)));
            }
            return num;
        }

        public string CheckExistElementsV2(double timeWait_Second = 0.0, params string[] querySelectors)
        {
            int num = 0;
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; }return (output + ''); "));
                    if (num != 0)
                    {
                        return querySelectors[num - 1];
                    }
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    bool flag = true;
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, string.Format("chrome.CheckExistElements({0},{1})", timeWait_Second, string.Join("|", querySelectors)));
            }
            return "";
        }

       
        public int CheckExistElements(double timeWait_Second, Dictionary<int, List<string>> dic)
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    foreach (KeyValuePair<int, List<string>> item in dic)
                    {
                        if (Convert.ToInt32(chrome.ExecuteScript("var arr='" + string.Join("|", item.Value) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; } return (output + ''); ")) != 0)
                        {
                            return item.Key;
                        }
                    }
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    bool flag = true;
                }
            }
            catch
            {
            }
            return 0;
        }

        public int SendEnter(int typeAttribute, string attributeValue)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        chrome.FindElement(By.Id(attributeValue)).SendKeys(Keys.Enter);
                        break;
                    case 2:
                        chrome.FindElement(By.Name(attributeValue)).SendKeys(Keys.Enter);
                        break;
                    case 3:
                        chrome.FindElement(By.XPath(attributeValue)).SendKeys(Keys.Enter);
                        break;
                    case 4:
                        chrome.FindElement(By.CssSelector(attributeValue)).SendKeys(Keys.Enter);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.SendEnter({typeAttribute},{attributeValue})");
            }
            return flag ? 1 : 0;
        }

        public bool FindElementChrome(int typeAttribute, string typeAttributeValue)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return flag;
            }
            try
            {
                switch(typeAttribute)
                {
                    case 1:
                        flag = chrome.FindElements(By.Id(typeAttributeValue)).Count > 0;
                        break;
                    case 2:
                        flag = chrome.FindElements(By.Name(typeAttributeValue)).Count > 0;
                        break;
                    case 3:
                        flag = chrome.FindElements(By.XPath(typeAttributeValue)).Count > 0;
                        break;
                    case 4:
                        flag = chrome.FindElements(By.CssSelector(typeAttributeValue)).Count > 0;
                        break;
                }
            }
            catch { }

            return flag;
        }

        public int Scroll(int x, int y)
        {
            //Discarded unreachable code: IL_0082, IL_00c9
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                string script = $"window.scrollTo({x}, {y})";
                chrome.ExecuteScript(script);
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.Scroll({x},{y})");
            }
            return flag ? 1 : 0;
        }

        public int ScrollSmooth(string JSpath)
        {
            //Discarded unreachable code: IL_0069, IL_00b8
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.ExecuteScript(JSpath + ".scrollIntoView({ behavior: 'smooth', block: 'center'});");
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.ScrollSmooth(" + JSpath + ")");
                return 0;
            }
        }

        public int ScrollSmoothIfNotExistOnScreen(string JSpath)
        {
            //Discarded unreachable code: IL_0088, IL_00d7
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                if (CheckExistElementOnScreen(JSpath) != 0)
                {
                    chrome.ExecuteScript(JSpath + ".scrollIntoView({ behavior: 'smooth', block: 'center'});");
                }
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.ScrollSmoothIfNotExistOnScreen(" + JSpath + ")");
                return 0;
            }
        }

        public int CheckExistElementOnScreen(string JSpath)
        {
            //Discarded unreachable code: IL_0084, IL_00ce
            int result = 0;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                result = Convert.ToInt32(chrome.ExecuteScript("var check='';x=" + JSpath + ";if(x.getBoundingClientRect().top<=0) check='-1'; else if(x.getBoundingClientRect().top+x.getBoundingClientRect().height>window.innerHeight) check='1'; else check='0'; return check;"));
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CheckExistElementOnScreen(" + JSpath + ")");
            }
            return result;
        }

        public Point GetSizeChrome()
        {
            //Discarded unreachable code: IL_00ba, IL_00c7
            Point result = new Point(0, 0);
            if (CheckIsLive())
            {
                try
                {
                    string text = chrome.ExecuteScript("return window.innerHeight+'|'+window.innerWidth").ToString();
                    result.X = Convert.ToInt32(text.Split('|')[1]);
                    result.Y = Convert.ToInt32(text.Split('|')[0]);
                }
                catch
                {
                }
            }
            return result;
        }

        public int Close()
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                try
                {
                    chrome.Quit();
                }
                catch
                {
                }
                if (process != null)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int ClearText()
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            new Actions(chrome).KeyDown(Keys.Shift).SendKeys(Keys.ArrowUp).SendKeys(Keys.ArrowUp)
                .SendKeys(Keys.ArrowUp)
                .SendKeys(Keys.Delete)
                .KeyUp(Keys.Shift)
                .Build()
                .Perform();
            return 1;
        }

        public int ScreenCapture(string imagePath, string fileName)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)chrome).GetScreenshot();
                screenshot.SaveAsFile(imagePath + (imagePath.EndsWith("\\") ? "" : "\\") + fileName + ".png");
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.ScreenCapture(" + imagePath + "," + fileName + ")");
            }
            return flag ? 1 : 0;
        }
        public Screenshot ScreenCaptureV2(string imagePath, string fileName)
        {
            Screenshot screenshot = ((ITakesScreenshot)chrome).GetScreenshot();
            string fullPath = imagePath + (imagePath.EndsWith("\\") ? "" : "\\") + fileName + ".png";
            screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
            return screenshot;
        }


        public int AddCookieIntoChrome(string cookie, string domain = ".facebook.com")
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                string[] array = cookie.Split(';');
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (text.Trim() != "")
                    {
                        string[] array3 = text.Split('=');
                        if (array3.Count() > 1 && array3[0].Trim() != "")
                        {
                            Cookie cookie2 = new Cookie(array3[0].Trim(), text.Substring(text.IndexOf('=') + 1).Trim(), domain, "/", DateTime.Now.AddDays(10.0));
                            chrome.Manage().Cookies.AddCookie(cookie2);
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.AddCookieIntoChrome(" + cookie + "," + domain + ")");
                return 0;
            }
        }

        public string GetCookieFromChrome(string domain = "facebook")
        {
            string text = "";
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                Cookie[] array = chrome.Manage().Cookies.AllCookies.ToArray();
                Cookie[] array2 = array;
                foreach (Cookie cookie in array2)
                {
                    if (cookie.Domain.Contains(domain))
                    {
                        text = text + cookie.Name + "=" + cookie.Value + "; ";
                    }
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.GetCookieFromChrome(" + domain + ")");
            }
            return text;
        }

        public int OpenNewTab(string url, bool switchToLastTab = true)
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.ExecuteScript("window.open('" + url + "', '_blank').focus();");
                if (switchToLastTab)
                {
                    chrome.SwitchTo().Window(chrome.WindowHandles.Last());
                }
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.OpenNewTab({url},{switchToLastTab})");
                return 0;
            }
        }

        public int CloseCurrentTab()
        {
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.CloseCurrentTab()");
                return 0;
            }
        }

        public int SwitchToFirstTab()
        {
            //Discarded unreachable code: IL_005e, IL_008d
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.SwitchTo().Window(chrome.WindowHandles.First());
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.SwitchToFirstTab()");
                return 0;
            }
        }

        public int SwitchToLastTab()
        {
            //Discarded unreachable code: IL_005e, IL_008d
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                chrome.SwitchTo().Window(chrome.WindowHandles.Last());
                return 1;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, "chrome.SwitchToLastTab()");
                return 0;
            }
        }

        public void DelayTime(double timeDelay_Seconds)
        {
            try
            {
                if (!CheckChromeClosed())
                {
                    Thread.Sleep(Convert.ToInt32(timeDelay_Seconds * 1000.0));
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.DelayTime({timeDelay_Seconds})");
            }
        }

        public static void ExportError(Chrome chrome, Exception ex, string error = "")
        {
            try
            {
                if (!(error == "chrome.Open()"))
                {
                    return;
                }
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }
                if (!Directory.Exists("log\\html"))
                {
                    Directory.CreateDirectory("log\\html");
                }
                if (!Directory.Exists("log\\images"))
                {
                    Directory.CreateDirectory("log\\images");
                }
                Random random = new Random();
                string text = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + random.Next(1000, 9999);
                if (chrome != null)
                {
                    string contents = chrome.ExecuteScript("var markup = document.documentElement.innerHTML;return markup;").ToString();
                    chrome.ScreenCapture("log\\images\\", text);
                    File.WriteAllText("log\\html\\" + text + ".html", contents);
                }
                using (StreamWriter streamWriter = new StreamWriter("log\\log.txt", append: true))
                {
                    streamWriter.WriteLine("-----------------------------------------------------------------------------");
                    streamWriter.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    streamWriter.WriteLine("File: " + text);
                    if (error != "")
                    {
                        streamWriter.WriteLine("Error: " + error);
                    }
                    streamWriter.WriteLine();
                    if (ex != null)
                    {
                        streamWriter.WriteLine("Type: " + ex.GetType().FullName);
                        streamWriter.WriteLine("Message: " + ex.Message);
                        streamWriter.WriteLine("StackTrace: " + ex.StackTrace);
                        ex = ex.InnerException;
                    }
                }
            }
            catch
            {
            }
        }

        //public int Select(int typeAttribute, string attributeValue, string value)
        //{
        //    bool flag = false;
        //    if (!CheckIsLive())
        //    {
        //        return -2;
        //    }
        //    try
        //    {
        //        switch (typeAttribute)
        //        {
        //            case 1:
        //                new SelectElement(chrome.FindElementById(attributeValue)).SelectByValue(value);
        //                break;
        //            case 2:
        //                new SelectElement(chrome.FindElementByName(attributeValue)).SelectByValue(value);
        //                break;
        //            case 3:
        //                new SelectElement(chrome.FindElementByXPath(attributeValue)).SelectByValue(value);
        //                break;
        //            case 4:
        //                new SelectElement(chrome.FindElementByCssSelector(attributeValue)).SelectByValue(value);
        //                break;
        //        }
        //        flag = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExportError(null, ex, $"chrome.Select({typeAttribute},{attributeValue},{value})");
        //    }
        //    return flag ? 1 : 0;
        //}

        internal bool GetProcess()
        {
            try
            {
                if (process != null)
                {
                    return true;
                }
                string title = "";
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        try
                        {
                            title = chrome.CurrentWindowHandle;
                        }
                        catch
                        {
                            title = Helpers.Common.CreateRandomStringNumber(15, rd);
                        }
                        if (title != "")
                        {
                            for (int j = 0; j < 30; j++)
                            {
                                chrome.ExecuteScript("document.title='" + title + "'");
                                DelayTime(1.0);
                                process = (from x in Process.GetProcessesByName("chrome")
                                           where x.MainWindowTitle.Contains(title)
                                           select x).FirstOrDefault();
                                if (process != null)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    DelayTime(1.0);
                }
            }
            catch
            {
            }
            return false;
        }

        public string CheckExistElementsv2(double timeWait_Second = 0.0, params string[] querySelectors)
        {
            int num = 0;
            if (!CheckIsLive())
            {
                return "-2";
            }
            try
            {
                int tickCount = Environment.TickCount;
                while (true)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; }return (output + ''); "));
                    if (num != 0)
                    {
                        return querySelectors[num - 1];
                    }
                    if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    bool flag = true;
                }
            }
            catch (Exception ex)
            {
                ExportError(null, ex, string.Format("chrome.CheckExistElementsv2({0},{1})", timeWait_Second, string.Join("|", querySelectors)));
            }
            return "";
        }
        public int Select(int typeAttribute, string attributeValue, string value)
        {
            bool flag = false;
            if (!CheckIsLive())
            {
                return -2;
            }
            try
            {
                switch (typeAttribute)
                {
                    case 1:
                        new SelectElement(chrome.FindElement(By.Id(attributeValue))).SelectByValue(value);
                        break;
                    case 2:
                        new SelectElement(chrome.FindElement(By.Name(attributeValue))).SelectByValue(value);
                        break;
                    case 3:
                        new SelectElement(chrome.FindElement(By.XPath(attributeValue))).SelectByValue(value);
                        break;
                    case 4:
                        new SelectElement(chrome.FindElement(By.CssSelector(attributeValue))).SelectByValue(value);
                        break;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ExportError(null, ex, $"chrome.Select({typeAttribute},{attributeValue},{value})");
            }
            return flag ? 1 : 0;
        }

        public bool CheckTextInChrome(string text1, string text2)
        {
            bool result = false;
            try
            {
                int num = 0;
                string text3;
                for (;;)
                {
                    text3 = chrome.FindElement(By.TagName("body")).Text;
                    bool flag = text3.ToLower().Contains(text1.ToLower()) || text3.ToLower().Contains(text2.ToLower());
                    if (flag)
                    {
                        break;
                    }
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        goto quit;
                    }
                    num++;
                }
                bool flag3 = text3.ToLower().Contains(text1.ToLower());
                if (flag3)
                {
                    result = true;
                }
                else
                {
                    result = true;
                }
                result = true;
            quit:;
            }
            catch
            {
            }
            return result;
        }

        public bool FindClickElement(string text1, string text2, bool click)
        {
            bool result = false;
            int num = 0;
            for (; ; )
            {
                string text3 = chrome.FindElement(By.TagName("body")).Text;
                try
                {
                    IWebElement webElement = chrome.FindElement(By.XPath("//*[text()='" + text1 + "']"));
                    bool displayed = webElement.Displayed;
                    if (displayed)
                    {
                        if (click)
                        {
                            webElement.Click();
                        }
                        result = true;
                        break;
                    }
                }
                catch
                {
                    try
                    {
                        IWebElement webElement2 = chrome.FindElement(By.XPath("//*[text()='" + text2 + "']"));
                        bool displayed2 = webElement2.Displayed;
                        if (displayed2)
                        {
                            if (click)
                            {
                                webElement2.Click();
                            }
                            result = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                bool flag = num == 2;
                if (flag)
                {
                    break;
                }
                num++;
            }
            return result;
        }

        public IWebElement returnRowElement(string text)
        {
            return chrome.FindElement(By.XPath(text));
        }
    }

}

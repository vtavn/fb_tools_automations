using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;

namespace Facebook_Tool_Request.Common
{
    internal class ChromedriverUpdater
    {
        private static string chromedriverPath = Directory.GetCurrentDirectory() + @"\chromedriver.exe";
        private static string latestVersionUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE";
        //private static string chromedriverDownloadUrl = "https://chromedriver.storage.googleapis.com/{0}/chromedriver_win32.zip";
        private static string chromedriverDownloadUrl = "https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing/{0}/win64/chromedriver-win32.zip";

        public static bool ChromeUpdate()
        {
            try
            {
                string latestVersion = GetLatestChromedriverVersion(latestVersionUrl);
                BackupExistingChromedriver();
                DownloadChromedriver(latestVersion);
                ChangeFile();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static void BackupExistingChromedriver()
        {
            if (File.Exists(chromedriverPath))
            {
                File.Copy(chromedriverPath, $"{chromedriverPath}.backup", true);
            }
        }
        static string GetLatestChromedriverVersion(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(url).Trim();
            }
        }
        private static void DownloadChromedriver(string version)
        {
            using (WebClient client = new WebClient())
            {
                string downloadUrl = string.Format(chromedriverDownloadUrl, version);
                client.DownloadFile(downloadUrl, "chromedriver.zip");
            }
        }
        private static void CloseProcess(string nameProcess)
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName(nameProcess);
                foreach (Process process in processesByName)
                {
                    process.Kill();
                }
            }
            catch
            {
            }
        }

        private static void ChangeFile()
        {
            CloseProcess("chromedriver");
            CloseProcess("chrome");
            if (File.Exists("chromedriver.exe"))
            {
                File.Delete(chromedriverPath);
            }
            if (File.Exists("chromedriver.zip") && !File.Exists("chromedriver.exe"))
            {
                ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "/chromedriver.zip", Directory.GetCurrentDirectory());
                //File.Delete("chromedriver.zip");
            }    

        }
    }
}

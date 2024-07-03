using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    public class SettingsTool
    {
        public static JSON_Settings GetSettings(string key)
        {
            bool flag = !SettingsTool.settings.ContainsKey(key);
            if (flag)
            {
                SettingsTool.settings.Add(key, new JSON_Settings(key, false));
            }
            return SettingsTool.settings[key];
        }

        private static void LoadSettings(string key)
        {
            bool flag = SettingsTool.settings.ContainsKey(key);
            if (flag)
            {
                SettingsTool.settings[key] = new JSON_Settings(key, false);
            }
            else
            {
                SettingsTool.settings.Add(key, new JSON_Settings(key, false));
            }
        }

        public static void SaveSettings(string key)
        {
            bool flag = SettingsTool.settings.ContainsKey(key);
            if (flag)
            {
                SettingsTool.settings[key].Save("");
            }
            SettingsTool.LoadSettings(key);
        }

        public static string GetPathProfile()
        {
            string text = SettingsTool.settings["configGeneral"].GetValue("txbPathProfile", "");
            bool flag = !text.Contains('\\');
            if (flag)
            {
                text = FileHelper.GetPathToCurrentFolder() + "\\" + ((text == "") ? "profiles" : text);
            }
            bool flag2 = !Directory.Exists(text);
            string result;
            if (flag2)
            {
                result = FileHelper.GetPathToCurrentFolder() + "\\profiles";
            }
            else
            {
                result = text;
            }
            return result;
        }

        public static string GetPathBackup()
        {
            return FileHelper.GetPathToCurrentFolder() + "\\backup";
        }

        private static Dictionary<string, JSON_Settings> settings = new Dictionary<string, JSON_Settings>();
    }
}

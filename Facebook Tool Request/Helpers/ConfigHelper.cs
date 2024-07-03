using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    public class ConfigHelper
    {
        public static string GetPathProfile()
        {
            JSON_Settings jsonHelper = new JSON_Settings("configGeneral", false);
            string text = jsonHelper.GetValue("txbPathProfile", "");
            bool flag = !text.Contains('\\');
            if (flag)
            {
                text = FileHelper.GetPathToCurrentFolder() + "\\" + ((jsonHelper.GetValue("txbPathProfile", "") == "") ? "profiles" : jsonHelper.GetValue("txbPathProfile", ""));
            }
            return text;
        }

        public static string GetPathBackup()
        {
            return FileHelper.GetPathToCurrentFolder() + "\\backup";
        }
    }
}

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Facebook_Tool_Request.Helpers
{
    public class CommonCSharp
    {
        public static void UpdateStatusDataGridView(DataGridView dgv, int row, string colName, object status)
        {
            try
            {
                dgv.Invoke(new MethodInvoker(delegate ()
                {
                    dgv.Rows[row].Cells[colName].Value = status;
                }));
            }
            catch
            {
            }
        }
        public static string GetStatusDataGridView(DataGridView dgv, int row, string colName)
        {
            string output = "";
            dgv.Invoke(new MethodInvoker(delegate ()
            {
                try
                {
                    output = dgv.Rows[row].Cells[colName].Value.ToString();
                }
                catch
                {
                }
            }));
            return output;
        }
        public static bool CheckBasicString(string text)
        {
            bool result = true;
            foreach (char c in text)
            {
                bool flag = (c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && c != '.' && c != '[' && c != ']';
                if (flag)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        public static bool IsContainNumber(string pValue)
        {
            foreach (char c in pValue)
            {
                bool flag = char.IsDigit(c);
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }
        public static string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string CreateRandomString(int lengText, Random rd = null)
        {
            string text = "";
            bool flag = rd == null;
            if (flag)
            {
                rd = new Random();
            }
            string text2 = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < lengText; i++)
            {
                text += text2[rd.Next(0, text2.Length)].ToString();
            }
            return text;
        }
        public static string CreateRandomNumber(int leng, Random rd = null)
        {
            string text = "";
            bool flag = rd == null;
            if (flag)
            {
                rd = new Random();
            }
            string text2 = "0123456789";
            for (int i = 0; i < leng; i++)
            {
                text += text2[rd.Next(0, text2.Length)].ToString();
            }
            return text;
        }
        public static void DelayTime(double second)
        {
            Application.DoEvents();
            Thread.Sleep(Convert.ToInt32(second * 1000.0));
        }
        public static void ExportError(ChromeDriver chrome, string error)
        {
            try
            {
                Random random = new Random();
                string text = string.Concat(new string[]
                {
                    DateTime.Now.Day.ToString(),
                    "_",
                    DateTime.Now.Month.ToString(),
                    "_",
                    DateTime.Now.Year.ToString(),
                    "_",
                    DateTime.Now.Hour.ToString(),
                    "_",
                    DateTime.Now.Minute.ToString(),
                    "_",
                    DateTime.Now.Second.ToString(),
                    "_",
                    random.Next(1000, 9999).ToString()
                });
                bool flag = chrome != null;
                if (flag)
                {
                    string contents = chrome.ExecuteScript("var markup = document.documentElement.innerHTML;return markup;", Array.Empty<object>()).ToString();
                    Screenshot screenshot = ((ITakesScreenshot)chrome).GetScreenshot();
                    screenshot.SaveAsFile("log\\images\\" + text + ".png");
                    File.WriteAllText("log\\html\\" + text + ".html", contents);
                }
                File.AppendAllText("log\\log.txt", string.Concat(new string[]
                {
                    DateTime.Now.ToString(),
                    "|<",
                    text,
                    ">|",
                    error,
                    Environment.NewLine
                }));
            }
            catch
            {
            }
        }
    }
}

using Facebook_Tool_Request.Common;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    internal class Utils
    {
        internal static int smethod_0(string string_0)
        {
            return Convert.ToInt32(string_0);
        }

        internal static object D0B427B9(object object_0, object object_1, StringSplitOptions stringSplitOptions_0)
        {
            return ((string)object_0).Split((char[])object_1, stringSplitOptions_0);
        }

        internal static object smethod_01(object object_0, object object_1)
        {
            return ((string)object_0).Split((char[])object_1);
        }

        internal static Point FD2A9229(int int_0, int int_1)
        {
            return new Point(int_0, int_1);
        }

        internal static bool C0910183(string string_0, string string_1)
        {
            return string_0 != string_1;
        }

        internal static bool smethod_02(string string_0)
        {
            return string.IsNullOrEmpty(string_0);
        }

        internal static string CEB1D508(object object_0)
        {
            return ((string)object_0).Trim();
        }

        internal static void smethod_03(object object_0, string string_0)
        {
            ((ChromeOptions)object_0).BinaryLocation = string_0;
        }

        internal static string F2898910(string string_0, string string_1, string string_2, string string_3)
        {
            return string_0 + string_1 + string_2 + string_3;
        }

        internal static string smethod_04(string string_0, string string_1, string string_2)
        {
            return string_0 + string_1 + string_2;
        }

        internal static string E097A431(string[] string_0)
        {
            return string.Concat(string_0);
        }

        internal static object F0B6EE93()
        {
            return ChromeDriverService.CreateDefaultService();
        }

        internal static object B40DCEA1(object object_0)
        {
            return ((RemoteWebDriver)object_0).WindowHandles;
        }

        internal static char smethod_011(object object_0, int int_0)
        {
            return ((string)object_0)[int_0];
        }

        internal static bool smethod_012(string string_0, string string_1)
        {
            return string_0 != string_1;
        }

        internal static bool smethod_013(string string_0, string string_1)
        {
            return string_0 == string_1;
        }

        internal static bool smethod_014(object object_0)
        {
            return ((IEnumerator)object_0).MoveNext();
        }

        internal static object DB2856BD(object object_0)
        {
            return ((IEnumerator)object_0).Current;
        }

        internal static bool B5A9BB12(object object_0, string string_0)
        {
            return ((string)object_0).StartsWith(string_0);
        }

        internal static string B9321482(object object_0, string string_0, string string_1)
        {
            return ((string)object_0).Replace(string_0, string_1);
        }

        internal static void BF0C4695(object object_0, bool bool_0)
        {
            ((RadioButton)object_0).Checked = bool_0;
        }

        internal static void smethod_015(object object_0, bool bool_0)
        {
            ((CheckBox)object_0).Checked = bool_0;
        }

        internal static string D8264592(object object_0, object object_1)
        {
            return ((string)object_0).TrimEnd((char[])object_1);
        }

        public static string smethod_3(Chrome C8B0A4B3)
        {
            string text = C8B0A4B3.ExecuteScript("var x='';document.querySelectorAll('[property=\"og:title\"]').length>0&&(x=document.querySelector('[property=\"og:title\"]').getAttribute('content')),''==x&&document.querySelectorAll('[data-gt] a').length>0&&(x=document.querySelector('[data-gt] a').innerText),''==x&&document.querySelectorAll('.actor').length>0&&(x=document.querySelector('.actor').innerText), x+''; return x;").ToString();
            bool flag = text == "";
            if (flag)
            {
                text = CEB1D508(((object[])smethod_01(C8B0A4B3.ExecuteScript("return document.title").ToString(), new char[]
                {
                    '-',
                    '|'
                }))[0]);
            }
            return text;
        }
    }
}

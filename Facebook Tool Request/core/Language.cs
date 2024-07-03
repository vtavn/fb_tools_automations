using System;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public class Language
    {
        public static string GetValue(string key)
        {
            return key;
        }
        public static void GetValue(Control control)
        {
            control.Text = Language.GetValue(control.Text);
        }
        public static void GetValue(ToolStripItem control)
        {
            control.Text = Language.GetValue(control.Text);
        }
        public static void GetValue(DataGridViewColumn control)
        {
            control.HeaderText = Language.GetValue(control.HeaderText);
        }
        public static string data = "";
    }
}

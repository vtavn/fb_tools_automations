using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    public static class Extensions
    {
        public static Bitmap ToBitmap(this string base64)
        {
            try
            {
                byte[] array = Convert.FromBase64String(base64);
                MemoryStream memoryStream = new MemoryStream(array, 0, array.Length);
                memoryStream.Write(array, 0, array.Length);
                return new Bitmap(Image.FromStream(memoryStream, true));
            }
            catch
            {
            }
            return null;
        }
        public static void SwitchSize(this Form f)
        {
            bool flag = f.WindowState == FormWindowState.Maximized;
            if (flag)
            {
                f.WindowState = FormWindowState.Normal;
            }
            else
            {
                Rectangle workingArea = Screen.FromHandle(f.Handle).WorkingArea;
                workingArea.Location = new Point(0, 0);
                f.MaximumSize = workingArea.Size;
                f.WindowState = FormWindowState.Maximized;
            }
        }
        private static void Control_Enter(object sender, EventArgs e)
        {
            Control control = sender as Control;
            bool flag = control.Text == "Search...";
            if (flag)
            {
                control.Text = "";
                control.ForeColor = Color.Black;
            }
        }
        private static void Control_Leave(object sender, EventArgs e)
        {
            Control control = sender as Control;
            bool flag = control.Text == "";
            if (flag)
            {
                control.Text = "Search...";
                control.ForeColor = Color.Gray;
            }
        }
        public static void SetPlaceholder(this Control control, EventHandler controlLeave, EventHandler controlEnter, string placeholderText = "Search...")
        {
            control.Text = placeholderText;
            control.ForeColor = Color.Gray;
            control.Leave += controlLeave;
            control.Enter += controlEnter;
        }

        private static EventHandler<EventArgs> ControlLeave = (sender, e) => Control_Leave((Control)sender, e);
        private static EventHandler<EventArgs> ControlEnter = (sender, e) => Control_Enter((Control)sender, e);


    }
}

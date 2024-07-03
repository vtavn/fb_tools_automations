using Emgu.CV.CvEnum;
using Facebook_Tool_Request.core.fTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            var mainForm = new core.fCuaIntro();
            CenterFormOnScreen(mainForm);
            Application.Run(mainForm);
        }
        static void CenterFormOnScreen(Form form)
        {
            Screen screen = Screen.FromControl(form);

            int screenWidth = screen.WorkingArea.Width;
            int screenHeight = screen.WorkingArea.Height;

            int formWidth = form.Width;
            int formHeight = form.Height;

            int x = (screenWidth - formWidth) / 2;
            int y = (screenHeight - formHeight) / 2;

            form.Location = new System.Drawing.Point(x, y);
        }
    }
}

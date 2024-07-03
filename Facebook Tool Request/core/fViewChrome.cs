using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fViewChrome : Form
    {
        public fViewChrome()
        {
            this.InitializeComponent();
            fViewChrome.remote = this;
        }

        public void AddChromeIntoPanel(IntPtr MainWindowHandle, int indexDevice, int width, int heigh, int x = -10, int y = -30)
        {
            base.Invoke(new MethodInvoker(delegate ()
            {
                User32Helper.SetParent(MainWindowHandle, (from Control h in this.panelListDevice.Controls
                                                          where h.Tag.Equals(indexDevice)
                                                          select h).FirstOrDefault<Control>().Handle);
                User32Helper.MoveWindow(MainWindowHandle, x, y, width, heigh, true);
            }));
        }

        public void RemovePanelDevice(int indexDevice)
        {
            Control ctr = this.panelListDevice.Controls["dv" + indexDevice.ToString()];
            this.panelListDevice.Invoke(new MethodInvoker(delegate ()
            {
                this.panelListDevice.Controls.Remove(ctr);
            }));
        }

        public void AddPanelDevice(int indexDevice, int width, int heigh)
        {
            Panel panel = new Panel();
            panel.Name = "dv" + indexDevice.ToString();
            panel.Tag = indexDevice;
            panel.Size = new Size(width, heigh);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            base.Invoke(new MethodInvoker(delegate ()
            {
                this.panelListDevice.Controls.Add(panel);
            }));
            for (int i = 0; i < 10; i++)
            {
                bool flag = this.panelListDevice.Controls["dv" + indexDevice.ToString()] != null;
                if (flag)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        private void TurnOffDevice(object sender, EventArgs e)
        {
            this.RemovePanelDevice(Convert.ToInt32((sender as PictureBox).Name.Replace("pic", "")));
        }

        public static fViewChrome remote;

        private void panelListDevice_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fViewChrome_Load(object sender, EventArgs e)
        {

        }
    }
}

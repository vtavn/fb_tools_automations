using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fCuaIntro : Form
    {
        public fCuaIntro()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            gunaProgressBar1.Increment(10);
            lbloading.Text = "Đang tải " + gunaProgressBar1.Value.ToString() + "%";
            if (gunaProgressBar1.Value == 100)
            {
                timer1.Enabled=false;
                fMain fMain = new fMain();
                fMain.Show();
                this.Hide();
            }    
        }

        private void fCuaIntro_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            lbVer.Text = "v."+version;
        }
    }
}

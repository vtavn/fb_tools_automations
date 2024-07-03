using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core.fPages
{
    public partial class fPages : Form
    {
        public fPages()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (base.Width == Screen.PrimaryScreen.WorkingArea.Width && base.Height == Screen.PrimaryScreen.WorkingArea.Height)
            {
                base.Width = Base.width;
                base.Height = Base.heigh;
                base.Top = Base.top;
                base.Left = Base.left;
            }
            else
            {
                Base.top = base.Top;
                Base.left = base.Left;
                base.Top = 0;
                base.Left = 0;
                base.Width = Screen.PrimaryScreen.WorkingArea.Width;
                base.Height = Screen.PrimaryScreen.WorkingArea.Height;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }
    }
}

using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core.fTools
{
    public partial class fGetIdPost : Form
    {
        public fGetIdPost()
        {
            InitializeComponent();
        }

        private void btnGetID_Click(object sender, EventArgs e)
        {
            if(txtLink.Text.Length > 0)
            {
                string idpost = cuakit.Helpers.GetIdPost(txtLink.Text.Trim());
               if(idpost.Length > 0)
                {
                    txtId.Text = idpost;
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("ID Private hoặc đường dẫn link sai! Thử lại đi!!", 1);
                }
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng điền Link cần lấy ID", 1);
            }
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            base.Close();

        }
    }
}

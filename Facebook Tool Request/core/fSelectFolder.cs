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

namespace Facebook_Tool_Request.core
{
    public partial class fSelectFolder : Form
    {
        public fSelectFolder()
        {
            this.InitializeComponent();
            fSelectFolder.pathFolder = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fSelectFolder_Load(object sender, EventArgs e)
        {
            fSelectFolder.pathFolder = "";
        }
        public static string pathFolder = "";

        private void btnAdd_Click(object sender, EventArgs e)
        {
            fSelectFolder.pathFolder = this.txtPathFolder.Text.Trim();
            bool flag = fSelectFolder.pathFolder == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập đường dẫn Folder!"), 2);
                this.txtPathFolder.Focus();
            }
            else
            {
                base.Close();
            }
        }
    }
}

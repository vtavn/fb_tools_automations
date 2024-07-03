using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fAddFolder : Form
    {
        public fAddFolder()
        {
            InitializeComponent();
            fAddFolder.isAdd = false;
        }

        public static bool isAdd;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtNameFolder.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng điền tên thư mục!"), 1);
                this.txtNameFolder.Focus();
            }
            else
            {
                bool flag2 = CommonSQL.CheckExitsFile(text);
                if (flag2)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Tên thư mục này đã tồn tại, vui lòng nhập tên khác!"), 1);
                    this.txtNameFolder.Focus();
                }
                else
                {
                    bool flag3 = CommonSQL.InsertFileToDatabase(text);
                    if (flag3)
                    {
                        fAddFolder.isAdd = true;
                        bool flag4 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Thêm thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
                        if (flag4)
                        {
                            base.Close();
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Thêm thư mục lỗi!"), 1);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txtNameFolder_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Return;
            if (flag)
            {
                this.btnAdd_Click(null, null);
            }
        }
    }
}

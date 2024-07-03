using core.KichBan;
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
    public partial class fThemKichBan : Form
    {
        public fThemKichBan(int type, string id = "")
        {
            this.InitializeComponent();
            this.type = type;
            this.id = id;
            bool flag = type == 1;
            if (flag)
            {
                this.btnAdd.Text = Language.GetValue("Cập nhật");
                this.txtTen.Text = InteractSQL.GetKichBanById(id).Rows[0]["TenKichBan"].ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private int type = 0;
        private string id = "";

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtTen.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập tên kịch bản!"), 2);
            }
            else
            {
                bool flag2 = this.type == 0;
                if (flag2)
                {
                    bool flag3 = InteractSQL.InsertKichBan(text);
                    if (flag3)
                    {
                        base.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                    }
                }
                else
                {
                    bool flag4 = InteractSQL.UpdateKichBan(this.id, text);
                    if (flag4)
                    {
                        base.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                    }
                }
            }
        }
    }
}

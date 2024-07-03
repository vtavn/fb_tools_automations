using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fNhapDuLieu : Form
    {
        public fNhapDuLieu(string linkPath, string title = "Nhập danh sách Uid cần clone", string status = "Danh sách Uid", string footer = "(Mỗi nội dung 1 dòng, spin nội dung {a|b|c})", List<string> lstData = null)
        {
            this.InitializeComponent();
            Helpers.Common.CreateFile(linkPath);
            this.linkPath = linkPath;
            this.status = status;
            this.lblTitle.Text = title;
            this.lblStatus.Text = status + " (0):";
            this.lblFooter.Text = footer;
            bool flag = lstData != null;
            if (flag)
            {
                File.WriteAllLines(linkPath, lstData);
                this.txtComment.Lines = lstData.ToArray();
            }
            else
            {
                this.txtComment.Lines = File.ReadAllLines(linkPath);
            }
            this.txtComment_TextChanged(null, null);
        }

        private string linkPath = "";

        private string status = "";

        private void txtComment_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txtComment.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblStatus.Text = this.status + " (" + list.Count.ToString() + "):";
            }
            catch
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(this.linkPath, this.txtComment.Text.Trim());
                bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Lưu thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
                if (flag)
                {
                    base.Close();
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }

        private void btnClosed_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

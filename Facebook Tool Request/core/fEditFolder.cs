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
    public partial class fEditFolder : Form
    {
        public fEditFolder(string idFile, string namefile)
        {
            this.InitializeComponent();
            this.idFile = idFile;
            this.nameFileOld = namefile;
            this.isSuccess = false;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtNameFileNew.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng điền tên thư mục mới!"), 3);
                this.txtNameFileNew.Focus();
            }
            else
            {
                bool flag2 = CommonSQL.CheckExitsFile(text);
                if (flag2)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Tên thư mục này đã tồn tại, vui lòng nhập tên khác!"), 3);
                    this.txtNameFileNew.Focus();
                }
                else
                {
                    bool flag3 = text.Equals(this.txtNameFileOld.Text.Trim());
                    if (flag3)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Tên thư mục mới phải khác thư mục cũ!"), 3);
                        this.txtNameFileNew.Focus();
                    }
                    else
                    {
                        bool flag4 = CommonSQL.UpdateFileNameToDatabase(this.idFile, text);
                        if (flag4)
                        {
                            this.isSuccess = true;
                            bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Cập nhật thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
                            if (flag5)
                            {
                                base.Close();
                            }
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox(Language.GetValue("Cập nhật tên thư mục lỗi!"), 1);
                        }
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txbNameFile_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Return;
            if (flag)
            {
                this.BtnAdd_Click(null, null);
            }
        }
        private void fEditFolder_Load(object sender, EventArgs e)
        {
            this.txtNameFileOld.Text = this.nameFileOld;
        }

        private string idFile;

        private string nameFileOld;

        public bool isSuccess = false;

        
    }
}

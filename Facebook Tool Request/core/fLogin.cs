using Facebook_Tool_Request.Helpers;
using Facebook_Tool_Request.Properties;
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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void coppyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vutienanh.net/");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string pass = txtPass.Text;
            string idthietbi = License.Hardware.getHDD();
            if (string.IsNullOrEmpty(email) )
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập tài khoản!"), 3);
            }else if (string.IsNullOrEmpty(pass))
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập tài khoản!"), 3);
            }
            else
            {
                btnLogin.Text = "ĐANG KIỂM TRA...";
                string checkKey = CommonRequest.checkLicense(email, pass, idthietbi);

                if (checkKey.StartsWith("0|"))
                {
                    string[] str = checkKey.Split('|');
                    MessageBoxHelper.ShowMessageBox(str[1], 1);
                    btnLogin.Text = "ĐĂNG NHẬP";
                    return;
                }
                else if (checkKey.StartsWith("1|"))
                {
                    string[] str = checkKey.Split('|');

                    DateTime ngayMua = DateTime.ParseExact(str[5], "dd/MM/yyyy", null);
                    DateTime ngayHetHan = ngayMua.AddDays(int.Parse(str[4]));
                    string convertNgayHetHan = ngayHetHan.ToString("dd/MM/yyyy");
                    //Settings.Default.isAdmin = str[6];
                    //Settings.Default.buy_at = str[5];
                    //Settings.Default.han = str[4];
                    //Settings.Default.user = str[2];
                    //Settings.Default.hethan = convertNgayHetHan;
                    Settings.Default.Save();
                    MessageBoxHelper.ShowMessageBox(str[1], 1);
                    new fMain().Show();
                    this.Hide();
                }


            }

        }

        private void fLogin_Load(object sender, EventArgs e)
        {
            txtMaMay.Text = License.Hardware.getHDD();

            //string ngayhethan = Settings.Default.hethan;
            //if(ngayhethan != "")
            //{
            //    DateTime ngayHetHan = DateTime.ParseExact(ngayhethan, "dd/MM/yyyy", null);
            //    DateTime ngayHienTai = DateTime.Now;
            //    int ketQuaSoSanh = ngayHienTai.CompareTo(ngayHetHan);

            //    if (ketQuaSoSanh > 0)
            //    {
            //        //Settings.Default.isAdmin = "";
            //        //Settings.Default.buy_at = "";
            //        //Settings.Default.han = "";
            //        //Settings.Default.user = "";
            //        //Settings.Default.hethan = "";
            //        Settings.Default.Save();

            //        MessageBoxHelper.ShowMessageBox("Hết hạn!", 1);
            //        Application.Exit(); // Kết thúc ứng dụng khi hết hạn
            //    }
            //    else if (ketQuaSoSanh < 0)
            //    {
            //        fMain mainForm = new fMain();
            //        mainForm.Shown += (s, args) =>
            //        {
            //            this.Hide();
            //        };
            //        mainForm.FormClosed += (s, args) => this.Close(); // Đóng form chính khi form mới được đóng
            //        mainForm.Show();
            //    }
            //    else
            //    {
            //        fMain mainForm = new fMain();
            //        mainForm.Shown += (s, args) =>
            //        {
            //            this.Hide();
            //        };
            //        mainForm.FormClosed += (s, args) => this.Close(); // Đóng form chính khi form mới được đóng
            //        mainForm.Show();
            //        Console.WriteLine("Ngày hết hạn là hôm nay");
            //    }
            //}
            
        }



        private void txtMaMay_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMaMay.Text);
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Coppy thành công!"), 1);

        }
    }
}

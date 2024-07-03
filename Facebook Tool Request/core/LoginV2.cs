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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Facebook_Tool_Request.core
{
    public partial class LoginV2 : Form
    {
        public LoginV2()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnQuest_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox("Nhấp vào nút Login để sử dụng!", 3);
        }

        private void LoginV2_Load(object sender, EventArgs e)
        {
            txtKey.Text = License.Hardware.getHDD();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text;
            string idthietbi = License.Hardware.getHDD();
            if (string.IsNullOrEmpty(key))
            {
                MessageBoxHelper.ShowMessageBox("Nhập key đê...", 3);
            }
            else
            {

                btnLogin.Text = "CHECK...";
                string checkKey = CommonRequest.checkLic2(key, idthietbi);
                if (checkKey.StartsWith("0|"))
                {
                    string[] str = checkKey.Split('|');
                    MessageBoxHelper.ShowMessageBox(str[1], 1);
                    btnLogin.Text = "LOGIN";
                    return;
                }
                else if (checkKey.StartsWith("1|"))
                {
                    string[] str = checkKey.Split('|');
                    if (str[2] == "99")
                    {
                        btnLogin.Text = "LOGIN";
                        MessageBoxHelper.ShowMessageBox(str[1], 1);
                        Application.Exit();
                        return;
                    }
                    if (str[4] == "0")
                    {
                        btnLogin.Text = "LOGIN";
                        MessageBoxHelper.ShowMessageBox("Thiết bị của bạn đã bị chặn sử dụng!", 1);
                        Application.Exit();
                        return;
                    }
                    int timenow = cuakit.Helpers.GetNowTimeStamp();
                    int checkExpired = cuakit.Helpers.checkExpired(Convert.ToDouble(timenow), Convert.ToDouble(str[2]));
                    string ngayhethan = cuakit.Helpers.UnixTimeStampToDateTime(Convert.ToDouble(str[2]));
                    if (checkExpired == 1)
                    {
                        btnLogin.Text = "LOGIN";
                        MessageBoxHelper.ShowMessageBox("Dịch vụ của bạn đã hết hạn vào ngày: " + ngayhethan, 1);
                        Application.Exit();
                        return;
                        //hết hạn
                    }
                    else if(checkExpired == -1)
                    {
                        //còn hạn
                        MessageBoxHelper.ShowMessageBox(str[1] + " Cảm ơn đã dùng dịch vụ!", 1);
                        new fMain().Show();
                        this.Hide();
                    }
                    else {
                        // hôm nay hết hạn
                        MessageBoxHelper.ShowMessageBox("Dịch vụ của bạn sẽ hết hạn vào ngày hôm nay!", 1);
                        new fMain().Show();
                        this.Hide();
                    }

                    //DateTime ngayMua = DateTime.ParseExact(str[5], "dd/MM/yyyy", null);
                    //DateTime ngayHetHan = ngayMua.AddDays(int.Parse(str[4]));
                    //string convertNgayHetHan = ngayHetHan.ToString("dd/MM/yyyy");
                    //Settings.Default.isAdmin = str[6];
                    //Settings.Default.buy_at = str[5];
                    //Settings.Default.han = str[4];
                    //Settings.Default.user = str[2];
                    //Settings.Default.hethan = convertNgayHetHan;
                    //Settings.Default.Save();
                    //MessageBoxHelper.ShowMessageBox(str[1], 1);
                    //new fMain().Show();
                    //this.Hide();
                    int a = 0;
                }
            }
        }
    }
}

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
    public partial class fMoTrinhDuyet : Form
    {
        public fMoTrinhDuyet()
        {
            this.InitializeComponent();
            fMoTrinhDuyet.isOK = false;
        }

        public static bool isOK;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCaiDatChung());
        }

        private void fMoTrinhDuyet_Load(object sender, EventArgs e)
        {
            JSON_Settings json_Settings = new JSON_Settings("configOpenBrowser", false);
            bool valueBool = json_Settings.GetValueBool("isUseProfile", true);
            if (valueBool)
            {
                this.rbSuDungProfile.Checked = true;
            }
            else
            {
                this.rbKhongDungProfile.Checked = true;
            }
            bool flag = json_Settings.GetValueInt("typeBrowserLogin", 0) == 0;
            if (flag)
            {
                this.rbLoginMFB.Checked = true;
            }
            else
            {
                this.rbLoginWWW.Checked = true;
            }

            bool flag2 = json_Settings.GetValueInt("typeOpenBrowser", 0) == 0;
            if (flag2)
            {
                this.rdOpenNormal.Checked = true;
            }
            else
            {
                this.rdOpenLoginFb.Checked = true;
            }
          
            switch (json_Settings.GetValueInt("typeLogin", 0))
            {
                case 0:
                    this.rbLoginUidPass.Checked = true;
                    break;
                case 1:
                    this.rbLoginEmailPass.Checked = true;
                    break;
                case 2:
                    this.rbLoginCookie.Checked = true;
                    break;
            }
            this.ckbAddChromeIntoForm.Checked = json_Settings.GetValueBool("ckbAddChromeIntoForm", false);
            this.nudWidthChrome.Value = json_Settings.GetValueInt("nudWidthChrome", 0);
            this.nudHeighChrome.Value = json_Settings.GetValueInt("nudHeighChrome", 0);
            this.cbbColumnChrome.Text = ((json_Settings.GetValue("cbbColumnChrome", "") == "") ? "5" : json_Settings.GetValue("cbbColumnChrome", ""));
            this.cbbRowChrome.Text = ((json_Settings.GetValue("cbbRowChrome", "") == "") ? "2" : json_Settings.GetValue("cbbRowChrome", ""));
            this.ckbGetCookie.Checked = json_Settings.GetValueBool("isGetCookie", false);
            this.ckbGetToken.Checked = json_Settings.GetValueBool("isGetToken", false);
            this.ckbAutoOpenLink.Checked = json_Settings.GetValueBool("ckbAutoOpenLink", false);
            this.txtLink.Text = json_Settings.GetValue("txtLink", "");
            this.ckbLoginHotmail.Checked = json_Settings.GetValueBool("ckbLoginHotmail", false);
            this.ckbAutoOpenLink_CheckedChanged(null, null);
            this.ckbAddChromeIntoForm_CheckedChanged(null, null);
            this.rdOpenLoginFb_CheckedChanged(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            JSON_Settings json_Settings = new JSON_Settings("configOpenBrowser", false);
            bool @checked = this.rbSuDungProfile.Checked;
            if (@checked)
            {
                json_Settings.Update("isUseProfile", true);
            }
            else
            {
                json_Settings.Update("isUseProfile", false);
            }
            bool checked2 = this.rbLoginUidPass.Checked;
            if (checked2)
            {
                json_Settings.Update("typeLogin", 0);
            }
            else
            {
                bool checked3 = this.rbLoginEmailPass.Checked;
                if (checked3)
                {
                    json_Settings.Update("typeLogin", 1);
                }
                else
                {
                    json_Settings.Update("typeLogin", 2);
                }
            }

            bool checked4 = this.rbLoginMFB.Checked;
            if (checked4)
            {
                json_Settings.Update("typeBrowserLogin", 0);
            }
            else
            {
                json_Settings.Update("typeBrowserLogin", 1);
            }

            bool checked5 = this.rdOpenNormal.Checked;
            if (checked5)
            {
                json_Settings.Update("typeOpenBrowser", 0);
            }
            else
            {
                json_Settings.Update("typeOpenBrowser", 1);
            }

            json_Settings.Update("ckbAddChromeIntoForm", this.ckbAddChromeIntoForm.Checked);
            json_Settings.Update("nudWidthChrome", this.nudWidthChrome.Value);
            json_Settings.Update("nudHeighChrome", this.nudHeighChrome.Value);
            json_Settings.Update("cbbColumnChrome", this.cbbColumnChrome.Text);
            json_Settings.Update("cbbRowChrome", this.cbbRowChrome.Text);
            json_Settings.Update("isGetCookie", this.ckbGetCookie.Checked);
            json_Settings.Update("isGetToken", this.ckbGetToken.Checked);
            json_Settings.Update("ckbAutoOpenLink", this.ckbAutoOpenLink.Checked);
            json_Settings.Update("txtLink", this.txtLink.Text);
            json_Settings.Update("ckbLoginHotmail", this.ckbLoginHotmail.Checked);
            json_Settings.Save("");
            fMoTrinhDuyet.isOK = true;
            bool flag4 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Lưu thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
            if (flag4)
            {
                base.Close();
            }

        }

        private void ckbAutoOpenLink_CheckedChanged(object sender, EventArgs e)
        {
            this.txtLink.Enabled = this.ckbAutoOpenLink.Checked;
        }

        private void ckbAddChromeIntoForm_CheckedChanged(object sender, EventArgs e)
        {
            this.plAddChromeVaoFormView.Enabled = this.ckbAddChromeIntoForm.Checked;
            this.plSapXepCuaSoChrome.Enabled = !this.ckbAddChromeIntoForm.Checked;
        }

        private void rdOpenLoginFb_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = rdOpenLoginFb.Checked;
            this.panel5.Enabled = rdOpenLoginFb.Checked;
        }
    }
}

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
    public partial class fSettingMail : Form
    {
        public static bool isOK;

        public fSettingMail()
        {
            this.InitializeComponent();
            fMoTrinhDuyet.isOK = false;
        }

        private void gunaLabel2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ckbChangePass_CheckedChanged(object sender, EventArgs e)
        {
            plPassword.Enabled = ckbChangePass.Checked;
        }

        private void rdPassAdd_CheckedChanged(object sender, EventArgs e)
        {
            txtPassMailNew.Enabled = rdPassAdd.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            JSON_Settings json_Settings = new JSON_Settings("configSettingMail", false);
            json_Settings.Update("ckbUnlockMail", ckbUnlockMail.Checked);
            json_Settings.Update("ckbAddMailKP", ckbAddMailKP.Checked);
            json_Settings.Update("ckbDeleteMailKPOld", ckbDeleteMailKPOld.Checked);
            json_Settings.Update("ckbChangePass", ckbChangePass.Checked);
            json_Settings.Update("ckbHidechrome", ckbHidechrome.Checked);

            bool @typeChangePass = rdPassRandom.Checked;
            if (@typeChangePass)
            {
                json_Settings.Update("typeChangePass", "random");
            }
            else
            {
                json_Settings.Update("typeChangePass", "custom");
                json_Settings.Update("txtPassMailNew", this.txtPassMailNew.Text.Trim());
            }
            json_Settings.Update("txtKeyMailKP", this.txtKeyMailKP.Text.Trim());

            bool @webPhone = rdPhoneIronSim.Checked;
            if (@webPhone)
            {
                json_Settings.Update("webPhone", "ironsim");
            }
            json_Settings.Update("txtApiKeyWebSo", this.txtApiKeyWebSo.Text.Trim());

            bool @webMailKP = rdMailInboxes.Checked;
            if (@webMailKP)
            {
                json_Settings.Update("webMailKP", "inboxes");
            }
            json_Settings.Update("txtMailDomainU", this.txtMailDomainU.Text.Trim());

            json_Settings.Save("");
            fSettingMail.isOK = true;
            bool flag4 = MessageBoxHelper.ShowMessageBoxWithQuestion("Lưu thành công, bạn có muốn đóng cửa sổ?") == DialogResult.Yes;
            if (flag4)
            {
                base.Close();
            }
        }
        private void fSettingMail_Load(object sender, EventArgs e)
        {
            JSON_Settings json_Settings = new JSON_Settings("configSettingMail", false);
            this.ckbUnlockMail.Checked = json_Settings.GetValueBool("ckbUnlockMail", false);
            this.ckbAddMailKP.Checked = json_Settings.GetValueBool("ckbAddMailKP", false);
            this.ckbHidechrome.Checked = json_Settings.GetValueBool("ckbHidechrome", false);
            this.ckbDeleteMailKPOld.Checked = json_Settings.GetValueBool("ckbDeleteMailKPOld", false);
            this.ckbChangePass.Checked = json_Settings.GetValueBool("ckbChangePass", false);
            this.txtPassMailNew.Text = json_Settings.GetValue("txtPassMailNew", "");
            this.txtKeyMailKP.Text = json_Settings.GetValue("txtKeyMailKP", "");
            this.txtApiKeyWebSo.Text = json_Settings.GetValue("txtApiKeyWebSo", "");
            this.txtMailDomainU.Text = json_Settings.GetValue("txtMailDomainU", "getnada.com");
            
            string typeChangepass = json_Settings.GetValue("typeChangePass", "random");
            if (typeChangepass == "random")
            {
                this.rdPassRandom.Checked = true;
            }
            else
            {
                this.rdPassAdd.Checked = true;
            }
            string typeWebphone = json_Settings.GetValue("webPhone", "ironsim");
            if (typeWebphone == "ironsim")
            {
                this.rdPhoneIronSim.Checked = true;
            }
            string typeWebMail = json_Settings.GetValue("webMailKP", "inboxes");
            if (typeWebMail == "inboxes")
            {
                this.rdMailInboxes.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnCheckKeyPhone_Click(object sender, EventArgs e)
        {
            if(txtApiKeyWebSo.Text != "")
            {
                btnCheckKeyPhone.Enabled = false;
                btnCheckKeyPhone.Text = "....";
                string checkkey = cuakit.Helpers.checkApiIronsim(txtApiKeyWebSo.Text.Trim());
                if (!string.IsNullOrEmpty(checkkey))
                {
                    MessageBoxHelper.ShowMessageBox(checkkey);
                    btnCheckKeyPhone.Enabled = true;
                    btnCheckKeyPhone.Text = "Check";
                }
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Nhập key đi rồi hãy check", 3);
            }

        }
    }
}

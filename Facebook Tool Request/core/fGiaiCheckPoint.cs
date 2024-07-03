using Facebook_Tool_Request.Common;
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
    public partial class fGiaiCheckPoint : Form
    {
        private JSON_Settings settings;
        public event EventHandler FormRunEvent;

        public fGiaiCheckPoint()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configGiaiCheckPoint", false);
            this.Load_cbbChangeLanguage();
            this.Load_cbbSiteCaptcha();
            this.Load_cbbAnhWeb();

        }

        private void lbHowPass_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox($"- Chọn mật khẩu tự đặt thì sẽ là tự đặt =))\n- Chọn mật khẩu random thì sẽ là ngẫu nhiên!", 1);
        }

        private void ckbGiaiCaptcha_CheckedChanged(object sender, EventArgs e)
        {
            plCaptcha.Enabled = ckbGiaiCaptcha.Checked;
        }

        private void btnChosseProfile_Click(object sender, EventArgs e)
        {
            this.txtAnhBackup.Text = Helpers.Common.SelectFolderNew("Chọn thư mục ảnh");
        }

        private void rdAnhBackup_CheckedChanged(object sender, EventArgs e)
        {
            this.txtAnhBackup.Enabled = rdAnhBackup.Checked;
        }

        private void rdAnhWeb_CheckedChanged(object sender, EventArgs e)
        {
            this.cbbAnhWeb.Enabled = rdAnhWeb.Checked;
        }
        private void Load_cbbChangeLanguage()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = SetupFolder.GetListCountryCountryCode();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[0] + " | " + array[1]);
            }
            this.cbbChangeLanguage.DataSource = new BindingSource(dictionary, null);
            this.cbbChangeLanguage.ValueMember = "Key";
            this.cbbChangeLanguage.DisplayMember = "Value";
        }

        private void Load_cbbSiteCaptcha()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = SetupFolder.GetListSiteCaptcha();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbSiteCaptcha.DataSource = new BindingSource(dictionary, null);
            this.cbbSiteCaptcha.ValueMember = "Key";
            this.cbbSiteCaptcha.DisplayMember = "Value";
        }

        private void Load_cbbAnhWeb()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = SetupFolder.GetListSiteImage();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbAnhWeb.DataSource = new BindingSource(dictionary, null);
            this.cbbAnhWeb.ValueMember = "Key";
            this.cbbAnhWeb.DisplayMember = "Value";
        }

        private void rbPassInput_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPassInput.Enabled = rbPassInput.Checked;
        }

        private void ckbChangeLanguage_CheckedChanged(object sender, EventArgs e)
        {
            this.cbbChangeLanguage.Enabled = ckbChangeLanguage.Checked;
        }

        private void ckbUpAnh_CheckedChanged(object sender, EventArgs e)
        {
            this.plUpAnh.Enabled = ckbUpAnh.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool passNew = this.rbPassInput.Checked;
                int passType = 0;
                if (passNew) passType = 0; else passType = 1;
                this.settings.Update("passType", passType);
                this.settings.Update("txtPassInput", this.txtPassInput.Text);
                this.settings.Update("ckbGiaiCaptcha", this.ckbGiaiCaptcha.Checked);
                this.settings.Update("cbbSiteCaptcha", this.cbbSiteCaptcha.SelectedValue.ToString());
                this.settings.Update("txtCaptchaKey", this.txtCaptchaKey.Text);
                this.settings.Update("ckbUpAnh", this.ckbUpAnh.Checked);

                bool rdAnhBackup = this.rdAnhBackup.Checked;
                int imageType = 0;
                if (rdAnhBackup) imageType = 0; else imageType = 1;
                this.settings.Update("imageType", imageType);
                this.settings.Update("txtAnhBackup", this.txtAnhBackup.Text);
                this.settings.Update("cbbAnhWeb", this.cbbAnhWeb.SelectedValue.ToString());
                this.settings.Update("ckChangeMd5Anh", this.ckChangeMd5Anh.Checked);
                this.settings.Update("ckbAnhCu", this.ckbAnhCu.Checked);
                this.settings.Update("ckbChangeLanguage", this.ckbChangeLanguage.Checked);
                this.settings.Update("cbbChangeLanguage", this.cbbChangeLanguage.SelectedValue.ToString());
                this.settings.Update("keyotp282", this.keyotp282.Text.Trim());
                this.settings.Save("");
                base.Close();
                FormRunEvent?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox("Lỗi!", 1);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fGiaiCheckPoint_Load(object sender, EventArgs e)
        {
            int passType = this.settings.GetValueInt("passType", 0);
            if (passType == 0)
            {
                this.rbPassInput.Checked = true;
            }else
            {
                this.rbPassRandom.Checked = true;
            }

            this.txtPassInput.Text = this.settings.GetValue("txtPassInput", "");
            this.ckbGiaiCaptcha.Checked = this.settings.GetValueBool("ckbGiaiCaptcha", false);
            this.cbbSiteCaptcha.SelectedValue = (this.settings.GetValue("cbbSiteCaptcha", "") == "") ? "0" : this.settings.GetValue("cbbSiteCaptcha", "");
            this.txtCaptchaKey.Text = this.settings.GetValue("txtCaptchaKey", "");
            this.ckbUpAnh.Checked = this.settings.GetValueBool("ckbUpAnh", false);

            int imageType = this.settings.GetValueInt("imageType", 1);
            if (imageType == 0)
            {
                this.rdAnhBackup.Checked = true;
            }
            else
            {
                this.rdAnhWeb.Checked = true;
            }
            this.txtAnhBackup.Text = this.settings.GetValue("txtAnhBackup", "");
            this.cbbAnhWeb.SelectedValue = (this.settings.GetValue("cbbAnhWeb", "") == "") ? "0" : this.settings.GetValue("cbbAnhWeb", "");
            this.ckChangeMd5Anh.Checked = this.settings.GetValueBool("ckChangeMd5Anh", false);
            this.ckbAnhCu.Checked = this.settings.GetValueBool("ckbAnhCu", false);
            this.ckbChangeLanguage.Checked = this.settings.GetValueBool("ckbChangeLanguage", false);
            this.cbbChangeLanguage.SelectedValue = (this.settings.GetValue("cbbChangeLanguage", "") == "") ? "0" : this.settings.GetValue("cbbChangeLanguage", "");
            this.keyotp282.Text = this.settings.GetValue("keyotp282", "f74b53b3edc6f37e2d823b2f27bc744e");
        }

        private void btCheckCaptcha_Click(object sender, EventArgs e)
        {

            if (this.cbbSiteCaptcha.SelectedValue.ToString() == "omocaptcha")
            {
                string rsVnd = WebCaptcha.checkAmountOmocaptcha(this.txtCaptchaKey.Text.Trim());
                if (rsVnd.StartsWith("success|"))
                {
                    string[] str = rsVnd.Split('|');
                    MessageBoxHelper.ShowMessageBox("Số dư: " + str[1], 1);
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("Key không lỗi hoặc không đúng. Vui lòng kiểm tra lại.", 2);
                }
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Cua Đẹp Trai Nên Không cần Check!", 2);
            }
        }

        private void cbbSiteCaptcha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cbbSiteCaptcha.SelectedValue.ToString() == "nopecha")
            {
                this.txtCaptchaKey.Text = "anhtuxautrai";
            }
            else if(this.cbbSiteCaptcha.SelectedValue.ToString() == "omocaptcha")
            {
                this.txtCaptchaKey.Text = "JlHRC8KwdDft6bL1G72oHDKd7kerIsnx2EoVADAa0zUgXLlYdCYGcZKZ9Z2yuqqMcb91mgATulv8MZfF";
            }
        }
    }
}

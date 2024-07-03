using core.KichBan;
using Facebook_Tool_Request.Helpers;
using Facebook_Tool_Request.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fCauHinhTuongTac : Form
    {
        public fCauHinhTuongTac()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configInteractGeneral", false);
        }

        private void btnShowNangCao_Click(object sender, EventArgs e)
        {
            //this.timer2.Start();
        }

        private JSON_Settings settings;

        private bool isCollapsed1 = false;

        private bool isCollapsed2 = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool flag = this.isCollapsed1;
            if (flag)
            {
                bool flag2 = this.plShowCoBan.Size == this.plShowCoBan.MaximumSize;
                if (flag2)
                {
                    this.timer1.Stop();
                    this.isCollapsed1 = false;
                }
                else
                {
                    this.btnShowCoBan.Image = Resources.collapse_arrow_24;
                    this.plShowNangCao.Top += 10;
                    this.plShowCoBan.Height += 10;
                    base.Height += 10;
                }
            }
            else
            {
                bool flag3 = this.plShowCoBan.Size == this.plShowCoBan.MinimumSize;
                if (flag3)
                {
                    this.timer1.Stop();
                    this.isCollapsed1 = true;
                }
                else
                {
                    this.btnShowCoBan.Image = Resources.expand_arrow_24;
                    this.plShowNangCao.Top -= 10;
                    this.plShowCoBan.Height -= 10;
                    base.Height -= 10;
                }
            }
        }

        private void btnShowCoBan_Click(object sender, EventArgs e)
        {
            //this.timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            bool flag = this.isCollapsed2;
            if (flag)
            {
                bool flag2 = this.plShowNangCao.Size == this.plShowNangCao.MaximumSize;
                if (flag2)
                {
                    this.timer2.Stop();
                    this.isCollapsed2 = false;
                }
                else
                {
                    this.btnShowNangCao.Image = Resources.collapse_arrow_24;
                    this.plShowNangCao.Height += 10;
                    base.Height += 10;
                }
            }
            else
            {
                bool flag3 = this.plShowNangCao.Size == this.plShowNangCao.MinimumSize;
                if (flag3)
                {
                    this.timer2.Stop();
                    this.isCollapsed2 = true;
                }
                else
                {
                    this.btnShowNangCao.Image = Resources.expand_arrow_24;
                    this.plShowNangCao.Height -= 10;
                    base.Height -= 10;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ckbCapNhatThongTin_Click(object sender, EventArgs e)
        {
            bool @checked = this.ckbCapNhatThongTin.Checked;
            if (@checked)
            {
                bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Sử dụng tính năng này có thể dẫn đến tài khoản Facebook bị khóa!") + "\r\n" + Language.GetValue("Bạn có chắc vẫn muốn sử dụng?")) == DialogResult.No;
                if (flag)
                {
                    this.ckbCapNhatThongTin.Checked = false;
                }
            }
        }

        private void ckbRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            this.plRepeatAll.Enabled = this.ckbRepeatAll.Checked;
        }

        private void ckbGetToken_Click(object sender, EventArgs e)
        {
            //bool @checked = this.ckbGetToken.Checked;
            //if (@checked)
            //{
            //    bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Sử dụng tính năng này có thể dẫn đến tài khoản Facebook bị khóa!") + "\r\n" + Language.GetValue("Bạn có chắc vẫn muốn sử dụng?")) == DialogResult.No;
            //    if (flag)
            //    {
            //        this.ckbGetToken.Checked = false;
            //    }
            //}
        }

        private void ckbCreateShortcut_CheckedChanged(object sender, EventArgs e)
        {
            bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Đang phát triển!")) == DialogResult.No;
            if (flag)
            {
                this.ckbCreateShortcut.Checked = false;
            }
            else
            {
                this.ckbCreateShortcut.Checked = false;
            }
            //bool @checked = this.ckbCreateShortcut.Checked;
            //if (@checked)
            //{
            //    Helpers.Common.ShowForm(new fCauHinhTaoShortcut());
            //}
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool @checked = this.rbLoginEmailPass.Checked;
            if (@checked)
            {
                num = 1;
            }
            else
            {
                bool checked2 = this.rbLoginCookie.Checked;
                if (checked2)
                {
                    num = 2;
                }
            }
            this.settings.Update("typeLogin", num);
            this.settings.Update("typeToken", this.cbbToken.SelectedValue.ToString());
            int num2 = this.rbLoginMFB.Checked ? 0 : 1;
            this.settings.Update("typeBrowserLogin", num2);
            this.settings.Update("ckbCreateProfile", this.ckbCreateProfile.Checked);
            this.settings.Update("ckbGetToken", this.ckbGetToken.Checked);
            this.settings.Update("ckbGetCookie", this.ckbGetCookie.Checked);
            this.settings.Update("ckbCapNhatThongTin", this.ckbCapNhatThongTin.Checked);
            this.settings.Update("ckbCheckLiveUid", this.ckbCheckLiveUid.Checked);
            this.settings.Update("ckbDontSaveBrowser", this.ckbDontSaveBrowser.Checked);
            this.settings.Update("ckbRepeatAll", this.ckbRepeatAll.Checked);
            this.settings.Update("nudDelayTurnFrom", this.nudDelayTurnFrom.Value);
            this.settings.Update("nudDelayTurnTo", this.nudDelayTurnTo.Value);
            this.settings.Update("nudSoLuotChay", this.nudSoLuotChay.Value);
            this.settings.Update("RepeatAllVIP", "false");
            //this.settings.Update("ckbLogOut", this.ckbLogOut.Checked);
            //this.settings.Update("ckbLogOutOldDevice", this.ckbLogOutOldDevice.Checked);
            this.settings.Update("ckbAutoLinkInstagram", this.ckbAutoLinkInstagram.Checked);
            this.settings.Update("ckbCreateShortcut", this.ckbCreateShortcut.Checked);
            //this.settings.Update("ckbAllowFollow", this.ckbAllowFollow.Checked);
            //this.settings.Update("ckbReviewTag", this.ckbReviewTag.Checked);
            //this.settings.Update("ckbBatThongBaoDangNhap", this.ckbBatThongBaoDangNhap.Checked);
            bool checked3 = this.rbTuongTacNhanh.Checked;
            if (checked3)
            {
                this.settings.Update("typeInteract", 0);
            }
            else
            {
                this.settings.Update("typeInteract", 1);
            }
            this.settings.Update("cbbKichBan", this.cbbKichBan.SelectedValue);
            this.settings.Update("ckbRandomHanhDong", this.ckbRandomHanhDong.Checked);
            this.settings.Save("");
            bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion("Lưu thành công, bạn có muốn đóng cửa sổ?") == DialogResult.Yes;
            if (flag)
            {
                base.Close();
            }
        }
        private void CheckedChangedFull()
        {
            this.ckbRepeatAll_CheckedChanged(null, null);
        }

        private void fCauHinhTuongTac_Load(object sender, EventArgs e)
        {
            this.LoadcbbKichBan();
            this.LoadSettings();
            this.CheckedChangedFull();
            this.Load_cbbGetToken();
            this.cbbToken.SelectedValue = (this.settings.GetValue("typeToken", "") == "") ? "0" : this.settings.GetValue("typeToken", "");

        }
        private void Load_cbbGetToken()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listTypeToken = SetupFolder.GetListGetToken();
            for (int i = 0; i < listTypeToken.Count; i++)
            {
                string[] array = listTypeToken[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbToken.DataSource = new BindingSource(dictionary, null);
            this.cbbToken.ValueMember = "Key";
            this.cbbToken.DisplayMember = "Value";
        }
        private void LoadSettings()
        {
            try
            {
                switch (this.settings.GetValueInt("typeLogin", 0))
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
                switch (this.settings.GetValueInt("typeToken", 1))
                {
                    case 1:
                        this.tokenEAAB.Checked = true;
                        break;
                    case 2:
                        this.tokenEAAG.Checked = true;
                        break;
                }

                bool flag = this.settings.GetValueInt("typeBrowserLogin", 0) == 0;
                if (flag)
                {
                    this.rbLoginMFB.Checked = true;
                }
                else
                {
                    this.rbLoginWWW.Checked = true;
                }

                this.ckbCreateProfile.Checked = this.settings.GetValueBool("ckbCreateProfile", false);
                this.ckbGetToken.Checked = this.settings.GetValueBool("ckbGetToken", false);
                this.ckbGetCookie.Checked = this.settings.GetValueBool("ckbGetCookie", false);
                this.ckbCapNhatThongTin.Checked = this.settings.GetValueBool("ckbCapNhatThongTin", false);
                this.ckbCheckLiveUid.Checked = this.settings.GetValueBool("ckbCheckLiveUid", false);
                this.ckbDontSaveBrowser.Checked = this.settings.GetValueBool("ckbDontSaveBrowser", false);
                this.ckbRepeatAll.Checked = this.settings.GetValueBool("ckbRepeatAll", false);
                this.nudDelayTurnFrom.Value = this.settings.GetValueInt("nudDelayTurnFrom", 0);
                this.nudDelayTurnTo.Value = this.settings.GetValueInt("nudDelayTurnTo", 0);
                this.nudSoLuotChay.Value = this.settings.GetValueInt("nudSoLuotChay", 0);
                //this.ckbLogOut.Checked = this.settings.GetValueBool("ckbLogOut", false);
                //this.ckbLogOutOldDevice.Checked = this.settings.GetValueBool("ckbLogOutOldDevice", false);
                this.ckbAutoLinkInstagram.Checked = this.settings.GetValueBool("ckbAutoLinkInstagram", false);
                this.ckbCreateShortcut.Checked = this.settings.GetValueBool("ckbCreateShortcut", false);
                //this.ckbAllowFollow.Checked = this.settings.GetValueBool("ckbAllowFollow", false);
                //this.ckbReviewTag.Checked = this.settings.GetValueBool("ckbReviewTag", false);
                //this.ckbBatThongBaoDangNhap.Checked = this.settings.GetValueBool("ckbBatThongBaoDangNhap", false);
                this.cbbKichBan.SelectedValue = this.settings.GetValue("cbbKichBan", "");
                bool flag2 = this.settings.GetValueInt("typeInteract", 0) == 0;
                if (flag2)
                {
                    this.rbTuongTacNhanh.Checked = true;
                }
                else
                {
                    this.rbTuongTacKichBan.Checked = true;
                }
                this.ckbRandomHanhDong.Checked = this.settings.GetValueBool("ckbRandomHanhDong", false);
            }
            catch
            {
            }
        }

        private void btnCauhinhnhanh_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fConfigInteract());
        }

        private void btnQlkichban_Click(object sender, EventArgs e)
        {
            string kickBan = "";
            try
            {
                bool flag = this.cbbKichBan.Items.Count > 0;
                if (flag)
                {
                    kickBan = this.cbbKichBan.SelectedValue.ToString();
                }
            }
            catch
            {
            }
            Helpers.Common.ShowForm(new fDanhSachKichBan(kickBan));
            this.LoadcbbKichBan();
        }

        private void LoadcbbKichBan()
        {
            int num = -1;
            bool flag = this.cbbKichBan.SelectedIndex != -1;
            if (flag)
            {
                num = this.cbbKichBan.SelectedIndex;
            }
            DataTable allKichBan = InteractSQL.GetAllKichBan();
            this.cbbKichBan.DataSource = allKichBan;
            this.cbbKichBan.ValueMember = "Id_KichBan";
            this.cbbKichBan.DisplayMember = "TenKichBan";
            bool flag2 = num != -1 && num != 0 && this.cbbKichBan.Items.Count >= num + 1;
            if (flag2)
            {
                this.cbbKichBan.SelectedIndex = num;
            }
            else
            {
                bool flag3 = this.cbbKichBan.Items.Count > 0;
                if (flag3)
                {
                    this.cbbKichBan.SelectedIndex = 0;
                }
            }
        }
    }
}

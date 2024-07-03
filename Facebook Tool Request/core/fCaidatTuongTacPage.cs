using Facebook_Tool_Request.core.KichBanPage;
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
using Facebook_Tool_Request.core.KichBanPage;

namespace Facebook_Tool_Request.core
{
    public partial class fCaidatTuongTacPage : Form
    {
        private JSON_Settings settings;

        public fCaidatTuongTacPage()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configTuongTacPage", false);

        }
        private void fCaidatTuongTacPage_Load(object sender, EventArgs e)
        {
            this.LoadcbbKichBanP();
            this.LoadSettings();
            this.ckbRepeatAll_CheckedChanged(null, null);
        }
        private void LoadcbbKichBanP()
        {
            int num = -1;
            bool flag = this.cbbKichBanP.SelectedIndex != -1;
            if (flag)
            {
                num = this.cbbKichBanP.SelectedIndex;
            }
            DataTable allKichBan = InteractSQL.GetAllKichBan();
            this.cbbKichBanP.DataSource = allKichBan;
            this.cbbKichBanP.ValueMember = "Id_KichBan";
            this.cbbKichBanP.DisplayMember = "TenKichBan";
            bool flag2 = num != -1 && num != 0 && this.cbbKichBanP.Items.Count >= num + 1;
            if (flag2)
            {
                this.cbbKichBanP.SelectedIndex = num;
            }
            else
            {
                bool flag3 = this.cbbKichBanP.Items.Count > 0;
                if (flag3)
                {
                    this.cbbKichBanP.SelectedIndex = 0;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void LoadSettings()
        {
            try {
                this.ckbCheckliveTokenP.Checked = this.settings.GetValueBool("ckbCheckliveTokenP", false);
                this.cbbKichBanP.SelectedValue = this.settings.GetValue("cbbKichBanP", "");
                this.nudSoLuotChay.Value = this.settings.GetValueInt("nudSoLuotChay", 5);
                this.nudDelayTurnFrom.Value = this.settings.GetValueInt("nudDelayTurnFrom", 2);
                this.nudDelayTurnTo.Value = this.settings.GetValueInt("nudDelayTurnTo", 5);
                this.ckbRepeatAll.Checked = this.settings.GetValueBool("ckbRepeatAll", false);
                this.plRepeat.Enabled = this.ckbRepeatAll.Checked;
            }
            catch
            {
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.settings.Update("ckbCheckliveTokenP", this.ckbCheckliveTokenP.Checked);
            this.settings.Update("cbbKichBanP", this.cbbKichBanP.SelectedValue);
            this.settings.Update("nudSoLuotChay", this.nudSoLuotChay.Value);
            this.settings.Update("nudDelayTurnFrom", this.nudDelayTurnFrom.Value);
            this.settings.Update("nudDelayTurnTo", this.nudDelayTurnTo.Value);
            this.settings.Update("ckbRepeatAll", this.ckbRepeatAll.Checked);
            this.settings.Save("");
            bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion("Lưu thành công, bạn có muốn đóng cửa sổ?") == DialogResult.Yes;
            if (flag)
            {
                base.Close();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ThemKichBan();
        }

        private void ThemKichBan()
        {
            try
            {
                Helpers.Common.ShowForm(new fThemKichBanPage(0, ""));   
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "open them kich ban page");
                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string kickBan = "";
            try
            {
                bool flag = this.cbbKichBanP.Items.Count > 0;
                if (flag)
                {
                    kickBan = this.cbbKichBanP.SelectedValue.ToString();
                }
            }
            catch
            {
            }
            Helpers.Common.ShowForm(new fThemKichBanPage(1, kickBan));
            this.LoadcbbKichBanP();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            this.XoaKichBan();
        }
        private void XoaKichBan()
        {
            try
            {
                string kickBan = "";
              
                    bool flag = this.cbbKichBanP.Items.Count > 0;
                    if (flag)
                    {
                        kickBan = this.cbbKichBanP.SelectedValue.ToString();

                        bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có chắc muốn xóa kịch bản này?") == DialogResult.Yes;
                        if (flag2)
                        {
                            bool flag3 = InteractSQL.DeleteKichBan(kickBan);
                            if (flag3)
                            {
                                this.LoadcbbKichBanP();
                            }
                            else
                            {
                                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
                            }
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox("Vui lòng thêm kịch bản trước!", 3);
                    }
                
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
            }
        }

        private void btnDup_Click(object sender, EventArgs e)
        {
            this.NhanBanKichBan();
        }

        private void NhanBanKichBan()
        {
            try
            {
                string id_KichBanCu = "";

                bool flag = this.cbbKichBanP.Items.Count > 0;
                if (flag)
                {
                    id_KichBanCu = this.cbbKichBanP.SelectedValue.ToString();
                    string str = this.cbbKichBanP.GetItemText(this.cbbKichBanP.SelectedItem).ToString();

                    string text = str + " - Copy";
                    int num = 2;
                    while (InteractSQL.CheckExistTenKichBan(text))
                    {
                        text = str + string.Format(" - Copy ({0})", num++);
                    }
                    bool flag2 = InteractSQL.DuplicateKichBan(id_KichBanCu, text);
                    if (flag2)
                    {
                        MessageBoxHelper.ShowMessageBox("Nhân bản thành công!", 1);
                        this.LoadcbbKichBanP();
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(("Có lỗi, vui lòng thử lại sau!"), 2);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng thêm kịch bản trước!"), 3);
                }
            }
            catch
            {
            }
        }

        private void ckbRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            this.plRepeat.Enabled = this.ckbRepeatAll.Checked;
        }
    }
}

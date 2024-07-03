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
    public partial class fChangeInfoConfig : Form
    {
        private JSON_Settings settings;

        public fChangeInfoConfig()
        {
            this.InitializeComponent();
            this.Load_cbbNgonNgu();
            this.settings = new JSON_Settings("configChange", false);
            this.LoadSettings();
            this.CheckedChangedFull();
        }
        private void Load_cbbNgonNgu()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = SetupFolder.GetListCountryCountryCode();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbNgonNgu.DataSource = new BindingSource(dictionary, null);
            this.cbbNgonNgu.ValueMember = "Key";
            this.cbbNgonNgu.DisplayMember = "Value";
        }

       

        private void btnClosed_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void CheckedChangedFull()
        {
            this.ckbGioiTinh_CheckedChanged(null, null);
            this.ckb2fa_CheckedChanged(null, null);
            this.ckbAddMail_CheckedChanged(null, null);
            this.ckbDoiPass_CheckedChanged(null, null);
            this.ckbDoiAvatar_CheckedChanged(null, null);
            this.ckbDoiAnhBia_CheckedChanged(null, null);
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCaiDatChung());
        }
        private void LoadSettings()
        {
            this.nudThread.Value = this.settings.GetValueInt("change_nudThread", 3);
            bool flag = this.settings.GetValueInt("typeLogin", 0) == 0;
            if (flag)
            {
                this.rbLoginUidPass.Checked = true;
            }
            else
            {
                this.rbLoginCookie.Checked = true;
            }
            bool flag5 = this.settings.GetValueInt("typeBrowserLogin", 0) == 0;
            if (flag5)
            {
                this.rbLoginMFB.Checked = true;
            }
            else
            {
                this.rbLoginWWW.Checked = true;
            }
            this.ckbCreateProfile.Checked = this.settings.GetValueBool("ckbCreateProfile", false);
            this.ckbDoiNgonNgu.Checked = this.settings.GetValueBool("change_ckbDoiNgonNgu", false);
            this.cbbNgonNgu.SelectedValue = this.settings.GetValue("change_cbbNgonNgu", "vi_VN");
            this.ckbDoiPass.Checked = this.settings.GetValueBool("change_ckbDoiPass", false);
            int typeChangePass = this.settings.GetValueInt("change_typeDoiPass", 0);
            bool flag1 = typeChangePass == 0;
            if (flag1)
            {
                this.rdPassTuNhap.Checked = true;
            }
            else
            {
                this.rdPassRandom.Checked = true;
            }
            this.ckbChangePassLinkHacked.Checked = this.settings.GetValueBool("change_ckbDoiPassUseLinkHacked", false);
            this.txtKeyDVFB.Text = this.settings.GetValue("change_txtKeyDVFB");
            this.txtKeyDVFB_TextChanged(null, null);
            this.cbbTypeDv.SelectedValue = this.settings.GetValue("change_cbbTypeDv");
            this.ckbXoaSdt.Checked = this.settings.GetValueBool("ckbXoaSdt", false);
            this.ckbLogOut.Checked = this.settings.GetValueBool("ckbLogOut", false);
            this.ckbXoaThietBiTinCay.Checked = this.settings.GetValueBool("change_ckbXoaThietBiTinCay", false);
            this.ckbGioiTinh.Checked = this.settings.GetValueBool("change_ckbGioiTinh", false);
            int typeGT = this.settings.GetValueInt("change_typeGioiTinh", 0);
            bool flag2 = typeGT == 0;
            if (flag2)
            {
                this.rbNu.Checked = true;
            }
            else
            {
                bool flag3 = typeGT == 1;
                if (flag3)
                {
                    this.rbNam.Checked = true;
                }
            }
            this.ckb2fa.Checked = this.settings.GetValueBool("change_ckb2fa", false);
            int type2fa = this.settings.GetValueInt("change_type2fa", 0);
            bool flag4 = type2fa == 0;
            if (flag4)
            {
                this.rdBat2fa.Checked = true;
            }
            else
            {
                this.rdTat2fa.Checked = true;
            }
            this.ckbAddMail.Checked = this.settings.GetValueBool("ckbAddMail", false);
            this.ckbMailDvFb.Checked = this.settings.GetValueBool("ckbMailDvFb", false);
            this.ckbMailCua.Checked = this.settings.GetValueBool("ckbMailCua", false);
            this.ckbAnMailMoi.Checked = this.settings.GetValueBool("ckbAnMailMoi", false);
            this.ckbXoaMailCu.Checked = this.settings.GetValueBool("ckbXoaMailCu", false);
            this.ckbCloseChrome.Checked = this.settings.GetValueBool("ckbCloseChrome", false);

            this.txtPathAvatar.Text = this.settings.GetValue("change_txtPathAvatar", "");
            this.txtPathCover.Text = this.settings.GetValue("change_txtPathCover", "");
            this.ckbDoiAvatar.Checked = this.settings.GetValueBool("change_ckbDoiAvatar", false);
            this.ckbAvatarThuTu.Checked = this.settings.GetValueBool("change_ckbAvatarThuTu", false);
            this.ckbDoiAnhBia.Checked = this.settings.GetValueBool("change_ckbDoiAnhBia", false);
            int valueInt = this.settings.GetValueInt("change_typeUpCover", 0);
            bool flag6 = valueInt == 0;
            if (flag6)
            {
                this.rdAnhNguoiDungDat.Checked = true;
            }
            //else
            //{
            //    this.rdAnhNgheThuat.Checked = true;
            //}
            this.ckbCoverThuTu.Checked = this.settings.GetValueBool("change_ckbCoverThuTu", false);
            this.ckbThemMoTa.Checked = this.settings.GetValueBool("change_ckbThemMoTa", false);
            this.ckbAutoDeleteFile.Checked = this.settings.GetValueBool("ckbAutoDeleteFile", false);

            this.ckbDoiNgaySinh.Checked = this.settings.GetValueBool("change_ckbDoiNgaySinh", false);
            this.nudNgayFrom.Value = this.settings.GetValueInt("change_nudNgayFrom", 1);
            this.nudNgayTo.Value = this.settings.GetValueInt("change_nudNgayTo", 30);
            this.nudThangFrom.Value = this.settings.GetValueInt("change_nudThangFrom", 1);
            this.nudThangTo.Value = this.settings.GetValueInt("change_nudThangTo", 12);
            this.nudNamFrom.Value = this.settings.GetValueInt("change_nudNamFrom", 1980);
            this.nudNamTo.Value = this.settings.GetValueInt("change_nudNamTo", 2000);
            this.ckbCapNhatThongTin.Checked = this.settings.GetValueBool("change_ckbCapNhatThongTin", false);
            this.ckbKhienAvt.Checked = this.settings.GetValueBool("change_ckbKhienAvt", false);

        }

        private void SaveSettings()
        {
            this.settings.Update("change_nudThread", Convert.ToInt32(this.nudThread.Value));
            bool typeLogin = this.rbLoginUidPass.Checked;
            if (typeLogin)
            {
                this.settings.Update("typeLogin", 0);
            }
            else
            {
                this.settings.Update("typeLogin", 1);
            }

            this.settings.Update("change_ckbDoiNgonNgu", this.ckbDoiNgonNgu.Checked);
            this.settings.Update("change_cbbNgonNgu", this.cbbNgonNgu.SelectedValue);
            this.settings.Update("change_ckbDoiPass", this.ckbDoiPass.Checked);
            int num1 = 0;
            bool typeChangePass = this.rdPassRandom.Checked;
            if (typeChangePass)
            {
                num1 = 1;
            }
            this.settings.Update("change_typeDoiPass", num1);
            this.settings.Update("change_ckbDoiPassUseLinkHacked", this.ckbChangePassLinkHacked.Checked);
            this.settings.Update("change_txtKeyDVFB", this.txtKeyDVFB.Text.Trim());
            this.settings.Update("change_cbbTypeDv", this.cbbTypeDv.SelectedValue);
            this.settings.Update("ckbLogOut", this.ckbLogOut.Checked.ToString());
            this.settings.Update("ckbXoaSdt", this.ckbXoaSdt.Checked.ToString());
            this.settings.Update("change_ckbXoaThietBiTinCay", this.ckbXoaThietBiTinCay.Checked);
            this.settings.Update("change_ckbGioiTinh", this.ckbGioiTinh.Checked);
            int num2 = 0;
            bool typeGT = this.rbNam.Checked;
            if (typeGT)
            {
                num2 = 1;
            }
            this.settings.Update("change_typeGioiTinh", num2);
            this.settings.Update("change_ckb2fa", this.ckb2fa.Checked);
            int num3 = 0;
            bool type2fa = this.rdTat2fa.Checked;
            if (type2fa)
            {
                num3 = 1;
            }
            this.settings.Update("change_type2fa", num3);
            this.settings.Update("themMail", 1);
            this.settings.Update("ckbAddMail", this.ckbAddMail.Checked.ToString());
            this.settings.Update("ckbMailDvFb", this.ckbMailDvFb.Checked.ToString());
            this.settings.Update("ckbMailCua", this.ckbMailCua.Checked.ToString());
            this.settings.Update("ckbAnMailMoi", this.ckbAnMailMoi.Checked.ToString());
            this.settings.Update("ckbXoaMailCu", this.ckbXoaMailCu.Checked.ToString());
            this.settings.Update("ckbCloseChrome", this.ckbCloseChrome.Checked.ToString());
            int num4 = this.rbLoginMFB.Checked ? 0 : 1;
            this.settings.Update("typeBrowserLogin", num4);
            this.settings.Update("ckbCreateProfile", this.ckbCreateProfile.Checked);

            this.settings.Update("change_txtPathAvatar", this.txtPathAvatar.Text);
            this.settings.Update("change_ckbAvatarThuTu", this.ckbAvatarThuTu.Checked);
            this.settings.Update("change_txtPathCover", this.txtPathCover.Text);
            this.settings.Update("change_ckbDoiAvatar", this.ckbDoiAvatar.Checked);
            this.settings.Update("change_ckbDoiAnhBia", this.ckbDoiAnhBia.Checked);
            int num = 0;
            //bool @checkedCv = this.rdAnhNgheThuat.Checked;
            //if (@checkedCv)
            //{
            //    num = 1;
            //}
            this.settings.Update("change_typeUpCover", num);
            this.settings.Update("change_ckbThemMoTa", this.ckbThemMoTa.Checked);
            this.settings.Update("change_ckbDoiNgaySinh", this.ckbDoiNgaySinh.Checked);
            this.settings.Update("change_nudNgayFrom", Convert.ToInt32(this.nudNgayFrom.Value));
            this.settings.Update("change_nudNgayTo", Convert.ToInt32(this.nudNgayTo.Value));
            this.settings.Update("change_nudThangFrom", Convert.ToInt32(this.nudThangFrom.Value));
            this.settings.Update("change_nudThangTo", Convert.ToInt32(this.nudThangTo.Value));
            this.settings.Update("change_nudNamFrom", Convert.ToInt32(this.nudNamFrom.Value));
            this.settings.Update("change_nudNamTo", Convert.ToInt32(this.nudNamTo.Value));
            this.settings.Update("change_ckbCapNhatThongTin", this.ckbCapNhatThongTin.Checked);
            this.settings.Update("change_ckbKhienAvt", this.ckbKhienAvt.Checked);

            //save 
            this.settings.Save("");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
                bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Lưu thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
                if (flag)
                {
                    base.Close();
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thử lại sau!"), 2);
            }
        }

        private void ckbDoiNgonNgu_CheckedChanged(object sender, EventArgs e)
        {
            this.cbbNgonNgu.Enabled = this.ckbDoiNgonNgu.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void rdPassTuNhap_CheckedChanged(object sender, EventArgs e)
        {
            this.btnNhapPass.Enabled = this.rdPassTuNhap.Checked;
        }

        private void btnNhapPass_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\doimk.txt", Language.GetValue("Nhập danh sách mật khẩu cần đổi"), Language.GetValue("Danh sách mật khẩu"), Language.GetValue("(Mỗi nội dung 1 dòng)"));
        }

        private void txtKeyDVFB_TextChanged(object sender, EventArgs e)
        {
            //string rsVnd = Helpers.CommonRequest.getAmountDongVanFb(this.txtKeyDVFB.Text.Trim());
            //if (rsVnd.StartsWith("success|"))
            //{
            //    string[] str = rsVnd.Split('|');
            //    this.lbAmountDVFB.Text = "Số dư: " + str[1] + " vnđ";
            //    this.loadTypeDVFB();
            //}
            //else
            //{
            //    MessageBoxHelper.ShowMessageBox(Language.GetValue("Key không lỗi hoặc không đúng. Vui lòng kiểm tra lại."), 2);
            //}
        }
        private void loadTypeDVFB()
        {
            Dictionary<string, string> dataSource = this.getListTypeDVFB();
            this.cbbTypeDv.DataSource = new BindingSource(dataSource, null);
            this.cbbTypeDv.ValueMember = "Key";
            this.cbbTypeDv.DisplayMember = "Value";
        }
        public Dictionary<string, string> getListTypeDVFB()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listTypeDVFB = Helpers.CommonRequest.GetTypeDongVanFb(this.settings.GetValue("change_txtKeyDVFB"));

            for (int i = 0; i < listTypeDVFB.Count; i++)
            {
                string[] array = listTypeDVFB[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            return dictionary;
        }

        private void ckbGioiTinh_CheckedChanged(object sender, EventArgs e)
        {
            this.panel5.Enabled = this.ckbGioiTinh.Checked;
        }

        private void ckb2fa_CheckedChanged(object sender, EventArgs e)
        {
            this.pl2fa.Enabled = this.ckb2fa.Checked;
        }

        private void ckbAddMail_CheckedChanged(object sender, EventArgs e)
        {
            this.plDoiMail.Enabled = this.ckbAddMail.Checked;

        }

        private void ckbDoiPass_CheckedChanged(object sender, EventArgs e)
        {
            this.plDoiMatKhau.Enabled = this.ckbDoiPass.Checked;
        }

        private void ckbDoiAvatar_CheckedChanged(object sender, EventArgs e)
        {
            this.plAvatar.Enabled = this.ckbDoiAvatar.Checked;
        }

        private void btnChosePathAvt_Click(object sender, EventArgs e)
        {
            this.txtPathAvatar.Text = Helpers.Common.SelectFolderNew();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.txtPathCover.Text = Helpers.Common.SelectFolderNew();
        }

        private void btnThemMoTa_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fNhapDuLieu2("configschange\\tieusu", "Nhập danh sách tiểu sử"));
        }

        private void ckbDoiAnhBia_CheckedChanged(object sender, EventArgs e)
        {
            this.plDoiAnhBia.Enabled = this.ckbDoiAnhBia.Checked;
        }

        private void ckbDoiNgaySinh_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Enabled = this.ckbDoiNgaySinh.Checked;
        }

        private void ckbCapNhatThongTin_CheckedChanged(object sender, EventArgs e)
        {
            this.panel3.Enabled = this.ckbCapNhatThongTin.Checked;
        }

        private void btnCapNhatThongTin_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\thongtincanhan\\NoiLamViec.txt", "Nhập danh sách Nơi làm việc", "Danh sách từ khóa", "(Mỗi nội dung 1 dòng)");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\thongtincanhan\\QueQuan.txt", "Nhập danh sách Quê quán", "Danh sách từ khóa", "(Mỗi nội dung 1 dòng)");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\thongtincanhan\\ThanhPho.txt", "Nhập danh sách Thành phố", "Danh sách từ khóa", "(Mỗi nội dung 1 dòng)");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\thongtincanhan\\TruongDH.txt", "Nhập danh sách Trường ĐH", "Danh sách từ khóa", "(Mỗi nội dung 1 dòng)");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Helpers.Common.OpenFileAndPressData("configschange\\thongtincanhan\\TruongTHPT.txt", "Nhập danh sách Trường THPT", "Danh sách từ khóa", "(Mỗi nội dung 1 dòng)");
        }
    }
}

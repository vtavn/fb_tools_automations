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
    public partial class fCaiDatHienThi : Form
    {
        public fCaiDatHienThi()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configDatagridview", false);
        }

        private JSON_Settings settings;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.settings.Update("cToken", this.ckbToken.Checked);
            this.settings.Update("ckbCookie", this.ckbCookie.Checked);
            this.settings.Update("ckbEmail", this.ckbEmail.Checked);
            this.settings.Update("ckbTen", this.ckbTen.Checked);
            this.settings.Update("ckbBanBe", this.ckbBanBe.Checked);
            this.settings.Update("ckbNhom", this.ckbNhom.Checked);
            this.settings.Update("ckbPage", this.ckbPage.Checked);
            this.settings.Update("ckbNgaySinh", this.ckbNgaySinh.Checked);
            this.settings.Update("ckbGioiTinh", this.ckbGioiTinh.Checked);
            this.settings.Update("ckbMatKhau", this.ckbMatKhau.Checked);
            this.settings.Update("ckbMatKhauMail", this.ckbMatKhauMail.Checked);
            this.settings.Update("ckbBackup", this.ckbBackup.Checked);
            this.settings.Update("ckbMa2FA", this.ckbMa2FA.Checked);
            this.settings.Update("ckbUseragent", this.ckbUseragent.Checked);
            this.settings.Update("ckbProxy", this.ckbProxy.Checked);
            this.settings.Update("ckbNgayTao", this.ckbNgayTao.Checked);
            this.settings.Update("ckbAvatar", this.ckbAvatar.Checked);
            this.settings.Update("ckbProfile", this.ckbProfile.Checked);
            this.settings.Update("ckbTinhTrang", this.ckbTinhTrang.Checked);
            this.settings.Update("ckbThuMuc", this.ckbThuMuc.Checked);
            this.settings.Update("ckbGhiChu", this.ckbGhiChu.Checked);
            this.settings.Update("ckbFollow", this.ckbFollow.Checked);
            this.settings.Update("ckbInteractEnd", this.ckbInteractEnd.Checked);
            this.settings.Update("cMailRecovery", this.cMailRecovery.Checked);
            this.settings.Update("cStatus282", this.cStatus282.Checked);

            //page
            this.settings.Update("ckbPLike", this.ckbPLike.Checked);
            this.settings.Update("ckbPFollow", this.ckbPFollow.Checked);
            this.settings.Update("ckbPTiepcan", this.ckbPTiepcan.Checked);
            this.settings.Update("ckbPTuongtac", this.ckbPTuongtac.Checked);
            this.settings.Update("ckbPNgaytao", this.ckbPNgaytao.Checked);
            this.settings.Update("ckbPAvtcover", this.ckbPAvtcover.Checked);
            this.settings.Update("ckbPFolder", this.ckbPFolder.Checked);
            this.settings.Update("ckbPGroup", this.ckbPGroup.Checked);

            this.settings.Save("");
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fCaiDatHienThi_Load(object sender, EventArgs e)
        {
            this.ckbToken.Checked = this.settings.GetValueBool("cToken", false);
            this.ckbCookie.Checked = this.settings.GetValueBool("ckbCookie", false);
            this.ckbEmail.Checked = this.settings.GetValueBool("ckbEmail", false);
            this.ckbTen.Checked = this.settings.GetValueBool("ckbTen", false);
            this.ckbBanBe.Checked = this.settings.GetValueBool("ckbBanBe", false);
            this.ckbNhom.Checked = this.settings.GetValueBool("ckbNhom", false);
            this.ckbPage.Checked = this.settings.GetValueBool("ckbPage", false);
            this.ckbNgaySinh.Checked = this.settings.GetValueBool("ckbNgaySinh", false);
            this.ckbGioiTinh.Checked = this.settings.GetValueBool("ckbGioiTinh", false);
            this.ckbMatKhau.Checked = this.settings.GetValueBool("ckbMatKhau", false);
            this.ckbMatKhauMail.Checked = this.settings.GetValueBool("ckbMatKhauMail", false);
            this.ckbBackup.Checked = this.settings.GetValueBool("ckbBackup", false);
            this.ckbMa2FA.Checked = this.settings.GetValueBool("ckbMa2FA", false);
            this.ckbUseragent.Checked = this.settings.GetValueBool("ckbUseragent", false);
            this.ckbProxy.Checked = this.settings.GetValueBool("ckbProxy", false);
            this.ckbNgayTao.Checked = this.settings.GetValueBool("ckbNgayTao", false);
            this.ckbAvatar.Checked = this.settings.GetValueBool("ckbAvatar", false);
            this.ckbProfile.Checked = this.settings.GetValueBool("ckbProfile", false);
            this.ckbTinhTrang.Checked = this.settings.GetValueBool("ckbTinhTrang", false);
            this.ckbThuMuc.Checked = this.settings.GetValueBool("ckbThuMuc", false);
            this.ckbGhiChu.Checked = this.settings.GetValueBool("ckbGhiChu", false);
            this.ckbFollow.Checked = this.settings.GetValueBool("ckbFollow", false);
            this.ckbInteractEnd.Checked = this.settings.GetValueBool("ckbInteractEnd", false);
            this.cMailRecovery.Checked = this.settings.GetValueBool("cMailRecovery", false);
            this.cStatus282.Checked = this.settings.GetValueBool("cStatus282", false);

            //page
            this.ckbPLike.Checked = this.settings.GetValueBool("ckbPLike", false);
            this.ckbPFollow.Checked = this.settings.GetValueBool("ckbPFollow", false);
            this.ckbPTiepcan.Checked = this.settings.GetValueBool("ckbPTiepcan", false);
            this.ckbPTuongtac.Checked = this.settings.GetValueBool("ckbPTuongtac", false);
            this.ckbPNgaytao.Checked = this.settings.GetValueBool("ckbPNgaytao", false);
            this.ckbPAvtcover.Checked = this.settings.GetValueBool("ckbPAvtcover", false);
            this.ckbPFolder.Checked = this.settings.GetValueBool("ckbPFolder", false);
            this.ckbPGroup.Checked = this.settings.GetValueBool("ckbPGroup", false);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

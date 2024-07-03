using core.KichBan;
using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core.fKichBan
{
    public partial class fHDDangStatus : Form
    {
        private JSON_Settings setting;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDDangStatus(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fHDDangStatus.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "HDDangStatus").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"CauHinh\", \"MoTa\") VALUES ('HDDangStatus', '{  \"ckbVanBan\": \"False\",\"txtNoiDung\": \"\",   \"ckbAnh\": \"False\",\"txtPathAnh\":\"\",\"nudSoLuongAnh\": \"1\",  \"ckbVideo\": \"False\",\"txtPathVideo\":\"\",\"nudSoLuongVideo\": \"1\"}', 'Đăng status');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "HDDangStatus");
                jsonStringOrPathFile = tuongTac.Rows[0]["CauHinh"].ToString();
                this.id_TuongTac = tuongTac.Rows[0]["Id_TuongTac"].ToString();
                this.txtTenHanhDong.Text = Language.GetValue(tuongTac.Rows[0]["MoTa"].ToString());
            }
            else
            {
                bool flag3 = type == 1;
                if (flag3)
                {
                    DataTable hanhDongById = InteractSQL.GetHanhDongById(id_HanhDong);
                    jsonStringOrPathFile = hanhDongById.Rows[0]["CauHinh"].ToString();
                    this.btnAdd.Text = Language.GetValue("Cập nhật");
                    this.txtTenHanhDong.Text = hanhDongById.Rows[0]["TenHanhDong"].ToString();
                }
            }
            this.setting = new JSON_Settings(jsonStringOrPathFile, true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fHDDangStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudSoLuongFrom.Value = this.setting.GetValueInt("nudSoLuongFrom", 1);
                this.nudSoLuongTo.Value = this.setting.GetValueInt("nudSoLuongTo", 1);
                this.nudKhoangCachFrom.Value = this.setting.GetValueInt("nudKhoangCachFrom", 5);
                this.nudKhoangCachTo.Value = this.setting.GetValueInt("nudKhoangCachTo", 10);
                this.ckbVanBan.Checked = this.setting.GetValueBool("ckbVanBan", false);
                this.ckbUseBackground.Checked = this.setting.GetValueBool("ckbUseBackground", false);
                this.txtNoiDung.Text = this.setting.GetValue("txtNoiDung", "");
                this.ckbAnh.Checked = this.setting.GetValueBool("ckbAnh", false);
                this.ckbVideo.Checked = this.setting.GetValueBool("ckbVideo", false);
                this.nudSoLuongAnhFrom.Value = this.setting.GetValueInt("nudSoLuongAnhFrom", 1);
                this.nudSoLuongAnhTo.Value = this.setting.GetValueInt("nudSoLuongAnhTo", 1);
                this.nudSoLuongVideoFrom.Value = this.setting.GetValueInt("nudSoLuongVideoFrom", 1);
                this.nudSoLuongVideoTo.Value = this.setting.GetValueInt("nudSoLuongVideoTo", 1);
                this.txtPathAnh.Text = this.setting.GetValue("txtPathAnh", "");
                this.txtPathVideo.Text = this.setting.GetValue("txtPathVideo", "");
                bool flag = this.setting.GetValueInt("typeNganCach", 0) == 1;
                if (flag)
                {
                    this.rbNganCachKyTu.Checked = true;
                }
                else
                {
                    this.rbNganCachMoiDong.Checked = true;
                }
                bool flag2 = this.setting.GetValueInt("typeUidTag", 0) == 0;
                if (flag2)
                {
                    this.rbUidBanBe.Checked = true;
                }
                else
                {
                    this.rbUidTuNhap.Checked = true;
                }
                this.ckbTag.Checked = this.setting.GetValueBool("ckbTag", false);
                this.txtUidTag.Text = this.setting.GetValue("txtUidTag", "");
                this.nudUidFrom.Value = this.setting.GetValueInt("nudUidFrom", 1);
                this.nudUidTo.Value = this.setting.GetValueInt("nudUidTo", 1);
            }
            catch (Exception ex)
            {
            }
            this.CheckedChangeFull();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtTenHanhDong.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập tên hành động!"), 3);
            }
            else
            {
                bool flag2 = this.ckbAnh.Checked && !Directory.Exists(this.txtPathAnh.Text.Trim());
                if (flag2)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Đường dẫn ảnh không tồn tại!"), 3);
                }
                else
                {
                    bool flag3 = this.ckbVideo.Checked && !Directory.Exists(this.txtPathVideo.Text.Trim());
                    if (flag3)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Đường dẫn video không tồn tại!"), 3);
                    }
                    else
                    {
                        JSON_Settings json_Settings = new JSON_Settings();
                        json_Settings.Update("nudSoLuongFrom", this.nudSoLuongFrom.Value);
                        json_Settings.Update("nudSoLuongTo", this.nudSoLuongTo.Value);
                        json_Settings.Update("nudKhoangCachFrom", this.nudKhoangCachFrom.Value);
                        json_Settings.Update("nudKhoangCachTo", this.nudKhoangCachTo.Value);
                        json_Settings.Update("ckbVanBan", this.ckbVanBan.Checked);
                        json_Settings.Update("ckbUseBackground", this.ckbUseBackground.Checked);
                        json_Settings.Update("ckbAnh", this.ckbAnh.Checked);
                        json_Settings.Update("ckbVideo", this.ckbVideo.Checked);
                        json_Settings.Update("nudSoLuongAnhFrom", this.nudSoLuongAnhFrom.Value);
                        json_Settings.Update("nudSoLuongAnhTo", this.nudSoLuongAnhTo.Value);
                        json_Settings.Update("nudSoLuongVideoFrom", this.nudSoLuongVideoFrom.Value);
                        json_Settings.Update("nudSoLuongVideoTo", this.nudSoLuongVideoTo.Value);
                        json_Settings.Update("txtNoiDung", this.txtNoiDung.Text.Trim());
                        json_Settings.Update("txtPathAnh", this.txtPathAnh.Text.Trim());
                        json_Settings.Update("txtPathVideo", this.txtPathVideo.Text.Trim());
                        int num = 0;
                        bool @checked = this.rbNganCachKyTu.Checked;
                        if (@checked)
                        {
                            num = 1;
                        }
                        json_Settings.Update("typeNganCach", num);
                        bool checked2 = this.rbUidBanBe.Checked;
                        if (checked2)
                        {
                            json_Settings.Update("typeUidTag", 0);
                        }
                        else
                        {
                            bool checked3 = this.rbUidTuNhap.Checked;
                            if (checked3)
                            {
                                json_Settings.Update("typeUidTag", 1);
                            }
                        }
                        json_Settings.Update("ckbTag", this.ckbTag.Checked);
                        json_Settings.Update("txtUidTag", this.txtUidTag.Text.Trim());
                        json_Settings.Update("nudUidFrom", this.nudUidFrom.Value);
                        json_Settings.Update("nudUidTo", this.nudUidTo.Value);
                        string fullString = json_Settings.GetFullString();
                        bool flag4 = this.type == 0;
                        if (flag4)
                        {
                            bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn thêm hành động mới?")) == DialogResult.Yes;
                            if (flag5)
                            {
                                bool flag6 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                                if (flag6)
                                {
                                    fHDDangStatus.isSave = true;
                                    base.Close();
                                }
                                else
                                {
                                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Thêm thất bại, vui lòng thử lại sau!"), 2);
                                }
                            }
                        }
                        else
                        {
                            bool flag7 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn cập nhật hành động?")) == DialogResult.Yes;
                            if (flag7)
                            {
                                bool flag8 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                                if (flag8)
                                {
                                    fHDDangStatus.isSave = true;
                                    base.Close();
                                }
                                else
                                {
                                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Cập nhật thất bại, vui lòng thử lại sau!"), 2);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckedChangeFull()
        {
            this.ckbVanBan_CheckedChanged(null, null);
            this.ckbAnh_CheckedChanged(null, null);
            this.ckbVideo_CheckedChanged(null, null);
            this.ckbTag_CheckedChanged(null, null);
            this.rbUidTuNhap_CheckedChanged(null, null);
        }

        private void ckbVanBan_CheckedChanged(object sender, EventArgs e)
        {
            this.plVanBan.Enabled = this.ckbVanBan.Checked;
            bool flag = !this.ckbVanBan.Checked;
            if (flag)
            {
                this.ckbUseBackground.Checked = false;
            }
        }

        private void ckbAnh_CheckedChanged(object sender, EventArgs e)
        {
            this.plAnh.Enabled = this.ckbAnh.Checked;
            bool @checked = this.ckbAnh.Checked;
            if (@checked)
            {
                this.ckbUseBackground.Checked = false;
            }
        }

        private void ckbVideo_CheckedChanged(object sender, EventArgs e)
        {
            this.plVideo.Enabled = this.ckbVideo.Checked;
            bool @checked = this.ckbVideo.Checked;
            if (@checked)
            {
                this.ckbUseBackground.Checked = false;
            }
        }

        private void txtNoiDung_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }
        private void UpdateSoLuongBinhLuan()
        {
            try
            {
                List<string> list = new List<string>();
                bool @checked = this.rbNganCachMoiDong.Checked;
                if (@checked)
                {
                    list = this.txtNoiDung.Lines.ToList<string>();
                }
                else
                {
                    list = this.txtNoiDung.Text.Split(new string[]
                    {
                        "[r]"
                    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblStatus.Text = string.Format(Language.GetValue("Danh sách nội dung ({0}):"), list.Count.ToString());
            }
            catch
            {
            }
        }
        private void UpdateSoLuongUid()
        {
            try
            {
                List<string> list = new List<string>();
                list = this.txtUidTag.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.label4.Text = string.Format(Language.GetValue("Danh sách Uid ({0}):"), list.Count.ToString());
            }
            catch
            {
            }
        }

        private void rbNganCachMoiDong_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void rbNganCachKyTu_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void ckbTag_CheckedChanged(object sender, EventArgs e)
        {
            this.plTag.Enabled = this.ckbTag.Checked;
        }

        private void txtUidTag_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongUid();
        }

        private void rbUidTuNhap_CheckedChanged(object sender, EventArgs e)
        {
            this.plUidTuNhap.Enabled = this.rbUidTuNhap.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập mỗi dòng là 1 nội dung!"), 1);
            this.txtNoiDung.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Nội dung phân cách mỗi dòng bằng đấu | ."), 1);
            this.txtNoiDung.Focus();
        }
    }
}

using Facebook_Tool_Request.core.KichBanPage;
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

namespace Facebook_Tool_Request.core.KichBanPage
{
    public partial class fSpamCmtPage : Form
    {
        private JSON_Settings setting;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fSpamCmtPage(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fSpamCmtPage.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            string text = "SpamCmtPage";
            string text2 = "Spam Comment Uid Page5";
            bool flag = InteractSQL.GetTuongTac("", "SpamCmtPage").Rows.Count == 0;
            if (flag)
            {
                string sql = string.Concat(new string[]
                 {
                    "INSERT INTO \"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('",
                    text,
                    "', '",
                    text2,
                    "');"
                 });

                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery(sql);
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "SpamCmtPage");
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
                bool @checked = this.rbFileUid.Checked;
                if (@checked)
                {
                    bool flag4 = this.txtPathFileUid.Text.Trim() == "";
                    if (flag4)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn File Uid!"), 3);
                        return;
                    }
                }
                else
                {
                    List<string> list = this.txtlistUid.Lines.ToList<string>();
                    list = Helpers.Common.RemoveEmptyItems(list);
                    bool flag5 = list.Count == 0;
                    if (flag5)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập danh sách Uid!"), 3);
                        return;
                    }
                }
                bool flag2 = this.ckbAnh.Checked && !Directory.Exists(this.txtPathAnh.Text.Trim());
                if (flag2)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Đường dẫn ảnh không tồn tại!"), 3);
                }
                else
                {
                    JSON_Settings json_Settings = new JSON_Settings();
                    json_Settings.Update("nudKhoangCachFrom", this.nudKhoangCachFrom.Value);
                    json_Settings.Update("nudPostUid", this.nudPostUid.Value);
                    json_Settings.Update("nudCountUid", this.nudCountUid.Value);
                    json_Settings.Update("nudKhoangCachTo", this.nudKhoangCachTo.Value);
                    json_Settings.Update("ckbAnh", this.ckbAnh.Checked);
                    json_Settings.Update("ckbRemoveUid", this.ckbRemoveUid.Checked);
                    json_Settings.Update("txtNoiDung", this.txtNoiDung.Text.Trim());
                    json_Settings.Update("txtPathAnh", this.txtPathAnh.Text.Trim());
                    json_Settings.Update("txtUid", this.txtlistUid.Text.Trim());
                    int num = 0;
                    bool @checked2 = this.rbNganCachKyTu.Checked;
                    if (@checked2)
                    {
                        num = 1;
                    }
                    json_Settings.Update("typeNganCach", num);

                    bool checked4 = this.rbFileUid.Checked;

                    if (checked4)
                    {
                        json_Settings.Update("typeListUid", 1);
                    }
                    else
                    {
                        json_Settings.Update("typeListUid", 0);
                    }

                    json_Settings.Update("txtPathFileUid", this.txtPathFileUid.Text.Trim());
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
                                fSpamCmtPage.isSave = true;
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
                                fSpamCmtPage.isSave = true;
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
                        "\n|\n"
                    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblStatus.Text = string.Format(Language.GetValue("Danh sách nội dung ({0}):"), list.Count.ToString());
            }
            catch
            {
            }
        }
        private void txtNoiDung_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void rbNganCachMoiDong_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void rbNganCachKyTu_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHuongDanContent());
        }

        private void ckbAnh_CheckedChanged(object sender, EventArgs e)
        {
            this.plAnh.Enabled = this.ckbAnh.Checked;
        }

        private void btnInsPathImg_Click(object sender, EventArgs e)
        {
            this.txtPathAnh.Text = Helpers.Common.SelectFolderNew("Chọn Folder => Ấn Open => OK");
        }
        private void countUidFromfile()
        {
            try
            {
                string[] uidArray = File.ReadAllLines(txtPathFileUid.Text);
                rbFileUid.Text = "Nhập từ File (" + uidArray.Length + ")";
            }
            catch
            { }
        }
        private void txtPathFileUid_TextChanged(object sender, EventArgs e)
        {
            this.countUidFromfile();
        }

        private void rbListUid_CheckedChanged(object sender, EventArgs e)
        {
            this.txtlistUid.Enabled = this.rbListUid.Checked;
        }
        private void UpdateSoLuongUid()
        {
            try
            {
                List<string> list = this.txtlistUid.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.rbListUid.Text = string.Format(Language.GetValue("Nhập (Mỗi Uid 1 dòng) ({0}):"), list.Count.ToString());
            }
            catch
            {
            }
        }
        private void txtlistUid_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongUid();
        }

        private void btnChooseFileUid_Click(object sender, EventArgs e)
        {
            this.txtPathFileUid.Text = Helpers.Common.SelectFile("Chọn File txt", "txt Files (*.txt)|*.txt|");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fSpamCmtPage_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudKhoangCachFrom.Value = this.setting.GetValueInt("nudKhoangCachFrom", 5);
                this.nudKhoangCachTo.Value = this.setting.GetValueInt("nudKhoangCachTo", 10);
                this.txtNoiDung.Text = this.setting.GetValue("txtNoiDung", "");
                this.txtlistUid.Text = this.setting.GetValue("txtUid", "");
                this.ckbAnh.Checked = this.setting.GetValueBool("ckbAnh", false);
                this.ckbRemoveUid.Checked = this.setting.GetValueBool("ckbRemoveUid", false);
                this.txtPathAnh.Text = this.setting.GetValue("txtPathAnh", "");
                this.txtPathFileUid.Text = this.setting.GetValue("txtPathFileUid", "");
                this.nudPostUid.Value = this.setting.GetValueInt("nudPostUid", 3);
                this.nudCountUid.Value = this.setting.GetValueInt("nudCountUid", 5);

                bool flag = this.setting.GetValueInt("typeNganCach", 0) == 1;
                if (flag)
                {
                    this.rbNganCachKyTu.Checked = true;
                }
                else
                {
                    this.rbNganCachMoiDong.Checked = true;
                }

                int valueInt = this.setting.GetValueInt("typeListUid", 0);
                bool flag2 = valueInt == 1;
                if (flag2)
                {
                    this.rbFileUid.Checked = true;
                }
                else
                {
                    this.rbListUid.Checked = true;
                }
            }
            catch (Exception ex)
            {
            }
            this.CheckedChangeFull();
        }
        private void CheckedChangeFull()
        {
            this.ckbAnh_CheckedChanged(null, null);
            this.rbFileUid_CheckedChanged(null, null);
            this.rbListUid_CheckedChanged(null, null);
            this.txtPathFileUid_TextChanged(null, null);
        }

        private void rbFileUid_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPathFileUid.Enabled = this.rbFileUid.Checked;
            this.btnChooseFileUid.Enabled = this.rbFileUid.Checked;
        }
    }
}

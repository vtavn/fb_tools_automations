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
    public partial class fSeedingFb : Form
    {
        private JSON_Settings setting = null;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fSeedingFb(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fSeedingFb.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "SeedingFb").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('SeedingFb', 'Seeding FB');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "SeedingFb");
                jsonStringOrPathFile = tuongTac.Rows[0]["CauHinh"].ToString();
                this.id_TuongTac = tuongTac.Rows[0]["Id_TuongTac"].ToString();
                this.txtTenHanhDong.Text = tuongTac.Rows[0]["MoTa"].ToString();
            }
            else
            {
                bool flag3 = type == 1;
                if (flag3)
                {
                    DataTable hanhDongById = InteractSQL.GetHanhDongById(id_HanhDong);
                    jsonStringOrPathFile = hanhDongById.Rows[0]["CauHinh"].ToString();
                    this.btnAdd.Text = "Cập nhật";
                    this.txtTenHanhDong.Text = hanhDongById.Rows[0]["TenHanhDong"].ToString();
                }
            }
            this.setting = new JSON_Settings(jsonStringOrPathFile, true);
        }

        private void gunaLabel2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ckbComment_CheckedChanged(object sender, EventArgs e)
        {
            grCommentFb.Enabled = ckbComment.Checked;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm2(new fHuongDanContent());
        }

        private void txtListComment_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox("Vui lòng nhập mỗi dòng là 1 nội dung!", 1);
            this.txtNoiDung.Focus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox("Nội dung phân cách mỗi dòng bằng đấu | .", 1);
            this.txtNoiDung.Focus();
        }
        private void rbNganCachKyTu_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }
        private void rbNganCachMoiDong_CheckedChanged(object sender, EventArgs e)
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
                        "\r\n|\r\n"
                    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lbNoidungCmt.Text = string.Format("Danh sách Comment ({0}):", list.Count.ToString());
            }
            catch
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //get log
        }

        private void ckbLike_CheckedChanged(object sender, EventArgs e)
        {
            grLike.Enabled = ckbLike.Checked;
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            this.txtPathImg.Text = Helpers.Common.SelectFolderNew("Chọn Folder => Ấn Open => OK");
        }

        private void ckbCmtImg_CheckedChanged(object sender, EventArgs e)
        {
            txtPathImg.Enabled = ckbCmtImg.Checked;
            gunaButton1.Enabled = ckbCmtImg.Checked;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtTenHanhDong.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng nhập tên hành động!", 3);
            }
            else
            {
                bool @comment = this.ckbComment.Checked;
                if (@comment)
                {
                    bool checkContentCmt = this.txtNoiDung.Text.Trim() == "";
                    if (checkContentCmt)
                    {
                        MessageBoxHelper.ShowMessageBox("Nhập nội dung comment đi!", 3);
                        return;
                    }
                }
                bool @cmtImg = this.ckbCmtImg.Checked && !Directory.Exists(this.txtPathImg.Text.Trim());
                if (@cmtImg)
                {
                    MessageBoxHelper.ShowMessageBox("Đường dẫn ảnh không tồn tại!", 3);
                    return;
                }
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("txtIdBaiViet", this.txtIdBaiViet.Text.Trim());
                json_Settings.Update("nudPage", this.nudPage.Value);
                json_Settings.Update("nudDelayFrom", this.nudDelayFrom.Value);
                json_Settings.Update("nudDelayTo", this.nudDelayTo.Value);
                json_Settings.Update("ckbComment", this.ckbComment.Checked);
                json_Settings.Update("ckbLike", this.ckbLike.Checked);
                json_Settings.Update("ckbShare", this.ckbShare.Checked);
                //cmt
                json_Settings.Update("ckbCmtLog", this.ckbCmtLog.Checked);
                json_Settings.Update("ckbCmtDeleteContent", this.ckbCmtDeleteContent.Checked);
                json_Settings.Update("ckbCmtImg", this.ckbCmtImg.Checked);
                json_Settings.Update("txtPathImg", this.txtPathImg.Text.Trim());
                json_Settings.Update("txtNoiDung", this.txtNoiDung.Text.Trim());
                int num = 0;
                bool @checked2 = this.rbNganCachKyTu.Checked;
                if (@checked2) num = 1;
                json_Settings.Update("typeNganCach", num);
                //like
                json_Settings.Update("ckbLikeLog", this.ckbLikeLog.Checked);
                if (this.ckbLike.Checked)
                {
                    string reaction = cbbReaction.Text.ToLower();
                    if (string.IsNullOrEmpty(reaction))
                    {
                        MessageBoxHelper.ShowMessageBox("Vui lòng chọn cảm xúc seeding!", 3);
                        return;
                    }
                    json_Settings.Update("ckbLikeType", reaction);
                }

                string fullString = json_Settings.GetFullString();
                bool flag2 = this.type == 0;
                if (flag2)
                {
                    bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có muốn thêm hành động mới?") == DialogResult.Yes;
                    if (flag5)
                    {
                        bool flag6 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                        if (flag6)
                        {
                            fSeedingFb.isSave = true;
                            base.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox("Thêm thất bại, vui lòng thử lại sau!", 2);
                        }
                    }
                }
                else
                {
                    bool flag7 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có muốn cập nhật hành động?") == DialogResult.Yes;
                    if (flag7)
                    {
                        bool flag8 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                        if (flag8)
                        {
                            fSeedingFb.isSave = true;
                            base.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox("Cập nhật thất bại, vui lòng thử lại sau!", 2);
                        }
                    }
                }
            }
        }

        private void fSeedingFb_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtIdBaiViet.Text = this.setting.GetValue("txtIdBaiViet", "");
                this.nudPage.Value = this.setting.GetValueInt("nudPage", 5);
                this.nudDelayFrom.Value = this.setting.GetValueInt("nudDelayFrom", 1);
                this.nudDelayTo.Value = this.setting.GetValueInt("nudDelayTo", 2);
                this.txtNoiDung.Text = this.setting.GetValue("txtNoiDung", "");
                this.ckbComment.Checked = this.setting.GetValueBool("ckbComment", false);
                this.ckbLike.Checked = this.setting.GetValueBool("ckbLike", false);
                this.ckbShare.Checked = this.setting.GetValueBool("ckbShare", false);
                this.ckbLikeLog.Checked = this.setting.GetValueBool("ckbLikeLog", false);
                this.ckbCmtLog.Checked = this.setting.GetValueBool("ckbCmtLog", false);
                this.ckbCmtDeleteContent.Checked = this.setting.GetValueBool("ckbCmtDeleteContent", false);
                this.ckbCmtImg.Checked = this.setting.GetValueBool("ckbCmtImg", false);
                this.txtPathImg.Text = this.setting.GetValue("txtPathImg", "");

                //reaction type 
                string reactionType = this.setting.GetValue("ckbLikeType", "random");
                if(!string.IsNullOrEmpty(reactionType))
                {
                    string str1 = reactionType.Substring(0, 1);
                    string str2 = reactionType.Substring(1);
                    str1 = str1.ToUpper();
                    reactionType = str1 + str2;
                    cbbReaction.SelectedItem = reactionType;
                }

                bool flag = this.setting.GetValueInt("typeNganCach", 0) == 1;
                if (flag)
                {
                    this.rbNganCachKyTu.Checked = true;
                }
                else
                {
                    this.rbNganCachMoiDong.Checked = true;
                }
            }
            catch (Exception ex)
            {
            }
            this.CheckedChangeFull();
        }
        private void CheckedChangeFull()
        {
            this.ckbCmtImg_CheckedChanged(null, null);
            this.ckbLike_CheckedChanged(null, null);
        }

        private void txtNoiDung_TextChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }
    }
}

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
    public partial class fHDPostStatus : Form
    {
        private JSON_Settings setting = null;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDPostStatus(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fSeedingFb.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "HDPostStatus").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('HDPostStatus', 'Đăng bài viết cá nhân!');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "HDPostStatus");
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

        private void btnInsPathImg_Click(object sender, EventArgs e)
        {
            this.txtPathImg.Text = Helpers.Common.SelectFolderNew("Chọn Folder => Ấn Open => OK");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
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
                bool @postWall = this.ckbPostWall.Checked;
                if (@postWall)
                {
                    bool checkContentCmt = this.txtNoidung.Text.Trim() == "";
                    if (checkContentCmt)
                    {
                        MessageBoxHelper.ShowMessageBox("Nhập nội dung comment đi!", 3);
                        return;
                    }
                }
                bool @cmtImg = this.ckbPostImg.Checked && !Directory.Exists(this.txtPathImg.Text.Trim());
                if (@cmtImg)
                {
                    MessageBoxHelper.ShowMessageBox("Đường dẫn ảnh không tồn tại!", 3);
                    return;
                }
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("txtNoidung", this.txtNoidung.Text.Trim());
                json_Settings.Update("ckbPostWall", this.ckbPostWall.Checked);
                json_Settings.Update("ckbPostImg", this.ckbPostImg.Checked);
                json_Settings.Update("txtPathImg", this.txtPathImg.Text.Trim());
                json_Settings.Update("nudPost", this.nudPost.Text.Trim());
                json_Settings.Update("nudImgPost", this.nudImgPost.Text.Trim());
                json_Settings.Update("ckbTagFr", this.ckbTagFr.Checked);
                json_Settings.Update("nudTagFr", this.nudTagFr.Text.Trim());
                json_Settings.Update("nudDelayFrom", this.nudDelayFrom.Value);
                json_Settings.Update("nudDelayTo", this.nudDelayTo.Value);
                json_Settings.Update("ckbPinPost", this.ckbPinPost.Checked);
                json_Settings.Update("ckbPostVideo", this.ckbPostVideo.Checked);
                json_Settings.Update("txtPathVideo", this.txtPathVideo.Text.Trim());

                string fullString = json_Settings.GetFullString();
                bool flag2 = this.type == 0;
                if (flag2)
                {
                    bool flag3 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có muốn thêm hành động mới?") == DialogResult.Yes;
                    if (flag3)
                    {
                        bool flag4 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                        if (flag4)
                        {
                            fHDPostStatus.isSave = true;
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
                    bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có muốn cập nhật hành động?") == DialogResult.Yes;
                    if (flag5)
                    {
                        bool flag6 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                        if (flag6)
                        {
                            fHDPostStatus.isSave = true;
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

        private void fHDPostStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudDelayFrom.Value = this.setting.GetValueInt("nudDelayFrom", 1);
                this.nudDelayTo.Value = this.setting.GetValueInt("nudDelayTo", 2);
                this.txtNoidung.Text = this.setting.GetValue("txtNoidung", "");
                this.ckbPostWall.Checked = this.setting.GetValueBool("ckbPostWall", false);
                this.ckbPinPost.Checked = this.setting.GetValueBool("ckbPinPost", false);
                this.ckbPostImg.Checked = this.setting.GetValueBool("ckbPostImg", false);
                this.ckbTagFr.Checked = this.setting.GetValueBool("ckbTagFr", false);
                this.txtPathImg.Text = this.setting.GetValue("txtPathImg", "");
                this.nudPost.Text = this.setting.GetValue("nudPost", "1");
                this.nudImgPost.Text = this.setting.GetValue("nudImgPost", "3");
                this.nudTagFr.Text = this.setting.GetValue("nudTagFr", "");
                this.ckbPostVideo.Checked = this.setting.GetValueBool("ckbPostVideo", false);
                this.txtPathVideo.Text = this.setting.GetValue("txtPathVideo", "");
            }
            catch (Exception ex)
            {
            }
        }

        private void btnInsPathVideo_Click(object sender, EventArgs e)
        {
            this.txtPathVideo.Text = Helpers.Common.SelectFolderNew("Chọn Folder => Ấn Open => OK");
        }

        private void ckbPostImg_CheckedChanged(object sender, EventArgs e)
        {
            //ckbPostVideo.Checked = !ckbPostImg.Checked;
        }

        private void ckbPostVideo_CheckedChanged(object sender, EventArgs e)
        {
           // ckbPostImg.Checked = !ckbPostVideo.Checked;
        }
    }
}

using core.KichBan;
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

namespace Facebook_Tool_Request.core.fKichBan
{
    public partial class fLuotNewFeed : Form
    {
        public fLuotNewFeed(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fLuotNewFeed.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "LuotNewFeed").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('LuotNewFeed', 'Tương tác Newsfeed');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "LuotNewFeed");
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

        private JSON_Settings setting = null;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

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
                bool @checked = this.ckbComment.Checked;
                if (@checked)
                {
                    List<string> list = this.txtComment.Lines.ToList<string>();
                    list = Helpers.Common.RemoveEmptyItems(list);
                    bool flag2 = list.Count == 0;
                    if (flag2)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập nội dung bình luận!"), 3);
                        return;
                    }
                }
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("nudTimeFrom", this.nudTimeFrom.Value);
                json_Settings.Update("nudTimeTo", this.nudTimeTo.Value);
                json_Settings.Update("nudDelayFrom", this.nudDelayFrom.Value);
                json_Settings.Update("nudDelayTo", this.nudDelayTo.Value);
                json_Settings.Update("ckbInteract", this.ckbInteract.Checked);
                json_Settings.Update("ckbComment", this.ckbComment.Checked);
                json_Settings.Update("txtComment", this.txtComment.Text.Trim());
                json_Settings.Update("nudCountLikeFrom", this.nudCountLikeFrom.Value);
                json_Settings.Update("nudCountLikeTo", this.nudCountLikeTo.Value);
                json_Settings.Update("nudCountCommentFrom", this.nudCountCommentFrom.Value);
                json_Settings.Update("nudCountCommentTo", this.nudCountCommentTo.Value);
                string text2 = "";
                bool checked2 = this.ckbLike.Checked;
                if (checked2)
                {
                    text2 += "0";
                }
                bool checked3 = this.ckbLove.Checked;
                if (checked3)
                {
                    text2 += "1";
                }
                bool checked4 = this.ckbCare.Checked;
                if (checked4)
                {
                    text2 += "2";
                }
                bool checked5 = this.ckbHaha.Checked;
                if (checked5)
                {
                    text2 += "3";
                }
                bool checked6 = this.ckbWow.Checked;
                if (checked6)
                {
                    text2 += "4";
                }
                bool checked7 = this.ckbSad.Checked;
                if (checked7)
                {
                    text2 += "5";
                }
                bool checked8 = this.ckbAngry.Checked;
                if (checked8)
                {
                    text2 += "6";
                }
                json_Settings.Update("typeCamXuc", text2);
                int num = 0;
                bool checked9 = this.rbNganCachKyTu.Checked;
                if (checked9)
                {
                    num = 1;
                }
                json_Settings.Update("typeNganCach", num);
                string fullString = json_Settings.GetFullString();
                bool flag3 = this.type == 0;
                if (flag3)
                {
                    bool flag4 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn thêm hành động mới?")) == DialogResult.Yes;
                    if (flag4)
                    {
                        bool flag5 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                        if (flag5)
                        {
                            fLuotNewFeed.isSave = true;
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
                    bool flag6 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn cập nhật hành động?")) == DialogResult.Yes;
                    if (flag6)
                    {
                        bool flag7 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                        if (flag7)
                        {
                            fLuotNewFeed.isSave = true;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fLuotNewFeed_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudTimeFrom.Value = this.setting.GetValueInt("nudTimeFrom", 10);
                this.nudTimeTo.Value = this.setting.GetValueInt("nudTimeTo", 30);
                this.nudDelayFrom.Value = this.setting.GetValueInt("nudDelayFrom", 1);
                this.nudDelayTo.Value = this.setting.GetValueInt("nudDelayTo", 3);
                this.nudCountLikeFrom.Value = this.setting.GetValueInt("nudCountLikeFrom", 1);
                this.nudCountLikeTo.Value = this.setting.GetValueInt("nudCountLikeTo", 3);
                this.nudCountCommentFrom.Value = this.setting.GetValueInt("nudCountCommentFrom", 1);
                this.nudCountCommentTo.Value = this.setting.GetValueInt("nudCountCommentTo", 3);
                this.ckbInteract.Checked = this.setting.GetValueBool("ckbInteract", false);
                this.ckbComment.Checked = this.setting.GetValueBool("ckbComment", false);
                this.txtComment.Text = this.setting.GetValue("txtComment", "");
                bool flag = this.setting.GetValue("typeCamXuc", "").Contains("0");
                if (flag)
                {
                    this.ckbLike.Checked = true;
                }
                bool flag2 = this.setting.GetValue("typeCamXuc", "").Contains("1");
                if (flag2)
                {
                    this.ckbLove.Checked = true;
                }
                bool flag3 = this.setting.GetValue("typeCamXuc", "").Contains("2");
                if (flag3)
                {
                    this.ckbCare.Checked = true;
                }
                bool flag4 = this.setting.GetValue("typeCamXuc", "").Contains("3");
                if (flag4)
                {
                    this.ckbHaha.Checked = true;
                }
                bool flag5 = this.setting.GetValue("typeCamXuc", "").Contains("4");
                if (flag5)
                {
                    this.ckbWow.Checked = true;
                }
                bool flag6 = this.setting.GetValue("typeCamXuc", "").Contains("5");
                if (flag6)
                {
                    this.ckbSad.Checked = true;
                }
                bool flag7 = this.setting.GetValue("typeCamXuc", "").Contains("6");
                if (flag7)
                {
                    this.ckbAngry.Checked = true;
                }
                bool flag8 = this.setting.GetValueInt("typeNganCach", 0) == 1;
                if (flag8)
                {
                    this.rbNganCachKyTu.Checked = true;
                }
                else
                {
                    this.rbNganCachMoiDong.Checked = true;
                }
            }
            catch
            {
            }
            this.CheckedChangeFull();
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
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
                    list = this.txtComment.Lines.ToList<string>();
                }
                else
                {
                    list = this.txtComment.Text.Split(new string[]
                    {
                        "\n|\n"
                    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblStatus.Text = string.Format(Language.GetValue("Nội dung bình luận ({0}):"), list.Count.ToString());
            }
            catch
            {
            }
        }
        private void CheckedChangeFull()
        {
            this.ckbInteract_CheckedChanged(null, null);
            this.ckbComment_CheckedChanged(null, null);
        }

        private void ckbInteract_CheckedChanged(object sender, EventArgs e)
        {
            this.plCountLike.Enabled = this.ckbInteract.Checked;
            this.panel2.Enabled = this.ckbInteract.Checked;
        }

        private void ckbComment_CheckedChanged(object sender, EventArgs e)
        {
            this.plCountComment.Enabled = this.ckbComment.Checked;
            this.plComment.Enabled = this.ckbComment.Checked;
        }

        private void rbNganCachMoiDong_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }

        private void rbNganCachKyTu_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateSoLuongBinhLuan();
        }
       
    }
}

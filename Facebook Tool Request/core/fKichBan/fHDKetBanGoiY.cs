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
    public partial class fHDKetBanGoiY : Form
    {
        private JSON_Settings setting;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDKetBanGoiY(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fHDKetBanGoiY.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            string text = "HDKetBanGoiY";
            string text2 = "Kết Bạn Gợi Ý";
            bool flag = InteractSQL.GetTuongTac("", text).Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery(string.Concat(new string[]
                 {
                    "INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('",
                    text,
                    "', '",
                    text2,
                    "');"
                 }));
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", text);
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

        private void fHDKetBanGoiY_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudSoLuongFrom.Value = this.setting.GetValueInt("nudSoLuongFrom", 0);
                this.nudSoLuongTo.Value = this.setting.GetValueInt("nudSoLuongTo", 0);
                this.nudDelayFrom.Value = this.setting.GetValueInt("nudDelayFrom", 0);
                this.nudDelayTo.Value = this.setting.GetValueInt("nudDelayTo", 0);
                this.ckbChiKetBanTenCoDau.Checked = this.setting.GetValueBool("ckbChiKetBanTenCoDau", false);
                this.ckbOnlyAddFriendWithMutualFriends.Checked = this.setting.GetValueBool("ckbOnlyAddFriendWithMutualFriends", false);
                this.nudTimesWarning.Value = this.setting.GetValueInt("nudTimesWarning", 3);
            }
            catch
            {
            }
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
                MessageBoxHelper.ShowMessageBox(("Vui lòng nhập tên hành động!"), 3);
            }
            else
            {
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("nudSoLuongFrom", this.nudSoLuongFrom.Value);
                json_Settings.Update("nudSoLuongTo", this.nudSoLuongTo.Value);
                json_Settings.Update("nudDelayFrom", this.nudDelayFrom.Value);
                json_Settings.Update("nudDelayTo", this.nudDelayTo.Value);
                json_Settings.Update("ckbChiKetBanTenCoDau", this.ckbChiKetBanTenCoDau.Checked);
                json_Settings.Update("ckbOnlyAddFriendWithMutualFriends", this.ckbOnlyAddFriendWithMutualFriends.Checked);
                json_Settings.Update("nudTimesWarning", this.nudTimesWarning.Value);
                string fullString = json_Settings.GetFullString();
                bool flag2 = this.type == 0;
                if (flag2)
                {
                    bool flag3 = MessageBoxHelper.ShowMessageBoxWithQuestion(("Bạn có muốn thêm hành động mới?")) == DialogResult.Yes;
                    if (flag3)
                    {
                        bool flag4 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                        if (flag4)
                        {
                            fHDKetBanGoiY.isSave = true;
                            base.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox(("Thêm thất bại, vui lòng thử lại sau!"), 2);
                        }
                    }
                }
                else
                {
                    bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion(("Bạn có muốn cập nhật hành động?")) == DialogResult.Yes;
                    if (flag5)
                    {
                        bool flag6 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                        if (flag6)
                        {
                            fHDKetBanGoiY.isSave = true;
                            base.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox(("Cập nhật thất bại, vui lòng thử lại sau!"), 2);
                        }
                    }
                }
            }
        }
    }
}

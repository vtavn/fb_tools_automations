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
    public partial class fHDTimKiemGoogle : Form
    {
        private JSON_Settings setting = null;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDTimKiemGoogle(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fHDTimKiemGoogle.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "HDTimKiemGoogle").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('HDTimKiemGoogle', 'Tìm kiếm Google');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "HDTimKiemGoogle");
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txtTuKhoa.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblStatus.Text = string.Format(Language.GetValue("Danh sách Từ khóa|Link Web ({0}):"), list.Count.ToString());
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
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập tên hành động!"), 3);
            }
            else
            {
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("nudCountTuKhoaFrom", this.nudCountTuKhoaFrom.Value);
                json_Settings.Update("nudCountTuKhoaTo", this.nudCountTuKhoaTo.Value);
                json_Settings.Update("nudCountPageFrom", this.nudCountPageFrom.Value);
                json_Settings.Update("nudCountPageTo", this.nudCountPageTo.Value);
                json_Settings.Update("nudCountLinkClickFrom", this.nudCountLinkClickFrom.Value);
                json_Settings.Update("nudCountLinkClickTo", this.nudCountLinkClickTo.Value);
                json_Settings.Update("nudCountTimeScrollFrom", this.nudCountTimeScrollFrom.Value);
                json_Settings.Update("nudCountTimeScrollTo", this.nudCountTimeScrollTo.Value);
                json_Settings.Update("txtTuKhoa", this.txtTuKhoa.Text.Trim());
                string fullString = json_Settings.GetFullString();
                bool flag2 = this.type == 0;
                if (flag2)
                {
                    bool flag3 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn thêm hành động mới?")) == DialogResult.Yes;
                    if (flag3)
                    {
                        bool flag4 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                        if (flag4)
                        {
                            fHDTimKiemGoogle.isSave = true;
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
                    bool flag5 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn cập nhật hành động?")) == DialogResult.Yes;
                    if (flag5)
                    {
                        bool flag6 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                        if (flag6)
                        {
                            fHDTimKiemGoogle.isSave = true;
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

        private void fHDTimKiemGoogle_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudCountTuKhoaFrom.Value = this.setting.GetValueInt("nudCountTuKhoaFrom", 1);
                this.nudCountTuKhoaTo.Value = this.setting.GetValueInt("nudCountTuKhoaTo", 1);
                this.nudCountPageFrom.Value = this.setting.GetValueInt("nudCountPageFrom", 3);
                this.nudCountPageTo.Value = this.setting.GetValueInt("nudCountPageTo", 3);
                this.nudCountLinkClickFrom.Value = this.setting.GetValueInt("nudCountLinkClickFrom", 3);
                this.nudCountLinkClickTo.Value = this.setting.GetValueInt("nudCountLinkClickTo", 5);
                this.nudCountTimeScrollFrom.Value = this.setting.GetValueInt("nudCountTimeScrollFrom", 30);
                this.nudCountTimeScrollTo.Value = this.setting.GetValueInt("nudCountTimeScrollTo", 30);
                this.txtTuKhoa.Text = this.setting.GetValue("txtTuKhoa", "");
            }
            catch
            {
            }
        }
    }
}

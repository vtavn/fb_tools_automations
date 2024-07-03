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
    public partial class fHDBuffViewFb : Form
    {
        private JSON_Settings setting = null;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDBuffViewFb(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fHDBuffViewFb.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            bool flag = InteractSQL.GetTuongTac("", "HDBuffViewFb").Rows.Count == 0;
            if (flag)
            {
                Facebook_Tool_Request.core.KichBanPage.Connector.Instance.ExecuteNonQuery("INSERT INTO \"main\".\"Tuong_Tac\" (\"TenTuongTac\", \"MoTa\") VALUES ('HDBuffViewFb', 'Buff View!');");
            }
            string jsonStringOrPathFile = "";
            bool flag2 = type == 0;
            if (flag2)
            {
                DataTable tuongTac = InteractSQL.GetTuongTac("", "HDBuffViewFb");
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

        private void fBuffViewFb_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudStart.Value = this.setting.GetValueInt("nudStart", 1);
                this.nudEnd.Value = this.setting.GetValueInt("nudEnd", 2);
                this.txtLinkFacebook.Text = this.setting.GetValue("txtLinkFacebook", "");
                this.ckbSharePost.Checked = this.setting.GetValueBool("ckbSharePost", false);
                this.ckbLikePost.Checked = this.setting.GetValueBool("ckbLikePost", false);
            }
            catch (Exception ex)
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
                MessageBoxHelper.ShowMessageBox("Vui lòng nhập tên hành động!", 3);
            }
            else
            {
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("txtLinkFacebook", this.txtLinkFacebook.Text.Trim());
                json_Settings.Update("nudStart", this.nudStart.Value);
                json_Settings.Update("nudEnd", this.nudEnd.Value);
                json_Settings.Update("ckbSharePost", this.ckbSharePost.Checked);
                json_Settings.Update("ckbLikePost", this.ckbLikePost.Checked);
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
    }
}

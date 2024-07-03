using core.KichBan;
using Facebook_Tool_Request.Helpers;
using Newtonsoft.Json;
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
    public partial class fChangeInfo_HD : Form
    {
        private JSON_Settings setting;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fChangeInfo_HD(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fChangeInfo_HD.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            string text = "ChangeInfo";
            string text2 = "Change Info";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txtKeyDVFB_TextChanged(object sender, EventArgs e)
        {
            string rsVnd = Helpers.CommonRequest.getAmountDongVanFb(this.txtKeyDVFB.Text);
            if (rsVnd.StartsWith("success|"))
            {
                string[] str = rsVnd.Split('|');
                this.lbAmountDVFB.Text = "Số dư: " + str[1] + " vnđ";
                this.loadTypeDVFB();
            }
            else
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Key không lỗi hoặc không đúng. Vui lòng kiểm tra lại."), 2);
            }
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
            List<string> listTypeDVFB = Helpers.CommonRequest.GetTypeDongVanFb(this.txtKeyDVFB.Text);
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

        private void ckbChangePass_CheckedChanged(object sender, EventArgs e)
        {
            this.plChangePass.Enabled = ckbChangePass.Checked;
        }

        private void btnRandomPass_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            txtPasswordNew.Text = ("Cua@" + Helpers.Common.CreateRandomStringNumber(6, rd));
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Tạo mật khẩu ngẫu nhiên thành công!"), 1);
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
                bool changePass = this.ckbChangePass.Checked;
                bool changeMail = this.ckbChangeMail.Checked;
                if (changePass)
                {
                    bool flag1 = this.txtPasswordNew.Text.Trim() == "";
                    if (flag1)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập mật khẩu mới muốn đổi!"), 3);
                        return;
                    }
                }
                if (changeMail){

                    bool flag1 = this.txtKeyDVFB.Text.Trim() == "";
                    if (flag1)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập api key dongvanfb!"), 3);
                        return;
                    }
                }
                JSON_Settings json_Settings = new JSON_Settings();
                json_Settings.Update("ckbChangePass", this.ckbChangePass.Checked);
                json_Settings.Update("txtPasswordNew", this.txtPasswordNew.Text.Trim());
                json_Settings.Update("ckbChangeMail", this.ckbChangeMail.Checked);
                json_Settings.Update("txtKeyDVFB", this.txtKeyDVFB.Text.Trim());
                json_Settings.Update("cbbTypeDv", this.cbbTypeDv.SelectedValue);
                json_Settings.Update("nudDelayFrom", this.nudDelayFrom.Value);
                json_Settings.Update("nudDelayTo", this.nudDelayTo.Value);
                json_Settings.Update("ckbGetCookie", this.ckbGetCookie.Checked);

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
                            fChangeInfo_HD.isSave = true;
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
                            fChangeInfo_HD.isSave = true;
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

        private void fChangeInfo_Load(object sender, EventArgs e)
        {
            this.CheckedChangeFull();
            try
            {
                this.ckbChangePass.Checked = this.setting.GetValueBool("ckbChangePass", false);
                this.txtPasswordNew.Text = this.setting.GetValue("txtPasswordNew", "");
                this.ckbChangeMail.Checked = this.setting.GetValueBool("ckbChangeMail", false);
                this.txtKeyDVFB.Text = this.setting.GetValue("txtKeyDVFB", "");
                this.txtKeyDVFB_TextChanged(null, null);
                this.cbbTypeDv.SelectedValue = this.setting.GetValue("cbbTypeDv");
                this.nudDelayFrom.Value = this.setting.GetValueInt("nudDelayFrom", 1);
                this.nudDelayTo.Value = this.setting.GetValueInt("nudDelayTo", 5);
                this.ckbGetCookie.Checked = this.setting.GetValueBool("ckbGetCookie", false);

            }
            catch { }
        }
        private void CheckedChangeFull()
        {
            this.ckbChangePass_CheckedChanged(null, null);
        }
    }
}

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
    public partial class fHDRegPage : Form
    {
        private JSON_Settings setting;

        private string id_KichBan;

        private string id_TuongTac;

        private string Id_HanhDong;

        private int type;

        public static bool isSave;

        public fHDRegPage(string id_KichBan, int type = 0, string id_HanhDong = "")
        {
            this.InitializeComponent();
            fHDRegPage.isSave = false;
            this.id_KichBan = id_KichBan;
            this.Id_HanhDong = id_HanhDong;
            this.type = type;
            string text = "HDRegPage";
            string text2 = "Reg Page";
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

        private void updateSoLuong(int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        List<string> list = new List<string>();
                        list = this.txtlistName.Lines.ToList<string>();
                        list = Helpers.Common.RemoveEmptyItems(list);
                        this.rbListName.Text = string.Format("Nhập (Mỗi tên 1 dòng) ({0}):", list.Count.ToString());
                        break;
                    case 2:
                        string[] nameArray = File.ReadAllLines(this.txtPathFileName.Text);
                        rbFileName.Text = "Nhập từ File (" + nameArray.Length + ")";
                        break;
                    case 3:
                        System.Collections.Generic.IEnumerable<string> avatarArray = Directory.EnumerateFiles(this.txtPathAvatar.Text, "*");
                        label4.Text = "Ảnh đại diện (" + avatarArray.Count() + ")";
                        break;
                    case 4:
                        System.Collections.Generic.IEnumerable<string> coverArray = Directory.EnumerateFiles(this.txtPathCover.Text, "*");
                        label5.Text = "Ảnh bìa (" + coverArray.Count() + ")";
                        break;
                    case 5:
                        System.Collections.Generic.IEnumerable<string> postImgArray = Directory.EnumerateFiles(this.txtPathImgPost.Text, "*");
                        lblAnhPost.Text = "Ảnh bài viết (" + postImgArray.Count() + ")";
                        break;
                    case 6:
                        List<string> list2 = new List<string>();
                        list2 = this.txtAdminInvite.Lines.ToList<string>();
                        list2 = Helpers.Common.RemoveEmptyItems(list2);
                        label25.Text = "Danh sách Uid Admin (" + list2.Count.ToString() + "):";
                        break;
                }
            }
            catch
            {
            }
        }

        private void txtlistName_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(1);
        }

        private void rbListName_CheckedChanged(object sender, EventArgs e)
        {
            this.txtlistName.Enabled = this.rbListName.Checked;
        }

        private void txtPathFileName_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(2);
        }

        private void txtPathAvatar_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(3);
        }

        private void txtPathCover_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(4);
        }

        private void btnChooseFileName_Click(object sender, EventArgs e)
        {
            this.txtPathFileName.Text = Helpers.Common.SelectFile("Chọn File txt", "txt Files (*.txt)|*.txt|");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.txtPathAvatar.Text = Helpers.Common.SelectFolderNew("Chọn một ảnh ngẫu nhiên sẽ tự động lấy Folder");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.txtPathCover.Text = Helpers.Common.SelectFolderNew("Chọn một ảnh ngẫu nhiên sẽ tự động lấy Folder");
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
                bool @checked = this.rbFileName.Checked;
                if (@checked)
                {
                    bool flag1 = this.txtPathFileName.Text.Trim() == "";
                    if (flag1)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn File Tên Tạo Page!"), 3);
                        return;
                    }
                }
                else
                {
                    List<string> list = this.txtlistName.Lines.ToList<string>();
                    list = Helpers.Common.RemoveEmptyItems(list);
                    bool flag2 = list.Count == 0;
                    if (flag2 && !this.rbAutoName.Checked)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập danh sách Tên Tạo Page!"), 3);
                        return;
                    }
                }
                bool flag3 = this.txtPathAvatar.Text.Trim() == "" && !Directory.Exists(this.txtPathAvatar.Text.Trim());
                if (flag3)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Đường dẫn folder ảnh đại diện không đúng?!"), 3);
                    return;
                }
                bool flag4 = this.txtPathCover.Text.Trim() == "" && !Directory.Exists(this.txtPathCover.Text.Trim());
                if (flag4)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Đường dẫn folder ảnh bìa không đúng?!"), 3);
                }
                else
                {
                    JSON_Settings json_Settings = new JSON_Settings();
                    json_Settings.Update("numberPageCreate", this.numberPageCreate.Value);
                    json_Settings.Update("nudKhoangCachFrom", this.nudKhoangCachFrom.Value);
                    json_Settings.Update("nudKhoangCachTo", this.nudKhoangCachTo.Value);
                    json_Settings.Update("ckbRandomString", this.ckbRandomString.Checked);
                    json_Settings.Update("txtlistName", this.txtlistName.Text.Trim());
                    json_Settings.Update("txtDescription", this.txtDescription.Text.Trim());
                    json_Settings.Update("txtWebsite", this.txtWebsite.Text.Trim());
                    json_Settings.Update("txtPhoneNumber", this.txtPhoneNumber.Text.Trim());
                    json_Settings.Update("txtPathFileName", this.txtPathFileName.Text.Trim());
                    json_Settings.Update("txtPathAvatar", this.txtPathAvatar.Text.Trim());
                    json_Settings.Update("txtPathCover", this.txtPathCover.Text.Trim());
                    json_Settings.Update("txtChuoibatdau", this.txtChuoibatdau.Text.Trim());
                    json_Settings.Update("rbAutoName", this.ckbRandomString.Checked);
                    json_Settings.Update("ckbDeleteNamePage", this.ckbDeleteNamePage.Checked);
                    bool check = this.rbFileName.Checked;
                    bool rbAutoName = this.rbAutoName.Checked;
                    if (check)
                    {
                        json_Settings.Update("typeListName", 1);
                    }
                    else if (rbAutoName)
                    {
                        json_Settings.Update("typeListName", 2);
                    }
                    else
                    {
                        json_Settings.Update("typeListName", 0);
                    }
                    json_Settings.Update("txtContentPost", this.txtContentPost.Text.Trim());
                    json_Settings.Update("txtPathImgPost", this.txtPathImgPost.Text.Trim());
                    json_Settings.Update("ckbFirstPost", this.ckbFirstPost.Checked);
                    json_Settings.Update("ckbPostImg", this.ckbPostImg.Checked);
                    json_Settings.Update("delayPostform", this.delayPostform.Value);
                    json_Settings.Update("delayPostto", this.delayPostto.Value);

                    json_Settings.Update("txtAddress", this.txtAddress.Text.Trim());
                    json_Settings.Update("txtEmail", this.txtEmail.Text.Trim());
                    json_Settings.Update("txtZipcode", this.txtZipcode.Text.Trim());

                    json_Settings.Update("ckbInviteAdmin", this.ckbInviteAdmin.Checked);
                    json_Settings.Update("txtAdminInvite", this.txtAdminInvite.Text.Trim());
                    json_Settings.Update("numberAdInvite", this.numberAdInvite.Value);

                    string fullString = json_Settings.GetFullString();
                    bool flag5 = this.type == 0;
                    if (flag5)
                    {
                        bool flag6 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn thêm hành động mới?")) == DialogResult.Yes;
                        if (flag6)
                        {
                            bool flag7 = InteractSQL.InsertHanhDong(this.id_KichBan, text, this.id_TuongTac, fullString);
                            if (flag7)
                            {
                                fHDRegPage.isSave = true;
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
                        bool flag8 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn cập nhật hành động?")) == DialogResult.Yes;
                        if (flag8)
                        {
                            bool flag9 = InteractSQL.UpdateHanhDong(this.Id_HanhDong, text, fullString);
                            if (flag9)
                            {
                                fHDRegPage.isSave = true;
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

        private void fHDRegPage_Load(object sender, EventArgs e)
        {
            try
            {
                this.nudKhoangCachFrom.Value = this.setting.GetValueInt("nudKhoangCachFrom", 5);
                this.nudKhoangCachTo.Value = this.setting.GetValueInt("nudKhoangCachTo", 5);
                this.numberPageCreate.Value = this.setting.GetValueInt("numberPageCreate", 4);
                this.txtlistName.Text = this.setting.GetValue("txtlistName", "");
                this.txtDescription.Text = this.setting.GetValue("txtDescription", "");
                this.txtWebsite.Text = this.setting.GetValue("txtWebsite", "");
                this.txtPhoneNumber.Text = this.setting.GetValue("txtPhoneNumber", "");
                this.txtPathFileName.Text = this.setting.GetValue("txtPathFileName", "");
                this.txtPathAvatar.Text = this.setting.GetValue("txtPathAvatar", "");
                this.txtPathCover.Text = this.setting.GetValue("txtPathCover", "");
                this.ckbRandomString.Checked = this.setting.GetValueBool("ckbRandomString", false);
                this.ckbDeleteNamePage.Checked = this.setting.GetValueBool("ckbDeleteNamePage", false);
                //post
                this.txtContentPost.Text = this.setting.GetValue("txtContentPost", "");
                this.txtPathImgPost.Text = this.setting.GetValue("txtPathImgPost", "");
                this.delayPostform.Value = this.setting.GetValueInt("delayPostform", 25);
                this.delayPostto.Value = this.setting.GetValueInt("delayPostto", 30);
                this.ckbFirstPost.Checked = this.setting.GetValueBool("ckbFirstPost", false);
                this.ckbPostImg.Checked = this.setting.GetValueBool("ckbPostImg", false);
                this.txtChuoibatdau.Text = this.setting.GetValue("txtChuoibatdau", "");
                this.rbAutoName.Checked = this.setting.GetValueBool("rbAutoName", false);
                int check = this.setting.GetValueInt("typeListName");

                this.txtAddress.Text = this.setting.GetValue("txtAddress", "");
                this.txtZipcode.Text = this.setting.GetValue("txtZipcode", "");
                this.txtEmail.Text = this.setting.GetValue("txtEmail", "");

                this.ckbInviteAdmin.Checked = this.setting.GetValueBool("ckbInviteAdmin", false);
                this.txtAdminInvite.Text = this.setting.GetValue("txtAdminInvite", "");
                this.numberAdInvite.Value = this.setting.GetValueInt("numberAdInvite", 1);

                if (check == 1)
                {
                    this.rbFileName.Checked = true;
                }
                else if (check == 2)
                {
                    this.rbAutoName.Checked = true;
                }
                else
                {
                    this.rbListName.Checked = true;
                }
            }
            catch { }
            this.CheckedChangeFull();
        }
        private void CheckedChangeFull()
        {
            this.txtlistName_TextChanged(null, null);
            this.rbListName_CheckedChanged(null, null);
            this.rbListName_CheckedChanged(null, null);
            this.txtPathAvatar_TextChanged(null, null);
            this.txtPathCover_TextChanged(null, null);
            this.txtPathImgPost_TextChanged(null, null);
            this.txtContentPost_TextChanged(null, null);
            this.txtAdminInvite_TextChanged(null, null);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHuongDanContent());
        }

        private void ckbFirstPost_CheckedChanged(object sender, EventArgs e)
        {
            this.plFistPost.Enabled = this.ckbFirstPost.Checked;
        }

        private void ckbPostImg_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPathImgPost.Enabled = this.ckbPostImg.Checked;
            this.btnChooseImgPost.Enabled = this.ckbPostImg.Checked;
        }

        private void txtContentPost_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPathImgPost_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(5);
        }

        private void btnChooseImgPost_Click(object sender, EventArgs e)
        {
            this.txtPathImgPost.Text = Helpers.Common.SelectFolderNew("Chọn một ảnh ngẫu nhiên sẽ tự động lấy Folder");
        }

        private void ckbRandomString_CheckedChanged(object sender, EventArgs e)
        {
            this.txtChuoibatdau.Enabled = this.ckbRandomString.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Chuỗi kí tự khi tích chọn sẽ tự động thêm 1 dãy chuỗi bạn đặt + 3 số ngẫu nhiên tránh trùng lặp tên page. ví dụ: Nguyễn An CSKH 003."), 3);
        }

        private void label21_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Hệ thống sẽ tự đặt tên theo chuẩn tên của người việt. ví dụ Nguyễn Thị Anh"), 3);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txtAdminInvite_TextChanged(object sender, EventArgs e)
        {
            this.updateSoLuong(6);
        }
    }
}

using Facebook_Tool_Request.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fAccountBin : Form
    {
        private Random rd = new Random();
        private bool isStop;
        private JSON_Settings setting_general;
        private JSON_Settings setting_ShowDtgv;
        private object lock_StartProxy = new object();
        private object lock_FinishProxy = new object();
        private int checkDelayChrome = 0;
        private object lock_checkDelayChrome = new object();
        private bool isLookStatus = false;
        private int indexCbbThuMucOld = -1;
        private bool isExcute_CbbThuMuc_SelectedIndexChanged = true;
        private int indexCbbTinhTrangOld = -1;
        private bool isExcute_CbbTinhTrang_SelectedIndexChanged = true;
        private JSON_Settings setting_MoTrinhDuyet;

        public fAccountBin()
        {
            InitializeComponent();
            LoadSetting();
            LoadConfigManHinh();
            LoadcbbSearch();
            menuStrip1.Cursor = Cursors.Hand;
        }
        private void LoadcbbSearch()
        {
            Dictionary<string, string> dataSource = new Dictionary<string, string>
        {
            { "cUid", "UID" },
            { "cToken", "Token" },
            { "cCookies", "Cookie" },
            { "cEmail", "Email" },
            { "cPassMail", "Pass email" },
            {
                "cName",
                "Tên"
            },
            {
                "cBirthday",
                "Ngày sinh"
            },
            {
                "cGender",
                "Giới tính"
            },
            {
                "cPassword",
                "Mật khẩu"
            },
            {
                "cGhiChu",
                "Ghi chú"
            },
            {
                "cInteractEnd",
                "Tương tác cuối"
            }
        };
            cbbSearch.DataSource = new BindingSource(dataSource, null);
            cbbSearch.ValueMember = "Key";
            cbbSearch.DisplayMember = "Value";
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (base.Width == Screen.PrimaryScreen.WorkingArea.Width && base.Height == Screen.PrimaryScreen.WorkingArea.Height)
            {
                base.Width = Base.width;
                base.Height = Base.heigh;
                base.Top = Base.top;
                base.Left = Base.left;
            }
            else
            {
                Base.top = base.Top;
                Base.left = base.Left;
                base.Top = 0;
                base.Left = 0;
                base.Width = Screen.PrimaryScreen.WorkingArea.Width;
                base.Height = Screen.PrimaryScreen.WorkingArea.Height;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }
        private void LoadSetting()
        {
            setting_general = new JSON_Settings("configGeneral");
        }
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCaiDatHienThi());
            LoadConfigManHinh();
        }

        private void LoadConfigManHinh()
        {
            setting_ShowDtgv = new JSON_Settings("configDatagridview");
            dtgvAcc.Columns["cToken"].Visible = setting_ShowDtgv.GetValueBool("cToken");
            dtgvAcc.Columns["cCookies"].Visible = setting_ShowDtgv.GetValueBool("ckbCookie");
            dtgvAcc.Columns["cEmail"].Visible = setting_ShowDtgv.GetValueBool("ckbEmail");
            dtgvAcc.Columns["cName"].Visible = setting_ShowDtgv.GetValueBool("ckbTen");
            dtgvAcc.Columns["cFriend"].Visible = setting_ShowDtgv.GetValueBool("ckbBanBe");
            dtgvAcc.Columns["cGroup"].Visible = setting_ShowDtgv.GetValueBool("ckbNhom");
            dtgvAcc.Columns["cBirthday"].Visible = setting_ShowDtgv.GetValueBool("ckbNgaySinh");
            dtgvAcc.Columns["cGender"].Visible = setting_ShowDtgv.GetValueBool("ckbGioiTinh");
            dtgvAcc.Columns["cPassword"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhau");
            dtgvAcc.Columns["cPassMail"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhauMail");
            dtgvAcc.Columns["cBackup"].Visible = setting_ShowDtgv.GetValueBool("ckbBackup");
            dtgvAcc.Columns["cFa2"].Visible = setting_ShowDtgv.GetValueBool("ckbMa2FA");
            dtgvAcc.Columns["cUseragent"].Visible = setting_ShowDtgv.GetValueBool("ckbUseragent");
            dtgvAcc.Columns["cProxy"].Visible = setting_ShowDtgv.GetValueBool("ckbProxy");
            dtgvAcc.Columns["cDateCreateAcc"].Visible = setting_ShowDtgv.GetValueBool("ckbNgayTao");
            dtgvAcc.Columns["cAvatar"].Visible = setting_ShowDtgv.GetValueBool("ckbAvatar");
            dtgvAcc.Columns["cProfile"].Visible = setting_ShowDtgv.GetValueBool("ckbProfile");
            dtgvAcc.Columns["cInfo"].Visible = setting_ShowDtgv.GetValueBool("ckbTinhTrang");
            dtgvAcc.Columns["cThuMuc"].Visible = setting_ShowDtgv.GetValueBool("ckbThuMuc");
            dtgvAcc.Columns["cGhiChu"].Visible = setting_ShowDtgv.GetValueBool("ckbGhiChu");
            dtgvAcc.Columns["cFollow"].Visible = setting_ShowDtgv.GetValueBool("ckbFollow");
            dtgvAcc.Columns["cInteractEnd"].Visible = setting_ShowDtgv.GetValueBool("ckbInteractEnd");
        }

        private void btnLoadAcc_Click(object sender, EventArgs e)
        {
            string text = "";
            if (cbbThuMuc.SelectedValue != null)
            {
                text = cbbThuMuc.SelectedValue.ToString();
            }
            LoadCbbThuMuc();
            if (text != "999999" && text != "-1")
            {
                indexCbbThuMucOld = -1;
                cbbThuMuc.SelectedValue = text;
                return;
            }
            isExcute_CbbThuMuc_SelectedIndexChanged = false;
            cbbThuMuc.SelectedValue = text;
            isExcute_CbbThuMuc_SelectedIndexChanged = true;
            LoadCbbTinhTrang(fChonNhieuFolder.lstChooseIdFilesFromBin);
        }
        private void LoadCbbThuMuc()
        {
            isExcute_CbbThuMuc_SelectedIndexChanged = false;
            DataTable allFilesFromDatabaseForBin = CommonSQL.GetAllFilesFromDatabaseForBin(isShowAll: true);
            cbbThuMuc.DataSource = allFilesFromDatabaseForBin;
            cbbThuMuc.ValueMember = "id";
            cbbThuMuc.DisplayMember = "name";
            isExcute_CbbThuMuc_SelectedIndexChanged = true;
        }
        private void LoadCbbTinhTrang(List<string> lstIdFile = null)
        {
            try
            {
                DataTable allInfoFromAccount = CommonSQL.GetAllInfoFromAccount(lstIdFile, isGetActive: false);
                cbbTinhTrang.DataSource = allInfoFromAccount;
                cbbTinhTrang.ValueMember = "id";
                cbbTinhTrang.DisplayMember = "name";
            }
            catch
            {
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbSearch.SelectedIndex == -1)
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng chọn kiểu tìm kiếm!"));
                    return;
                }
                string columnName = cbbSearch.SelectedValue.ToString();
                string text = txbSearch.Text.Trim();
                if (text == "")
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng nhập nội dung tìm kiếm!"));
                    return;
                }
                List<int> list = new List<int>();
                text = Helpers.Common.ConvertToUnSign(text.ToLower());
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    string text2 = dtgvAcc.Rows[i].Cells[columnName].Value.ToString();
                    text2 = Helpers.Common.ConvertToUnSign(text2.ToLower());
                    text = Helpers.Common.ConvertToUnSign(text.ToLower());
                    if (text2.Contains(text))
                    {
                        list.Add(i);
                    }
                }
                int index = 0;
                if (list.Count <= 0)
                {
                    return;
                }
                int index2 = dtgvAcc.CurrentRow.Index;
                if (index2 >= list[list.Count - 1])
                {
                    index = 0;
                }
                else
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (index2 < list[j])
                        {
                            index = j;
                            break;
                        }
                    }
                }
                int index3 = list[index];
                dtgvAcc.CurrentCell = dtgvAcc.Rows[index3].Cells[columnName];
                dtgvAcc.ClearSelection();
                dtgvAcc.Rows[index3].Selected = true;
            }
            catch
            {
            }
        }

        private void cbbThuMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isExcute_CbbThuMuc_SelectedIndexChanged || cbbThuMuc.SelectedValue == null || !StringHelper.CheckStringIsNumber(cbbThuMuc.SelectedValue.ToString()) || (cbbThuMuc.SelectedValue.ToString() != "999999" && indexCbbThuMucOld == cbbThuMuc.SelectedIndex))
            {
                return;
            }
            string text = cbbThuMuc.SelectedValue.ToString();
            string text2 = text;
            if (!(text2 == "-1"))
            {
                if (text2 == "999999")
                {
                    Helpers.Common.ShowForm(new fChonNhieuFolder(isFromBin: true));
                    if (!fChonNhieuFolder.isAdd || fChonNhieuFolder.lstChooseIdFilesFromBin == null || fChonNhieuFolder.lstChooseIdFilesFromBin.Count == 0)
                    {
                        isExcute_CbbThuMuc_SelectedIndexChanged = false;
                        cbbThuMuc.SelectedIndex = ((indexCbbThuMucOld != -1) ? indexCbbThuMucOld : 0);
                        isExcute_CbbThuMuc_SelectedIndexChanged = true;
                    }
                    else
                    {
                        LoadCbbTinhTrang(fChonNhieuFolder.lstChooseIdFilesFromBin);
                    }
                }
                else
                {
                    LoadCbbTinhTrang(GetIdFile());
                }
            }
            else
            {
                LoadCbbTinhTrang();
            }
            indexCbbThuMucOld = cbbThuMuc.SelectedIndex;
        }
        private List<string> GetIdFile()
        {
            List<string> result = null;
            try
            {
                string text = cbbThuMuc.SelectedValue.ToString();
                string text2 = text;
                if (!(text2 == "-1"))
                {
                    result = ((!(text2 == "999999")) ? new List<string> { cbbThuMuc.SelectedValue.ToString() } : CollectionHelper.CloneList(fChonNhieuFolder.lstChooseIdFilesFromBin));
                }
            }
            catch
            {
            }
            return result;
        }

        private void cbbTinhTrang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isExcute_CbbTinhTrang_SelectedIndexChanged || cbbTinhTrang.SelectedValue == null || !StringHelper.CheckStringIsNumber(cbbTinhTrang.SelectedValue.ToString()) || (cbbTinhTrang.SelectedValue.ToString() != "-1" && indexCbbTinhTrangOld == cbbTinhTrang.SelectedIndex))
            {
                return;
            }
            string text = cbbThuMuc.SelectedValue.ToString();
            string text2 = text;
            if (!(text2 == "-1"))
            {
                if (text2 == "999999")
                {
                    LoadAccountFromFile(fChonNhieuFolder.lstChooseIdFilesFromBin, cbbTinhTrang.Text);
                }
                else
                {
                    LoadAccountFromFile(GetIdFile(), cbbTinhTrang.Text);
                }
            }
            else
            {
                LoadAccountFromFile(null, cbbTinhTrang.Text);
            }
            indexCbbTinhTrangOld = cbbTinhTrang.SelectedIndex;
        }
        private void LoadAccountFromFile(List<string> lstIdFile = null, string info = "", bool isGetActive = false)
        {
            try
            {
                dtgvAcc.Rows.Clear();
                if (info == "[Tất cả tình trạng]" || info == ("[Tất cả tình trạng]"))
                {
                    info = "";
                }
                DataTable accFromFile = CommonSQL.GetAccFromFile(lstIdFile, info, isGetActive);
                LoadDtgvAccFromDatatable(accFromFile);
            }
            catch (Exception)
            {
            }
        }
        private void LoadDtgvAccFromDatatable(DataTable tableAccount)
        {
            for (int i = 0; i < tableAccount.Rows.Count; i++)
            {
                DataRow dataRow = tableAccount.Rows[i];
                dtgvAcc.Rows.Add(false, dtgvAcc.RowCount + 1, dataRow["id"], dataRow["uid"], dataRow["token"], dataRow["cookie1"], dataRow["email"], dataRow["phone"], dataRow["name"], dataRow["follow"], dataRow["friends"], dataRow["groups"], dataRow["birthday"], dataRow["gender"], dataRow["pass"], "", dataRow["passmail"], dataRow["backup"], dataRow["fa2"], dataRow["useragent"], dataRow["proxy"], dataRow["dateCreateAcc"], dataRow["avatar"], dataRow["profile"], dataRow["nameFile"], dataRow["interactEnd"], dataRow["info"], dataRow["ghiChu"], dataRow["dateDelete"], "");
            }
            CountCheckedAccount(0);
            SetRowColor();
            CountTotalAccount();
        }
        private void CountCheckedAccount(int count = -1)
        {
            if (count == -1)
            {
                count = 0;
                for (int i = 0; i < dtgvAcc.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        count++;
                    }
                }
            }
            lblCountSelect.Text = count.ToString();
        }
        private void SetRowColor(int indexRow, int typeColor)
        {
            switch (typeColor)
            {
                case 1:
                    dtgvAcc.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                    break;
                case 2:
                    dtgvAcc.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                    break;
            }
        }

        private void SetRowColor(int indexRow = -1)
        {
            LoadSetting();
            if (setting_general.GetValueInt("typePhanBietMau") == 0)
            {
                if (indexRow == -1)
                {
                    for (int i = 0; i < dtgvAcc.RowCount; i++)
                    {
                        string infoAccount = GetInfoAccount(i);
                        if (infoAccount == "Live")
                        {
                            dtgvAcc.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                        }
                        else if (infoAccount.Contains("Die") || infoAccount.Contains(("Checkpoint")) || infoAccount.Contains("Changed pass"))
                        {
                            dtgvAcc.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                        }
                    }
                }
                else
                {
                    string infoAccount2 = GetInfoAccount(indexRow);
                    if (infoAccount2 == "Live")
                    {
                        dtgvAcc.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                    }
                    else if (infoAccount2.Contains("Die") || infoAccount2.Contains(("Checkpoint")) || infoAccount2.Contains("Changed pass"))
                    {
                        dtgvAcc.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                    }
                }
            }
            else if (indexRow == -1)
            {
                for (int j = 0; j < dtgvAcc.RowCount; j++)
                {
                    string infoAccount3 = GetInfoAccount(j);
                    if (infoAccount3 == "Live")
                    {
                        dtgvAcc.Rows[j].DefaultCellStyle.ForeColor = Color.Green;
                    }
                    else if (infoAccount3.Contains("Die") || infoAccount3.Contains(("Checkpoint")) || infoAccount3.Contains("Changed pass"))
                    {
                        dtgvAcc.Rows[j].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                string infoAccount4 = GetInfoAccount(indexRow);
                if (infoAccount4 == "Live")
                {
                    dtgvAcc.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Green;
                }
                else if (infoAccount4.Contains("Die") || infoAccount4.Contains(("Checkpoint")) || infoAccount4.Contains("Changed pass"))
                {
                    dtgvAcc.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }
        private void CountTotalAccount()
        {
            try
            {
                lblCountTotal.Text = dtgvAcc.Rows.Count.ToString();
            }
            catch
            {
            }
        }
        public string GetInfoAccount(int indexRow)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, "cInfo");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            KhoiPhucTaiKhoan();
        }
        public int CountChooseRowInDatagridview()
        {
            int num = 0;
            try
            {
                for (int i = 0; i < dtgvAcc.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        num++;
                    }
                }
            }
            catch
            {
            }
            return num;
        }
        private void UpdateSTTOnDtgvAcc()
        {
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, i, "cSTT", i + 1);
            }
        }
        private void KhoiPhucTaiKhoan()
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần khôi phục!"), 3);
                return;
            }
            Helpers.Common.ShowForm(new fCauHinhKhoiPhucTaiKhoan());
            if (fCauHinhKhoiPhucTaiKhoan.isOK)
            {
                try
                {
                    if (fCauHinhKhoiPhucTaiKhoan.typeThuMuc == 0)
                    {
                        List<string> list = new List<string>();
                        List<string> list2 = new List<string>();
                        for (int i = 0; i < dtgvAcc.RowCount; i++)
                        {
                            if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                            {
                                string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, i, "cId");
                                string idFileFromIdAccount = CommonSQL.GetIdFileFromIdAccount(statusDataGridView);
                                list.Add(statusDataGridView);
                                list2.Add(idFileFromIdAccount);
                                dtgvAcc.Rows.RemoveAt(i--);
                            }
                        }
                        UpdateSTTOnDtgvAcc();
                        CommonSQL.UpdateFieldToFile(list2, "active", "1");
                        if (CommonSQL.UpdateFieldToAccount(list, "active", "1"))
                        {
                            MessageBoxHelper.ShowMessageBox(string.Format("Đã khôi phục thành công {0} tài khoản!", list.Count));
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox("Lỗi khôi phục tài khoản, vui lòng thử lại sau!", 2);
                        }
                    }
                    else
                    {
                        string idFile = fCauHinhKhoiPhucTaiKhoan.idFile;
                        List<string> list3 = new List<string>();
                        for (int j = 0; j < dtgvAcc.RowCount; j++)
                        {
                            if (Convert.ToBoolean(dtgvAcc.Rows[j].Cells["cChose"].Value))
                            {
                                string statusDataGridView2 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, j, "cId");
                                list3.Add(statusDataGridView2);
                                dtgvAcc.Rows.RemoveAt(j--);
                            }
                        }
                        UpdateSTTOnDtgvAcc();
                        if (CommonSQL.UpdateFieldToAccount(list3, "idFile", idFile) && CommonSQL.UpdateFieldToAccount(list3, "active", "1"))
                        {
                            MessageBoxHelper.ShowMessageBox(string.Format("Đã khôi phục thành công {0} tài khoản!", list3.Count));
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox("Lỗi khôi phục tài khoản, vui lòng thử lại sau!", 2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helpers.Common.ExportError(null, ex, "Khoi Phuc Tai Khoan");
                }
            }
            UpdateTotalCountRecord();
            UpdateSelectCountRecord();
        }
        private void UpdateTotalCountRecord()
        {
            try
            {
                lblCountTotal.Text = dtgvAcc.Rows.Count.ToString();
            }
            catch
            {
            }
        }
        private void UpdateSelectCountRecord()
        {
            int num = 0;
            for (int i = 0; i < dtgvAcc.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                {
                    num++;
                }
            }
            try
            {
                lblCountSelect.Text = num.ToString();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteAccount();
        }
        private void RefreshDtgvAcc()
        {
            LoadRowColor();
            UpdateSTTOnDtgvAcc();
            UpdateTotalCountRecord();
            UpdateSelectCountRecord();
        }
        private void LoadRowColor(int rowIndex = -1)
        {
            LoadSetting();
            if (setting_general.GetValueInt("typePhanBietMau") == 0)
            {
                if (rowIndex == -1)
                {
                    for (int i = 0; i < dtgvAcc.RowCount; i++)
                    {
                        string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, i, "cInfo");
                        if (statusDataGridView == "Live")
                        {
                            dtgvAcc.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                        }
                        else if (statusDataGridView.Contains("Die") || statusDataGridView.Contains(("Checkpoint")) || statusDataGridView.Contains("Changed pass"))
                        {
                            dtgvAcc.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                        }
                    }
                    return;
                }
                string statusDataGridView2 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, rowIndex, "cInfo");
                string text = statusDataGridView2;
                if (!(text == "Live"))
                {
                    if (text == "Die")
                    {
                        dtgvAcc.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                    }
                }
                else
                {
                    dtgvAcc.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                }
            }
            else if (rowIndex == -1)
            {
                for (int j = 0; j < dtgvAcc.RowCount; j++)
                {
                    string statusDataGridView3 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, j, "cInfo");
                    if (statusDataGridView3.Contains("Die") || statusDataGridView3.Contains(("Checkpoint")))
                    {
                        dtgvAcc.Rows[j].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                string statusDataGridView4 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, rowIndex, "cInfo");
                string text2 = statusDataGridView4;
                if (!(text2 == "Live") && text2 == "Die")
                {
                    dtgvAcc.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void DeleteAccount()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                {
                    list.Add(dtgvAcc.Rows[i].Cells["cId"].Value.ToString());
                }
            }
            if (list.Count == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần xóa!"), 3);
                return;
            }
            if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format(("Bạn có thực sự muốn xóa {0} tài khoản đã chọn?"), CountChooseRowInDatagridview()) + "\r\n\r\n" + ("Chu\u0301 y\u0301: Ta\u0300i khoa\u0309n đa\u0303 xo\u0301a thi\u0300 không thê\u0309 khôi phu\u0323c la\u0323i đươ\u0323c nư\u0303a!")) == DialogResult.Yes)
            {
                if (CommonSQL.DeleteAccountToDatabase(list, isReallyDelete: true))
                {
                    for (int j = 0; j < dtgvAcc.RowCount; j++)
                    {
                        if (Convert.ToBoolean(dtgvAcc.Rows[j].Cells["cChose"].Value))
                        {
                            dtgvAcc.Rows.RemoveAt(j--);
                        }
                    }
                    if (CommonSQL.DeleteFileToDatabaseIfEmptyAccount())
                    {
                        RefreshDtgvAcc();
                    }
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox(("Xóa thất bại, vui lòng thử lại sau!"), 2);
                }
                UpdateSTTOnDtgvAcc();
            }
            UpdateTotalCountRecord();
            UpdateSelectCountRecord();
        }

        private void dtgvAcc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    dtgvAcc.CurrentRow.Cells["cChose"].Value = !Convert.ToBoolean(dtgvAcc.CurrentRow.Cells["cChose"].Value);
                    CountCheckedAccount();
                }
                catch
                {
                }
            }
        }

        private void dtgvAcc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dtgvAcc.CurrentRow.Cells["cChose"].Value = !Convert.ToBoolean(dtgvAcc.CurrentRow.Cells["cChose"].Value);
                CountCheckedAccount();
            }
            catch
            {
            }
        }

        private void dtgvAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 32)
            {
                ChoseRowInDatagrid("ToggleCheck");
            }
        }

        private void dtgvAcc_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            List<string> list = new List<string> { "cStt", "cFriend", "cGroup", "cFollow" };
            if (list.Contains(e.Column.Name))
            {
                e.SortResult = int.Parse((e.CellValue1.ToString() == "") ? "-1" : e.CellValue1.ToString()).CompareTo(int.Parse((e.CellValue2.ToString() == "") ? "-1" : e.CellValue2.ToString()));
                e.Handled = true;
            }
            else
            {
                e.SortResult = ((e.CellValue1.ToString() == "") ? "" : e.CellValue1.ToString()).CompareTo((e.CellValue2.ToString() == "") ? "" : e.CellValue2.ToString());
                e.Handled = true;
            }
        }
        public void SetCellAccount(int indexRow, string column, object value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, column, value);
        }

        private void ChoseRowInDatagrid(string modeChose)
        {
            switch (modeChose)
            {
                case "All":
                    {
                        for (int k = 0; k < dtgvAcc.RowCount; k++)
                        {
                            SetCellAccount(k, "cChose", true);
                        }
                        CountCheckedAccount(dtgvAcc.RowCount);
                        break;
                    }
                case "UnAll":
                    {
                        for (int j = 0; j < dtgvAcc.RowCount; j++)
                        {
                            SetCellAccount(j, "cChose", false);
                        }
                        CountCheckedAccount(0);
                        break;
                    }
                case "SelectHighline":
                    {
                        DataGridViewSelectedRowCollection selectedRows = dtgvAcc.SelectedRows;
                        for (int l = 0; l < selectedRows.Count; l++)
                        {
                            SetCellAccount(selectedRows[l].Index, "cChose", true);
                        }
                        CountCheckedAccount();
                        break;
                    }
                case "ToggleCheck":
                    {
                        for (int i = 0; i < dtgvAcc.SelectedRows.Count; i++)
                        {
                            int index = dtgvAcc.SelectedRows[i].Index;
                            SetCellAccount(index, "cChose", !Convert.ToBoolean(GetCellAccount(index, "cChose")));
                        }
                        CountCheckedAccount();
                        break;
                    }
            }
        }
        public string GetCellAccount(int indexRow, string column)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, column);
        }

        private void fAccountBin_Load(object sender, EventArgs e)
        {
            LoadConfigManHinh();
            LoadCbbThuMuc();
            LoadCbbTinhTrang();
        }

        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("UnAll");
        }

        private void tấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("All");
        }

        private void bôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("SelectHighline");
        }

        private void ctmsAcc_Opening(object sender, CancelEventArgs e)
        {
            trạngTháiToolStripMenuItem.DropDownItems.Clear();
            List<string> list = new List<string>();
            string text2 = "";
            string text3 = "";
            for (int j = 0; j < dtgvAcc.RowCount; j++)
            {
                text2 = GetCellAccount(j, "cStatus");
                if (text2 != "")
                {
                    text3 = Regex.Match(text2, "\\(IP: (.*?)\\)").Value;
                    if (text3 != "")
                    {
                        text2 = text2.Replace(text3, "").Trim();
                    }
                    text3 = Regex.Match(text2, "\\[(.*?)\\]").Value;
                    if (text3 != "")
                    {
                        text2 = text2.Replace(text3, "").Trim();
                    }
                    if (text2 != "" && !list.Contains(text2))
                    {
                        list.Add(text2);
                    }
                }
            }
            for (int k = 0; k < list.Count; k++)
            {
                trạngTháiToolStripMenuItem.DropDownItems.Add(list[k]);
                trạngTháiToolStripMenuItem.DropDownItems[k].Click += SelectGridByStatus;
            }
            tinhTrangToolStripMenuItem.DropDownItems.Clear();
            list = new List<string>();
            string text4 = "";
            for (int l = 0; l < dtgvAcc.RowCount; l++)
            {
                text4 = GetCellAccount(l, "cInfo");
                if (!text4.Equals("") && !list.Contains(text4))
                {
                    list.Add(text4);
                }
            }
            for (int m = 0; m < list.Count; m++)
            {
                tinhTrangToolStripMenuItem.DropDownItems.Add(list[m]);
                tinhTrangToolStripMenuItem.DropDownItems[m].Click += SelectGridByInfo;
            }
        }
        private void SelectGridByStatus(object sender, EventArgs e)
        {
            ChooseAccountByValue("cStatus", (sender as ToolStripMenuItem).Text);
        }
        private void ChooseAccountByValue(string column, string value)
        {
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                dtgvAcc.Rows[i].Cells["cChose"].Value = GetCellAccount(i, column).Contains(value);
            }
            CountCheckedAccount();
        }

        private void SelectGridByInfo(object sender, EventArgs e)
        {
            ChooseAccountByValue("cInfo", (sender as ToolStripMenuItem).Text);
        }
    }
}

using core.KichBan;
using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.core.Enum;
using Facebook_Tool_Request.Helpers;
using Facebook_Tool_Request.Properties;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fBasetup : Form
    {
        public static string curDir = Directory.GetCurrentDirectory() + @"/";
        private Random rd = new Random();
        private bool isStop;
        private bool isExcute_CbbThuMuc_SelectedIndexChanged = true;
        private int indexCbbThuMucOld = -1;
        private bool isExcute_CbbTinhTrang_SelectedIndexChanged = true;
        private int indexCbbTinhTrangOld = -1;
        private bool isCountCheckAccountWhenChayTuongTac = false;
        private JSON_Settings setting_general;
        private JSON_Settings setting_basetup;
        private JSON_Settings setting_ShowDtgv;
        private JSON_Settings setting_InteractGeneral;
        private List<ShopLike> listShopLike = null;
        private List<string> listProxyShopLike = null;
        private JSON_Settings setting_MoTrinhDuyet;
        private JSON_Settings setting_GiaiCheckpoint;
        private JSON_Settings setting_OpenMail;
        private int checkDelayChrome = 0;
        private List<Thread> lstThread = null;

        private List<TinsoftProxy> listTinsoft = null;
        private List<string> listApiTinsoft = null;

        private List<MinProxy> listMinproxy = null;
        private List<string> listApiMinproxy = null;

        //private string ipx = "";
        private object lock_StartProxy = new object();
        private object lock_FinishProxy = new object();
        private object lock_checkDelayChrome = new object();
        private int namee = 1;
        public static fMain remote;
        private object lock_checkDelayRequest = new object();
        private int checkDelayRequest = 0;

        public fBasetup()
        {
            this.InitializeComponent();
        }
        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            new fMain().Show();
            this.Hide();
        }
        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fAddFolder());
            if (fAddFolder.isAdd)
            {
                LoadCbbThuMuc();
                cbbThuMuc.SelectedIndex = cbbThuMuc.Items.Count - 2;
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
                    Helpers.Common.ShowForm(new fChonNhieuFolder());
                    if (!fChonNhieuFolder.isAdd || fChonNhieuFolder.lstChooseIdFiles == null || fChonNhieuFolder.lstChooseIdFiles.Count == 0)
                    {
                        isExcute_CbbThuMuc_SelectedIndexChanged = false;
                        cbbThuMuc.SelectedIndex = ((indexCbbThuMucOld != -1) ? indexCbbThuMucOld : 0);
                        isExcute_CbbThuMuc_SelectedIndexChanged = true;
                    }
                    else
                    {
                        LoadCbbTinhTrang(fChonNhieuFolder.lstChooseIdFiles);
                    }
                }
                else
                {
                    LoadCbbTinhTrang(GetIdFile());
                }
            }
            indexCbbThuMucOld = cbbThuMuc.SelectedIndex;
            if (cbbThuMuc.SelectedValue != null)
            {
                string text3 = cbbThuMuc.SelectedValue.ToString();
                if (text3 == "-1" || text3 == "999999")
                {
                    //btnDeleteFolder.BackColor = Color.Gray;
                    //btnDeleteFolder.Enabled = false;
                    //btnEditFolder.BackColor = Color.Gray;
                    //btnEditFolder.Enabled = false;
                }
                else
                {
                    //btnDeleteFolder.BackColor = Color.White;
                    //btnDeleteFolder.Enabled = true;
                    //btnEditFolder.BackColor = Color.White;
                    //btnEditFolder.Enabled = true;
                }
            }
        }
        private void LoadCbbTinhTrang(List<string> lstIdFile = null)
        {
            try
            {
                DataTable allInfoFromAccount = CommonSQL.GetAllInfoFromAccount(lstIdFile);
                cbbTinhTrang.DataSource = allInfoFromAccount;
                cbbTinhTrang.ValueMember = "id";
                cbbTinhTrang.DisplayMember = "name";
            }
            catch
            {
            }
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
                    result = ((!(text2 == "999999")) ? new List<string> { cbbThuMuc.SelectedValue.ToString() } : CollectionHelper.CloneList(fChonNhieuFolder.lstChooseIdFiles));
                }
            }
            catch
            {
            }
            return result;
        }
        private void LoadCbbThuMuc()
        {
            isExcute_CbbThuMuc_SelectedIndexChanged = false;
            DataTable allFilesFromDatabase = CommonSQL.GetAllFilesFromDatabase(isShowAll: true);
            cbbThuMuc.DataSource = allFilesFromDatabase;
            cbbThuMuc.ValueMember = "id";
            cbbThuMuc.DisplayMember = "name";
            isExcute_CbbThuMuc_SelectedIndexChanged = true;
        }
        private void LoadcbbSearch()
        {
            Dictionary<string, string> dataSource = new Dictionary<string, string>
        {
            { "cUid", "UID" },
            {
                "cPassword",
                "Mật khẩu"
            },
            {
                "cInfo",
                "Tình trạng"
            },
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
            { "cToken", "Token" },
            { "cCookies", "Cookie" },
            { "cEmail", "Email" },
            { "cPassMail", "Pass email" },
            { "cFa2", "2FA" },
            {
                "cGhiChu",
                "Ghi chú"
            },
            { "cStatus282", "Trạng thái 282" },
            {
                "cInteractEnd",
                "Tương tác cuối"
            }
        };
            cbbSearch.DataSource = new BindingSource(dataSource, null);
            cbbSearch.ValueMember = "Key";
            cbbSearch.DisplayMember = "Value";
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
                    LoadAccountFromFile(fChonNhieuFolder.lstChooseIdFiles, cbbTinhTrang.Text);
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
        private void LoadAccountFromFile(List<string> lstIdFile = null, string info = "")
        {
            try
            {
                dtgvAcc.Rows.Clear();
                if (info == "[Tất cả tình trạng]" || info == "[Tất cả tình trạng]")
                {
                    info = "";
                }
                DataTable accFromFile = CommonSQL.GetAccFromFile(lstIdFile, info);
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
                dtgvAcc.Rows.Add(false, dtgvAcc.RowCount + 1, dataRow["id"], dataRow["uid"], dataRow["token"], dataRow["cookie1"], dataRow["email"], dataRow["phone"], dataRow["name"], dataRow["follow"], dataRow["friends"], dataRow["groups"], dataRow["pages"], dataRow["birthday"], dataRow["gender"], dataRow["pass"], "", dataRow["passmail"], dataRow["backup"], dataRow["fa2"], dataRow["useragent"], dataRow["proxy"], dataRow["dateCreateAcc"], dataRow["avatar"], dataRow["profile"], dataRow["nameFile"], dataRow["interactEnd"], dataRow["info"], dataRow["ghiChu"], dataRow["status282"], "");
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
        private void CountTotalSelectedRow()
        {
            try
            {
                this.lblCountPaint.Text = dtgvAcc.SelectedRows.Count.ToString();
            }
            catch { }
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
        private void BtnLoadAcc_Click(object sender, EventArgs e)
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
            LoadCbbTinhTrang(fChonNhieuFolder.lstChooseIdFiles);
        }
        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            int selectedIndex = cbbThuMuc.SelectedIndex;
            if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có thực sự muốn xóa thư mục [{0}] không?", cbbThuMuc.Text)) == DialogResult.Yes)
            {
                if (CommonSQL.DeleteFileToDatabase(cbbThuMuc.SelectedValue.ToString()))
                {
                    MessageBoxHelper.ShowMessageBox(string.Format("Xoá thành công thư mục [{0}] !", cbbThuMuc.Text));
                    LoadCbbThuMuc();
                    cbbThuMuc.SelectedIndex = selectedIndex - 1;
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox(string.Format("Không thể xóa thư mục [{0}] !", cbbThuMuc.Text), 2);
                }
            }
        }
        private void btnEditFolder_Click(object sender, EventArgs e)
        {
            List<string> idFile = GetIdFile();
            if (idFile != null && idFile.Count == 1)
            {
                fEditFolder fEditFile2 = new fEditFolder(idFile[0], cbbThuMuc.Text);
                fEditFile2.ShowInTaskbar = false;
                fEditFile2.ShowDialog();
                int selectedIndex = cbbThuMuc.SelectedIndex;
                if (fEditFile2.isSuccess)
                {
                    LoadCbbThuMuc();
                    indexCbbThuMucOld = -1;
                    cbbThuMuc.SelectedIndex = selectedIndex;
                }
            }
        }
        private void fBasetup_Load(object sender, EventArgs e)
        {
            Base.useragentDefault = CommonChrome.GetUserAgentDefault();
            SetupFolder.StartApplication();
            LoadSetting();
            LoadConfigManHinh();
            LoadCbbThuMuc();
            LoadCbbTinhTrang();
            LoadcbbSearch();
            Base.width = base.Width;
            Base.heigh = base.Height;
            Load_cbbLoginType();
            Load_cbbLoginWeb();
            Load_ConfigBa();
        }
        private void Load_ConfigBa()
        {
            this.deviceId.Text = setting_basetup.GetValue("deviceId", License.Hardware.getHDD());
            this.timeDelayTo.Value = setting_basetup.GetValueInt("timeDelayTo", 1);
            this.timeDelayFrom.Value = setting_basetup.GetValueInt("timeDelayFrom", 2);
            this.ckbSleep.Checked = setting_basetup.GetValueBool("ckbSleep", true);
            this.turn.Value = setting_basetup.GetValueInt("turn", 2);
            this.turnDelay.Value = setting_basetup.GetValueInt("turnDelay", 5);
            this.ckbCheckLive.Checked = setting_basetup.GetValueBool("ckbCheckLive", true);

            this.delayChromeTo.Value = setting_basetup.GetValueInt("delayChromeTo", 1);
            this.delayChromeFrom.Value = setting_basetup.GetValueInt("delayChromeFrom", 2);
            this.ckbUsingProfile.Checked = setting_basetup.GetValueBool("ckbUsingProfile", true);
            this.numberThread.Value = setting_basetup.GetValueInt("numberThread", 5);

            this.cbbLoginType.SelectedValue = (setting_basetup.GetValue("cbbLoginType", "") == "") ? "0" : setting_basetup.GetValue("cbbLoginType", "");
            this.cbbLoginWeb.SelectedValue = (setting_basetup.GetValue("cbbLoginWeb", "") == "") ? "0" : setting_basetup.GetValue("cbbLoginWeb", "");

            //task
            this.task_SpamCmtPage.Checked = setting_basetup.GetValueBool("task_SpamCmtPage", false);

        }
        private void LoadConfigManHinh()
        {
            setting_ShowDtgv = new JSON_Settings("configDatagridview");
            dtgvAcc.Columns["cToken"].Visible = setting_ShowDtgv.GetValueBool("cToken");
            dtgvAcc.Columns["cCookies"].Visible = setting_ShowDtgv.GetValueBool("ckbCookie");
            dtgvAcc.Columns["cEmail"].Visible = false;
            dtgvAcc.Columns["cName"].Visible = setting_ShowDtgv.GetValueBool("ckbTen");
            dtgvAcc.Columns["cFriend"].Visible = false;
            dtgvAcc.Columns["cGroup"].Visible = false;
            dtgvAcc.Columns["cPages"].Visible = setting_ShowDtgv.GetValueBool("ckbPage");
            dtgvAcc.Columns["cBirthday"].Visible = false;
            dtgvAcc.Columns["cGender"].Visible = setting_ShowDtgv.GetValueBool("ckbGioiTinh");
            dtgvAcc.Columns["cPassword"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhau");
            dtgvAcc.Columns["cPassMail"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhauMail");
            dtgvAcc.Columns["cMailRecovery"].Visible = false;
            dtgvAcc.Columns["cBackup"].Visible = false;
            dtgvAcc.Columns["cFa2"].Visible = setting_ShowDtgv.GetValueBool("ckbMa2FA");
            dtgvAcc.Columns["cUseragent"].Visible = false;
            dtgvAcc.Columns["cProxy"].Visible = false;
            dtgvAcc.Columns["cDateCreateAcc"].Visible = false;
            dtgvAcc.Columns["cAvatar"].Visible = false;
            dtgvAcc.Columns["cProfile"].Visible = false;
            dtgvAcc.Columns["cInfo"].Visible = setting_ShowDtgv.GetValueBool("ckbTinhTrang");
            dtgvAcc.Columns["cThuMuc"].Visible = setting_ShowDtgv.GetValueBool("ckbThuMuc");
            dtgvAcc.Columns["cGhiChu"].Visible = setting_ShowDtgv.GetValueBool("ckbGhiChu");
            dtgvAcc.Columns["cFollow"].Visible = false;
            dtgvAcc.Columns["cInteractEnd"].Visible = setting_ShowDtgv.GetValueBool("ckbInteractEnd");
            dtgvAcc.Columns["cStatus282"].Visible = false;
        }
        public void SetCellAccount(int indexRow, string column, object value, bool isAllowEmptyValue = true)
        {
            if (isAllowEmptyValue || !(value.ToString().Trim() == ""))
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, column, value);
            }
        }
        private void LoadSetting()
        {
            setting_general = new JSON_Settings("configGeneral");
            setting_basetup = new JSON_Settings("configBaSetup");
            setting_InteractGeneral = new JSON_Settings("configInteractGeneral");
            Settings.Default.PathChrome = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            Settings.Default.Save();
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
        private void dtgvAcc_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isCountCheckAccountWhenChayTuongTac && e.ColumnIndex == 0)
            {
                CountCheckedAccount();
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
            List<string> list = new List<string> { "cStt", "cFriend", "cGroup", "cPages", "cFollow" };
            if (list.Contains(e.Column.Name))
            {
                e.SortResult = int.Parse((e.CellValue1.ToString() == "") ? "-1" : e.CellValue1.ToString()).CompareTo(int.Parse((e.CellValue2.ToString() == "") ? "-1" : e.CellValue2.ToString()));
                e.Handled = true;
            }
            else if (e.Column.Name == "cBirthday")
            {
                string text = e.CellValue1.ToString();
                string text2 = e.CellValue2.ToString();
                int num = int.Parse((text == "") ? "-1" : ((text.Split('/').Length >= 3) ? text.Split('/')[2] : "-1"));
                int value = int.Parse((text2 == "") ? "-1" : ((text2.Split('/').Length >= 3) ? text2.Split('/')[2] : "-1"));
                e.SortResult = num.CompareTo(value);
                e.Handled = true;
            }
            else if (e.Column.Name == "cDateCreateAcc")
            {
                try
                {
                    string text3 = e.CellValue1.ToString();
                    string text4 = e.CellValue2.ToString();
                    int num2 = int.Parse((text3 == "") ? "-1" : ((text3.Split(',').Length >= 2) ? text3.Split(',')[1] : "-1"));
                    int value2 = int.Parse((text4 == "") ? "-1" : ((text4.Split(',').Length >= 2) ? text4.Split(',')[1] : "-1"));
                    e.SortResult = num2.CompareTo(value2);
                    e.Handled = true;
                }
                catch (Exception)
                {
                    e.SortResult = -1.CompareTo(-1);
                    e.Handled = true;
                }
            }
            else
            {
                e.SortResult = ((e.CellValue1.ToString() == "") ? "" : e.CellValue1.ToString()).CompareTo((e.CellValue2.ToString() == "") ? "" : e.CellValue2.ToString());
                e.Handled = true;
            }
        }
        private void TransfomerAccount(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountSelect.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn chuyển!", 3);
                return;
            }
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có thực sự muốn chuyển {0} tài khoản sang thư mục [{1}]?", lblCountSelect.Text, toolStripMenuItem.Text)) == DialogResult.Yes)
            {
                TransfomAccountToOrtherFile(toolStripMenuItem.Tag.ToString());
            }
        }
        private void UpdateSttOnDatatable()
        {
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                SetCellAccount(i, "cSTT", i + 1);
            }
        }
        private void TransfomAccountToOrtherFile(string idFileTo)
        {
            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        list.Add(dtgvAcc.Rows[i].Cells["cId"].Value.ToString());
                    }
                }
                if (CommonSQL.UpdateFieldToAccount(list, "idfile", idFileTo))
                {
                    for (int j = 0; j < dtgvAcc.RowCount; j++)
                    {
                        if (Convert.ToBoolean(dtgvAcc.Rows[j].Cells["cChose"].Value))
                        {
                            dtgvAcc.Rows.RemoveAt(j--);
                        }
                    }
                    SetRowColor();
                    UpdateSttOnDatatable();
                    CountCheckedAccount(0);
                    CountTotalAccount();
                    MessageBoxHelper.ShowMessageBox(string.Format("Chuyển thành công {0} tài khoản!", list.Count));
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("Chuyển thất bại, vui lòng thử lại sau!", 2);
                }
            }
            catch
            {
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
        public void SetStatusAccount(int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", value);
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
                        else if (infoAccount.Contains("Die") || infoAccount.Contains("Checkpoint") || infoAccount.Contains("Changed pass") || infoAccount.Contains("282") || infoAccount.Contains("956") || infoAccount.Contains("Mail Changed") || infoAccount.Contains("VHH"))
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
                    else if (infoAccount2.Contains("Die") || infoAccount2.Contains("Checkpoint") || infoAccount2.Contains("Changed pass") || infoAccount2.Contains("282") || infoAccount2.Contains("956") || infoAccount2.Contains("Mail Changed") || infoAccount2.Contains("VHH"))
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
                    else if (infoAccount3.Contains("Die") || infoAccount3.Contains("Checkpoint") || infoAccount3.Contains("Changed pass") || infoAccount3.Contains("282") || infoAccount3.Contains("956") || infoAccount3.Contains("Mail Changed") || infoAccount3.Contains("VHH"))
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
                else if (infoAccount4.Contains("Die") || infoAccount4.Contains(("Checkpoint")) || infoAccount4.Contains("Changed pass") || infoAccount4.Contains("282") || infoAccount4.Contains("956") || infoAccount4.Contains("Mail Changed") || infoAccount4.Contains("VHH"))
                {
                    dtgvAcc.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }
        public string GetInfoAccount(int indexRow)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, "cInfo");
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbSearch.SelectedIndex == -1)
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng chọn kiểu tìm kiếm!"), 3);
                    return;
                }
                string columnName = cbbSearch.SelectedValue.ToString();
                string text = txbSearch.Text.Trim();
                if (text == "")
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng nhập nội dung tìm kiếm!"), 3);
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
                int num = -1;
                try
                {
                    num = dtgvAcc.CurrentRow.Index;
                }
                catch
                {
                    num = -1;
                }
                if (list.Count <= 0)
                {
                    return;
                }
                if (num >= list[list.Count - 1])
                {
                    index = 0;
                }
                else
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (num < list[j])
                        {
                            index = j;
                            break;
                        }
                    }
                }
                int index2 = list[index];
                dtgvAcc.CurrentCell = dtgvAcc.Rows[index2].Cells[columnName];
                dtgvAcc.ClearSelection();
                dtgvAcc.Rows[index2].Selected = true;
            }
            catch (Exception)
            {
            }
        }
        public static List<string> GetListTypeLogin()
        {
            return new List<string>
            {
                "uidpass|uid/pass",
                "cookie|cookie",
                "emailpass|email/pass",
            };
        }
        private void Load_cbbLoginType()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = GetListTypeLogin();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbLoginType.DataSource = new BindingSource(dictionary, null);
            this.cbbLoginType.ValueMember = "Key";
            this.cbbLoginType.DisplayMember = "Value";
        }
        public static List<string> GetListTypeWebLogin()
        {
            return new List<string>
            {
                "www|www",
                "mbasic|mbasic",
                "loginall|mbasic->www",
            };
        }
        private void Load_cbbLoginWeb()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryCountryCode = GetListTypeWebLogin();
            for (int i = 0; i < listCountryCountryCode.Count; i++)
            {
                string[] array = listCountryCountryCode[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            this.cbbLoginWeb.DataSource = new BindingSource(dictionary, null);
            this.cbbLoginWeb.ValueMember = "Key";
            this.cbbLoginWeb.DisplayMember = "Value";
        }
        private void dtgvAcc_Paint(object sender, PaintEventArgs e)
        {
            CountTotalSelectedRow();
        }
        private void tấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("All");
        }
        private void bôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("SelectHighline");
        }
        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("UnAll");
        }
        public int CountChooseRowInDatagridview()
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(lblCountSelect.Text);
            }
            catch
            {
            }
            return result;
        }
        private void checkLiveWallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiemTraTaiKhoan(0);
        }
        private void KiemTraTaiKhoan(int type)
        {
            LoadSetting();
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần thực hiện!", 3);
            }
            else
            {
                int iThread = 0;
                int maxThread = setting_general.GetValueInt("nudHideThread", 10);
                string tokenTrungGian = setting_general.GetValue("token");
                isStop = false;
                new Thread((ThreadStart)delegate
                {
                    switch (type)
                    {
                        case 0:
                            {
                                int num4 = 0;
                                while (num4 < dtgvAcc.Rows.Count && !isStop)
                                {
                                    if (Convert.ToBoolean(dtgvAcc.Rows[num4].Cells["cChose"].Value))
                                    {
                                        if (iThread < maxThread)
                                        {
                                            Interlocked.Increment(ref iThread);
                                            int row3 = num4++;
                                            new Thread((ThreadStart)delegate
                                            {
                                                SetStatusAccount(row3, "Đang kiểm tra...");
                                                CheckMyWall(row3, tokenTrungGian);
                                                Interlocked.Decrement(ref iThread);
                                                SetCellAccount(row3, "cChose", false);
                                            }).Start();
                                        }
                                        else
                                        {
                                            Application.DoEvents();
                                            Thread.Sleep(200);
                                        }
                                    }
                                    else
                                    {
                                        num4++;
                                    }
                                }
                                break;
                            }
                        case 2:
                            {
                                int num = 0;
                                while (num < dtgvAcc.Rows.Count && !isStop)
                                {
                                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                                    {
                                        if (iThread < maxThread)
                                        {
                                            Interlocked.Increment(ref iThread);
                                            int row6 = num++;
                                            new Thread((ThreadStart)delegate
                                            {
                                                SetStatusAccount(row6, "Đang kiểm tra...");
                                                CheckInfoUid(row6);
                                                Interlocked.Decrement(ref iThread);
                                                SetCellAccount(row6, "cChose", false);
                                            }).Start();
                                        }
                                        else
                                        {
                                            Application.DoEvents();
                                            Thread.Sleep(200);
                                        }
                                    }
                                    else
                                    {
                                        num++;
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                int num = 0;
                                while (num < dtgvAcc.Rows.Count && !isStop)
                                {
                                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                                    {
                                        if (iThread < maxThread)
                                        {
                                            Interlocked.Increment(ref iThread);
                                            int row6 = num++;
                                            new Thread((ThreadStart)delegate
                                            {
                                                SetStatusAccount(row6, "Đang kiểm tra...");
                                                CheckCountPage(row6);
                                                Interlocked.Decrement(ref iThread);
                                                SetCellAccount(row6, "cChose", false);
                                            }).Start();
                                        }
                                        else
                                        {
                                            Application.DoEvents();
                                            Thread.Sleep(200);
                                        }
                                    }
                                    else
                                    {
                                        num++;
                                    }
                                }
                                break;
                            }
                    }
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 60000)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                }).Start();
            }
        }
        private void CheckCountPage(int row)
        {
            try
            {
                string cellAccount = GetCellAccount(row, "cId");
                string cellAccount2 = GetCellAccount(row, "cUid");
                string cellAccount3 = GetCellAccount(row, "cToken");
                string cellAccount4 = GetCellAccount(row, "cCookies");

                if (!CheckIsUidFacebook(cellAccount2))
                {
                    SetStatusAccount(row, "Uid không hợp lệ!");
                    return;
                }
                string text = "";
                string text2 = CommonRequest.getInfoWithUidNoToken(cellAccount2);

                string tokenPage = CommonRequest.getCountPage(cellAccount3, cellAccount4, "", 1);
                if (tokenPage != "" || tokenPage != null)
                {
                    CommonSQL.UpdateMultiFieldToAccount(cellAccount, "pages", tokenPage);
                    SetCellAccount(row, "cPages", tokenPage);
                    text = ("Cập nhật thông tin thành công!");
                    SetStatusAccount(row, text);
                }
                else
                {
                    SetStatusAccount(row, "Không check được!");
                }

            }
            catch
            {
                SetStatusAccount(row, "Không check được!");
            }
        }
        private void CheckInfoUid(int row)
        {
            try
            {
                string cellAccount = GetCellAccount(row, "cId");
                string cellAccount2 = GetCellAccount(row, "cUid");
                if (!CheckIsUidFacebook(cellAccount2))
                {
                    SetStatusAccount(row, "Uid không hợp lệ!");
                    return;
                }
                string text = "";
                string text2 = CommonRequest.getInfoWithUidNoToken(cellAccount2);
                if (text2.StartsWith("0|"))
                {
                    if (CommonRequest.CheckLiveWall(cellAccount2).StartsWith("0|"))
                    {
                        SetStatusAccount(row, "Tài khoản Die!");
                        SetInfoAccount(cellAccount, row, "Die");
                    }
                    else
                    {
                        SetStatusAccount(row, "Không check được!");
                    }
                }
                else if (text2.StartsWith("1|"))
                {
                    string[] array = text2.Split('|');
                    string name = array[1];
                    string friends = array[2];
                    string datecreated = array[3];
                    CommonSQL.UpdateMultiFieldToAccount(cellAccount, "name|friends|dateCreateAcc", name + "|" + friends + "|" + datecreated);
                    SetCellAccount(row, "cName", name);
                    SetCellAccount(row, "cFriend", friends);
                    SetCellAccount(row, "cDateCreateAcc", datecreated);
                    SetInfoAccount(cellAccount, row, "Live");
                    text = "Cập nhật thông tin thành công!";
                    SetStatusAccount(row, text);
                }
                else
                {
                    SetStatusAccount(row, "Không check được!");
                }
            }
            catch
            {
                SetStatusAccount(row, "Không check được!");
            }
        }
        private bool CheckIsUidFacebook(string uid)
        {
            return Helpers.Common.IsNumber(uid) && !uid.StartsWith("0");
        }
        private void CheckMyWall(int row, string tokenTg)
        {
            try
            {
                string cId = GetCellAccount(row, "cId");
                string cUid = GetCellAccount(row, "cUid");
                string cStatus282 = GetCellAccount(row, "cStatus282");
                string cInfo = GetCellAccount(row, "cInfo");
                if (!CheckIsUidFacebook(cUid))
                {
                    SetStatusAccount(row, "Uid không hợp lệ!");
                    return;
                }
                string text = "";
                string text2 = "";
                string text3 = CommonRequest.CheckLiveWall(cUid);
                if (text3.StartsWith("0|"))
                {
                    text = "Die";
                    text2 = "Wall die";
                }
                else if (text3.StartsWith("1|"))
                {
                    text = "Live";
                    text2 = "Wall live";
                }
                else
                {
                    text2 = "Không check được!";
                }

                SetStatusAccount(row, text2);
                if (text != "")
                {
                    SetInfoAccount(cId, row, text);
                }

                if (!string.IsNullOrEmpty(cStatus282) && text == "Live")
                {
                    CommonSQL.UpdateFieldToAccount(cId, "status282", "");
                    SetStatusAccount(row, "Giải 282 Thành công!");
                    SetCellAccount(row, "cStatus282", "");
                }
                else if (!string.IsNullOrEmpty(cStatus282) && text == "Die" && !cInfo.Contains("282-Phone") && !cInfo.Contains("VHH"))
                {
                    SetInfoAccount(cId, row, "282");
                    SetStatusAccount(row, "282 Chưa về!");
                }
                else if (!string.IsNullOrEmpty(cStatus282) && text == "Die" && cInfo.Contains("282-Phone") && !cInfo.Contains("VHH"))
                {
                    SetInfoAccount(cId, row, "282-Phone");
                    SetStatusAccount(row, "CP 282 - Phone!");
                }
                else if (string.IsNullOrEmpty(cStatus282) && text == "Die" && cInfo.Contains("VHH"))
                {
                    CommonSQL.UpdateFieldToAccount(cId, "status282", "");
                    SetInfoAccount(cId, row, "VHH");
                    SetStatusAccount(row, "Acc Vô hiệu hoá! Vứt!");
                }

            }
            catch
            {
                SetStatusAccount(row, "Không check được!");
            }
        }
        public void SetInfoAccount(string id, int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cInfo", value);
            SetRowColor(indexRow);
            CommonSQL.UpdateFieldToAccount(id, "info", value);
        }
        private void CopyRowDatagrid(string modeCopy)
        {
            try
            {
                string text = "";
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(GetCellAccount(i, "cChose")))
                    {
                        list.Add(GetCellAccount(i, "cId"));
                        break;
                    }
                }
                if (list.Count == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn danh sách tài khoản muốn copy thông tin!", 3);
                    return;
                }
                switch (modeCopy)
                {
                    case "token":
                        {
                            for (int k = 0; k < dtgvAcc.RowCount; k++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[k].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(k, "cToken") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "useragent":
                        {
                            for (int num5 = 0; num5 < dtgvAcc.RowCount; num5++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num5].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num5, "cUseragent") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "proxy":
                        {
                            for (int num9 = 0; num9 < dtgvAcc.RowCount; num9++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num9].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num9, "cProxy").Split('*')[0] + "\r\n";
                                }
                            }
                            break;
                        }
                    case "cookie":
                        {
                            for (int num = 0; num < dtgvAcc.RowCount; num++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                                {
                                    string cellAccount = GetCellAccount(num, "cCookies");
                                    text = text + cellAccount + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid|pass":
                        {
                            for (int num11 = 0; num11 < dtgvAcc.RowCount; num11++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num11].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num11, "cUid") + "|" + GetCellAccount(num11, "cPassword") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid|pass|2fa":
                        {
                            for (int num7 = 0; num7 < dtgvAcc.RowCount; num7++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num7].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num7, "cUid") + "|" + GetCellAccount(num7, "cPassword") + "|" + GetCellAccount(num7, "cFa2") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid":
                        {
                            for (int num3 = 0; num3 < dtgvAcc.RowCount; num3++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num3].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num3, "cUid") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "pass":
                        {
                            for (int m = 0; m < dtgvAcc.RowCount; m++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[m].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(m, "cPassword") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "mail":
                        {
                            for (int num12 = 0; num12 < dtgvAcc.RowCount; num12++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num12].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num12, "cEmail") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "2fa":
                        {
                            for (int num10 = 0; num10 < dtgvAcc.RowCount; num10++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num10].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num10, "cFa2") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "ma2fa":
                        {
                            for (int num8 = 0; num8 < dtgvAcc.RowCount; num8++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num8].Cells["cChose"].Value))
                                {
                                    text = text + Helpers.Common.GetTotp(GetCellAccount(num8, "cFa2")) + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid|pass|token|cookie":
                        {
                            for (int num6 = 0; num6 < dtgvAcc.RowCount; num6++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num6].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num6, "cUid") + "|" + GetCellAccount(num6, "cPassword") + "|" + GetCellAccount(num6, "cToken") + "|" + GetCellAccount(num6, "cCookies") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "mail|passmail":
                        {
                            for (int num4 = 0; num4 < dtgvAcc.RowCount; num4++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num4].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num4, "cEmail") + "|" + GetCellAccount(num4, "cPassMail") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid|pass|token|cookie|mail|passmail":
                        {
                            for (int num2 = 0; num2 < dtgvAcc.RowCount; num2++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num2].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(num2, "cUid") + "|" + GetCellAccount(num2, "cPassword") + "|" + GetCellAccount(num2, "cToken") + "|" + GetCellAccount(num2, "cCookies") + "|" + GetCellAccount(num2, "cEmail") + "|" + GetCellAccount(num2, "cPassMail") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "uid|pass|token|cookie|mail|passmail|fa2":
                        {
                            for (int n = 0; n < dtgvAcc.RowCount; n++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[n].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(n, "cUid") + "|" + GetCellAccount(n, "cPassword") + "|" + GetCellAccount(n, "cToken") + "|" + GetCellAccount(n, "cCookies") + "|" + GetCellAccount(n, "cEmail") + "|" + GetCellAccount(n, "cPassMail") + "|" + GetCellAccount(n, "cFa2") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "name":
                        {
                            for (int l = 0; l < dtgvAcc.RowCount; l++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[l].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(l, "cName") + "\r\n";
                                }
                            }
                            break;
                        }
                    case "birthday":
                        {
                            for (int j = 0; j < dtgvAcc.RowCount; j++)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[j].Cells["cChose"].Value))
                                {
                                    text = text + GetCellAccount(j, "cBirthday") + "\r\n";
                                }
                            }
                            break;
                        }
                }
                Clipboard.SetText(text.TrimEnd('\r', '\n'));
            }
            catch
            {
            }
        }
        private void uIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("uid");
        }
        private void passToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("pass");
        }
        private void tokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("token");
        }
        private void cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("cookie");
        }
        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("mail");
        }
        private void fAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("2fa");
        }
        private void mãBảoMật2FAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("ma2fa");
        }
        private void uidPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("uid|pass");
        }
        private void uidPass2FAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("uid|pass|2fa");
        }
        private void mailPassMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRowDatagrid("mail|passmail");
        }
        private void ctmsAcc_Opening(object sender, CancelEventArgs e)
        {
            chuyểnThưMụcToolStripMenuItem.DropDownItems.Clear();
            DataTable allFilesFromDatabase = CommonSQL.GetAllFilesFromDatabase();
            int num2 = 0;
            for (int i = 0; i < allFilesFromDatabase.Rows.Count; i++)
            {
                DataRow dataRow = allFilesFromDatabase.Rows[i];
                if (dataRow["id"].ToString() != ((cbbThuMuc.SelectedValue == null) ? "" : cbbThuMuc.SelectedValue.ToString()))
                {
                    chuyểnThưMụcToolStripMenuItem.DropDownItems.Add(dataRow["name"].ToString());
                    chuyểnThưMụcToolStripMenuItem.DropDownItems[i - num2].Tag = dataRow["id"];
                    chuyểnThưMụcToolStripMenuItem.DropDownItems[i - num2].Click += TransfomerAccount;
                }
                else
                {
                    num2++;
                }
            }
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
        private void tảiLạiDanhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnLoadAcc_Click(null, null);
        }
        private void xoáTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAccount(isDeleteProfile: false);
        }
        private void DeleteAccount(bool isDeleteProfile)
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
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần xóa!"));
            }
            else
            {
                if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format(("Bạn có muốn xóa {0} tài khoản đã chọn?"), CountChooseRowInDatagridview()) + "\r\n\r\n" + ("(Ta\u0300i khoa\u0309n sau khi xo\u0301a se\u0303 đươ\u0323c lưu ta\u0323i mu\u0323c [Ta\u0300i khoa\u0309n đa\u0303 xo\u0301a])")) != DialogResult.Yes)
                {
                    return;
                }
                if (isDeleteProfile)
                {
                    int iThread = 0;
                    int num = 0;
                    while (num < dtgvAcc.Rows.Count)
                    {
                        if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                        {
                            if (iThread < 10)
                            {
                                Interlocked.Increment(ref iThread);
                                int row = num++;
                                new Thread((ThreadStart)delegate
                                {
                                    SetStatusAccount(row, "Đang xoá profile...");
                                    DeleteProfile(row);
                                    Interlocked.Decrement(ref iThread);
                                }).Start();
                            }
                            else
                            {
                                Application.DoEvents();
                                Thread.Sleep(200);
                            }
                        }
                        else
                        {
                            num++;
                        }
                    }
                    while (iThread > 0)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                }
                if (CommonSQL.DeleteAccountToDatabase(list))
                {
                    for (int j = 0; j < dtgvAcc.RowCount; j++)
                    {
                        if (Convert.ToBoolean(dtgvAcc.Rows[j].Cells["cChose"].Value))
                        {
                            dtgvAcc.Rows.RemoveAt(j--);
                        }
                    }
                    UpdateSttOnDatatable();
                    CountTotalAccount();
                    CountCheckedAccount(0);
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("Xóa thất bại, vui lòng thử lại sau!", 2);
                }
            }
        }
        private void DeleteProfile(int row)
        {
            try
            {
                string id = dtgvAcc.Rows[row].Cells["cId"].Value.ToString();
                string text = dtgvAcc.Rows[row].Cells["cUid"].Value.ToString();
                if (text.Trim() == "")
                {
                    SetStatusAccount(row, "Chưa tạo profile!");
                    return;
                }
                string path = setting_general.GetValue("txbPathProfile") + "\\" + text;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive: true);
                    SetStatusAccount(row, ("Xóa profile thành công!"));
                    SetCellAccount(row, "cProfile", "No");
                    CommonSQL.UpdateFieldToAccount(id, "profile", "No");
                }
                else
                {
                    SetStatusAccount(row, "Chưa tạo profile!");
                }
            }
            catch
            {
                SetStatusAccount(row, ("Xóa profile thất bại!"));
            }
        }
        private void lọcTrùngTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountSelect.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tất cả tài khoản rồi hãy ấn lọc.!", 3);
                return;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            int num = 0;
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                if (!Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                {
                    continue;
                }
                try
                {
                    string cellAccount = GetCellAccount(i, "cUid");
                    if (list2.Contains(cellAccount))
                    {
                        list.Add(dtgvAcc.Rows[i].Cells["cId"].Value.ToString());
                        dtgvAcc.Rows.RemoveAt(i);
                        i--;
                        num++;
                    }
                    else
                    {
                        list2.Add(cellAccount);
                    }
                }
                catch
                {
                }
            }
            CommonSQL.DeleteAccountToDatabase(list);
            UpdateSttOnDatatable();
            CountTotalAccount();
            CountCheckedAccount();
            MessageBoxHelper.ShowMessageBox(string.Format("Đã loại bỏ {0} tài khoản bị trùng!", num));
        }
        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        list.Add(GetCellAccount(i, "cUid") + "|" + GetCellAccount(i, "cPassword") + "|" + GetCellAccount(i, "cToken") + "|" + GetCellAccount(i, "cCookies") + "|" + GetCellAccount(i, "cEmail") + "|" + GetCellAccount(i, "cPassMail") + "|" + GetCellAccount(i, "cFa2") + "|" + GetCellAccount(i, "cUseragent") + "|" + GetCellAccount(i, "cProxy").Split('*')[0] + "|" + GetCellAccount(i, "cName") + "|" + GetCellAccount(i, "cGender") + "|" + GetCellAccount(i, "cFollow") + "|" + GetCellAccount(i, "cFriend") + "|" + GetCellAccount(i, "cGroup") + "|" + GetCellAccount(i, "cBirthday") + "|" + GetCellAccount(i, "cDateCreateAcc") + "|" + GetCellAccount(i, "cGhiChu"));
                    }
                }
                if (list.Count <= 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn danh sách tài khoản muốn copy thông tin!", 3);
                }
                else
                {
                    Helpers.Common.ShowForm(new fCoppy(list));
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Error Copy");
            }
        }
        private void cControl(string dt)
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    if (dt == "start")
                    {
                        panel2.Enabled = false;
                        panel1.Enabled = false;
                        btnStop.Enabled = true;
                        btnStart.Enabled = false;
                        gr1.Enabled = false;
                        gr2.Enabled = false;
                        gr3.Enabled = false;
                    }
                    else if (dt == "stop")
                    {
                        panel2.Enabled = true;
                        panel1.Enabled = true;
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;
                        gr1.Enabled = true;
                        gr2.Enabled = true;
                        gr3.Enabled = true;
                    }
                }
                catch (Exception)
                {
                }
            });
        }
        public List<string> GetListKeyTinsoft()
        {
            List<string> list = new List<string>();
            try
            {
                if (setting_general.GetValueInt("typeApiTinsoft") == 0)
                {
                    RequestXNet requestXNet = new RequestXNet("", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)", "", 0);
                    string json = requestXNet.RequestGet("http://proxy.tinsoftsv.com/api/getUserKeys.php?key=" + setting_general.GetValue("txtApiUser"));
                    JObject jObject = JObject.Parse(json);
                    foreach (JToken item in (IEnumerable<JToken>)jObject["data"])
                    {
                        if (Convert.ToBoolean(item["success"].ToString()))
                        {
                            list.Add(item["key"].ToString());
                        }
                    }
                }
                else
                {
                    List<string> valueList = setting_general.GetValueList("txtApiProxy");
                    foreach (string item2 in valueList)
                    {
                        if (TinsoftProxy.CheckApiProxy(item2))
                        {
                            list.Add(item2);
                        }
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                setting_basetup.Update("deviceId", this.deviceId.Text);
                setting_basetup.Update("timeDelayTo", this.timeDelayTo.Value);
                setting_basetup.Update("timeDelayFrom", this.timeDelayFrom.Value);
                setting_basetup.Update("ckbSleep", this.ckbSleep.Checked);
                setting_basetup.Update("turn", this.turn.Value);
                setting_basetup.Update("turnDelay", this.turnDelay.Value);
                setting_basetup.Update("ckbCheckLive", this.ckbCheckLive.Checked);

                setting_basetup.Update("delayChromeTo", this.delayChromeTo.Value);
                setting_basetup.Update("delayChromeFrom", this.delayChromeFrom.Value);
                setting_basetup.Update("ckbUsingProfile", this.ckbUsingProfile.Checked);
                setting_basetup.Update("numberThread", this.numberThread.Value);
                setting_basetup.Update("cbbLoginType", this.cbbLoginType.SelectedValue.ToString());
                setting_basetup.Update("cbbLoginWeb", this.cbbLoginWeb.SelectedValue.ToString());

                //task
                setting_basetup.Update("task_SpamCmtPage", this.task_SpamCmtPage.Checked);
                setting_basetup.Save("");

                //start
                handleStart(sender, e);
            }
            catch { }
        }

        private void handleStart(object sender, EventArgs e)
        {
            try
            {
                LoadSetting();
                setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
                setting_GiaiCheckpoint = new JSON_Settings("configGiaiCheckPoint");

                string profilePath = "";
                profilePath = ConfigHelper.GetPathProfile();
                if (Directory.Exists(profilePath))
                {
                    goto isProfile;
                }
                MessageBoxHelper.ShowMessageBox("Đường dẫn profile không hợp lệ!", 3);
                goto quit;
            isProfile:
                if (!(Base.useragentDefault == ""))
                {
                    goto openChorme;
                }
                Base.useragentDefault = CommonChrome.GetUserAgentDefault();
                if (!(Base.useragentDefault == ""))
                {
                    goto openChorme;
                }
                MessageBoxHelper.ShowMessageBox("Phiên bản Chromedriver hiện tại không khả dụng, vui lòng cập nhật!", 3);
                goto quit;
            openChorme:
                LoadSetting();
                int maxThread = setting_basetup.GetValueInt("numberThread", 5);
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        {
                            listProxyShopLike = setting_general.GetValueList("txtApiShopLike");
                            if (listProxyShopLike.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox("Proxy ShopLike không đủ, vui lòng nhập thêm!", 2);
                                return;
                            }
                            listShopLike = new List<ShopLike>();
                            for (int j = 0; j < listProxyShopLike.Count; j++)
                            {
                                ShopLike item2 = new ShopLike(listProxyShopLike[j], 0, setting_general.GetValueInt("nudLuongPerIPShopLike"), setting_general.GetValue("cbbLocationShopLikePrx"));
                                listShopLike.Add(item2);
                            }
                            if (listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike") < maxThread)
                            {
                                maxThread = listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike");
                            }
                            break;
                        }
                    case 2:
                        {
                            listApiTinsoft = GetListKeyTinsoft();
                            if (listApiTinsoft.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox("Proxy Tinsoft không đủ, vui lòng mua thêm!", 2);
                                return;
                            }
                            listTinsoft = new List<TinsoftProxy>();
                            for (int m = 0; m < listApiTinsoft.Count; m++)
                            {
                                TinsoftProxy item4 = new TinsoftProxy(listApiTinsoft[m], setting_general.GetValueInt("nudLuongPerIPTinsoft"), setting_general.GetValueInt("cbbLocationTinsoft"));
                                listTinsoft.Add(item4);
                            }
                            if (listApiTinsoft.Count * setting_general.GetValueInt("nudLuongPerIPTinsoft") < maxThread)
                            {
                                maxThread = listApiTinsoft.Count * setting_general.GetValueInt("nudLuongPerIPTinsoft");
                            }
                            break;
                        }
                    case 3:
                        {
                            listApiMinproxy = setting_general.GetValueList("txtApiMinproxy");

                            if (listApiMinproxy.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox("Key MinProxy không đủ, vui lòng mua thêm!", 2);
                                return;
                            }

                            listMinproxy = new List<MinProxy>();
                            for (int i = 0; i < listApiMinproxy.Count; i++)
                            {
                                MinProxy item = new MinProxy(listApiMinproxy[i], 0, setting_general.GetValueInt("nudLuongPerIPMinProxy"), "");
                                listMinproxy.Add(item);
                            }

                            if (listApiMinproxy.Count * setting_general.GetValueInt("nudLuongPerIPMinProxy") < maxThread)
                            {
                                maxThread = listApiMinproxy.Count * setting_general.GetValueInt("nudLuongPerIPMinProxy");
                            }
                            break;
                        }
                }

                List<int> lstPossition = new List<int>();
                for (int num = 0; num < maxThread; num++)
                {
                    lstPossition.Add(0);
                }
                cControl("start");
                isStop = false;
                checkDelayChrome = 0;
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    try
                    {
                        int num = 0;
                        while (num < dtgvAcc.Rows.Count && !isStop)
                        {
                            if (!Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                            {
                                num++;
                                goto isBreak;
                            }
                            if (isStop)
                            {
                                break;
                            }
                            if (lstThread.Count < maxThread)
                            {
                                if (isStop)
                                {
                                    break;
                                }
                                int row = num++;
                                Thread thread = new Thread((ThreadStart)delegate
                                {
                                    try
                                    {
                                        int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                        runOneThread(row, indexOfPossitionApp, profilePath);
                                        Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                        SetCellAccount(row, "cChose", false);
                                    }
                                    catch (Exception ex)
                                    {
                                        Helpers.Common.ExportError(null, ex);
                                    }
                                })
                                {
                                    Name = row.ToString()
                                };
                                lstThread.Add(thread);
                                Helpers.Common.DelayTime(1.0);
                                thread.Start();
                                goto isBreak;
                            }
                            if (isStop)
                            {
                                break;
                            }
                            if (setting_general.GetValueInt("ip_iTypeChangeIp") == 1 || setting_general.GetValueInt("ip_iTypeChangeIp") == 2 || setting_general.GetValueInt("ip_iTypeChangeIp") == 3)
                            {
                                for (int num1 = 0; num1 < lstThread.Count; num1++)
                                {
                                    if (!lstThread[num1].IsAlive)
                                    {
                                        lstThread.RemoveAt(num1--);
                                    }
                                }
                                goto isBreak;
                            }
                        isBreak:
                            if (isStop)
                            {
                                break;
                            }
                        }
                        for (int num2 = 0; num2 < lstThread.Count; num2++)
                        {
                            lstThread[num2].Join();
                        }
                    }
                    catch (Exception ex)
                    {
                        Helpers.Common.ExportError(null, ex);
                        cControl("stop");
                    }
                    cControl("stop");
                }).Start();
            quit:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
                cControl("stop");
            }
        }
        private async void runOneThread(int indexRow, int indexPos, string profilePath)
        {
            setting_GiaiCheckpoint = new JSON_Settings("configGiaiCheckPoint");
            string cProxy = "";
            Chrome chrome = null;
            int num = 0;
            bool flag = false;
            string cStatus = "";
            int typeProxy = 0;
            string cCheckProxy = "";
            ShopLike shopLike = null;
            TinsoftProxy tinsoftProxy = null;
            MinProxy minProxy = null;
            bool flag2 = false;
            string cCid = "";
            string cUid = GetCellAccount(indexRow, "cUid");
            string cId = GetCellAccount(indexRow, "cId");
            string cEmail = GetCellAccount(indexRow, "cEmail");
            string cPassMail = GetCellAccount(indexRow, "cPassMail");
            string cFa2 = GetCellAccount(indexRow, "cFa2");
            string cPassword = GetCellAccount(indexRow, "cPassword");
            string cCookies = GetCellAccount(indexRow, "cCookies");
            string cToken = GetCellAccount(indexRow, "cToken");
            string cUseragent = GetCellAccount(indexRow, "cUseragent");
            if (cUid == "")
            {
                cUid = Regex.Match(cCookies, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, cStatus + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike...");
                            lock (lock_StartProxy)
                            {
                                while (!isStop)
                                {
                                    shopLike = null;
                                    while (!isStop)
                                    {
                                        foreach (ShopLike item5 in listShopLike)
                                        {
                                            if (shopLike == null || item5.daSuDung < shopLike.daSuDung)
                                            {
                                                shopLike = item5;
                                            }
                                        }
                                        if (shopLike.daSuDung != shopLike.limit_theads_use)
                                        {
                                            break;
                                        }
                                    }
                                    if (isStop)
                                    {
                                        break;
                                    }
                                    if (shopLike.daSuDung > 0 || shopLike.ChangeProxy())
                                    {
                                        cProxy = shopLike.proxy;
                                        if (cProxy == "")
                                        {
                                            cProxy = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag3 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    cStatus = "(IP: " + cProxy.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, cStatus + "Check IP...");
                                    cCheckProxy = Helpers.Common.CheckProxy(cProxy, 0);
                                    if (cCheckProxy == "")
                                    {
                                        flag4 = false;
                                    }
                                }
                                if (!flag4)
                                {
                                    ShopLike shopLike2 = shopLike;
                                    shopLike2.dangSuDung--;
                                    shopLike2 = shopLike;
                                    shopLike2.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        case 2:
                            SetStatusAccount(indexRow, "Đang lấy proxy Tinsoft...");
                            lock (lock_StartProxy)
                            {
                                while (!isStop)
                                {
                                    tinsoftProxy = null;
                                    while (!isStop)
                                    {
                                        foreach (TinsoftProxy item in listTinsoft)
                                        {
                                            if (tinsoftProxy == null || item.daSuDung < tinsoftProxy.daSuDung)
                                            {
                                                tinsoftProxy = item;
                                            }
                                        }
                                        if (tinsoftProxy.daSuDung != tinsoftProxy.limit_theads_use)
                                        {
                                            break;
                                        }
                                    }
                                    if (isStop)
                                    {
                                        break;
                                    }
                                    if (tinsoftProxy.daSuDung > 0 || tinsoftProxy.ChangeProxy())
                                    {
                                        cProxy = tinsoftProxy.proxy;
                                        if (cProxy == "")
                                        {
                                            cProxy = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, cStatus + Language.GetValue("Đã dừng!"));
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    cStatus = "(IP: " + cProxy.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, cStatus + "Check IP...");
                                    cCheckProxy = Helpers.Common.CheckProxy(cProxy, 0);
                                    if (cCheckProxy == "")
                                    {
                                        flag12 = false;
                                    }
                                }
                                if (!flag12)
                                {
                                    tinsoftProxy.dangSuDung--;
                                    tinsoftProxy.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        case 3:
                            SetStatusAccount(indexRow, "Đang lấy Proxy MinProxy ...");
                            lock (lock_StartProxy)
                            {
                                while (!isStop)
                                {
                                    minProxy = null;
                                    while (!isStop)
                                    {
                                        foreach (MinProxy item5 in listMinproxy)
                                        {
                                            if (minProxy == null || item5.daSuDung < minProxy.daSuDung)
                                            {
                                                minProxy = item5;
                                            }
                                        }
                                        if (minProxy.daSuDung != minProxy.limit_theads_use)
                                        {
                                            break;
                                        }
                                    }
                                    if (isStop)
                                    {
                                        break;
                                    }
                                    if (minProxy.daSuDung > 0 || minProxy.ChangeProxy())
                                    {
                                        cProxy = minProxy.proxy;
                                        if (cProxy == "")
                                        {
                                            cProxy = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag3 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    cStatus = "(IP: " + cProxy.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, cStatus + "Check IP...");
                                    cCheckProxy = Helpers.Common.CheckProxy(cProxy, 0);
                                    if (cCheckProxy == "")
                                    {
                                        flag4 = false;
                                    }
                                }
                                if (!flag4)
                                {
                                    MinProxy minProxy2 = minProxy;
                                    minProxy2.dangSuDung--;
                                    minProxy2 = minProxy;
                                    minProxy2.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        default:
                            {
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    cStatus = "(IP: " + cCheckProxy + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                    break;
                                }
                                try
                                {
                                    SetStatusAccount(indexRow, cStatus + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num5 = rd.Next(1, 3);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - 3 < 0)
                                                {
                                                    SetStatusAccount(indexRow, cStatus + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                                        goto stop;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayChrome++;
                                        }
                                    }
                                    SetStatusAccount(indexRow, cStatus + "Đang mở trình duyệt...");
                                    if (cUseragent == "")
                                    {
                                        cUseragent = Base.useragentDefault;
                                    }
                                    string pathP = "";
                                    if (profilePath != "" && cUid != "")
                                    {
                                        pathP = profilePath + "\\" + cUid;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(pathP))
                                        {
                                            pathP = "";
                                        }
                                    }

                                    Point position = Helpers.Common.GetPointFromIndexPosition(indexPos, 5, 2);
                                    Point sizeChrome = Helpers.Common.GetSizeChrome(5, 2);
                                    chrome = new Chrome
                                    {
                                        IndexChrome = indexRow,
                                        UserAgent = cUseragent,
                                        ProfilePath = pathP,
                                        Size = sizeChrome,
                                        Position = position,
                                        TimeWaitForSearchingElement = 3,
                                        TimeWaitForLoadingPage = 120,
                                        Proxy = cProxy,
                                        TypeProxy = typeProxy,
                                        scaleChorme = false,
                                    };
                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, cStatus + "Đã dừng!");
                                        goto stop;
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, cStatus + "Lỗi mở trình duyệt!");
                                            goto stop;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + cProxy + "\"");

                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && cProxy.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://api.myip.com/");
                                            chrome.DelayTime(1.0);
                                            string pageSource = chrome.GetPageSource();
                                            if (!pageSource.Contains("ip"))
                                            {
                                                chrome.Close();
                                                num6++;
                                                if (num6 < 3)
                                                {
                                                    continue;
                                                }
                                                SetStatusAccount(indexRow, cStatus + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        if (!chrome.GetProcess())
                                        {
                                            SetStatusAccount(indexRow, cStatus + "Không check được chrome!");
                                            break;
                                        }

                                        string linkLogin = "";
                                        linkLogin = (setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") != 0) ? "https://www.facebook.com/" : "https://m.facebook.com/";
                                        if (pathP.Trim() != "")
                                        {
                                            switch (CommonChrome.CheckLiveCookie(chrome, linkLogin))
                                            {
                                                case 1:
                                                    flag = true;
                                                    SetStatusAccount(indexRow, cStatus + "Nick Live");
                                                    SetInfoAccount(cId, indexRow, "Live");
                                                    SetRowColor(indexRow, 2);
                                                    goto stop;
                                                case -2:
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto stop;
                                                case -3:
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto stop;
                                                case 2:
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cId, indexRow, "Checkpoint");
                                                    goto unlockCheckPoint;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            int valueInt = setting_MoTrinhDuyet.GetValueInt("typeLogin");
                                            switch (valueInt)
                                            {
                                                case 0:
                                                    SetStatusAccount(indexRow, cStatus + "Đăng nhập bằng uid|pass...");
                                                    break;
                                                case 1:
                                                    SetStatusAccount(indexRow, cStatus + "Đăng nhập bằng email|pass...");
                                                    break;
                                                case 2:
                                                    SetStatusAccount(indexRow, cStatus + "Đăng nhập bằng cookie...");
                                                    break;
                                            }
                                            string isLogin = LoginFacebook(chrome, valueInt, linkLogin, cUid, cEmail, cPassword, cFa2, cCookies, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                            switch (isLogin)
                                            {
                                                case "-3":
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto stop;
                                                case "-2":
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto errorBreak;
                                                case "0":
                                                    SetStatusAccount(indexRow, cStatus + "Đăng nhập thất bại!");
                                                    goto errorBreak;
                                                case "1":
                                                    flag = true;
                                                    goto errorBreak;
                                                case "2":
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cId, indexRow, "Checkpoint");
                                                    goto unlockCheckPoint;
                                                case "3":
                                                    SetStatusAccount(indexRow, cStatus + "Không có 2fa!");
                                                    goto errorBreak;
                                                case "4":
                                                    SetStatusAccount(indexRow, cStatus + "Tài khoản không đúng!");
                                                    goto errorBreak;
                                                case "5":
                                                    SetStatusAccount(indexRow, cStatus + "Mật khẩu không đúng!");
                                                    SetInfoAccount(cId, indexRow, "Changed pass");
                                                    goto errorBreak;
                                                case "6":
                                                    SetStatusAccount(indexRow, cStatus + "Mã 2fa không đúng!");
                                                    goto errorBreak;
                                                default:
                                                    {
                                                        SetStatusAccount(indexRow, cStatus + isLogin);
                                                        goto errorBreak;
                                                    }
                                                errorBreak:
                                                    if (flag)
                                                    {
                                                        SetStatusAccount(indexRow, cStatus + "Nick Live");
                                                        SetInfoAccount(cId, indexRow, "Live");
                                                        SetRowColor(indexRow, 2);
                                                        break;
                                                    }
                                                    SetStatusAccount(indexRow, cStatus + "Đăng nhập thất bại!");
                                                    SetRowColor(indexRow, 1);
                                                    ScreenCaptureError(chrome, cUid, 1);
                                                    goto stop;
                                            }
                                        }
                                        SetInfoAccount(cId, indexRow, "Live");
                                        SetRowColor(indexRow, 2);
                                        goto stop;

                                    unlockCheckPoint:
                                        SetStatusAccount(indexRow, cStatus + "Check...");

                                        goto stop;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SetStatusAccount(indexRow, cStatus + "Lỗi không xác định!");
                                    Helpers.Common.ExportError(chrome, ex);
                                }
                                break;
                            }
                        stop:
                            chrome.Close();
                            break;
                    }
                    break;
                }
            }
            if (!flag && setting_MoTrinhDuyet.GetValueBool("isAutoCloseChromeLoginFail"))
            {
                try
                {
                    chrome.Close();
                    cControl("stop");
                }
                catch
                {
                }
            }
            if (flag2 && Directory.Exists(setting_general.GetValue("txbPathProfile") + "\\" + cCid))
            {
                string pathFr = setting_general.GetValue("txbPathProfile") + "\\" + cCid;
                string pathTo = setting_general.GetValue("txbPathProfile") + "\\" + cUid;
                if (!Helpers.Common.MoveFolder(pathFr, pathTo) && Helpers.Common.CopyFolder(pathFr, pathTo))
                {
                    Helpers.Common.DeleteFolder(pathFr);
                }
            }
            lock (lock_FinishProxy)
            {
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        shopLike?.DecrementDangSuDung();
                        break;
                    case 2:
                        tinsoftProxy?.DecrementDangSuDung();
                        break;
                    case 3:
                        minProxy?.DecrementDangSuDung();
                        break;
                }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            cControl("stop");
        }
        private void ScreenCaptureError(Chrome chrome, string uid, int type)
        {
            try
            {
                if (chrome != null)
                {
                    string text = Application.StartupPath + "\\log_capture";
                    switch (type)
                    {
                        case 0:
                            text += "\\checkpoint";
                            break;
                        case 1:
                            text += "\\loginfail";
                            break;
                        case 2:
                            text += "\\disconnect";
                            break;
                    }
                    Helpers.Common.CreateFolder(text);
                    chrome.ScreenCapture(text, uid);
                    File.WriteAllText(text + "\\" + uid + ".txt", chrome.GetURL());
                    File.WriteAllText(text + "\\" + uid + ".html", chrome.GetPageSource());
                }
            }
            catch
            {
            }
        }

        private string LoginFacebook(Chrome chrome, int typeLogin, string typeWeb, string uid, string email, string pass, string fa2, string cookie, int tocDoGoVanBan, bool isDontSaveBrowser)
        {
            string result = "";
            switch (typeLogin)
            {
                case 0:
                    if (uid.Trim() == "" || pass.Trim() == "")
                    {
                        if (uid.Trim() == "")
                        {
                            result = "Không tìm thấy UID!";
                        }
                        else if (pass.Trim() == "")
                        {
                            result = "Không tìm thấy Pass!";
                        }
                    }
                    else
                    {
                        result = CommonChrome.LoginFacebookUsingUidPassNew(chrome, uid, pass, fa2, typeWeb, tocDoGoVanBan, isDontSaveBrowser);
                    }
                    break;
                case 1:
                    if (email.Trim() == "" || pass.Trim() == "")
                    {
                        if (email.Trim() == "")
                        {
                            result = "Không tìm thấy Email!";
                        }
                        else if (pass.Trim() == "")
                        {
                            result = "Không tìm thấy Pass!";
                        }
                    }
                    else
                    {
                        result = CommonChrome.LoginFacebookUsingUidPassNew(chrome, email, pass, fa2, typeWeb, tocDoGoVanBan, isDontSaveBrowser);
                    }
                    break;
                case 2:
                    result = (!(cookie.Trim() == "")) ? CommonChrome.LoginFacebookUsingCookie(chrome, cookie, typeWeb) : "Không tìm thấy Cookie!";
                    break;
            }
            return result;
        }

    }
}

using core;
using core.KichBan;
using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.core.Enum;
using Facebook_Tool_Request.Helpers;
using Facebook_Tool_Request.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Web;
using AutoUpdaterDotNET;
using Facebook_Tool_Request.core.fTools;
using cuakit;
using OpenQA.Selenium;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using OtpNet;

namespace Facebook_Tool_Request.core
{
    public partial class fMain : Form
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
        private JSON_Settings setting_ShowDtgv;
        private JSON_Settings setting_InteractGeneral;
        private JSON_Settings setting_InteractGeneralPage;
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
        //config thao tac
        private Dictionary<string, List<string>> dicDangStatus_NoiDung = null;
        private Dictionary<string, List<string>> dicDangStatus_NoiDungGoc = null;
        private Dictionary<string, List<string>> dicSpamComment_NoiDung = null;
        private Dictionary<string, List<string>> dicSpamComment_NoiDungGoc = null;
        private Dictionary<string, List<string>> dicSpamComment_listUid = null;

        private Dictionary<string, List<string>> dicSpamCommentPage_NoiDung = null;
        private Dictionary<string, List<string>> dicSpamCommentPage_NoiDungGoc = null;
        private Dictionary<string, List<string>> dicSpamCommentPage_listUid = null;

        private object lock_baivietprofile = new object();
        private Dictionary<string, List<string>> dicHDRegPage_Name = null;
        private Dictionary<string, List<string>> dicHDRegPage_NameGoc = null;
        private Dictionary<string, List<string>> dicSeedingFb_NoiDung = null;
        private Dictionary<string, List<string>> dicSeedingFb_NoiDungGoc = null;
        private Dictionary<string, List<string>> dicHDKetBanUid = null;

        public fMain()
        {
            InitializeComponent();
            remote = this;
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
                    result = (!(text2 == "999999")) ? new List<string> { cbbThuMuc.SelectedValue.ToString() } : CollectionHelper.CloneList(fChonNhieuFolder.lstChooseIdFiles);
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

        private void fMain_Load(object sender, EventArgs e)
        {
            AutoUpdate();
            Base.useragentDefault = CommonChrome.GetUserAgentDefault();
            SetupFolder.StartApplication();
            LoadSetting();
            LoadConfigManHinh();
            LoadCbbThuMuc();
            LoadCbbTinhTrang();
            LoadcbbSearch();
            Base.width = base.Width;
            Base.heigh = base.Height;
            //lblNgayHetHan.Text = Settings.Default.hethan;
            //lblFullName.Text = Settings.Default.user
            lblNgayHetHan.Text = "Vĩnh Viễn";
            lblFullName.Text = License.Hardware.getHDD();
            menuStrip1.Cursor = Cursors.Hand;
            this.titleApplication.Text = "BaSetup.Vn ToolKit Ver " + lblVersions.Text;
            UpdateSQLVersionNew();

            //load page
            LoadPageFromFile();
        }
        public void UpdateSQLVersionNew()
        {
            bool tablePage = CommonSQL.CheckExistTable("pages");
            bool tablepageFolders = CommonSQL.CheckExistTable("pageFolders");
            bool tableinvitePage = CommonSQL.CheckExistTable("invitePages");
            bool tablePages = CommonSQL.CheckExistTable("pagess");

            if (!tablePages)
            {
                string query = @"CREATE TABLE pagess (id INTEGER,pageId TEXT,pageName TEXT,like TEXT,follow TEXT,uid TEXT,lastInteract TEXT,categoryId INTEGER, tiepcan TEXT, tuongtac TEXT, ghichu TEXT, status TEXT, datecreated TEXT, idbusiness TEXT, interactEnd TEXT, info TEXT, avatar TEXT, token TEXT, countgroup TEXT, PRIMARY KEY(id AUTOINCREMENT))";
                try
                {
                    Connector.Instance.ExecuteNonQuery(query);
                }
                catch
                {
                }
            }
            if (!tablePage)
            {
                string query = @"CREATE TABLE pages (id	INTEGER,uidAdmins TEXT,pageId	TEXT,idAccess	TEXT,pageName	TEXT,follow	TEXT,status	TEXT,idfolder	TEXT,active	TEXT,dateImport	TEXT,deleteDate TEXT,interactEnd TEXT, PRIMARY KEY(id AUTOINCREMENT));";
                try
                {
                    Connector.Instance.ExecuteNonQuery(query);
                }
                catch
                {
                }
            }

            if (!tablepageFolders)
            {
                string query = @"CREATE TABLE pageFolders (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, createddate TEXT , active INT);";
                try
                {
                    Connector.Instance.ExecuteNonQuery(query);
                }
                catch
                {
                }
            }

            if (!tableinvitePage)
            {
                string query = @"CREATE TABLE invitePages (id	INTEGER,uid_clone	TEXT,uid_page	TEXT,name_page	TEXT,active	TEXT,note	TEXT,status	TEXT,dateImport	TEXT,interactEnd TEXT, PRIMARY KEY(id AUTOINCREMENT));";
                try
                {
                    Connector.Instance.ExecuteNonQuery(query);
                }
                catch
                {
                }
            }
        }
        private void AutoUpdate()
        {
            //update
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            string version = fvi.FileVersion;
            lblVersions.Text = version;
            AutoUpdater.DownloadPath = "update";
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.IsUpdateAvailable)
            {
                //DialogResult dialogResult;
                //dialogResult =
                //        MessageBox.Show(
                //            $@"Có phiên bản mới {args.CurrentVersion}. Phiên bản hiện tại  {args.InstalledVersion}. Cập nhật thôi nhỉ? ", @"Cập nhật phần mềm",
                //            MessageBoxButtons.YesNo,
                //            MessageBoxIcon.Information);
                AutoUpdater.ShowUpdateForm(args);
                //if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                //{
                //    try
                //    {
                //        if (AutoUpdater.DownloadUpdate(args))
                //        {
                //            Application.Exit();
                //        }
                //    }
                //    catch (Exception exception)
                //    {
                //        MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                //            MessageBoxIcon.Error);
                //    }
                //}
            }
            else
            {
                MessageBox.Show(@"Phiên bản mới nhất rồi nè!.", @"Úi Sời!!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

        public void SetCellAccount(int indexRow, string column, object value, bool isAllowEmptyValue = true)
        {
            if (isAllowEmptyValue || !(value.ToString().Trim() == ""))
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, column, value);
            }
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
        private void tấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("All");
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

        private void bôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("SelectHighline");
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

        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("UnAll");
        }

        private void xoáTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAccount(isDeleteProfile: false);
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
                    MessageBoxHelper.ShowMessageBox(("Xóa thất bại, vui lòng thử lại sau!"), 2);
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
        public void SetStatusAccount(int indexRow, string value, int timeWait = -1)
        {
            if (timeWait != -1)
            {
                if (timeWait != 0)
                {
                    DatagridviewHelper.SetStatusDataGridViewWithWait(this.dtgvAcc, indexRow, "cStatus", timeWait, value);
                }
            }
            else
            {
                DatagridviewHelper.SetStatusDataGridView(this.dtgvAcc, indexRow, "cStatus", value);
            }
        }

        private void tảiLạiDanhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnLoadAcc_Click(null, null);
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

        private void càiĐặtHiểnThịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullString = new JSON_Settings("configDatagridview").GetFullString();
            Helpers.Common.ShowForm(new fCaiDatHienThi());
            if (fullString != new JSON_Settings("configDatagridview").GetFullString())
            {
                LoadConfigManHinh();
            }
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
            dtgvAcc.Columns["cPages"].Visible = setting_ShowDtgv.GetValueBool("ckbPage");
            dtgvAcc.Columns["cBirthday"].Visible = setting_ShowDtgv.GetValueBool("ckbNgaySinh");
            dtgvAcc.Columns["cGender"].Visible = setting_ShowDtgv.GetValueBool("ckbGioiTinh");
            dtgvAcc.Columns["cPassword"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhau");
            dtgvAcc.Columns["cPassMail"].Visible = setting_ShowDtgv.GetValueBool("ckbMatKhauMail");
            dtgvAcc.Columns["cMailRecovery"].Visible = setting_ShowDtgv.GetValueBool("cMailRecovery");
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
            dtgvAcc.Columns["cStatus282"].Visible = setting_ShowDtgv.GetValueBool("cStatus282");

            //page
            dtgvPage.Columns["cLikeP"].Visible = setting_ShowDtgv.GetValueBool("ckbPLike");
            dtgvPage.Columns["cFollowP"].Visible = setting_ShowDtgv.GetValueBool("ckbPFollow");
            dtgvPage.Columns["cTiepCanP"].Visible = setting_ShowDtgv.GetValueBool("ckbPTiepcan");
            dtgvPage.Columns["cTuongTac"].Visible = setting_ShowDtgv.GetValueBool("ckbPTuongtac");
            dtgvPage.Columns["cDateCreated"].Visible = setting_ShowDtgv.GetValueBool("ckbPNgaytao");
            dtgvPage.Columns["cInfoP"].Visible = setting_ShowDtgv.GetValueBool("ckbPAvtcover");
            dtgvPage.Columns["cThuMucP"].Visible = setting_ShowDtgv.GetValueBool("ckbPFolder");
            dtgvPage.Columns["cGroupP"].Visible = setting_ShowDtgv.GetValueBool("ckbPGroup");

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCaiDatChung());
        }

        private void LoadSetting()
        {
            setting_general = new JSON_Settings("configGeneral");
            setting_InteractGeneral = new JSON_Settings("configInteractGeneral");
            setting_InteractGeneralPage = new JSON_Settings("configTuongTacPage");
            Settings.Default.PathChrome = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            Settings.Default.Save();
        }

        private void btnRandomAccount_Click(object sender, EventArgs e)
        {
            RandomAccountListView();
        }

        private void RandomAccountListView(int soLuot = 1)
        {
            try
            {
                for (int i = 0; i < soLuot; i++)
                {
                    if (dtgvAcc.RowCount <= 1)
                    {
                        continue;
                    }
                    List<DataGridViewRow> list = new List<DataGridViewRow>();
                    foreach (DataGridViewRow item in (IEnumerable)dtgvAcc.Rows)
                    {
                        list.Add(item);
                    }
                    int num = list.Count;
                    while (num > 1)
                    {
                        num--;
                        int index = Base.rd.Next(num + 1);
                        DataGridViewRow value = list[index];
                        list[index] = list[num];
                        list[num] = value;
                    }
                    dtgvAcc.Rows.Clear();
                    foreach (DataGridViewRow item2 in list)
                    {
                        dtgvAcc.Rows.Add(item2);
                    }
                }
            }
            catch
            {
            }
        }

        private void GetTokenByRequest(int type)
        {
            //GetTokenByRequest(1);
            try
            {
                if (CountChooseRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox(("Vui lòng chọn những tài khoản muốn Get Token!"), 3);
                    return;
                }
                LoadSetting();
                int iThread = 0;
                int maxThread = setting_general.GetValueInt("nudHideThread", 10);
                isStop = false;
                bool isChangeIPSuccess = false;
                int curChangeIp = 0;
                new Thread((ThreadStart)delegate
                {
                    cControl("start");
                    int num = type;
                    int num2 = num;
                    if (num2 != 0)
                    {
                        if (num2 == 1)
                        {
                            int num3 = 0;
                            while (num3 < dtgvAcc.Rows.Count && !isStop)
                            {
                                if (!Convert.ToBoolean(dtgvAcc.Rows[num3].Cells["cChose"].Value))
                                {
                                    num3++;
                                    continue;
                                }
                                if (iThread < maxThread)
                                {
                                    Interlocked.Increment(ref iThread);
                                    int row2 = num3++;
                                    new Thread((ThreadStart)delegate
                                    {
                                        SetStatusAccount(row2, ("Đang kiểm tra..."));
                                        string statusDataGridView3 = CommonCSharp.GetStatusDataGridView(dtgvAcc, row2, "cId");
                                        string statusDataGridView4 = CommonCSharp.GetStatusDataGridView(dtgvAcc, row2, "cCookies");
                                        string text3 = GetCellAccount(row2, "cUseragent");
                                        string text4 = "";
                                        int typeProxy2 = 0;
                                        if (text3 == "" || text4.Split(':').Length == 4)
                                        {
                                            text3 = Base.useragentDefault;
                                        }
                                        if (statusDataGridView4 == "")
                                        {
                                            SetStatusAccount(row2, ("Cookie trống!"));
                                        }
                                        else
                                        {
                                            string tokenEAAG = CommonRequest.GetTokenEAAGNo2Fa(statusDataGridView4, text3, text4, typeProxy2);
                                            if (tokenEAAG == "-1")
                                            {
                                                SetStatusAccount(row2, "Cookie die!");
                                            }
                                            else if (tokenEAAG == "" || tokenEAAG == null)
                                            {
                                                SetStatusAccount(row2, "Lấy token thất bại!");
                                            }
                                            else
                                            {
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView3, "token", tokenEAAG);
                                                SetCellAccount(row2, "cToken", tokenEAAG);
                                                SetStatusAccount(row2, ("Lấy token thành công!"));
                                            }

                                        }
                                    }).Start();
                                    continue;
                                }
                                if (Convert.ToInt32((setting_general.GetValue("ip_iTypeChangeIp") == "") ? "0" : setting_general.GetValue("ip_iTypeChangeIp")) == 0 || Convert.ToInt32((setting_general.GetValue("ip_iTypeChangeIp") == "") ? "0" : setting_general.GetValue("ip_iTypeChangeIp")) == 1)
                                {
                                    Helpers.Common.DelayTime(1.0);
                                    continue;
                                }
                                while (iThread > 0)
                                {
                                    Helpers.Common.DelayTime(1.0);
                                }
                                if (isStop)
                                {
                                    break;
                                }
                                Interlocked.Increment(ref curChangeIp);
                                if (curChangeIp < Convert.ToInt32((setting_general.GetValue("ip_nudChangeIpCount") == "") ? "1" : setting_general.GetValue("ip_nudChangeIpCount")))
                                {
                                    continue;
                                }
                                goto errorChangeIp2;
                            }
                        }
                    }
                    else
                    {
                        int num4 = 0;
                        while (num4 < dtgvAcc.Rows.Count && !isStop)
                        {
                            if (!Convert.ToBoolean(dtgvAcc.Rows[num4].Cells["cChose"].Value))
                            {
                                num4++;
                                continue;
                            }
                            if (iThread < maxThread)
                            {
                                Interlocked.Increment(ref iThread);
                                int row = num4++;
                                new Thread((ThreadStart)delegate
                                {
                                    SetStatusAccount(row, "Đang kiểm tra...");
                                    string statusDataGridView = CommonCSharp.GetStatusDataGridView(dtgvAcc, row, "cId");
                                    string statusDataGridView2 = CommonCSharp.GetStatusDataGridView(dtgvAcc, row, "cCookies");
                                    string text = GetCellAccount(row, "cUseragent");
                                    string text2 = "";
                                    int typeProxy = 0;
                                    if (setting_general.GetValueInt("ip_iTypeChangeIp") == 9)
                                    {
                                        text2 = GetCellAccount(row, "cProxy");
                                        typeProxy = (text2.EndsWith("*1") ? 1 : 0);
                                        if (text2.EndsWith("*0") || text2.EndsWith("*1"))
                                        {
                                            text2 = text2.Substring(0, text2.Length - 2);
                                        }
                                    }
                                    if (text == "" || text2.Split(':').Length == 4)
                                    {
                                        text = Base.useragentDefault;
                                    }
                                    if (statusDataGridView2 == "")
                                    {
                                        SetStatusAccount(row, ("Cookie trống!"));
                                    }
                                    else
                                    {
                                        //string tokenEAAAAZ = CommonRequest.GetTokenEAAAAZ(statusDataGridView2, text, text2, typeProxy);
                                        //if (tokenEAAAAZ == "-1")
                                        //{
                                        //    SetStatusAccount(row, "Cookie die!");
                                        //}
                                        //else
                                        //{
                                        //    CommonSQL.UpdateFieldToAccount(statusDataGridView, "token", tokenEAAAAZ);
                                        //    SetCellAccount(row, "cToken", tokenEAAAAZ);
                                        //    SetStatusAccount(row, ("Lấy token thành công!"));
                                        //}
                                    }
                                    Interlocked.Decrement(ref iThread);
                                }).Start();
                                continue;
                            }
                            if (setting_general.GetValueInt("ip_iTypeChangeIp") == 0 || setting_general.GetValueInt("ip_iTypeChangeIp") == 1 || setting_general.GetValueInt("ip_iTypeChangeIp") == 2 || setting_general.GetValueInt("ip_iTypeChangeIp") == 3)
                            {
                                Helpers.Common.DelayTime(1.0);
                                continue;
                            }
                            while (iThread > 0)
                            {
                                Helpers.Common.DelayTime(1.0);
                            }
                            if (isStop)
                            {
                                break;
                            }
                            Interlocked.Increment(ref curChangeIp);
                            if (curChangeIp < Convert.ToInt32(setting_general.GetValueInt("ip_nudChangeIpCount", 1)))
                            {
                                continue;
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                isChangeIPSuccess = Helpers.Common.ChangeIP(setting_general.GetValueInt("ip_iTypeChangeIp"), setting_general.GetValueInt("typeDcom"), setting_general.GetValue("ip_txtProfileNameDcom"), setting_general.GetValue("txtUrlHilink"), setting_general.GetValueInt("ip_cbbHostpot"), setting_general.GetValue("ip_txtNordVPN"));
                                if (isChangeIPSuccess)
                                {
                                    break;
                                }
                            }
                            if (isChangeIPSuccess)
                            {
                                curChangeIp = 0;
                                continue;
                            }
                            goto errorChangeIp;
                        }
                    }
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 60000)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                    goto stop;
                errorChangeIp:
                    MessageBoxHelper.ShowMessageBox("Không thể đổi IP", 3);
                    goto stop;
                stop:
                    cControl("stop");
                    return;
                errorChangeIp2:
                    MessageBoxHelper.ShowMessageBox("Không thể đổi IP", 3);
                    goto stop;
                }).Start();


            }
            catch
            {

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
                        btnStartThaotac.Enabled = false;
                        btnRunPage.Enabled = false;
                        grFolder.Enabled = false;
                        btnStopThaoTac.Enabled = true;
                        autoBASETUPToolStripMenuItem.Enabled = false;
                        gunaButton7.Enabled = true;

                    }
                    else if (dt == "stop")
                    {
                        // btnStopThaoTac.Text = ("Dừng Thao Tác");
                        btnStartThaotac.Enabled = true;
                        btnRunPage.Enabled = true;
                        grFolder.Enabled = true;
                        btnStopThaoTac.Enabled = false;
                        gunaButton7.Enabled = false;
                        autoBASETUPToolStripMenuItem.Enabled = true;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void tiếnHànhMởToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần mở trình duyệt!"), 3);
            }
            else
            {
                MoTrinhDuyet();
            }
        }

        private void MoTrinhDuyet()
        {
            try
            {
                LoadSetting();
                setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
                string profilePath = "";
                if (!setting_MoTrinhDuyet.GetValueBool("isUseProfile"))
                {
                    goto isProfile;
                }

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
                int maxThread = CountChooseRowInDatagridview();
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        {
                            listProxyShopLike = setting_general.GetValueList("txtApiShopLike");

                            if (listProxyShopLike.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox(("Key ShopLike không đủ, vui lòng mua thêm!"), 2);
                                return;
                            }

                            listShopLike = new List<ShopLike>();
                            for (int i = 0; i < listProxyShopLike.Count; i++)
                            {
                                ShopLike item = new ShopLike(listProxyShopLike[i], 0, setting_general.GetValueInt("nudLuongPerIPShopLike"), setting_general.GetValue("cbbLocationShopLikePrx"));
                                listShopLike.Add(item);
                            }

                            if (listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike") < maxThread)
                            {
                                maxThread = listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike");
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
                isStop = false;
                List<int> lstPossition = new List<int>();
                for (int n = 0; n < CountChooseRowInDatagridview(); n++)
                {
                    lstPossition.Add(0);
                }
                checkDelayChrome = 0;
                if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                {
                    OpenFormViewChrome();
                }
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    try
                    {
                        int num = 0;
                        while (num < dtgvAcc.Rows.Count && !isStop)
                        {
                            if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                            {
                                if (isStop || lstThread.Count >= maxThread)
                                {
                                    break;
                                }
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
                                        MoTrinhDuyetOneThread(row, indexOfPossitionApp, profilePath);
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
                            }
                            else
                            {
                                num++;
                            }
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
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
                    }
                }).Start();
            quit:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
            }
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
        private string ConvertCookie(string cookie)
        {
            string text = "";
            string value = Regex.Match(cookie + ";", "sb=(.*?);").Groups[1].Value;
            if (value != "")
            {
                text = text + "sb=" + value + "; ";
            }
            string value2 = Regex.Match(cookie + ";", "wd=(.*?);").Groups[1].Value;
            if (value2 != "")
            {
                text = text + "wd=" + value2 + "; ";
            }
            string value3 = Regex.Match(cookie + ";", "datr=(.*?);").Groups[1].Value;
            if (value3 != "")
            {
                text = text + "datr=" + value3 + "; ";
            }
            string value4 = Regex.Match(cookie + ";", "locale=(.*?);").Groups[1].Value;
            if (value4 != "")
            {
                text = text + "locale=" + value4 + "; ";
            }
            string value5 = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
            if (value5 != "")
            {
                text = text + "c_user=" + value5 + "; ";
            }
            string value6 = Regex.Match(cookie + ";", "xs=(.*?);").Groups[1].Value;
            if (value6 != "")
            {
                text = text + "xs=" + value6 + "; ";
            }
            string value7 = Regex.Match(cookie + ";", "fr=(.*?);").Groups[1].Value;
            if (value7 != "")
            {
                text = text + "fr=" + value7 + "; ";
            }
            string value8 = Regex.Match(cookie + ";", "spin=(.*?);").Groups[1].Value;
            if (value8 != "")
            {
                text = text + "spin=" + value8 + "; ";
            }
            return text;
        }
        private string ConvertCookieOptimize(string cookie)
        {
            string text = "";
            string value = Regex.Match(cookie + ";", "sb=(.*?);").Groups[1].Value;
            if (value != "")
            {
                text = text + "sb=" + value + "; ";
            }
            string value2 = Regex.Match(cookie + ";", "wd=(.*?);").Groups[1].Value;
            if (value2 != "")
            {
                //text = text + "wd=" + value2 + "; ";
            }
            string value3 = Regex.Match(cookie + ";", "datr=(.*?);").Groups[1].Value;
            if (value3 != "")
            {
                text = text + "datr=" + value3 + "; ";
            }
            string value4 = Regex.Match(cookie + ";", "locale=(.*?);").Groups[1].Value;
            if (value4 != "")
            {
                text = text + "locale=" + value4 + "; ";
            }
            string value5 = Regex.Match(cookie + ";", "c_user=(.*?);").Groups[1].Value;
            if (value5 != "")
            {
                text = text + "c_user=" + value5 + "; ";
            }
            string value6 = Regex.Match(cookie + ";", "xs=(.*?);").Groups[1].Value;
            if (value6 != "")
            {
                text = text + "xs=" + value6 + "; ";
            }
            string value7 = Regex.Match(cookie + ";", "fr=(.*?);").Groups[1].Value;
            if (value7 != "")
            {
                text = text + "fr=" + value7 + "; ";
            }
            string value8 = Regex.Match(cookie + ";", "spin=(.*?);").Groups[1].Value;
            if (value8 != "")
            {
                //  text = text + "spin=" + value8 + "; ";
            }
            return text;
        }

        private void MoTrinhDuyetOneThread(int indexRow, int indexPos, string profilePath)
        {
            string text = "";
            Chrome chrome = null;
            int num = 0;
            bool flag = false;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            MinProxy minProxy = null;
            TinsoftProxy tinsoftProxy = null;
            bool flag2 = false;
            string text4 = "";
            string text5 = GetCellAccount(indexRow, "cUid");
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cEmail");
            string cellAccount3 = GetCellAccount(indexRow, "cPassMail");
            string cellAccount4 = GetCellAccount(indexRow, "cFa2");
            string cellAccount5 = GetCellAccount(indexRow, "cPassword");
            string cellAccount6 = GetCellAccount(indexRow, "cCookies");
            string cellAccount7 = GetCellAccount(indexRow, "cToken");
            string text6 = GetCellAccount(indexRow, "cUseragent");
            if (text5 == "")
            {
                text5 = Regex.Match(cellAccount6, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
                            lock (lock_StartProxy)
                            {
                                while (!isStop)
                                {
                                    shopLike = null;
                                    while (!isStop)
                                    {
                                        foreach (ShopLike prx in listShopLike)
                                        {
                                            if (shopLike == null || prx.daSuDung < shopLike.daSuDung)
                                            {
                                                shopLike = prx;
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        foreach (MinProxy prx in listMinproxy)
                                        {
                                            if (minProxy == null || prx.daSuDung < minProxy.daSuDung)
                                            {
                                                minProxy = prx;
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                try
                                {
                                    SetStatusAccount(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num5 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num5 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                                        goto end;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayChrome++;
                                        }
                                    }
                                    SetStatusAccount(indexRow, text2 + "Đang mở trình duyệt...");
                                    string app = "data:,";

                                    if (text6 == "")
                                    {
                                        text6 = Base.useragentDefault;
                                    }
                                    string text7 = "";
                                    if (profilePath != "" && text5 != "")
                                    {
                                        text7 = profilePath + "\\" + text5;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(text7))
                                        {
                                            text7 = "";
                                        }
                                    }
                                    Point position;
                                    Point size;
                                    if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                    {
                                        position = new Point(-1000, 0);
                                        size = new Point(setting_MoTrinhDuyet.GetValueInt("nudWidthChrome") + 16, setting_MoTrinhDuyet.GetValueInt("nudHeighChrome"));
                                    }
                                    else
                                    {
                                        position = Helpers.Common.GetPointFromIndexPosition(indexPos, setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                        size = Helpers.Common.GetSizeChrome(setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                    }
                                    chrome = new Chrome
                                    {
                                        IndexChrome = indexRow,
                                        DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract")),
                                        UserAgent = text6,
                                        ProfilePath = text7,
                                        Size = size,
                                        Position = position,
                                        TimeWaitForSearchingElement = 3,
                                        TimeWaitForLoadingPage = 120,
                                        Proxy = text,
                                        TypeProxy = typeProxy,
                                        App = app,
                                        IsUsePortable = setting_general.GetValueBool("ckbUsePortable"),
                                        PathToPortableZip = setting_general.GetValue("txtPathToPortableZip")
                                    };

                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                        break;
                                    }
                                    if (setting_general.GetValueInt("typeBrowser") != 0)
                                    {
                                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Lỗi mở trình duyệt!");
                                            break;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");

                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
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
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        if (!chrome.GetProcess())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không check được chrome!");
                                            break;
                                        }
                                        if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                        {
                                            fViewChrome.remote.AddPanelDevice(chrome.IndexChrome, chrome.Size.X, chrome.Size.Y - 10);
                                            fViewChrome.remote.AddChromeIntoPanel(chrome.process.MainWindowHandle, chrome.IndexChrome, chrome.Size.X + 17, chrome.Size.Y, -10, 0);
                                        }

                                        //if (setting_general.GetValueBool("ckbThunhoPage"))
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}
                                        //else
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}

                                        //type login
                                        if (setting_MoTrinhDuyet.GetValueInt("typeOpenBrowser") == 1)
                                        {
                                            string text8 = "";
                                            text8 = (setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") != 0) ? "https://www.facebook.com/" : "https://m.facebook.com/";
                                            if (text7.Trim() != "")
                                            {
                                                switch (CommonChrome.CheckLiveCookie(chrome, text8))
                                                {
                                                    case 1:
                                                        flag = true;
                                                        break;
                                                    case -2:
                                                        chrome.Status = StatusChromeAccount.ChromeClosed;
                                                        goto quit;
                                                    case -3:
                                                        chrome.Status = StatusChromeAccount.NoInternet;
                                                        goto quit;
                                                    case 2:
                                                        chrome.Status = StatusChromeAccount.Checkpoint;
                                                        SetInfoAccount(cellAccount, indexRow, "Checkpoint");
                                                        //break;
                                                        goto quit;
                                                }
                                            }
                                            if (!flag)
                                            {
                                                int valueInt = setting_MoTrinhDuyet.GetValueInt("typeLogin");
                                                switch (valueInt)
                                                {
                                                    case 0:
                                                        SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng uid|pass..."));
                                                        break;
                                                    case 1:
                                                        SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng email|pass..."));
                                                        break;
                                                    case 2:
                                                        SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng cookie..."));
                                                        break;
                                                }
                                                string text9 = LoginFacebook(chrome, valueInt, text8, text5, cellAccount2, cellAccount5, cellAccount4, cellAccount6, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                                switch (text9)
                                                {
                                                    case "-3":
                                                        chrome.Status = StatusChromeAccount.NoInternet;
                                                        goto quit;
                                                    case "-2":
                                                        chrome.Status = StatusChromeAccount.ChromeClosed;
                                                        goto errorBreak;
                                                    case "0":
                                                        SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                        goto errorBreak;
                                                    case "1":
                                                        flag = true;
                                                        goto errorBreak;
                                                    case "2":
                                                        chrome.Status = StatusChromeAccount.Checkpoint;
                                                        SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                        goto errorBreak;
                                                    case "3":
                                                        SetStatusAccount(indexRow, text2 + ("Không có 2fa!"));
                                                        goto errorBreak;
                                                    case "4":
                                                        SetStatusAccount(indexRow, text2 + ("Tài khoản không đúng!"));
                                                        goto errorBreak;
                                                    case "5":
                                                        SetStatusAccount(indexRow, text2 + ("Mật khẩu không đúng!"));
                                                        SetInfoAccount(cellAccount, indexRow, "Changed pass");
                                                        goto errorBreak;
                                                    case "6":
                                                        SetStatusAccount(indexRow, text2 + ("Mã 2fa không đúng!"));
                                                        goto errorBreak;
                                                    default:
                                                        {
                                                            SetStatusAccount(indexRow, text2 + text9);
                                                            goto errorBreak;
                                                        }
                                                    errorBreak:
                                                        if (flag)
                                                        {
                                                            break;
                                                        }
                                                        SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                        SetRowColor(indexRow, 1);
                                                        ScreenCaptureError(chrome, text5, 1);
                                                        goto quit;
                                                }
                                            }

                                            if (setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") == 1 && !chrome.GetURL().StartsWith(text8))
                                            {
                                                chrome.GotoURL(text8);
                                            }
                                            SetStatusAccount(indexRow, text2 + "Đăng nhập thành công!");

                                            SetInfoAccount(cellAccount, indexRow, "Live");
                                            SetRowColor(indexRow, 2);
                                            if (setting_MoTrinhDuyet.GetValueBool("isGetCookie"))
                                            {
                                                text6 = ConvertCookie(chrome.GetCookieFromChrome());
                                                CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                                CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", text6);
                                                SetCellAccount(indexRow, "cCookies", text6);
                                            }

                                            if (setting_MoTrinhDuyet.GetValueBool("isGetToken"))
                                            {
                                                SetStatusAccount(indexRow, text2 + ("Bắt đầu lấy Token!"));

                                                int type = setting_InteractGeneral.GetValueInt("typeToken");
                                                if (type == 1)
                                                {
                                                    text6 = ConvertCookie(chrome.GetCookieFromChrome());
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", text6);
                                                    SetCellAccount(indexRow, "cCookies", text6);
                                                    SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                    // get token 
                                                    Helpers.Common.DelayTime(1);
                                                    chrome.GotoURL("https://adsmanager.facebook.com/adsmanager/");
                                                    chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Cua Toolkit<br>Đang Thao Tác...</b>'; document.querySelector('body').style = 'font-size:18px; color:red;text-align: center; background-color:#fff'");
                                                    chrome.DelayTime(2.0);
                                                    // get token 
                                                    SetStatusAccount(indexRow, text2 + "Get Token...");
                                                    object body = chrome.ExecuteScript(@"function getTokenEAAB() { let tokens = window.__accessToken; if (tokens) { return tokens; } else { return '';  } } return getTokenEAAB();");
                                                    string token = body.ToString();
                                                    //save
                                                    if (token != "")
                                                    {
                                                        CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                        SetCellAccount(indexRow, "cToken", token);
                                                        SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                        DelayThaoTacNho();
                                                    }
                                                }
                                                if (type == 2)
                                                {
                                                    text6 = ConvertCookie(chrome.GetCookieFromChrome());
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", text6);
                                                    SetCellAccount(indexRow, "cCookies", text6);
                                                    SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                    Helpers.Common.DelayTime(1);
                                                    SetStatusAccount(indexRow, text2 + ("Bắt đầu lấy Token!"));
                                                    chrome.GotoURL("https://business.facebook.com/content_management");
                                                    chrome.DelayTime(1.0);
                                                    string pageSource = chrome.GetPageSource();
                                                    chrome.DelayTime(1.0);
                                                    if (chrome.CheckTextInChrome("Two-factor authentication required", "Yêu cầu xác thực 2 yếu tố") && cellAccount4 != "")
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Send code 2FA!");
                                                        //get 2fa
                                                        string cleanedAccount4 = cellAccount4.Replace(" ", "").Replace("\n", "");
                                                        string totp = Helpers.Common.GetTotp(cleanedAccount4);
                                                        bool flag10 = !string.IsNullOrEmpty(totp);
                                                        if (flag10)
                                                        {
                                                            chrome.SendKeys(3, "//div/input", totp, 0.1, true, 0.1);
                                                            chrome.DelayTime(1.0);
                                                            chrome.SendEnter(3, "//div/input");
                                                            if (chrome.CheckTextInChrome("The login code you entered doesn't match the one sent to your phone. Please check the number and try again.", "Mã đăng nhập bạn nhập không khớp với mã đã gửi đến điện thoại của bạn. Vui lòng kiểm tra số này và thử lại"))
                                                            {
                                                                string totp2 = Helpers.Common.GetTotp(cleanedAccount4);
                                                                bool flag11 = !string.IsNullOrEmpty(totp2);
                                                                if (flag11)
                                                                {
                                                                    chrome.SendKeys(3, "//div/input", totp2, 0.1, true, 0.1);
                                                                    chrome.DelayTime(1.0);
                                                                    chrome.SendEnter(3, "//div/input");
                                                                    if (chrome.CheckTextInChrome("Đã xảy ra lỗi. Vui lòng thử lại sau.", "Something went wrong. Please try again later"))
                                                                    {
                                                                        SetStatusAccount(indexRow, text2 + "Bị chặn get token.");
                                                                    }
                                                                }
                                                            }

                                                            chrome.DelayTime(1.0);
                                                            if (chrome.CheckTextInChrome("Page posts", "Bài viết trên trang"))
                                                            {
                                                                SetStatusAccount(indexRow, text2 + "Get Token...");
                                                                string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                                if (token != "")
                                                                {
                                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                                    SetCellAccount(indexRow, "cToken", token);
                                                                    SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                                    Helpers.Common.DelayTime(1);
                                                                    DelayThaoTacNho();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // get token 
                                                        SetStatusAccount(indexRow, text2 + "Get Token...");
                                                        string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                        //save
                                                        if (token != "")
                                                        {
                                                            CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                            SetCellAccount(indexRow, "cToken", token);
                                                            SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                            DelayThaoTacNho();
                                                        }
                                                    }
                                                }
                                                if (type == 3)
                                                {
                                                    int wChromeOld = chrome.chrome.Manage().Window.Size.Width;
                                                    int hChromeOld = chrome.chrome.Manage().Window.Size.Height;
                                                    chrome.chrome.Manage().Window.Size = new Size(500, 700);

                                                    text6 = ConvertCookie(chrome.GetCookieFromChrome());
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", text6);
                                                    SetCellAccount(indexRow, "cCookies", text6);
                                                    SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                    // get token 
                                                    Helpers.Common.DelayTime(1);
                                                    chrome.GotoURL("https://www.facebook.com/dialog/oauth?scope=user_about_me,pages_read_engagement,user_actions.books,user_actions.fitness,user_actions.music,user_actions.news,user_actions.video,user_activities,user_birthday,user_education_history,user_events,user_friends,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_managed_groups,user_photos,user_posts,user_relationship_details,user_relationships,user_religion_politics,user_status,user_tagged_places,user_videos,user_website,user_work_history,email,manage_notifications,manage_pages,publish_actions,publish_pages,read_friendlists,read_insights,read_page_mailboxes,read_stream,rsvp_event,read_mailbox&response_type=token&client_id=124024574287414&redirect_uri=fb124024574287414://authorize/&sso_key=com&display=&fbclid=IwAR1KPwp2DVh2Cu7KdeANz-dRC_wYNjjHk5nR5F-BzGGj7-gTnKimAmeg08k");
                                                    chrome.DelayTime(2.0);
                                                    chrome.ExecuteScript("document.querySelector('[name=\"__CONFIRM__\"]').click()");
                                                    chrome.DelayTime(2.0);
                                                    // get token 
                                                    SetStatusAccount(indexRow, text2 + "Get Token...");

                                                    chrome.GotoURL("view-source:https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https://www.instagram.com/accounts/signup/&&scope=email&response_type=token");
                                                    string token = Regex.Match(chrome.GetURL(), "#access_token=(.*?)&").Groups[1].Value;
                                                    //save
                                                    chrome.chrome.Manage().Window.Size = new Size(wChromeOld, hChromeOld);
                                                    if (token != "")
                                                    {
                                                        CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                        SetCellAccount(indexRow, "cToken", token);
                                                        SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                    }
                                                    else
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Get Token Thất bại.");
                                                    }
                                                }
                                            }
                                            flag2 = !CheckIsUidFacebook(text5);
                                            if (flag2)
                                            {
                                                text4 = text5;
                                                text5 = Regex.Match(chrome.GetCookieFromChrome() + ";", "c_user=(.*?);").Groups[1].Value;
                                                CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                                SetCellAccount(indexRow, "cUid", text5);
                                            }
                                            chrome.GotoURL("https://facebook.com");
                                            SetStatusAccount(indexRow, text2 + "Hoàn tất.");
                                        }
                                        else
                                        {
                                            SetStatusAccount(indexRow, text2 + "Mở profile thành công!.");
                                            flag = true;
                                        }

                                        if (setting_MoTrinhDuyet.GetValueBool("ckbAutoOpenLink"))
                                        {
                                            chrome.GotoURL(setting_MoTrinhDuyet.GetValue("txtLink"));
                                        }
                                        if (setting_MoTrinhDuyet.GetValueBool("ckbLoginHotmail") && (cellAccount2.Contains("hotmail") || cellAccount2.Contains("outlook")))
                                        {
                                            chrome.OpenNewTab("https://login.live.com/login.srf");
                                            chrome.DelayTime(1.0);
                                            if (chrome.CheckExistElement("[name=\"loginfmt\"]", 10.0) == 1)
                                            {
                                                chrome.SendKeys(2, "loginfmt", cellAccount2);
                                                chrome.DelayTime(0.1);
                                                chrome.Click(1, "idSIButton9");
                                                if (chrome.CheckExistElement("[name=\"passwd\"]", 10.0) == 1)
                                                {
                                                    chrome.DelayTime(2.0);
                                                    chrome.SendKeys(2, "passwd", cellAccount3);
                                                    chrome.DelayTime(2.0);
                                                    chrome.Click(1, "idSIButton9", 0, 0, "", 0, 10);
                                                    int num7 = 0;
                                                    while (true)
                                                    {
                                                        if (num7 < 10)
                                                        {
                                                            switch (chrome.CheckExistElements(0.0, "[name=\"DontShowAgain\"]", "#O365_MainLink_NavMenu", "#reload-button"))
                                                            {
                                                                case 1:
                                                                    chrome.Click(2, "DontShowAgain");
                                                                    chrome.DelayTime(0.1);
                                                                    chrome.Click(1, "idSIButton9");
                                                                    break;
                                                                case 3:
                                                                    chrome.Click(4, "#reload-button");
                                                                    goto IL_3490;
                                                                default:
                                                                    switch (chrome.CheckExistElements(0.0, "#idA_IL_ForgotPassword0", "[name=\"passwd\"]"))
                                                                    {
                                                                        case 1:
                                                                            SetStatusAccount(indexRow, ("Mail sai pass!"));
                                                                            goto quit;
                                                                        case 2:
                                                                            SetStatusAccount(indexRow, ("Không có pass mail!"));
                                                                            goto quit;
                                                                        default:
                                                                            if (chrome.GetURL().Contains("https://account.live.com/Abuse"))
                                                                            {
                                                                                SetStatusAccount(indexRow, ("Mail đã bị khóa!"));
                                                                                goto end;
                                                                            }
                                                                            break;
                                                                    }
                                                                    goto IL_3490;
                                                                case 2:
                                                                    break;
                                                            }
                                                        }
                                                        if (!chrome.GetURL().StartsWith("https://outlook.live.com"))
                                                        {
                                                            chrome.GotoURL("https://outlook.live.com/mail/0/");
                                                        }
                                                        if (chrome.CheckExistElement("#iShowSkip") == 1)
                                                        {
                                                            chrome.Click(4, "#iShowSkip");
                                                        }
                                                        break;
                                                    IL_3490:
                                                        chrome.DelayTime(1.0);
                                                        num7++;
                                                    }
                                                }
                                            }
                                        }

                                        break;

                                    }
                                quit:;
                                }
                                catch (Exception ex)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
                                    Helpers.Common.ExportError(chrome, ex);
                                }
                                break;
                            }
                        end:
                            break;
                    }
                    break;
                }
            }
            if (chrome != null)
            {
                StatusChromeAccount status = chrome.Status;
                StatusChromeAccount statusChromeAccount = status;
                if (statusChromeAccount == StatusChromeAccount.ChromeClosed || statusChromeAccount == StatusChromeAccount.Checkpoint || statusChromeAccount == StatusChromeAccount.NoInternet)
                {
                    SetRowColor(indexRow, 1);
                    SetStatusAccount(indexRow, text2 + GetContentStatusChrome.GetContent(chrome.Status));
                }
            }
            if (!flag)
            {
                try
                {
                    chrome.Close();
                }
                catch
                {
                }
            }
            if (flag2 && Directory.Exists(setting_general.GetValue("txbPathProfile") + "\\" + text4))
            {
                string text10 = setting_general.GetValue("txbPathProfile") + "\\" + text4;
                string pathTo = setting_general.GetValue("txbPathProfile") + "\\" + text5;
                if (!Helpers.Common.MoveFolder(text10, pathTo) && Helpers.Common.CopyFolder(text10, pathTo))
                {
                    Helpers.Common.DeleteFolder(text10);
                }
            }
        }
        private int CheckFacebookLogout(Chrome chrome, string uid, string pass, string fa2, bool isSendRequest = false)
        {
            int result = 0;
            CommonChrome.CheckStatusAccount(chrome, isSendRequest);
            switch (chrome.Status)
            {
                case StatusChromeAccount.ChromeClosed:
                    result = -2;
                    break;
                case StatusChromeAccount.LoginWithUserPass:
                case StatusChromeAccount.LoginWithSelectAccount:
                    {
                        string text = CommonChrome.LoginFacebookUsingUidPassNew(chrome, uid, pass, fa2, "https://m.facebook.com/", 2);
                        result = ((text == "1") ? 1 : 2);
                        break;
                    }
                case StatusChromeAccount.Checkpoint:
                    result = -1;
                    break;
                case StatusChromeAccount.NoInternet:
                    result = -3;
                    break;
            }
            return result;
        }
        private bool CheckIsUidFacebook(string uid)
        {
            return Helpers.Common.IsNumber(uid) && !uid.StartsWith("0");
        }
        private void OpenFormViewChrome()
        {
            bool flag = false;
            FormCollection openForms = Application.OpenForms;
            foreach (Form item in openForms)
            {
                if (item.Name == "fInterfaceChrome")
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                new fViewChrome().Show();
            }
        }
        public void SetInfoAccount(string id, int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cInfo", value);
            SetRowColor(indexRow);
            CommonSQL.UpdateFieldToAccount(id, "info", value);
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

        public string GetInfoAccount(int indexRow)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, "cInfo");
        }

        private void càiĐặtTươngTácToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCauHinhTuongTac());
            if (namee != 1)
            {
                return;
            }
        }

        private void tạoProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui chọn tài khoản cần tạo profile!", 3);
            }
            else
            {
                UpdateInfoAccount(0);
            }
        }

        private void checkProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountTotal.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn check!", 3);
                return;
            }

            int iThread = 0;
            int maxThread = 10;
            string profilePath = ConfigHelper.GetPathProfile();
            new Thread((ThreadStart)delegate
            {
                int num = 0;
                while (num < dtgvAcc.RowCount)
                {
                    if (Convert.ToBoolean(GetCellAccount(num, "cChose")))
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                SetStatusAccount(row, "Check profile...");
                                CheckProfile(row, profilePath);
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
            }).Start();
        }
        private void CheckProfile(int row, string profilePath)
        {
            try
            {
                string cellAccount = GetCellAccount(row, "cId");
                string cellAccount2 = GetCellAccount(row, "cUid");
                profilePath = profilePath + "\\" + cellAccount2;
                if (Directory.Exists(profilePath))
                {
                    SetStatusAccount(row, "Đã có profile!");
                    SetCellAccount(row, "cProfile", "Yes");
                    CommonSQL.UpdateFieldToAccount(cellAccount, "profile", "Yes");
                }
                else
                {
                    SetStatusAccount(row, "Chưa tạo profile!");
                    SetCellAccount(row, "cProfile", "No");
                    CommonSQL.UpdateFieldToAccount(cellAccount, "profile", "No");
                }
            }
            catch
            {
            }
        }

        private void xoáProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountTotal.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn xoá profile!", 3);
                return;
            }
            if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có chắc muốn xóa Profile của {0} tài khoản?", CountChooseRowInDatagridview())) != DialogResult.Yes)
            {
                return;
            }
            LoadSetting();
            int iThread = 0;
            int maxThread = setting_general.GetValueInt("nudHideThread", 10);
            new Thread((ThreadStart)delegate
            {
                int num = 0;
                while (num < dtgvAcc.Rows.Count)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                    {
                        if (iThread < maxThread)
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
            }).Start();
        }

        private void dọnDẹpProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Helpers.Common.ShowForm(new fClearProfile());
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox(ex.ToString(), 3);
            }
        }

        private void xoáCacheProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountTotal.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn xoá cache!", 3);
                return;
            }
            if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có chắc muốn xóa Cache Profile của {0} tài khoản?", CountChooseRowInDatagridview())) != DialogResult.Yes)
            {
                return;
            }
            LoadSetting();
            int iThread = 0;
            int maxThread = setting_general.GetValueInt("nudHideThread", 10);
            new Thread((ThreadStart)delegate
            {
                int num = 0;
                while (num < dtgvAcc.Rows.Count)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                SetStatusAccount(row, "Đang xoá Cache Profile...");
                                DeleteCacheProfile(row);
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
            }).Start();
        }
        private void ShowTrangThai(string content)
        {
            lblTrangThai.Invoke((MethodInvoker)delegate
            {
                lblTrangThai.Text = content;
            });
        }
        private void DeleteCacheProfile(int row)
        {
            try
            {
                string text = dtgvAcc.Rows[row].Cells["cId"].Value.ToString();
                string text2 = dtgvAcc.Rows[row].Cells["cUid"].Value.ToString();
                if (text2.Trim() == "")
                {
                    SetStatusAccount(row, "Chưa tạo profile!");
                    return;
                }
                string text3 = setting_general.GetValue("txbPathProfile") + "\\" + text2;
                if (Directory.Exists(text3))
                {
                    Directory.Delete(text3 + "\\Default\\Cache", recursive: true);
                    SetStatusAccount(row, ("Xóa Cache Profile thành công!"));
                }
                else
                {
                    SetStatusAccount(row, "Chưa tạo profile!");
                }
            }
            catch
            {
                SetStatusAccount(row, ("Xóa Cache Profile thất bại!"));
            }
        }

        private void coppyProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblCountTotal.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản muốn coppy!"), 3);
                return;
            }
            CopyFolder(ConfigHelper.GetPathProfile());
        }
        private void CopyFolder(string sourceFolder)
        {
            Helpers.Common.ShowForm(new fSelectFolder());
            string pathFolder = fSelectFolder.pathFolder;
            if (pathFolder == "")
            {
                return;
            }
            List<string> list = new List<string>();
            for (int i = 0; i < dtgvAcc.Rows.Count; i++)
            {
                try
                {
                    if (Convert.ToBoolean(GetCellAccount(i, "cChose")))
                    {
                        string cellAccount = GetCellAccount(i, "cUid");
                        list.Add(sourceFolder + "\\" + cellAccount + "|" + pathFolder + "\\" + cellAccount);
                    }
                }
                catch
                {
                }
            }
            if (list.Count > 0)
            {
                Helpers.Common.ShowForm(new fShowProgressBar(list));
                MessageBoxHelper.ShowMessageBox("Đã coppy dữ liệu xong!");
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
        private void CloseFormViewChrome()
        {
            FormCollection openForms = Application.OpenForms;
            foreach (Form frm in openForms)
            {
                if (frm.Name == "fViewChrome")
                {
                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.Close();
                    });
                    break;
                }
            }
        }
        private void DelayThaoTacNho(int timeAdd = 0)
        {
            Helpers.Common.DelayTime(rd.Next(timeAdd + 1, timeAdd + 4));
        }
        private Dictionary<string, List<string>> GetDictionaryIntoHanhDong(string idKichBan, string tenTuongTac, string field = "txtUid")
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            try
            {
                List<string> idHanhDongByIdKichBanAndTenTuongTac = InteractSQL.GetIdHanhDongByIdKichBanAndTenTuongTac(idKichBan, tenTuongTac);
                if (idHanhDongByIdKichBanAndTenTuongTac.Count > 0)
                {
                    for (int i = 0; i < idHanhDongByIdKichBanAndTenTuongTac.Count; i++)
                    {
                        string text = idHanhDongByIdKichBanAndTenTuongTac[i];
                        JSON_Settings jSON_Settings = new JSON_Settings(InteractSQL.GetCauHinhFromHanhDong(text), isJsonString: true);
                        List<string> list = new List<string>();
                        list = ((!(field == "txtUid")) ? jSON_Settings.GetValueList(field, jSON_Settings.GetValueInt("typeNganCach")) : jSON_Settings.GetValueList(field));
                        dictionary.Add(text, list);
                    }
                }
            }
            catch
            {
            }
            return dictionary;
        }
        private void SaveDictionaryIntoHanhDong(Dictionary<string, List<string>> dic, string field = "txtUid")
        {
            if (dic.Count <= 0)
            {
                return;
            }
            foreach (KeyValuePair<string, List<string>> item in dic)
            {
                string key = item.Key;
                List<string> value = item.Value;
                JSON_Settings jSON_Settings = new JSON_Settings(InteractSQL.GetCauHinhFromHanhDong(key), isJsonString: true);
                jSON_Settings.Update(field, value);
                InteractSQL.UpdateHanhDong(key, "", jSON_Settings.GetFullString());
            }
        }

        private void SaveDictionaryIntoHanhDong(Dictionary<string, List<string>> dic, bool isCheckDeleteComment, string field = "txtUid")
        {
            if (dic.Count <= 0)
            {
                return;
            }
            foreach (KeyValuePair<string, List<string>> item in dic)
            {
                string key = item.Key;
                List<string> value = item.Value;
                JSON_Settings jSON_Settings = new JSON_Settings(InteractSQL.GetCauHinhFromHanhDong(key), isJsonString: true);
                if (isCheckDeleteComment && jSON_Settings.GetValueBool("ckbAutoDeleteComment"))
                {
                    jSON_Settings.Update(field, value);
                    InteractSQL.UpdateHanhDong(key, "", jSON_Settings.GetFullString());
                }
            }
        }

        private void btnStopThaoTac_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                btnStopThaoTac.Enabled = false;
                //    btnStopThaoTac.Text = ("Đang Dừng...");
            }
            catch
            {
            }
        }

        private void btnStartThaotac_Click(object sender, EventArgs e)
        {

        }
        public string GetStatusAccount(int indexRow)
        {
            string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, "cStatus");
            if (statusDataGridView.StartsWith("("))
            {
                return statusDataGridView.Substring(statusDataGridView.IndexOf(')') + 1).Trim();
            }
            return statusDataGridView;
        }
        public string GetStatusPage(int indexRow)
        {
            string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dtgvPage, indexRow, "cStatusP");
            if (statusDataGridView.StartsWith("("))
            {
                return statusDataGridView.Substring(statusDataGridView.IndexOf(')') + 1).Trim();
            }
            return statusDataGridView;
        }
        public List<string> CloneList(List<string> lstFrom)
        {
            List<string> list = new List<string>();
            try
            {
                for (int i = 0; i < lstFrom.Count; i++)
                {
                    list.Add(lstFrom[i]);
                }
            }
            catch
            {
            }
            return list;
        }

        private void ExcuteOneThread(int indexRow, int indexPos, string idKichBan, string profilePath)
        {
            int num = 0;
            string text = "";
            Chrome chrome = null;
            int num2 = 0;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            MinProxy minProxy = null;
            TinsoftProxy tinsoftProxy = null;
            bool flag = false;
            string text4 = "";
            int checkPostSuccess = 0;
            int num3 = 0;
            bool flag2 = false;
            string cUid = GetCellAccount(indexRow, "cUid");
            string cId = GetCellAccount(indexRow, "cId");
            string cEmail = GetCellAccount(indexRow, "cEmail");
            string cFa2 = GetCellAccount(indexRow, "cFa2");
            string cPassword = GetCellAccount(indexRow, "cPassword");
            string cCookie = GetCellAccount(indexRow, "cCookies");
            string cToken = GetCellAccount(indexRow, "cToken");
            string cUseragent = GetCellAccount(indexRow, "cUseragent");
            setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
            if (cUid == "")
            {
                cUid = Regex.Match(cCookie, "c_user=(.*?);").Groups[1].Value;
            }

            if (setting_InteractGeneral.GetValueBool("ckbCheckLiveUid", valueDefault: true) && CheckIsUidFacebook(cUid) && CommonRequest.CheckLiveWall(cUid).StartsWith("0|"))
            {
                SetStatusAccount(indexRow, "Tài khoản Die!");
                SetInfoAccount(cId, indexRow, "Die");
                num3 = 1;
            }
            else if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                num3 = 1;
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag5 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                bool flag6 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag6 = false;
                                    }
                                }
                                if (!flag6)
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        foreach (MinProxy minp in listMinproxy)
                                        {
                                            if (minProxy == null || minp.daSuDung < minProxy.daSuDung)
                                            {
                                                minProxy = minp;
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag5 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                bool flag6 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag6 = false;
                                    }
                                }
                                if (!flag6)
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                num = Environment.TickCount;
                                try
                                {
                                    SetStatusAccount(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num7 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
                                            if (num7 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num7 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num7 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                                        num3 = 1;
                                                        goto endThread;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayChrome++;
                                        }
                                    }
                                    SetStatusAccount(indexRow, text2 + "Đang mở trình duyệt...");
                                    string text8 = "data:,";
                                    Point position;
                                    Point size;
                                    if (setting_general.GetValueBool("ckbAddChromeIntoForm"))
                                    {
                                        if (text8 == "")
                                        {
                                            position = new Point(-1000, 0);
                                            size = new Point(setting_general.GetValueInt("nudWidthChrome") + 16, setting_general.GetValueInt("nudHeighChrome") + 132);
                                        }
                                        else
                                        {
                                            position = new Point(-1000, 0);
                                            size = new Point(setting_general.GetValueInt("nudWidthChrome") + 33, setting_general.GetValueInt("nudHeighChrome") + 39);
                                        }
                                    }
                                    else
                                    {
                                        position = Helpers.Common.GetPointFromIndexPosition(indexPos, setting_general.GetValueInt("cbbColumnChrome", 3), setting_general.GetValueInt("cbbRowChrome", 2));
                                        size = Helpers.Common.GetSizeChrome(setting_general.GetValueInt("cbbColumnChrome", 3), setting_general.GetValueInt("cbbRowChrome", 2));
                                    }

                                    if (cUseragent == "")
                                    {
                                        cUseragent = Base.useragentDefault;
                                    }
                                    string pathProfileChrome = "";
                                    if (cUid != "")
                                    {
                                        pathProfileChrome = profilePath + "\\" + cUid;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(pathProfileChrome))
                                        {
                                            pathProfileChrome = "";
                                        }
                                    }
                                    Chrome chrome2 = new Chrome();
                                    chrome2.IndexChrome = indexRow;
                                    chrome2.DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract"));
                                    chrome2.UserAgent = cUseragent;
                                    chrome2.ProfilePath = pathProfileChrome;
                                    chrome2.Size = size;
                                    chrome2.Position = position;
                                    chrome2.TimeWaitForSearchingElement = 3;
                                    chrome2.TimeWaitForLoadingPage = 120;
                                    chrome2.Proxy = text;
                                    chrome2.TypeProxy = typeProxy;
                                    chrome2.DisableSound = true;
                                    chrome2.App = text8;
                                    chrome2.IsUsePortable = setting_general.GetValueBool("ckbUsePortable");
                                    chrome2.PathToPortableZip = setting_general.GetValue("txtPathToPortableZip");
                                    chrome = chrome2;
                                    if (setting_general.GetValue("sizeChrome").Contains("x"))
                                    {
                                        chrome.IsUseEmulator = true;
                                        string text10 = setting_general.GetValue("sizeChrome").Substring(0, setting_general.GetValue("sizeChrome").LastIndexOf('x'));
                                        int pixelRatio = Convert.ToInt32(setting_general.GetValue("sizeChrome").Split('x')[2]);
                                        chrome.Size_Emulator = new Point(Convert.ToInt32(text10.Split('x')[0]), Convert.ToInt32(text10.Split('x')[1]));
                                        if (cUseragent == "")
                                        {
                                            chrome.UserAgent = Base.useragentDefault;
                                        }
                                        chrome.PixelRatio = pixelRatio;
                                    }
                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                        num3 = 1;
                                        break;
                                    }
                                    if (setting_general.GetValueInt("typeBrowser") != 0)
                                    {
                                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                    }
                                    int num8 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Lỗi mở trình duyệt!");
                                            num3 = 1;
                                            break;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");
                                        chrome.DelayTime(2.0);
                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
                                            chrome.DelayTime(1.0);
                                            string pageSource = chrome.GetPageSource();
                                            if (!pageSource.Contains("ip"))
                                            {
                                                chrome.Close();
                                                num8++;
                                                if (num8 < 3)
                                                {
                                                    continue;
                                                }
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                num3 = 1;
                                                break;
                                            }
                                        }
                                        if (!chrome.GetProcess())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không check được chrome!");
                                            num3 = 1;
                                            break;
                                        }
                                        if (setting_general.GetValueBool("ckbAddChromeIntoForm"))
                                        {
                                            if (text8 != "")
                                            {
                                                fViewChrome.remote.AddPanelDevice(chrome.IndexChrome, chrome.Size.X - 33, chrome.Size.Y - 39);
                                                fViewChrome.remote.AddChromeIntoPanel(chrome.process.MainWindowHandle, chrome.IndexChrome, chrome.Size.X, chrome.Size.Y);
                                            }
                                            else
                                            {
                                                fViewChrome.remote.AddPanelDevice(chrome.IndexChrome, chrome.Size.X - 16, chrome.Size.Y - 132);
                                                fViewChrome.remote.AddChromeIntoPanel(chrome.process.MainWindowHandle, chrome.IndexChrome, chrome.Size.X + 17, chrome.Size.Y, -10, -125);
                                            }
                                        }

                                        //if (setting_general.GetValueBool("ckbThunhoPage"))
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}
                                        //else
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}

                                        SetStatusAccount(indexRow, text2 + "Đang đăng nhập...");
                                        bool flag14 = false;
                                        string text11 = "https://www.facebook.com/";
                                        if (setting_InteractGeneral.GetValueInt("typeBrowserLogin") == 0)
                                        {
                                            text11 = "https://m.facebook.com/";
                                        }
                                        if (pathProfileChrome.Trim() != "")
                                        {
                                            if (chrome.CheckExistElement("[data-cookiebanner=\"accept_button\"]") == 1)
                                            {
                                                chrome.ExecuteScript("document.querySelector('[data-cookiebanner=\"accept_button\"]').click()");
                                            }
                                            if (chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway") && chrome.CheckExistElement("[data-nt=\"NT:IMAGE\"]", 15.0) == 1)
                                            {
                                                chrome.ExecuteScript("document.querySelector('[data-nt=\"NT:IMAGE\"]').click()");
                                                chrome.DelayTime(2.0);
                                            }
                                            num2 = CommonChrome.CheckLiveCookie(chrome, text11);
                                            switch (num2)
                                            {
                                                case 1:
                                                    flag14 = true;
                                                    break;
                                                case -2:
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto quit;
                                                case -3:
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case 2:
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cId, indexRow, ("Checkpoint"));
                                                    if (setting_InteractGeneral.GetValueBool("ckbCheckCp"))
                                                    {
                                                        string text12 = "";
                                                        chrome.GotoURL("https://mbasic.facebook.com/");
                                                        switch (chrome.CheckExistElements(5.0, "[name=\"email\"]", "#checkpoint_title"))
                                                        {
                                                            case 1:
                                                                text12 = "723";
                                                                break;
                                                            case 2:
                                                                text12 = "cpxp";
                                                                break;
                                                        }
                                                        string uRL = chrome.GetURL();
                                                        if (uRL.Contains("1501092823525282"))
                                                        {
                                                            int num9 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('.y.bb.bc').length").ToString());
                                                            string value = Regex.Matches(chrome.ExecuteScript("return document.querySelectorAll('.y.bb.bc')[" + (num9 - 1) + "].innerText").ToString(), "\\d+")[1].Value;
                                                            text12 = "282-" + value;
                                                        }
                                                        else if (uRL.Contains("828281030927956"))
                                                        {
                                                            text12 = ((chrome.CheckExistElement("[href=\"/help/203305893040179\"]") != 1) ? "956-Bắt đầu" : "956-Tìm hiểu thêm");
                                                        }
                                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cGhiChu", text12);
                                                        CommonSQL.UpdateFieldToAccount(cId, "ghiChu", text12);
                                                    }
                                                    num3 = 1;
                                                    goto quit;
                                            }
                                        }
                                        if (!flag14)
                                        {
                                            int valueInt = setting_InteractGeneral.GetValueInt("typeLogin");
                                            switch (valueInt)
                                            {
                                                case 0:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng uid|pass...");
                                                    break;
                                                case 1:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng email|pass...");
                                                    break;
                                                case 2:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng cookie...");
                                                    break;
                                            }
                                            string text13 = LoginFacebook(chrome, valueInt, text11, cUid, cEmail, cPassword, cFa2, cCookie, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                            switch (text13)
                                            {
                                                case "-3":
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case "-2":
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto errorLogin;
                                                case "0":
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    goto errorLogin;
                                                case "1":
                                                    flag14 = true;
                                                    goto errorLogin;
                                                case "2":
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cId, indexRow, "Checkpoint");
                                                    goto errorLogin;
                                                case "3":
                                                    SetStatusAccount(indexRow, text2 + "Không có 2fa!");
                                                    goto errorLogin;
                                                case "4":
                                                    SetStatusAccount(indexRow, text2 + "Tài khoản không đúng!");
                                                    goto errorLogin;
                                                case "5":
                                                    SetStatusAccount(indexRow, text2 + "Mật khẩu không đúng!");
                                                    SetInfoAccount(cId, indexRow, "Changed pass");
                                                    goto errorLogin;
                                                case "6":
                                                    SetStatusAccount(indexRow, text2 + "Mã 2fa không đúng!");
                                                    goto errorLogin;
                                                default:
                                                    {
                                                        SetStatusAccount(indexRow, text2 + text13);
                                                        goto errorLogin;
                                                    }
                                                errorLogin:
                                                    if (flag14)
                                                    {
                                                        break;
                                                    }
                                                    SetRowColor(indexRow, 1);
                                                    ScreenCaptureError(chrome, cUid, 1);
                                                    if (setting_InteractGeneral.GetValueBool("ckbCheckCp"))
                                                    {
                                                        string text14 = "";
                                                        chrome.GotoURL("https://mbasic.facebook.com/");
                                                        switch (chrome.CheckExistElements(5.0, "[name=\"email\"]", "#checkpoint_title"))
                                                        {
                                                            case 1:
                                                                text14 = "723";
                                                                break;
                                                            case 2:
                                                                text14 = "cpxp";
                                                                break;
                                                        }
                                                        string uRL2 = chrome.GetURL();
                                                        if (uRL2.Contains("1501092823525282"))
                                                        {
                                                            int num10 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('.y.bb.bc').length").ToString());
                                                            string value2 = Regex.Matches(chrome.ExecuteScript("return document.querySelectorAll('.y.bb.bc')[" + (num10 - 1) + "].innerText").ToString(), "\\d+")[1].Value;
                                                            text14 = "282-" + value2;
                                                        }
                                                        else if (uRL2.Contains("828281030927956"))
                                                        {
                                                            text14 = ((chrome.CheckExistElement("[href=\"/help/203305893040179\"]") != 1) ? "956-Bắt đầu" : "956-Tìm hiểu thêm");
                                                        }
                                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cGhiChu", text14);
                                                        CommonSQL.UpdateFieldToAccount(cId, "ghiChu", text14);
                                                    }
                                                    num3 = 1;
                                                    goto quit;
                                            }
                                        }
                                        SetStatusAccount(indexRow, text2 + "Đăng nhập thành công!");
                                        SetRowColor(indexRow, 2);
                                        if (chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway") && chrome.CheckExistElement("[data-nt=\"NT:IMAGE\"]", 15.0) == 1)
                                        {
                                            chrome.ExecuteScript("document.querySelector('[data-nt=\"NT:IMAGE\"]').click()");
                                            chrome.DelayTime(2.0);
                                        }
                                        if (chrome.CheckExistElement("[data-cookiebanner=\"accept_button\"]") == 1)
                                        {
                                            chrome.ExecuteScript("document.querySelector('[data-cookiebanner=\"accept_button\"]').click()");
                                        }
                                        flag = !CheckIsUidFacebook(cUid);
                                        if (flag)
                                        {
                                            text4 = cUid;
                                            cUid = Regex.Match(chrome.GetCookieFromChrome() + ";", "c_user=(.*?);").Groups[1].Value;
                                            CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                            SetCellAccount(indexRow, "cUid", cUid);
                                        }
                                        if (setting_InteractGeneral.GetValueBool("ckbGetCookie"))
                                        {
                                            cCookie = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                            CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                                            SetCellAccount(indexRow, "cCookies", cCookie);
                                        }
                                        if (setting_InteractGeneral.GetValueBool("ckbGetToken"))
                                        {
                                            int type = setting_InteractGeneral.GetValueInt("typeToken");
                                            if (type == 1)
                                            {
                                                cCookie = ConvertCookie(chrome.GetCookieFromChrome());
                                                CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                                CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                                                SetCellAccount(indexRow, "cCookies", cCookie);
                                                SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                // get token 
                                                Helpers.Common.DelayTime(1);
                                                chrome.GotoURL("https://adsmanager.facebook.com/adsmanager/");
                                                chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Cua Toolkit<br>Đang Thao Tác...</b>'; document.querySelector('body').style = 'font-size:18px; color:red;text-align: center; background-color:#fff'");
                                                chrome.DelayTime(2.0);
                                                // get token 
                                                SetStatusAccount(indexRow, text2 + "Get Token...");
                                                object body = chrome.ExecuteScript(@"function getTokenEAAB() { let tokens = window.__accessToken; if (tokens) { return tokens; } else { return '';  } } return getTokenEAAB();");
                                                string token = body.ToString();
                                                //save
                                                if (token != "")
                                                {
                                                    CommonSQL.UpdateFieldToAccount(cId, "token", token);
                                                    SetCellAccount(indexRow, "cToken", token);
                                                    SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                    DelayThaoTacNho();
                                                }
                                            }
                                            if (type == 2)
                                            {
                                                cCookie = ConvertCookie(chrome.GetCookieFromChrome());
                                                CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                                CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                                                SetCellAccount(indexRow, "cCookies", cCookie);
                                                SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                Helpers.Common.DelayTime(1);
                                                SetStatusAccount(indexRow, text2 + ("Bắt đầu lấy Token!"));
                                                chrome.GotoURL("https://business.facebook.com/content_management");
                                                chrome.DelayTime(1.0);
                                                string pageSource = chrome.GetPageSource();
                                                chrome.DelayTime(1.0);
                                                if (chrome.CheckTextInChrome("Two-factor authentication required", "Yêu cầu xác thực 2 yếu tố") && cFa2 != "")
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Send code 2FA!");
                                                    //get 2fa
                                                    string cleanedAccount3 = cFa2.Replace(" ", "").Replace("\n", "");
                                                    string totp = Helpers.Common.GetTotp(cleanedAccount3);
                                                    bool flag10 = !string.IsNullOrEmpty(totp);
                                                    if (flag10)
                                                    {
                                                        chrome.SendKeys(3, "//div/input", totp, 0.1, true, 0.1);
                                                        chrome.DelayTime(1.0);
                                                        chrome.SendEnter(3, "//div/input");
                                                        if (chrome.CheckTextInChrome("The login code you entered doesn't match the one sent to your phone. Please check the number and try again.", "Mã đăng nhập bạn nhập không khớp với mã đã gửi đến điện thoại của bạn. Vui lòng kiểm tra số này và thử lại"))
                                                        {
                                                            string totp2 = Helpers.Common.GetTotp(cleanedAccount3);
                                                            bool flag11 = !string.IsNullOrEmpty(totp2);
                                                            if (flag11)
                                                            {
                                                                chrome.SendKeys(3, "//div/input", totp2, 0.1, true, 0.1);
                                                                chrome.DelayTime(1.0);
                                                                chrome.SendEnter(3, "//div/input");
                                                                if (chrome.CheckTextInChrome("Đã xảy ra lỗi. Vui lòng thử lại sau.", "Something went wrong. Please try again later"))
                                                                {
                                                                    SetStatusAccount(indexRow, text2 + "Bị chặn get token.");
                                                                }
                                                            }
                                                        }

                                                        chrome.DelayTime(1.0);
                                                        if (chrome.CheckTextInChrome("Page posts", "Bài viết trên trang"))
                                                        {
                                                            SetStatusAccount(indexRow, text2 + "Get Token...");
                                                            //object body = chrome.ExecuteScript(@"function getTokenEAAG() { const body = document.body.innerHTML; const match = body.match(/EAA(.*?)\""/); if (match && match[1]) {  const token = 'EAA' + match[1]; return token;  } else {    return '';  } } return getTokenEAAG();");
                                                            //string token = body.ToString();
                                                            string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                            if (token != "")
                                                            {
                                                                CommonSQL.UpdateFieldToAccount(cId, "token", token);
                                                                SetCellAccount(indexRow, "cToken", token);
                                                                SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                                Helpers.Common.DelayTime(1);
                                                                DelayThaoTacNho();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // get token 
                                                    SetStatusAccount(indexRow, text2 + "Get Token...");
                                                    //object body = chrome.ExecuteScript(@"function getTokenEAAG() { const body = document.body.innerHTML; const match = body.match(/EAA(.*?)\""/); if (match && match[1]) {  const token = 'EAA' + match[1]; return token;  } else {    return '';  } } return getTokenEAAG();");
                                                    //string token = body.ToString();
                                                    string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                    //save
                                                    if (token != "")
                                                    {
                                                        CommonSQL.UpdateFieldToAccount(cId, "token", token);
                                                        SetCellAccount(indexRow, "cToken", token);
                                                        SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                        DelayThaoTacNho();
                                                    }
                                                }
                                            }
                                            if (type == 3)
                                            {
                                                int wChromeOld = chrome.chrome.Manage().Window.Size.Width;
                                                int hChromeOld = chrome.chrome.Manage().Window.Size.Height;
                                                chrome.chrome.Manage().Window.Size = new Size(500, 700);

                                                cCookie = ConvertCookie(chrome.GetCookieFromChrome());
                                                CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                                CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                                                SetCellAccount(indexRow, "cCookies", cCookie);
                                                SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                                // get token 
                                                Helpers.Common.DelayTime(1);
                                                chrome.GotoURL("https://www.facebook.com/dialog/oauth?scope=user_about_me,pages_read_engagement,user_actions.books,user_actions.fitness,user_actions.music,user_actions.news,user_actions.video,user_activities,user_birthday,user_education_history,user_events,user_friends,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_managed_groups,user_photos,user_posts,user_relationship_details,user_relationships,user_religion_politics,user_status,user_tagged_places,user_videos,user_website,user_work_history,email,manage_notifications,manage_pages,publish_actions,publish_pages,read_friendlists,read_insights,read_page_mailboxes,read_stream,rsvp_event,read_mailbox&response_type=token&client_id=124024574287414&redirect_uri=fb124024574287414://authorize/&sso_key=com&display=&fbclid=IwAR1KPwp2DVh2Cu7KdeANz-dRC_wYNjjHk5nR5F-BzGGj7-gTnKimAmeg08k");
                                                chrome.DelayTime(2.0);
                                                chrome.ExecuteScript("document.querySelector('[name=\"__CONFIRM__\"]').click()");
                                                chrome.DelayTime(2.0);
                                                // get token 
                                                SetStatusAccount(indexRow, text2 + "Get Token...");

                                                chrome.GotoURL("view-source:https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https://www.instagram.com/accounts/signup/&&scope=email&response_type=token");
                                                string token = Regex.Match(chrome.GetURL(), "#access_token=(.*?)&").Groups[1].Value;
                                                chrome.chrome.Manage().Window.Size = new Size(wChromeOld, hChromeOld);
                                                //save
                                                if (token != "")
                                                {
                                                    CommonSQL.UpdateFieldToAccount(cId, "token", token);
                                                    SetCellAccount(indexRow, "cToken", token);
                                                    SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                    DelayThaoTacNho();
                                                }
                                                else
                                                {
                                                    goto endThread;
                                                }
                                            }
                                        }
                                        SetStatusAccount(indexRow, text2 + "Chạy tương tác...");
                                        //tương tác nhanh
                                        if (setting_InteractGeneral.GetValueInt("typeInteract") == 0)
                                        {
                                            //chưa làm nè
                                        }
                                        else
                                        {
                                            DataTable dataTable = InteractSQL.GetAllHanhDongByKichBan(idKichBan);
                                            //random kịch bản
                                            if (setting_InteractGeneral.GetValueBool("ckbRandomHanhDong"))
                                            {
                                                dataTable = Helpers.Common.ShuffleDataTable(dataTable);
                                                dataTable = Helpers.Common.ShuffleDataTable(dataTable);
                                                dataTable = Helpers.Common.ShuffleDataTable(dataTable);
                                            }
                                            string text15 = "";
                                            string text16 = "";
                                            DataTable dataTable2 = new DataTable();
                                            string cauHinhFromKichBan = InteractSQL.GetCauHinhFromKichBan(idKichBan);
                                            JSON_Settings jSON_Settings = new JSON_Settings(cauHinhFromKichBan, isJsonString: true);
                                            int valueInt2 = jSON_Settings.GetValueInt("typeSoLuongHanhDong");
                                            int valueInt3 = jSON_Settings.GetValueInt("nudHanhDongFrom");
                                            int valueInt4 = jSON_Settings.GetValueInt("nudHanhDongTo");
                                            int num11 = dataTable.Rows.Count;
                                            if (valueInt2 == 1 && valueInt3 <= valueInt4)
                                            {
                                                num11 = Base.rd.Next(valueInt3, valueInt4 + 1);
                                                if (num11 > dataTable.Rows.Count)
                                                {
                                                    num11 = dataTable.Rows.Count;
                                                }
                                            }
                                            int num12 = 0;
                                            while (num12 < num11)
                                            {
                                                if (isStop)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                                    num3 = 1;
                                                    goto endThread;
                                                }
                                                try
                                                {
                                                    text16 = dataTable.Rows[num12]["TenHanhDong"].ToString();
                                                    text15 = dataTable.Rows[num12]["Id_HanhDong"].ToString();
                                                    SetStatusAccount(indexRow, text2 + "Đang" + " " + text16 + "...");
                                                    dataTable2 = InteractSQL.GetHanhDongById(text15);
                                                    JSON_Settings jSON_Settings2 = new JSON_Settings(dataTable2.Rows[0]["CauHinh"].ToString(), isJsonString: true);
                                                    //danh sách chạy kịch bản
                                                    switch (dataTable2.Rows[0]["TenTuongTac"].ToString())
                                                    {
                                                        case "HDDangStatus":
                                                            try
                                                            {
                                                                num2 = HDDangStatus(indexRow, text2, chrome, cToken, jSON_Settings2.GetValueInt("nudSoLuongFrom", 1), jSON_Settings2.GetValueInt("nudSoLuongTo", 1), jSON_Settings2.GetValueInt("nudKhoangCachFrom"), jSON_Settings2.GetValueInt("nudKhoangCachTo"), jSON_Settings2.GetValueBool("ckbVanBan"), jSON_Settings2.GetValueBool("ckbUseBackground"), jSON_Settings2.GetValueBool("ckbAnh"), jSON_Settings2.GetValue("txtPathAnh"), jSON_Settings2.GetValueInt("nudSoLuongAnhFrom"), jSON_Settings2.GetValueInt("nudSoLuongAnhTo"), jSON_Settings2.GetValueBool("ckbVideo"), jSON_Settings2.GetValue("txtPathVideo"), jSON_Settings2.GetValueInt("nudSoLuongVideoFrom"), jSON_Settings2.GetValueInt("nudSoLuongVideoTo"), rd, jSON_Settings2.GetValueBool("ckbTag"), jSON_Settings2.GetValueInt("typeUidTag"), jSON_Settings2.GetValueList("txtUidTag"), jSON_Settings2.GetValueInt("nudUidFrom"), jSON_Settings2.GetValueInt("nudUidTo"), text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDDangStatus");
                                                            }
                                                            break;
                                                        case "SpamCommentPage":
                                                            try
                                                            {
                                                                num2 = SpamCommentPage(chrome, indexRow, text, jSON_Settings2.GetValueInt("nudCountUid", 5), jSON_Settings2.GetValueInt("nudPostUid", 3), jSON_Settings2.GetValueInt("nudKhoangCachFrom", 1), jSON_Settings2.GetValueInt("nudKhoangCachTo", 1), jSON_Settings2.GetValueInt("nudnumberPage", 1), jSON_Settings2.GetValueBool("ckbAnh"), jSON_Settings2.GetValue("txtPathAnh"), jSON_Settings2.GetValue("txtPathFileUid"), jSON_Settings2.GetValueInt("typeListUid"), jSON_Settings2.GetValueBool("ckbRemoveUid"), rd, text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "SpamCommentPage");
                                                            }
                                                            break;
                                                        case "HDRegPage":
                                                            try
                                                            {
                                                                num2 = HDRegPage(chrome, indexRow, text, jSON_Settings2.GetValueInt("nudKhoangCachFrom", 1), jSON_Settings2.GetValueInt("nudKhoangCachTo", 1), jSON_Settings2.GetValueInt("numberPageCreate", 1), jSON_Settings2.GetValueInt("typeListName"), jSON_Settings2.GetValue("txtPathFileName"), jSON_Settings2.GetValue("txtDescription"), jSON_Settings2.GetValue("txtPathAvatar"), jSON_Settings2.GetValue("txtPathCover"), jSON_Settings2.GetValue("txtWebsite"), jSON_Settings2.GetValue("txtPhoneNumber"), jSON_Settings2.GetValue("txtAddress"), jSON_Settings2.GetValue("txtZipcode"), jSON_Settings2.GetValue("txtEmail"), jSON_Settings2.GetValueBool("ckbRandomString"), jSON_Settings2.GetValueBool("ckbFirstPost"), jSON_Settings2.GetValue("txtContentPost"), jSON_Settings2.GetValueBool("ckbPostImg"), jSON_Settings2.GetValue("txtPathImgPost"), jSON_Settings2.GetValueInt("delayPostform"), jSON_Settings2.GetValueInt("delayPostto"), jSON_Settings2.GetValueBool("ckbInviteAdmin"), jSON_Settings2.GetValueList("txtAdminInvite"), jSON_Settings2.GetValueInt("numberAdInvite"), jSON_Settings2.GetValue("txtChuoibatdau"), jSON_Settings2.GetValueBool("ckbDeleteNamePage"), rd, text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "SpamCommentPage");
                                                            }
                                                            break;
                                                        case "LuotNewFeed":
                                                            try
                                                            {
                                                                num2 = LuotNewFeed(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudTimeFrom"), jSON_Settings2.GetValueInt("nudTimeTo"), jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), jSON_Settings2.GetValueBool("ckbInteract"), jSON_Settings2.GetValueInt("nudCountLikeFrom", 1), jSON_Settings2.GetValueInt("nudCountLikeTo", 3), jSON_Settings2.GetValue("typeCamXuc"), jSON_Settings2.GetValueBool("ckbComment"), jSON_Settings2.GetValueInt("nudCountCommentFrom", 1), jSON_Settings2.GetValueInt("nudCountCommentTo", 3), jSON_Settings2.GetValueList("txtComment"), rd, text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "LuotNewFeed");
                                                            }
                                                            break;
                                                        case "ChangeInfo":
                                                            try
                                                            {
                                                                num2 = ChangeInfo(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), jSON_Settings2.GetValueBool("ckbChangePass"), jSON_Settings2.GetValueBool("ckbChangeMail"), jSON_Settings2.GetValue("txtPasswordNew"), jSON_Settings2.GetValue("txtKeyDVFB"), jSON_Settings2.GetValue("cbbTypeDv"), jSON_Settings2.GetValueBool("ckbGetCookie"), rd, text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "LuotNewFeed");
                                                            }
                                                            break;
                                                        case "HDKetBanGoiY":
                                                            try
                                                            {
                                                                num2 = HDKetBanGoiY(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudSoLuongFrom"), jSON_Settings2.GetValueInt("nudSoLuongTo"), jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), jSON_Settings2.GetValueBool("ckbChiKetBanTenCoDau"), jSON_Settings2.GetValueBool("ckbOnlyAddFriendWithMutualFriends"), jSON_Settings2.GetValueInt("nudTimesWarning", 3), rd, text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDKetBanGoiY");
                                                            }
                                                            break;
                                                        case "HDKetBanUid":
                                                            try
                                                            {
                                                                num2 = HDKetBanUid(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), rd, text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDKetBanUid");
                                                            }
                                                            break;
                                                        case "HDXacNhanKetBan":
                                                            try
                                                            {
                                                                num2 = HDXacNhanKetBan(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudSoLuongFrom"), jSON_Settings2.GetValueInt("nudSoLuongTo"), jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), rd, text16, jSON_Settings2.GetValueBool("ckbChiKetBanTenCoDau"), jSON_Settings2.GetValueBool("ckbOnlyAddFriendWithMutualFriends"));
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDXacNhanKetBan");
                                                            }
                                                            break;
                                                        case "HDTimKiemGoogle":
                                                            try
                                                            {
                                                                num2 = HDTimKiemGoogle(indexRow, text2, chrome, jSON_Settings2.GetValueList("txtTuKhoa"), jSON_Settings2.GetValueInt("nudCountTuKhoaFrom"), jSON_Settings2.GetValueInt("nudCountTuKhoaTo"), jSON_Settings2.GetValueInt("nudCountPageFrom"), jSON_Settings2.GetValueInt("nudCountPageTo"), jSON_Settings2.GetValueInt("nudCountLinkClickFrom"), jSON_Settings2.GetValueInt("nudCountLinkClickTo"), jSON_Settings2.GetValueInt("nudCountTimeScrollFrom"), jSON_Settings2.GetValueInt("nudCountTimeScrollTo"), text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDTimKiemGoogle");
                                                            }
                                                            break;
                                                        case "HDNghiGiaiLao":
                                                            try
                                                            {
                                                                num2 = HDNghiGiaiLao(indexRow, text2, jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDNghiGiaiLao");
                                                            }
                                                            break;
                                                        case "SeedingFb":
                                                            try
                                                            {
                                                                num2 = HDSeedingFb(chrome, indexRow, text, jSON_Settings2.GetValue("txtIdBaiViet"), jSON_Settings2.GetValueInt("nudPage"), jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), jSON_Settings2.GetValueBool("ckbComment"), jSON_Settings2.GetValueBool("ckbLike"), jSON_Settings2.GetValueBool("ckbShare"), jSON_Settings2.GetValueBool("ckbCmtLog"), jSON_Settings2.GetValueBool("ckbCmtDeleteContent"), jSON_Settings2.GetValueBool("ckbCmtImg"), jSON_Settings2.GetValue("txtPathImg"), jSON_Settings2.GetValueBool("ckbLikeLog"), jSON_Settings2.GetValue("ckbLikeType"), rd, text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "SeedingFb");
                                                            }
                                                            break;
                                                        case "HDNhiemVuClone":
                                                            try
                                                            {
                                                                num2 = HDNhiemVuClone(indexRow, text2, chrome, jSON_Settings2.GetValueInt("nudDelayFrom"), jSON_Settings2.GetValueInt("nudDelayTo"), jSON_Settings2.GetValueInt("nudRun"), rd, text16, text15);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDNhiemVuClone");
                                                            }
                                                            break;
                                                        case "HDPostStatus":
                                                            try
                                                            {
                                                                num2 = HDPostStatus(indexRow, text2, chrome, jSON_Settings2.GetValue("txtNoidung"), jSON_Settings2.GetValueBool("ckbPostWall"), jSON_Settings2.GetValueBool("ckbPostImg"), jSON_Settings2.GetValueBool("ckbTagFr"), jSON_Settings2.GetValue("txtPathImg"), jSON_Settings2.GetValue("nudPost"), jSON_Settings2.GetValue("nudImgPost"), jSON_Settings2.GetValue("nudTagFr"), jSON_Settings2.GetValueInt("delayFrom"), jSON_Settings2.GetValueInt("delayTo"), jSON_Settings2.GetValueBool("ckbPinPost"), jSON_Settings2.GetValueBool("ckbPostVideo"), jSON_Settings2.GetValue("txtPathVideo"), rd, text15, text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDPostStatus");
                                                            }
                                                            break;
                                                        case "HDBuffViewFb":
                                                            try
                                                            {
                                                                num2 = HDBuffViewFb(indexRow, text2, chrome, jSON_Settings2.GetValue("txtLinkFacebook"), jSON_Settings2.GetValueInt("nudStart"), jSON_Settings2.GetValueInt("nudEnd"), jSON_Settings2.GetValueBool("ckbSharePost"), jSON_Settings2.GetValueBool("ckbLikePost"), rd, text15, text16);
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                Helpers.Common.ExportError(e, "HDPostStatus");
                                                            }
                                                            break;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    Helpers.Common.ExportError(e, "Tuong tac theo kich ban");
                                                }
                                                if (num2 == -4)
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                                num2 = CheckFacebookLogout(chrome, cUid, cPassword, cFa2, isSendRequest: true);
                                                switch (num2)
                                                {
                                                    case -3:
                                                        chrome.Status = StatusChromeAccount.NoInternet;
                                                        num3 = 1;
                                                        goto quit;
                                                    case -2:
                                                        chrome.Status = StatusChromeAccount.ChromeClosed;
                                                        num3 = 1;
                                                        goto quit;
                                                    case -1:
                                                        chrome.Status = StatusChromeAccount.Checkpoint;
                                                        SetInfoAccount(cId, indexRow, ("Checkpoint"));
                                                        num3 = 1;
                                                        goto quit;
                                                    case 2:
                                                        num3 = 0;
                                                        goto quit;
                                                }
                                                num12++;
                                            }
                                        }
                                        goto runFeatured1;
                                    runFeatured1:
                                        if (setting_InteractGeneral.GetValueBool("ckbAllowFollow"))
                                        {
                                            SetStatusAccount(indexRow, text2 + "Cho phép người khác follow...");
                                            AllowFollow(chrome);
                                            DelayThaoTacNho();
                                        }
                                        if (setting_InteractGeneral.GetValueBool("ckbReviewTag"))
                                        {
                                            SetStatusAccount(indexRow, text2 + "Bật duyệt bài viết...");
                                            ReviewTag(chrome);
                                            DelayThaoTacNho();
                                        }
                                        if (setting_InteractGeneral.GetValueBool("ckbBatThongBaoDangNhap"))
                                        {
                                            SetStatusAccount(indexRow, text2 + "Bật thông báo đăng nhập...");
                                            BatThongBaoDangNhap(chrome, indexRow);
                                            DelayThaoTacNho();
                                        }
                                        //if (setting_InteractGeneral.GetValueBool("ckbCapNhatThongTin"))
                                        //{
                                        //    try
                                        //    {
                                        //        SetStatusAccount(indexRow, text2 + "Cập nhật thông tin...");
                                        //        UpdateInfoWhenInteracting(chrome, indexRow);
                                        //        DelayThaoTacNho();
                                        //    }
                                        //    catch (Exception ex73)
                                        //    {
                                        //        CommonCSharp.ExportError(null, ex73.ToString());
                                        //    }
                                        //}
                                        //if (setting_InteractGeneral.GetValueBool("ckbAutoLinkInstagram"))
                                        //{
                                        //    DelayThaoTacNho();
                                        //    SetStatusAccount(indexRow, text2 + ("Liên kê\u0301t Instagram..."));
                                        //    if (LinkToInstagram(chrome))
                                        //    {
                                        //        DelayThaoTacNho();
                                        //    }
                                        //}
                                        if (setting_InteractGeneral.GetValueBool("ckbLogOutOldDevice"))
                                        {
                                            DelayThaoTacNho();
                                            SetStatusAccount(indexRow, text2 + "Đăng xuất thiết bị cũ...");
                                            LogoutOldDevice(chrome);
                                            DelayThaoTacNho();
                                        }
                                        //if (setting_InteractGeneral.GetValueBool("ckbLogOut"))
                                        //{
                                        //    DelayThaoTacNho();
                                        //    SetStatusAccount(indexRow, text2 + ("Đăng xuất..."));
                                        //    Logout(chrome);
                                        //    DelayThaoTacNho();
                                        //}
                                        //if (setting_InteractGeneral.GetValueBool("ckbCreateShortcut"))
                                        //{
                                        //    DelayThaoTacNho();
                                        //    SetStatusAccount(indexRow, text2 + ("Tạo Shortcut..."));
                                        //    string text17 = dtgvAcc.Rows[indexRow].Cells["cName"].Value.ToString();
                                        //    string nameShortcut = ((text17 == "") ? cUid : (text17 + "_" + cUid));
                                        //    CreateShortcutChrome(setting_general.GetValue("txbPathProfile") + "\\" + cUid, nameShortcut);
                                        //    DelayThaoTacNho();
                                        //}
                                        break;
                                    }
                                quit:;
                                }
                                catch (Exception e)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
                                    num3 = 1;
                                    Helpers.Common.ExportError(chrome, e, "Lỗi không xác định!");
                                }
                                break;
                            }
                        endThread:
                            break;
                    }
                    break;
                }
            }
            string text18 = "";
            if (num3 == 1)
            {
                if (chrome != null)
                {
                    if (chrome.Status == StatusChromeAccount.Checkpoint)
                    {
                        ScreenCaptureError(chrome, cUid, 0);
                    }
                    StatusChromeAccount status = chrome.Status;
                    StatusChromeAccount statusChromeAccount = status;
                    if (statusChromeAccount == StatusChromeAccount.ChromeClosed || statusChromeAccount == StatusChromeAccount.Checkpoint || statusChromeAccount == StatusChromeAccount.NoInternet)
                    {
                        SetRowColor(indexRow, 1);
                        text18 = GetContentStatusChrome.GetContent(chrome.Status);
                    }
                    else
                    {
                        text18 = GetStatusAccount(indexRow);
                    }
                }
                else
                {
                    text18 = GetStatusAccount(indexRow);
                }
            }
            else if (CheckIsUidFacebook(cUid) && CommonRequest.CheckLiveWall(cUid).StartsWith("0|"))
            {
                SetInfoAccount(cId, indexRow, "Die");
                text18 = "Tài khoản Die!";
            }
            else
            {
                chrome.Status = StatusChromeAccount.Empty;
                CommonChrome.CheckStatusAccount(chrome, isSendRequest: true);
                StatusChromeAccount status2 = chrome.Status;
                StatusChromeAccount statusChromeAccount2 = status2;
                if (statusChromeAccount2 == StatusChromeAccount.ChromeClosed || statusChromeAccount2 == StatusChromeAccount.Checkpoint || statusChromeAccount2 == StatusChromeAccount.NoInternet)
                {
                    SetRowColor(indexRow, 1);
                    text18 = GetContentStatusChrome.GetContent(chrome.Status);
                }
            }
            try
            {
                if (chrome?.CheckIsLive() ?? false)
                {
                    int num13 = rd.Next(setting_general.GetValueInt("nudDelayCloseChromeFrom"), setting_general.GetValueInt("nudDelayCloseChromeTo") + 1);

                    if (num13 > 0)
                    {
                        int tickCount2 = Environment.TickCount;
                        while ((Environment.TickCount - tickCount2) / 1000 - num13 < 0)
                        {
                            if (isStop)
                            {
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                break;
                            }
                            SetStatusAccount(indexRow, text2 + "Đóng trình duyệt sau {time}s...".Replace("{time}", (num13 - (Environment.TickCount - tickCount2) / 1000).ToString()));
                            Helpers.Common.DelayTime(0.5);
                        }
                    }

                    try
                    {
                        bool valueBool14 = SettingsTool.GetSettings("configGeneral").GetValueBool("ckbAddChromeIntoForm", false);
                        if (valueBool14)
                        {
                            fViewChrome.remote.RemovePanelDevice(chrome.IndexChrome);
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        chrome.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            string text19 = text18;
            string text20 = text19;
            if (text20 == "")
            {
                SetStatusAccount(indexRow, text2 + "Đã chạy xong!" + (flag2 ? "- Facebook blocked!" : "") + " [" + Helpers.Common.ConvertSecondsToTime((Environment.TickCount - num) / 1000) + "(s)]");
                SetCellAccount(indexRow, "cInteractEnd", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                CommonSQL.UpdateFieldToAccount(cId, "interactEnd", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetInfoAccount(cId, indexRow, "Live");
            }
            else
            {
                SetStatusAccount(indexRow, text2 + text18 + (flag2 ? "- Facebook blocked!" : ""));
            }

            if (flag && Directory.Exists(setting_general.GetValue("txbPathProfile") + "\\" + text4))
            {
                string text21 = setting_general.GetValue("txbPathProfile") + "\\" + text4;
                string pathTo = setting_general.GetValue("txbPathProfile") + "\\" + cUid;
                if (!Helpers.Common.MoveFolder(text21, pathTo) && Helpers.Common.CopyFolder(text21, pathTo))
                {
                    Helpers.Common.DeleteFolder(text21);
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

        //hanh dong
        public int HDDangStatus(int indexRow, string statusProxy, Chrome chrome, string token, int soLuongFrom, int soLuongTo, int khoangCachFrom, int khoangCachTo, bool isVanBan, bool isUseBackground, bool isAnh, string pathFolderAnh, int soLuongAnhFrom, int soLuongAnhTo, bool isVideo, string pathFolderVideo, int soLuongVideoFrom, int soLuongVideoTo, Random rd, bool isTag, int typeUidTag, List<string> lstUidTag, int nudUidFrom, int nudUidTo, string tenHanhDong, string id_HanhDong)
        {
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int num = 0;
            try
            {
                int num2 = rd.Next(soLuongFrom, soLuongTo + 1);
                while (num < num2)
                {
                    try
                    {
                        if (chrome.CheckChromeClosed())
                        {
                            return -2;
                        }
                        if (isTag && typeUidTag == 0)
                        {
                            //get token and get list uid friend
                            //  lstUidTag = CommonChrome.GetMyListUidFriend(chrome);
                        }
                        int num3;
                        do
                        {
                            chrome.GotoURL("https://m.facebook.com/me");
                            num3 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                        }
                        while (num3 == 1);
                        if (new List<int> { -3, -2, -1, 2 }.Contains(num3))
                        {
                            return -1;
                        }
                        string text = "";
                        switch (chrome.CheckExistElements(10.0, "[data-sigil=\"show_composer\"]", "#timelineBody > div > div > div > div:nth-child(2)"))
                        {
                            case 1:
                                text = "[data-sigil=\"show_composer\"]";
                                break;
                            case 2:
                                text = "#timelineBody > div > div > div > div:nth-child(2)";
                                break;
                        }
                        if (!(text != ""))
                        {
                            continue;
                        }
                        chrome.DelayTime(1.0);
                        chrome.Click(4, text);
                        chrome.DelayTime(2.0);
                        if (chrome.CheckExistElement("[data-sigil=\"touchable aaa_public\"]") == 1)
                        {
                            chrome.Click(4, "[data-sigil=\"touchable aaa_public\"]");
                        }
                        if (chrome.CheckExistElement("[data-sigil=\"aaa_tooltip_text mflyout-remove-on-click\"]") == 1)
                        {
                            chrome.Click(4, "[data-sigil=\"aaa_tooltip_text mflyout-remove-on-click\"]");
                        }
                        string text2 = "";
                        switch (chrome.CheckExistElements(10.0, "[data-sigil=\"composer-textarea m-textarea-input\"]", "[data-sigil=\"m-textarea-input composer-textarea\"]"))
                        {
                            case 1:
                                text2 = "[data-sigil=\"composer-textarea m-textarea-input\"]";
                                break;
                            case 2:
                                text2 = "[data-sigil=\"m-textarea-input composer-textarea\"]";
                                break;
                        }
                        if (!(text2 != ""))
                        {
                            continue;
                        }
                        if (chrome.CheckExistElement("input[value=\"300645083384735\"]") != 1)
                        {
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "#m_composer_privacy_selector #m_privacy_button_text_id");
                            if (chrome.CheckExistElement("input[value=\"300645083384735\"]", 5.0) == 1)
                            {
                                chrome.DelayTime(1.0);
                                chrome.ExecuteScript("document.querySelector('input[value=\"300645083384735\"]').click();");
                            }
                            else
                            {
                                chrome.ExecuteScript("document.querySelector('[data-sigil=\"dialog-cancel-button\"]').click()");
                            }
                        }
                        if (isVanBan)
                        {
                            string item = "";
                            lock (dicDangStatus_NoiDung)
                            {
                                if (dicDangStatus_NoiDung[id_HanhDong].Count == 0)
                                {
                                    dicDangStatus_NoiDung[id_HanhDong] = CloneList(dicDangStatus_NoiDungGoc[id_HanhDong]);
                                }
                                item = dicDangStatus_NoiDung[id_HanhDong][rd.Next(0, dicDangStatus_NoiDung[id_HanhDong].Count)];
                                dicDangStatus_NoiDung[id_HanhDong].Remove(item);
                            }
                            item = Helpers.Common.SpinText(item, rd);
                            if (isTag)
                            {
                                string text3 = "";
                                int num4 = rd.Next(nudUidFrom, nudUidTo + 1);
                                for (int i = 0; i < num4; i++)
                                {
                                    if (lstUidTag.Count == 0)
                                    {
                                        break;
                                    }
                                    text3 = lstUidTag[rd.Next(0, lstUidTag.Count)];
                                    lstUidTag.Remove(text3);
                                    item = item + " @[" + text3 + ":]";
                                }
                            }
                            if (item.Trim() != "")
                            {
                                string text4 = chrome.ExecuteScript("return document.title.split('-')[0]").ToString().Trim();
                                string newValue = text4.Substring(text4.LastIndexOf(' ') + 1).Trim();
                                item = item.Replace("[u]", text4);
                                item = item.Replace("[name]", newValue);
                                item = GetIconFacebook.ProcessString(item, rd);
                                if (!Helpers.Common.CheckStringIsContainIcon(item))
                                {
                                    chrome.SendKeys(4, text2, item);
                                    chrome.DelayTime(1.0);
                                }
                                else
                                {
                                    item = item.Replace("\r\n", "*rr*nn").Replace("\n", "*rr*nn");
                                    item = item.Replace("*rr*nn", "\\r\\n").Replace("\"", "\\\"");
                                    for (int j = 0; j < 10; j++)
                                    {
                                        chrome.ExecuteScript("document.querySelector('" + text2 + "').value=\"" + item + "\"");
                                        chrome.DelayTime(2.0);
                                        if (chrome.ExecuteScript("return document.querySelector('" + text2 + "').value+''").ToString() != "")
                                        {
                                            break;
                                        }
                                    }
                                    chrome.Click(4, text2);
                                }
                                if (isUseBackground && !isAnh && !isVideo)
                                {
                                    int num5 = chrome.CountElement("#structured_composer_form div[aria-label]");
                                    if (num5 > 0)
                                    {
                                        chrome.Click(4, "#structured_composer_form div[aria-label]", rd.Next(1, num5));
                                        chrome.DelayThaoTacNho();
                                    }
                                }
                            }
                        }
                        int num6 = 0;
                        if (isAnh)
                        {
                            List<string> list = Directory.GetFiles(pathFolderAnh).ToList();
                            if (list.Count > 0)
                            {
                                List<string> list2 = CloneList(list);
                                string text5 = "";
                                int num7 = rd.Next(soLuongAnhFrom, soLuongAnhTo + 1);
                                for (int k = 0; k < num7; k++)
                                {
                                    if (list2.Count == 0)
                                    {
                                        break;
                                    }
                                    text5 = list2[rd.Next(0, list2.Count)];
                                    list2.Remove(text5);
                                    chrome.SendKeys(1, "photo_input", text5);
                                    num6++;
                                    Helpers.Common.DelayTime(2.0);
                                }
                            }
                        }
                        if (isVideo)
                        {
                            List<string> list3 = Directory.GetFiles(pathFolderVideo).ToList();
                            if (list3.Count > 0)
                            {
                                List<string> list4 = CloneList(list3);
                                string text6 = "";
                                int num8 = rd.Next(soLuongVideoFrom, soLuongVideoTo + 1);
                                for (int l = 0; l < num8; l++)
                                {
                                    if (list4.Count == 0)
                                    {
                                        break;
                                    }
                                    text6 = list4[rd.Next(0, list4.Count)];
                                    list4.Remove(text6);
                                    chrome.SendKeys(1, "video_input", text6);
                                    num6++;
                                    Helpers.Common.DelayTime(2.0);
                                }
                            }
                        }
                        try
                        {
                            for (int m = 0; m < 120; m++)
                            {
                                if (Convert.ToInt32(chrome.ExecuteScript("return (document.querySelectorAll('#structured_composer_form img').length+'')").ToString()) == num6)
                                {
                                    break;
                                }
                                Helpers.Common.DelayTime(1.0);
                            }
                        }
                        catch
                        {
                        }
                        for (int n = 0; n < 10; n++)
                        {
                            chrome.DelayTime(5.0);
                            string text7 = "";
                            try
                            {
                                text7 = chrome.ExecuteScript("return (document.querySelector('[data-sigil=\"touchable submit_composer\"]').value)").ToString();
                            }
                            catch
                            {
                                break;
                            }
                        }
                        if (chrome.Click(4, "#composer-main-view-id > div > div > div:nth-child(3) > div button") != 1)
                        {
                            continue;
                        }
                        for (int num9 = 0; num9 < 300; num9++)
                        {
                            Helpers.Common.DelayTime(1.0);
                            try
                            {
                                if (!Convert.ToBoolean(chrome.ExecuteScript("var x='false'; if(document.querySelector('[data-sigil=\"inprogress\"]')!=null) x=(document.querySelector('[data-sigil=\"inprogress\"]').getAttribute('style')=='')+''; return x")))
                                {
                                    break;
                                }
                            }
                            catch
                            {
                                break;
                            }
                        }
                        num++;
                        if (CommonChrome.CheckFacebookBlocked(chrome))
                        {
                            return -4;
                        }
                        SetStatusAccount(indexRow, statusProxy + ("Đang") + $" {tenHanhDong} ({num}/{num2})...");
                        if (num < num2)
                        {
                            chrome.DelayRandom(khoangCachFrom, khoangCachTo);
                            continue;
                        }
                        return num;
                    }
                    catch
                    {
                        num = -1;
                    }
                }
            }
            catch
            {
            }
            return num;
        }
        public int HDRegPage(Chrome chrome, int indexRow, string proxy, int nudKhoangCachFrom, int nudKhoangCachTo, int numberPageCreate, int typeListName, string txtPathFileName, string txtDescription, string txtPathAvatar, string txtPathCover, string txtWebsite, string txtPhoneNumber, string txtAddress, string txtZipcode, string txtEmail, bool ckbRandomString, bool ckbFirstPost, string txtContentPost, bool ckbPostImg, string txtPathImgPost, int delayPostform, int delayPostto, bool ckbInviteAdmin, List<string> txtAdminInvite, int numberAdInvite, string txtChuoibatdau, bool ckbDeleteNamePage, Random rd, string tenHanhDong, string id_HanhDong)
        {
            int num = 0;
            string uid = GetCellAccount(indexRow, "cUid");
            string password = GetCellAccount(indexRow, "cPassword");
            string code2fa = GetCellAccount(indexRow, "cFa2");
            string token = GetCellAccount(indexRow, "cToken");
            string cookie = GetCellAccount(indexRow, "cCookies");
            string userAgent = GetCellAccount(indexRow, "cUseragent");
            if (userAgent == "")
            {
                userAgent = Base.useragentDefault;
            }
            string text2 = "(IP: " + proxy.Split(':')[0] + ") ";
            int typeProxy = 0;
            int pageCreated = 0;
            string namePage = "";
            int num2 = 0;
            string linkAvt = "";
            string linkCover = "";
            string linkImgPost = "";
            string uidPage = "";
            string uidPageProfile = "";
            string[] str = new string[10];
            try
            {
                while (pageCreated < numberPageCreate + 1)
                {
                    bool flag = false;
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }

                    List<string> pathAvatars = new List<string>();
                    List<string> pathAvt = new List<string>();
                    List<string> pathCovers = new List<string>();
                    List<string> pathCover = new List<string>();
                    List<string> pathPostImgs = new List<string>();
                    List<string> pathPostImg = new List<string>();

                    pathAvatars = Directory.GetFiles(txtPathAvatar).ToList();
                    pathAvt = CloneList(pathAvatars);
                    pathCovers = Directory.GetFiles(txtPathCover).ToList();
                    pathCover = CloneList(pathCovers);

                    if (pathAvatars.Count == 0 || pathAvt.Count == 0 || pathCovers.Count == 0 || pathCover.Count == 0)
                    {
                        goto emptyImgSpam;
                    }
                    if (ckbFirstPost)
                    {
                        pathPostImgs = Directory.GetFiles(txtPathImgPost).ToList();
                        pathPostImg = CloneList(pathPostImgs);
                        if (pathPostImgs.Count == 0 || pathPostImg.Count == 0)
                        {
                            goto emptyImgSpam;
                        }
                    }
                    //check account spam 
                    //if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                    //{
                    //    SetStatusAccount(indexRow, text2 + ("Cookie die!"));
                    //    return -1;
                    //}
                    //else
                    //{
                    SetStatusAccount(indexRow, text2 + ("OK!"));
                    goto initCreated;
                //}

                initCreated:
                    SetStatusAccount(indexRow, text2 + ("Đang khởi tạo dữ liệu..."));
                    List<string> list = new List<string>();
                    if (typeListName == 1)
                    {
                        list = File.ReadAllLines(txtPathFileName).ToList();
                        list = Helpers.Common.RemoveEmptyItems(list);
                        if (list.Count == 0)
                        {
                            goto emptyName;
                        }
                    }
                    else if (typeListName == 2)
                    {
                        //
                    }
                    else
                    {
                        list = CloneList(dicHDRegPage_Name[id_HanhDong]);
                        if (list.Count == 0)
                        {
                            goto emptyName;
                        }
                    }
                    //get 1 ten
                    lock (lock_baivietprofile)
                    {
                        if (typeListName == 1)
                        {
                            list = File.ReadAllLines(txtPathFileName).ToList();
                            list = Helpers.Common.RemoveEmptyItems(list);
                            if (list.Count == 0)
                            {
                                goto emptyName;
                            }
                            namePage = list[rd.Next(0, list.Count)];
                            //list.Remove(namePage);
                            File.WriteAllLines(txtPathFileName, list);
                        } else if (typeListName == 2)
                        {
                            namePage = Helpers.Common.getNameVietNamRandom();
                        }
                        else
                        {

                            if (dicHDRegPage_Name[id_HanhDong].Count == 0)
                            {
                                goto emptyName;
                            }
                            namePage = dicHDRegPage_Name[id_HanhDong][rd.Next(0, dicHDRegPage_Name[id_HanhDong].Count)];
                            //remove name
                            if (ckbDeleteNamePage) dicHDRegPage_Name[id_HanhDong].Remove(namePage);
                        }
                    }
                    if (ckbRandomString)
                    {
                        namePage = namePage + " " + txtChuoibatdau + " " + Helpers.Common.CreateRandomNumber(3);
                    }
                checkAndOpenChorme:
                    SetStatusAccount(indexRow, text2 + $" Chuẩn bị tạo page: {namePage} lượt {pageCreated + 1}/{numberPageCreate}");
                    chrome.GotoURL("https://www.facebook.com/");
                    chrome.DelayTime(2.0);
                    int checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                    if (checkLogoutFb == 1)
                    {
                        goto startCreated;
                    }
                    if (new List<int> { -3, -2, -1, 2 }.Contains(checkLogoutFb))
                    {
                        return -1;
                    }
                    //checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                    //if (checkLogoutFb == 1)
                    //{
                    //    goto quit;
                    //}
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                startCreated:
                    string pageSource = chrome.GetPageSource();
                    cookie = chrome.GetCookieFromChrome();
                    CommonSQL.UpdateFieldToAccount(uid, "cookie1", cookie);
                    string regPage = Helpers.CommonRequest.regPageRequest(pageSource, cookie, proxy, userAgent, typeProxy, namePage, txtDescription);
                    str = regPage.Split('|');
                    if (regPage.StartsWith("0|"))
                    {
                        string msgErr = str[1];
                        SetStatusAccount(indexRow, text2 + $"Tạch: {msgErr}");
                        Helpers.Common.SaveLog("error", $"Id Clone: {uid} - {msgErr}", "created_page");
                        break;
                    }
                    else if (regPage.StartsWith("1|"))
                    {
                        uidPage = str[1];
                        uidPageProfile = str[2];
                        SetStatusAccount(indexRow, text2 + $"Đã tạo thành công page {namePage}/{uidPage} !");
                        Helpers.Common.SaveLog("success", $"https://fb.com/{uidPage} - Name: {namePage} - Id Clone: {uid}", "created_page");
                        SetStatusAccount(indexRow, text2 + $"Chuẩn bị Cập nhật thông tin trang...");
                        //change info
                        goto changeInfo;
                    }
                changeInfo:
                    flag = true;
                    pageCreated++;
                    DelayThaoTacNho(1);
                    cookie = str[3];
                    SetStatusAccount(indexRow, text2 + $"Cập nhật thông tin trang...");
                    bool updateInfoBasic = Helpers.CommonRequest.updateInfoPageRequest(pageSource, cookie, proxy, userAgent, typeProxy, str[2], txtAddress, txtZipcode, txtEmail, txtPhoneNumber, txtWebsite);

                    if (updateInfoBasic)
                    {
                        SetStatusAccount(indexRow, text2 + "Cập nhật thông tin cơ bản thành công.");
                        DelayThaoTacNho(1);
                    }

                    //Helpers.CommonRequest.uploadImgRequestFb(login, cookie, proxy, userAgent, typeProxy, pathUrl);
                    if (pathAvt.Count == 0)
                    {
                        pathAvt = CloneList(pathAvatars);
                    }
                    linkAvt = pathAvt[rd.Next(0, pathAvt.Count)];
                    pathAvt.Remove(linkAvt);
                    SetStatusAccount(indexRow, text2 + ("Đang chuẩn bị ảnh đại diện..."));
                    linkAvt = Helpers.CommonRequest.uploadImgRequestFb(pageSource, cookie, proxy, userAgent, typeProxy, linkAvt).Split('|')[1];
                    DelayThaoTacNho(1);
                    if (pathCover.Count == 0)
                    {
                        pathCover = CloneList(pathCovers);
                    }
                    linkCover = pathCover[rd.Next(0, pathCover.Count)];
                    pathCover.Remove(linkCover);
                    SetStatusAccount(indexRow, text2 + ("Đang chuẩn bị ảnh bìa..."));
                    linkCover = Helpers.CommonRequest.uploadImgRequestFb(pageSource, cookie, proxy, userAgent, typeProxy, linkCover).Split('|')[1];
                    DelayThaoTacNho(1);
                    bool changeCoverAvt = Helpers.CommonRequest.changeAvtCoverRequestFb(pageSource, cookie, proxy, userAgent, typeProxy, str[2], linkAvt, linkCover);

                    if (ckbInviteAdmin)
                    {
                        SetStatusAccount(indexRow, text2 + $"Mời Admin Vào Page!");
                        DelayThaoTacNho(1);
                        goto ckbInviteAdmin;
                    }
                    else
                    {
                        goto checkDone;
                    }

                ckbInviteAdmin:
                    int inviteSuccss = 0;

                    List<string> uidArr = txtAdminInvite;

                    if (uidArr.Count == 0)
                    {
                        goto checkDone;
                    }
                    if (numberAdInvite > uidArr.Count) numberAdInvite = 1;

                    //get ngãu nhiên admin 
                    SetStatusAccount(indexRow, text2 + $"Lấy danh sách admin ngãu nhiên...");
                    List<string> randomIdAdmin = cuakit.Helpers.GetRandomIDs(uidArr, numberAdInvite);
                    SetStatusAccount(indexRow, text2 + $"Mời Admin {inviteSuccss}/{randomIdAdmin.Count}");
                    //string uidInvite = uidArr[rd.Next(0, uidArr.Count)];
                    string login = cuakit.Meta.loginFacebookWithCookie(cookie, userAgent, proxy, typeProxy);
                    string switchUid = Helpers.CommonRequest.switchToUidRequestFb(login, cookie, proxy, userAgent, typeProxy, uidPageProfile);
                    string loadHomeSwitch = cuakit.Meta.loginFacebookWithCookie(switchUid.Split('|')[1], userAgent, proxy, typeProxy);
                    string authInvitePage = Helpers.CommonRequest.authInviteUidRequestFb(loadHomeSwitch, switchUid.Split('|')[1], proxy, userAgent, typeProxy, password, switchUid.Split('|')[0]);

                    if (authInvitePage != "" || authInvitePage != null || randomIdAdmin != null)
                    {
                        foreach (string uidadmin in randomIdAdmin)
                        {
                            bool inviteUidToPage = Helpers.CommonRequest.inviteUidToPageRequestFb(loadHomeSwitch, authInvitePage.Split('|')[2], proxy, userAgent, typeProxy, authInvitePage.Split('|')[0], uidadmin);
                            if (inviteUidToPage)
                            {
                                bool checkExist = CommonSQL.CheckUidPageAndUidCloneActiveInvite(uidadmin, authInvitePage.Split('|')[0]);

                                if (!checkExist)
                                {
                                    bool insertCheck = CommonSQL.InsertInvitePageToDatabase(uidadmin, authInvitePage.Split('|')[0]);
                                    if (insertCheck)
                                    {
                                        inviteSuccss++;
                                        SetStatusAccount(indexRow, text2 + $"Mời OK Uid: {uidadmin} - {inviteSuccss}/{randomIdAdmin.Count}");
                                        DelayThaoTacNho(1);
                                    }
                                }

                            }
                        }
                        goto checkDone;
                    }
                    else
                    {
                        SetStatusAccount(indexRow, text2 + $"Không mời Admin được!");
                        goto checkDone;
                    }

                checkDone:
                    if (flag)
                    {
                        num++;
                        if (pageCreated == numberPageCreate)
                        {
                            SetStatusAccount(indexRow, text2 + $"Đã tạo page ({num}:{pageCreated}/{numberPageCreate})!");
                            goto quit;
                        }
                        else
                        {
                            num2 = 0;
                            if (tenHanhDong == "")
                            {
                                SetStatusAccount(indexRow, text2 + $"Đã tạo page ({num}:{pageCreated}/{numberPageCreate})!");
                            }
                            else
                            {
                                SetStatusAccount(indexRow, text2 + $"Đang {tenHanhDong} ({num}:{pageCreated}/{numberPageCreate})...");
                            }
                            if (chrome.CheckChromeClosed())
                            {
                                return -2;
                            }
                            Helpers.Common.DelayTime(rd.Next(nudKhoangCachFrom, nudKhoangCachTo + 1));
                        }

                    }
                    else
                    {
                        num2++;
                        if (num2 == 5)
                        {
                            break;
                        }
                    }
                    continue;

                emptyName:
                    SetStatusAccount(indexRow, text2 + "Không Lấy Được Tên Nào Để Tạo Page.");
                    return 0;
                emptyImgSpam:
                    SetStatusAccount(indexRow, text2 + "Không Lấy Được Ảnh.");
                    return 0;
                quit:
                    break;
                    //end while
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }
        private int HDNghiGiaiLao(int indexRow, string statusProxy, int delayFrom, int delayTo, string tenHanhDong)
        {
            try
            {
                int num = rd.Next(delayTo, delayTo + 1);
                if (num > 0)
                {
                    int tickCount = Environment.TickCount;
                    while ((Environment.TickCount - tickCount) / 1000 - num < 0 && !isStop)
                    {
                        SetStatusAccount(indexRow, statusProxy + string.Format(("Đang {0}, đợi {{time}}s..."), tenHanhDong).Replace("{time}", (num - (Environment.TickCount - tickCount) / 1000).ToString()));
                        Helpers.Common.DelayTime(0.5);
                    }
                }
                return 1;
            }
            catch
            {
            }
            return 0;
        }
        private int HDTimKiemGoogle(int indexRow, string statusProxy, Chrome chrome, List<string> lstTuKhoa, int countTurnFrom, int countTurnTo, int countPageFrom, int countPageTo, int countLinkClickFrom, int countLinkClickTo, int countTimeScrollFrom, int countTimeScrollTo, string tenHanhDong)
        {
            int result = 0;
            try
            {
                int num = Base.rd.Next(countTurnFrom, countTurnTo + 1);
                for (int i = 0; i < num; i++)
                {
                    SetStatusAccount(indexRow, statusProxy + "Đang" + $" {tenHanhDong} ({i}/{num})...");
                    if (lstTuKhoa.Count == 0)
                    {
                        break;
                    }
                    string text = lstTuKhoa[Base.rd.Next(0, lstTuKhoa.Count)];
                    lstTuKhoa.Remove(text);
                    string content = text.Split('|')[0];
                    string text2 = text.Split('|')[1];
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    chrome.GotoURL("https://www.google.com.vn/");
                    chrome.DelayRandom(2, 3);
                    if (chrome.CheckExistElement("textarea[role =\"combobox\"]", 3.0) != 1)
                    {
                        continue;
                    }
                    chrome.DelayRandom(1, 2);
                    if (chrome.ScrollSmoothIfNotExistOnScreen("document.querySelector('textarea[role =\"combobox\"]')") == 1)
                    {
                        chrome.DelayRandom(1, 2);
                    }
                    switch (setting_general.GetValueInt("tocDoGoVanBan"))
                    {
                        case 0:
                            chrome.SendKeys(rd, 4, "textarea[role=\"combobox\"]", content, 0.08);
                            break;
                        case 1:
                            chrome.SendKeys(4, "textarea[role=\"combobox\"]", content, 0.08);
                            break;
                        case 2:
                            chrome.SendKeys(4, "textarea[role=\"combobox\"]", content);
                            break;
                    }
                    chrome.DelayRandom(2, 3);
                    chrome.SendEnter(4, "textarea[role=\"combobox\"]");
                    chrome.DelayRandom(2, 3);
                    string cssSelector = chrome.GetCssSelector("a", "href", text2);
                    if (cssSelector == "")
                    {
                        int num2 = Base.rd.Next(countPageFrom, countPageTo + 1);
                        for (int j = 0; j < num2 - 1 && chrome.CheckExistElement("[aria-label=\"Page " + (j + 2) + "\"]") == 1; j++)
                        {
                            chrome.ScrollSmooth("document.querySelector('[aria-label=\"Page " + (j + 2) + "\"]')");
                            chrome.DelayRandom(2, 3);
                            chrome.Click(4, "[aria-label=\"Page " + (j + 2) + "\"]");
                            chrome.DelayRandom(2, 3);
                            cssSelector = chrome.GetCssSelector("a", "href", text2);
                            if (cssSelector != "")
                            {
                                break;
                            }
                        }
                    }
                    if (cssSelector != "")
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            if (Base.rd.Next(0, 100) % 5 == 1)
                            {
                                chrome.ScrollSmooth(-Base.rd.Next(100, 300));
                            }
                            else
                            {
                                chrome.ScrollSmooth(Base.rd.Next(100, 300));
                            }
                            chrome.DelayRandom(1, 2);
                        }
                        if (chrome.CheckExistElementOnScreen("document.querySelector('" + cssSelector + "')") != 0)
                        {
                            chrome.ScrollSmooth("document.querySelector('" + cssSelector + "')");
                        }
                        chrome.DelayRandom(2, 3);
                        chrome.Click(4, cssSelector);
                    }
                    else
                    {
                        chrome.GotoURL(text2);
                    }
                    int num3 = Base.rd.Next(countLinkClickFrom, countLinkClickTo + 1);
                    int num4 = Convert.ToInt32(chrome.ExecuteScript("var count=0; document.querySelectorAll('a').forEach(e=>{if(e.getAttribute('href')!=null && e.getAttribute('href')!='') count++}); return count+''").ToString());
                    for (int l = 0; l < num3; l++)
                    {
                        if (num4 == 0)
                        {
                            break;
                        }
                        int index = Base.rd.Next(1, num4 + 1);
                        chrome.ScrollSmoothIfNotExistOnScreen("document.querySelectorAll('a')[" + index + "]");
                        chrome.DelayRandom(2, 3);
                        chrome.Click(4, "a", index);
                        chrome.DelayTime(3.0);
                        int num5 = rd.Next(1, 3);
                        for (int m = 0; m < num5; m++)
                        {
                            chrome.ScrollSmooth(rd.Next(100, 300));
                            chrome.DelayRandom(2, 3);
                        }
                        if (chrome.GetURL() != text2)
                        {
                            chrome.GotoBackPage();
                            chrome.DelayRandom(2, 3);
                        }
                    }
                    int num6 = Base.rd.Next(countTimeScrollFrom, countTimeScrollTo + 1);
                    int tickCount = Environment.TickCount;
                    int num7 = 1;
                    do
                    {
                        num7 = ((Base.rd.Next(1, 1000) % 5 != 0) ? 1 : (-1));
                        chrome.ScrollSmooth(num7 * Base.rd.Next(100, 300));
                        chrome.DelayRandom(2, 3);
                    }
                    while (Environment.TickCount - tickCount < num6 * 1000);
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(chrome, ex, "HDSearchGoogle");
                result = -1;
            }
            return result;
        }
        public int SpamCommentPage(Chrome chrome, int indexRow, string proxy, int nudCountUid, int nudPostUid, int nudKhoangCachFrom, int nudKhoangCachTo, int nudnumberPage, bool ckbAnh, string txtPathAnh, string pathFileUid, int typeListUid, bool ckbRemoveUid, Random rd, string tenHanhDong, string id_HanhDong)
        {
            string uid = GetCellAccount(indexRow, "cUid");
            string cid = GetCellAccount(indexRow, "cId");
            string password = GetCellAccount(indexRow, "cPassword");
            string code2fa = GetCellAccount(indexRow, "cFa2");
            string token = GetCellAccount(indexRow, "cToken");
            string cookie = GetCellAccount(indexRow, "cCookies");
            string userAgent = GetCellAccount(indexRow, "cUseragent");
            if (userAgent == "")
            {
                userAgent = Base.useragentDefault;
            }
            string content = "";
            string pathImg = "";
            string text2 = "(IP: " + proxy.Split(':')[0] + ") ";
            int typeProxy = 0;
            int num = 0;
            string text3 = "";
            int num3 = 0;
            int pageUsing = 0;
            string tokenPage = "";
            string postId = "";
            int countUidUsing = 0;
            int spamCountSend = 0;
            string tokenNew = token;
            try
            {
                int toiDadungPage = nudnumberPage;
                while (pageUsing < toiDadungPage + 1)
                {
                    bool flag = false;
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    //check account spam 
                    if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                    {
                        SetStatusAccount(indexRow, text2 + "Cookie die!");
                        return -1;
                    }
                    else
                    {
                        SetStatusAccount(indexRow, text2 + "Cookie live!");
                        token = GetCellAccount(indexRow, "cToken");
                        //check token
                        if (token != "")
                        {
                            bool checkLiveToken = CommonRequest.CheckLiveToken(cookie, token, userAgent, proxy, typeProxy);
                            if (checkLiveToken)
                            {
                                //process in here
                                SetStatusAccount(indexRow, text2 + "Token Live!");
                                goto initSpam;
                            }
                            else
                            {
                                //get token
                                SetStatusAccount(indexRow, text2 + "Token die!");
                                SetStatusAccount(indexRow, text2 + "Get Token...");
                                Helpers.Common.DelayTime(1);
                                goto getToken;
                            }
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + "Không có token!");
                            goto getToken;
                        }
                    }
                getToken:
                    SetStatusAccount(indexRow, text2 + "Đang get token...");
                    Helpers.Common.DelayTime(1);
                    string text6 = ConvertCookie(chrome.GetCookieFromChrome());
                    CommonSQL.UpdateFieldToAccount(cid, "cookie1", text6);
                    SetCellAccount(indexRow, "cCookies", text6);
                    SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                    Helpers.Common.DelayTime(1);
                    SetStatusAccount(indexRow, text2 + "Bắt đầu lấy Token!");
                    int type = setting_InteractGeneral.GetValueInt("typeToken");
                    if (type == 1)
                    {
                        text6 = ConvertCookie(chrome.GetCookieFromChrome());
                        CommonSQL.UpdateFieldToAccount(cid, "cookie1", text6);
                        SetCellAccount(indexRow, "cCookies", text6);
                        SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                        cookie = GetCellAccount(indexRow, "cCookies");
                        // get token 
                        Helpers.Common.DelayTime(1);
                        chrome.GotoURL("https://adsmanager.facebook.com/adsmanager/");
                        chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Cua Toolkit<br>Đang Thao Tác...</b>'; document.querySelector('body').style = 'font-size:18px; color:red;text-align: center; background-color:#fff'");
                        chrome.DelayTime(2.0);
                        // get token 
                        SetStatusAccount(indexRow, text2 + "Get Token...");
                        object body = chrome.ExecuteScript(@"function getTokenEAAB() { let tokens = window.__accessToken; if (tokens) { return tokens; } else { return '';  } } return getTokenEAAB();");
                        tokenNew = body.ToString();
                        //save
                        if (!string.IsNullOrEmpty(tokenNew))
                        {
                            CommonSQL.UpdateFieldToAccount(cid, "token", tokenNew);
                            SetCellAccount(indexRow, "cToken", tokenNew);
                            SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                            token = GetCellAccount(indexRow, "cToken");
                            SetStatusAccount(indexRow, text2 + "Goto spam.");
                            goto initSpam;
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + " Lỗi Get Token!");
                            return -1;
                        }
                    }
                    if (type == 2)
                    {
                        if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                        {
                            SetStatusAccount(indexRow, text2 + " Cookie Die!");
                            return -1;
                        }
                        SetStatusAccount(indexRow, text2 + " Cookie OK!");

                        string c2Fa = code2fa.Replace(" ", "").Replace("\n", "");
                        Meta request = new Meta(cookie, userAgent, proxy);
                        Helpers.Common.DelayTime(1.0);
                        if (!string.IsNullOrEmpty(c2Fa))
                        {
                            SetStatusAccount(indexRow, text2 + " Get 2FA Code...");
                            Helpers.Common.DelayTime(1.0);
                            TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                            string code = getcode.GetCode(c2Fa);
                            string checkAuth = request.Post("https://business.facebook.com/security/twofactor/reauth/enter/", "&approvals_code=" + code + "&save_device=false&hash").Result;
                            if (!checkAuth.Contains("\"codeConfirmed\":true"))
                            {
                                SetStatusAccount(indexRow, text2 + " 2FA Fail!");
                                return -1;
                            }
                        }
                        Helpers.Common.DelayTime(1.0);
                        SetStatusAccount(indexRow, text2 + " Get Token EAAG...");
                        string getHtml = request.Get("https://business.facebook.com/content_management").Result;
                        tokenNew = Regex.Match(getHtml, "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                        if (!string.IsNullOrEmpty(tokenNew))
                        {
                            CommonSQL.UpdateFieldToAccount(cid, "token", tokenNew);
                            SetCellAccount(indexRow, "cToken", tokenNew);
                            SetStatusAccount(indexRow, text2 + " OK Get Token EAAG.");
                            goto initSpam;
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + " Lỗi Get Token!");
                            return -1;
                        }
                    }
                    if (type == 3)
                    {
                        int wChromeOld = chrome.chrome.Manage().Window.Size.Width;
                        int hChromeOld = chrome.chrome.Manage().Window.Size.Height;
                        chrome.chrome.Manage().Window.Size = new Size(500, 700);

                        text6 = ConvertCookie(chrome.GetCookieFromChrome());
                        CommonSQL.UpdateFieldToAccount(cid, "cookie1", text6);
                        SetCellAccount(indexRow, "cCookies", text6);
                        SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                        cookie = GetCellAccount(indexRow, "cCookies");
                        // get token 
                        Helpers.Common.DelayTime(1);
                        chrome.GotoURL("https://www.facebook.com/dialog/oauth?scope=user_about_me,pages_read_engagement,user_actions.books,user_actions.fitness,user_actions.music,user_actions.news,user_actions.video,user_activities,user_birthday,user_education_history,user_events,user_friends,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_managed_groups,user_photos,user_posts,user_relationship_details,user_relationships,user_religion_politics,user_status,user_tagged_places,user_videos,user_website,user_work_history,email,manage_notifications,manage_pages,publish_actions,publish_pages,read_friendlists,read_insights,read_page_mailboxes,read_stream,rsvp_event,read_mailbox&response_type=token&client_id=124024574287414&redirect_uri=fb124024574287414://authorize/&sso_key=com&display=&fbclid=IwAR1KPwp2DVh2Cu7KdeANz-dRC_wYNjjHk5nR5F-BzGGj7-gTnKimAmeg08k");
                        chrome.DelayTime(2.0);
                        chrome.ExecuteScript("document.querySelector('[name=\"__CONFIRM__\"]').click()");
                        chrome.DelayTime(2.0);
                        // get token 
                        SetStatusAccount(indexRow, text2 + "Get Token...");

                        chrome.GotoURL("view-source:https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https://www.instagram.com/accounts/signup/&&scope=email&response_type=token");
                        tokenNew = Regex.Match(chrome.GetURL(), "#access_token=(.*?)&").Groups[1].Value;
                        chrome.chrome.Manage().Window.Size = new Size(wChromeOld, hChromeOld);
                        //save
                        if (!string.IsNullOrEmpty(tokenNew))
                        {
                            CommonSQL.UpdateFieldToAccount(cid, "token", tokenNew);
                            SetCellAccount(indexRow, "cToken", tokenNew);
                            SetStatusAccount(indexRow, text2 + " OK Get Token EAAGw-insta.");
                            goto initSpam;
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + " Lỗi Get Token!");
                            return -1;
                        }
                    }

                initSpam:
                    SetStatusAccount(indexRow, text2 + "Đang khởi tạo dữ liệu...");
                    //get list uid
                    List<string> list = new List<string>();
                    if (typeListUid == 1)
                    {
                        list = File.ReadAllLines(pathFileUid).ToList();
                        list = Helpers.Common.RemoveEmptyItems(list);
                        if (list.Count == 0)
                        {
                            goto emptyUidSpam;
                        }
                    }
                    else if (!ckbRemoveUid)
                    {
                        list = CloneList(dicSpamComment_listUid[id_HanhDong]);
                        if (list.Count == 0)
                        {
                            goto emptyUidSpam;
                        }
                    }
                    //get list img
                    List<string> lstFrom = new List<string>();
                    List<string> list2 = new List<string>();
                    if (ckbAnh)
                    {
                        lstFrom = Directory.GetFiles(txtPathAnh).ToList();
                        list2 = CloneList(lstFrom);
                        if (lstFrom.Count == 0 || list2.Count == 0)
                        {
                            goto emptyImgSpam;
                        }
                    }
                    //get 1 uid spam
                    if (ckbRemoveUid)
                    {
                        lock (lock_baivietprofile)
                        {
                            if (typeListUid == 1)
                            {
                                list = File.ReadAllLines(pathFileUid).ToList();
                                list = Helpers.Common.RemoveEmptyItems(list);
                                if (list.Count == 0)
                                {
                                    goto emptyUidSpam;
                                }
                                text3 = list[rd.Next(0, list.Count)];
                                list.Remove(text3);
                                File.WriteAllLines(pathFileUid, list);
                            }
                            else
                            {
                                lock (dicSpamComment_listUid)
                                {
                                    if (dicSpamComment_listUid[id_HanhDong].Count == 0)
                                    {
                                        goto emptyUidSpam;
                                    }
                                    text3 = dicSpamComment_listUid[id_HanhDong][rd.Next(0, dicSpamComment_listUid[id_HanhDong].Count)];
                                    dicSpamComment_listUid[id_HanhDong].Remove(text3);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (list.Count == 0)
                        {
                            SetStatusAccount(indexRow, text2 + "Hết Uid Spam!");
                            break;
                        }
                        text3 = list[rd.Next(0, list.Count)];
                        list.Remove(text3);
                    }
                    SetStatusAccount(indexRow, text2 + $" Chuẩn bị spam uid: {text3} lượt {pageUsing + 1}/{nudnumberPage}");
                    goto checkAndOpenChorme;

                checkAndOpenChorme:
                    SetStatusAccount(indexRow, text2 + $"Check Uid... {countUidUsing}/{nudCountUid}");
                    chrome.GotoURL("https://facebook.com/" + text3);
                    chrome.DelayTime(1.0);
                    SetStatusAccount(indexRow, text2 + $" Switch Page: {text3}...");
                    //get token page
                    tokenPage = CommonRequest.getTokenPage(tokenNew, cookie, proxy, typeProxy);
                    chrome.DelayTime(1.0);
                    if (!string.IsNullOrEmpty(tokenPage))
                    {
                        DelayThaoTacNho(1);
                        postId = CommonRequest.getPostNew(text3, tokenPage, cookie, proxy, typeProxy, nudPostUid);
                        //check post
                        if (postId != null && postId != "")
                        {
                            SetStatusAccount(indexRow, text2 + $" Post Id: {postId}...");
                            chrome.GotoURL("https://facebook.com/" + postId);
                            chrome.DelayTime(1.0);
                            int checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                            if (checkLogoutFb == 1)
                            {
                                goto startSpam;
                            }

                            if (new List<int> { -3, -2, -1, 2 }.Contains(checkLogoutFb))
                            {
                                return -1;
                            }
                            checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                            if (checkLogoutFb == 1)
                            {
                                goto quit;
                            }
                            if (chrome.CheckChromeClosed())
                            {
                                return -2;
                            }
                        }
                        else
                        {
                            if (countUidUsing < nudCountUid)
                            {
                                SetStatusAccount(indexRow, text2 + $"{countUidUsing}/{nudCountUid} Uid Không Có Post Nào, Đang lấy uid khác");
                                countUidUsing++;
                                goto initSpam;
                            }
                            else
                            {
                                SetStatusAccount(indexRow, text2 + $"{countUidUsing}/{nudCountUid} Dừng Luồng!");
                                goto quit;
                            }

                        }
                    }

                startSpam:
                    SetStatusAccount(indexRow, text2 + "Check ok, Chuẩn bị spam...");
                    if (ckbAnh)
                    {
                        if (list2.Count == 0)
                        {
                            list2 = CloneList(lstFrom);
                        }
                        pathImg = list2[rd.Next(0, list2.Count)];
                        list2.Remove(pathImg);
                        SetStatusAccount(indexRow, text2 + "Đang chuẩn bị ảnh spam...");
                        pathImg = CommonRequest.UploadImgToServer(pathImg);
                        DelayThaoTacNho(1);
                    }
                    //get noi dung
                    SetStatusAccount(indexRow, text2 + "Chuẩn bị nội dung spam...");
                    lock (dicSpamComment_NoiDung)
                    {
                        if (dicSpamComment_NoiDung[id_HanhDong].Count == 0)
                        {
                            break;
                        }
                        content = dicSpamComment_NoiDung[id_HanhDong][rd.Next(0, dicSpamComment_NoiDung[id_HanhDong].Count)];
                        goto convertContent;
                    }
                convertContent:
                    SetStatusAccount(indexRow, text2 + "Chuyển đổi nội dung spam...");
                    content = Helpers.Common.SpinText(content, rd);
                    string name = Helpers.CommonRequest.getInfoByUid(text3);
                    //content = content.Replace("[u]", name);
                    content = content.Replace("[u]", string.Concat(new string[] { "@[", text3, ":", name, "]" }));
                    content = GetIconFacebook.ProcessString(content, rd);
                    content = HttpUtility.UrlEncode(content);
                    goto sendCommentApi;

                sendCommentApi:
                    //send api comment
                    flag = true;
                    pageUsing++;
                    if (tokenPage != "" && tokenPage != null && postId != "" && postId != null)
                    {
                        SetStatusAccount(indexRow, text2 + $" Send Spam {spamCountSend}/3: {postId}");
                        bool sendComment = CommonRequest.sendCommentByTokenCookie(postId, content, pathImg, ckbAnh, tokenPage, cookie, proxy, typeProxy);
                        if (sendComment)
                        {
                            SetStatusAccount(indexRow, text2 + $" Comment thành công {postId}");
                            Helpers.Common.SaveLog("success", $"Uid: {text3} - https://fb.com/{postId} - OK");
                            DelayThaoTacNho(1);
                            goto checkDone;
                        }
                        else
                        {
                            if (spamCountSend > 2)
                            {
                                SetStatusAccount(indexRow, text2 + $" Comment thất bại: {postId}");
                                Helpers.Common.SaveLog("error", $"Uid: {text3} - https://fb.com/{postId} - ERROR");
                                DelayThaoTacNho(1);
                                goto checkDone;
                            }
                            else
                            {
                                spamCountSend++;
                                SetStatusAccount(indexRow, text2 + $" ReSend {spamCountSend}/3: {postId}");
                                goto sendCommentApi;
                            }
                        }
                    }

                checkDone:
                    if (flag)
                    {
                        num++;
                        if (pageUsing == toiDadungPage)
                        {
                            SetStatusAccount(indexRow, text2 + ("Đang tương tác") + $" profile ({num}:{pageUsing}/{toiDadungPage})...");
                            goto quit;
                        }
                        else
                        {
                            num3 = 0;
                            if (tenHanhDong == "")
                            {
                                SetStatusAccount(indexRow, text2 + ("Đang tương tác") + $" profile ({num}:{pageUsing}/{toiDadungPage})...");
                            }
                            else
                            {
                                SetStatusAccount(indexRow, text2 + ("Đang") + $" {tenHanhDong} ({num}:{pageUsing}/{toiDadungPage})...");
                            }
                            if (chrome.CheckChromeClosed())
                            {
                                return -2;
                            }
                            Helpers.Common.DelayTime(rd.Next(nudKhoangCachFrom, nudKhoangCachTo + 1));
                        }

                    }
                    else
                    {
                        num3++;
                        if (num3 == 5)
                        {
                            break;
                        }
                    }
                    continue;

                emptyUidSpam:
                    SetStatusAccount(indexRow, text2 + ("Hết Uid Spam!"));
                    return 0;
                emptyImgSpam:
                    SetStatusAccount(indexRow, text2 + ("Chưa có ảnh Spam!"));
                    return 0;
                quit:
                    break;
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }
        public int LuotNewFeed(int indexRow, string statusProxy, Chrome chrome, int timeFrom, int timeTo, int delayFrom, int delayTo, bool isLike, int countLikeFrom, int countLikeTo, string type, bool isComment, int countCommentFrom, int countCommentTo, List<string> lstComment, Random rd, string tenHanhDong = "")
        {
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int result = 0;
            int num = chrome.chrome.Manage().Window.Size.Width;
            int num2 = chrome.chrome.Manage().Window.Size.Height;
            chrome.chrome.Manage().Window.Size = new Size(500, 700);
            try
            {
                if (chrome.CheckChromeClosed())
                {
                    return -2;
                }
                while (chrome.GotoURLIfNotExist("https://www.facebook.com") == 1)
                {
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    if (isLike)
                    {
                        num6 = rd.Next(countLikeFrom, countLikeTo + 1);
                    }
                    lstComment = Helpers.Common.RemoveEmptyItems(lstComment);
                    List<string> list = CloneList(lstComment);
                    string text = "";
                    if (isComment)
                    {
                        num7 = rd.Next(countCommentFrom, countCommentTo + 1);
                    }
                    ClosePopup(chrome);
                    int num9 = 0;
                    int num10 = 0;
                    int num11 = rd.Next(timeFrom, timeTo + 1);
                    int tickCount = Environment.TickCount;
                    while (Environment.TickCount - tickCount < num11 * 1000)
                    {
                        int num12 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                        if (num12 == 1)
                        {
                            goto stop;
                        }
                        if (new List<int> { -3, -2, -1, 2 }.Contains(num12))
                        {
                            return -1;
                        }
                        SetStatusAccount(indexRow, statusProxy + ("Đang") + $" {tenHanhDong}(Like:{num3}/{num6}, Comment:{num4}/{num7}, Share:{num5}/{num8})...");
                        if (chrome.CheckExistElementv2($"document.querySelectorAll('[role=\"feed\"] .x1lliihq')[{num9}]") == 1)
                        {
                            num12 = chrome.CheckExistElementOnScreen("document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1)>[role=\"button\"]')[" + num9 * 2 + "]");
                            switch (num12)
                            {
                                case -1:
                                case 1:
                                    if (chrome.ScrollSmooth(num12 * rd.Next(chrome.GetSizeChrome().Y / 2, chrome.GetSizeChrome().Y)) != 2)
                                    {
                                        break;
                                    }
                                    goto endHD;
                                case 0:
                                    if (isLike && num3 < num6 && rd.Next(1, 100) % 2 == 0 && chrome.ExecuteScript("return document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1)>[role=\"button\"]')[" + num9 * 2 + "].querySelectorAll('span')[2].getAttribute('style')+''").ToString() == "null")
                                    {
                                        chrome.ScrollSmooth("document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1)>[role=\"button\"]')[" + num9 * 2 + "]");
                                        chrome.DelayTime(1.0);
                                        int num13 = Convert.ToInt32(type[rd.Next(0, type.Length)]) - 48;
                                        chrome.ExecuteScript("document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1)>[role=\"button\"]')[" + (num9 * 2 + 1) + "].click()");
                                        if (chrome.Click(4, "[role=\"dialog\"] [role=\"toolbar\"] [role=\"button\"]", num13 + 1) == 1)
                                        {
                                            num3++;
                                            DelayThaoTacNho();
                                        }
                                    }
                                    if (isComment && num4 < num7 && rd.Next(1, 100) % 2 == 0 && Convert.ToBoolean(chrome.ExecuteScript($"return document.querySelectorAll('[role=\"feed\"]>[data-pagelet] div:nth-child(4)>[data-visualcompletion=\"ignore-dynamic\"]')[{num9}].querySelectorAll('div:nth-child(2)>div>div>[role=\"button\"]')[2] !=null").ToString()))
                                    {
                                        chrome.ScrollSmooth($"document.querySelectorAll('[role=\"feed\"]>[data-pagelet] div:nth-child(4)>[data-visualcompletion=\"ignore-dynamic\"]')[{num9}].querySelectorAll('div:nth-child(2)>div>div>[role=\"button\"]')[2]");
                                        chrome.DelayTime(1.0);
                                        if (list.Count == 0)
                                        {
                                            list = CloneList(lstComment);
                                        }
                                        text = list[rd.Next(0, list.Count)];
                                        list.Remove(text);
                                        text = Helpers.Common.SpinText(text, rd);
                                        if (chrome.Click(4, "[role=\"feed\"]>[data-pagelet] div:nth-child(4)>[data-visualcompletion=\"ignore-dynamic\"]", num9, 4, "div:nth-child(2)>div>div>[role=\"button\"]", 2) == 1)
                                        {
                                            DelayThaoTacNho();
                                            if (chrome.CheckExistElementv2("document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1) [role=\"textbox\"]')[" + num9 + "]", 10.0) == 1)
                                            {
                                                chrome.DelayTime(1.0);
                                                chrome.ScrollSmooth("document.querySelectorAll('[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1) [role=\"textbox\"]')[" + num9 + "]");
                                                chrome.DelayTime(1.0);
                                                text = text.Replace("[u]", CommonChrome.GetNameFromPost(chrome));
                                                chrome.SendKeysv2(4, "[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1) [role=\"textbox\"]", num9, 0, "", 0, text);
                                                DelayThaoTacNho();
                                                chrome.SendEnter(4, "[role=\"article\"] div[data-visualcompletion=\"ignore-dynamic\"] >div>div>div>div:nth-child(2)>div>div:nth-child(1) [role=\"textbox\"]", num9);
                                            }
                                            DelayThaoTacNho();
                                            num4++;
                                        }
                                    }
                                    num9++;
                                    break;
                                default:
                                    num9++;
                                    break;
                            }
                            Helpers.Common.DelayTime(rd.Next(delayFrom, delayTo + 1));
                        }
                        else
                        {
                            num10++;
                            if (num10 >= 3)
                            {
                                break;
                            }
                            CommonChrome.ScrollRandom(chrome);
                        }
                    }
                    break;
                stop:;
                }
            endHD:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(chrome, ex, "Error HDTuongTacNewsfeed");
                result = -1;
            }
            chrome.chrome.Manage().Window.Size = new Size(num, num2);
            return result;
        }
        public int ChangeInfo(int indexRow, string statusProxy, Chrome chrome, int DelayFrom, int DelayTo, bool ckbChangePass, bool ckbChangeMail, string txtPasswordNew, string txtKeyDVFB, string cbbTypeDv, bool ckbGetCookie, Random rd, string tenHanhDong = "")
        {
            string cId = GetCellAccount(indexRow, "cId");
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int result = 0;
            int num = chrome.chrome.Manage().Window.Size.Width;
            int num2 = chrome.chrome.Manage().Window.Size.Height;
            chrome.chrome.Manage().Window.Size = new Size(500, 700);
            try
            {
                if (chrome.CheckChromeClosed())
                {
                    return -2;
                }

                ClosePopup(chrome);
                int checkLogout = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                if (checkLogout == 1)
                {
                    goto stop;
                }
                if (new List<int> { -3, -2, -1, 2 }.Contains(checkLogout))
                {
                    return -1;
                }

                //change pass
                if (ckbChangePass)
                {
                    SetStatusAccount(indexRow, statusProxy + "Đang đổi mật khẩu...");
                    chrome.GotoURL("https://mbasic.facebook.com/settings/security/password/");
                    chrome.DelayTime(2.0);
                    chrome.SendKeys(2, "password_old", cellAccount2);
                    chrome.DelayTime(2.0);
                    chrome.SendKeys(2, "password_new", txtPasswordNew);
                    chrome.DelayTime(2.0);
                    chrome.SendKeys(2, "password_confirm", txtPasswordNew);
                    chrome.DelayTime(2.0);
                    chrome.Click(2, "save");
                    chrome.DelayTime(1.0);
                    if (chrome.CheckExistElement("label:nth-of-type(2)>table>tbody>tr>td>div>div", 3.0) == 1)
                    {
                        chrome.Click(3, "//label[2]/table/tbody/tr/td/div/div");
                        chrome.DelayTime(1.0);
                        chrome.Click(2, "submit_action");
                        CommonSQL.UpdateFieldToAccount(cId, "pass", txtPasswordNew);
                        SetCellAccount(indexRow, "cPassword", txtPasswordNew);
                        SetStatusAccount(indexRow, statusProxy + "Đổi thành công!");
                        Helpers.Common.DelayTime(rd.Next(DelayFrom, DelayTo + 1));
                    }
                    else
                    {
                        string pageSource = chrome.GetPageSource();
                        if (pageSource.Contains("Để bảo vệ công đồng khỏi spam"))
                        {
                            CommonSQL.UpdateFieldToAccount(cId, "ghiChu", "Nick đang chặn đổi mật khẩu.");
                            SetStatusAccount(indexRow, statusProxy + "Nick tạm thời chặn đổi mật khẩu!");
                            SetRowColor(indexRow, 1);
                        }
                        else if (pageSource.Contains("Hãy nhập mật khẩu hợp lệ rồi thử lại."))
                        {
                            CommonSQL.UpdateFieldToAccount(cId, "ghiChu", "Changed Pass");
                            SetStatusAccount(indexRow, statusProxy + "Mật khẩu cũ sai!");
                            SetRowColor(indexRow, 1);
                        }
                        else if (pageSource.Contains("Mật khẩu này phải khác mật khẩu cũ."))
                        {
                            SetStatusAccount(indexRow, statusProxy + "Mật khẩu đang giống mật khẩu muốn đổi!");
                        }
                        else
                        {
                            SetStatusAccount(indexRow, statusProxy + "Không thể đổi mật khẩu!");
                            SetRowColor(indexRow, 1);
                        }
                    }
                }

                //change maill
                if (ckbChangeMail)
                {
                    SetStatusAccount(indexRow, statusProxy + "Đang đổi mail mới...");
                    chrome.GotoURL("https://mbasic.facebook.com/settings/email/add");
                    chrome.DelayTime(2.0);
                    if (chrome.CheckExistElement("[name=\"email\"]", 5.0) == 1)
                    {
                        int buyCount = 0;
                        int buyErr = 3;
                        bool buyCountStatus = false;

                        while (buyCount < buyErr && !buyCountStatus)
                        {
                            // Mua mail mới
                            SetStatusAccount(indexRow, statusProxy + $"Đang mua mail mới {buyCount}/{buyErr}");
                            string mailnew = Helpers.CommonRequest.buyEmailDongVanFb(txtKeyDVFB, cbbTypeDv);

                            if (mailnew.StartsWith("success|"))
                            {
                                buyCountStatus = true;
                                SetStatusAccount(indexRow, statusProxy + "Mua thành công...");
                                // Lấy thông tin mail mới
                                string[] str = mailnew.Split('|');
                                string emailnew = str[1];
                                string passmailnew = str[2];
                                string fullmailpass = emailnew + "|" + passmailnew;

                                // Gửi mail mới
                                chrome.SendKeys(2, "email", emailnew);
                                chrome.DelayTime(2.0);
                                if (chrome.CheckExistElement("[name=\"save_password\"]", 5.0) == 1)
                                {
                                    chrome.SendKeys(2, "save_password", cellAccount2);
                                    chrome.DelayTime(2.0);
                                }
                                chrome.Click(2, "save");
                                chrome.DelayTime(2.0);

                                chrome.GotoURL("https://mbasic.facebook.com/entercode.php?cp=" + emailnew);
                                chrome.DelayTime(2.0);

                                // Lấy code mail
                                int getOtpCount = 0;
                                int getOtpErr = 4;
                                bool getOtpStatus = false;

                                while (getOtpCount < getOtpErr && !getOtpStatus)
                                {
                                    SetStatusAccount(indexRow, statusProxy + $"Đang lấy otp {buyCount}/{buyErr}");
                                    string getOtp = Helpers.CommonRequest.getOtpDongVanFb(emailnew, passmailnew, "facebook");
                                    if (getOtp.StartsWith("success|"))
                                    {

                                        // Lấy thông tin mail mới
                                        string[] str2 = getOtp.Split('|');
                                        string otp = str2[1];
                                        SetStatusAccount(indexRow, statusProxy + $"Get thành công OTP: {otp}");
                                        chrome.SendKeys(2, "code", otp);
                                        chrome.DelayTime(1.0);
                                        chrome.Click(3, "//div[2]/form/table/tbody/tr/td[2]/input");
                                        chrome.DelayTime(1.0);
                                        string pageSource = chrome.GetPageSource();

                                        int a = 0;
                                        if (pageSource.Contains("Mã xác nhận không hợp lệ."))
                                        {
                                            SetStatusAccount(indexRow, statusProxy + $"Mã xác nhận không hợp lệ: {otp}");
                                            chrome.SendKeys(2, "code", "");
                                            buyCount++;
                                        }
                                        else
                                        {
                                            CommonSQL.UpdateFieldToAccount(cId, "email", emailnew);
                                            CommonSQL.UpdateFieldToAccount(cId, "passmail", passmailnew);
                                            SetStatusAccount(indexRow, statusProxy + $"Thêm email thành công : {emailnew}/{passmailnew}");
                                            getOtpStatus = true;
                                        }

                                    }
                                    else
                                    {
                                        string[] str2 = getOtp.Split('|');
                                        SetStatusAccount(indexRow, statusProxy + "Lỗi: " + str2[1]);
                                        buyCount++;
                                    }

                                }
                                if (getOtpCount >= getOtpErr && !getOtpStatus)
                                {
                                    SetStatusAccount(indexRow, statusProxy + $"Get OTP thất bại {getOtpCount}/{getOtpErr}.");
                                    goto stop;
                                }
                                ///
                            }
                            else
                            {
                                string[] str = mailnew.Split('|');
                                SetStatusAccount(indexRow, statusProxy + "Lỗi: " + str[1]);
                                buyCount++;
                            }
                        }

                        if (buyCount >= buyErr && !buyCountStatus)
                        {
                            SetStatusAccount(indexRow, statusProxy + $"Hệ thống mail quá tải {buyCount}/{buyErr}.");
                            goto stop;
                        }
                    }
                    else
                    {
                        SetStatusAccount(indexRow, statusProxy + "Không thể thêm mail mới!");
                    }
                }

                //get cookie after change info
                if (ckbGetCookie)
                {
                    chrome.GotoURL("https://www.facebook.com/");
                    Helpers.Common.DelayTime(rd.Next(DelayFrom, DelayTo + 1));
                    string cookieNew = chrome.GetCookieFromChrome();
                    SetCellAccount(indexRow, "cCookies", cookieNew);
                    CommonSQL.UpdateFieldToAccount(cId, "cookie", cookieNew);
                    SetStatusAccount(indexRow, statusProxy + "Get Coookie Thành Công!");
                }
            stop:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(chrome, ex, "Error ChangeInfo");
                result = -1;
            }
            chrome.chrome.Manage().Window.Size = new Size(num, num2);

            return result;
        }
        public int HDKetBanGoiY(int indexRow, string statusProxy, Chrome chrome, int soLuongFrom, int soLuongTo, int delayFrom, int delayTo, bool ckbChiKetBanTenCoDau, bool ckbOnlyAddFriendWithMutualFriends, int maxError, Random rd, string tenHanhDong = "")
        {
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int num = 0;
            int num2 = -1;
            int num3 = 0;
            int num4 = 0;
            try
            {
                int num5 = rd.Next(soLuongFrom, soLuongTo + 1);
                while (true)
                {
                processAddFr:
                    if (CommonChrome.GoToFriendSuggest(chrome) == -2)
                    {
                        return -2;
                    }
                    int num6 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                    if (num6 == 1)
                    {
                        continue;
                    }
                    if (new List<int> { -3, -2, -1, 2 }.Contains(num6))
                    {
                        return -1;
                    }
                    while (num < num5)
                    {
                        num6 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                        if (num6 == 1)
                        {
                            goto processAddFr;
                        }
                        if (new List<int> { -3, -2, -1, 2 }.Contains(num6))
                        {
                            return -1;
                        }
                        num2++;
                        if (Convert.ToBoolean(chrome.ExecuteScript("return document.querySelectorAll('[data-sigil=\"m-add-friend-button-container\"]>div>div>div>a')[" + num2 + "]==null?'false':'true'")))
                        {
                            chrome.ScrollSmoothIfNotExistOnScreen("document.querySelectorAll('[data-sigil=\"m-add-friend-button-container\"]>div>div>div>a')[" + num2 + "]");
                            chrome.DelayTime(1.0);
                            bool flag = true;
                            if (ckbChiKetBanTenCoDau)
                            {
                                string ten = chrome.ExecuteScript("return document.querySelectorAll('[data-sigil=\"undoable-action\"]>div:nth-child(2)>div>div>h3>a,[data-sigil=\"undoable-action\"]>div:nth-child(2)>div>div>h1>a')[" + num2 + "].innerText").ToString();
                                if (!Helpers.Common.IsVNName(ten))
                                {
                                    flag = false;
                                }
                            }
                            if (flag && ckbOnlyAddFriendWithMutualFriends)
                            {
                                string pValue = chrome.ExecuteScript("return document.querySelectorAll('[data-sigil=\"m-add-friend-source-replaceable\"]')[" + num2 + "].innerText").ToString();
                                flag = Helpers.Common.IsContainNumber(pValue);
                            }
                            if (!flag)
                            {
                                continue;
                            }
                            chrome.ExecuteScript("document.querySelectorAll('[data-sigil=\"m-add-friend-button-container\"]>div>div>div>a>button>span')[" + num2 + "].click();");
                            //chrome.Click(4, "[data-sigil=\"m-add-friend-button-container\"]>div>div>div>a>button>span", num2);
                            DelayThaoTacNho();
                            if (CommonChrome.SkipNotifyWhenAddFriend(chrome))
                            {
                                num3++;
                                if (num3 >= maxError)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                num3 = 0;
                            }
                            num++;
                            if (tenHanhDong == "")
                            {
                                SetStatusAccount(indexRow, statusProxy + ("Đang kết bạn gợi ý") + $" ({num}/{num5})...");
                            }
                            else
                            {
                                SetStatusAccount(indexRow, statusProxy + ("Đang") + $" {tenHanhDong} ({num}/{num5})...");
                            }
                            if (chrome.CheckChromeClosed())
                            {
                                return -2;
                            }
                            delayFrom = ((delayFrom > 2) ? (delayFrom - 2) : 0);
                            delayTo = ((delayTo > 2) ? (delayTo - 2) : 0);
                            chrome.DelayTime(rd.Next(delayFrom, delayTo + 1));
                        }
                        else
                        {
                            num4++;
                            if (num4 == 3)
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }
        public int HDXacNhanKetBan(int indexRow, string statusProxy, Chrome chrome, int soLuongFrom, int soLuongTo, int delayFrom, int delayTo, Random rd, string tenHanhDong = "", bool ckbChiKetBanTenCoDau = false, bool ckbOnlyAddFriendWithMutualFriends = false)
        {
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            try
            {
                int num5 = rd.Next(soLuongFrom, soLuongTo + 1);
                while (true)
                {
                processAcpFr:
                    if (chrome.GotoURL("https://m.facebook.com/friends/center/requests/all") == -2)
                    {
                        return -2;
                    }
                    chrome.DelayRandom(3, 5);
                    int num6 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                    if (num6 == 1)
                    {
                        continue;
                    }
                    if (new List<int> { -3, -2, -1, 2 }.Contains(num6))
                    {
                        return -1;
                    }
                    if (num5 <= 0)
                    {
                        break;
                    }
                    int num7 = 0;
                    switch (chrome.CheckExistElements(10.0, "#friends_center_main>div>header>h3>span", "[data-sigil=\"m-friend-center-req-badge\"]"))
                    {
                        case 1:
                            num7 = Convert.ToInt32(chrome.ExecuteScript("var dem='0';var x= document.querySelector('#friends_center_main>div>header>h3>span');if(x!=null) dem=x.innerText; return dem;").ToString().Replace(",", "")
                                .Replace(".", ""));
                            break;
                        case 2:
                            num7 = Convert.ToInt32(chrome.ExecuteScript("return document.querySelector('[data-sigil=\"m-friend-center-req-badge\"]').innerText").ToString().Replace(",", "")
                                .Replace(".", ""));
                            break;
                    }
                    if (num7 <= 0)
                    {
                        break;
                    }
                    num4 = ((num5 < 100) ? 5 : 10);
                    int num8 = -1;
                    while (num < num7)
                    {
                        num6 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                        if (num6 == 1)
                        {
                            goto processAcpFr;
                        }
                        if (new List<int> { -3, -2, -1, 2 }.Contains(num6))
                        {
                            return -1;
                        }
                        num8++;
                        if (Convert.ToBoolean(chrome.ExecuteScript("return document.querySelectorAll('[data-sigil*=\"m-optimistic-response-root\"]')[" + num8 + "]==null?'false':'true'")))
                        {
                            chrome.ScrollSmoothIfNotExistOnScreen("document.querySelectorAll('button[data-sigil=\"touchable confirm-request\"]')[" + num8 + "]");
                            chrome.DelayTime(1.0);
                            bool flag = true;
                            if (ckbChiKetBanTenCoDau)
                            {
                                string ten = chrome.ExecuteScript("return document.querySelectorAll('[data-sigil*=\"m-optimistic-response-root\"] h3,[data-sigil*=\"m-optimistic-response-root\"] h1')[" + num8 + "].innerText").ToString();
                                if (!Helpers.Common.IsVNName(ten))
                                {
                                    flag = false;
                                }
                            }
                            if (flag && ckbOnlyAddFriendWithMutualFriends)
                            {
                                string pValue = chrome.ExecuteScript("return document.querySelectorAll('[data-sigil=\"m-add-friend-source-replaceable\"]')[" + num8 + "].innerText").ToString();
                                flag = Helpers.Common.IsContainNumber(pValue);
                            }
                            if (!flag)
                            {
                                continue;
                            }
                            chrome.ExecuteScript("document.querySelectorAll('button[data-sigil=\"touchable confirm-request\"]>span')[" + num8 + "].click()");
                            //chrome.Click(4, "button[data-sigil=\"touchable confirm-request\"]", num8);
                            DelayThaoTacNho();
                            if (CommonChrome.SkipNotifyWhenAddFriend(chrome))
                            {
                                num3++;
                                if (num3 >= num4)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                num3 = 0;
                            }
                            num++;
                            if (tenHanhDong == "")
                            {
                                SetStatusAccount(indexRow, statusProxy + ("Đang xác nhận kết bạn") + $" ({num}/{num5})...");
                            }
                            else
                            {
                                SetStatusAccount(indexRow, statusProxy + ("Đang") + $" {tenHanhDong} ({num}/{num5})...");
                            }
                            if (num >= num5)
                            {
                                break;
                            }
                            if (chrome.CheckChromeClosed())
                            {
                                return -2;
                            }
                            delayFrom = ((delayFrom > 2) ? (delayFrom - 2) : 0);
                            delayTo = ((delayTo > 2) ? (delayTo - 2) : 0);
                            chrome.DelayTime(rd.Next(delayFrom, delayTo + 1));
                        }
                        else
                        {
                            num2++;
                            if (num2 == 5)
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }
        public int HDSeedingFb(Chrome chrome, int indexRow, string proxy, string txtIdBaiViet, int nudPage, int nudDelayFrom, int nudDelayTo, bool ckbComment, bool ckbLike, bool ckbShare, bool ckbCmtLog, bool ckbCmtDeleteContent, bool ckbCmtImg, string txtPathImg, bool ckbLikeLog, string ckbLikeType, Random rd, string tenHanhDong, string id_HanhDong)
        {
            string uid = GetCellAccount(indexRow, "cUid");
            string password = GetCellAccount(indexRow, "cPassword");
            string code2fa = GetCellAccount(indexRow, "cFa2");
            string token = GetCellAccount(indexRow, "cToken");
            string cookie = GetCellAccount(indexRow, "cCookies");
            string userAgent = GetCellAccount(indexRow, "cUseragent");
            if (userAgent == "")
            {
                userAgent = Base.useragentDefault;
            }
            string content = "";
            string pathImg = "";
            string text2 = "(IP: " + proxy.Split(':')[0] + ") ";
            int typeProxy = 0;
            int num = 0;
            int spamCount = 0;
            string tokenNew = "";
            //code here
            try
            {
                while (true)
                {
                    if (dicSeedingFb_NoiDung[id_HanhDong].Count == 0)
                    {
                        SetStatusAccount(indexRow, text2 + "Hết nội dung seeding...");
                        break;
                    }
                    bool flag = false;
                    //check chrome 
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                    {
                        SetStatusAccount(indexRow, text2 + ("Cookie die!"));
                        return -1;
                    }
                    else
                    {
                        SetStatusAccount(indexRow, text2 + ("Cookie live!"));
                        //check token
                        if (token != "")
                        {
                            bool checkLiveToken = CommonRequest.CheckLiveToken(cookie, token, userAgent, proxy, typeProxy);
                            if (checkLiveToken)
                            {
                                //process in here
                                SetStatusAccount(indexRow, text2 + ("Token Live!"));
                                goto initSpam;
                            }
                            else
                            {
                                //get token
                                SetStatusAccount(indexRow, text2 + ("Token die!"));
                                SetStatusAccount(indexRow, text2 + "Get Token...");
                                Helpers.Common.DelayTime(1);
                                goto getToken;
                            }
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + ("Không có token!"));
                            goto getToken;
                        }
                    getToken:
                        SetStatusAccount(indexRow, text2 + "Đang get token...");
                        Helpers.Common.DelayTime(1);
                        string text6 = ConvertCookie(chrome.GetCookieFromChrome());
                        CommonSQL.UpdateFieldToAccount(uid, "cookie1", text6);
                        SetCellAccount(indexRow, "cCookies", text6);
                        SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                        Helpers.Common.DelayTime(1);
                        SetStatusAccount(indexRow, text2 + "Bắt đầu lấy Token!");

                        if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                        {
                            SetStatusAccount(indexRow, text2 + " Cookie Die!");
                            return -1;
                        }
                        SetStatusAccount(indexRow, text2 + " Cookie OK!");

                        string c2Fa = code2fa.Replace(" ", "").Replace("\n", "");
                        Meta request = new Meta(cookie, userAgent, proxy);
                        Helpers.Common.DelayTime(1.0);
                        if (!string.IsNullOrEmpty(c2Fa))
                        {
                            SetStatusAccount(indexRow, text2 + " Get 2FA Code...");
                            Helpers.Common.DelayTime(1.0);
                            TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                            string code = getcode.GetCode(c2Fa);
                            string checkAuth = request.Post("https://business.facebook.com/security/twofactor/reauth/enter/", "&approvals_code=" + code + "&save_device=false&hash").Result;
                            if (!checkAuth.Contains("\"codeConfirmed\":true"))
                            {
                                SetStatusAccount(indexRow, text2 + " 2FA Fail!");
                                return -1;
                            }
                        }
                        Helpers.Common.DelayTime(1.0);
                        SetStatusAccount(indexRow, text2 + " Get Token EAAG...");
                        string getHtml = request.Get("https://business.facebook.com/content_management").Result;
                        tokenNew = Regex.Match(getHtml, "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                        if (tokenNew != "")
                        {
                            CommonSQL.UpdateFieldToAccount(uid, "token", tokenNew);
                            SetCellAccount(indexRow, "cToken", tokenNew);
                            SetStatusAccount(indexRow, text2 + " OK Get Token EAAG.");
                            goto initSpam;
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + " Lỗi Get Token!");
                            return -1;
                        }

                    initSpam:
                        SetStatusAccount(indexRow, text2 + "Đang khởi tạo dữ liệu...");
                        //check uid comment 
                        SetStatusAccount(indexRow, text2 + $" Chuẩn bị seeding uid: {txtIdBaiViet}");
                        goto checkAndOpenChorme;

                    checkAndOpenChorme:
                        SetStatusAccount(indexRow, text2 + $"Check Uid...");
                        chrome.GotoURL("https://facebook.com/" + txtIdBaiViet);
                        chrome.DelayTime(1.0);
                        int checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                        if (checkLogoutFb == 1)
                        {
                            goto startSpam;
                        }

                        if (new List<int> { -3, -2, -1, 2 }.Contains(checkLogoutFb))
                        {
                            return -1;
                        }
                        checkLogoutFb = CheckFacebookLogout(chrome, uid, password, code2fa);
                        if (checkLogoutFb == 1)
                        {
                            goto quit;
                        }
                        if (chrome.CheckChromeClosed())
                        {
                            return -2;
                        }
                    startSpam:
                        SetStatusAccount(indexRow, text2 + "Lấy page...");
                        //get list page
                        List<string> listTokens = CommonRequest.getTokenPageLimit(tokenNew, cookie, nudPage, proxy, typeProxy);
                        if (listTokens.Count > 0)
                        {
                            SetStatusAccount(indexRow, text2 + "Lấy xong, Chuẩn bị seeding...");
                            foreach (string isPage in listTokens)
                            {
                                string tokenPage = isPage.Split('|')[1];
                                string uidPage = isPage.Split('|')[0];
                                //chuc nang chay
                                //comment
                                if (ckbComment)
                                {
                                    if (ckbCmtImg)
                                    {
                                        //get list img
                                        SetStatusAccount(indexRow, text2 + "Lấy Ảnh...");

                                        List<string> lstFrom = new List<string>();
                                        List<string> list2 = new List<string>();

                                        lstFrom = Directory.GetFiles(txtPathImg).ToList();
                                        list2 = CloneList(lstFrom);
                                        if (lstFrom.Count == 0 || list2.Count == 0)
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không có ảnh.!");
                                            ckbCmtImg = false;
                                        }

                                        if (ckbCmtImg)
                                        {
                                            SetStatusAccount(indexRow, text2 + "Upload ảnh...");
                                            if (list2.Count == 0)
                                            {
                                                list2 = CloneList(lstFrom);
                                            }
                                            pathImg = list2[rd.Next(0, list2.Count)];
                                            list2.Remove(pathImg);
                                            SetStatusAccount(indexRow, text2 + ("Đang chuẩn bị ảnh spam..."));
                                            pathImg = CommonRequest.UploadImgToServer(pathImg);
                                            DelayThaoTacNho(1);
                                        }
                                    }

                                    int spamCountSend = 0;
                                    SetStatusAccount(indexRow, text2 + "Chuẩn bị nội dung comment...");
                                    lock (dicSeedingFb_NoiDung)
                                    {
                                        if (dicSeedingFb_NoiDung[id_HanhDong].Count == 0)
                                        {
                                            SetStatusAccount(indexRow, text2 + "Hết nội dung spam...");
                                            break;
                                        }
                                        content = dicSeedingFb_NoiDung[id_HanhDong][rd.Next(0, dicSeedingFb_NoiDung[id_HanhDong].Count)];
                                        if (ckbCmtDeleteContent) dicSeedingFb_NoiDung[id_HanhDong].Remove(content);
                                        goto convertContent;
                                    }
                                convertContent:
                                    SetStatusAccount(indexRow, text2 + ("Chuyển đổi nội dung spam..."));
                                    content = Helpers.Common.SpinText(content, rd);
                                    //string name = Helpers.CommonRequest.getInfoByUid(text3);
                                    //content = content.Replace("[u]", name);
                                    content = GetIconFacebook.ProcessString(content, rd);
                                    content = HttpUtility.UrlEncode(content);
                                    goto sendCommentApi;
                                sendCommentApi:
                                    flag = true;
                                    if (tokenPage != "" && tokenPage != null && txtIdBaiViet != "" && txtIdBaiViet != null)
                                    {
                                        SetStatusAccount(indexRow, text2 + $" Send Spam {spamCountSend}/3: {txtIdBaiViet}");
                                        bool sendComment = CommonRequest.sendCommentByTokenCookie(txtIdBaiViet, content, pathImg, ckbCmtImg, tokenPage, cookie, proxy, typeProxy);
                                        if (sendComment)
                                        {
                                            SetStatusAccount(indexRow, text2 + $" Comment thành công {txtIdBaiViet}");
                                            Helpers.Common.SaveLog("success", $"IdPost: {txtIdBaiViet} - https://fb.com/{txtIdBaiViet} - OK", "seeding_pages");
                                            DelayThaoTacNho(1);
                                            goto checkDone;
                                        }
                                        else
                                        {
                                            if (spamCountSend > 2)
                                            {
                                                SetStatusAccount(indexRow, text2 + $" Comment thất bại: {txtIdBaiViet}");
                                                Helpers.Common.SaveLog("error", $"Uid: {txtIdBaiViet} - https://fb.com/{txtIdBaiViet} - ERROR", "seeding_pages");
                                                DelayThaoTacNho(1);
                                                goto checkDone;
                                            }
                                            else
                                            {
                                                spamCountSend++;
                                                SetStatusAccount(indexRow, text2 + $" ReSend {spamCountSend}/3: {txtIdBaiViet}");
                                                goto sendCommentApi;
                                            }
                                        }
                                    }
                                checkDone:
                                    if (flag)
                                    {
                                        num++;
                                        SetStatusAccount(indexRow, text2 + "Đang" + $" {tenHanhDong} ({num}:{uidPage}/{nudPage})...");
                                        if (chrome.CheckChromeClosed())
                                        {
                                            return -2;
                                        }
                                        Helpers.Common.DelayTime(rd.Next(nudDelayFrom, nudDelayTo + 1));
                                    }
                                    continue;
                                }
                                Helpers.Common.DelayTime(rd.Next(nudDelayFrom, nudDelayTo + 1));
                                if (ckbLike)
                                {
                                    //
                                }
                                if (ckbShare)
                                {

                                }
                            }
                        }
                        else
                        {
                            SetStatusAccount(indexRow, text2 + "Nick không có page nào cả!");
                            goto quit;
                        }
                    quit:
                        break;
                    }
                }

            }
            catch
            {
                num = -1;
            }
            return num;
        }
        public int HDKetBanUid(int indexRow, string statusProxy, Chrome chrome, int delayFrom, int delayTo, Random rd, string tenHanhDong = "", string id_HanhDong = "")
        {
            string cellAccount = GetCellAccount(indexRow, "cUid");
            string cellAccount2 = GetCellAccount(indexRow, "cPassword");
            string cellAccount3 = GetCellAccount(indexRow, "cFa2");
            int num = 0;
            string uidfb = "";
            try
            {
                while (true)
                {
                    if (dicHDKetBanUid[id_HanhDong].Count == 0)
                    {
                        SetStatusAccount(indexRow, statusProxy + "Chạy hết Uid. Dừng Luồng!");
                        break;
                    }
                    int num2 = CheckFacebookLogout(chrome, cellAccount, cellAccount2, cellAccount3);
                    if (num2 == 1)
                    {
                        continue;
                    }
                    if (new List<int> { -3, -2, -1, 2 }.Contains(num2))
                    {
                        return -1;
                    }

                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    //lấy id 
                    SetStatusAccount(indexRow, statusProxy + "Lấy Id Kết Bạn...");
                    lock (dicHDKetBanUid)
                    {
                        if (dicHDKetBanUid[id_HanhDong].Count == 0)
                        {
                            SetStatusAccount(indexRow, statusProxy + "Chạy hết Uid. Dừng Luồng!");
                            break;
                        }
                        uidfb = dicHDKetBanUid[id_HanhDong][rd.Next(0, dicHDKetBanUid[id_HanhDong].Count)];
                        dicHDKetBanUid[id_HanhDong].Remove(uidfb);
                        SetStatusAccount(indexRow, statusProxy + "Kết bạn uid: " + uidfb);
                        goto ketbanUid;
                    }
                ketbanUid:
                    //check uid 
                    chrome.GotoURL("https://mbasic.facebook.com/" + uidfb);
                    chrome.DelayTime(1);
                    if (chrome.FindElementChrome(3, "//*[contains(@href,'a/friends/profile/add/?subject')]"))
                    {
                        SetStatusAccount(indexRow, statusProxy + "Gửi Lời Mời Kết Bạn: " + uidfb);
                        try
                        {
                            chrome.DelayTime(1);
                            chrome.Click(3, "//*[contains(@href,'a/friends/profile/add/?subject')]");
                            SetStatusAccount(indexRow, statusProxy + "Click Gửi Lời Mời: " + uidfb);
                        }
                        catch
                        {
                        }

                        try
                        {
                            chrome.DelayTime(1);
                            chrome.Click(3, "//*[@value='Xác nhận']");
                            SetStatusAccount(indexRow, statusProxy + "Click Xác Nhận: " + uidfb);
                        }
                        catch
                        {
                        }
                        SetStatusAccount(indexRow, statusProxy + "Gửi Lời Mời OK: " + uidfb);
                    }
                    else
                    {
                        SetStatusAccount(indexRow, statusProxy + "Không tìm thấy nút kết bạn: " + uidfb);
                        chrome.DelayTime(1);
                    }
                    //delay
                    num++;
                    if (tenHanhDong == "")
                    {
                        SetStatusAccount(indexRow, statusProxy + "Đang kết bạn theo uid" + $" ({num})...");
                    }
                    else
                    {
                        SetStatusAccount(indexRow, statusProxy + "Đang" + $" {tenHanhDong} ({num})...");
                    }
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    delayFrom = ((delayFrom > 2) ? (delayFrom - 2) : 0);
                    delayTo = ((delayTo > 2) ? (delayTo - 2) : 0);
                    chrome.DelayTime(rd.Next(delayFrom, delayTo + 1));
                }
            }
            catch
            {
                num = -1;
            }

            return num;
        }
        public int HDNhiemVuClone(int indexRow, string statusProxy, Chrome chrome, int delayFrom, int delayTo, int nudRun, Random rd, string tenHanhDong = "", string id_HanhDong = "")
        {
            int num = 0;
            try
            {
                chrome.GotoURL("https://m.facebook.com/editor");
                chrome.DelayTime(2);
                if (chrome.CheckTextInChrome("Trang này hiện không tồn tại", "This Page Isn't Available Right Now"))
                {
                    SetStatusAccount(indexRow, statusProxy + "Nick bị chặn tính năng này!");
                    return -1;
                }

                SetStatusAccount(indexRow, statusProxy + "Làm " + nudRun + " nhiệm vụ.");
                int runCount = 0;
                while (runCount < nudRun)
                {
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    try
                    {
                        if (chrome.FindElementChrome(3, "/html/body/div[1]/div/div[4]/div/div[1]/div/div/div[4]/div/div/div/div/div[1]/div/div[3]/div[3]/div[3]"))
                        {
                            SetStatusAccount(indexRow, statusProxy + "Click");

                            chrome.DelayTime(1);
                            chrome.Click(3, "/html/body/div[1]/div/div[4]/div/div[1]/div/div/div[4]/div/div/div/div/div[1]/div/div[3]/div[3]/div[3]");
                            runCount++;
                        }
                        chrome.DelayTime(1);
                        chrome.GotoURL("https://m.facebook.com/editor");
                    }
                    catch { }
                    //delay
                    if (tenHanhDong == "")
                    {
                        SetStatusAccount(indexRow, statusProxy + "Đang kết bạn theo uid" + $"({runCount}/{nudRun})...");
                    }
                    else
                    {
                        SetStatusAccount(indexRow, statusProxy + "Đang" + $" {tenHanhDong} ({runCount}/{nudRun})...");
                    }
                    if (chrome.CheckChromeClosed())
                    {
                        return -2;
                    }
                    delayFrom = ((delayFrom > 2) ? (delayFrom - 2) : 0);
                    delayTo = ((delayTo > 2) ? (delayTo - 2) : 0);
                    chrome.DelayTime(rd.Next(delayFrom, delayTo + 1));
                }
            }
            catch
            {
                num = -1;
            }

            return num;
        }
        public int HDPostStatus(int indexRow, string statusProxy, Chrome chrome, string content, bool ckbPostWall, bool ckbPostImg, bool ckbTagFr, string txtPathImg, string nudPost, string nudImgPost, string nudTagFr, int delayFrom, int delayTo, bool ckbPinPost, bool ckbPostVideo, string txtPathVideo, Random rd, string id_HanhDong = "", string tenHanhDong = "")
        {
            string cUid = GetCellAccount(indexRow, "cUid");
            int num = 0;
            List<string> ListImg = Helpers.Common.GetIMGfromFolder(txtPathImg);
            int nudImg = int.Parse(nudImgPost);
            try
            {
                while (true)
                {
                    try
                    {
                        string tempPostPost = "";
                        if (chrome.CheckChromeClosed()) return -2;

                        chrome.GotoURL("https://www.facebook.com/");
                        string pageSource = chrome.GetPageSource();

                        string uidChorme = Regex.Match(pageSource, "CurrentUserInitialData\":{\"ACCOUNT_ID\":\"(\\d+)\",\"USER_ID\":\"(\\d+)\"").Groups[1].Value;
                        if (cUid != uidChorme) return -1;

                        string cookie = ConvertCookieOptimize(chrome.GetCookieFromChrome());
                        string userAgent = chrome.GetUseragent();
                        string proxy = chrome.Proxy;

                        //
                        Meta request = new Meta(cookie, userAgent, proxy);
                        string tempLogin = request.Get("https://www.facebook.com/").Result;
                        string checkUidRequest = Regex.Match(pageSource, "CurrentUserInitialData\":{\"ACCOUNT_ID\":\"(\\d+)\",\"USER_ID\":\"(\\d+)\"").Groups[1].Value;
                        if (cUid != checkUidRequest) return -1;


                        string attachments = "";
                        if (ckbPostImg)
                        {
                            SetStatusAccount(indexRow, statusProxy + "Chuẩn bị ảnh...");
                            for (int ianh = 0; ianh < nudImg; ianh++)
                            {
                                string idanh = request.UpLoadImg(Helpers.Common.GetRamdom(ListImg));
                                attachments += "{\"photo\":{\"id\":\"" + idanh + "\"}},";
                            }
                            attachments = attachments.TrimEnd(',');
                        }
                        string uidFr = "";
                        if (ckbTagFr)
                        {
                            SetStatusAccount(indexRow, statusProxy + "Lấy danh sách bạn bè...");
                            string tempFr = request.GraphApi("&variables={\"include_viewer\":false,\"options\":[\"FRIENDS_ONLY\"],\"scale\":1,\"typeahead_context\":\"mentions\",\"typeaheadFirstDegreeFilters\":[\"USER\"],\"useSections\":null}&doc_id=5606622172783175").Result;
                            MatchCollection matchFrUids = Helpers.Common.RegexMatches(tempFr, "{\"id\":\"(\\d+)");
                            List<string> list = new List<string>();
                            foreach (Match match in matchFrUids)
                            {
                                string uidFb = match.Groups[1].Value;
                                if (!string.IsNullOrEmpty(uidFb) && !uidFb.Equals(cUid) && !list.Contains("\"" + uidFb + "\""))
                                {
                                    list.Add("\"" + uidFb + "\"");
                                }
                            }
                            uidFr = string.Join(",", list.OrderBy(x => x).Take(int.Parse(nudTagFr)));
                        }

                        content = Helpers.Common.SpinText(content, rd);
                        content = GetIconFacebook.ProcessString(content, rd);

                        //post wall
                        if (ckbPostWall)
                        {
                            SetStatusAccount(indexRow, statusProxy + "Chuẩn bị đăng bài...");
                            tempPostPost = request.PostStatusGraphApi(attachments, uidFr, content, "5961636673901979").Result;
                            string uidPostSuccess = Helpers.Common.RegexMatch(tempPostPost, "wwwURL\":\"(.*?)\"").Groups[1].Value;
                            if (string.IsNullOrEmpty(uidPostSuccess))
                            {
                                uidPostSuccess = Helpers.Common.RegexMatch(tempPostPost, "post_id\":\"(\\d+)").Groups[1].Value;
                                if (!string.IsNullOrEmpty(uidPostSuccess))
                                {
                                    uidPostSuccess = "https://www.facebook.com/" + uidPostSuccess;
                                }
                                else
                                {
                                    uidPostSuccess = Helpers.Common.RegexMatch(tempPostPost, "\"story_id\":\"(.*?)\"").Groups[1].Value;
                                    if (!string.IsNullOrEmpty(uidPostSuccess))
                                    {
                                        uidPostSuccess = "https://www.facebook.com/" + Helpers.Common.Base64Decode(uidPostSuccess).Replace("S:_I" + cUid + ":", "");
                                    }
                                    else
                                    {
                                        uidPostSuccess = request.GetError(tempPostPost);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(uidPostSuccess))
                            {
                                Helpers.Common.saveActivityLog(cUid, "Đăng bài viết", uidPostSuccess);
                            }
                            else
                            {
                                uidPostSuccess = "Lỗi xảy ra";
                            }
                            SetStatusAccount(indexRow, statusProxy + "OK:" + uidPostSuccess);

                            if (ckbPinPost && string.IsNullOrEmpty(uidPostSuccess))
                            {
                                SetStatusAccount(indexRow, statusProxy + "Ghim bài: " + uidPostSuccess);
                                Task.Delay(delayFrom * 1000).Wait();

                                string storyId = Helpers.Common.RegexMatch(tempPostPost, "\"story_id\":\"(.*?)\"").Groups[1].Value;
                                if (string.IsNullOrEmpty(storyId))
                                {
                                    storyId = Helpers.Common.RegexMatch(tempPostPost, "\"story\":{\"id\":\"(.*?)\"").Groups[1].Value;
                                }
                                if (string.IsNullOrEmpty(storyId))
                                {
                                    SetStatusAccount(indexRow, statusProxy + "Không tìm thấy UID Post!");
                                }
                                else
                                {
                                    string rqPinPost = request.PostGraphApi(string.Concat(new string[]
                                    {
                                        "&variables={\"input\":{\"story_id\":\"",
                                        storyId,
                                        "\",\"actor_id\":\"",
                                        cUid,
                                        "\",\"client_mutation_id\":\"2\"},\"afterTime\":null,\"beforeTime\":null,\"displayCommentsFeedbackContext\":null,\"displayCommentsContextEnableComment\":null,\"displayCommentsContextIsAdPreview\":null,\"displayCommentsContextIsAggregatedShare\":null,\"displayCommentsContextIsStorySet\":null,\"feedLocation\":\"TIMELINE\",\"feedbackSource\":0,\"focusCommentID\":null,\"memorializedSplitTimeFilter\":null,\"postedBy\":null,\"privacy\":null,\"privacySelectorRenderLocation\":\"COMET_STREAM\",\"scale\":1,\"taggedInOnly\":null,\"omitPinnedPost\":true,\"renderLocation\":\"timeline\",\"useDefaultActor\":false,\"UFI2CommentsProvider_commentsKey\":\"ProfileCometTimelineRoute\",\"__relay_internal__pv__GroupsCometDelayCheckBlockedUsersrelayprovider\":false,\"__relay_internal__pv__IsWorkUserrelayprovider\":false,\"__relay_internal__pv__IsMergQAPollsrelayprovider\":false,\"__relay_internal__pv__StoriesRingrelayprovider\":false}&doc_id=6054548907914367"
                                    })).Result;
                                    if (!rqPinPost.Contains("{\"data\":{\"pin_profile_pinned_post\":{\"viewer\""))
                                    {
                                        SetStatusAccount(indexRow, statusProxy + "Ghim bài lỗi!");
                                    }
                                    else
                                    {
                                        SetStatusAccount(indexRow, statusProxy + "Ghim bài thành công!");
                                        Helpers.Common.saveActivityLog(cUid, "Ghim bài viết", uidPostSuccess);
                                    }
                                }
                            }


                            if (ckbPostVideo)
                            {

                            }
                        }
                        request.TryDispose();
                    }
                    catch (Exception ex)
                    {
                        SetStatusAccount(indexRow, statusProxy + "Lỗi đăng bài!");
                        num = -1;
                    }
                    goto delayTask;
                delayTask:
                    if (delayFrom > 0)
                    {
                        Task.Delay(delayFrom * 1000).Wait();
                        goto stopTask;
                    }
                stopTask:

                    break;
                }

            }
            catch { }
            return num;
        }
        public int HDBuffViewFb(int indexRow, string statusProxy, Chrome chrome, string txtLinkFacebook, int nudStart, int nudEnd, bool ckbSharePost, bool ckbLikePost, Random rd, string id_HanhDong = "", string tenHanhDong = "")
        {
            string cUid = GetCellAccount(indexRow, "cUid");
            int num = 0;
            string metaShare = "";
            bool logRq = false;
            string idPost = "";
            try
            {
                while (true)
                {
                    SetStatusAccount(indexRow, statusProxy + "View Facebook " + txtLinkFacebook);
                    chrome.GotoURL(txtLinkFacebook);
                    chrome.DelayTime(2);

                    Helpers.Common.saveActivityLog(cUid, "View Link", txtLinkFacebook);

                    //check like and post get id
                    if (ckbSharePost || ckbLikePost)
                    {
                        string cookie = ConvertCookieOptimize(chrome.GetCookieFromChrome());
                        string userAgent = chrome.GetUseragent();
                        string proxy = chrome.Proxy;

                        Meta request = new Meta(cookie, userAgent, proxy);
                        string tempLogin = request.Get("https://www.facebook.com/").Result;
                        string checkUidRequest = Regex.Match(tempLogin, "CurrentUserInitialData\":{\"ACCOUNT_ID\":\"(\\d+)\",\"USER_ID\":\"(\\d+)\"").Groups[1].Value;
                        if (cUid != checkUidRequest)
                        {
                            logRq = false;
                            SetStatusAccount(indexRow, statusProxy + "Login check false");
                        }
                        else
                        {
                            metaShare = request.getDataShare(txtLinkFacebook);
                            idPost = Regex.Match(metaShare, ":\\[(\\d+)\\]}\"").Groups[1].Value;
                            if (!string.IsNullOrEmpty(idPost))
                            {
                                logRq = true;
                                SetStatusAccount(indexRow, statusProxy + "Get Id Post OK!");
                            }
                            else
                            {
                                logRq = false;
                                SetStatusAccount(indexRow, statusProxy + "Không lấy được IdPost. Bỏ qua Share or Like!");
                            }

                        }
                        //like
                        if (!string.IsNullOrEmpty(idPost) && !Helpers.Common.getLog(cUid, "reactions", idPost))
                        {
                            if (ckbLikePost && logRq && !string.IsNullOrEmpty(idPost))
                            {
                                SetStatusAccount(indexRow, statusProxy + "Like bài viết...");
                                string dataPost = "&fb_api_req_friendly_name=CometUFIFeedbackReactMutation&variables={\"input\":{\"attribution_id_v2\":\"\",\"feedback_id\":\"{Var:feedbackId}\",\"feedback_reaction_id\":\"{Var:reactionID}\",\"feedback_source\":\"OBJECT\",\"is_tracking_encrypted\":true,\"tracking\":[\"\"],\"session_id\":\"{Var:ssid}\",\"actor_id\":\"{Var:UID}\",\"client_mutation_id\":\"2\"},\"useDefaultActor\":false,\"scale\":1}&doc_id=6324189467690403";
                                List<string> list = new List<string>
                                        {
                                            "1635855486666999",//like
                                            "1678524932434102",//love
                                            "613557422527858", //care
                                            "115940658764963", //haha
                                            "478547315650144", //wow
                                            "908563459236466", //sad
                                            "444813342392137" //angry
                                        };
                                int index = 0;
                                Random random = new Random();
                                index = random.Next(0, 5);
                                dataPost = dataPost.Replace("{Var:reactionID}", list[index]);
                                dataPost = dataPost.Replace("{Var:ssid}", Guid.NewGuid().ToString());
                                dataPost = dataPost.Replace("{Var:feedbackId}", Helpers.Common.Base64Encode("feedback:" + idPost));
                                string tempLike = request.PostGraphApi(dataPost).Result;
                                if (Helpers.Common.checkContainsOrEmpty(tempLike, "{\"data\":{\"feedback_react\":{\"feedback\":{\"can_viewer_react\":true"))
                                {
                                    SetStatusAccount(indexRow, statusProxy + "Like Thành Công!");
                                    Helpers.Common.saveActivityLog(cUid, "Like bài viết", txtLinkFacebook);
                                    Helpers.Common.saveLog(cUid, "reactions", idPost);
                                }
                                else
                                {
                                    SetStatusAccount(indexRow, statusProxy + "Like Lỗi!");
                                }
                            }
                        }
                        else
                        {
                            SetStatusAccount(indexRow, statusProxy + "Đã thả cảm xúc trước đó!");
                        }

                        //share
                        if (ckbSharePost && logRq)
                        {
                            SetStatusAccount(indexRow, statusProxy + "Share bài viết...");

                            string tempShareRq = request.PostGraphApi(string.Concat(new string[]
                                                                    {
                                                                    "&variables={\"input\":{\"attachments\":",
                                                                    metaShare,
                                                                    ",\"audiences\":{\"undirected\":{\"privacy\":{\"allow\":[],\"base_state\":\"EVERYONE\",\"deny\":[],\"tag_expansion_state\":\"UNSPECIFIED\"}}},\"is_tracking_encrypted\":true,\"navigation_data\":{\"attribution_id_v2\":\"\"},\"source\":\"www\",\"tracking\":[\"\"],\"actor_id\":\"",
                                                                    cUid,
                                                                    "\",\"client_mutation_id\":\"1\"},\"renderLocation\":\"homepage_stream\",\"scale\":1,\"privacySelectorRenderLocation\":\"COMET_STREAM\",\"useDefaultActor\":false,\"displayCommentsContextEnableComment\":null,\"feedLocation\":\"NEWSFEED\",\"displayCommentsContextIsAdPreview\":null,\"displayCommentsContextIsAggregatedShare\":null,\"displayCommentsContextIsStorySet\":null,\"displayCommentsFeedbackContext\":null,\"feedbackSource\":1,\"focusCommentID\":null,\"UFI2CommentsProvider_commentsKey\":\"CometModernHomeFeedQuery\",\"__relay_internal__pv__GroupsCometDelayCheckBlockedUsersrelayprovider\":false,\"__relay_internal__pv__IsWorkUserrelayprovider\":false,\"__relay_internal__pv__IsMergQAPollsrelayprovider\":false,\"__relay_internal__pv__StoriesArmadilloReplyEnabledrelayprovider\":false,\"__relay_internal__pv__StoriesRingrelayprovider\":false}&doc_id=8895513403852982"
                                                                    })).Result;

                            string idPostShare = Regex.Match(tempShareRq, "post_id\":\"(\\d+)\"").Groups[1].Value;
                            if (!string.IsNullOrEmpty(idPostShare))
                            {
                                string linkShare = "https://www.facebook.com/" + idPostShare;
                                SetStatusAccount(indexRow, statusProxy + "Share bài viết thành công!");
                                Helpers.Common.saveActivityLog(cUid, "Share bài viết", linkShare);
                            }
                            else
                            {
                                SetStatusAccount(indexRow, statusProxy + "Share bài viết thất bại!");
                            }
                        }
                    }

                    //click play
                    string[] xpaths =
                    {
                        "/html/body/div[1]/div/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div/div/div[1]/div/div/div/div/div/div/div[2]/div/i/div/i",
                        "/html/body/div[1]/div/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div/div/div[1]/div/div/div/div/div/div/div[2]/div/div[2]",
                        "/html/body/div[1]/div/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div/div/div[1]/div/div/div/div/div/div/div[2]/div/div[3]"
                    };

                    foreach (var xpath in xpaths)
                    {
                        if (chrome.FindElementChrome(3, xpath))
                        {
                            SetStatusAccount(indexRow, statusProxy + "Click Play!");
                            chrome.Click(3, xpath);
                            break;
                        }
                    }

                    if (isStop)
                    {
                        SetStatusAccount(indexRow, statusProxy + "Đã dừng!");
                        break;
                    }

                    int randomtime = rd.Next(nudStart, nudEnd + 1);
                    if (randomtime > 0)
                    {
                        int timeConvert = randomtime * 60;
                        int tickCount = Environment.TickCount;
                        while ((Environment.TickCount - tickCount) / 1000 - timeConvert < 0 && !isStop)
                        {
                            SetStatusAccount(indexRow, statusProxy + string.Format("Đang treo dừng sau {{time}}s...", tenHanhDong).Replace("{time}", (timeConvert - (Environment.TickCount - tickCount) / 1000).ToString()));
                            Helpers.Common.DelayTime(0.5);
                        }
                    }
                    SetStatusAccount(indexRow, statusProxy + "Chạy xong!");
                    break;
                }
            }
            catch
            {
                num = 0;
            }


            return num;
        }
        public string RunCMD(string cmd)
        {
            Process cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.Arguments = "/c " + cmd;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit();
            bool flag = string.IsNullOrEmpty(output);
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                result = output;
            }
            return result;
        }

        public void ClosePopup(Chrome chrome)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    switch (chrome.CheckExistElements(5.0, "[aria-label=\"Done\"]", "[aria-label=\"Xong\"]", "body > div > div > div > div > div > div > div > div > div > div > div > div:nth-child(5) > div", "[data-visualcompletion=\"ignore\"] [role=\"dialog\"] [role=\"button\"]", "[style=\"transform: translateX(0%) translateZ(1px);\"]>div>div:nth-child(2)>div:nth-child(3)>div", "[style=\"transform: translate(16px, 0px);\"] [role=\"button\"]", "[style=\"padding-top: 40px;\"]>div>div>[role=\"button\"]", "[style=\"transform: translateX(0%) translateZ(1px);\"] [role=\"button\"]", "[role=\"dialog\"]>div>div>div:nth-child(3)>div", "[role=\"dialog\"] [style*=\"transform: translate\"]>div>div>div [role=\"button\"]"))
                    {
                        default:
                            return;
                        case 1:
                            chrome.ScrollSmooth("document.querySelector('[aria-label=\"Done\"]')");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "[aria-label=\"Done\"]");
                            break;
                        case 2:
                            chrome.ScrollSmooth("document.querySelector('[aria-label=\"Xong\"]')");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "[aria-label=\"Xong\"]");
                            break;
                        case 3:
                            chrome.ScrollSmooth("document.querySelector('body > div > div > div > div > div > div > div > div > div > div > div > div:nth-child(5) > div')");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "body > div > div > div > div > div > div > div > div > div > div > div > div:nth-child(5) > div");
                            chrome.DelayTime(1.0);
                            if (chrome.CheckExistElement("body > div > div > div > div > div > div > div > div > div > div:nth-child(2) > div > div > div:nth-child(3) > div", 10.0) == 1)
                            {
                                chrome.Click(4, "body > div > div > div > div > div > div > div > div > div > div:nth-child(2) > div > div > div:nth-child(3) > div");
                                chrome.DelayTime(1.0);
                                if (chrome.CheckExistElement("[data-visualcompletion=\"ignore\"] [role=\"dialog\"] [role=\"button\"]", 10.0) == 1)
                                {
                                    chrome.Click(4, "[data-visualcompletion=\"ignore\"] [role=\"dialog\"] [role=\"button\"]");
                                    chrome.DelayTime(1.0);
                                }
                            }
                            break;
                        case 4:
                            chrome.ExecuteScript("document.querySelector('[data-visualcompletion=\"ignore\"] [role=\"dialog\"] [role=\"button\"]').click()");
                            chrome.DelayTime(1.0);
                            break;
                        case 5:
                            chrome.ScrollSmooth("document.querySelector('[method=\"POST\"]>div>div:nth-child(2)>div>div>div:nth-child(2)>div:nth-child(3) [role=\"button\"]')");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "[method=\"POST\"]>div>div:nth-child(2)>div>div>div:nth-child(2)>div:nth-child(3) [role=\"button\"]");
                            chrome.DelayTime(1.0);
                            break;
                        case 6:
                            chrome.ScrollSmooth("document.querySelector('[style=\"transform: translate(16px, 0px);\"] [role=\"button\"]')");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "[style=\"transform: translate(16px, 0px);\"] [role=\"button\"]");
                            chrome.DelayTime(1.0);
                            break;
                        case 7:
                            chrome.Click(4, "[style=\"padding-top: 40px;\"]>div>div>[role=\"button\"]");
                            chrome.DelayTime(1.0);
                            break;
                        case 8:
                            chrome.ExecuteScript("document.querySelector('[style=\"transform: translateX(0%) translateZ(1px);\"] [role=\"button\"]').click()");
                            chrome.DelayTime(1.0);
                            break;
                        case 9:
                            chrome.ExecuteScript("document.querySelector('[role=\"dialog\"]>div>div>div:nth-child(3)>div').click()");
                            chrome.DelayRandom(2, 4);
                            break;
                        case 10:
                            chrome.ExecuteScript("document.querySelector('[role=\"dialog\"] [style*=\"transform: translate\"]>div>div>div [role=\"button\"]').click()");
                            chrome.DelayTime(1.0);
                            break;
                    }
                    chrome.DelayTime(1.0);
                }
            }
            catch
            {
            }
        }
        private bool LogoutOldDevice(Chrome chrome)
        {
            bool result = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/settings/security_login/sessions/log_out_all/confirm/");
                chrome.DelayTime(1.0);
                string text = "";
                for (int i = 0; i < 10; i++)
                {
                    text = chrome.ExecuteScript("return document.documentElement.innerHTML.match(new RegExp('/security/settings/sessions/log_out_all/(.*?)\"'))[0].replace('\"','').split('amp;').join('');").ToString();
                    if (text != "")
                    {
                        chrome.GotoURL("https://m.facebook.com" + text);
                        result = true;
                        break;
                    }
                    chrome.DelayTime(1.0);
                }
            }
            catch
            {
            }
            return result;
        }
        private bool AllowFollow(Chrome chrome)
        {
            bool result = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/settings/subscribe/");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("[data-sigil=\"audience-options-list\"]>label", 10.0) == 1 && !Convert.ToBoolean(chrome.ExecuteScript("return document.querySelector('[data-sigil=\"audience-options-list\"]>label').getAttribute('data-sigil').includes('selected')+''")) && chrome.Click(4, "[data-sigil=\"audience-options-list\"]>label") == 1)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        private bool ReviewTag(Chrome chrome)
        {
            bool result = true;
            try
            {
                CommonChrome.GoToSetting_TimelineAndTagging(chrome);
                string cssSelector = chrome.GetCssSelector("a", "href", "/privacy/touch/tags/review/");
                if (cssSelector != "" && chrome.Click(4, cssSelector) == 1)
                {
                    chrome.DelayThaoTacNho();
                    if (!Convert.ToBoolean(chrome.ExecuteScript("return document.querySelector('input[type=\"checkbox\"]').checked+''").ToString()))
                    {
                        chrome.Click(4, "form div[role=\"button\"]");
                    }
                    return true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private int BatThongBaoDangNhap(Chrome chrome, int indexRow)
        {
            int result = 1;
            string cellAccount = GetCellAccount(indexRow, "cPassword");
            try
            {
                chrome.GotoURL("https://m.facebook.com/login_alerts/settings/");
                DelayThaoTacNho();
                for (int i = 0; i < 2; i++)
                {
                    if (chrome.CheckExistElement("article [data-sigil=\"touchable\"] a", 5.0) != 1)
                    {
                        continue;
                    }
                    chrome.Click(4, "article [data-sigil=\"touchable\"] a", i);
                    chrome.DelayTime(1.0);
                    if (chrome.CheckExistElement("fieldset label:nth-child(1) [checked=\"1\"]", 5.0) == 1)
                    {
                        chrome.Click(4, "fieldset label:nth-child(1)");
                        chrome.DelayTime(1.0);
                        chrome.Click(4, "[type=\"submit\"]");
                        chrome.DelayTime(1.0);
                        if (chrome.CheckExistElement("[type=\"password\"]", 5.0) == 1)
                        {
                            chrome.SendKeys(4, "[type=\"password\"]", cellAccount);
                            DelayThaoTacNho();
                            chrome.Click(4, "[type=\"submit\"]");
                            DelayThaoTacNho();
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        private void UpdateInfoWhenInteracting(Chrome chrome, int row)
        {
            try
            {
                string cellAccount = GetCellAccount(row, "cId");
                string infoAccountFromUidUsingCookie = CommonChrome.GetInfoAccountFromUidUsingCookie(chrome);
                if (!infoAccountFromUidUsingCookie.Contains("|"))
                {
                    if (infoAccountFromUidUsingCookie == "-1")
                    {
                        SetInfoAccount(cellAccount, row, "Die");
                    }
                    return;
                }
                string[] array = infoAccountFromUidUsingCookie.Split('|');
                CommonSQL.UpdateMultiFieldToAccount(cellAccount, "name|gender|birthday|friends|groups|dateCreateAcc|follow" + ((array[5] != "") ? "|email" : ""), array[1] + "|" + array[2] + "|" + array[3] + "|" + array[6] + "|" + array[7] + "|" + array[9] + "|" + array[10] + ((array[5] != "") ? ("|" + array[5]) : ""), isAllowEmptyValue: false);
                SetCellAccount(row, "cName", array[1], isAllowEmptyValue: false);
                SetCellAccount(row, "cGender", array[2], isAllowEmptyValue: false);
                SetCellAccount(row, "cBirthday", array[3], isAllowEmptyValue: false);
                SetCellAccount(row, "cEmail", array[5], isAllowEmptyValue: false);
                SetCellAccount(row, "cFriend", array[6], isAllowEmptyValue: false);
                SetCellAccount(row, "cGroup", array[7], isAllowEmptyValue: false);
                SetCellAccount(row, "cDateCreateAcc", array[9], isAllowEmptyValue: false);
                SetCellAccount(row, "cFollow", array[10], isAllowEmptyValue: false);
                SetInfoAccount(cellAccount, row, "Live");
            }
            catch (Exception ex)
            {
                CommonCSharp.ExportError(null, ex.ToString());
            }
        }
        //private bool CreateShortcutChrome(string profilePath, string nameShortcut)
        //{
        //    try
        //    {
        //        nameShortcut = Helpers.Common.ConvertToUnSign(nameShortcut);
        //        if (profilePath.StartsWith("profiles\\"))
        //        {
        //            profilePath = Application.StartupPath + "\\" + profilePath;
        //        }
        //        string path = Application.StartupPath + "\\images\\chrome.ico";
        //        if (!File.Exists(path))
        //        {
        //            using (FileStream outputStream = new FileStream(path, FileMode.Create))
        //            {
        //                Resources.chrome.Save(outputStream);
        //            }
        //        }
        //        if (Helpers.Common.CreateShortcutChrome(nameShortcut, setting_InteractGeneral.GetValue("txtPathShortcut"), profilePath, path, setting_InteractGeneral.GetValue("txtPathChromeOrigin")))
        //        {
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return false;
        //}
        private void closeChorme_Click(object sender, EventArgs e)
        {
            bool areyouok = MessageBoxHelper.ShowMessageBoxWithQuestion("Đồng ý để tắt tất cả chrome đang bật!") == DialogResult.Yes;
            if (areyouok)
            {
                CloseProcess("chromedriver");
                CloseProcess("Chrome");
            }
        }
        public void CloseProcess(string nameProcess)
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName(nameProcess);
                foreach (Process process in processesByName)
                {
                    process.Kill();
                }
            }
            catch
            {
                MessageBox.Show("Lỗi");
            }
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
        private void quảnLýKịchBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kickBan = "";
            Helpers.Common.ShowForm(new fDanhSachKichBan(kickBan));
        }
        private void checkTokenLiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần kiểm tra!"), 3);
            }
            else
            {
                checkLiveTokenAndCookie();
            }
        }
        private void checkLiveTokenAndCookie()
        {
            LoadSetting();
            int iThread = 0;
            int maxThread = setting_general.GetValueInt("nudHideThread", 10);
            isStop = false;
            cControl("start");
            switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
            {
                case 1:
                    {
                        listProxyShopLike = setting_general.GetValueList("txtApiShopLike");

                        if (listProxyShopLike.Count == 0)
                        {
                            MessageBoxHelper.ShowMessageBox(("Key ShopLike không đủ, vui lòng mua thêm!"), 2);
                            return;
                        }

                        listShopLike = new List<ShopLike>();
                        for (int i = 0; i < listProxyShopLike.Count; i++)
                        {
                            ShopLike item = new ShopLike(listProxyShopLike[i], 0, setting_general.GetValueInt("nudLuongPerIPShopLike"), setting_general.GetValue("cbbLocationShopLikePrx"));
                            listShopLike.Add(item);
                        }

                        if (listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike") < maxThread)
                        {
                            maxThread = listProxyShopLike.Count * setting_general.GetValueInt("nudLuongPerIPShopLike");
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
            new Thread((ThreadStart)delegate
            {
                cControl("start");
                int num = 0;
                while (num < dtgvAcc.Rows.Count)
                {
                    Application.DoEvents();
                    if (isStop)
                    {
                        break;
                    }
                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                SetStatusAccount(row, ("Đang kiểm tra..."));
                                CheckTokenAndCookieOneThread(row);
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
                int tickCount = Environment.TickCount;
                while (iThread > 0 && Environment.TickCount - tickCount <= 30000)
                {
                    Application.DoEvents();
                    Thread.Sleep(300);
                }
                cControl("stop");
            }).Start();
        }
        private void CheckTokenAndCookieOneThread(int indexRow)
        {
            string text = "";
            int num = 0;
            int num3 = 0;
            bool flag = false;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            MinProxy minProxy = null;
            TinsoftProxy tinsoftProxy = null;
            string text4 = "";
            string text5 = GetCellAccount(indexRow, "cUid");
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cEmail");
            string cellAccount3 = GetCellAccount(indexRow, "cPassMail");
            string cellAccount4 = GetCellAccount(indexRow, "cFa2");
            string cellAccount5 = GetCellAccount(indexRow, "cPassword");
            string cellAccount6 = GetCellAccount(indexRow, "cCookies");
            string cellAccount7 = GetCellAccount(indexRow, "cToken");
            string text6 = GetCellAccount(indexRow, "cUseragent");
            string msg = "";
            if (text5 == "")
            {
                text5 = Regex.Match(cellAccount6, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        //get proxy shoplike
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag12 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + ("Đã dừng!"));
                                    break;
                                }
                                bool flag13 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag13 = false;
                                    }
                                }
                                if (!flag13)
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                                    goto stopBreak;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    goto stopBreak;
                                }
                                try
                                {
                                    lock (lock_checkDelayRequest)
                                    {
                                        if (checkDelayRequest > 0)
                                        {
                                            //default delay 1-2 second
                                            int num5 = rd.Next(1, 2 + 1);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num5 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + ("Chạy tiến trình sau") + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        goto stopBreak;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayRequest++;
                                        }
                                    }
                                    SetStatusAccount(indexRow, text2 + ("Đang check..."));
                                    if (text6 == "")
                                    {
                                        text6 = Base.useragentDefault;
                                    }
                                    if (isStop)
                                    {
                                        goto stopBreak;
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            string checkProxy = Helpers.Common.CheckProxy(text, 0);
                                            if (checkProxy == "")
                                            {
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        string proxy = text; //typeProxy = 0
                                        string userAgent = text6;
                                        string cookie = cellAccount6;
                                        string token = cellAccount7;
                                        if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                                        {
                                            SetStatusAccount(indexRow, text2 + ("Cookie die!"));
                                            SetCellAccount(indexRow, "cCookies", "");
                                            SetCellAccount(indexRow, "cToken", "");
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", "");
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "token", "");
                                            break;
                                        }
                                        else
                                        {
                                            if (token != "")
                                            {
                                                bool checkLiveToken = CommonRequest.CheckLiveToken(cookie, token, userAgent, proxy, typeProxy);
                                                if (checkLiveToken)
                                                {
                                                    //process in here
                                                    SetStatusAccount(indexRow, text2 + ("Cookie live/Token Live"));
                                                    break;
                                                }
                                                else
                                                {
                                                    SetCellAccount(indexRow, "cToken", "");
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "token", "");
                                                    SetStatusAccount(indexRow, text2 + ("Cookie live/Token die"));
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, text2 + ("Cookie live/Không có token!"));
                                                break;
                                            }

                                        }

                                    }

                                }
                                catch (Exception ex)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
                                    Helpers.Common.ExportError(null, ex);
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
                                break;
                            }
                        stopBreak:
                            SetStatusAccount(indexRow, text2 + "Đã dừng!");
                            break;
                    }
                    break;
                }
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            //string ss = Facebook_Tool_Request.Helpers.Common.GetVersionChromeEXE();
            string diskStr = this.RunCMD("wmic diskdrive get serialNumber");
            string biosStr = this.RunCMD("wmic bios get serialnumber");

            int a = 0;
            //string txtCaptchaKey = "sub_1OPgq1CRwBwvt6ptW5FX94uX";
            //string code = "";
            //string ImagesBase64 = "iVBORw0KGgoAAAANSUhEAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAACeNSURBVHhe7d0FkCu70QXgF2ZmZmbmvDAzMycvzMzMzEwvzMzMzMzMzKy/PtUqv6LbY4/HY6/97nSVane99oBGOn36dEveJ0022WSTbbhtBFD99Kc/TX/60592/ppsss2x73znO+mZz3xmutvd7paufe1rp/Oe97zplKc8ZTrxiU+crnSlK6U3v/nN6be//W3697//nf7zn//sfGqysW3XgerHP/5xuuUtb5mufOUrp8c+9rHpk5/85ARak+2qfe5zn0v3ute90iUvecl05jOfOYPTZS972QxMD3jAA9KnP/3pDEyTLWd/+9vfdn6bb7sOVEDpgQ98YDr2sY+d9tlnn3Se85wnPfGJT0wf+MAH0g9/+MP017/+dfJUk63EgM2//vWv9Mc//jGPt5vc5CaZKR3zmMdMF7rQhdKd7nSn9MhHPjK99rWvTd/73vd2PjXZGPajH/0oPfWpT81stI9tROj3i1/8Ir3kJS9JV7nKVdLJT37ydLjDHS6D1jnOcY70whe+MH37299Of/7znycvNtnSxov/+te/ziHdm970pnSNa1wjHepQh0qHOcxh0kUvetHsJN/2trelz372s+lXv/rVzqcmG9P064UvfOE8x5/ylKfsvDrbNk5MdxMvetGL0rnPfe48gA5+8IP/F7Se97zn5VBxsskWMaycs/vgBz+YHd8Vr3jFdNCDHjQd/vCHTxe72MXSy1/+8vStb30r/fznP09/+ctfdj412Srs97//fbrrXe+a5/TRjna09Mtf/nLnP7Nt44Cqtp/97GfpFa94RQYtN3awgx0sg9c5z3nO9LSnPW0Crck67e9//3tmRcbPIx7xiKw3HeUoR0nHPe5x06Uudan0lre8JU+af/7zn5O0sCbT3w9/+MPzXBZef+pTn9r5z3zbaKBiNIQ//OEPmaq/+tWvzoMMYGkQmab1hCc8IetZe5uZZNo//vGP/FNf7c2Tzv1//OMfz+PhVre6VR4rZzjDGdJpT3vadLWrXS29613vWkjAnWw8E24/7GEPyyB1rGMdK73jHe/Y+U8/23igqo2XpGd96UtfynRdFuaQhzxkDg9PeMIT5rj3gApaJiGPhCr/5Cc/Sd/4xjfShz70oaynvO51r8s/P/zhD2dAl6AAXnsLcGFO97znPdPlLne5LIKf//znT5e4xCXSHe94x/Tud78798Vku2fm4z3ucY8MUsc//vHTG97whp3/9LetAqragJbMwcc+9rH0ghe8IHtMgughDnGIdJrTnCank5/0pCfl92yzAafvfve7OVS5853vnBnkYQ972PzQuxrwPutZz5puf/vbp7e//e05qzW26f+vfe1r6X3ve18GSM/hE5/4RE7dA46vfOUr2amsyr75zW+me9/73uniF794OuMZz5gucpGLpFvf+tbp8Y9/fPrIRz4yJV42wDjJz3/+8+mGN7xhHpenPvWp01vf+tad/y5mWwtUtQl7pI/f85735JSn7CHQMmHPfvaz50K9Zz3rWZmJbIsBFxP+vve9b06Zt2DUtx3oQAdKJzrRifKk/v73v79z9OUNs3NtnIJEBx0Rk7ngBS+YQePSl750evSjH73z7nGM2C0rp65Jnxj45zvf+XKId/e73z295jWvyYBJ+wCUQH6y3TEh9utf//q077775nHomX30ox/d+e/idoAAqtqEOzI8b3zjG3MNDGZ16EMfOrMQk0hxqVKIvtmG3TAZqIc+9KHpqEc96h7AM7Qd+MAHztrAQx7ykFHS7lYTXPe61w3PVRpt6Itf/OLOJ4YZwAZAWNORj3zkDIxA17E5I+K412hR3nOWs5wls0nXJjSebP0mAhCKl/Er2iFJLGMHOKCqDf2n5dCz7nOf+2RPj2UZ8PQMcTNA+93vfrfzid0116vwEDNpJ31Xw5iAkJ/R/9smCeH4y3g3ht3c6EY3Cs9RGiCRii7m/uhFvK0yALVx9DQ//a2MgKPxGlZ0i1vcIidM1NVxMPoGK1ZmcLrTnS48Z2knO9nJMsAVE4ZorqH8rk02nmGwEl5l/Hp2imbHWGlygAaq2gzQL3/5y+k5z3lOut3tbpfOda5z5Ul7vOMdL3tfKWwTYbfqaExQVNn1tJOubhIHAOAIRzhCZhOnP/3pc+iFTfibRge4os/WTThoIuuXIUZ/KtrDrAYwnvvc5+b6JY2eqB5OKC5MpylJWVudYFDf4AY3yJ7YfQrbMSepbGxQ35R1dp5ddL7SfAa4mTicEVlAWEgz4bx+8IMfZLBVGY21bUKY+Jvf/CY7VYzfNb/3ve/NS8qMW9c69Fmt2oxdWjAHIjQ3nzixV77ylTvvWN72GqCqTcd+5jOfSY973OPyxDAZDG4/b3Ob26SnP/3p6Qtf+ELWvtZlxHKTsZ1wpWGCJzjBCdJNb3rTnOXrEqpNOJkuGS8ZluhYpR3nOMfJdUZdEwDTlGF1PqllWh/xmiaEnc4D1dIAp3AMC6IrWX1w0pOe9H9C2yMe8Yj5HgHwFa5whfxcJAMsBqaFATLXsN9+++V6qPr4UXOsk5zkJDkkBMr69khHOlKWAYBcYaCuDbgf4xjHSF/96ld3FQw8O+tdPbdTnepUeZ0hh3qBC1wgP3cAu4mGmZJS6MSryrDulUBVmzCEJ6B7WBhdJjc96373u1968YtfPKoIHRkwwBTqiVY3k0zMrwC2r5lwaleAMaYVHVfDeDBJ4C1jBxBkFgEJ5uLcwAU4Xeta18pAIWQW0gGf6Jht8756kgnxZAytQNDP1nkSW5VY+B9z/W2Y6G9sWEV5dJ66ua/9998/H0fooRXmhLkAeiyAHsgp6QPH3+1wkKan1Ca6JyA2Rhi1jbbXA1VtBjLKbXGqmiwe2AC55jWvmZ785CfnIjWTf0wzaaw3s6SDZxfW0WRMRiyDOPz+979/592LGwCyGwBAaAd+aac4xSkyULpf2TtsDKvEpKSThSAykJIUJjjwEIrc/OY33+NYGBEGU7/mPuw6QMQX3grxMBhgqOSir9BKn9If9bG7wlxAJYyaZUWnAma7yaRq87yEq8InRaucgyz2Va961XT/+98/A/w6jePoAkclKsJp42vVADoBVYeZiDSVy1/+8nlyWLpjwplkL3vZy7LWUbz/UCsggq1gcze72c1ymv1Rj3pU1nDUQI2lmQEJx6QrWXxL28JkgKOJLcwATphPn0HneBFQne1sZ0t3uMMd9ngdU73+9a//XzCkU2FJixgdsTiP0pyv7LxRNyBI69lGMy7WXbTMAXNML33pS7O2RD6QNTVmzAPsU4JDGFqc6HWuc50cjWCprnmVthFAVTzbppp0K9ptAasJYCIQegEKBkaYHRKb08AMSKl+MT5wxFj87efYXt41AlcAodGgnFdo63zug1ZG5LaWkn6loJMXN5Dr6+FNhWEtQFhTB2CxtPZ/NDbHG2Imgz6vj4dNPehBD8rV6PXrmuckbD+gmP7G+DmRIWNtlnE6j3nMYzLgY/Z1P/qb86z7npRgM0HjZV067sYwKjdsEhUNwSTycIoBMmv+TGbNZBYKeG1Mc166BcZk3RhqK+zBNGRfTF6sBCvASojKr3rVqxYucXA/PmfyCvfqwaEBwUUZRx/jARXhmcgWd1/mMpfJuhOhmgcFnIBZMxABGA0NA8KUrn71q+cwRG0M7aq9bvoWz4yJ1q8Tr/WZyTbEsL22rkzfv/Od79zjXBohnQa2rWZ8GOfCV/eIxdNM9TthfdnyktqMaYmVtg85AqU8fse8sSgAZUysm1jsClDROpQJyOxIqx/96EfPBZkatDaReGux+te//vUMXoRkFJSmcZe73CVPGlmhoSX5tQEEQrIHZjCYjCYVViD7wmPTC1yzdHF5SCX0A7Kuk5aDMVjCYWeHWYyIZ3SOdnBo6PXQCd3Hbnvb2+ZQ1rl4TBkyfS/EtRymNf0jI4YpPf/5z88gKvVPB2qvXUYOWEXFqkIGSQF9JIygTeknrI7DEeZGIYR+VJvT1oo94xnPyCGJKvj6dU0iwHjZVjPGMN2ImWqWCy3qHCPjiGVyo3NoyjxkTjmqsUnBIrZWoDIo1dQotqRZFH1kVpN1KjstRkV+towdaig04MEmapFWDQ/GYWGrlLxqbjG7gSMrA4iAEmGYZ7PGTdaw3vSP57MzpKUckc4k03SQgxzkv+cszWv24V6lAR0Ooj23JgToE1oAWpkyDqOeTPqObqTfMLb62KXRmXjqUipQC+IADlOl22F6+h6wtpk+wOU+TB7vrf+ncXhqtrbd6GwR6EtG0I+WMcxI4qQ9toblE/FpVnVks1u2FqDiJU1wA6948jGaSa3ifIgJL5/97GenM53pTPlYmICJJQMnQyUM4unpM8BMcSTxUEhRX4O4np5TwMiukRgRIMZWTEj6FsZG5+EpNROwPk5pjr/qDAq78Y1vvIceoRmcfep13K9nqh4NCyt1W0ALkDN92AIM9qy/MEZhJrAB9MIbYQ59TP9jbEJMa/naY2gci3Pr06JRGQ+YlFIHY41z2XbD1gE1VgN8OULApZ858KGlM1iyuRP1rdf8b93h3SxbOVC5WZN8Vp3Q0GZgCiUWNeGCIkKhC5Hwete7Xg4jaFCK1oQl2JCyhFJXNathEQRHIWoxA0g62cQpg0HWy+QR8hDm2+NoGOKqMyhMWO3a2vO73z57BRFgsUg6kRCLjocJYr5l8TegBxjtOQDJLBFWqIfVmUxYQ63hAVfOBWMFcLw9ZmtiqXAHdptaGDmW6R99g032FbONKc4BwyeZALqINIhysOGxss1j2cqBiqAZCXV9WleNTGmASuVyXwOa4np02sPg2ellhGMis4EujFChzoOpPSLUiuFVCANblDu6FmBl4rYDx98ygxgAT0hnUdAXsRkNG1mHJzPIu9YUEkznmaUnPL33uyeLvzErwAHshbYcAt1P39THB9zGBWA3eWh9Jdx0XUCe08CYZCH9NMGwUuBE3MfEasewyQZY3Jc+M/7KPQ9xSMZG3/Fh7HkG+o3kUhyTeVM/j7qJKvT3ptlKgYqn7QIpXlI9D3AgiAsjZJk0HSU2JqjTpWgZUecCMuJ6XzMw6GS8v7DDxAIuhfUo8sSsZP0i87r/C+2iPaFkRYR+XWYPJd5M+NN+VqPX9P1WjjEMY4yuQ0gxT8x3na2+gY3JTslWcSCyo9Z8lTFAtKdP0R0lTLBHTRisdsc5y0T0rEw0zGERA14Yq74W3s8z53JejIxTAriKLIWers1WNV3joY8ZZ1ihe5SoEQHoNzVofi6iR+oLwMMJ2K1U+QVN0c/6eXEUohhhtPC5PB/9j0VZ+mSsltdL40CX1b1WZSsDKiBAFG07Q7OIFpvhGXma1kPwQAaphoLKTMgQtuAAvIi5fcw5nAsISnWX9X0aoLKYsq+HMygsI4nASlgyjzYTedtFtVgJAX+RZTLLmolTxH+NBiJcxTDnGWYA0Op70LBQfW0XRzVVwjzZTcDkGVrSw1GMZZgY1ua6bY1jaQ+dkZ6oTKKvCclluNr70axUGPJcsCaONAqxvNbHIdQGzEsdnxDN+PP8AIxjYakAtuzH5n0cg/ENgGVtSRrR0idkAEPetJCv2EqAihhMQ2g7Q5NJky1a1IitLfBhVBYR9zEMgC7Vhlx0KnrNosa7le1V68ZrzTqeSSy9336uNCBOc6EBrdr233//nKnEEGk7AEpIheHOW9YSAZWEhEnN0azChH9tSp7zUdVfX4cGdDikvsZJ0ddM2PZYXlMV3ycbWpsdZqOMnXFrFQJ2tKhh5NHOEQCrLBI3BjkM37ajpAbDZYDT9t2RpKLEQ7i9qbYSoEIf0fu2M3hWGsMQs96tTanrcHsWzTPgoLiy/qxmYtljfagZAFGan6bVZSZxxEQ0GpgBhymaHLSiMsiKjQkCwmAhNxDnPITakguAy7Yos0xYhcUo+pQkUNsGGMZiS+5T+GJC62N9oz9aXQpwmXxtX5q0wvRFzLGtsYuAAJNZZNsSjLLNEJcm5Ba+zTLrKUuYRz+VeaZf6oO6lKZu5pxw1X3TXlsTftojqv2cjQdJLZtsowOVh416tp0BFJZZ0iDVXUoJSiueaZ554AZg/VmhFi+yqJeszWeFsPVxNXoXTx8Z0Ozax+nBD35w1ujoIgDL/blOdV4Ay2exHuGC4wtHDEiD2SCWrUPt9TNP6ssdhVqYgkHqOP5fFla7fu/jeTGrInoLASUAZhnvbPLQSobW2QgzIk3OPQrNhTbYgWsywaJiVEAVjTf3sAijKkbbEq62x9OUQVibOc+sagD4nl17DNneumzCM/U89CUHL/yiX4kU6HtC5vY4XWI4oKIRRqbeLyrQFUJyOIBxk210oKILGPRth9AnFJgNNR3dehITWagxzwwCQFl/Vmgwb3V9HyNc1sfVeGQFoV1GrG0HXwEkg5YAK9Q1WGkZBqZ7dVw/Da4ijJZm1wJaBL2iJB+817FNcoNf9sfELlqc7FpUlUzsx0DHNiyJ03BeJQSAmbModVfFaGdtMTAWGoXDHGMtGJcGqDiRIUbLKVpQ2ziZWSK9MW4/rQikPAegwFlgoBwvMDYWJSKwwLJkpW1e93nPlw4VFbk6J5BtWThnQC+MAA67s6pi0210oFKs13ZGCWOWMSFjuyhV/G9R6izDGniZ+nOakKUNI4YYPa71VACCptGaiYqJYDAFROpWD27/N1kMeu+vRe+6YW92PgB+NDj9DGR4bcCkYBVLMHjbsBF7i0oUZIRWEQpYnxmBND1T4oQB6jZkAsItmBXDLC15qt+vyTTKeg01+lLr3DT9bc+uyNSPAbKo9ARbdTyfL8/eOk8MK1ozqQEymXHZaMzYcyz9ZBmSkpn2MxwYoC/mmcusR+NHyEcP3QYbFah4PMyp7RACcUTbFzHZuvrBKFhTDzUv7HBN5SukS+NZiPrMg8QwhjZAVb5HvzQTsYj8BpZQzSSls0knd3lNnrWtOXKfMlgAmROoJzEmJeMFcAEggd9PfeK8LTC1RqOK9qmSKRoCVEK2WdvfuD7g0Z6PIyshlQlZwr3S6JBdbFzfyvLV79cAldB3qLlWmp0+bo/tmVjz6JqFl8oasFmAE+lbAMqzpaUpTwCsRcOtWQ4HQaIANJ61sLpNHtRmG+d2vGjYmaJlJmOOXbbvMUZJDF3PatNsVKCilUQV1x5QS0cXNRkJi10dz1pBk8yksKHdLBNqSM3W12PwyRQRkj1QoSHGVnZLEGZqvLgGZGlkGmGzbkK0drIbfAYjYdnEK2sUeVI6R1exJ0DiKYWTwIdugBFqQJGeYWICvKLXCZEMbsxpHjC1hlERztvrUNTaRzh2LSYTFicpgS0BjVnlDZ6HSd2e044AQpS6bEQDWoR+9x6Za4jqwSxtwoqWMc8v6p/SPE9AgfFxxtF7NMBRA557AmiACcgpxiQVGF/C4sKa5pn3ttqr5rrMOfql3TEKg6tbGTPbYqMClYmrorvtFFmhoaYgTsgnrEGDrVEzcT1UIZcHMctM+q56rlU3tSxAScOigKN6LeFcpBfwtn3rWExqIZ7dNLHYIXU+BmokHAtJ5tUgAU5aS6vlAOVZQMWDR+UZWJXi21abMnZmOTmM1iqD+jMaxqKvFzEAgblzLuqfMCq6aHtNmudH6MY86aRRmKgBMuOUY7ETh2ePjQnjhK2AdhkhW4Fxu6OqZswR4yOQ8swW/Ur13bZRgYpX4D3ajumTmesymgUBUopatTOPjG6X6m4MbpapB+pa9rLKJp2OlZlkrYc0UaMQwYBfpOAOgxJSmqxS0pY/oPk0iVkhQzEMMir+M/AtXZllJhhgaT/LUc0CKoYJCIXazwKr+m9gYEzNYoruHROtP6cBqj6lJ/pPAS4pADPGjNw//Qa7I2BjQ5Fj0Yjh9VrEuhH5OVg6kHG7ChOiSsJE5+9qohD9tk02KlCh6O1WsR4wWj/USrjDA8lOEDIJko4tzp5Vs8RMmnYCrKOpGcM6IuOFI6ASFnaVNcwzekNZFqF/ZJCEZLM0PBOo1ryEZPQgr1tQPMtMELpZ+Wxp2Ou8YlGsypKV9rNtM9HnMUXsOtJFARVmxIC/6y0/FY5yfpyq5r3Ct8I+MCjMEGPicKJnNa+5dmN2HUYWsTg8uo62YZ/ruq4xbVSgovO02QXUV7i2rJlwNu+KQMdgQnPVBNF4hDQyMNLIdIZWnI0a0NMM1tKAbN0M2HrQ+h3Frj9TPieb1sWOZOOiazJhSo3TosZD6h9MpeghnAaW1cWO1FsJraTxORlLmgx6E3leRhTYRFvVYFTC7S4rWpPJAhzaz5emX7tWMAAbQEpjozHSLNvPC7UxeaERtlm2SvFsjMlIhK6bUM77PUcOxHYzSgM83+j9dQNwfeqtxjJ9KgyPlnTVDSiTZ7bRRgUqA7xlVCYzYXlZ41lbLQKwyMCYcDJ7BpQBVnQhrbCvqAEGVN9xUXRAp4aGTkGIRNeJ9sIDEwM7Q5sNCA/dUhnX5X+yi0BG8/usWhuaRLQoVCPwDzXswldOlTo2yQdgIDSxlQ3QrsMo4aHrB5wyZDKVPLP9s7rYoNc5DSFtxIqcm64osaIfNfeEDWNafnIkXld24Rm1x9D0DyFbyK/8AkP1t1BfTZDmdwDStci7bgAmYkb0TyE3dujZkhVk06ybA5RAV78y1zxLXNeEjJIdXeL/qsy4o6lF16QBaBlX0ck22qhApVCR16k7yAAR+y9r0tPR3tgGMiMumzyABZvyO8DAEFqNyoA10E04g5CGZAKaxAZYabUpUCSEYiwqiOsQrf5M3WaZUgtAW1+Xht0sWyWMZUgyYBrYhKxlqxvRiRRc6lM7ctZisPBNgajtl60tIyxrnIFCRb/bCrdkYeuGqQAr5QHzWMsyzXMojIfDid6D3RuPygbcC9ZhPBgjJnZxRMDa+JkVJhdTk9flZDjpedreKk0WnOOOrs14MD+31UYFKg87EmeFZssahhJVIBuEs0wI4/z1Z4CnbE4fAzgGH7BT5mDR7jwQ6mPYSDSRVRzrx8iAqfsxwbBXpRTKAywitjavvjbHANQmouUwMopS1sISAINJzAq95jWha1RjhCGZLIRoz0bfy3oRqAn92I+wTGbRM4gyVhiUMFa/K/9QvCqkJ6z72/1gv4DTPUUJHBIBbTQSjYc+PyEn3atL89QnY6x2GGrmSPQVZtq2f3npqEDFI0HutpNMiD5ZqFmGwUT1MlKts8zDkdFpP4dBzMuwAQY1LoRplebL3kNtWE4UlhJyZaEUclqjN+ucJhw2iIGZRI4pbFEdLUQhCAMEDAAo8vhtaN42oQsWZcsUGpSGeQmN7RZh0ausooEf7VqATQEUIAlU57EUYWebNYsKeelRFiljRirZMWLPXgIgYjgYl+LascxYAI4RKNYNQK9Tn6qNc4p29ND0xTzdcZNtVKBiwoK2k3jZZb7tl5mIdJb22AbOLDPghSvt50xc3rnLhIF0GyBFr1g2HGvNdQlbI3GWBla8thAKk9OvqpUBhfsxIOkqUvCADfvArjCr0jAR4qntXEqRpRIPYNYVItA5+mYenbv9PBa1SFYJ2AqlASNAVLpBy3Ld7pc25Ziyk4AdK+MMbToHPDGvSExXVhAtYxpq6sq6di1om+uha67bZgGVfh3T0a7bRgcqAnOU0TKoZtXDzDMTW/awPS59YpbxhK6p/RyAkAnqMgI9kELl+1YK9zHMAOi0Wl7bVHgDSteu3om4X4R7+ltpRcg3SAmlsnF0t7qvgYGJT3sSakk6RAI0hmJ7kj7mPFhVewyhXlfFc2GAnqXPY7S0RwwMSJENADQGXoBaoSSGR2tT1gF89YXPF7NIt70OQIX5jWGYalf6vysMtIf+ukMtY0CIH13P2BHBum10oCLYRV+xbZLMyoTNM4PcgGwFaAuT55kJHaVu6U4GfWvKLExonr5kfMYwGa96ZwlsMKp61pQVlFXtJvcyIF8MgPD2XZk2oN93jR+goNe0xwBU5boBZykj0M9CIswEKyTgy0SVshBNn9NYbGENHGTZbAPsGF3PweuRLgOoFEIua3S/aBsZ164vLTWKtD5Cvgr0McfPPANE+ra9Fk1fALJttdGBinV5H4WEQyech4Dyt0BlkGBNs8z/Iz1FU+BYf17YA0yEFkM9Ytf1lIp0zFCpAODm9bsqmwHz0LqqyGT6oi/r1AjcrgPQyI71MYyvPQ5NzAQFSIDMmrISdrp3wIyFCOWEmXQnAFpYoT4XZvcdJ4AgYlQcmHByGZM9VosVORNlEcounF+xbft/zTiSBOBk12HmSFchLa1xGaKw27YSoJJij5ZIyDh1hQWzTCGiDE+0rSum1Ec/okdFTILnM+HKYHLtdCGTeogJz2hpROI2ZFR8CAzoY6rGTUbX3lWb431qmzCKZQ0zcV1dmTohQ9llgiaGhbbmWk1eaXBAFC2I1TwTz18/llBT6EGn1D9tWLrMRNbHBPb2GowVk3OoYYP6JFrDJzqot5vWJ11bb8viDhnzQ4xYTmKJroOeOabTW7etBKgUyUWZNo0XFQr0MYOadzcIWiZVGj2sTxEbPaTdRaE0eo1iPx6JTkOgXdSEQsCHQO6YNKGWWZlUBhJm4bp5XMWRdJc6JKyb96nfWiaT5PjuvSvM1BS9ShpgrQCGfqfvfUmrCW/JCZZpXR2BGxBFxxNOE+sJ6mMnICLTp2q62usAVHSvIQY4LWiOtkcha2BQrSlwnZWgGMPZzDNA5TlF1yAxgbVuq60EqJiJH3lvIra1WeqADGTU2YQ2OEo8D3gs6OXRImZWN2ygL6UlSkf6mWZgm8wyS4uIjkAZW5ARqtPkqpqjEFBoow6oTAKfES45d9c+VfpMLRKBdhHvjNUR7mXNouyi5ro9Dyv7gWU92VwX0NLUNgnpFYhyHFhMVC7iGHSddZk+xjrb68CEsIghpqgzchxCVjuN1kJ+MeMXy4q0UIAuDG5LLsY2sgVAas+v2eNqHWC5KlsZUKHD0WJRDTuS9aIP8dpEU8AlpQ68hBXtrpldzbGiMKXLZI+6NKGyxMIEBLSAzbELgPLeii3pDvQm4CK0ieqh3Esb+hUzqKXgMRgg4P1d2aO6mQQYHxHXJLTNi/2yeHOMy9Y36q8s3FYIqY4qchalASVZTfcIPA1m6W27IpTdVIXr/uccrUdWV9Qek/bkvesyQBUtHQFUXTtxzjIZyKho2TgT6s7aGcLYJWZHTgHoL/LlEEOMg3fP7bk1z3SZrcB321YGVMxE76LDmsyJVr+mFgiTakscDDyTtIBJ3RZZGiAEVEgZeb7SnEPWCAi5fmwDK/E7YRi4KJzsyp5p7qMLqIoBQNkwJQFdoXLU9JnzY2X6xPUVRmRCzCvq1ISoQCpiB0y4yFlYTycEbw07EOq2x9U/NKx1Gb1L+UJ7HdgpdrGIYat242jHpGYvtD4LetVPeZaRVAH0u7ZUHsMAlXC3Pa9mTi3i0DfNVgpUJqoM0CJLNWg2dJxa/wBSChW9Hu0tteg32aLIFp+qk2qPNVbDQvpSfazAUhcpeffYxfjGaLy9dDvmMO/6aFMmfPTtQUBW+UZ7fEx5nZXZgIp21l6H6673Dp9nhGaMKVrWpFp/kS+7sLSpS3N0jlXtTUVWkOmMzks2iBzOtthKgYoZ0FLwUcYuaqqolfsbMLwS9qIGxGQWKkULUGUFFzU03S6HCisjDzq0yQgBG95rSDaLIIqR0EnUUkXnGNLcozV+hO6odqw1CQ99o7/rDFcxIEdob8+D0Sm9WJfp4+h7EgGV59DHjC3V/pGD4Bhlhb1nEQPiEbP1HIBGkRPGNEDVtYme+yPHbKutHKiYh2yCRLF/22g/Cv4MNCvjhVDFrAGL1loN/VJTho05rtR9n+1CoiZEoreVmiBi/BCQqk2fyZxJ6asTmhWqzmtYlPCIdjRrgvgfNoRtWkuHyRLNIxHWezEtzNezkhRQ6kEXWyej0s++1r+9Z9ejpqiPWQoUPXvyg2MMEaEBub6LGFoBv7HNMjOJnfZ8GtaJtW+rrQWoimEwqojtAdUlHsvg8PhCpzb7RninybSfIW4va7J3wiEswUOV3QKsdCoDi2YFJC06Vexnoz5pajsXYHRo9TxNaqhhZ76Awv3b6K7V6YCFbKZqb2J2pOO1oQttCmuiu9inSGhttwHbzzieeyWsS3JEJuRy33QgISIRF4PRJ6sKbSIDVBg7UNUHnhUmIwSdl/VzDwTurkywZJDtfYaaWjzaVnRsS4PsHTamASr6a3Q+jmqdz2VsWytQMZNZ8aV0rZoP2QiDXd0KcCo1VlENjoW3ROP2IdC1xjJMBjBYBiKEkRAggCqXABbCMul3Av5urEYnmGJGMqOApYTUgN8kBawR+8KQMA9V8dgjnUq5gSRBeY++VcEPtABwX41tt80zUfsmC6lgF0PCWGYthAdwJjbxWYZUxT5QIXhzUPrHs1/WbMRY93HdCPd9wvC+JlEkIomEfHVx035US5jJLu2NPc2bGFL+NktrH4KU/CaaVDZQw0ostzCh1DYJVYGfWiv3xLMCaVqQMNSkM+Es+jWRZGwMNCxODZPyBNXe2GWbCjdIo/R4PXixLpsZ+norx3V8jgIz3ebq5SFm8ioVEbJzAJwRgPPaWAwZyyQPlMXWdbbYM+hTsNzHABV9NwIqRbF9C6030XYdqBYxk7v9tmTNnlGbZvQlYCKzKIzyUzmB5IBQQyFrCSmxoqLzqMki6tJHhHDRoCuNJoRBKdr0JagATNlBnTEtjVay3377ZfYp3LPTpbqaRUXiyRY3WWbgR3slLajRA17CZaGaZzGGASrHbZ+9RuccuixsE2yrgEphaPQ9dIt+f9s6TFhBlJYuNhgNIOGusMRAxZjUIQlXlHAYxMRp2T66CXYFVLAtYZgm9PWaJqSgqQFvWw/Ti+gpfgeQbR/JNgltZE63uZ7mgGIcBJ0QiBkrY5iIxPiReZZ95QBLJtPiaixxW22rgIrnib6CfNlV8qsylN5A9FPq2IDUeD6DlA5ncAkxDFzi7rLZQiZc7EpWYG4SBbJZMqpRuC38E6pus6axN5qxwwlhzAp2OTQOj5TgNWNxW22rgEopQVTBTWOZ7P+N1hJ98ULb6H2yrBip0gqDmghNz7AQGtubbPuNA+QIx3CCu2VbBVTSq9E30VjrNdn/mjCzz1Ka0mhnNLPyN32sb8HkZJOt2rYKqIi/0b4/Y3zB6QHNFCmqRl5k+VLdaBsTUE22KbZVQKWEIdqszVKPyfY0onzZXXPRBqj6VnZPNtmqbauAihDtW23bSSUtP9meZv/zeV8i0dUAlS1LJptsE2yrgEqGKtofWw3RZHuabVzUWbX91acBqilJMdmm2FYBlcxFtPeQ9WmT7Wkq9pcJ/az1m2yyTbCtAioWfZmoauzJ9rRlNSrbkUw22SbY1gGVcKSdVL7iabI9TV2UdX1tf/VpCkZ9K81kk22CbR1Q2bWxXf9GMJ5sT1PEuQxQYa+TTbYJtnVAZXlIu1f5vK9131uta/+uPg1Q+QabySbbBNs6oJpsssn2PpuAarLJJtt4m4Bqsskm23BL6f8AAIQccxYxJUEAAAAASUVORK5CYII=";
            //string Apikey = txtCaptchaKey;s
            //string jobId = WebCaptcha.createTaskNopecha(Apikey, ImagesBase64);
            //string resul = "";
            //for (int i = 0; i < 10; i++)
            //{
            //    resul = WebCaptcha.getResultNopecha(Apikey, jobId);
            //    if (!string.IsNullOrEmpty(resul))
            //    {
            //        break;
            //    }
            //    Thread.Sleep(1000);
            //}
            //if (resul == "")
            //{
            //    code = resul;
            //}

            //code = resul;

            //int a = 0;

            //string token = "EAABwzLixnjYBO3iRW5WcblJ8FuyHZCeISJy0cUJnIot4ltAmwZBmjbWRqLn7fQZA4iiYVtdMPlhcpmJBtLKsMvwXPZBE4uzZC02Q3Ar62JijHczofdW8siZCF7tbeFynUcbZAlDE7qpEfqrAAuWCJBFaQvfoaRZCAezWaKKMR6ZAVygJQ7fiXkT7kDfkM9RUh58GWDXgZD";
            //int limitQty = 10;
            //string uidAdmin = "100036379687209";

            //string url = $"https://graph.facebook.com/me/accounts?fields=id,name,access_token,additional_profile_id&limit={limitQty}&access_token={token}";
            //bool continueFetching = true;
            //int pageSucess = 0;
            //while (continueFetching)
            //{
            //    var rs = CommonRequest.GetPage(url, "", "");
            //    List<string> lstQuery = new List<string>();

            //    if (rs.Item1.Count > 0)
            //    {
            //        string data = rs.Item1[0].ToString();
            //        JArray jsonArray = JArray.Parse(data);

            //        foreach (var jsonString in jsonArray)
            //        {
            //            JObject jsonObject = (JObject)jsonString;

            //            string id = jsonObject["id"].ToString();
            //            string name = jsonObject["name"].ToString();
            //            string accessToken = jsonObject["access_token"].ToString();
            //            string additionalProfileId = jsonObject["additional_profile_id"].ToString();

            //            lstQuery.Add(CommonSQL.ConvertToSqlInsertPage(additionalProfileId, name, "0", "0", uidAdmin, "", "-1", "0", "", "", "", "", id, "", "Public", "none", accessToken, "0"));
            //            pageSucess++;
            //        }
            //    }

            //    bool checkQr = lstQuery.Count > 0;

            //    if (checkQr)
            //    {
            //        lstQuery = CommonSQL.ConvertToSqlInsertPage(lstQuery);

            //        for (int j = 0; j < lstQuery.Count; j++)
            //        {
            //            Connector.Instance.ExecuteNonQuery(lstQuery[j]);
            //        }

            //    }

            //    if (!string.IsNullOrEmpty(rs.Item2?.ToString()))
            //    {
            //        url = rs.Item2.ToString();
            //    }
            //    else
            //    {
            //        continueFetching = false;
            //    }
            //    LoadPageFromFile();
            //    Thread.Sleep(5000);
            //}


            //string sql = CommonSQL.ConvertToSqlInsertPage("123123", "page name", "0", "0", "60000", "", "-1", "0", "", "", "", "date create", "id bm", "", "Public", "avatar=yes cover=yes", "EAAAG", "0");

            // string ssss = CuaHelpers.GetJazoest();
            //string uid = "100005002950631";
            //string pass = "Cua@fuwa6h";
            //string c2fa = "QNEKMVJFZQHRHUYYNBP7YUTACIUS77SH";
            //string apikey = "882a8490361da98702bf97a021ddc14d";
            //string secret = "62f8ce9f74b12f84c123cc23437a4a32";
            //string oauth = "350685531728";

            //var rs = CommonRequest.RunGetAccessToken2FA(uid, pass, c2fa, apikey, secret, oauth);

            //string token = rs.Item1.ToString();
            //string cookie = rs.Item2.ToString();

            //Random rd = new Random();
            //string content = "☎  Nhóm hỗ trợ: {https://zalo.me/g/odtvr252|https://zalo.me/g/odtv152|https://zalo.me/g/3vrq152}\r\n📞 Hotline / Zalo: {𝟎𝟗𝟖𝟏.𝟑𝟑.𝟎𝟗𝟖𝟏|𝟎𝟖𝟏𝟖.𝟖𝟏𝟖.𝟓𝟔𝟕|𝟎𝟑𝟓𝟑.𝟒𝟓𝟒.𝟗𝟑𝟗|𝟎𝟑𝟗𝟐.𝟗𝟕𝟒.𝟗𝟕𝟒|𝟎𝟑𝟒𝟑.𝟖𝟗𝟖.𝟗𝟐𝟗}";

            //content = Helpers.Common.SpinText(content, rd);

            ////int a = 0;
            //Meta request = new Meta();

            //string ss = request.getIdPost("https://www.facebook.com/watch/?v=1237040440293020");

            //int a = 0;

            //otarjacquelyn2530@hotmail.com|T4T3msfN0l

            //string otpFromMail2 = EmailHelper.GetOtpFromMail(1, "otarjacquelyn2530@hotmail.com", "T4T3msfN0l");

            // string otpMail = CuaGetMail.getOtpMail("joisse22381829@hotmail.com", "Qwad0jLagV", "security@facebookmail.com", 1000);

            // string urlImg = await WebImage.dowloadThispersondoesnotexist("123123", "282-new");

            // WebImage.ChangeMD5OfFile("D:\\tl-theongay\\FacebookToolRequest\\Facebook Tool Request\\bin\\Debug\\backup\\123123\\image\\282-new.jpg");

            //            string img2 = await WebImage.downloadUnrealperson(i);

            //WebImage webImage = new WebImage();

            //string getImg = webImage.Get("https://api.unrealperson.com/person", "", "https://www.unrealperson.com/").Result;
            //string idImage = Regex.Match(getImg, "\"image_url\":\"(.*?)\"").Groups[1].Value;
            //string imgSuccess = webImage.DownloadImageFormUrl("https://api.unrealperson.com/image?name=" + idImage + "&type=tpdne", "123123", idImage, "https://www.unrealperson.com/").Result;

            //MessageBox.Show(img2);
            //string text35 = "dallasbrabandfx946@hotmail.com";
            //string text38 = text35.Split(new char[] {'@'})[0].Substring(text35.Split(new char[] { '@' })[0].Length - 1) + "@" + text35.Split(new char[] { '@' })[1];

            //setting_GiaiCheckpoint = new JSON_Settings("configGiaiCheckPoint");

            //string urlImg = "";
            //string cUid = "44";
            //string directoryPath = Path.Combine("backup", cUid, "image");
            //Directory.CreateDirectory(directoryPath);
            //string xxx = setting_GiaiCheckpoint.GetValue("imageType");

            //if (setting_GiaiCheckpoint.GetValue("imageType") == "0")
            //{
            //    for (; ; )
            //    {
            //        if (setting_GiaiCheckpoint.GetValueBool("ckbAnhCu"))
            //        {
            //            string pathImgUid = directoryPath;

            //            List<string> lstFrom = new List<string>();
            //            lstFrom = Directory.GetFiles(pathImgUid).ToList();

            //            if (lstFrom.Count == 0)
            //            {
            //                List<string> lstAnhSan = new List<string>();
            //                lstAnhSan = Directory.GetFiles(setting_GiaiCheckpoint.GetValue("txtAnhBackup").ToString()).ToList();
            //                if (lstAnhSan.Count == 0)
            //                {
            //                    urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
            //                    break;
            //                }
            //                else
            //                {
            //                    urlImg = Path.GetFullPath(lstAnhSan[rd.Next(0, lstAnhSan.Count)]);
            //                    urlImg = FileHelper.CoppyFile(urlImg, directoryPath +"\\" + Path.GetFileName(urlImg));

            //                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //                    urlImg = FileHelper.renameFile(urlImg, "ul282_" + currentTimestamp.ToString());
            //                    if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
            //                    {
            //                        WebImage.ChangeMD5OfFile(urlImg);
            //                    }
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                urlImg = Path.GetFullPath(lstFrom[rd.Next(0, lstFrom.Count)]);
            //                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //                urlImg = FileHelper.renameFile(urlImg, "rn_ul282_" + currentTimestamp.ToString());

            //                if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
            //                {
            //                    WebImage.ChangeMD5OfFile(urlImg);
            //                }
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            List<string> lstAnhSan = new List<string>();
            //            lstAnhSan = Directory.GetFiles(setting_GiaiCheckpoint.GetValue("txtAnhBackup").ToString()).ToList();
            //            if (lstAnhSan.Count == 0)
            //            {
            //                urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
            //                break;
            //            }
            //            else
            //            {
            //                urlImg = Path.GetFullPath(lstAnhSan[rd.Next(0, lstAnhSan.Count)]);
            //                urlImg = FileHelper.CoppyFile(urlImg, directoryPath + Path.GetFileName(urlImg));

            //                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //                urlImg = FileHelper.renameFile(urlImg, "ul282_" + currentTimestamp.ToString());
            //                if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
            //                {
            //                    WebImage.ChangeMD5OfFile(urlImg);
            //                }
            //                break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    for (; ; )
            //    {
            //        if (setting_GiaiCheckpoint.GetValueBool("ckbAnhCu"))
            //        {
            //            string pathImgUid = directoryPath;

            //            List<string> lstFrom = new List<string>();
            //            lstFrom = Directory.GetFiles(pathImgUid).ToList();

            //            if (lstFrom.Count == 0)
            //            {
            //                urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
            //                break;
            //            }
            //            else
            //            {
            //                urlImg = Path.GetFullPath(lstFrom[rd.Next(0, lstFrom.Count)]);
            //                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //                urlImg = FileHelper.renameFile(urlImg, "rn_ul282_" + currentTimestamp.ToString());

            //                if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
            //                {
            //                    WebImage.ChangeMD5OfFile(urlImg);
            //                }
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
            //            break;
            //        }
            //    }
            //}

            //string ahi = urlImg;

            //try
            //{
            //    if (CountChooseRowInDatagridview() == 0)
            //    {
            //        MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn gửi lên server!", 3);
            //        return;
            //    }
            //    List<string> list = new List<string>();
            //    MessageBoxHelper.ShowMessageBox($"Chuẩn bị gửi {CountChooseRowInDatagridview()} tài khoản lên server");

            //    int sendOk = 0;

            //    for (int i = 0; i < dtgvAcc.RowCount; i++)
            //    {
            //        if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
            //        {
            //            DataGridViewRow dataGridViewRow = dtgvAcc.Rows[i];
            //            try
            //            {
            //                string uid = dataGridViewRow.Cells["cUid"].Value.ToString();
            //                string password = dataGridViewRow.Cells["cPassword"].Value.ToString();
            //                string ma2fa = dataGridViewRow.Cells["cFa2"].Value.ToString();
            //                string name = dataGridViewRow.Cells["cName"].Value.ToString();
            //                string token = dataGridViewRow.Cells["cToken"].Value.ToString();
            //                string cookie = dataGridViewRow.Cells["cCookies"].Value.ToString();
            //                string email = dataGridViewRow.Cells["cEmail"].Value.ToString(); 
            //                string passmail = dataGridViewRow.Cells["cPassMail"].Value.ToString();
            //                string trangthai = dataGridViewRow.Cells["cInfo"].Value.ToString();
            //                string folderId = dataGridViewRow.Cells["cThuMuc"].Value.ToString();

            //                string jsonData = $"{{\"uid\":\"{uid}\",\"password\":\"{password}\",\"ma2fa\":\"{ma2fa}\",\"cookie\":\"{cookie}\",\"email\":\"{email}\",\"passmail\":\"{passmail}\",\"name\":\"{name}\",\"type\":\"{folderId}\",\"device\":\"{License.Hardware.getHDD()}\",\"proxy\":\"\",\"useragent\":\"\"}}";
            //                if(trangthai == "Live" && !string.IsNullOrEmpty(cookie))
            //                {
            //                    ApiAutoCua.addClone(jsonData);
            //                    sendOk++;
            //                }
            //            }
            //            catch
            //            {
            //            }
            //        }
            //    }

            //    MessageBoxHelper.ShowMessageBox($"Gửi thành công {sendOk.ToString()} tài khoản lên server");


            //    int a1x = 1;

            //}
            //catch (Exception ex)
            //{
            //    Helpers.Common.ExportError(null, ex, "Open Form ChangeInfo");
            //}
            //int ax = 1;
        }

        private void toolStripStatusLabel9_Click(object sender, EventArgs e)
        {
            //Settings.Default.isAdmin = "";
            //Settings.Default.buy_at = "";
            //Settings.Default.han = "";
            //Settings.Default.user = "";
            //Settings.Default.hethan = "";
            //Settings.Default.Save();
            //Hide();
        }
        private void lấyCookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần lấy cookie!"), 3);
            }
            else
            {
                UpdateInfoAccount(0);
            }
        }
        private void UpdateInfoAccount(int type)
        {
            try
            {
                LoadSetting();
                setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
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
                int maxThread = setting_general.GetValueInt("nudInteractThread", 3);
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
                                        UpdateInfoAccountOneThread(row, indexOfPossitionApp, profilePath, type);
                                        //UpdateInfoAccountOneThreadRequest(row, indexOfPossitionApp, type);
                                        Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                        if (!setting_InteractGeneral.GetValueBool("ckbRepeatAll"))
                                        {
                                            SetCellAccount(row, "cChose", false);
                                        }
                                    }
                                    catch (Exception ex3)
                                    {
                                        Helpers.Common.ExportError(null, ex3);
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
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
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
        private void UpdateInfoAccountOneThreadRequest(int indexRow, int indexPos, int type)
        {
            string text = "";
            int typeProxy = 0;
            string cId = GetCellAccount(indexRow, "cId");
            string c2Fa = GetCellAccount(indexRow, "cFa2");
            string cookie = GetCellAccount(indexRow, "cCookies");
            string userAgent = GetCellAccount(indexRow, "cUseragent");
            ShopLike shopLike = null;
            MinProxy minProxy = null;
            TinsoftProxy tinsoftProxy = null;
            string text2 = "";
            string text3 = "";

            if (userAgent == "")
            {
                userAgent = Base.useragentDefault;
            }
            if (string.IsNullOrEmpty(cookie)) return;

            while (true)
            {
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                    text = shopLike.proxy;
                                    if (text == "")
                                    {
                                        text = shopLike.GetProxy();
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
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                break;
                            }
                            bool flag4 = true;
                            if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                            {
                                SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                            }
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text2 = "(IP: " + text.Split(':')[0] + ") ";
                                SetStatusAccount(indexRow, text2 + "Check IP...");
                                text3 = Helpers.Common.CheckProxy(text, 0);
                                if (text3 == "")
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
                                    text = tinsoftProxy.proxy;
                                    if (text == "")
                                    {
                                        text = tinsoftProxy.GetProxy();
                                    }
                                    tinsoftProxy.dangSuDung++;
                                    tinsoftProxy.daSuDung++;
                                    break;
                                }
                                bool flag11 = true;
                            }
                            if (isStop)
                            {
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                break;
                            }
                            bool flag12 = true;
                            if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                            {
                                SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                            }
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text2 = "(IP: " + text.Split(':')[0] + ") ";
                                SetStatusAccount(indexRow, text2 + "Check IP...");
                                text3 = Helpers.Common.CheckProxy(text, 0);
                                if (text3 == "")
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
                                    text = minProxy.proxy;
                                    if (text == "")
                                    {
                                        text = minProxy.GetProxy();
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
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                break;
                            }
                            bool flag4 = true;
                            if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                            {
                                SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                            }
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text2 = "(IP: " + text.Split(':')[0] + ") ";
                                SetStatusAccount(indexRow, text2 + "Check IP...");
                                text3 = Helpers.Common.CheckProxy(text, 0);
                                if (text3 == "")
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
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                goto stop;
                            }
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text2 = "(IP: " + text3 + ")";
                            }
                            if (isStop)
                            {
                                SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                goto stop;
                            }

                            if (type == 0)
                            {
                                SetStatusAccount(indexRow, text2 + " Login Request.");
                                Meta request = new Meta(cookie, userAgent, text);

                            }
                            if (type == 1)
                            {
                                if (!CommonRequest.CheckLiveCookie(cookie, userAgent, text, typeProxy).StartsWith("1|"))
                                {
                                    SetStatusAccount(indexRow, text2 + " Cookie Die!");
                                    goto stop;
                                }
                                SetStatusAccount(indexRow, text2 + " Cookie OK!");

                                Meta request = new Meta(cookie, userAgent, text);
                                string token = "";
                                Helpers.Common.DelayTime(1.0);
                                if (!string.IsNullOrEmpty(c2Fa))
                                {
                                    SetStatusAccount(indexRow, text2 + " Get 2FA Code...");
                                    Helpers.Common.DelayTime(1.0);
                                    TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                                    string code = getcode.GetCode(c2Fa);
                                    string checkAuth = request.Post("https://business.facebook.com/security/twofactor/reauth/enter/", "&approvals_code=" + code + "&save_device=false&hash").Result;
                                    if (!checkAuth.Contains("\"codeConfirmed\":true"))
                                    {
                                        SetStatusAccount(indexRow, text2 + " 2FA Fail!");
                                        goto stop;
                                    }
                                }
                                Helpers.Common.DelayTime(1.0);
                                SetStatusAccount(indexRow, text2 + " Get Token EAAG...");
                                string getHtml = request.Get("https://business.facebook.com/content_management").Result;
                                token = Regex.Match(getHtml, "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                if (token != "")
                                {
                                    CommonSQL.UpdateFieldToAccount(cId, "token", token);
                                    SetCellAccount(indexRow, "cToken", token);
                                    SetStatusAccount(indexRow, text2 + " OK Get Token EAAG.");
                                }
                                else
                                {
                                    SetStatusAccount(indexRow, text2 + " Lỗi Get Token!");
                                }
                                goto stop;
                            }

                        }
                        break;
                    stop:
                        break;
                }
                break;
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
        private void UpdateInfoAccountOneThread(int indexRow, int indexPos, string profilePath, int type)
        {
            string text = "";
            Chrome chrome = null;
            int num = 0;
            bool flag = false;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            TinsoftProxy tinsoftProxy = null;
            MinProxy minProxy = null;
            bool flag2 = false;
            string text4 = "";
            string text5 = GetCellAccount(indexRow, "cUid");
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cEmail");
            string cellAccount3 = GetCellAccount(indexRow, "cPassMail");
            string cellAccount4 = GetCellAccount(indexRow, "cFa2");
            string cellAccount5 = GetCellAccount(indexRow, "cPassword");
            string cellAccount6 = GetCellAccount(indexRow, "cCookies");
            string cellAccount7 = GetCellAccount(indexRow, "cToken");
            string text6 = GetCellAccount(indexRow, "cUseragent");
            if (text5 == "")
            {
                text5 = Regex.Match(cellAccount6, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + ("Đã dừng!"));
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                try
                                {
                                    SetStatusAccount(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num5 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num5 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
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
                                    SetStatusAccount(indexRow, text2 + "Đang mở trình duyệt...");
                                    string app = "data:,";
                                    if (text6 == "")
                                    {
                                        text6 = Base.useragentDefault;
                                    }
                                    string text7 = "";
                                    if (profilePath != "" && text5 != "")
                                    {
                                        text7 = profilePath + "\\" + text5;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(text7))
                                        {
                                            text7 = "";
                                        }
                                    }
                                    Point position;
                                    Point size;
                                    if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                    {
                                        position = new Point(-1000, 0);
                                        size = new Point(setting_MoTrinhDuyet.GetValueInt("nudWidthChrome") + 16, setting_MoTrinhDuyet.GetValueInt("nudHeighChrome"));
                                    }
                                    else
                                    {
                                        position = Helpers.Common.GetPointFromIndexPosition(indexPos, setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                        size = Helpers.Common.GetSizeChrome(setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                    }
                                    chrome = new Chrome
                                    {
                                        IndexChrome = indexRow,
                                        DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract")),
                                        UserAgent = text6,
                                        ProfilePath = text7,
                                        Size = size,
                                        scaleChorme = false,
                                        Position = position,
                                        TimeWaitForSearchingElement = 3,
                                        TimeWaitForLoadingPage = 120,
                                        Proxy = text,
                                        TypeProxy = typeProxy,
                                        App = app,
                                        IsUsePortable = setting_general.GetValueBool("ckbUsePortable"),
                                        PathToPortableZip = setting_general.GetValue("txtPathToPortableZip")
                                    };
                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                        goto stop;
                                    }
                                    if (setting_general.GetValueInt("typeBrowser") != 0)
                                    {
                                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Lỗi mở trình duyệt!");
                                            goto stop;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");

                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
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
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        if (!chrome.GetProcess())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không check được chrome!");
                                            break;
                                        }
                                        if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                        {
                                            fViewChrome.remote.AddPanelDevice(chrome.IndexChrome, chrome.Size.X, chrome.Size.Y - 10);
                                            fViewChrome.remote.AddChromeIntoPanel(chrome.process.MainWindowHandle, chrome.IndexChrome, chrome.Size.X + 17, chrome.Size.Y, -10, 0);
                                        }

                                        //if (setting_general.GetValueBool("ckbThunhoPage"))
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}
                                        //else
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}

                                        string text8 = "";
                                        text8 = ((setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") != 0) ? "https://www.facebook.com/" : "https://m.facebook.com/");
                                        if (text7.Trim() != "")
                                        {
                                            switch (CommonChrome.CheckLiveCookie(chrome, text8))
                                            {
                                                case 1:
                                                    flag = true;
                                                    break;
                                                case -2:
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto quit;
                                                case -3:
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case 2:
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    goto quit;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            int valueInt = setting_MoTrinhDuyet.GetValueInt("typeLogin");
                                            switch (valueInt)
                                            {
                                                case 0:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng uid|pass..."));
                                                    break;
                                                case 1:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng email|pass..."));
                                                    break;
                                                case 2:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng cookie..."));
                                                    break;
                                            }
                                            string text9 = LoginFacebook(chrome, valueInt, text8, text5, cellAccount2, cellAccount5, cellAccount4, cellAccount6, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                            switch (text9)
                                            {
                                                case "-3":
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case "-2":
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto errorBreak;
                                                case "0":
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    goto errorBreak;
                                                case "1":
                                                    flag = true;
                                                    goto errorBreak;
                                                case "2":
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    goto errorBreak;
                                                case "3":
                                                    SetStatusAccount(indexRow, text2 + ("Không có 2fa!"));
                                                    goto errorBreak;
                                                case "4":
                                                    SetStatusAccount(indexRow, text2 + ("Tài khoản không đúng!"));
                                                    goto errorBreak;
                                                case "5":
                                                    SetStatusAccount(indexRow, text2 + ("Mật khẩu không đúng!"));
                                                    SetInfoAccount(cellAccount, indexRow, "Changed pass");
                                                    goto errorBreak;
                                                case "6":
                                                    SetStatusAccount(indexRow, text2 + ("Mã 2fa không đúng!"));
                                                    goto errorBreak;
                                                default:
                                                    {
                                                        SetStatusAccount(indexRow, text2 + text9);
                                                        goto errorBreak;
                                                    }
                                                errorBreak:
                                                    if (flag)
                                                    {
                                                        break;
                                                    }
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    SetRowColor(indexRow, 1);
                                                    ScreenCaptureError(chrome, text5, 1);
                                                    goto quit;
                                            }
                                        }
                                        if (setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") == 1 && !chrome.GetURL().StartsWith(text8))
                                        {
                                            chrome.GotoURL(text8);
                                        }
                                        SetStatusAccount(indexRow, text2 + "Đăng nhập thành công!");

                                        SetInfoAccount(cellAccount, indexRow, "Live");
                                        SetRowColor(indexRow, 2);
                                        if (type == 0)
                                        {
                                            string ccnew = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", ccnew);
                                            SetCellAccount(indexRow, "cCookies", ccnew);
                                            SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                            goto stop;
                                        }
                                        if (type == 1)
                                        {
                                            string ccnew = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", ccnew);
                                            SetCellAccount(indexRow, "cCookies", ccnew);
                                            SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                            Helpers.Common.DelayTime(1);
                                            SetStatusAccount(indexRow, text2 + ("Bắt đầu lấy Token!"));
                                            chrome.GotoURL("https://business.facebook.com/content_management");
                                            chrome.DelayTime(1.0);
                                            string pageSource = chrome.GetPageSource();
                                            chrome.DelayTime(1.0);
                                            if (chrome.CheckTextInChrome("Two-factor authentication required", "Yêu cầu xác thực 2 yếu tố") && cellAccount4 != "")
                                            {
                                                SetStatusAccount(indexRow, text2 + "Send code 2FA!");
                                                //get 2fa
                                                string cleanedAccount4 = cellAccount4.Replace(" ", "").Replace("\n", "");
                                                string totp = Helpers.Common.GetTotp(cleanedAccount4);
                                                bool flag10 = !string.IsNullOrEmpty(totp);
                                                if (flag10)
                                                {
                                                    chrome.SendKeys(3, "//div/input", totp, 0.1, true, 0.1);
                                                    chrome.SendEnter(3, "//div/input");
                                                    if (chrome.CheckTextInChrome("The login code you entered doesn't match the one sent to your phone. Please check the number and try again.", "Mã đăng nhập bạn nhập không khớp với mã đã gửi đến điện thoại của bạn. Vui lòng kiểm tra số này và thử lại"))
                                                    {
                                                        string totp2 = Helpers.Common.GetTotp(cleanedAccount4);
                                                        bool flag11 = !string.IsNullOrEmpty(totp2);
                                                        if (flag11)
                                                        {
                                                            chrome.SendKeys(3, "//div/input", totp2, 0.1, true, 0.1);
                                                            chrome.DelayTime(1.0);
                                                            chrome.SendEnter(3, "//div/input");
                                                            if (chrome.CheckTextInChrome("Đã xảy ra lỗi. Vui lòng thử lại sau.", "Something went wrong. Please try again later"))
                                                            {
                                                                SetStatusAccount(indexRow, text2 + "Bị chặn get token.");
                                                                goto stop;
                                                            }
                                                        }
                                                    }

                                                    chrome.DelayTime(1.0);
                                                    if (chrome.CheckTextInChrome("Page posts", "Bài viết trên trang"))
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Get Token...");
                                                        //object body = chrome.ExecuteScript(@"function getTokenEAAG() { const body = document.body.innerHTML; const match = body.match(/EAA(.*?)\""/); if (match && match[1]) {  const token = 'EAA' + match[1]; return token;  } else {    return '';  } } return getTokenEAAG();");
                                                        //string token = body.ToString();
                                                        string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                        if (token != "")
                                                        {
                                                            CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                            SetCellAccount(indexRow, "cToken", token);
                                                            SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                            Helpers.Common.DelayTime(1);
                                                            goto stop;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // get token 
                                                SetStatusAccount(indexRow, text2 + "Get Token...");
                                                //object body = chrome.ExecuteScript(@"function getTokenEAAG() { const body = document.body.innerHTML; const match = body.match(/EAA(.*?)\""/); if (match && match[1]) {  const token = 'EAA' + match[1]; return token;  } else {    return '';  } } return getTokenEAAG();");
                                                //string token = body.ToString();
                                                string token = Regex.Match(chrome.GetPageSource(), "EAAG(.*?)\"").Value.Replace("\"", "").Replace("\\", "");
                                                if (token != "")
                                                {
                                                    CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                    SetCellAccount(indexRow, "cToken", token);
                                                    SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                    goto stop;
                                                }
                                            }
                                        }
                                        if (type == 2)
                                        {
                                            string ccnew = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", ccnew);
                                            SetCellAccount(indexRow, "cCookies", ccnew);
                                            SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                            // get token 
                                            //string token = CommonRequest.getTokenEAAB(ccnew, text6, text);
                                            //int ax = 1;
                                            Helpers.Common.DelayTime(1);
                                            chrome.GotoURL("https://adsmanager.facebook.com/adsmanager/");
                                            chrome.ExecuteScript("document.querySelector('body').innerHTML='<b>Cua Toolkit<br>Đang Thao Tác...</b>'; document.querySelector('body').style = 'font-size:18px; color:red;text-align: center; background-color:#fff'");
                                            chrome.DelayTime(2.0);
                                            // get token 
                                            SetStatusAccount(indexRow, text2 + "Get Token...");
                                            object body = chrome.ExecuteScript(@"function getTokenEAAB() { let tokens = window.__accessToken; if (tokens) { return tokens; } else { return '';  } } return getTokenEAAB();");
                                            string token = body.ToString();
                                            //save
                                            if (token != "")
                                            {

                                                CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                SetCellAccount(indexRow, "cToken", token);
                                                SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                                goto stop;
                                            }
                                        }
                                        if (type == 3)
                                        {
                                            int wChromeOld = chrome.chrome.Manage().Window.Size.Width;
                                            int hChromeOld = chrome.chrome.Manage().Window.Size.Height;
                                            chrome.chrome.Manage().Window.Size = new Size(500, 700);

                                            string ccnew = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", ccnew);
                                            SetCellAccount(indexRow, "cCookies", ccnew);
                                            SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                            // get token 
                                            Helpers.Common.DelayTime(1);
                                            chrome.GotoURL("https://www.facebook.com/dialog/oauth?scope=user_about_me,pages_read_engagement,user_actions.books,user_actions.fitness,user_actions.music,user_actions.news,user_actions.video,user_activities,user_birthday,user_education_history,user_events,user_friends,user_games_activity,user_groups,user_hometown,user_interests,user_likes,user_location,user_managed_groups,user_photos,user_posts,user_relationship_details,user_relationships,user_religion_politics,user_status,user_tagged_places,user_videos,user_website,user_work_history,email,manage_notifications,manage_pages,publish_actions,publish_pages,read_friendlists,read_insights,read_page_mailboxes,read_stream,rsvp_event,read_mailbox&response_type=token&client_id=124024574287414&redirect_uri=fb124024574287414://authorize/&sso_key=com&display=&fbclid=IwAR1KPwp2DVh2Cu7KdeANz-dRC_wYNjjHk5nR5F-BzGGj7-gTnKimAmeg08k");
                                            chrome.DelayTime(2.0);
                                            chrome.ExecuteScript("document.querySelector('[name=\"__CONFIRM__\"]').click()");
                                            chrome.DelayTime(2.0);
                                            // get token 
                                            SetStatusAccount(indexRow, text2 + "Get Token...");

                                            chrome.GotoURL("view-source:https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https://www.instagram.com/accounts/signup/&&scope=email&response_type=token");
                                            string token = Regex.Match(chrome.GetURL(), "#access_token=(.*?)&").Groups[1].Value;
                                            //save
                                            chrome.chrome.Manage().Window.Size = new Size(wChromeOld, hChromeOld);
                                            if (token != "")
                                            {
                                                CommonSQL.UpdateFieldToAccount(cellAccount, "token", token);
                                                SetCellAccount(indexRow, "cToken", token);
                                                SetStatusAccount(indexRow, text2 + "Get Token Thành Công.");
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, text2 + "Get Token Thất bại.");
                                            }
                                            goto stop;
                                        }
                                    }
                                quit:;
                                }
                                catch (Exception ex)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
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
            if (chrome != null)
            {
                StatusChromeAccount status = chrome.Status;
                StatusChromeAccount statusChromeAccount = status;
                if (statusChromeAccount == StatusChromeAccount.ChromeClosed || statusChromeAccount == StatusChromeAccount.Checkpoint || statusChromeAccount == StatusChromeAccount.NoInternet)
                {
                    SetRowColor(indexRow, 1);
                    SetStatusAccount(indexRow, text2 + GetContentStatusChrome.GetContent(chrome.Status));
                }
            }
            if (!flag && setting_MoTrinhDuyet.GetValueBool("isAutoCloseChromeLoginFail"))
            {
                try
                {
                    chrome.Close();
                    CloseFormViewChrome();
                    cControl("stop");
                }
                catch
                {
                }
            }
            if (flag2 && Directory.Exists(setting_general.GetValue("txbPathProfile") + "\\" + text4))
            {
                string text10 = setting_general.GetValue("txbPathProfile") + "\\" + text4;
                string pathTo = setting_general.GetValue("txbPathProfile") + "\\" + text5;
                if (!Helpers.Common.MoveFolder(text10, pathTo) && Helpers.Common.CopyFolder(text10, pathTo))
                {
                    Helpers.Common.DeleteFolder(text10);
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
        private void checkLiveUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiemTraTaiKhoan(2);
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
                    cControl("start");
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
                        case 4:
                            {
                                int num8 = 0;
                                while (num8 < this.dtgvAcc.Rows.Count && !this.isStop)
                                {
                                    bool flag9 = Convert.ToBoolean(this.dtgvAcc.Rows[num8].Cells["cChose"].Value);
                                    if (flag9)
                                    {
                                        bool flag10 = iThread < maxThread;
                                        if (flag10)
                                        {
                                            Interlocked.Increment(ref iThread);
                                            int row4 = num8++;
                                            new Thread(delegate ()
                                            {
                                                SetStatusAccount(row4, "Đang kiểm tra...", -1);
                                                this.CheckAccountMail(row4);
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
                                        num8++;
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
                    cControl("stop");
                }).Start();
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
        private void CheckAccountMail(int row)
        {
            try
            {
                string mail = this.dtgvAcc.Rows[row].Cells["cEmail"].Value.ToString();
                string passmail = this.dtgvAcc.Rows[row].Cells["cPassMail"].Value.ToString();
                bool flag = mail == "" || passmail == "";
                if (flag)
                {
                    this.SetStatusAccount(row, "Không tìm thấy mail!", -1);
                }
                else
                {
                    bool flag2 = ImapHelper.CheckConnectImap(mail, passmail);
                    if (flag2)
                    {
                        this.SetStatusAccount(row, "Mail OK!", -1);
                    }
                    else
                    {
                        this.SetStatusAccount(row, "Mất Imap Check Mail!", -1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.SetStatusAccount(row, "Lỗi!", -1);
            }
        }
        private void checkLiveWallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiemTraTaiKhoan(0);
        }
        private void checkPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiemTraTaiKhoan(3);
        }
        private void checkAvatarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSetting();
            int iThread = 0;
            int maxThread = setting_general.GetValueInt("nudHideThread", 10);
            isStop = false;
            new Thread((ThreadStart)delegate
            {
                cControl("start");
                int num = 0;
                while (num < dtgvAcc.Rows.Count)
                {
                    Application.DoEvents();
                    if (isStop)
                    {
                        break;
                    }
                    if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                SetStatusAccount(row, "Đang kiểm tra...");
                                CheckMyAvatar(row);
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
                int tickCount = Environment.TickCount;
                while (iThread > 0 && Environment.TickCount - tickCount <= 30000)
                {
                    Application.DoEvents();
                    Thread.Sleep(300);
                }
                cControl("stop");
            }).Start();
        }
        private void CheckMyAvatar(int row, string token = "")
        {
            try
            {
                string uid = dtgvAcc.Rows[row].Cells["cUid"].Value.ToString();
                string id = dtgvAcc.Rows[row].Cells["cId"].Value.ToString();
                switch (CommonRequest.CheckAvatarFromUid(uid, token))
                {
                    case 0:
                        SetStatusAccount(row, ("Không có avatar!"), -1);
                        SetCellAccount(row, "cAvatar", "No", true);
                        CommonSQL.UpdateFieldToAccount(id, "avatar", "No");
                        break;
                    case 1:
                        SetStatusAccount(row, ("Đã có avatar!"), -1);
                        SetCellAccount(row, "cAvatar", "Yes", true);
                        CommonSQL.UpdateFieldToAccount(id, "avatar", "Yes");
                        break;
                    case 2:
                        SetStatusAccount(row, ("Có lỗi xảy ra!"), -1);
                        SetCellAccount(row, "cAvatar", "Unknown", true);
                        CommonSQL.UpdateFieldToAccount(id, "avatar", "Unknown");
                        break;
                }
            }
            catch
            {
                SetStatusAccount(row, "Lỗi!", -1);
            }
        }
        private void đổiNgônNgữTiếngViệtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần đổi ngôn ngữ!"), 3);
            }
            else
            {
                ChangeLanguageAccount(0);
            }
        }
        private void ChangeLanguageAccount(int type)
        {
            try
            {
                LoadSetting();
                setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
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
                int maxThread = setting_general.GetValueInt("nudInteractThread", 3);
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
                                        ChangeLangugeOneThread(row, indexOfPossitionApp, profilePath, type);
                                        Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                        if (!setting_InteractGeneral.GetValueBool("ckbRepeatAll"))
                                        {
                                            SetCellAccount(row, "cChose", false);
                                        }
                                    }
                                    catch (Exception ex3)
                                    {
                                        Helpers.Common.ExportError(null, ex3);
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
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
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
        private void ChangeLangugeOneThread(int indexRow, int indexPos, string profilePath, int type)
        {
            string text = "";
            Chrome chrome = null;
            int num = 0;
            bool flag = false;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            TinsoftProxy tinsoftProxy = null;
            MinProxy minProxy = null;
            bool flag2 = false;
            string text4 = "";
            string text5 = GetCellAccount(indexRow, "cUid");
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cEmail");
            string cellAccount3 = GetCellAccount(indexRow, "cPassMail");
            string cellAccount4 = GetCellAccount(indexRow, "cFa2");
            string cellAccount5 = GetCellAccount(indexRow, "cPassword");
            string cellAccount6 = GetCellAccount(indexRow, "cCookies");
            string cellAccount7 = GetCellAccount(indexRow, "cToken");
            string text6 = GetCellAccount(indexRow, "cUseragent");
            if (text5 == "")
            {
                text5 = Regex.Match(cellAccount6, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, ("Đang lấy Proxy ShopLike ..."));
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + ("Đã dừng!"));
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                            SetStatusAccount(indexRow, ("Đang lấy Proxy MinProxy ..."));
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                try
                                {
                                    SetStatusAccount(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num5 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num5 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
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
                                    SetStatusAccount(indexRow, text2 + "Đang mở trình duyệt...");
                                    string app = "data:,";

                                    if (text6 == "")
                                    {
                                        text6 = Base.useragentDefault;
                                    }
                                    string text7 = "";
                                    if (profilePath != "" && text5 != "")
                                    {
                                        text7 = profilePath + "\\" + text5;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(text7))
                                        {
                                            text7 = "";
                                        }
                                    }
                                    Point position;
                                    Point size;
                                    if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                    {
                                        position = new Point(-1000, 0);
                                        size = new Point(setting_MoTrinhDuyet.GetValueInt("nudWidthChrome") + 16, setting_MoTrinhDuyet.GetValueInt("nudHeighChrome"));
                                    }
                                    else
                                    {
                                        position = Helpers.Common.GetPointFromIndexPosition(indexPos, setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                        size = Helpers.Common.GetSizeChrome(setting_MoTrinhDuyet.GetValueInt("cbbColumnChrome", 5), setting_MoTrinhDuyet.GetValueInt("cbbRowChrome", 2));
                                    }
                                    chrome = new Chrome
                                    {
                                        IndexChrome = indexRow,
                                        DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract")),
                                        UserAgent = text6,
                                        ProfilePath = text7,
                                        Size = size,
                                        Position = position,
                                        TimeWaitForSearchingElement = 3,
                                        TimeWaitForLoadingPage = 120,
                                        Proxy = text,
                                        TypeProxy = typeProxy,
                                        App = app,
                                        IsUsePortable = setting_general.GetValueBool("ckbUsePortable"),
                                        PathToPortableZip = setting_general.GetValue("txtPathToPortableZip")
                                    };
                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                        goto stop;
                                    }
                                    if (setting_general.GetValueInt("typeBrowser") != 0)
                                    {
                                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Lỗi mở trình duyệt!");
                                            goto stop;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");

                                        //if (setting_general.GetValueBool("ckbThunhoPage"))
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}
                                        //else
                                        //{
                                        //    chrome.GotoURL("chrome://settings");
                                        //    chrome.ExecuteScript("chrome.settingsPrivate.setDefaultZoom(1);");
                                        //}

                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
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
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        if (!chrome.GetProcess())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không check được chrome!");
                                            break;
                                        }
                                        if (setting_MoTrinhDuyet.GetValueBool("ckbAddChromeIntoForm"))
                                        {
                                            fViewChrome.remote.AddPanelDevice(chrome.IndexChrome, chrome.Size.X, chrome.Size.Y - 10);
                                            fViewChrome.remote.AddChromeIntoPanel(chrome.process.MainWindowHandle, chrome.IndexChrome, chrome.Size.X + 17, chrome.Size.Y, -10, 0);
                                        }
                                        string text8 = "";
                                        text8 = ((setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") != 0) ? "https://www.facebook.com/" : "https://m.facebook.com/");
                                        if (text7.Trim() != "")
                                        {
                                            switch (CommonChrome.CheckLiveCookie(chrome, text8))
                                            {
                                                case 1:
                                                    flag = true;
                                                    break;
                                                case -2:
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto quit;
                                                case -3:
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case 2:
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    goto quit;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            int valueInt = setting_MoTrinhDuyet.GetValueInt("typeLogin");
                                            switch (valueInt)
                                            {
                                                case 0:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng uid|pass...");
                                                    break;
                                                case 1:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng email|pass...");
                                                    break;
                                                case 2:
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập bằng cookie...");
                                                    break;
                                            }
                                            string text9 = LoginFacebook(chrome, valueInt, text8, text5, cellAccount2, cellAccount5, cellAccount4, cellAccount6, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                            switch (text9)
                                            {
                                                case "-3":
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto quit;
                                                case "-2":
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto errorBreak;
                                                case "0":
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    goto errorBreak;
                                                case "1":
                                                    flag = true;
                                                    goto errorBreak;
                                                case "2":
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    goto errorBreak;
                                                case "3":
                                                    SetStatusAccount(indexRow, text2 + ("Không có 2fa!"));
                                                    goto errorBreak;
                                                case "4":
                                                    SetStatusAccount(indexRow, text2 + ("Tài khoản không đúng!"));
                                                    goto errorBreak;
                                                case "5":
                                                    SetStatusAccount(indexRow, text2 + ("Mật khẩu không đúng!"));
                                                    SetInfoAccount(cellAccount, indexRow, "Changed pass");
                                                    goto errorBreak;
                                                case "6":
                                                    SetStatusAccount(indexRow, text2 + ("Mã 2fa không đúng!"));
                                                    goto errorBreak;
                                                default:
                                                    {
                                                        SetStatusAccount(indexRow, text2 + text9);
                                                        goto errorBreak;
                                                    }
                                                errorBreak:
                                                    if (flag)
                                                    {
                                                        break;
                                                    }
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    SetRowColor(indexRow, 1);
                                                    ScreenCaptureError(chrome, text5, 1);
                                                    goto quit;
                                            }
                                        }
                                        if (setting_MoTrinhDuyet.GetValueInt("typeBrowserLogin") == 1 && !chrome.GetURL().StartsWith(text8))
                                        {
                                            chrome.GotoURL(text8);
                                        }
                                        SetStatusAccount(indexRow, text2 + "Đăng nhập thành công!");
                                        SetInfoAccount(cellAccount, indexRow, "Live");
                                        SetRowColor(indexRow, 2);
                                        //change to vietnam
                                        if (type == 0)
                                        {
                                            text6 = ConvertCookie(chrome.GetCookieFromChrome());
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "uid", text5);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "cookie1", text6);
                                            SetCellAccount(indexRow, "cCookies", text6);
                                            SetStatusAccount(indexRow, text2 + "Get Cookie Thành Công.");
                                            SetStatusAccount(indexRow, text2 + ("Đổi ngôn ngữ sang tiếng việt..."));
                                            Helpers.Common.DelayTime(1);
                                            chrome.GotoURL("https://mbasic.facebook.com/settings/language/");
                                            chrome.DelayTime(1.0);
                                            string pageSource = chrome.GetPageSource();
                                            chrome.DelayTime(1.0);
                                            if (pageSource.Contains("Cài đặt ngôn ngữ và vùng"))
                                            {
                                                SetStatusAccount(indexRow, text2 + ("Tài khoản đang là tiếng việt."));
                                                chrome.DelayTime(1.0);
                                                goto stop;
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, text2 + "Đang thao tác đổi tiếng việt...");
                                                chrome.DelayTime(1.0);
                                                chrome.Click(3, "//h3/a/strong");
                                                chrome.DelayTime(1.0);
                                                chrome.ExecuteScript("document.querySelector(\"input[value='Vietnamese']\").click()");
                                                chrome.DelayTime(1.0);
                                                SetStatusAccount(indexRow, text2 + "Đổi thành công!");
                                                chrome.DelayTime(1.0);
                                                goto stop;
                                            }

                                        }
                                    }
                                quit:;
                                }
                                catch (Exception ex)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
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
            if (chrome != null)
            {
                StatusChromeAccount status = chrome.Status;
                StatusChromeAccount statusChromeAccount = status;
                if (statusChromeAccount == StatusChromeAccount.ChromeClosed || statusChromeAccount == StatusChromeAccount.Checkpoint || statusChromeAccount == StatusChromeAccount.NoInternet)
                {
                    SetRowColor(indexRow, 1);
                    SetStatusAccount(indexRow, text2 + GetContentStatusChrome.GetContent(chrome.Status));
                }
            }
            if (!flag && setting_MoTrinhDuyet.GetValueBool("isAutoCloseChromeLoginFail"))
            {
                try
                {
                    CloseFormViewChrome();
                    cControl("stop");
                    chrome.Close();
                }
                catch
                {
                    cControl("stop");
                }
            }
            if (flag2 && Directory.Exists(setting_general.GetValue("txbPathProfile") + "\\" + text4))
            {
                string text10 = setting_general.GetValue("txbPathProfile") + "\\" + text4;
                string pathTo = setting_general.GetValue("txbPathProfile") + "\\" + text5;
                if (!Helpers.Common.MoveFolder(text10, pathTo) && Helpers.Common.CopyFolder(text10, pathTo))
                {
                    Helpers.Common.DeleteFolder(text10);
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
        private void loginHotmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginMail();
        }
        private void checkpoint956ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                unlockCheckpoint(956);
            }
        }
        private void unlockCheckpoint(int type)
        {
            try
            {
                LoadSetting();
                setting_MoTrinhDuyet = new JSON_Settings("configOpenBrowser");
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
                int maxThread = setting_general.GetValueInt("nudInteractThread", 3);
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
                                        unlockCheckpointOneThread(row, indexOfPossitionApp, profilePath, type);
                                        Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                        if (!setting_InteractGeneral.GetValueBool("ckbRepeatAll"))
                                        {
                                            SetCellAccount(row, "cChose", false);
                                        }
                                    }
                                    catch (Exception ex3)
                                    {
                                        Helpers.Common.ExportError(null, ex3);
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
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
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
        private void unlockCheckpointOneThread(int indexRow, int indexPos, string profilePath, int type)
        {
            string text = "";
            Chrome chrome = null;
            int num = 0;
            bool flag = false;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            TinsoftProxy tinsoftProxy = null;
            MinProxy minProxy = null;
            string text4 = GetCellAccount(indexRow, "cUid");
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cEmail");
            string cellAccount3 = GetCellAccount(indexRow, "cPassMail");
            string cellAccount4 = GetCellAccount(indexRow, "cFa2");
            string cellAccount5 = GetCellAccount(indexRow, "cPassword");
            string cellAccount6 = GetCellAccount(indexRow, "cCookies");
            string cellAccount7 = GetCellAccount(indexRow, "cToken");
            string text5 = GetCellAccount(indexRow, "cUseragent");
            if (cellAccount2 == "" || cellAccount3 == "")
            {
                if (cellAccount2 == "")
                {
                    SetStatusAccount(indexRow, text2 + "Không tìm thấy email!");
                }
                else if (cellAccount3 == "")
                {
                    SetStatusAccount(indexRow, text2 + "Không tìm thấy pass email!");
                }
            }
            if (text4 == "")
            {
                text4 = Regex.Match(cellAccount6, "c_user=(.*?);").Groups[1].Value;
            }
            if (isStop)
            {
                SetStatusAccount(indexRow, text2 + "Đã dừng!");
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusAccount(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag4 = false;
                                    }
                                }
                                if (!flag4)
                                {
                                    shopLike.dangSuDung--;
                                    shopLike.daSuDung--;
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
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
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag4 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusAccount(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag4 = false;
                                    }
                                }
                                if (!flag4)
                                {
                                    minProxy.dangSuDung--;
                                    minProxy.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        default:
                            {
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    if (setting_general.GetValueInt("ip_iTypeChangeIp") != 7 && setting_general.GetValueInt("ip_iTypeChangeIp") != 8 && setting_general.GetValueInt("ip_iTypeChangeIp") != 10)
                                    {
                                        if (text != "")
                                        {
                                            text2 = "(IP: " + text.Split(':')[0] + ") ";
                                        }
                                        SetStatusAccount(indexRow, text2 + "Check IP...");
                                        bool flag10 = false;
                                        int num4 = 0;
                                        while (num4 < 30)
                                        {
                                            Helpers.Common.DelayTime(1.0);
                                            text3 = Helpers.Common.CheckProxy(text, typeProxy);
                                            if (text3 != "")
                                            {
                                                flag10 = true;
                                                break;
                                            }
                                            if (!isStop)
                                            {
                                                num4++;
                                                continue;
                                            }
                                            goto isStop;
                                        }
                                        if (!flag10)
                                        {
                                            if (text != "")
                                            {
                                                SetStatusAccount(indexRow, text2 + ("Không thể kết nối proxy!"));
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, text2 + ("Không có kết nối Internet!"));
                                            }
                                            break;
                                        }
                                    }
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                try
                                {
                                    SetStatusAccount(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num5 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
                                            if (num5 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num5 < 0)
                                                {
                                                    SetStatusAccount(indexRow, text2 + "Mở trình duyệt sau" + " {time}s...".Replace("{time}", (num5 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                                        goto quit;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayChrome++;
                                        }
                                    }
                                    SetStatusAccount(indexRow, text2 + "Đang mở trình duyệt...");
                                    if (text5 == "")
                                    {
                                        text5 = Base.useragentDefault;
                                    }
                                    string text6 = "";
                                    if (profilePath != "" && text4 != "")
                                    {
                                        text6 = profilePath + "\\" + text4;
                                        if (!setting_InteractGeneral.GetValueBool("ckbCreateProfile") && !Directory.Exists(text6))
                                        {
                                            text6 = "";
                                        }
                                    }
                                    Point pointFromIndexPosition = Helpers.Common.GetPointFromIndexPosition(indexPos, 5, 2);
                                    Point sizeChrome = Helpers.Common.GetSizeChrome(5, 2);
                                    chrome = new Chrome
                                    {
                                        DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract")),
                                        UserAgent = text5,
                                        ProfilePath = text6,
                                        Size = sizeChrome,
                                        Position = pointFromIndexPosition,
                                        TimeWaitForSearchingElement = 3,
                                        TimeWaitForLoadingPage = 120,
                                        Proxy = text,
                                        TypeProxy = typeProxy,
                                        IsUsePortable = setting_general.GetValueBool("ckbUsePortable"),
                                        PathToPortableZip = setting_general.GetValue("txtPathToPortableZip")
                                    };
                                    if (isStop)
                                    {
                                        SetStatusAccount(indexRow, text2 + "Đã dừng!");
                                        break;
                                    }
                                    if (setting_general.GetValueInt("typeBrowser") != 0)
                                    {
                                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                    }
                                    int num6 = 0;
                                    while (true)
                                    {
                                        if (!chrome.Open())
                                        {
                                            SetStatusAccount(indexRow, text2 + "Lỗi mở trình duyệt!");
                                            break;
                                        }
                                        chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");
                                        if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                        {
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
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
                                                SetStatusAccount(indexRow, text2 + "Lỗi kết nối proxy!");
                                                break;
                                            }
                                        }
                                        string text7 = "https://m.facebook.com/";
                                        if (text6.Trim() != "")
                                        {
                                            num = CommonChrome.CheckLiveCookie(chrome, text7);
                                            switch (num)
                                            {
                                                case 1:
                                                    flag = true;
                                                    break;
                                                case -2:
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    goto end;
                                                case -3:
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto end;
                                                case 2:
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    break;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            int valueInt = setting_MoTrinhDuyet.GetValueInt("typeLogin");
                                            switch (valueInt)
                                            {
                                                case 0:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng uid|pass..."));
                                                    break;
                                                case 1:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng email|pass..."));
                                                    break;
                                                case 2:
                                                    SetStatusAccount(indexRow, text2 + ("Đăng nhập bằng cookie..."));
                                                    break;
                                            }
                                            string text8 = LoginFacebook(chrome, valueInt, text7, text4, cellAccount2, cellAccount5, cellAccount4, cellAccount6, setting_general.GetValueInt("tocDoGoVanBan"), setting_InteractGeneral.GetValueBool("ckbDontSaveBrowser"));
                                            switch (text8)
                                            {
                                                case "-3":
                                                    chrome.Status = StatusChromeAccount.NoInternet;
                                                    goto end;
                                                case "-2":
                                                    chrome.Status = StatusChromeAccount.ChromeClosed;
                                                    break;
                                                case "0":
                                                    SetStatusAccount(indexRow, text2 + "Đăng nhập thất bại!");
                                                    break;
                                                case "1":
                                                    flag = true;
                                                    break;
                                                case "2":
                                                    chrome.Status = StatusChromeAccount.Checkpoint;
                                                    SetInfoAccount(cellAccount, indexRow, ("Checkpoint"));
                                                    num = 2;
                                                    break;
                                                case "3":
                                                    SetStatusAccount(indexRow, text2 + ("Không có 2fa!"));
                                                    break;
                                                case "4":
                                                    SetStatusAccount(indexRow, text2 + ("Tài khoản không đúng!"));
                                                    break;
                                                case "5":
                                                    SetStatusAccount(indexRow, text2 + ("Mật khẩu không đúng!"));
                                                    SetInfoAccount(cellAccount, indexRow, "Changed pass");
                                                    break;
                                                case "6":
                                                    SetStatusAccount(indexRow, text2 + ("Mã 2fa không đúng!"));
                                                    break;
                                                default:
                                                    SetStatusAccount(indexRow, text2 + text8);
                                                    SetInfoAccount(text4, indexRow, "Live");
                                                    SetStatusAccount(indexRow, text2 + ("Acc Live."));
                                                    break;
                                            }
                                        }
                                        if (num != 2)
                                        {
                                            SetInfoAccount(cellAccount, indexRow, "Live");
                                            SetStatusAccount(indexRow, text2 + ("Acc Live."));
                                            break;
                                        }
                                        string text9 = "";
                                        int num7 = 0;
                                        //JSON_Settings jSON_Settings = new JSON_Settings("configCheckpoint");
                                        //chrome.GotoURLIfNotExist("https://m.facebook.com");
                                        //chrome.DelayTime(3.0);
                                        //if (chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway"))
                                        //{
                                        //    SetStatusAccount(indexRow, text2 + ("Tài khoản bị spam!"));
                                        //    break;
                                        //}
                                        //if (chrome.CheckExistElement("[href*=\"/save_locale/?loc=vi_VN\"]", 5.0) == 1)
                                        //{
                                        //    chrome.ExecuteScript("document.querySelector('[href*=\"/save_locale/?loc=vi_VN\"]').click()");
                                        //    chrome.DelayTime(3.0);
                                        //}
                                        //giải checkpoint here
                                        chrome.GotoURL("https://www.facebook.com");
                                        string urlCurrent = chrome.GetURL();
                                        if (urlCurrent.Contains("1501092823525282"))
                                        {
                                            string cpText = "Lỗi nên dừng lại";
                                            if (urlCurrent.Contains("1501092823525282"))
                                            {
                                                cpText = "CP 282";
                                            }
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "ghiChu", cpText);
                                            SetStatusAccount(indexRow, text2 + cpText);
                                            goto quit;
                                        } else if (urlCurrent.Contains("828281030927956"))
                                        {
                                            SetStatusAccount(indexRow, text2 + "CP 956");
                                        }
                                        else
                                        {
                                            SetStatusAccount(indexRow, text2 + "Không phải CP 956!");
                                            goto quit;
                                        }

                                        SetStatusAccount(indexRow, text2 + "Đang giải checkpoint 956!");


                                        try
                                        {
                                            chrome.FindClickElement("Bắt đầu các bước bảo mật", "Start security steps", true);
                                        }
                                        catch
                                        {
                                            SetStatusAccount(indexRow, text2 + "Bắt đầu");
                                        }

                                        try
                                        {
                                            chrome.FindClickElement("Bắt đầu", "Get started", true);
                                        }
                                        catch
                                        {
                                            SetStatusAccount(indexRow, text2 + "Bắt đầu các bước bảo mật!");
                                        }

                                        if (chrome.CheckTextInChrome("If you don’t have another device", "If you don’t have another device"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "If you don’t have another device");
                                            chrome.FindClickElement("Start security steps", "Start security steps", true);
                                        }

                                        if (chrome.CheckTextInChrome("Cách mở khóa tài khoản của bạn bằng thiết bị hiện tại", "How to unlock your account with your current device"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Cách mở khóa tài khoản của bạn bằng thiết bị hiện tại");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Các bước tiếp theo để mở khóa tài khoản", "Next steps to unlock your account"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Các bước tiếp theo để mở khóa tài khoản");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Bảo mật thông tin đăng nhập của bạn", "Secure Your Login Details"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Bảo mật thông tin đăng nhập của bạn");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Thông tin đăng nhập mới của bạn", "Your new login details"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Thông tin đăng nhập mới của bạn");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Bạn đã mở khóa tài khoản", "You've unlocked your account"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Mở không cần mail...");
                                            SetInfoAccount(text4, indexRow, "Live");
                                            chrome.GotoURL("https://www.facebook.com");
                                            goto quit;
                                        }

                                        if (chrome.CheckTextInChrome("Xác nhận đây là tài khoản của bạn", "Confirm this is your account"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Xác nhận đây là tài khoản của bạn");
                                            chrome.FindClickElement("Nhận mã qua email", "Get a code by email", true);
                                        }

                                        if ((chrome.CheckTextInChrome("Chọn email", "Select an email") || chrome.CheckTextInChrome("Chúng sẽ gửi m", "Get a code by email") || chrome.CheckTextInChrome("Nhận mã qua email", "Get a code by email")))
                                        {
                                            SetStatusAccount(indexRow, text2 + "Get Code");
                                            chrome.FindClickElement("Nhận mã", "Get code", true);
                                        }

                                        for (; ; )
                                        {
                                            bool flag64 = chrome.CheckTextInChrome("Nhập mã", "Enter code");
                                            if (flag64)
                                            {
                                                SetStatusAccount(indexRow, text2 + "Nhập mã");
                                                break;
                                            }
                                        }
                                        SetStatusAccount(indexRow, text2 + "Đang lấy mã từ hotmail...");
                                        //get code from mail
                                        //string otpFromMail2 = EmailHelper.GetOtpFromMail(1, cellAccount2, cellAccount3);
                                        goto getOtp;
                                        string otpHotmailNew = "";
                                    getOtp:
                                        for (; ; )
                                        {
                                            SetStatusAccount(indexRow, text2 + "Đang lấy mã từ hotmail...");
                                            otpHotmailNew = CuaGetMail.getOtpMail(cellAccount2, cellAccount3, "security@facebookmail.com");
                                            bool checkmail = otpHotmailNew.ToString() != "";
                                            if (checkmail)
                                            {
                                                goto giaiCheckpoint;
                                            }
                                            bool flag4 = num > 2;
                                            if (flag4)
                                            {
                                                SetStatusAccount(indexRow, text2 + "Mail khong ve!");
                                                goto quit;

                                            }
                                            num++;
                                        }

                                    giaiCheckpoint:
                                        SetStatusAccount(indexRow, text2 + "OTP: " + otpHotmailNew);

                                        for (; ; )
                                        {
                                            chrome.SendKeys(3, "/html/body/div/div/div/div/div/div/div/div/div[2]/div/div/div/div[1]/div/div/div[1]/div/div/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div/div/label/div/div/input", otpHotmailNew, 0.1);
                                            chrome.DelayTime(2.0);
                                            chrome.Click(3, "/html/body/div/div/div/div/div/div/div/div/div[2]/div/div/div/div[1]/div/div/div[1]/div/div/div/div/div/div/div/div/div/div/div/div/div[4]/div/div[2]/div/div/div[1]/div/span/span");
                                            //chrome.FindClickElement("Nhận mã", "Get Code", true);
                                            chrome.DelayTime(3.0);
                                            bool checkOtp = chrome.CheckTextInChrome("SMS thất bại", "SMS Security Check Failed") | chrome.CheckTextInChrome("Mã bạn nhập không chính xác", "Incorrect Code");
                                            if (checkOtp)
                                            {
                                                SetStatusAccount(indexRow, text2 + "OTP Lỗi!");
                                                goto quit;
                                            }
                                            break;
                                        }

                                        if (chrome.CheckTextInChrome("Các bước tiếp theo để mở khóa tài khoản", "Next Steps to Unlock Your Account"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Các bước tiếp theo để mở khóa tài khoản");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Bảo mật thông tin đăng nhập của bạn", "Secure Your Login Details"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Bảo mật thông tin đăng nhập của bạn");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        if (chrome.CheckTextInChrome("Your new login details", "Thông tin đăng nhập mới của bạn"))
                                        {
                                            chrome.DelayTime(2.0);
                                            SetStatusAccount(indexRow, text2 + "Your new login details");
                                            chrome.FindClickElement("Tiếp", "Next", true);
                                        }

                                        SetStatusAccount(indexRow, text2 + ("Checking..."));
                                        CommonChrome.GoToHome(chrome);
                                        if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                        {
                                            SetStatusAccount(indexRow, text2 + "Xong.");
                                            SetInfoAccount(text4, indexRow, "Live");
                                            break;
                                        }
                                        else
                                        {
                                            SetStatusAccount(indexRow, text2 + "Tạch.");
                                        }

                                        if (text9 == "")
                                        {
                                            urlCurrent = chrome.GetURL();
                                            string cpText = "Cp Dạng khác!";
                                            if (urlCurrent.Contains("1501092823525282"))
                                            {
                                                cpText = "CP 282";
                                            }
                                            SetStatusAccount(indexRow, text2 + cpText);
                                            CommonSQL.UpdateFieldToAccount(cellAccount, "ghiChu", cpText);
                                        }
                                        else
                                        {
                                            SetStatusAccount(indexRow, text2 + ("Xong! - " + text9));
                                        }

                                        break;
                                    }
                                end:;
                                }
                                catch (Exception ex2)
                                {
                                    SetStatusAccount(indexRow, text2 + "Lỗi không xác định!");
                                    Helpers.Common.ExportError(chrome, ex2);
                                }
                                break;
                            }
                        isStop:
                            chrome.Close();
                            SetStatusAccount(indexRow, text2 + "Đã dừng!");
                            break;
                        quit:
                            chrome.Close();
                            break;
                    }
                    break;
                }
            }
            try
            {
                chrome.Close();
            }
            catch
            {
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
        private void button3_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
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
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            try
            {
                Helpers.Common.KillProcess("chromedriver");
                Environment.Exit(0);
            }
            catch
            {
                Close();
            }
        }
        private void btnChangeInfo_Click(object sender, EventArgs e)
        {


        }
        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFormUpdate("Password");
        }
        private void OpenFormUpdate(string type)
        {
            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(GetCellAccount(i, "cChose")))
                    {
                        list.Add(GetCellAccount(i, "cId"));
                    }
                }
                if (list.Count == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần cập nhập!", 3);
                }
                else
                {
                    Helpers.Common.ShowForm(new fupdateData(this, type));
                }
            }
            catch
            {
            }
        }
        private void mailPassMailToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFormUpdate("Mail|pass");
        }
        private void fAToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFormUpdate("2FA");
        }
        private void tokenEAAGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                UpdateInfoAccount(1);
            }
        }
        private void tokenEAABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                UpdateInfoAccount(2);
            }
        }
        private void lọcTrùngUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fTienIchLocTrung());
        }
        private void tàiKhoảnĐãXoáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fAccountBin());
        }
        private void btnRestart_Click(object sender, EventArgs e)
        {
            bool areyouok = MessageBoxHelper.ShowMessageBoxWithQuestion(("Thực hiện thao tác này sẽ đóng tất cả cửa sổ chorme đang chạy, và khởi động lại chương trình. Bạn có chắc muốn khởi động lại phần mềm?")) == DialogResult.Yes;
            if (areyouok)
            {
                CloseProcess("chromedriver");
                CloseProcess("chrome");
                Application.Restart();
            }
        }
        private void quétUidBàiViếtToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void getIdPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fGetIdPost());
        }
        private void quảnLýPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(OpenForm2);
            thread.Start();

        }
        private void OpenForm2()
        {
            Application.Run(new core.fPages.fPages());
        }
        private void hiểnThịCộtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullString = new JSON_Settings("configDatagridview").GetFullString();
            Helpers.Common.ShowForm(new fCaiDatHienThi());
            if (fullString != new JSON_Settings("configDatagridview").GetFullString())
            {
                LoadConfigManHinh();
            }
        }
        private void quảnLýLờiMờiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm2(new core.fPages.fPageInvite());
        }
        private void danhSáchPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm2(new core.fPages.fPages());
        }
        private void nhậpTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "";
                if (cbbThuMuc.SelectedValue != null)
                {
                    text = cbbThuMuc.SelectedValue.ToString();
                }
                Helpers.Common.ShowForm(new fImportAccount(text));
                if (fImportAccount.isAddAccount || fImportAccount.isAddFile)
                {
                    LoadCbbThuMuc();
                    indexCbbThuMucOld = -1;
                    if (fImportAccount.isAddAccount)
                    {
                        cbbThuMuc.SelectedValue = fImportAccount.idFileAdded;
                    }
                    else
                    {
                        cbbThuMuc.SelectedValue = text;
                    }
                }
            }
            catch
            {
            }
        }
        private void gunaButton2_Click(object sender, EventArgs e)
        {
            bool areyouok = MessageBoxHelper.ShowMessageBoxWithQuestion("Thực hiện thao tác này sẽ đóng tất cả cửa sổ chorme đang chạy, và khởi động lại chương trình. Bạn có chắc muốn khởi động lại phần mềm?") == DialogResult.Yes;
            if (areyouok)
            {
                CloseProcess("chromedriver");
                CloseProcess("chrome");
                Application.Restart();
            }
        }
        private void btnChangeInfos_Click(object sender, EventArgs e)
        {
            try
            {
                if (CountChooseRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn Đổi thông tin!", 3);
                    return;
                }
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        DataGridViewRow dataGridViewRow = dtgvAcc.Rows[i];
                        try
                        {
                            list.Add(dataGridViewRow.Cells["cStt"].Value.ToString() + "|" + dataGridViewRow.Cells["cId"].Value.ToString() + "|" + dataGridViewRow.Cells["cUid"].Value.ToString() + "|" + dataGridViewRow.Cells["cCookies"].Value.ToString() + "|" + dataGridViewRow.Cells["cName"].Value.ToString() + "|" + dataGridViewRow.Cells["cBirthday"].Value.ToString() + "|" + dataGridViewRow.Cells["cGender"].Value.ToString() + "|" + dataGridViewRow.Cells["cPassword"].Value.ToString() + "|" + dataGridViewRow.Cells["cFa2"].Value.ToString() + "|" + dataGridViewRow.Cells["cUseragent"].Value.ToString() + "|" + dataGridViewRow.Cells["cProxy"].Value.ToString() + "|" + dataGridViewRow.Cells["cInfo"].Value.ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                Helpers.Common.ShowForm(new fChangeInfo(list));
                if (fChangeInfo.isAdd)
                {
                    LoadAccountFromFile(GetIdFile(), cbbTinhTrang.Text);
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Open Form ChangeInfo");
            }
        }
        private void càiĐặtHotmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fSettingMail());
        }
        private void loginHotmailToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox("Thiết bị của bạn chưa được phép dùng tính năng này!", 3);
            //LoginMail();
        }
        private void LoginMail(bool isUseProfile = false)
        {
            LoadSetting();
            string profilePath = "";
            if (isUseProfile)
            {
                profilePath = ConfigHelper.GetPathProfile();
                if (!Directory.Exists(profilePath))
                {
                    MessageBoxHelper.ShowMessageBox("Đường dẫn profile không hợp lệ!", 3);
                    return;
                }
            }
            List<int> lstPossition = new List<int>();
            for (int i = 0; i < CountChooseRowInDatagridview(); i++)
            {
                lstPossition.Add(0);
            }
            lstThread = new List<Thread>();
            new Thread((ThreadStart)delegate
            {
                try
                {
                    int num = 0;
                    while (num < dtgvAcc.Rows.Count)
                    {
                        if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                        {
                            int row = num++;
                            Thread thread = new Thread((ThreadStart)delegate
                            {
                                int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                LoginMailOneThread(row, indexOfPossitionApp, profilePath);
                            })
                            {
                                Name = row.ToString()
                            };
                            lstThread.Add(thread);
                            thread.Start();
                        }
                        else
                        {
                            num++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helpers.Common.ExportError(null, ex, "LoginMail()");
                }
            }).Start();
        }
        private void LoginMailOneThread(int indexRow, int indexPos, string profilePath = "")
        {

            Chrome chrome = null;
            string cellAccount = GetCellAccount(indexRow, "cId");
            string cellAccount2 = GetCellAccount(indexRow, "cUid");
            string cellAccount3 = GetCellAccount(indexRow, "cEmail");
            string cellAccount4 = GetCellAccount(indexRow, "cPassMail");
            string useragentIPad = SetupFolder.GetUseragentIPad(rd);
            try
            {
                setting_OpenMail = new JSON_Settings("configSettingMail");
                SetStatusAccount(indexRow, "Đang mở trình duyệt...");
                Point pointFromIndexPosition = Helpers.Common.GetPointFromIndexPosition(indexPos, 5, 2);
                Point sizeChrome = Helpers.Common.GetSizeChrome(5, 2);
                bool flag = false;
                try
                {
                    string profilePath2 = "";
                    if (profilePath != "" && (cellAccount2 != "" || cellAccount3 != ""))
                    {
                        if (cellAccount2 != "")
                        {
                            profilePath2 = profilePath + "\\" + cellAccount2;
                        }
                        else if (cellAccount3 != "")
                        {
                            profilePath2 = profilePath + "\\" + cellAccount3;
                        }
                    }
                    bool disableImage = false;
                    bool incognito = setting_OpenMail.GetValueBool("ckbHidechrome", false);
                    string prefixMail = setting_OpenMail.GetValue("txtKeyMailKP", "");
                    string mailDomain = setting_OpenMail.GetValue("txtMailDomainU");
                    chrome = new Chrome
                    {
                        Incognito = incognito,
                        DisableImage = disableImage,
                        Size = sizeChrome,
                        Position = pointFromIndexPosition,
                        TimeWaitForSearchingElement = 3,
                        TimeWaitForLoadingPage = 60,
                        ProfilePath = profilePath2,
                        UserAgent = useragentIPad
                    };
                    if (setting_general.GetValueInt("typeBrowser") != 0)
                    {
                        chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                    }
                    if (!chrome.Open())
                    {
                        SetStatusAccount(indexRow, "Lỗi mở trình duyệt!");
                        return;
                    }
                    SetStatusAccount(indexRow, "Đang đăng nhập...");
                    //login hotmail

                    while (true)
                    {
                        if (cellAccount3.Contains("hotmail") || cellAccount3.Contains("outlook"))
                        {
                            chrome.GotoURL("https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&ct=1623656281&rver=7.0.6737.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26RpsCsrfState%3d3dda2084-78c3-9378-7f87-d6a45f17ab8e&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=90015");
                            chrome.DelayTime(1.0);
                            if (chrome.CheckExistElement("[name=\"loginfmt\"]", 10.0) != 1)
                            {
                                break;
                            }
                            chrome.SendKeys(2, "loginfmt", cellAccount3);
                            chrome.DelayTime(0.1);
                            chrome.Click(1, "idSIButton9");
                            if (chrome.CheckExistElement("[name=\"passwd\"]", 10.0) != 1)
                            {
                                break;
                            }
                            chrome.DelayTime(2.0);
                            chrome.SendKeys(2, "passwd", cellAccount4);
                            chrome.DelayTime(2.0);
                            chrome.Click(1, "idSIButton9", 0, 0, "", 0, 10);
                            //submit button login
                            bool checkpass = chrome.CheckTextInChrome("Your account or password is incorrect", "Tài khoản hoặc mật khẩu của bạn không chính xác");
                            if (checkpass)
                            {
                                goto incorrectPass;
                            }

                            //check diaglog
                            for (int j = 0; j < 10; chrome.DelayTime(1.0), j++)
                            {
                                bool check1 = chrome.CheckTextInChrome("Stay signed in", "Stay signed in");
                                if (check1)
                                {
                                    try
                                    {
                                        chrome.Click(2, "DontShowAgain");
                                        chrome.DelayTime(0.1);
                                        chrome.Click(1, "idSIButton9");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error Stay signed in!");
                                        break;
                                    }
                                }
                                bool check2 = chrome.CheckTextInChrome("Your Microsoft account brings everything together", "Your Microsoft account brings everything together");
                                if (check2)
                                {
                                    try
                                    {
                                        chrome.Click(1, "id__0");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error Your Microsoft account brings everything together!");
                                        break;
                                    }
                                }
                                bool check3 = chrome.CheckTextInChrome("We're updating our terms", "We're updating our terms");
                                if (check3)
                                {
                                    try
                                    {
                                        chrome.Click(1, "iNext");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error We're updating our terms!");
                                        break;
                                    }
                                }
                                bool check4 = chrome.CheckTextInChrome("Break free from your passwords", "Break free from your passwords");
                                if (check4)
                                {
                                    try
                                    {
                                        chrome.Click(1, "iCancel");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error Break free from your passwords!");
                                        break;
                                    }
                                }
                                bool check5 = chrome.CheckTextInChrome("Is your security info still accurate?", "Is your security info still accurate?");
                                if (check5)
                                {
                                    try
                                    {
                                        chrome.Click(1, "iLooksGood");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error Is your security info still accurate!");
                                        break;
                                    }
                                }
                                bool check6 = chrome.CheckTextInChrome("We're updating our terms", "We're updating our terms");
                                if (check6)
                                {
                                    try
                                    {
                                        chrome.Click(1, "iNext");
                                        break;
                                    }
                                    catch
                                    {
                                        SetStatusAccount(indexRow, "Error We're updating our terms!");
                                        break;
                                    }
                                }
                                bool check7 = chrome.CheckTextInChrome("Help us protect your account", "Help us protect your account");
                                bool check8 = chrome.CheckTextInChrome("What security info would you like to add", "What security info would you like to add");
                                if (check7 && check8)
                                {
                                    goto addMailKP;
                                }
                            }

                            int num = 0;
                            while (true)
                            {
                                bool flag15 = chrome.CheckTextInChrome("Help us protect your account", "Help us protect your account");
                                if (flag15)
                                {
                                    goto protectAccount;
                                }
                                else
                                {
                                    bool flag17 = chrome.GetURL().Contains("recover");
                                    if (flag17)
                                    {
                                        goto mailBlock;
                                    }
                                    bool flag18 = num >= 2;
                                    if (flag18)
                                    {
                                        goto mailBlock;
                                    }
                                }
                                num++;
                                chrome.DelayTime(1);

                                if (!chrome.GetURL().StartsWith("https://outlook.live.com"))
                                {
                                    chrome.GotoURL("https://outlook.live.com/mail/0/");
                                }
                                flag = true;
                                break;
                            }
                            if (flag)
                            {
                                SetStatusAccount(indexRow, "Đăng nhập thành công!");
                                //
                            }
                            else
                            {
                                SetStatusAccount(indexRow, "Đăng nhập thất bại!");
                            }
                        addMailKP:
                            SetStatusAccount(indexRow, "Thêm email khôi phục...");
                            chrome.DelayTime(0.1);
                            string mailKP1 = cellAccount3.Split(new char[] { '@' })[0] + prefixMail + "@" + mailDomain;
                            chrome.SendKeys(1, "EmailAddress", mailKP1);
                            chrome.Click(1, "iNext");

                            bool check9 = chrome.CheckTextInChrome("Enter password", "Enter password");
                            if (check9)
                            {
                                try
                                {
                                    chrome.SendKeys(2, "passwd", cellAccount4);
                                    chrome.DelayTime(0.1);
                                    chrome.Click(1, "idSIButton9");
                                }
                                catch { }
                            }
                            int num2 = 0;
                            for (; ; )
                            {
                                bool check10 = chrome.CheckTextInChrome("Nhập mã", "Enter code");
                                if (check10)
                                {
                                    SetStatusAccount(indexRow, "Get Code Mail KP...");
                                    for (; ; )
                                    {
                                        //get code mail
                                        string codeMail = cuakit.Helpers.getCodeMailInboxes(mailKP1);
                                        bool ccode = codeMail == "";
                                        if (ccode)
                                        {
                                            goto noCode;
                                        }
                                        SetStatusAccount(indexRow, "Nhập code: " + codeMail);
                                        chrome.DelayTime(0.1);
                                        for (; ; )
                                        {
                                            try
                                            {
                                                chrome.SendKeys(1, "iOttText", codeMail);
                                                break;
                                            }
                                            catch
                                            {
                                                SetStatusAccount(indexRow, "Lỗi nhập code!");
                                            }
                                        }
                                        for (; ; )
                                        {
                                            try
                                            {
                                                chrome.Click(1, "iNext");
                                                break;
                                            }
                                            catch
                                            {
                                            }
                                        }

                                        for (; ; )
                                        {
                                            bool check1 = chrome.CheckTextInChrome("Stay signed in", "Stay signed in");
                                            if (check1)
                                            {
                                                try
                                                {
                                                    chrome.Click(2, "DontShowAgain");
                                                    chrome.DelayTime(0.1);
                                                    chrome.Click(1, "idSIButton9");
                                                    break;
                                                }
                                                catch
                                                {
                                                    SetStatusAccount(indexRow, "Error Stay signed in!");
                                                    break;
                                                }
                                            }

                                            bool check4 = chrome.CheckTextInChrome("Break free from your passwords", "Break free from your passwords");
                                            if (check4)
                                            {
                                                try
                                                {
                                                    chrome.Click(1, "iCancel");
                                                    break;
                                                }
                                                catch
                                                {
                                                    SetStatusAccount(indexRow, "Error Break free from your passwords!");
                                                    break;
                                                }
                                            }
                                        }

                                        bool check11 = chrome.CheckTextInChrome("That code didn't work", "That code didn't work");
                                        if (!check11)
                                        {
                                            break;
                                        }
                                        num2++;
                                    }
                                }
                            }



                        protectAccount:
                            SetStatusAccount(indexRow, "Xác nhận email khôi phục...");
                            chrome.DelayTime(1);
                            for (; ; )
                            {
                                try
                                {
                                    chrome.Click(1, "iProof0");
                                    break;
                                }
                                catch
                                {
                                }
                            }
                            //send mail kp
                            try
                            {
                                string mailKP = cellAccount3.Split(new char[] { '@' })[0] + prefixMail;
                                chrome.SendKeys(1, "iProofEmail", mailKP);
                            }
                            catch { }
                            try
                            {
                                chrome.DelayTime(1);
                                chrome.Click(1, "iSelectProofAction");
                            }
                            catch { }
                        mailBlock:
                            SetStatusAccount(indexRow, "Mail bị khoá...");
                        incorrectPass:
                            SetStatusAccount(indexRow, "Sai mật khẩu mail!");
                        noCode:
                            SetStatusAccount(indexRow, "Code không về!");
                        }
                        break;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    SetStatusAccount(indexRow, ("Lỗi đăng nhập!"));
                    Helpers.Common.ExportError(chrome, ex, "Login Error!");
                }
            }
            catch (Exception ex2)
            {
                SetStatusAccount(indexRow, ("Lỗi đăng nhập không xác định!"));
                Helpers.Common.ExportError(chrome, ex2);
            }
        }
        private void btnupdateapp_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("https://api.cua.monster/license/ver.xml");
        }
        private void mởProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần mở trình duyệt!", 3);
            }
            else
            {
                MoTrinhDuyet();
            }
        }
        private void càiĐặtCấuHìnhProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fMoTrinhDuyet());
        }
        private void gunaButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Helpers.Common.CreateFolder(Environment.CurrentDirectory + "\\log");
                Process.Start(Environment.CurrentDirectory + "\\log");
            }
            catch
            {
            }
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
        private void btGiaicheckpoint_Click(object sender, EventArgs e)
        {
            try
            {
                if (CountChooseRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn Đổi thông tin!", 3);
                    return;
                }
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        DataGridViewRow dataGridViewRow = dtgvAcc.Rows[i];
                        try
                        {
                            list.Add(dataGridViewRow.Cells["cStt"].Value.ToString() + "|" + dataGridViewRow.Cells["cId"].Value.ToString() + "|" + dataGridViewRow.Cells["cUid"].Value.ToString() + "|" + dataGridViewRow.Cells["cCookies"].Value.ToString() + "|" + dataGridViewRow.Cells["cName"].Value.ToString() + "|" + dataGridViewRow.Cells["cBirthday"].Value.ToString() + "|" + dataGridViewRow.Cells["cGender"].Value.ToString() + "|" + dataGridViewRow.Cells["cPassword"].Value.ToString() + "|" + dataGridViewRow.Cells["cFa2"].Value.ToString() + "|" + dataGridViewRow.Cells["cUseragent"].Value.ToString() + "|" + dataGridViewRow.Cells["cProxy"].Value.ToString() + "|" + dataGridViewRow.Cells["cInfo"].Value.ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                Helpers.Common.ShowForm(new fChangeInfo(list));
                if (fChangeInfo.isAdd)
                {
                    LoadAccountFromFile(GetIdFile(), cbbTinhTrang.Text);
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Open Form ChangeInfo");
            }
        }
        private void wwwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSetting();
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần thực hiện!", 3);
            }
            else
            {
                fGiaiCheckPoint formCp = new fGiaiCheckPoint();
                formCp.FormRunEvent += fGiaiCheckPoint_Running;
                formCp.Show();
            }
        }
        private void fGiaiCheckPoint_Running(object sender, EventArgs e)
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
                int maxThread = setting_general.GetValueInt("nudInteractThread", 5);
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
                                        runThreadGiaiCheckPointFb(row, indexOfPossitionApp, profilePath);
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
        private async void runThreadGiaiCheckPointFb(int indexRow, int indexPos, string profilePath)
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
            string keyOtp282 = setting_GiaiCheckpoint.GetValue("keyotp282", "f74b53b3edc6f37e2d823b2f27bc744e");

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
                                    SetStatusAccount(indexRow, cStatus + ("Đã dừng!"));
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
                                    string app = "data:,";

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
                                        App = app,
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
                                            chrome.GotoURL("https://cloudflare.com/cdn-cgi/trace");
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

                                                    chrome.GotoURL("https://www.facebook.com");
                                                    string ccNew = ConvertCookie(chrome.GetCookieFromChrome());
                                                    CommonSQL.UpdateFieldToAccount(cId, "cookie1", ccNew);
                                                    SetCellAccount(indexRow, "cCookies", ccNew);

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
                                                        chrome.GotoURL("https://www.facebook.com");
                                                        string ccNew = ConvertCookie(chrome.GetCookieFromChrome());
                                                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", ccNew);
                                                        SetCellAccount(indexRow, "cCookies", ccNew);

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
                                        string urlCurrent = chrome.GetURL();
                                        if (urlCurrent == "https://www.facebook.com/checkpoint/?next" || urlCurrent == "https://www.facebook.com/checkpoint/block/?next=https%3A%2F%2Fwww.facebook.com%2F")
                                        {
                                            int wChromeOld = chrome.chrome.Manage().Window.Size.Width;
                                            int hChromeOld = chrome.chrome.Manage().Window.Size.Height;
                                            chrome.chrome.Manage().Window.Size = new Size(800, 700);

                                            //proxy black list
                                            if(chrome.CheckTextInChrome("We'll send you a code to your email address", "We'll send you a code to your email address"))
                                            {
                                                //proxy black list not working...
                                                SetStatusAccount(indexRow, cStatus + "Proxy bẩn không thể đăng nhập!.");
                                                SetInfoAccount(cId, indexRow, "Die");
                                                SetRowColor(indexRow, 1);
                                                goto stop;
                                            }

                                            //check login thiết bị
                                            if (chrome.CheckTextInChrome("Check the login details shown. Was it you?", "Check the login details shown. Was it you?"))
                                            {
                                                SetStatusAccount(indexRow, cStatus + "Check the login details shown...");
                                                chrome.Click(1, "checkpointSubmitButton");
                                                SetStatusAccount(indexRow, cStatus + "Check live...");
                                                CommonChrome.GoToHome(chrome);
                                                if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                                {
                                                    string ccNew = ConvertCookie(chrome.GetCookieFromChrome());
                                                    SetStatusAccount(indexRow, cStatus + "Unlock Thiết bị Xong!.");
                                                    SetInfoAccount(cId, indexRow, "Live");
                                                    CommonSQL.UpdateFieldToAccount(cId, "cookie1", ccNew);
                                                    SetCellAccount(indexRow, "cCookies", ccNew);
                                                    goto stop;
                                                }
                                                else
                                                {
                                                    SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                    goto stop;
                                                }
                                            }

                                            SetStatusAccount(indexRow, cStatus + "Checkpoint Thiết bị mới!");
                                            SetStatusAccount(indexRow, cStatus + "Checkpoint");
                                            if(chrome.CheckTextInChrome("Login approval needed", "Login approval needed"))
                                            {
                                                chrome.Click(1, "checkpointSubmitButton");
                                            }
                                            Thread.Sleep(500);
                                            if(chrome.CheckTextInChrome("Choose an option", "Chọn một cách"))
                                            {
                                                Thread.Sleep(500);
                                                if(chrome.CheckTextInChrome("Yêu cầu gửi mã vào email của bạn", "Get a code sent to your email address"))
                                                {
                                                    chrome.FindClickElement("Yêu cầu gửi mã vào email của bạn", "Get a code sent to your email address", true);
                                                    Thread.Sleep(500);
                                                    chrome.Click(1, "checkpointSubmitButton");
                                                    Thread.Sleep(1000);
                                                    if (chrome.CheckTextInChrome("Yêu cầu gửi mã vào email của bạn", "Have a code sent to your email address") || chrome.CheckTextInChrome("Yêu cầu gửi mã vào email của bạn", "Get a code sent to your email"))
                                                    {
                                                        string otpHotmailNew = "";
                                                        Thread.Sleep(2000);
                                                        string mailCheck = cEmail.Split(new char[] { '@' })[0].Substring(cEmail.Split(new char[] { '@' })[0].Length - 1) + "@" + cEmail.Split(new char[] { '@' })[1];
                                                        if (!chrome.CheckTextInChrome(mailCheck, mailCheck))
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Mail không trùng!");
                                                            SetInfoAccount(cId, indexRow, "Mail Changed");
                                                            SetRowColor(indexRow, 1);
                                                            goto stop;
                                                        }
                                                        goto getOtp1;

                                                    getOtp1:
                                                        chrome.Click(1, "checkpointSubmitButton");
                                                        Thread.Sleep(2000);

                                                        for (; ; )
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Đang lấy mã từ hotmail...");
                                                            otpHotmailNew = CuaGetMail.getOtpMail(cEmail, cPassMail, "security@facebookmail.com");
                                                            bool checkmail = otpHotmailNew.ToString() != "";
                                                            if (checkmail)
                                                            {
                                                                goto giaiCheckpoint1;
                                                            }
                                                            if (num > 2)
                                                            {
                                                                SetStatusAccount(indexRow, cStatus + "Mail không về!");
                                                                goto stop;
                                                            }
                                                            num++;
                                                        }

                                                    giaiCheckpoint1:
                                                        SetStatusAccount(indexRow, cStatus + "OTP: " + otpHotmailNew);
                                                        for (; ; )
                                                        {
                                                            chrome.SendKeys(2, "captcha_response", otpHotmailNew, 0.1);
                                                            Thread.Sleep(2000);
                                                            chrome.Click(1, "checkpointSubmitButton");
                                                            Thread.Sleep(3000);
                                                            bool checkOtp = chrome.CheckTextInChrome("Incorrect Code", "Incorrect Code") | chrome.CheckTextInChrome("The code you entered is incorrect", "The code you entered is incorrect");
                                                            if (checkOtp)
                                                            {
                                                                SetStatusAccount(indexRow, cStatus + "OTP Lỗi!");
                                                                goto stop;
                                                            }
                                                            break;
                                                        }
                                                        if(chrome.CheckTextInChrome("Check the login details shown. Was it you?", "Hãy kiểm tra thông tin đăng nhập dưới đây. Đó có phải là bạn không?"))
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Check the login details shown...");
                                                            chrome.Click(1, "checkpointSubmitButton");
                                                        }
                                                        if (chrome.CheckTextInChrome("Tất cả đã xong", "You're all set"))
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "You're all set...");
                                                            Thread.Sleep(2000);
                                                            chrome.Click(1, "checkpointSubmitButton");
                                                            Thread.Sleep(2000);
                                                        }

                                                        SetStatusAccount(indexRow, cStatus + "Check live...");
                                                        CommonChrome.GoToHome(chrome);
                                                        if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                                        {
                                                            string ccNew = ConvertCookie(chrome.GetCookieFromChrome());
                                                            SetStatusAccount(indexRow, cStatus + "Unlock Thiết bị Xong!.");
                                                            SetInfoAccount(cId, indexRow, "Live");
                                                            CommonSQL.UpdateFieldToAccount(cId, "cookie1", ccNew);
                                                            SetCellAccount(indexRow, "cCookies", ccNew);
                                                            goto stop;
                                                        }
                                                        else
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                            goto stop;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SetStatusAccount(indexRow, cStatus + "Không có lựa chọn mở bằng mail!");
                                                    SetInfoAccount(cId, indexRow, "Checkpoint");
                                                    SetRowColor(indexRow, 1);
                                                    goto stop;
                                                }
                                            }

                                        }
                                        else if (Regex.IsMatch(urlCurrent, "1501092823525282")) {
                                            SetStatusAccount(indexRow, cStatus + " Checkpoint 282...");
                                            SetInfoAccount(cId, indexRow, "282");

                                            chrome.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
                                            Thread.Sleep(1000);

                                            if (chrome.CheckTextInChrome("you submitted an appeal", "you submitted an appeal"))
                                            {
                                                SetStatusAccount(indexRow, cStatus + " Đã gửi ảnh trước đó! Chờ về!");
                                                goto stop;
                                            }

                                            changeLanguage(chrome);
                                            Thread.Sleep(1000);
                                            chrome.GotoURL("https://mbasic.facebook.com");
                                            Thread.Sleep(5000);
                                            chrome.GotoURL("https://mbasic.facebook.com");
                                            Thread.Sleep(2000);

                                            if (chrome.CheckTextInChrome("Enter mobile number", "Enter mobile number"))
                                            {
                                                SetStatusAccount(indexRow, "CP 282 - Số điện thoại!");
                                                CommonSQL.UpdateFieldToAccount(cId, "status282", "282-Phone");
                                                SetCellAccount(indexRow, "cStatus282", "282-Phone");
                                                SetInfoAccount(cId, indexRow, "282-Phone");
                                                SetRowColor(indexRow, 1);
                                                goto unlock282Phone;
                                                //unlock 282 phone
                                            }

                                            Thread.Sleep(1000);
                                            if (chrome.CheckTextInChrome("We suspended your account", "We suspended your account"))
                                            {
                                                chrome.Click(2, "action_proceed");
                                                Thread.Sleep(5000);
                                            }

                                            if (chrome.CheckTextInChrome("Enter mobile number", "Enter mobile number"))
                                            {
                                                SetStatusAccount(indexRow, "CP 282 - Số điện thoại!");
                                                CommonSQL.UpdateFieldToAccount(cId, "status282", "282-Phone");
                                                SetCellAccount(indexRow, "cStatus282", "282-Phone");
                                                SetInfoAccount(cId, indexRow, "282-Phone");
                                                SetRowColor(indexRow, 1);
                                                goto unlock282Phone;
                                            }

                                            string captchaResponse = "";
                                            if (chrome.CheckTextInChrome("Help us confirm it's you", "Help us confirm it's you"))
                                            {
                                                captchaResponse = giaiCaptcha(indexRow, chrome, setting_GiaiCheckpoint.GetValue("cbbSiteCaptcha"), setting_GiaiCheckpoint.GetValue("txtCaptchaKey"));
                                                Thread.Sleep(2000);
                                            }

                                            if (chrome.CheckTextInChrome("Enter mobile number", "Enter mobile number"))
                                            {
                                                SetStatusAccount(indexRow, "CP 282 - Số điện thoại!");
                                                CommonSQL.UpdateFieldToAccount(cId, "status282", "282-Phone");
                                                SetCellAccount(indexRow, "cStatus282", "282-Phone");
                                                SetInfoAccount(cId, indexRow, "282-Phone");
                                                SetRowColor(indexRow, 1);
                                                goto unlock282Phone;
                                            }

                                            if (!string.IsNullOrEmpty(captchaResponse))
                                            {
                                                chrome.SendKeys(1, "captcha_response", captchaResponse);
                                                chrome.Click(2, "action_submit_bot_captcha_response");
                                                Thread.Sleep(2000);
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, "Giải Captcha thất bại!");
                                                SetRowColor(indexRow, 1);
                                                goto stop;
                                            }

                                            if (chrome.CheckTextInChrome("Enter mobile number", "Enter mobile number"))
                                            {
                                                SetStatusAccount(indexRow, "CP 282 - Số điện thoại!");
                                                CommonSQL.UpdateFieldToAccount(cId, "status282", "282-Phone");
                                                SetCellAccount(indexRow, "cStatus282", "282-Phone");
                                                SetInfoAccount(cId, indexRow, "282-Phone");
                                                SetRowColor(indexRow, 1);
                                                goto unlock282Phone;
                                            }

                                            if (chrome.CheckTextInChrome("Upload a photo of yourself", "Upload a photo of yourself") || chrome.CheckTextInChrome("Upload a verification selfie", "Upload a verification selfie"))
                                            {
                                                if (setting_GiaiCheckpoint.GetValueBool("ckbUpAnh"))
                                                {
                                                    for (int i = 0; i <= 2; i++)
                                                    {
                                                        bool submitImg = await submitImage282(chrome, indexRow, cUid, cId);
                                                        if (submitImg)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SetStatusAccount(indexRow, cStatus + " Checkpoint 282 up ảnh. Không chọn option giải captcha!");
                                                    goto stop;
                                                }
                                            }

                                        unlock282Phone:
                                            //get phone
                                            string rsPhone = WebCaptcha.getPhoneSimFast(keyOtp282, "1");
                                            if (rsPhone == null)
                                            {
                                                SetStatusAccount(indexRow, "Kho hết số!");
                                                goto stop;
                                            }
                                            var spPhone = rsPhone.Split('|');
                                            string idOrder = spPhone[0];
                                            string phone = spPhone[1];

                                            if(!string.IsNullOrEmpty(phone))
                                            {
                                                chrome.SendKeys(2, "contact_point", phone);
                                                chrome.DelayTime(2.0);
                                                chrome.Click(2, "action_set_contact_point");
                                                chrome.DelayTime(2.0);

                                                if(chrome.CheckTextInChrome("Enter the 6-digit", "Enter the 6-digit"))
                                                {
                                                    string otp = "";
                                                    for (int i = 0; i <= 30; i++)
                                                    {
                                                        otp = WebCaptcha.getSmsOtpFastSim(idOrder, keyOtp282);
                                                        if (!string.IsNullOrEmpty(otp))
                                                        {
                                                            otp = otp.Trim();
                                                            break;
                                                        }
                                                        Thread.Sleep(1000);
                                                    }
                                                    if(string.IsNullOrEmpty(otp))
                                                    {
                                                        SetStatusAccount(indexRow, "OTP không về!");
                                                        goto stop;
                                                    }

                                                    chrome.SendKeys(2, "code", otp);
                                                    chrome.DelayTime(2.0);
                                                    chrome.SendEnter(2, "code");
                                                   //chrome.Click(2, "action_submit_code");
                                                    chrome.DelayTime(2.0);

                                                    if(chrome.CheckTextInChrome("Upload a verification selfie", "Upload a verification selfie"))
                                                    {
                                                        if (setting_GiaiCheckpoint.GetValueBool("ckbUpAnh"))
                                                        {
                                                            for (int i = 0; i <= 2; i++)
                                                            {
                                                                bool submitImg = await submitImage282(chrome, indexRow, cUid, cId);
                                                                if (submitImg)
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + " Checkpoint 282 up ảnh. Không chọn option giải captcha!");
                                                            goto stop;
                                                        }
                                                    }
                                                    
                                                }    
                                            }

                                            //get sms

                                            goto stop;
                                        }
                                        else if (Regex.IsMatch(urlCurrent, "7956"))
                                        {
                                            if (chrome.CheckTextInChrome("Thử quay lại trên điện thoại hoặc máy tính mà trước", "Try coming back on a phone or computer you’ve used to log into Facebook before"))
                                            {
                                                SetStatusAccount(indexRow, cStatus + "Checkpoint 956-Khoá tím");
                                                SetInfoAccount(cId, indexRow, "CP 956-Khoá Tím");
                                                SetRowColor(indexRow, 1);
                                                goto stop;
                                            }   
                                            else
                                            {
                                                if (chrome.CheckTextInChrome("Bắt đầu các bước bảo mật", "Start security steps"))
                                                {
                                                    SetStatusAccount(indexRow, cStatus + "Checkpoint 956");
                                                    changeLanguage(chrome);

                                                    chrome.GotoURL("https://www.facebook.com");
                                                    Thread.Sleep(500);
                                                    chrome.FindClickElement("Start security steps", "Bắt đầu các bước bảo mật", true);
                                                    Thread.Sleep(2000);

                                                    if (chrome.CheckTextInChrome("How to unlock your account with your current device", "How to unlock your account with your current device"))
                                                    {
                                                        chrome.FindClickElement("Next", "Next", true);
                                                        Thread.Sleep(5000);
                                                    }

                                                    if (chrome.CheckTextInChrome("Confirm this is your account", "Confirm this is your account"))
                                                    {
                                                        Thread.Sleep(5000);
                                                        if (chrome.CheckTextInChrome("Get a code by email", "Get a code by email"))
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Checkpoint 956 gửi mail đang giải...");
                                                            chrome.FindClickElement("Get a code by email", "Get a code by email", true);
                                                            Thread.Sleep(5000);
                                                            if (chrome.CheckTextInChrome("Get a code by email", "Get a code by email"))
                                                            {
                                                                chrome.FindClickElement("Get code", "Get code", true);
                                                                Thread.Sleep(5000);

                                                                string otpHotmailNew = "";
                                                                if (chrome.CheckTextInChrome("Enter code", "Enter code"))
                                                                {
                                                                    string mailCheck = cEmail.Split(new char[] { '@' })[0].Substring(cEmail.Split(new char[] { '@' })[0].Length - 1) + "@" + cEmail.Split(new char[] { '@' })[1];
                                                                    Thread.Sleep(2000);
                                                                    if (!chrome.CheckTextInChrome(mailCheck, mailCheck))
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Mail không trùng!");
                                                                        SetInfoAccount(cId, indexRow, "Mail Changed");
                                                                        SetRowColor(indexRow, 1);
                                                                        goto stop;
                                                                    }
                                                                    goto getOtp;

                                                                getOtp:
                                                                    for (; ; )
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Đang lấy mã từ hotmail...");
                                                                        otpHotmailNew = CuaGetMail.getOtpMail(cEmail, cPassMail, "security@facebookmail.com");
                                                                        bool checkmail = otpHotmailNew.ToString() != "";
                                                                        if (checkmail)
                                                                        {
                                                                            goto giaiCheckpoint;
                                                                        }
                                                                        if (num > 2)
                                                                        {
                                                                            SetStatusAccount(indexRow, cStatus + "Mail không về!");
                                                                            goto stop;
                                                                        }
                                                                        num++;
                                                                    }

                                                                giaiCheckpoint:
                                                                    SetStatusAccount(indexRow, cStatus + "OTP: " + otpHotmailNew);
                                                                    for (; ; )
                                                                    {
                                                                        chrome.SendKeys(3, "/html/body/div/div/div/div/div/div/div/div/div[2]/div/div/div/div[1]/div/div/div[1]/div/div/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div/div/label/div/div/input", otpHotmailNew, 0.1);
                                                                        Thread.Sleep(2000);
                                                                        chrome.FindClickElement("Submit", "Submit", true);
                                                                        Thread.Sleep(3000);
                                                                        bool checkOtp = chrome.CheckTextInChrome("Incorrect Code", "Incorrect Code") | chrome.CheckTextInChrome("The code you entered is incorrect", "The code you entered is incorrect");
                                                                        if (checkOtp)
                                                                        {
                                                                            SetStatusAccount(indexRow, cStatus + "OTP Lỗi!");
                                                                            goto stop;
                                                                        }
                                                                        break;
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Next steps to unlock your account", "Next steps to unlock your account"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Next", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Secure your login details", "Secure your login details"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Next", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Your new login details", "Your new login details"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Next", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("You've unlocked your account", "You've unlocked your account"))
                                                                    {
                                                                        chrome.FindClickElement("Back to Facebook", "Back to Facebook", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    CommonChrome.GoToHome(chrome);
                                                                    if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Unlock 956 Xong!.");
                                                                        SetInfoAccount(cId, indexRow, "Live");
                                                                        goto stop;
                                                                    }
                                                                    else
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                                        goto stop;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (chrome.CheckTextInChrome("Confirm your identity", "Confirm your identity"))
                                                        {
                                                            chrome.FindClickElement("Confirm your identity", "Confirm your identity", true);
                                                            Thread.Sleep(5000);
                                                        }
                                                    }

                                                    Thread.Sleep(2000);
                                                    if (chrome.CheckTextInChrome("Help Us Confirm Your Identity", "Help Us Confirm Your Identity"))
                                                    {
                                                        SetStatusAccount(indexRow, cStatus + "Checkpoint 956 Xác minh mới!");
                                                        SetInfoAccount(cId, indexRow, "CP 956-XMDT");
                                                        SetRowColor(indexRow, 1);
                                                        goto stop;
                                                    }

                                                }
                                                else
                                                {
                                                    SetStatusAccount(indexRow, cStatus + "956 - Bắt Đầu");
                                                    SetStatusAccount(indexRow, cStatus + "Checkpoint 956");
                                                    changeLanguage(chrome);

                                                    chrome.GotoURL("https://www.facebook.com");
                                                    Thread.Sleep(500);

                                                    chrome.FindClickElement("Get started", "Bắt đầu", true);
                                                    Thread.Sleep(2000);

                                                    if (chrome.CheckTextInChrome("How to unlock your account", "Next steps to unlock your account") || chrome.CheckTextInChrome("Cách mở khóa tài khoản", "Cách mở khóa tài khoản"))
                                                    {
                                                        chrome.FindClickElement("Tiếp", "Next", true);
                                                        Thread.Sleep(5000);
                                                        if (chrome.CheckTextInChrome("Secure your login details", "Secure your login details"))
                                                        {
                                                            chrome.FindClickElement("Tiếp", "Next", true);
                                                            Thread.Sleep(5000);

                                                            if (chrome.CheckTextInChrome("Your new login details", "Your new login details"))
                                                            {
                                                                chrome.FindClickElement("Tiếp", "Next", true);
                                                                Thread.Sleep(5000);

                                                                if (chrome.CheckTextInChrome("You've unlocked your account", "You've unlocked your account"))
                                                                {
                                                                    chrome.FindClickElement("Back to Facebook", "Back to Facebook", true);
                                                                    Thread.Sleep(5000);
                                                                }

                                                                CommonChrome.GoToHome(chrome);
                                                                if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                                                {
                                                                    SetStatusAccount(indexRow, cStatus + "Unlock 956 Xong!.");
                                                                    SetInfoAccount(cId, indexRow, "Live");
                                                                    goto stop;
                                                                }
                                                                else
                                                                {
                                                                    SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                                    goto stop;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (chrome.CheckTextInChrome("Confirm this is your account", "Xác nhận đây là tài khoản của bạn"))
                                                    {
                                                        Thread.Sleep(5000);
                                                        if (chrome.CheckTextInChrome("Get a code by email", "Nhận mã qua email"))
                                                        {
                                                            SetStatusAccount(indexRow, cStatus + "Checkpoint 956 gửi mail đang giải...");
                                                            chrome.FindClickElement("Get a code by email", "Nhận mã qua email", true);
                                                            Thread.Sleep(5000);
                                                            if (chrome.CheckTextInChrome("Get a code by email", "Nhận mã qua email"))
                                                            {
                                                                chrome.FindClickElement("Get code", "Nhận mã", true);
                                                                Thread.Sleep(5000);

                                                                string otpHotmailNew = "";
                                                                if (chrome.CheckTextInChrome("Enter code", "Nhập mã"))
                                                                {
                                                                    string mailCheck = cEmail.Split(new char[] { '@' })[0].Substring(cEmail.Split(new char[] { '@' })[0].Length - 1) + "@" + cEmail.Split(new char[] { '@' })[1];
                                                                    Thread.Sleep(2000);
                                                                    if (!chrome.CheckTextInChrome(mailCheck, mailCheck))
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Mail không trùng!");
                                                                        SetInfoAccount(cId, indexRow, "Mail Changed");
                                                                        SetRowColor(indexRow, 1);
                                                                        goto stop;
                                                                    }
                                                                    goto getOtp;

                                                                getOtp:
                                                                    for (; ; )
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Đang lấy mã từ hotmail...");
                                                                        otpHotmailNew = CuaGetMail.getOtpMail(cEmail, cPassMail, "security@facebookmail.com");
                                                                        bool checkmail = otpHotmailNew.ToString() != "";
                                                                        if (checkmail)
                                                                        {
                                                                            goto giaiCheckpoint;
                                                                        }
                                                                        if (num > 2)
                                                                        {
                                                                            SetStatusAccount(indexRow, cStatus + "Mail không về!");
                                                                            goto stop;
                                                                        }
                                                                        num++;
                                                                    }

                                                                giaiCheckpoint:
                                                                    SetStatusAccount(indexRow, cStatus + "OTP: " + otpHotmailNew);
                                                                    for (; ; )
                                                                    {
                                                                        chrome.SendKeys(3, "/html/body/div[1]/div/div/div/div/div/div/div[2]/div/div/div[1]/div/div/div[1]/div/div/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div/div/label/div/div/input", otpHotmailNew, 0.1);
                                                                        Thread.Sleep(2000);
                                                                        chrome.FindClickElement("Submit", "Gửi", true);
                                                                        Thread.Sleep(3000);
                                                                        bool checkOtp = chrome.CheckTextInChrome("Incorrect Code", "Incorrect Code") | chrome.CheckTextInChrome("The code you entered is incorrect", "The code you entered is incorrect");
                                                                        if (checkOtp)
                                                                        {
                                                                            SetStatusAccount(indexRow, cStatus + "OTP Lỗi!");
                                                                            goto stop;
                                                                        }
                                                                        break;
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Next steps to unlock your account", "Next steps to unlock your account"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Tiếp", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Secure your login details", "Secure your login details"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Tiếp", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("Your new login details", "Your new login details"))
                                                                    {
                                                                        chrome.FindClickElement("Next", "Tiếp", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    if (chrome.CheckTextInChrome("You've unlocked your account", "You've unlocked your account"))
                                                                    {
                                                                        chrome.FindClickElement("Back to Facebook", "Back to Facebook", true);
                                                                        Thread.Sleep(5000);
                                                                    }

                                                                    CommonChrome.GoToHome(chrome);
                                                                    if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Unlock 956 Xong!.");
                                                                        SetInfoAccount(cId, indexRow, "Live");
                                                                        goto stop;
                                                                    }
                                                                    else
                                                                    {
                                                                        SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                                        goto stop;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (chrome.CheckTextInChrome("Confirm your identity", "Confirm your identity"))
                                                        {
                                                            chrome.FindClickElement("Confirm your identity", "Confirm your identity", true);
                                                            Thread.Sleep(5000);
                                                        }
                                                    }

                                                    if (chrome.CheckTextInChrome("Help Us Confirm Your Identity", "Help Us Confirm Your Identity"))
                                                    {
                                                        chrome.FindClickElement("Get started", "Bắt đầu", true);
                                                        Thread.Sleep(5000);
                                                    }

                                                    if (chrome.CheckTextInChrome("We'll take you through a few steps", "We'll take you through a few steps"))
                                                    {
                                                        SetStatusAccount(indexRow, cStatus + "Checkpoint 956 Xác minh mới!");
                                                        SetInfoAccount(cId, indexRow, "CP 956-XMDT");
                                                        SetRowColor(indexRow, 1);
                                                        goto stop;
                                                    }
                                                }
                                            }
                                        }
                                        else if (urlCurrent.Contains("checkpoint/disabled") || chrome.CheckTextInChrome("disabled your account", "disabled your account"))
                                        {
                                            SetInfoAccount(cId, indexRow, "VHH");
                                            SetStatusAccount(indexRow, "Nick Vô Hiệu Hoá! Vứt!");
                                            CommonSQL.UpdateFieldToAccount(cId, "status282", "");
                                            SetCellAccount(indexRow, "cStatus282", "");
                                            SetRowColor(indexRow, 1);
                                            goto stop;
                                        }
                                        else if (Regex.IsMatch(urlCurrent, "5049"))
                                        {
                                            SetStatusAccount(indexRow, cStatus + "049 - Cp Spam");

                                            if (chrome.CheckTextInChrome("We suspect automated behavior on your account", "We suspect automated behavior on your account"))
                                            {
                                                chrome.FindClickElement("Dismiss", "Dismiss", true);
                                                Thread.Sleep(2000);
                                            }

                                            CommonChrome.GoToHome(chrome);
                                            if (CommonChrome.CheckLiveCookie(chrome) == 1)
                                            {
                                                SetStatusAccount(indexRow, cStatus + "Unlock 049 Xong!.");
                                                SetInfoAccount(cId, indexRow, "Live");
                                                goto stop;
                                            }
                                            else
                                            {
                                                SetStatusAccount(indexRow, cStatus + "Tạch.");
                                                goto stop;
                                            }
                                        }
                                        else
                                        {
                                            SetStatusAccount(indexRow, cStatus + "Checkpoint Loại khác...");

                                        }

                                        
                                        goto stop;
                                        //run thread
                                        //string ccnew = ConvertCookie(chrome.GetCookieFromChrome());
                                        //CommonSQL.UpdateFieldToAccount(cId, "uid", cUid);
                                        //CommonSQL.UpdateFieldToAccount(cId, "cookie1", ccnew);
                                        //SetCellAccount(indexRow, "cCookies", ccnew);
                                        //SetStatusAccount(indexRow, cStatus + "Get Cookie Thành Công.");
                                        //Helpers.Common.DelayTime(1);
                                        //SetStatusAccount(indexRow, cStatus + ("Bắt đầu lấy Token!"));
                                        //chrome.GotoURL("https://business.facebook.com/content_management");
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
                    CloseFormViewChrome();
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

        private async Task<bool> submitImage282(Chrome chrome, int indexRow, string cUid, string cId)
        {
            bool result = false;
            SetStatusAccount(indexRow, "Chuẩn bị ảnh...");
            string urlImg = "";
            string directoryPath = Path.Combine("backup", cUid, "image");
            Directory.CreateDirectory(directoryPath);

            if (setting_GiaiCheckpoint.GetValue("imageType") == "0")
            {
                for (; ; )
                {
                    if (setting_GiaiCheckpoint.GetValueBool("ckbAnhCu"))
                    {
                        string pathImgUid = directoryPath;

                        List<string> lstFrom = new List<string>();
                        lstFrom = Directory.GetFiles(pathImgUid).ToList();

                        if (lstFrom.Count == 0)
                        {
                            List<string> lstAnhSan = new List<string>();
                            lstAnhSan = Directory.GetFiles(setting_GiaiCheckpoint.GetValue("txtAnhBackup").ToString()).ToList();
                            if (lstAnhSan.Count == 0)
                            {
                                urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
                                break;
                            }
                            else
                            {
                                urlImg = Path.GetFullPath(lstAnhSan[rd.Next(0, lstAnhSan.Count)]);
                                urlImg = FileHelper.CoppyFile(urlImg, directoryPath + "\\" + Path.GetFileName(urlImg));

                                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                                urlImg = FileHelper.renameFile(urlImg, "ul282_" + currentTimestamp.ToString());
                                if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
                                {
                                    WebImage.ChangeMD5OfFile(urlImg);
                                }
                                break;
                            }
                        }
                        else
                        {
                            urlImg = Path.GetFullPath(lstFrom[rd.Next(0, lstFrom.Count)]);
                            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            urlImg = FileHelper.renameFile(urlImg, "rn_ul282_" + currentTimestamp.ToString());

                            if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
                            {
                                WebImage.ChangeMD5OfFile(urlImg);
                            }
                            break;
                        }
                    }
                    else
                    {
                        List<string> lstAnhSan = new List<string>();
                        lstAnhSan = Directory.GetFiles(setting_GiaiCheckpoint.GetValue("txtAnhBackup").ToString()).ToList();
                        if (lstAnhSan.Count == 0)
                        {
                            urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
                            break;
                        }
                        else
                        {
                            urlImg = Path.GetFullPath(lstAnhSan[rd.Next(0, lstAnhSan.Count)]);
                            urlImg = FileHelper.CoppyFile(urlImg, directoryPath + Path.GetFileName(urlImg));

                            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            urlImg = FileHelper.renameFile(urlImg, "ul282_" + currentTimestamp.ToString());
                            if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
                            {
                                WebImage.ChangeMD5OfFile(urlImg);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                for (; ; )
                {
                    if (setting_GiaiCheckpoint.GetValueBool("ckbAnhCu"))
                    {
                        string pathImgUid = directoryPath;

                        List<string> lstFrom = new List<string>();
                        lstFrom = Directory.GetFiles(pathImgUid).ToList();

                        if (lstFrom.Count == 0)
                        {
                            urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
                            break;
                        }
                        else
                        {
                            urlImg = Path.GetFullPath(lstFrom[rd.Next(0, lstFrom.Count)]);
                            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            urlImg = FileHelper.renameFile(urlImg, "rn_ul282_" + currentTimestamp.ToString());

                            if (setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"))
                            {
                                WebImage.ChangeMD5OfFile(urlImg);
                            }
                            break;
                        }
                    }
                    else
                    {
                        urlImg = await getImageWeb(cUid, "ul282", setting_GiaiCheckpoint.GetValue("cbbAnhWeb"), setting_GiaiCheckpoint.GetValueBool("ckChangeMd5Anh"));
                        break;
                    }
                }
            }
            Thread.Sleep(1000);

            if (!string.IsNullOrEmpty(urlImg))
            {
                Thread.Sleep(500);
                chrome.SendKeys(2, "mobile_image_data", urlImg);
                chrome.Click(2, "action_upload_image");
                Thread.Sleep(500);
                if (chrome.CheckTextInChrome("You can only upload", "You can only upload"))
                {
                    SetStatusAccount(indexRow, "Up ảnh thất bại");
                    return false;
                }
                SetStatusAccount(indexRow, "Đợi xét duyệt - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                CommonSQL.UpdateFieldToAccount(cId, "status282", "Đợi xét duyệt - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetCellAccount(indexRow, "cStatus282", "Đợi xét duyệt - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetRowColor(indexRow, 1);

                return true;
            }

            return result;
        }
        private void changeLanguage(Chrome chrome)
        {
            try
            {
                chrome.GotoURL("https://d.facebook.com/language/");
                chrome.ExecuteScript("document.querySelector(\"input[value='English (US)']\").click();");
                chrome.ExecuteScript("document.querySelector(\"div[value='en_US']\").click();");
            }
            catch { }
        }

        private string giaiCaptcha(int indexRow, Chrome chrome, string cbbSiteCaptcha, string txtCaptchaKey)
        {
            SetStatusAccount(indexRow, "Đang giải captcha...");
            string code = "";
            try
            {
                if (cbbSiteCaptcha == "omocaptcha")
                {

                    IWebElement element = chrome.returnRowElement("//*[contains(@src,'captcha/tfbimage')]");
                    Bitmap imagesCaptcha = GetElementScreenShot(chrome, element, indexRow);
                    string ImagesBase64 = BitmapToBase64(imagesCaptcha);
                    string Apikey = txtCaptchaKey;
                    string jobId = WebCaptcha.createTaskOmocaptcha(Apikey, ImagesBase64);
                    string resul = "";
                    for (int i = 0; i < 10; i++)
                    {
                        resul = WebCaptcha.getResultOmocaptcha(Apikey, jobId);
                        if (resul != "")
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    if (resul == "")
                    {
                        code = resul;
                    }

                    code = resul;
                }else if(cbbSiteCaptcha == "nopecha")
                {
                    IWebElement element = chrome.returnRowElement("//*[contains(@src,'captcha/tfbimage')]");
                    Bitmap imagesCaptcha = GetElementScreenShot(chrome, element, indexRow);
                    string ImagesBase64 = BitmapToBase64(imagesCaptcha);
                    string Apikey = "sub_1OPgq1CRwBwvt6ptW5FX94uX";
                    string jobId = WebCaptcha.createTaskNopecha(Apikey, ImagesBase64);
                    string resul = "";
                    for (int i = 0; i < 10; i++)
                    {
                        resul = WebCaptcha.getResultNopecha(Apikey, jobId);
                        if (!string.IsNullOrEmpty(resul))
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    if (resul == "")
                    {
                        code = resul;
                    }

                    code = resul;
                }    
            }
            catch { }

            return code;
        }
        public async Task<string> getImageWeb(string uid, string namefile, string cbbAnhWeb, bool ckChangeMd5Anh)
        {
            string urlImg = "";
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            namefile += "_" + currentTimestamp.ToString();

            if (cbbAnhWeb == "unrealperson")
            {
                urlImg = await WebImage.downloadUnrealperson(uid);
            }
            else if (cbbAnhWeb == "thispersondoesnotexist")
            {
                urlImg = await WebImage.dowloadThispersondoesnotexist(uid, namefile);
            }

            if (ckChangeMd5Anh)
            {
                WebImage.ChangeMD5OfFile(urlImg);
            }

            return urlImg;
        }

        public Bitmap GetElementScreenShot(Chrome chrome, IWebElement element, int indexRow)
        {
            Random random = new Random();
            int num = random.Next(1111111, 99999999);
            string cUid = GetCellAccount(indexRow, "cUid") + "_" + num;
            Screenshot sc = chrome.ScreenCaptureV2("log_capture\\log_captcha", cUid);
            int width = element.Size.Width;
            int height = element.Size.Height;
            Point location = element.Location;

            var img = System.Drawing.Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            return img.Clone(new Rectangle(location.X, location.Y, width, height), img.PixelFormat);
        }

        public string BitmapToBase64(Bitmap image)
        {
            using (var ms = new MemoryStream())
            {
                using (var bitmap = new Bitmap(image))
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    return Convert.ToBase64String(ms.GetBuffer());
                }
            }
        }

        private void autoBASETUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new fBasetup().Show();
            this.Hide();
        }

        private void dtgvAcc_Paint(object sender, PaintEventArgs e)
        {
            CountTotalSelectedRow();
        }
        private void CountTotalSelectedRow()
        {
            try
            {
                this.lblCountPaint.Text = dtgvAcc.SelectedRows.Count.ToString();
            }
            catch { }
        }

        private void btnVipAuto_Click(object sender, EventArgs e)
        {
            new fToolVip().Show();
        }

        private void gửiTokenLênToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                UpdateInfoAccount(1);
            }
        }

        private void tokenEAABwinstagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                UpdateInfoAccount(3);
            }
        }

        private void cậpNhậpThôngTinToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void GetTokenNew(int type, string idApp)
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
                    cControl("start");
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
                                                SetStatusAccount(row3, "Đang xử lý...");
                                                RunThreadGetTokenNew(row3, idApp);
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
                    }
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 60000)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                    cControl("stop");
                }).Start();
            }
        }
        private void RunThreadGetTokenNew(int row, string idApp)
        {
            try
            {
                string cId = GetCellAccount(row, "cId");
                string cUid = GetCellAccount(row, "cUid");
                string cCookie = GetCellAccount(row, "cCookies");
                string userAgent = null;
                string proxy = "";
                int typeProxy = 0;
                if (CheckIsUidFacebook(cUid) && CommonRequest.CheckLiveWall(cUid).StartsWith("0|"))
                {
                    SetCellAccount(row, "cCookies", "");
                    SetCellAccount(row, "cToken", "");
                    SetStatusAccount(row, "Nick Die Rồi!");
                    SetInfoAccount(cId, row, "Die");
                    CommonSQL.UpdateFieldToAccount(cId, "cookie1", "");
                    CommonSQL.UpdateFieldToAccount(cId, "token", "");
                    return;
                }
                if (!CommonRequest.CheckLiveCookie(cCookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                {
                    SetCellAccount(row, "cCookies", "");
                    SetCellAccount(row, "cToken", "");
                    SetStatusAccount(row, "Cookie Die!");
                    CommonSQL.UpdateFieldToAccount(cId, "cookie1", "");
                    CommonSQL.UpdateFieldToAccount(cId, "token", "");
                    return;
                }

                if (idApp == "instagram")
                {
                    string token = CommonRequest.getTokenInstaNew(cCookie, proxy);
                    if (!string.IsNullOrEmpty(token))
                    {
                        SetCellAccount(row, "cCookies", cCookie);
                        SetCellAccount(row, "cToken", token);
                        SetStatusAccount(row, "Get Token Instagram Thành Công!");
                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                        CommonSQL.UpdateFieldToAccount(cId, "token", token);
                        return;
                    }
                    else
                    {
                        SetStatusAccount(row, "Get Token Thất Bại!");
                        return;
                    }
                }

                string fb_dtsg = CommonRequest.getFbdtsgNew(cCookie, proxy);
                if (!string.IsNullOrEmpty(fb_dtsg))
                {
                    string token = CommonRequest.getTokenAppNew(cCookie, idApp, fb_dtsg, proxy);

                    if (!string.IsNullOrEmpty(token))
                    {
                        SetCellAccount(row, "cCookies", cCookie);
                        SetCellAccount(row, "cToken", token);
                        SetStatusAccount(row, "Get Token Thành Công!");
                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", cCookie);
                        CommonSQL.UpdateFieldToAccount(cId, "token", token);
                        return;
                    }
                    else
                    {
                        SetStatusAccount(row, "Get Token Thất Bại!");
                        return;
                    }
                }
                else
                {
                    SetStatusAccount(row, "Get Token Thất Bại!");
                    return;
                }
            }
            catch
            {
                SetStatusAccount(row, "Lỗi!");
            }
        }

        private void facebookForAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "350685531728");
            }
        }

        private void facebookLiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "275254692598279");
            }
        }

        private void messengerForAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "256002347743983");
            }
        }

        private void facebookForIphoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "6628568379");
            }
        }

        private void messengerForIphoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "237759909591655");
            }
        }

        private void messengerForLiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "200424423651082");
            }
        }

        private void aDSManagerAppAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "438142079694454");
            }
        }

        private void aDSManagerAppIOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "1479723375646806");
            }
        }

        private void messengerForIphoneDevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "202805033077166");
            }
        }

        private void pageIOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "165907476854626");
            }
        }

        private void pageAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "121876164619130");
            }
        }

        private void pageWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "121876164619130");
            }
        }

        private void businessManagerBusinessManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "436761779744620");
            }
        }

        private void messengerKidsIOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "436761779744620");
            }
        }

        private void messengerIOSHouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "184182168294603");
            }
        }

        private void facebookIpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "173847642670370");
            }
        }

        private void instagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy token!", 3);
            }
            else
            {
                GetTokenNew(0, "instagram");
            }
        }

        private void quétUIDCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fScanLive());
        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            UpdateDataGridViewsWidth();
        }
        private void UpdateDataGridViewsWidth()
        {
            int panelHeight = panel2.Height;
            int panelViewWidth = panelHeight / 2;

            panel1.Height = panelViewWidth;
            panel3.Height = panelViewWidth - 5;
        }

        //page
        private void LoadPageFromFile()
        {
            try
            {
                dtgvPage.Rows.Clear();
                DataTable dataPages = CommonSQL.GetPageFromFile(null, "");
                LoadDtgvPageFromDatatable(dataPages);
            }
            catch (Exception)
            {
            }
        }
        private void LoadDtgvPageFromDatatable(DataTable dataPages)
        {

            for (int i = 0; i < dataPages.Rows.Count; i++)
            {
                DataRow dataRow = dataPages.Rows[i];
                dtgvPage.Rows.Add(false, dtgvPage.RowCount + 1, dataRow["id"], dataRow["pageId"], dataRow["pageName"], dataRow["like"], dataRow["follow"], dataRow["tiepcan"], dataRow["idbusiness"], dataRow["uid"], dataRow["token"], dataRow["countgroup"], dataRow["tuongtac"], dataRow["datecreated"], dataRow["avatar"], "Mặc định", dataRow["info"], dataRow["lastInteract"], "");
            }
            CountCheckedPage(0);
            SetRowColorPage();
            CountTotalPage();
        }

        private void SetRowColorPage()
        {
            for (int j = 0; j < dtgvPage.RowCount; j++)
            {
                string cStatusP = GetInfoPage(j);
                if (cStatusP == "Public")
                {
                    dtgvPage.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                }
                else if (cStatusP.Contains("Die"))
                {
                    dtgvPage.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                }
            }

        }

        private void CountTotalPage()
        {
            try
            {
                lbTotalP.Text = dtgvPage.Rows.Count.ToString();
            }
            catch
            {
            }
        }
        public string GetInfoPage(int indexRow)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvPage, indexRow, "cStatusP");
        }

        private void lấyCookieNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy cookie!", 3);
            }
            else
            {
                GetCookieApp(0);
            }
        }
        private void GetCookieApp(int type)
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
                isStop = false;
                new Thread((ThreadStart)delegate
                {
                    cControl("start");
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
                                                SetStatusAccount(row3, "Đang xử lý...");
                                                RunThreadGetCookieApp(row3);
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
                    }
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 60000)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                    cControl("stop");
                }).Start();
            }
        }
        private void RunThreadGetCookieApp(int row)
        {
            try
            {
                string cId = GetCellAccount(row, "cId");
                string cUid = GetCellAccount(row, "cUid");
                string cPassword = GetCellAccount(row, "cPassword");
                string c2Fa = GetCellAccount(row, "cFa2");
                string proxy = "";
                int typeProxy = 0;

                string apikey = "882a8490361da98702bf97a021ddc14d";
                string secret = "62f8ce9f74b12f84c123cc23437a4a32";
                string oauth = "350685531728";

                var rs = CommonRequest.RunGetAccessToken2FA(cUid, cPassword, c2Fa, apikey, secret, oauth);

                if (!string.IsNullOrEmpty(rs.Item2.ToString()) && !string.IsNullOrEmpty(rs.Item1.ToString()))
                {
                    string token = rs.Item1.ToString();
                    string cookie = rs.Item2.ToString();

                    if (token == "verify" && cookie == "verify")
                    {
                        SetCellAccount(row, "cCookies", "");
                        SetCellAccount(row, "cToken", "");
                        SetStatusAccount(row, "Xác minh tài khoản!");
                        SetInfoAccount(cId, row, "Checkpoint");
                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", "");
                        CommonSQL.UpdateFieldToAccount(cId, "token", "");
                        return;
                    } else if (token == "invalid" && cookie == "invalid")
                    {
                        SetCellAccount(row, "cCookies", "");
                        SetCellAccount(row, "cToken", "");
                        SetStatusAccount(row, "Sai mật khẩu!");
                        SetInfoAccount(cId, row, "Changed pass");
                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", "");
                        CommonSQL.UpdateFieldToAccount(cId, "token", "");
                        return;
                    }
                    else
                    {
                        SetCellAccount(row, "cCookies", cookie);
                        SetCellAccount(row, "cToken", token);
                        SetStatusAccount(row, "Get Cookie Thành Công!");
                        CommonSQL.UpdateFieldToAccount(cId, "cookie1", cookie);
                        CommonSQL.UpdateFieldToAccount(cId, "token", token);
                        return;
                    }
                }
                else
                {
                    SetStatusAccount(row, "Get Cookie Thất Bại!");
                    return;
                }


            }
            catch
            {
                SetStatusAccount(row, "Lỗi!");
            }
        }

        private void CountCheckedPage(int count = -1)
        {
            if (count == -1)
            {
                count = 0;
                for (int i = 0; i < dtgvPage.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dtgvPage.Rows[i].Cells["cChoosePage"].Value))
                    {
                        count++;
                    }
                }
            }
            lbCheckP.Text = count.ToString();
        }

        private void dtgvPage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    dtgvPage.CurrentRow.Cells["cChoosePage"].Value = !Convert.ToBoolean(dtgvPage.CurrentRow.Cells["cChoosePage"].Value);
                    CountCheckedPage();
                }
                catch
                {
                }
            }
        }

        private void dtgvPage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dtgvPage.CurrentRow.Cells["cChoosePage"].Value = !Convert.ToBoolean(dtgvPage.CurrentRow.Cells["cChoosePage"].Value);
                CountCheckedPage();
            }
            catch
            {
            }
        }

        private void dtgvPage_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (isCountCheckAccountWhenChayTuongTac && e.ColumnIndex == 0)
            //{
            //    CountCheckedPage();
            //}
        }
        private void CountTotalSelectedRowPage()
        {
            try
            {
                this.lbChosseP.Text = dtgvPage.SelectedRows.Count.ToString();
            }
            catch { }
        }

        private void dtgvPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 32)
            {
                ChoseRowInDatagridPage("ToggleCheck");
            }
        }


        private void dtgvPage_Paint(object sender, PaintEventArgs e)
        {
            CountTotalSelectedRowPage();
        }

        private void dtgvPage_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            List<string> list = new List<string> { "cSttP", "cLikeP", "cFollowP", "cGroupP" };
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
        public void SetCellPage(int indexRow, string column, object value, bool isAllowEmptyValue = true)
        {
            if (isAllowEmptyValue || !(value.ToString().Trim() == ""))
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvPage, indexRow, column, value);
            }
        }
        public string GetCellPaget(int indexRow, string column)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvPage, indexRow, column);
        }
        private void ChoseRowInDatagridPage(string modeChose)
        {
            switch (modeChose)
            {
                case "All":
                    {
                        for (int k = 0; k < dtgvPage.RowCount; k++)
                        {
                            SetCellPage(k, "cChoosePage", true);
                        }
                        CountCheckedPage(dtgvPage.RowCount);
                        break;
                    }
                case "UnAll":
                    {
                        for (int j = 0; j < dtgvPage.RowCount; j++)
                        {
                            SetCellPage(j, "cChoosePage", false);
                        }
                        CountCheckedPage(0);
                        break;
                    }
                case "SelectHighline":
                    {
                        DataGridViewSelectedRowCollection selectedRows = dtgvPage.SelectedRows;
                        for (int l = 0; l < selectedRows.Count; l++)
                        {
                            SetCellPage(selectedRows[l].Index, "cChoosePage", true);
                        }
                        CountCheckedPage();
                        break;
                    }
                case "ToggleCheck":
                    {
                        for (int i = 0; i < dtgvPage.SelectedRows.Count; i++)
                        {
                            int index = dtgvPage.SelectedRows[i].Index;
                            SetCellPage(index, "cChoosePage", !Convert.ToBoolean(GetCellPaget(index, "cChoosePage")));
                        }
                        CountCheckedPage();
                        break;
                    }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel3.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void đổiThôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CountChooseRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn Đổi thông tin!", 3);
                    return;
                }
                List<string> list = new List<string>();
                for (int i = 0; i < dtgvAcc.RowCount; i++)
                {
                    if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        DataGridViewRow dataGridViewRow = dtgvAcc.Rows[i];
                        try
                        {
                            list.Add(dataGridViewRow.Cells["cStt"].Value.ToString() + "|" + dataGridViewRow.Cells["cId"].Value.ToString() + "|" + dataGridViewRow.Cells["cUid"].Value.ToString() + "|" + dataGridViewRow.Cells["cCookies"].Value.ToString() + "|" + dataGridViewRow.Cells["cName"].Value.ToString() + "|" + dataGridViewRow.Cells["cBirthday"].Value.ToString() + "|" + dataGridViewRow.Cells["cGender"].Value.ToString() + "|" + dataGridViewRow.Cells["cPassword"].Value.ToString() + "|" + dataGridViewRow.Cells["cFa2"].Value.ToString() + "|" + dataGridViewRow.Cells["cUseragent"].Value.ToString() + "|" + dataGridViewRow.Cells["cProxy"].Value.ToString() + "|" + dataGridViewRow.Cells["cInfo"].Value.ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                Helpers.Common.ShowForm(new fChangeInfo(list));
                if (fChangeInfo.isAdd)
                {
                    LoadAccountFromFile(GetIdFile(), cbbTinhTrang.Text);
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "Open Form ChangeInfo");
            }
        }

        private void thêmThưMụcMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fAddFolder());
            if (fAddFolder.isAdd)
            {
                LoadCbbThuMuc();
                cbbThuMuc.SelectedIndex = cbbThuMuc.Items.Count - 2;
            }
        }

        private void btnStopThaoTac_Click_1(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                btnStopThaoTac.Enabled = false;
                gunaButton7.Enabled = false;
                ShowTrangThai("");
            }
            catch
            {
            }
        }

        private void btnStartThaotac_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (CountChooseRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản muốn chạy!", 3);
                    return;
                }
                string profilePath = ConfigHelper.GetPathProfile();
                if (!Directory.Exists(profilePath))
                {
                    MessageBoxHelper.ShowMessageBox("Đường dẫn profile không hợp lệ!", 3);
                    return;
                }
                if (Base.useragentDefault == "")
                {
                    Base.useragentDefault = CommonChrome.GetUserAgentDefault();
                    if (Base.useragentDefault == "")
                    {
                        MessageBoxHelper.ShowMessageBox("Phiên bản Chromedriver hiện tại không khả dụng, vui lòng cập nhật!", 3);
                        return;
                    }
                }
                LoadSetting();
                int maxThread = setting_general.GetValueInt("nudInteractThread", 2);
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
                if (setting_general.GetValueBool("ckbAddChromeIntoForm"))
                {
                    OpenFormViewChrome();
                }
                isCountCheckAccountWhenChayTuongTac = true;
                isStop = false;
                int curChangeIp = 0;
                bool isChangeIPSuccess = false;
                checkDelayChrome = 0;
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    //running
                    try
                    {
                        List<string> list = new List<string>();
                        string idKichBan = "";
                        string text = "";
                        int num3 = setting_InteractGeneral.GetValueInt("nudSoLuotChay", 1);
                        if (num3 == 0)
                        {
                            num3 = 1;
                        }
                        int num4 = 0;
                        while (num4 < num3)
                        {
                            //processing
                            list = new List<string>();
                            if (setting_InteractGeneral.GetValueBool("ckbRepeatAll"))
                            {
                                text = (num3 > 1) ? string.Format("Lượt {0}/{1}. ", num4 + 1, num3) : "";
                                //random kich ban
                                if (setting_InteractGeneral.GetValueBool("RepeatAllVIP"))
                                {
                                    list = setting_InteractGeneral.GetValueList("lstIdKichBan");
                                    if (setting_InteractGeneral.GetValueBool("ckbRandomKichBan"))
                                    {
                                        list = Helpers.Common.ShuffleList(list);
                                        list = Helpers.Common.ShuffleList(list);
                                        list = Helpers.Common.ShuffleList(list);
                                    }
                                }
                                else
                                {
                                    list.Add(setting_InteractGeneral.GetValue("cbbKichBan"));
                                }
                            }
                            else
                            {
                                list.Add(setting_InteractGeneral.GetValue("cbbKichBan"));
                            }
                            int num5 = 0;

                            while (true)
                            {
                                if (num5 < list.Count && !isStop)
                                {
                                    idKichBan = list[num5];
                                    if (text != "")
                                    {
                                        ShowTrangThai(text + string.Format("Kịch bản" + ": {0}...", InteractSQL.GetTenKichBanById(idKichBan)));
                                    }
                                    if (setting_InteractGeneral.GetValueBool("ckbRepeatAll") && setting_InteractGeneral.GetValueBool("RepeatAllVIP") && setting_InteractGeneral.GetValueBool("ckbRandomThuTuTaiKhoan"))
                                    {
                                        dtgvAcc.Invoke((MethodInvoker)delegate
                                        {
                                            RandomAccountListView();
                                        });
                                    }
                                    if (setting_InteractGeneral.GetValueInt("typeInteract") == 1)
                                    {
                                        dicDangStatus_NoiDung = GetDictionaryIntoHanhDong(idKichBan, "HDDangStatus", "txtNoiDung");
                                        dicDangStatus_NoiDungGoc = GetDictionaryIntoHanhDong(idKichBan, "HDDangStatus", "txtNoiDung");
                                        //comment
                                        dicSpamComment_NoiDung = GetDictionaryIntoHanhDong(idKichBan, "SpamCommentPage", "txtNoiDung");
                                        dicSpamComment_NoiDungGoc = GetDictionaryIntoHanhDong(idKichBan, "SpamCommentPage", "txtNoiDung");
                                        dicSpamComment_listUid = GetDictionaryIntoHanhDong(idKichBan, "SpamCommentPage");
                                        //reg page
                                        dicHDRegPage_Name = GetDictionaryIntoHanhDong(idKichBan, "HDRegPage", "txtlistName");
                                        dicHDRegPage_NameGoc = GetDictionaryIntoHanhDong(idKichBan, "HDRegPage", "txtlistName");
                                        //seeding fb
                                        dicSeedingFb_NoiDung = GetDictionaryIntoHanhDong(idKichBan, "SeedingFb", "txtNoiDung");
                                        dicSpamComment_NoiDungGoc = GetDictionaryIntoHanhDong(idKichBan, "SeedingFb", "txtNoiDung");
                                        //ket bạn uid
                                        dicHDKetBanUid = GetDictionaryIntoHanhDong(idKichBan, "HDKetBanUid", "txtListUid");
                                    }
                                    int num6 = 0;
                                    while (num6 < dtgvAcc.Rows.Count && !isStop)
                                    {
                                        if (!Convert.ToBoolean(dtgvAcc.Rows[num6].Cells["cChose"].Value))
                                        {
                                            num6++;
                                            goto checkStop;
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
                                            int row = num6++;
                                            Thread thread = new Thread((ThreadStart)delegate
                                            {
                                                try
                                                {
                                                    int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                                    ExcuteOneThread(row, indexOfPossitionApp, idKichBan, profilePath);
                                                    Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                                    if (!setting_InteractGeneral.GetValueBool("ckbRepeatAll"))
                                                    {
                                                        SetCellAccount(row, "cChose", false);
                                                    }
                                                }
                                                catch (Exception ex3)
                                                {
                                                    Helpers.Common.ExportError(null, ex3);
                                                }
                                            })
                                            {
                                                Name = row.ToString()
                                            };
                                            lstThread.Add(thread);
                                            Helpers.Common.DelayTime(1.0);
                                            thread.Start();
                                            goto checkStop;
                                        }
                                        if (isStop)
                                        {
                                            break;
                                        }
                                        if (setting_general.GetValueInt("ip_iTypeChangeIp") == 0 || setting_general.GetValueInt("ip_iTypeChangeIp") == 1 || setting_general.GetValueInt("ip_iTypeChangeIp") == 2 || setting_general.GetValueInt("ip_iTypeChangeIp") == 3)
                                        {
                                            for (int num8 = 0; num8 < lstThread.Count; num8++)
                                            {
                                                if (!lstThread[num8].IsAlive)
                                                {
                                                    lstThread.RemoveAt(num8--);
                                                }
                                            }
                                            goto checkStop;
                                        }
                                        for (int num9 = 0; num9 < lstThread.Count; num9++)
                                        {
                                            lstThread[num9].Join();
                                            lstThread.RemoveAt(num9--);
                                        }
                                        if (isStop)
                                        {
                                            break;
                                        }
                                        Interlocked.Increment(ref curChangeIp);
                                        if (curChangeIp < setting_general.GetValueInt("ip_nudChangeIpCount", 1))
                                        {
                                            goto checkStop;
                                        }
                                        MessageBoxHelper.ShowMessageBox("Lỗi change ip!", 2);
                                        goto end;
                                    checkStop:
                                        if (isStop)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 0; i < lstThread.Count; i++)
                                    {
                                        lstThread[i].Join();
                                    }
                                    if (setting_InteractGeneral.GetValueInt("typeInteract") == 1)
                                    {
                                        SaveDictionaryIntoHanhDong(dicSpamComment_listUid);
                                    }
                                    if (!isStop)
                                    {
                                        if (num5 + 1 < list.Count)
                                        {
                                            int num12 = Base.rd.Next(setting_InteractGeneral.GetValueInt("nudDelayKichBanFrom"), setting_InteractGeneral.GetValueInt("nudDelayKichBanTo") + 1);
                                            int tickCount = Environment.TickCount;
                                            while ((Environment.TickCount - tickCount) / 1000 - num12 < 0)
                                            {
                                                ShowTrangThai(text + string.Format("Chạy kịch bản tiếp theo sau {time} giây...".Replace("{time}", (num12 - (Environment.TickCount - tickCount) / 1000).ToString())));
                                                Helpers.Common.DelayTime(0.5);
                                                if (isStop)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        num5++;
                                        continue;
                                    }
                                }
                                if (!setting_InteractGeneral.GetValueBool("ckbRepeatAll") || isStop)
                                {
                                    break;
                                }
                                if (num4 + 1 < num3)
                                {
                                    int num13 = Base.rd.Next(setting_InteractGeneral.GetValueInt("nudDelayTurnFrom"), setting_InteractGeneral.GetValueInt("nudDelayTurnTo") + 1) * 60;
                                    int tickCount2 = Environment.TickCount;
                                    while ((Environment.TickCount - tickCount2) / 1000 - num13 < 0)
                                    {
                                        ShowTrangThai(text + string.Format("Chạy lượt tiếp theo sau {time} giây...".Replace("{time}", (num13 - (Environment.TickCount - tickCount2) / 1000).ToString())));
                                        Helpers.Common.DelayTime(0.5);
                                        if (isStop)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (isStop)
                                {
                                    break;
                                }
                                num4++;
                                goto quit;
                            }
                            break;
                        quit:;
                        }
                    end:;
                    }
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
                    }
                    CloseFormViewChrome();
                    cControl("stop");
                }).Start();
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
                cControl("stop");
                isCountCheckAccountWhenChayTuongTac = false;
            }
        }

        private void lấyPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChooseRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tài khoản cần lấy page!", 3);
            }
            else
            {
                GetPageInAccount();
            }
        }

        private void GetPageInAccount()
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
                isStop = false;
                cControl("start");
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        {
                            listProxyShopLike = setting_general.GetValueList("txtApiShopLike");

                            if (listProxyShopLike.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox(("Key ShopLike không đủ, vui lòng mua thêm!"), 2);
                                return;
                            }

                            listShopLike = new List<ShopLike>();
                            for (int i = 0; i < listProxyShopLike.Count; i++)
                            {
                                ShopLike item = new ShopLike(listProxyShopLike[i], 0, setting_general.GetValueInt("nudLuongPerIPShopLike"), setting_general.GetValue("cbbLocationShopLikePrx"));
                                listShopLike.Add(item);
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
                //create thread
                new Thread((ThreadStart)delegate
                {
                    cControl("start");
                    int num = 0;
                    while (num < dtgvAcc.Rows.Count)
                    {
                        Application.DoEvents();
                        if (isStop)
                        {
                            break;
                        }
                        if (Convert.ToBoolean(dtgvAcc.Rows[num].Cells["cChose"].Value))
                        {
                            if (iThread < maxThread)
                            {
                                Interlocked.Increment(ref iThread);
                                int row = num++;
                                new Thread((ThreadStart)delegate
                                {
                                    SetStatusAccount(row, "Đang kiểm tra...");
                                    RunThreadGetPageInAcccount(row);
                                    Interlocked.Decrement(ref iThread);
                                    SetCellAccount(row, "cChose", false);
                                    LoadPageFromFile();
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
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 30000)
                    {
                        Application.DoEvents();
                        Thread.Sleep(300);
                    }
                    cControl("stop");
                }).Start();

            }
        }
        private void RunThreadGetPageInAcccount(int row)
        {
            try
            {
                string cId = GetCellAccount(row, "cId");
                string cUid = GetCellAccount(row, "cUid");
                string cPassword = GetCellAccount(row, "cPassword");
                string c2Fa = GetCellAccount(row, "cFa2");
                string cToken = GetCellAccount(row, "cToken");
                string cCookie = GetCellAccount(row, "cCookies");
                string userAgent = GetCellAccount(row, "cUseragent");
                int limitQty = 25;
                string uidAdmin = cUid;
                string proxyGet = "";
                string text2 = "";
                string text3 = "";
                if (userAgent == "")
                {
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36";
                }
                string proxy = "";
                int typeProxy = 0;
                ShopLike shopLike = null;
                MinProxy minProxy = null;
                TinsoftProxy tinsoftProxy = null;

                if (string.IsNullOrEmpty(cToken))
                {
                    SetStatusAccount(row, "Không có token!");
                    return;
                }

                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        //get proxy shoplike
                        case 1:
                            SetStatusAccount(row, "Đang lấy Proxy ShopLike ...");
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
                                        proxyGet = shopLike.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                            SetStatusAccount(row, "Đang lấy proxy Tinsoft...");
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
                                        proxyGet = tinsoftProxy.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag12 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + ("Đã dừng!"));
                                    break;
                                }
                                bool flag13 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag13 = false;
                                    }
                                }
                                if (!flag13)
                                {
                                    tinsoftProxy.dangSuDung--;
                                    tinsoftProxy.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        case 3:
                            SetStatusAccount(row, "Đang lấy Proxy MinProxy ...");
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
                                        proxyGet = minProxy.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                                    goto stopBreak;
                                }

                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }

                                if (isStop)
                                {
                                    goto stopBreak;
                                }

                                //delay run
                                lock (lock_checkDelayRequest)
                                {
                                    if (checkDelayRequest > 0)
                                    {
                                        //default delay 1-2 second
                                        int numRand = rd.Next(1, 2 + 1);
                                        if (numRand > 0)
                                        {
                                            int tickCount = Environment.TickCount;
                                            while ((Environment.TickCount - tickCount) / 1000 - numRand < 0)
                                            {
                                                SetStatusAccount(row, text2 + "Chạy tiến trình sau" + " {time}s...".Replace("{time}", (numRand - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                Helpers.Common.DelayTime(0.5);
                                                if (isStop)
                                                {
                                                    goto stopBreak;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        checkDelayRequest++;
                                    }
                                }
                                SetStatusAccount(row, text2 + "Đang check...");
                                if (isStop)
                                {
                                    goto stopBreak;
                                }
                                while (true)
                                {
                                    if (!setting_general.GetValueBool("ckbKhongCheckIP") && proxyGet.Split(':').Length > 1)
                                    {
                                        string checkProxy = Helpers.Common.CheckProxy(proxyGet, 0);
                                        if (checkProxy == "")
                                        {
                                            SetStatusAccount(row, text2 + "Lỗi kết nối proxy!");
                                            break;
                                        }
                                    }
                                    proxy = proxyGet;

                                    //proccess
                                    bool checkLiveToken = CommonRequest.CheckLiveToken(cCookie, cToken, userAgent, proxy, typeProxy);
                                    if (!checkLiveToken)
                                    {
                                        SetStatusAccount(row, "Token Die!");
                                        CommonSQL.UpdateFieldToAccount(cId, "token", "");
                                        return;
                                    }
                                    //xoá all page trước đó !
                                    try
                                    {
                                        bool delAllPage = CommonSQL.DeleteAllPageById(cUid);
                                        if(delAllPage)
                                        {
                                            UpdateSttPageOnDatatable();
                                            CountTotalPage();
                                            CountCheckedPage(0);
                                            SetStatusAccount(row, "Xoá tất cả page trước đó thành công!");
                                            LoadPageFromFile();
                                            Thread.Sleep(1000);
                                        }
                                    }
                                    catch { }
                                    //
                                    string url = $"https://graph.facebook.com/me/accounts?fields=id,name,access_token,additional_profile_id&access_token={cToken}"; // &limit={limitQty}
                                    bool continueFetching = true;
                                    int pageSucess = 0;
                                    while (continueFetching)
                                    {
                                        var rs = CommonRequest.GetPage(url, proxy, userAgent);
                                        List<string> lstQuery = new List<string>();

                                        if (rs.Item1.Count > 0)
                                        {
                                            string data = rs.Item1[0].ToString();
                                            JArray jsonArray = JArray.Parse(data);

                                            foreach (var jsonString in jsonArray)
                                            {
                                                JObject jsonObject = (JObject)jsonString;

                                                string id = jsonObject["id"].ToString();
                                                string name = jsonObject["name"].ToString();
                                                string accessToken = jsonObject["access_token"].ToString();
                                                string additionalProfileId = jsonObject["additional_profile_id"]?.ToString() ?? id;

                                                lstQuery.Add(CommonSQL.ConvertToSqlInsertPage(additionalProfileId, name, "0", "0", uidAdmin, "", "-1", "0", "", "", "", "", id, "", "Public", "none", accessToken, "0"));
                                                pageSucess++;
                                            }
                                        }

                                        bool checkQr = lstQuery.Count > 0;

                                        if (checkQr)
                                        {
                                            lstQuery = CommonSQL.ConvertToSqlInsertPage(lstQuery);

                                            for (int j = 0; j < lstQuery.Count; j++)
                                            {
                                                Connector.Instance.ExecuteNonQuery(lstQuery[j]);
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(rs.Item2?.ToString()))
                                        {
                                            url = rs.Item2.ToString();
                                        }
                                        else
                                        {
                                            continueFetching = false;
                                        }
                                        SetStatusAccount(row, "Nghỉ 5s.Lấy thành công: " + pageSucess.ToString() + " pages");
                                        Thread.Sleep(5000);
                                    }
                                    CommonSQL.UpdateMultiFieldToAccount(cId, "pages", pageSucess.ToString());
                                    SetCellAccount(row, "cPages", pageSucess.ToString());
                                    SetStatusAccount(row, "Hoàn tất! Lấy thành công: " + pageSucess.ToString() + " pages");
                                    LoadPageFromFile();
                                    break;
                                }
                                return;
                            }
                        stopBreak:
                            SetStatusAccount(row, text2 + "Đã dừng!");
                            break;
                    }
                    break;
                }
                return;
            }
            catch (Exception ex)
            {
                SetStatusAccount(row, "Lỗi!");
                Helpers.Common.ExportError(null, ex);

            }
        }
        private void tảiLạiDanhSáchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadPageFromFile();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fullString = new JSON_Settings("configDatagridview").GetFullString();
            Helpers.Common.ShowForm(new fCaiDatHienThi());
            if (fullString != new JSON_Settings("configDatagridview").GetFullString())
            {
                LoadConfigManHinh();
            }
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            BtnLoadAcc_Click(null, null);
        }

        private void gunaButton6_Click(object sender, EventArgs e)
        {
            LoadPageFromFile();
        }

        private void lọcTrùngPageUidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbCheckP.Text) == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn tất cả page rồi hãy ấn lọc.!", 3);
                return;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            int num = 0;
            for (int i = 0; i < dtgvPage.RowCount; i++)
            {
                if (!Convert.ToBoolean(dtgvPage.Rows[i].Cells["cChoosePage"].Value))
                {
                    continue;
                }
                try
                {
                    string cIdPage = GetCellPaget(i, "cIdPage");
                    string cUidAd = GetCellPaget(i, "cUidAd");
                    if (list2.Contains(cIdPage) && list3.Contains(cUidAd))
                    {
                        list.Add(dtgvPage.Rows[i].Cells["cIdP"].Value.ToString());
                        dtgvPage.Rows.RemoveAt(i);
                        i--;
                        num++;
                    }
                    else
                    {
                        list2.Add(cIdPage);
                        list3.Add(cUidAd);
                    }
                }
                catch
                {
                }
            }

            if (num > 0)
            {
                if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có chắc muốn xóa {0} page bị trùng?", num)) != DialogResult.Yes)
                {
                    return;
                }
                CommonSQL.DeletePageToDatabase(list);
                UpdateSttPageOnDatatable();
                CountTotalPage();
                CountCheckedPage();
                MessageBoxHelper.ShowMessageBox(string.Format("Đã loại bỏ {0} page bị trùng!", num));
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Không có page nào trùng lặp!");
            }

        }
        private void UpdateSttPageOnDatatable()
        {
            for (int i = 0; i < dtgvPage.RowCount; i++)
            {
                SetCellPage(i, "cSttP", i + 1);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagridPage("All");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagridPage("SelectHighline");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagridPage("UnAll");
        }

        private void ctmsPage_Opening(object sender, CancelEventArgs e)
        {
            trangthaitooltripmenu.DropDownItems.Clear();
            List<string> list = new List<string>();
            string cTrangthaiP = "";
            string text3 = "";
            for (int j = 0; j < dtgvPage.RowCount; j++)
            {
                cTrangthaiP = GetCellPaget(j, "cTrangthaiP");
                if (cTrangthaiP != "")
                {
                    text3 = Regex.Match(cTrangthaiP, "\\(IP: (.*?)\\)").Value;
                    if (text3 != "")
                    {
                        cTrangthaiP = cTrangthaiP.Replace(text3, "").Trim();
                    }
                    text3 = Regex.Match(cTrangthaiP, "\\[(.*?)\\]").Value;
                    if (text3 != "")
                    {
                        cTrangthaiP = cTrangthaiP.Replace(text3, "").Trim();
                    }
                    if (cTrangthaiP != "" && !list.Contains(cTrangthaiP))
                    {
                        list.Add(cTrangthaiP);
                    }
                }
            }

            tinhtrangpagetoolstrip.DropDownItems.Clear();
            list = new List<string>();
            string cStatusP = "";
            for (int l = 0; l < dtgvPage.RowCount; l++)
            {
                cStatusP = GetCellPaget(l, "cStatusP");
                if (!cStatusP.Equals("") && !list.Contains(cStatusP))
                {
                    list.Add(cStatusP);
                }
            }
            for (int m = 0; m < list.Count; m++)
            {
                tinhtrangpagetoolstrip.DropDownItems.Add(list[m]);
                tinhtrangpagetoolstrip.DropDownItems[m].Click += SelectGridByStatusP;
            }
        }

        private void ChoosePageByValue(string column, string value)
        {
            for (int i = 0; i < dtgvPage.RowCount; i++)
            {
                dtgvPage.Rows[i].Cells["cChoosePage"].Value = GetCellPaget(i, column).Contains(value);
            }
            CountCheckedPage();
        }

        private void SelectGridByStatusP(object sender, EventArgs e)
        {
            ChoosePageByValue("cStatusP", (sender as ToolStripMenuItem).Text);
        }

        private void gunaButton4_Click_1(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fCaidatTuongTacPage());
        }

        private void xoáPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeletePage();
        }
        private void DeletePage()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dtgvPage.RowCount; i++)
            {
                if (Convert.ToBoolean(dtgvPage.Rows[i].Cells["cChoosePage"].Value))
                {
                    list.Add(dtgvPage.Rows[i].Cells["cIdP"].Value.ToString());
                }
            }
            if (list.Count == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn page cần xóa!");
            }
            else
            {
                if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format("Bạn có muốn xóa {0} page đã chọn?", CountChoosePageRowInDatagridview())) != DialogResult.Yes)
                {
                    return;
                }
                if (CommonSQL.DeletePageToDatabase(list))
                {
                    for (int j = 0; j < dtgvPage.RowCount; j++)
                    {
                        if (Convert.ToBoolean(dtgvPage.Rows[j].Cells["cChoosePage"].Value))
                        {
                            dtgvPage.Rows.RemoveAt(j--);
                        }
                    }
                    UpdateSttPageOnDatatable();
                    CountTotalPage();
                    CountCheckedPage(0);
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("Xóa thất bại, vui lòng thử lại sau!", 2);
                }
            }
        }
        public int CountChoosePageRowInDatagridview()
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(lbCheckP.Text);
            }
            catch
            {
            }
            return result;
        }
        public void SetStatusPage(int indexRow, string value, int timeWait = -1)
        {
            if (timeWait != -1)
            {
                if (timeWait != 0)
                {
                    DatagridviewHelper.SetStatusDataGridViewWithWait(this.dtgvPage, indexRow, "cTrangthaiP", timeWait, value);
                }
            }
            else
            {
                DatagridviewHelper.SetStatusDataGridView(this.dtgvPage, indexRow, "cTrangthaiP", value);
            }
        }
        public void SetInfoPage(string id, int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvPage, indexRow, "cInfoP", value);
            SetRowColor(indexRow);
            CommonSQL.UpdateFieldToPage(id, "info", value);
        }

        private void btnRunPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (CountChoosePageRowInDatagridview() == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn page muốn chạy!", 3);
                    return;
                }

                LoadSetting();
                int maxThread = setting_general.GetValueInt("nudHideThread", 10);
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
                isCountCheckAccountWhenChayTuongTac = true;
                isStop = false;
                int curChangeIp = 0;
                bool isChangeIPSuccess = false;
                checkDelayChrome = 0;
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    //running
                    try
                    {
                        List<string> list = new List<string>();
                        string idKichBan = "";
                        string text = "";
                        int num3 = setting_InteractGeneralPage.GetValueInt("nudSoLuotChay", 1);
                        if (num3 == 0)
                        {
                            num3 = 1;
                        }
                        int num4 = 0;
                        while (num4 < num3)
                        {
                            //processing
                            list = new List<string>();
                            if (setting_InteractGeneralPage.GetValueBool("ckbRepeatAll"))
                            {
                                text = (num3 > 1) ? string.Format("Lượt {0}/{1}. ", num4 + 1, num3) : "";
                                //random kich ban
                                list.Add(setting_InteractGeneralPage.GetValue("cbbKichBanP"));
                            }
                            else
                            {
                                list.Add(setting_InteractGeneralPage.GetValue("cbbKichBanP"));
                            }
                            int num5 = 0;

                            while (true)
                            {
                                if (num5 < list.Count && !isStop)
                                {
                                    idKichBan = list[num5];
                                    if (text != "")
                                    {
                                        ShowTrangThai(text + string.Format("Kịch bản Page" + ": {0}...", Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetTenKichBanById(idKichBan)));
                                    }
                                    //random page nếu làm

                                    //ds kịch bản
                                    dicSpamCommentPage_NoiDung = GetDictionaryIntoHanhDongPage(idKichBan, "SpamCmtPage", "txtNoiDung");
                                    dicSpamCommentPage_NoiDungGoc = GetDictionaryIntoHanhDongPage(idKichBan, "SpamCmtPage", "txtNoiDung");
                                    dicSpamCommentPage_listUid = GetDictionaryIntoHanhDongPage(idKichBan, "SpamCmtPage");

                                    int num6 = 0;
                                    while (num6 < dtgvPage.Rows.Count && !isStop)
                                    {
                                        if (!Convert.ToBoolean(dtgvPage.Rows[num6].Cells["cChoosePage"].Value))
                                        {
                                            num6++;
                                            goto checkStop;
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
                                            int row = num6++;
                                            Thread thread = new Thread((ThreadStart)delegate
                                            {
                                                try
                                                {
                                                    int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                                    ExcuteOneThreadPage(row, indexOfPossitionApp, idKichBan);
                                                    Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                                    if (!setting_InteractGeneralPage.GetValueBool("ckbRepeatAll"))
                                                    {
                                                        SetCellPage(row, "cChoosePage", false);
                                                    }
                                                }
                                                catch (Exception ex3)
                                                {
                                                    Helpers.Common.ExportError(null, ex3);
                                                }
                                            })
                                            {
                                                Name = row.ToString()
                                            };
                                            lstThread.Add(thread);
                                            Helpers.Common.DelayTime(1.0);
                                            thread.Start();
                                            goto checkStop;
                                        }
                                        if (isStop)
                                        {
                                            break;
                                        }
                                        if (setting_general.GetValueInt("ip_iTypeChangeIp") == 0 || setting_general.GetValueInt("ip_iTypeChangeIp") == 1 || setting_general.GetValueInt("ip_iTypeChangeIp") == 2 || setting_general.GetValueInt("ip_iTypeChangeIp") == 3)
                                        {
                                            for (int num8 = 0; num8 < lstThread.Count; num8++)
                                            {
                                                if (!lstThread[num8].IsAlive)
                                                {
                                                    lstThread.RemoveAt(num8--);
                                                }
                                            }
                                            goto checkStop;
                                        }
                                        for (int num9 = 0; num9 < lstThread.Count; num9++)
                                        {
                                            lstThread[num9].Join();
                                            lstThread.RemoveAt(num9--);
                                        }
                                        if (isStop)
                                        {
                                            break;
                                        }
                                        Interlocked.Increment(ref curChangeIp);
                                        if (curChangeIp < setting_general.GetValueInt("ip_nudChangeIpCount", 1))
                                        {
                                            goto checkStop;
                                        }
                                        MessageBoxHelper.ShowMessageBox("Lỗi change ip!", 2);
                                        goto end;
                                    checkStop:
                                        if (isStop)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 0; i < lstThread.Count; i++)
                                    {
                                        lstThread[i].Join();
                                    }

                                    SaveDictionaryIntoHanhDongPage(dicSpamCommentPage_listUid);

                                    if (!isStop)
                                    {
                                        if (num5 + 1 < list.Count)
                                        {
                                            int num12 = Base.rd.Next(setting_InteractGeneralPage.GetValueInt("nudDelayTurnFrom"), setting_InteractGeneralPage.GetValueInt("nudDelayTurnTo") + 1);
                                            int tickCount = Environment.TickCount;
                                            while ((Environment.TickCount - tickCount) / 1000 - num12 < 0)
                                            {
                                                ShowTrangThai(text + string.Format("Chạy kịch bản Page tiếp theo sau {time} giây...".Replace("{time}", (num12 - (Environment.TickCount - tickCount) / 1000).ToString())));
                                                Helpers.Common.DelayTime(0.5);
                                                if (isStop)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        num5++;
                                        continue;
                                    }
                                }
                                if (!setting_InteractGeneralPage.GetValueBool("ckbRepeatAll") || isStop)
                                {
                                    break;
                                }
                                if (num4 + 1 < num3)
                                {
                                    int num13 = Base.rd.Next(setting_InteractGeneralPage.GetValueInt("nudDelayTurnFrom"), setting_InteractGeneralPage.GetValueInt("nudDelayTurnTo") + 1) * 60;
                                    int tickCount2 = Environment.TickCount;
                                    while ((Environment.TickCount - tickCount2) / 1000 - num13 < 0)
                                    {
                                        ShowTrangThai(text + string.Format("Chạy lượt tiếp theo sau {time} giây...".Replace("{time}", (num13 - (Environment.TickCount - tickCount2) / 1000).ToString())));
                                        Helpers.Common.DelayTime(0.5);
                                        if (isStop)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (isStop)
                                {
                                    break;
                                }
                                num4++;
                                goto quit;
                            }
                            break;
                        quit:;
                        }
                    end:;
                    }
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
                    }
                    cControl("stop");
                }).Start();
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
                cControl("stop");
                isCountCheckAccountWhenChayTuongTac = false;
            }
        }
        
        private void ExcuteOneThreadPage(int indexRow, int indexPos, string idKichBan)
        {
            int num = 0;
            string text = "";
            int num2 = 0;
            string text2 = "";
            int typeProxy = 0;
            string text3 = "";
            ShopLike shopLike = null;
            MinProxy minProxy = null;
            TinsoftProxy tinsoftProxy = null;
            bool flag = false;
            string text4 = "";
            int checkPostSuccess = 0;
            int num3 = 0;
            bool flag2 = false;
            bool flag22 = false;
            bool flag222 = false;
            //page
            string cTokenP = GetCellPaget(indexRow, "cTokenP");
            string cId = GetCellPaget(indexRow, "cIdP");

            if (isStop)
            {
                SetStatusPage(indexRow, text2 + "Đã dừng!");
                num3 = 1;
            }
            else
            {
                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        case 1:
                            SetStatusPage(indexRow, "Đang lấy Proxy ShopLike ...");
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
                                        text = shopLike.proxy;
                                        if (text == "")
                                        {
                                            text = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag5 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                bool flag6 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusPage(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusPage(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag6 = false;
                                    }
                                }
                                if (!flag6)
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
                            SetStatusPage(indexRow, "Đang lấy proxy Tinsoft...");
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
                                        text = tinsoftProxy.proxy;
                                        if (text == "")
                                        {
                                            text = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag11 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag12 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusPage(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusPage(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
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
                            SetStatusPage(indexRow, "Đang lấy Proxy MinProxy ...");
                            lock (lock_StartProxy)
                            {
                                while (!isStop)
                                {
                                    minProxy = null;
                                    while (!isStop)
                                    {
                                        foreach (MinProxy minp in listMinproxy)
                                        {
                                            if (minProxy == null || minp.daSuDung < minProxy.daSuDung)
                                            {
                                                minProxy = minp;
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
                                        text = minProxy.proxy;
                                        if (text == "")
                                        {
                                            text = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag5 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                bool flag6 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusPage(indexRow, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text.Split(':')[0] + ") ";
                                    SetStatusPage(indexRow, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(text, 0);
                                    if (text3 == "")
                                    {
                                        flag6 = false;
                                    }
                                }
                                if (!flag6)
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
                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }
                                if (isStop)
                                {
                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
                                    num3 = 1;
                                    break;
                                }
                                num = Environment.TickCount;
                                try
                                {
                                    SetStatusPage(indexRow, text2 + "Chờ đến lượt...");
                                    lock (lock_checkDelayChrome)
                                    {
                                        if (checkDelayChrome > 0)
                                        {
                                            int num7 = rd.Next(3, 5);
                                            if (num7 > 0)
                                            {
                                                int tickCount = Environment.TickCount;
                                                while ((Environment.TickCount - tickCount) / 1000 - num7 < 0)
                                                {
                                                    SetStatusPage(indexRow, text2 + "Chạy sau" + " {time}s...".Replace("{time}", (num7 - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                    Helpers.Common.DelayTime(0.5);
                                                    if (isStop)
                                                    {
                                                        SetStatusPage(indexRow, text2 + "Đã dừng!");
                                                        num3 = 1;
                                                        goto endThread;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            checkDelayChrome++;
                                        }
                                    }
                                    SetStatusPage(indexRow, text2 + "Đang chuẩn bị chạy...");

                                    if (isStop)
                                    {
                                        SetStatusPage(indexRow, text2 + "Đã dừng!");
                                        num3 = 1;
                                        break;
                                    }

                                    int num8 = 0;
                                    while (true)
                                    {
                                        SetStatusPage(indexRow, text2 + "Chạy tương tác...");
                                        if (isStop)
                                        {
                                            SetStatusPage(indexRow, text2 + "Đã dừng!");
                                            num3 = 1;
                                            break;
                                        }
                                        DataTable dataTable = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetAllHanhDongByKichBan(idKichBan);
                                        string text15 = "";
                                        string text16 = "";
                                        DataTable dataTable2 = new DataTable();
                                        string cauHinhFromKichBan = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetCauHinhFromKichBan(idKichBan);
                                        JSON_Settings jSON_Settings = new JSON_Settings(cauHinhFromKichBan, isJsonString: true);
                                        int valueInt2 = jSON_Settings.GetValueInt("typeSoLuongHanhDong");
                                        int valueInt3 = jSON_Settings.GetValueInt("nudHanhDongFrom");
                                        int valueInt4 = jSON_Settings.GetValueInt("nudHanhDongTo");
                                        int num11 = dataTable.Rows.Count;
                                        if (valueInt2 == 1 && valueInt3 <= valueInt4)
                                        {
                                            num11 = Base.rd.Next(valueInt3, valueInt4 + 1);
                                            if (num11 > dataTable.Rows.Count)
                                            {
                                                num11 = dataTable.Rows.Count;
                                            }
                                        }
                                        int num12 = 0;
                                        while (num12 < num11)
                                        {
                                            if (isStop)
                                            {
                                                SetStatusPage(indexRow, text2 + "Đã dừng!");
                                                num3 = 1;
                                                goto endThread;
                                            }
                                            try
                                            {
                                                text16 = dataTable.Rows[num12]["TenHanhDong"].ToString();
                                                text15 = dataTable.Rows[num12]["Id_HanhDong"].ToString();
                                                SetStatusPage(indexRow, text2 + "Đang" + " " + text16 + "...");
                                                dataTable2 = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetHanhDongById(text15);
                                                JSON_Settings jSON_Settings2 = new JSON_Settings(dataTable2.Rows[0]["CauHinh"].ToString(), isJsonString: true);
                                                //danh sách chạy kịch bản
                                                switch (dataTable2.Rows[0]["TenTuongTac"].ToString())
                                                {
                                                    case "SpamCmtPage":
                                                        try
                                                        {
                                                            num2 = SpamCmtPage(indexRow, text, jSON_Settings2.GetValueInt("nudCountUid", 5), jSON_Settings2.GetValueInt("nudPostUid", 3), jSON_Settings2.GetValueInt("nudKhoangCachFrom", 1), jSON_Settings2.GetValueInt("nudKhoangCachTo", 1), jSON_Settings2.GetValueBool("ckbAnh"), jSON_Settings2.GetValue("txtPathAnh"), jSON_Settings2.GetValue("txtPathFileUid"), jSON_Settings2.GetValueInt("typeListUid"), jSON_Settings2.GetValueBool("ckbRemoveUid"), rd, text16, text15);
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            Helpers.Common.ExportError(e, "SpamCmtPage");
                                                        }
                                                        break;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                Helpers.Common.ExportError(e, "Tuong tac theo kich ban page");
                                            }
                                            if (num2 == -4)
                                            {
                                                flag2 = true;
                                                break;
                                            }
                                            if (num2 == -1)
                                            {
                                                flag22 = true;
                                                // goto endThread;
                                                break;
                                            }
                                            if(num2 == 5)
                                            {
                                                flag222 = true;
                                                break;
                                            }    
                                            num12++;
                                        }
                                        break;
                                    }
                                quit:;
                                }
                                catch (Exception e)
                                {
                                    SetStatusPage(indexRow, text2 + "Lỗi không xác định!");
                                    num3 = 1;
                                }
                                break;
                            }
                        endThread:
                            break;
                    }
                    break;
                }
            }

            string text18 = "";
            if (num3 == 1)
            {
                text18 = GetStatusPage(indexRow);
            }
            try
            {
                int num13 = rd.Next(5, 10);
                if (num13 > 0)
                {
                    int tickCount2 = Environment.TickCount;
                    while ((Environment.TickCount - tickCount2) / 1000 - num13 < 0)
                    {
                        if (isStop)
                        {
                            SetStatusPage(indexRow, text2 + "Đã dừng!");
                            break;
                        }
                        SetStatusPage(indexRow, text2 + "Nghỉ luồng sau {time}s...".Replace("{time}", (num13 - (Environment.TickCount - tickCount2) / 1000).ToString()));
                        Helpers.Common.DelayTime(0.5);
                    }
                }
            }
            catch
            {
            }
            string text19 = text18;
            string text20 = text19;
            if (text20 == "")
            {
                SetStatusPage(indexRow, text2 + "Đã chạy xong!" + (flag2 ? "- Facebook blocked!" : "") + (flag22 ? "- Token die!" : "") + " [" + Helpers.Common.ConvertSecondsToTime((Environment.TickCount - num) / 1000) + "(s)]");
                SetCellPage(indexRow, "cLastInteractP", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                CommonSQL.UpdateFieldToPage(cId, "lastInteract", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SetInfoPage(cId, indexRow, "Public");
            }
            else
            {
                SetStatusPage(indexRow, text2 + text18 + (flag2 ? "- Facebook blocked!" : "") + (flag22 ? "- Token die!" : ""));
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

        //private void ExcuteOneThreadPage(int indexRow, int indexPos, string idKichBan)
        //{
        //    int num = 0;
        //    string text = "";
        //    int num2 = 0;
        //    string text2 = "";
        //    int typeProxy = 0;
        //    string text3 = "";
        //    ShopLike shopLike = null;
        //    MinProxy minProxy = null;
        //    TinsoftProxy tinsoftProxy = null;
        //    bool flag = false;
        //    string text4 = "";
        //    int checkPostSuccess = 0;
        //    int num3 = 0;
        //    bool flag2 = false;
        //    bool flag22 = false;
        //    //page
        //    string cTokenP = GetCellPaget(indexRow, "cTokenP");
        //    string cId = GetCellPaget(indexRow, "cIdP");

        //    if (isStop)
        //    {
        //        SetStatusPage(indexRow, text2 + "Đã dừng!");
        //        num3 = 1;
        //    }
        //    else
        //    {
        //        while (true)
        //        {
        //            switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
        //            {
        //                case 1:
        //                    SetStatusPage(indexRow, "Đang lấy Proxy ShopLike ...");
        //                    lock (lock_StartProxy)
        //                    {
        //                        while (!isStop)
        //                        {
        //                            shopLike = null;
        //                            while (!isStop)
        //                            {
        //                                foreach (ShopLike item5 in listShopLike)
        //                                {
        //                                    if (shopLike == null || item5.daSuDung < shopLike.daSuDung)
        //                                    {
        //                                        shopLike = item5;
        //                                    }
        //                                }
        //                                if (shopLike.daSuDung != shopLike.limit_theads_use)
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                            if (isStop)
        //                            {
        //                                break;
        //                            }
        //                            if (shopLike.daSuDung > 0 || shopLike.ChangeProxy())
        //                            {
        //                                text = shopLike.proxy;
        //                                if (text == "")
        //                                {
        //                                    text = shopLike.GetProxy();
        //                                }
        //                                ShopLike shopLike2 = shopLike;
        //                                shopLike2.dangSuDung++;
        //                                shopLike2 = shopLike;
        //                                shopLike2.daSuDung++;
        //                                break;
        //                            }
        //                            bool flag5 = true;
        //                        }
        //                        if (isStop)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                            num3 = 1;
        //                            break;
        //                        }
        //                        bool flag6 = true;
        //                        if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Delay check IP...");
        //                            Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
        //                        }
        //                        if (!setting_general.GetValueBool("ckbKhongCheckIP"))
        //                        {
        //                            text2 = "(IP: " + text.Split(':')[0] + ") ";
        //                            SetStatusPage(indexRow, text2 + "Check IP...");
        //                            text3 = Helpers.Common.CheckProxy(text, 0);
        //                            if (text3 == "")
        //                            {
        //                                flag6 = false;
        //                            }
        //                        }
        //                        if (!flag6)
        //                        {
        //                            ShopLike shopLike2 = shopLike;
        //                            shopLike2.dangSuDung--;
        //                            shopLike2 = shopLike;
        //                            shopLike2.daSuDung--;
        //                            continue;
        //                        }
        //                        goto default;
        //                    }
        //                case 2:
        //                    SetStatusPage(indexRow, "Đang lấy proxy Tinsoft...");
        //                    lock (lock_StartProxy)
        //                    {
        //                        while (!isStop)
        //                        {
        //                            tinsoftProxy = null;
        //                            while (!isStop)
        //                            {
        //                                foreach (TinsoftProxy item in listTinsoft)
        //                                {
        //                                    if (tinsoftProxy == null || item.daSuDung < tinsoftProxy.daSuDung)
        //                                    {
        //                                        tinsoftProxy = item;
        //                                    }
        //                                }
        //                                if (tinsoftProxy.daSuDung != tinsoftProxy.limit_theads_use)
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                            if (isStop)
        //                            {
        //                                break;
        //                            }
        //                            if (tinsoftProxy.daSuDung > 0 || tinsoftProxy.ChangeProxy())
        //                            {
        //                                text = tinsoftProxy.proxy;
        //                                if (text == "")
        //                                {
        //                                    text = tinsoftProxy.GetProxy();
        //                                }
        //                                tinsoftProxy.dangSuDung++;
        //                                tinsoftProxy.daSuDung++;
        //                                break;
        //                            }
        //                            bool flag11 = true;
        //                        }
        //                        if (isStop)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                            break;
        //                        }
        //                        bool flag12 = true;
        //                        if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Delay check IP...");
        //                            Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
        //                        }
        //                        if (!setting_general.GetValueBool("ckbKhongCheckIP"))
        //                        {
        //                            text2 = "(IP: " + text.Split(':')[0] + ") ";
        //                            SetStatusPage(indexRow, text2 + "Check IP...");
        //                            text3 = Helpers.Common.CheckProxy(text, 0);
        //                            if (text3 == "")
        //                            {
        //                                flag12 = false;
        //                            }
        //                        }
        //                        if (!flag12)
        //                        {
        //                            tinsoftProxy.dangSuDung--;
        //                            tinsoftProxy.daSuDung--;
        //                            continue;
        //                        }
        //                        goto default;
        //                    }
        //                case 3:
        //                    SetStatusPage(indexRow, "Đang lấy Proxy MinProxy ...");
        //                    lock (lock_StartProxy)
        //                    {
        //                        while (!isStop)
        //                        {
        //                            minProxy = null;
        //                            while (!isStop)
        //                            {
        //                                foreach (MinProxy minp in listMinproxy)
        //                                {
        //                                    if (minProxy == null || minp.daSuDung < minProxy.daSuDung)
        //                                    {
        //                                        minProxy = minp;
        //                                    }
        //                                }
        //                                if (minProxy.daSuDung != minProxy.limit_theads_use)
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                            if (isStop)
        //                            {
        //                                break;
        //                            }
        //                            if (minProxy.daSuDung > 0 || minProxy.ChangeProxy())
        //                            {
        //                                text = minProxy.proxy;
        //                                if (text == "")
        //                                {
        //                                    text = minProxy.GetProxy();
        //                                }
        //                                MinProxy minProxy2 = minProxy;
        //                                minProxy2.dangSuDung++;
        //                                minProxy2 = minProxy;
        //                                minProxy2.daSuDung++;
        //                                break;
        //                            }
        //                            bool flag5 = true;
        //                        }
        //                        if (isStop)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                            num3 = 1;
        //                            break;
        //                        }
        //                        bool flag6 = true;
        //                        if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Delay check IP...");
        //                            Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
        //                        }
        //                        if (!setting_general.GetValueBool("ckbKhongCheckIP"))
        //                        {
        //                            text2 = "(IP: " + text.Split(':')[0] + ") ";
        //                            SetStatusPage(indexRow, text2 + "Check IP...");
        //                            text3 = Helpers.Common.CheckProxy(text, 0);
        //                            if (text3 == "")
        //                            {
        //                                flag6 = false;
        //                            }
        //                        }
        //                        if (!flag6)
        //                        {
        //                            MinProxy minProxy2 = minProxy;
        //                            minProxy2.dangSuDung--;
        //                            minProxy2 = minProxy;
        //                            minProxy2.daSuDung--;
        //                            continue;
        //                        }
        //                        goto default;
        //                    }
        //                default:
        //                    {
        //                        if (isStop)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                            num3 = 1;
        //                            break;
        //                        }
        //                        if (!setting_general.GetValueBool("ckbKhongCheckIP"))
        //                        {
        //                            text2 = "(IP: " + text3 + ") ";
        //                        }
        //                        if (isStop)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                            num3 = 1;
        //                            break;
        //                        }
        //                        num = Environment.TickCount;
        //                        try
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Chờ đến lượt...");
        //                            lock (lock_checkDelayChrome)
        //                            {
        //                                if (checkDelayChrome > 0)
        //                                {
        //                                    int num7 = rd.Next(setting_general.GetValueInt("nudDelayOpenChromeFrom", 1), setting_general.GetValueInt("nudDelayOpenChromeTo", 1) + 1);
        //                                    if (num7 > 0)
        //                                    {
        //                                        int tickCount = Environment.TickCount;
        //                                        while ((Environment.TickCount - tickCount) / 1000 - num7 < 0)
        //                                        {
        //                                            SetStatusPage(indexRow, text2 + "Chạy sau" + " {time}s...".Replace("{time}", (num7 - (Environment.TickCount - tickCount) / 1000).ToString()));
        //                                            Helpers.Common.DelayTime(0.5);
        //                                            if (isStop)
        //                                            {
        //                                                SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                                                num3 = 1;
        //                                                goto endThread;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    checkDelayChrome++;
        //                                }
        //                            }
        //                            SetStatusPage(indexRow, text2 + "Đang chuẩn bị chạy...");

        //                            if (isStop)
        //                            {
        //                                SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                                num3 = 1;
        //                                break;
        //                            }

        //                            int num8 = 0;
        //                            while (true)
        //                            {
        //                                SetStatusPage(indexRow, text2 + "Chạy tương tác...");
        //                                if (isStop)
        //                                {
        //                                    SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                                    num3 = 1;
        //                                    break;
        //                                }
        //                                DataTable dataTable = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetAllHanhDongByKichBan(idKichBan);
        //                                string text15 = "";
        //                                string text16 = "";
        //                                DataTable dataTable2 = new DataTable();
        //                                string cauHinhFromKichBan = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetCauHinhFromKichBan(idKichBan);
        //                                JSON_Settings jSON_Settings = new JSON_Settings(cauHinhFromKichBan, isJsonString: true);
        //                                int valueInt2 = jSON_Settings.GetValueInt("typeSoLuongHanhDong");
        //                                int valueInt3 = jSON_Settings.GetValueInt("nudHanhDongFrom");
        //                                int valueInt4 = jSON_Settings.GetValueInt("nudHanhDongTo");
        //                                int num11 = dataTable.Rows.Count;
        //                                if (valueInt2 == 1 && valueInt3 <= valueInt4)
        //                                {
        //                                    num11 = Base.rd.Next(valueInt3, valueInt4 + 1);
        //                                    if (num11 > dataTable.Rows.Count)
        //                                    {
        //                                        num11 = dataTable.Rows.Count;
        //                                    }
        //                                }
        //                                int num12 = 0;
        //                                while (num12 < num11)
        //                                {
        //                                    if (isStop)
        //                                    {
        //                                        SetStatusPage(indexRow, text2 + "Đã dừng!");
        //                                        num3 = 1;
        //                                        goto endThread;
        //                                    }
        //                                    try
        //                                    {
        //                                        text16 = dataTable.Rows[num12]["TenHanhDong"].ToString();
        //                                        text15 = dataTable.Rows[num12]["Id_HanhDong"].ToString();
        //                                        SetStatusPage(indexRow, text2 + "Đang" + " " + text16 + "...");
        //                                        dataTable2 = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetHanhDongById(text15);
        //                                        JSON_Settings jSON_Settings2 = new JSON_Settings(dataTable2.Rows[0]["CauHinh"].ToString(), isJsonString: true);
        //                                        //danh sách chạy kịch bản
        //                                        switch (dataTable2.Rows[0]["TenTuongTac"].ToString())
        //                                        {
        //                                            case "SpamCmtPage":
        //                                                try
        //                                                {
        //                                                    num2 = SpamCmtPage(indexRow, text, jSON_Settings2.GetValueInt("nudCountUid", 5), jSON_Settings2.GetValueInt("nudPostUid", 3), jSON_Settings2.GetValueInt("nudKhoangCachFrom", 1), jSON_Settings2.GetValueInt("nudKhoangCachTo", 1), jSON_Settings2.GetValueBool("ckbAnh"), jSON_Settings2.GetValue("txtPathAnh"), jSON_Settings2.GetValue("txtPathFileUid"), jSON_Settings2.GetValueInt("typeListUid"), jSON_Settings2.GetValueBool("ckbRemoveUid"), rd, text16, text15);
        //                                                }
        //                                                catch (Exception e)
        //                                                {
        //                                                    Helpers.Common.ExportError(e, "SpamCmtPage");
        //                                                }
        //                                                break;
        //                                        }
        //                                    }
        //                                    catch (Exception e)
        //                                    {
        //                                        Helpers.Common.ExportError(e, "Tuong tac theo kich ban page");
        //                                    }
        //                                    if (num2 == -4)
        //                                    {
        //                                        flag2 = true;
        //                                        goto endThread;
        //                                    }
        //                                    if (num2 == -1)
        //                                    {
        //                                        flag22 = true;
        //                                        goto endThread;
        //                                    }
        //                                    num12++;
        //                                }

        //                            }
        //                        quit:;
        //                        }
        //                        catch (Exception e)
        //                        {
        //                            SetStatusPage(indexRow, text2 + "Lỗi không xác định!");
        //                        }
        //                        break;
        //                    }
        //                endThread:
        //                    break;
        //            }
        //            break;
        //        }
        //    }

        //    string text20 = "";
        //    if (text20 == "")
        //    {
        //        SetStatusPage(indexRow, text2 + "Đã chạy xong!" + (flag2 ? "- Facebook blocked!" : "") + (flag22 ? "- Token die!" : "") + " [" + Helpers.Common.ConvertSecondsToTime((Environment.TickCount - num) / 1000) + "(s)]");
        //        SetCellPage(indexRow, "cLastInteractP", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        //        CommonSQL.UpdateFieldToPage(cId, "interactEnd", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        //        //SetInfoPage(cId, indexRow, "Public");
        //    }

        //    lock (lock_FinishProxy)
        //    {
        //        switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
        //        {
        //            case 1:
        //                shopLike?.DecrementDangSuDung();
        //                break;
        //            case 2:
        //                tinsoftProxy?.DecrementDangSuDung();
        //                break;
        //            case 3:
        //                minProxy?.DecrementDangSuDung();
        //                break;
        //        }
        //    }
        //}

        public int SpamCmtPage(int indexRow, string proxy, int nudCountUid, int nudPostUid, int nudKhoangCachFrom, int nudKhoangCachTo, bool ckbAnh, string txtPathAnh, string pathFileUid, int typeListUid, bool ckbRemoveUid, Random rd, string tenHanhDong, string id_HanhDong)
        {
            string cid = GetCellPaget(indexRow, "cIdP");
            string token = GetCellPaget(indexRow, "cTokenP");
            string cookie = "";
            string userAgent = Base.useragentDefault;
            string content = "";
            string pathImg = "";
            string text2 = "(IP: " + proxy.Split(':')[0] + ") ";
            int typeProxy = 0;
            int num = 0;
            string text3 = "";
            int num3 = 0;
            string tokenPage = "";
            string postId = "";
            int countUidUsing = 0;
            int spamCountSend = 0;
            int countUidErr = 0;
            try
            {
                
                while (countUidUsing < nudCountUid + 1)
                {
                    if (countUidErr > 2)
                    {
                        SetStatusPage(indexRow, text2 + "Comment thất bại quá nhiều! Dừng Luồng!");
                        num = 5;
                        goto quit;
                    }

                    if (isStop)
                    {
                        SetStatusPage(indexRow, text2 + "Đã dừng!");
                        num = 1;
                        goto quit;
                    }
                    bool flag = false;

                    //check token
                    if (token != "")
                    {
                        string checkLiveToken = CommonRequest.GraphFacebook(cookie, token, userAgent, proxy, typeProxy);
                        if (checkLiveToken == "Error")
                        {
                            SetStatusPage(indexRow, text2 + "Token die!");
                            goto tokendie;
                        }
                        else
                        {
                            SetStatusPage(indexRow, text2 + "Token Live!");
                            goto initSpam;
                        }
                    }
                    else
                    {
                        SetStatusPage(indexRow, text2 + "Không có token!");
                        goto tokendie;
                    }

                initSpam:
                    SetStatusPage(indexRow, text2 + "Đang khởi tạo dữ liệu...");
                    //get list uid
                    List<string> list = new List<string>();
                    if (typeListUid == 1)
                    {
                        list = File.ReadAllLines(pathFileUid).ToList();
                        list = Helpers.Common.RemoveEmptyItems(list);
                        if (list.Count == 0)
                        {
                            goto emptyUidSpam;
                        }
                    }
                    else if (!ckbRemoveUid)
                    {
                        list = CloneList(dicSpamCommentPage_listUid[id_HanhDong]);
                        if (list.Count == 0)
                        {
                            goto emptyUidSpam;
                        }
                    }
                    //get list img
                    List<string> lstFrom = new List<string>();
                    List<string> list2 = new List<string>();
                    if (ckbAnh)
                    {
                        lstFrom = Directory.GetFiles(txtPathAnh).ToList();
                        list2 = CloneList(lstFrom);
                        if (lstFrom.Count == 0 || list2.Count == 0)
                        {
                            goto emptyImgSpam;
                        }
                    }
                    //get 1 uid spam
                    if (ckbRemoveUid)
                    {
                        lock (lock_baivietprofile)
                        {
                            if (typeListUid == 1)
                            {
                                list = File.ReadAllLines(pathFileUid).ToList();
                                list = Helpers.Common.RemoveEmptyItems(list);
                                if (list.Count == 0)
                                {
                                    goto emptyUidSpam;
                                }
                                text3 = list[rd.Next(0, list.Count)];
                                list.Remove(text3);
                                File.WriteAllLines(pathFileUid, list);
                            }
                            else
                            {
                                lock (dicSpamCommentPage_listUid)
                                {
                                    if (dicSpamCommentPage_listUid[id_HanhDong].Count == 0)
                                    {
                                        goto emptyUidSpam;
                                    }
                                    text3 = dicSpamCommentPage_listUid[id_HanhDong][rd.Next(0, dicSpamCommentPage_listUid[id_HanhDong].Count)];
                                    dicSpamCommentPage_listUid[id_HanhDong].Remove(text3);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (list.Count == 0)
                        {
                            SetStatusPage(indexRow, text2 + "Hết Uid Spam!");
                            break;
                        }
                        text3 = list[rd.Next(0, list.Count)];
                        list.Remove(text3);
                    }
                    SetStatusPage(indexRow, text2 + $" Chuẩn bị spam uid: {text3} lượt {countUidUsing + 1}");
                    goto checkPostProfile;

                checkPostProfile:
                    if (isStop)
                    {
                        SetStatusPage(indexRow, text2 + "Đã dừng!");
                        num = 1;
                        goto quit;
                    }
                    SetStatusPage(indexRow, text2 + $"Check Uid... {countUidUsing}/{nudCountUid}");
                    tokenPage = token;
                    Thread.Sleep(1000);
                    if (!string.IsNullOrEmpty(tokenPage))
                    {
                        DelayThaoTacNho(1);
                        postId = CommonRequest.getPostNew(text3, tokenPage, cookie, proxy, typeProxy, nudPostUid);
                        //check post
                        if (postId != null && postId != "")
                        {
                            SetStatusPage(indexRow, text2 + $" Post Id: {postId}...");
                            Thread.Sleep(1000);
                            goto startSpam;
                        }
                        else
                        {
                            if (countUidUsing < nudCountUid + 1)
                            {
                                SetStatusPage(indexRow, text2 + $"{countUidUsing}/{nudCountUid} Uid Không Có Post Nào, Đang lấy uid khác");
                                goto initSpam;
                            }
                            else
                            {
                                SetStatusPage(indexRow, text2 + $"{countUidUsing}/{nudCountUid} Dừng Luồng!");
                                goto quit;
                            }

                        }
                    }

                startSpam:
                    if (isStop)
                    {
                        SetStatusPage(indexRow, text2 + "Đã dừng!");
                        num = 1;
                        goto quit;
                    }
                    SetStatusPage(indexRow, text2 + "Check ok, Chuẩn bị spam...");
                    if (ckbAnh)
                    {
                        if (list2.Count == 0)
                        {
                            list2 = CloneList(lstFrom);
                        }
                        pathImg = list2[rd.Next(0, list2.Count)];
                        list2.Remove(pathImg);
                        SetStatusPage(indexRow, text2 + "Đang chuẩn bị ảnh spam...");
                        pathImg = CommonRequest.UploadImgToServer(pathImg);
                        DelayThaoTacNho(1);
                    }
                    //get noi dung
                    SetStatusPage(indexRow, text2 + "Chuẩn bị nội dung spam...");
                    lock (dicSpamCommentPage_NoiDung)
                    {
                        if (dicSpamCommentPage_NoiDung[id_HanhDong].Count == 0)
                        {
                            break;
                        }
                        content = dicSpamCommentPage_NoiDung[id_HanhDong][rd.Next(0, dicSpamCommentPage_NoiDung[id_HanhDong].Count)];
                        goto convertContent;
                    }

                convertContent:
                    SetStatusPage(indexRow, text2 + "Chuyển đổi nội dung spam...");
                    content = Helpers.Common.SpinText(content, rd);
                    if (content.Contains("[u]"))
                    {
                        string name = Helpers.CommonRequest.getInfoByUid(text3);
                        content = content.Replace("[u]", string.Concat(new string[] { "@[", text3, ":", name, "]" }));
                    }
                    content = GetIconFacebook.ProcessString(content, rd);
                    content = HttpUtility.UrlEncode(content);
                    goto sendCommentApi;

                sendCommentApi:
                    //send api comment
                    flag = true;
                    
                    if (tokenPage != "" && tokenPage != null && postId != "" && postId != null)
                    {
                        SetStatusPage(indexRow, text2 + $" Send Spam {spamCountSend}/3: {postId}");
                        bool sendComment = CommonRequest.sendCommentByTokenCookie(postId, content, pathImg, ckbAnh, tokenPage, cookie, proxy, typeProxy);
                        if (sendComment)
                        {
                            countUidUsing++;
                            SetStatusPage(indexRow, text2 + $" Comment thành công {postId}");
                            Helpers.Common.SaveLog("success", $"Uid: {text3} - https://fb.com/{postId} - OK");
                            DelayThaoTacNho(1);
                            goto checkDone;
                        }
                        else
                        {
                            if (spamCountSend > 2)
                            {
                                SetStatusPage(indexRow, text2 + $" Comment thất bại: {postId}");
                                Helpers.Common.SaveLog("error", $"Uid: {text3} - https://fb.com/{postId} - ERROR");
                                countUidErr++;
                                DelayThaoTacNho(1);
                                goto checkDone;
                            }
                            else
                            {
                                spamCountSend++;
                                SetStatusPage(indexRow, text2 + $" ReSend {spamCountSend}/3: {postId}");
                                goto sendCommentApi;
                            }
                        }
                    }

                checkDone:
                    if (flag)
                    {
                        num++;
                        if (countUidUsing == nudCountUid)
                        {
                            SetStatusPage(indexRow, text2 + "Đang tương tác" + $" profile ({num}:{countUidUsing}/{nudCountUid})...");
                            goto quit;
                        }
                        else
                        {
                            num3 = 0;
                            if (tenHanhDong == "")
                            {
                                SetStatusPage(indexRow, text2 + "Đang tương tác" + $" profile ({num}:{nudCountUid})...");
                            }
                            else
                            {
                                SetStatusPage(indexRow, text2 + "Đang" + $" {tenHanhDong} ({num}:{nudCountUid})...");
                            }
                            Helpers.Common.DelayTime(rd.Next(nudKhoangCachFrom, nudKhoangCachTo + 1));
                        }
                    }
                    else
                    {
                        num3++;
                        if (num3 == 3)
                        {
                            goto quit;
                        }
                    }
                    continue;

                emptyUidSpam:
                    SetStatusPage(indexRow, text2 + ("Hết Uid Spam!"));
                    return 0;
                emptyImgSpam:
                    SetStatusPage(indexRow, text2 + ("Chưa có ảnh Spam!"));
                    return 0;
                tokendie:
                    num = -1;
                    break;
                quit:
                    break;
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }

        private Dictionary<string, List<string>> GetDictionaryIntoHanhDongPage(string idKichBan, string tenTuongTac, string field = "txtUid")
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            try
            {
                List<string> idHanhDongByIdKichBanAndTenTuongTac = Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetIdHanhDongByIdKichBanAndTenTuongTac(idKichBan, tenTuongTac);
                if (idHanhDongByIdKichBanAndTenTuongTac.Count > 0)
                {
                    for (int i = 0; i < idHanhDongByIdKichBanAndTenTuongTac.Count; i++)
                    {
                        string text = idHanhDongByIdKichBanAndTenTuongTac[i];
                        JSON_Settings jSON_Settings = new JSON_Settings(Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetCauHinhFromHanhDong(text), isJsonString: true);
                        List<string> list = new List<string>();
                        list = ((!(field == "txtUid")) ? jSON_Settings.GetValueList(field, jSON_Settings.GetValueInt("typeNganCach")) : jSON_Settings.GetValueList(field));
                        dictionary.Add(text, list);
                    }
                }
            }
            catch
            {
            }
            return dictionary;
        }
        private void SaveDictionaryIntoHanhDongPage(Dictionary<string, List<string>> dic, string field = "txtUid")
        {
            if (dic.Count <= 0)
            {
                return;
            }
            foreach (KeyValuePair<string, List<string>> item in dic)
            {
                string key = item.Key;
                List<string> value = item.Value;
                JSON_Settings jSON_Settings = new JSON_Settings(Facebook_Tool_Request.core.KichBanPage.InteractSQL.GetCauHinhFromHanhDong(key), isJsonString: true);
                jSON_Settings.Update(field, value);
                InteractSQL.UpdateHanhDong(key, "", jSON_Settings.GetFullString());
            }
        }

        private void gunaButton7_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                btnStopThaoTac.Enabled = false;
                gunaButton7.Enabled = false;
                ShowTrangThai("");
            }
            catch
            {
            }
        }

        private void getPageConToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountChoosePageRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn page cần lấy token!", 3);
            }
            else
            {
                //GetPageInPage();
                MessageBoxHelper.ShowMessageBox("Phần mềm chưa được cập nhật!", 3);
            }
        }
        private void GetPageInPage()
        {
            LoadSetting();
            if (CountChoosePageRowInDatagridview() == 0)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng chọn page cần thực hiện!", 3);
            }
            else
            {
                int iThread = 0;
                int maxThread = setting_general.GetValueInt("nudHideThread", 10);
                isStop = false;
                cControl("start");
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        {
                            listProxyShopLike = setting_general.GetValueList("txtApiShopLike");

                            if (listProxyShopLike.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox("Key ShopLike không đủ, vui lòng mua thêm!", 2);
                                return;
                            }

                            listShopLike = new List<ShopLike>();
                            for (int i = 0; i < listProxyShopLike.Count; i++)
                            {
                                ShopLike item = new ShopLike(listProxyShopLike[i], 0, setting_general.GetValueInt("nudLuongPerIPShopLike"), setting_general.GetValue("cbbLocationShopLikePrx"));
                                listShopLike.Add(item);
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
                //create thread
                new Thread((ThreadStart)delegate
                {
                    cControl("start");
                    int num = 0;
                    while (num < dtgvPage.Rows.Count)
                    {
                        Application.DoEvents();
                        if (isStop)
                        {
                            break;
                        }
                        if (Convert.ToBoolean(dtgvPage.Rows[num].Cells["cChoosePage"].Value))
                        {
                            if (iThread < maxThread)
                            {
                                Interlocked.Increment(ref iThread);
                                int row = num++;
                                new Thread((ThreadStart)delegate
                                {
                                    SetStatusPage(row, ("Đang kiểm tra..."));
                                    RunThreadGetPageInPage(row);
                                    Interlocked.Decrement(ref iThread);
                                    SetCellPage(row, "cChoosePage", false);
                                    LoadPageFromFile();
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
                    int tickCount = Environment.TickCount;
                    while (iThread > 0 && Environment.TickCount - tickCount <= 30000)
                    {
                        Application.DoEvents();
                        Thread.Sleep(300);
                    }
                    cControl("stop");
                }).Start();

            }
        }
        private void RunThreadGetPageInPage(int row)
        {
            try
            {
                string cId = GetCellAccount(row, "cId");
                string cUid = GetCellAccount(row, "cUid");
                string cPassword = GetCellAccount(row, "cPassword");
                string c2Fa = GetCellAccount(row, "cFa2");
                string cToken = GetCellAccount(row, "cToken");
                string cCookie = GetCellAccount(row, "cCookies");
                string userAgent = GetCellAccount(row, "cUseragent");
                int limitQty = 25;
                string uidAdmin = cUid;
                string proxyGet = "";
                string text2 = "";
                string text3 = "";
                if (userAgent == "")
                {
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36";
                }
                string proxy = "";
                int typeProxy = 0;
                ShopLike shopLike = null;
                MinProxy minProxy = null;
                TinsoftProxy tinsoftProxy = null;

                if (string.IsNullOrEmpty(cToken))
                {
                    SetStatusAccount(row, "Không có token!");
                    return;
                }

                while (true)
                {
                    switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                    {
                        //get proxy shoplike
                        case 1:
                            SetStatusAccount(row, "Đang lấy Proxy ShopLike ...");
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
                                        proxyGet = shopLike.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = shopLike.GetProxy();
                                        }
                                        ShopLike shopLike2 = shopLike;
                                        shopLike2.dangSuDung++;
                                        shopLike2 = shopLike;
                                        shopLike2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                            SetStatusAccount(row, "Đang lấy proxy Tinsoft...");
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
                                        proxyGet = tinsoftProxy.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = tinsoftProxy.GetProxy();
                                        }
                                        tinsoftProxy.dangSuDung++;
                                        tinsoftProxy.daSuDung++;
                                        break;
                                    }
                                    bool flag12 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + ("Đã dừng!"));
                                    break;
                                }
                                bool flag13 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag13 = false;
                                    }
                                }
                                if (!flag13)
                                {
                                    tinsoftProxy.dangSuDung--;
                                    tinsoftProxy.daSuDung--;
                                    continue;
                                }
                                goto default;
                            }
                        case 3:
                            SetStatusAccount(row, "Đang lấy Proxy MinProxy ...");
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
                                        proxyGet = minProxy.proxy;
                                        if (proxyGet == "")
                                        {
                                            proxyGet = minProxy.GetProxy();
                                        }
                                        MinProxy minProxy2 = minProxy;
                                        minProxy2.dangSuDung++;
                                        minProxy2 = minProxy;
                                        minProxy2.daSuDung++;
                                        break;
                                    }
                                    bool flag2 = true;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(row, text2 + "Đã dừng!");
                                    break;
                                }
                                bool flag3 = true;
                                if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                                {
                                    SetStatusAccount(row, text2 + "Delay check IP...");
                                    Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                                }
                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + proxyGet.Split(':')[0] + ") ";
                                    SetStatusAccount(row, text2 + "Check IP...");
                                    text3 = Helpers.Common.CheckProxy(proxyGet, 0);
                                    if (text3 == "")
                                    {
                                        flag3 = false;
                                    }
                                }
                                if (!flag3)
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
                                    goto stopBreak;
                                }

                                if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                                {
                                    text2 = "(IP: " + text3 + ") ";
                                }

                                if (isStop)
                                {
                                    goto stopBreak;
                                }

                                //delay run
                                lock (lock_checkDelayRequest)
                                {
                                    if (checkDelayRequest > 0)
                                    {
                                        //default delay 1-2 second
                                        int numRand = rd.Next(1, 2 + 1);
                                        if (numRand > 0)
                                        {
                                            int tickCount = Environment.TickCount;
                                            while ((Environment.TickCount - tickCount) / 1000 - numRand < 0)
                                            {
                                                SetStatusAccount(row, text2 + "Chạy tiến trình sau" + " {time}s...".Replace("{time}", (numRand - (Environment.TickCount - tickCount) / 1000).ToString()));
                                                Helpers.Common.DelayTime(0.5);
                                                if (isStop)
                                                {
                                                    goto stopBreak;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        checkDelayRequest++;
                                    }
                                }
                                SetStatusAccount(row, text2 + "Đang check...");
                                if (isStop)
                                {
                                    goto stopBreak;
                                }
                                while (true)
                                {
                                    if (!setting_general.GetValueBool("ckbKhongCheckIP") && proxyGet.Split(':').Length > 1)
                                    {
                                        string checkProxy = Helpers.Common.CheckProxy(proxyGet, 0);
                                        if (checkProxy == "")
                                        {
                                            SetStatusAccount(row, text2 + "Lỗi kết nối proxy!");
                                            break;
                                        }
                                    }
                                    proxy = proxyGet;

                                    //proccess
                                    bool checkLiveToken = CommonRequest.CheckLiveToken(cCookie, cToken, userAgent, proxy, typeProxy);
                                    if (!checkLiveToken)
                                    {
                                        SetStatusAccount(row, "Token Die!");
                                        CommonSQL.UpdateFieldToAccount(cId, "token", "");
                                        return;
                                    }
                                    //xoá all page trước đó !
                                    try
                                    {
                                        bool delAllPage = CommonSQL.DeleteAllPageById(cUid);
                                        if (delAllPage)
                                        {
                                            UpdateSttPageOnDatatable();
                                            CountTotalPage();
                                            CountCheckedPage(0);
                                            SetStatusAccount(row, "Xoá tất cả page trước đó thành công!");
                                            LoadPageFromFile();
                                            Thread.Sleep(1000);
                                        }
                                    }
                                    catch { }
                                    //
                                    string url = $"https://graph.facebook.com/me/accounts?fields=id,name,access_token,additional_profile_id&limit={limitQty}&access_token={cToken}";
                                    bool continueFetching = true;
                                    int pageSucess = 0;
                                    while (continueFetching)
                                    {
                                        var rs = CommonRequest.GetPage(url, proxy, userAgent);
                                        List<string> lstQuery = new List<string>();

                                        if (rs.Item1.Count > 0)
                                        {
                                            string data = rs.Item1[0].ToString();
                                            JArray jsonArray = JArray.Parse(data);

                                            foreach (var jsonString in jsonArray)
                                            {
                                                JObject jsonObject = (JObject)jsonString;

                                                string id = jsonObject["id"].ToString();
                                                string name = jsonObject["name"].ToString();
                                                string accessToken = jsonObject["access_token"].ToString();
                                                string additionalProfileId = jsonObject["additional_profile_id"]?.ToString() ?? id;

                                                lstQuery.Add(CommonSQL.ConvertToSqlInsertPage(additionalProfileId, name, "0", "0", uidAdmin, "", "-1", "0", "", "", "", "", id, "", "Public", "none", accessToken, "0"));
                                                pageSucess++;
                                            }
                                        }

                                        bool checkQr = lstQuery.Count > 0;

                                        if (checkQr)
                                        {
                                            lstQuery = CommonSQL.ConvertToSqlInsertPage(lstQuery);

                                            for (int j = 0; j < lstQuery.Count; j++)
                                            {
                                                Connector.Instance.ExecuteNonQuery(lstQuery[j]);
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(rs.Item2?.ToString()))
                                        {
                                            url = rs.Item2.ToString();
                                        }
                                        else
                                        {
                                            continueFetching = false;
                                        }
                                        SetStatusAccount(row, "Nghỉ 5s.Lấy thành công: " + pageSucess.ToString() + " pages");
                                        Thread.Sleep(5000);
                                    }
                                    CommonSQL.UpdateMultiFieldToAccount(cId, "pages", pageSucess.ToString());
                                    SetCellAccount(row, "cPages", pageSucess.ToString());
                                    SetStatusAccount(row, "Hoàn tất! Lấy thành công: " + pageSucess.ToString() + " pages");
                                    LoadPageFromFile();
                                    break;
                                }
                                return;
                            }
                        stopBreak:
                            SetStatusAccount(row, text2 + "Đã dừng!");
                            break;
                    }
                    break;
                }
                return;
            }
            catch (Exception ex)
            {
                SetStatusAccount(row, "Lỗi!");
                Helpers.Common.ExportError(null, ex);

            }
        }

        private void checkImapMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiemTraTaiKhoan(4);
        }

        private void giảiPhóngDungLượngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fGiaiPhongDungLuong(false));
        }

        private void giảiPhóngDungLượngShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool areyouok = MessageBoxHelper.ShowMessageBoxWithQuestion("Tính năng này sẽ dọn dẹp dung lượng máy tính và sau đó TẮT MÁY bạn có chắc muốn dùng?") == DialogResult.Yes;
            if (areyouok)
            {
                Helpers.Common.ShowForm(new fGiaiPhongDungLuong(true));
            }
        }

        private void checkLiveTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = MessageBoxHelper.ShowYesNo("Thay vì dùng cái này thì get token mới sẽ ổn hơn nhé!?") == DialogResult.Yes;
            if (flag)
            {
                this.KiemTraPage(0, false);
            }
        }
        private void KiemTraPage(int type, bool useProxy = false)
        {
            int iThread = 0;
            int maxThread = SettingsTool.GetSettings("configGeneral").GetValueInt("nudHideThread", 10);
            this.isStop = false;
            new Thread(delegate ()
            {
                this.cControl("start");
                switch (type)
                {
                    case 0:
                        {
                            int num4 = 0;
                            while (num4 < this.dtgvPage.Rows.Count && !this.isStop)
                            {
                                bool flag = Convert.ToBoolean(this.dtgvPage.Rows[num4].Cells["cChoosePage"].Value);
                                if (flag)
                                {
                                    bool flag2 = iThread < maxThread;
                                    if (flag2)
                                    {
                                        Interlocked.Increment(ref iThread);
                                        int row3 = num4++;
                                        new Thread(delegate ()
                                        {
                                            this.SetStatusPage(row3, "Đang kiểm tra...", -1);
                                            this.CheckTokenPage(row3);
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
                                    num4++;
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
                this.cControl("stop");
                LoadPageFromFile();
            }).Start();
        }

        private void CheckTokenPage(int row)
        {
            try
            {
                this.GetCellPaget(row, "cIdP");
                string token = this.GetCellAccount(row, "cTokenP");
                string cId = GetCellPaget(row, "cIdP");
                string text = ""; 
                string text3;

                string rsCheckToken = GraphFacebook($"https://graph.facebook.com/me?access_token={token}");
                if (rsCheckToken == "Error")
                {
                    text = "Die";
                    text3 = "Token Die";
                }
                else
                {
                    text = "Public";
                    text3 = "Token live";

                }
                this.SetStatusPage(row, text3, -1);
                this.SetInfoPage(cId, row, text);

            }
            catch
            {
                this.SetStatusPage(row, "Không check được!", -1);
            }
        }
        public static string GraphFacebook(string url, string proxy = "")
        {
            RequestXNet requestXNet = new RequestXNet("", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36", proxy, 0);
            string rs = "";
            try
            {
                rs = requestXNet.RequestGet(url);
                if (!string.IsNullOrEmpty(rs))
                {
                    return rs;
                }
            }
            catch
            {
                rs = "Error";
            }
            return rs;
        }

        private void checkAvatarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadSetting();
            int iThread = 0;
            int maxThread = setting_general.GetValueInt("nudHideThread", 10);
            isStop = false;
            new Thread((ThreadStart)delegate
            {
                cControl("start");
                int num = 0;
                while (num < dtgvPage.Rows.Count)
                {
                    Application.DoEvents();
                    if (isStop)
                    {
                        break;
                    }
                    if (Convert.ToBoolean(dtgvPage.Rows[num].Cells["cChoosePage"].Value))
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                SetStatusPage(row, "Đang kiểm tra...");
                                CheckMyAvatarPage(row);
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
                int tickCount = Environment.TickCount;
                while (iThread > 0 && Environment.TickCount - tickCount <= 30000)
                {
                    Application.DoEvents();
                    Thread.Sleep(300);
                }
                cControl("stop");
            }).Start();
        }
        private void CheckMyAvatarPage(int row, string token = "")
        {
            try
            {
                string uid = dtgvPage.Rows[row].Cells["cIdPage"].Value.ToString();
                string id = dtgvPage.Rows[row].Cells["cIdP"].Value.ToString();
                switch (CommonRequest.CheckAvatarFromUid(uid, token))
                {
                    case 0:
                        SetStatusPage(row, "Không có avatar!", -1);
                     //   SetCellAccount(row, "cAvatar", "No", true);
                       // CommonSQL.UpdateFieldToAccount(id, "avatar", "No");
                        break;
                    case 1:
                        SetStatusPage(row, "Đã có avatar!", -1);
                     //   SetCellAccount(row, "cAvatar", "Yes", true);
                     //   CommonSQL.UpdateFieldToAccount(id, "avatar", "Yes");
                        break;
                    case 2:
                        SetStatusPage(row, "Có lỗi xảy ra!", -1);
                       // SetCellAccount(row, "cAvatar", "Unknown", true);
                      //  CommonSQL.UpdateFieldToAccount(id, "avatar", "Unknown");
                        break;
                }
            }
            catch
            {
                SetStatusAccount(row, "Lỗi!", -1);
            }
        }
    }
}
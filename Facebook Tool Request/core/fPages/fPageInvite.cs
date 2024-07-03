using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.core.Enum;
using Facebook_Tool_Request.Helpers;
using Facebook_Tool_Request.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using cuakit;
using System.Drawing.Printing;
using Newtonsoft.Json.Linq;
using System.Windows.Interop;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium;
using System.Security.Cryptography;

namespace Facebook_Tool_Request.core.fPages
{
    public partial class fPageInvite : Form
    {
        private Random rd = new Random();
        private bool isStop = false;
        private JSON_Settings setting_general;
        private List<int> lstPossition = new List<int>();
        private List<Thread> lstThread = null;
        private List<ShopLike> listShopLike = null;
        private List<string> listProxyShopLike = null;
        private object lock_StartProxy = new object();
        private object lock_FinishProxy = new object();
        private object lock1 = new object();
        private JSON_Settings settings;

        public fPageInvite()
        {
            InitializeComponent();
            LoadInvitePage();
            LoadConfig();
            CountAcc();
        }

        private void LoadInvitePage(bool loadFull = true)
        {
            try
            {
                dtgvInvite.Rows.Clear();
                DataTable accFromFile = CommonSQL.GetInvitePageGop(loadFull);
                LoadDtgvInviteFromDbTable(accFromFile);
            }
            catch (Exception)
            {
            }
            this.settings = new JSON_Settings("configInvitePage", false);
        }
        private void LoadDtgvInviteFromDbTable(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string uid_clone = dr["uid_clone"].ToString();
                string uid_pages = dr["uid_pages"].ToString().Replace(",", Environment.NewLine);
                int countTotal = uid_pages.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
                dtgvInvite.Rows.Add(false, dtgvInvite.RowCount + 1, uid_clone, uid_pages, countTotal);
            }

        }

        private void SetRowColor(int indexRow)
        {
            if (indexRow == -1)
            {
                for (int i = 0; i < dtgvInvite.RowCount; i++)
                {
                    string infoAccount = GetInfoAccount(i);
                    if (infoAccount == "1")
                    {
                        dtgvInvite.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                    }
                    else if (infoAccount == "0")
                    {
                        dtgvInvite.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(242, 190, 34);
                    }
                    else
                    {
                        dtgvInvite.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                    }
                }
            }
        }
        public string GetInfoAccount(int indexRow)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvInvite, indexRow, "cActive");
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }
        private void LoadConfig()
        {
            setting_general = new JSON_Settings("configGeneral");
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
        private void button1_Click(object sender, EventArgs e)
        {
            
            //bool checkExist = CommonSQL.UpdateInvitePage("100009353051610", "100094816984689");
            // string name = CommonRequest.getInfoByUid("100094816984689");
            // if(!checkExist)
            // {
            //     bool insertCheck = CommonSQL.InsertInvitePageToDatabase("100094816984689", "100009353051610");
            //     if(insertCheck)
            //     {
            //         MessageBox.Show("OK");
            //     }
            // }
        }
        private void CountAcc()
        {
            try
            {
                lblTotal.Text = dtgvInvite.RowCount.ToString();
            }
            catch
            {
            }
        }
        private void dtgvInvite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    try
                    {
                        if (Convert.ToBoolean(dtgvInvite.CurrentRow.Cells["cChose"].Value))
                        {
                            dtgvInvite.CurrentRow.Cells["cChose"].Value = false;
                        }
                        else
                        {
                            dtgvInvite.CurrentRow.Cells["cChose"].Value = true;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            UpdateSelectCountRecord();
        }
        private void UpdateSelectCountRecord(int count = -1)
        {
            if (count == -1)
            {
                count = 0;
                for (int i = 0; i < dtgvInvite.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dtgvInvite.Rows[i].Cells["cChose"].Value))
                    {
                        count++;
                    }
                }
            }
            lblChoosed.Text = count.ToString();
        }
        private void dtgvInvite_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dtgvInvite.CurrentRow.Cells["cChose"].Value = !Convert.ToBoolean(dtgvInvite.CurrentRow.Cells["cChose"].Value);
                UpdateSelectCountRecord();
            }
            catch
            {
            }
        }
        private void dtgvInvite_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue == 32)
                {
                    for (int i = 0; i < dtgvInvite.SelectedRows.Count; i++)
                    {
                        int index = dtgvInvite.SelectedRows[i].Index;
                        if (Convert.ToBoolean(dtgvInvite.Rows[index].Cells["cChose"].Value))
                        {
                            dtgvInvite.Rows[index].Cells["cChose"].Value = false;
                        }
                        else
                        {
                            dtgvInvite.Rows[index].Cells["cChose"].Value = true;
                        }
                    }
                }
            }
            catch
            {
            }
            UpdateSelectCountRecord();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                LoadConfig();
                int maxThread = settings.GetValueInt("nudThreadRun", 5);

            //thread
            initThread:
                for (int i = 0; i < maxThread; i++)
                {
                    lstPossition.Add(0);
                }
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        {
                            listProxyShopLike = setting_general.GetValueList("txtApiShopLike");
                            if (listProxyShopLike.Count == 0)
                            {
                                MessageBoxHelper.ShowMessageBox(("Proxy ShopLike không đủ, vui lòng nhập thêm!"), 2);
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
                }
                rControl("start");
                isStop = false;
                int curChangeIp = 0;
                bool isChangeIPSuccess = false;
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    try
                    {
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 < dtgvInvite.Rows.Count && !isStop)
                            {
                                if (Convert.ToBoolean(dtgvInvite.Rows[num3].Cells["cChose"].Value))
                                {
                                    if (lstThread.Count < maxThread)
                                    {
                                        if (isStop)
                                        {
                                            goto joinThread;
                                        }
                                        int row = num3++;
                                        
                                        Thread thread = new Thread((ThreadStart)delegate
                                        {
                                            try
                                            {
                                                int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                                ExcuteOneThread(row, indexOfPossitionApp);
                                                Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                                DatagridviewHelper.SetStatusDataGridView(dtgvInvite, row, "cChose", false);
                                            }
                                            catch (Exception ex)
                                            {
                                                Helpers.Common.ExportError(null, ex);
                                            }
                                        });
                                        lstThread.Add(thread);
                                        thread.Start();
                                    }
                                    else if (setting_general.GetValueInt("ip_iTypeChangeIp") != 1)
                                    {
                                        for (int num4 = 0; num4 < lstThread.Count; num4++)
                                        {
                                            lstThread[num4].Join();
                                            lstThread.RemoveAt(num4--);
                                        }
                                        if (isStop)
                                        {
                                            goto joinThread;
                                        }
                                    }
                                    else
                                    {
                                        for (int num6 = 0; num6 < lstThread.Count; num6++)
                                        {
                                            if (!lstThread[num6].IsAlive)
                                            {
                                                lstThread.RemoveAt(num6--);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    num3++;
                                }
                                if (!isStop)
                                {
                                    continue;
                                }
                            }
                            goto joinThread;
                        joinThread:
                            for (int num7 = 0; num7 < lstThread.Count; num7++)
                            {
                                lstThread[num7].Join();
                            }
                            break;
                        }
                    }
                    catch (Exception ex2)
                    {
                        Helpers.Common.ExportError(null, ex2);
                    }
                    MessageBoxHelper.ShowMessageBox("Chạy Xong!" + Environment.NewLine + "Tải lại để làm mới danh sách Pages");
                    rControl("stop");
                }).Start();
                goto endProcess;
            endProcess:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
            }
        }
        private void ExcuteOneThread(int data, int indexPos)
        {
            int num = 0;
            int num2 = 0;
            string text = "";
            int num3 = (int)data;
            string cUidClone = DatagridviewHelper.GetStatusDataGridView(dtgvInvite, num3, "cUidClone");
            string cUidPage = DatagridviewHelper.GetStatusDataGridView(dtgvInvite, num3, "cUidPage");
            string cTotal = DatagridviewHelper.GetStatusDataGridView(dtgvInvite, num3, "cTotalInvite");

            int nudCountRun = this.settings.GetValueInt("nudCountRun", 2);
            int nudFrom = this.settings.GetValueInt("nudFrom", 3);
            int nudTo = this.settings.GetValueInt("nudTo", 5);
            string typeLogin = this.settings.GetValue("loginType", "0");
            if (cUidPage == null && cTotal != "0")
            {
                LoadStatusDatagridView(num3, "List Uid Page Không Đúng.");
                return;
            }
            string[] uidArray = cUidPage.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (uidArray.Count() <= nudCountRun)
            {
                nudCountRun = uidArray.Count();
            }

            string[] listUidPages = uidArray.Take(nudCountRun).ToArray();

            DataTable infoUid = CommonSQL.GetInfoProfileById(cUidClone);
            if(infoUid.Rows.Count <= 0)
            {
                LoadStatusDatagridView(num3, "Tài khoản chưa có!");
                return;
            }
            DataRow dataRow = infoUid.Rows[0];
            string cookie = dataRow["cookie1"].ToString();
            string userAgent = dataRow["useragent"].ToString();
            string uid = dataRow["uid"].ToString();
            string pass = dataRow["pass"].ToString();
            string tfa = dataRow["fa2"].ToString();
            ShopLike shopLike = null;
            string text5 = "";
            int typeProxy = 0;
            string text6 = "";
            string checkInPage = "";

            if (string.IsNullOrEmpty(userAgent))
            {
                 userAgent = Base.useragentDefault;
            }

            if(typeLogin == "1")
            {
                if (cookie == "")
                {
                    LoadStatusDatagridView(num3, text5 + "Không có Cookie!");
                    return;
                }
            }
            else
            {
                if(string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pass))
                {
                    LoadStatusDatagridView(num3, text5 + "Nick Nhận Page Thiếu: uid,pass!");
                    return;
                }
            }
            while (true)
            {
                if (isStop)
                {
                    LoadStatusDatagridView(num3, text5 + ("Đã dừng!"));
                    break;
                }
                switch (setting_general.GetValueInt("ip_iTypeChangeIp"))
                {
                    case 1:
                        LoadStatusDatagridView(num3, ("Đang lấy Proxy ShopLike ..."));
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
                                bool flag = true;
                            }
                            if (isStop)
                            {
                                LoadStatusDatagridView(num3, text5 + ("Đã dừng!"));
                                num = 1;
                                break;
                            }
                            bool flag2 = true;
                            if (setting_general.GetValueInt("nudDelayCheckIP") > 0)
                            {
                                LoadStatusDatagridView(num3, text5 + "Delay check IP...");
                                Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayCheckIP"));
                            }
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text5 = "(IP: " + text.Split(':')[0] + ") ";
                                LoadStatusDatagridView(num3, text5 + "Check IP...");
                                text6 = Helpers.Common.CheckProxy(text, 0);
                                if (text6 == "")
                                {
                                    flag2 = false;
                                }
                            }
                            if (!flag2)
                            {
                                ShopLike shopLike2 = shopLike;
                                shopLike2.dangSuDung--;
                                shopLike2 = shopLike;
                                shopLike2.daSuDung--;
                                continue;
                            }
                            goto default;
                        }
                    default:
                        try
                        {
                            SetStatusAccount(num3, text5 + "Đang chuẩn bị dữ liệu...");
                            string proxy = text;

                            if (isStop)
                            {
                                SetStatusAccount(num3, text5 + ("Đã dừng!"));
                                num = 1;
                                break;
                            }

                            int num5 = 0;
                            while (true)
                            {
                                SetStatusAccount(num3, text5 + "Đăng nhập...");
                                if (typeLogin == "1")
                                {
                                    SetStatusAccount(num3, text5 + "Đăng nhập bằng cookie...");
                                    if (!CommonRequest.CheckLiveCookie(cookie, userAgent, proxy, typeProxy).StartsWith("1|"))
                                    {
                                        SetRowColor(num3, 1);
                                        SetStatusAccount(num3, text5 + "Cookie die!");
                                        goto endQuit;
                                    }

                                }
                                else
                                {
                                    SetStatusAccount(num3, text5 + "Đăng nhập bằng mật khẩu...");
                                    //login uid pass
                                    string loginFb = loginFacebookRequest(uid, pass, tfa, userAgent, proxy);
                                    if (loginFb.StartsWith("1|"))
                                    {
                                        SetStatusAccount(num3, text5 + "Đăng nhập thành công!");
                                        cookie = loginFb.Split('|')[1];
                                    }
                                    else
                                    {
                                        SetStatusAccount(num3, text5 + loginFb.Split('|')[1]);
                                        goto endQuit;
                                    }
                                }

                                foreach (string uidPage in listUidPages)
                                    {
                                        string uidPageOK = uidPage.Replace(Environment.NewLine, "").Trim();

                                        Meta request = new Meta(cookie, userAgent, proxy);
                                        string namePage = ""; //CommonRequest.getInfoByUid(cUidPage);
                                        goto checkInvitePage;

                                    checkInvitePage:
                                        checkInPage = request.Get("https://www.facebook.com/" + uidPageOK).Result;
                                        if (Regex.Match(checkInPage, "\"USER_ID\":\"(.*?)\"").Groups[1].Value.Trim() != cUidClone.Trim() && Regex.Match(checkInPage, "\"personal_user_id\":\"(.*?)\"").Groups[1].Value.Trim() != cUidClone.Trim())
                                        {
                                            SetStatusAccount(num3, text5 + "Login False.");
                                            continue;
                                        }
                                        goto runThread;

                                    runThread:
                                        SetStatusAccount(num3, text5 + ("Login OK!"));
                                        if (isStop)
                                        {
                                            SetStatusAccount(num3, text5 + ("Đã dừng!"));
                                            break;
                                        }
                                        string profileAdminInvite = Regex.Match(checkInPage, "\"profile_admin_invite_id\":\"(.*?)\"")?.Groups[1]?.Value.ToString();
                                        if (string.IsNullOrEmpty(profileAdminInvite))
                                        {
                                            SetStatusAccount(num3, text5 + uidPageOK + " - Đã xác nhận!");
                                            bool checkUpdate = CommonSQL.UpdateInvitePage(cUidClone, uidPageOK, namePage, "Đã xác nhận!");
                                            continue;
                                        }
                                        var variables = "{\"input\":{\"client_mutation_id\":\"1\",\"actor_id\":\"" + cUidClone + "\",\"is_accept\":true,\"profile_admin_invite_id\":\"" + profileAdminInvite + "\",\"user_id\":\"" + uidPageOK + "\"},\"scale\":1.5}";
                                        var dataSend = $"&variables={variables}&doc_id=8184939334914113";
                                        string jsonInvite = request.PostGraphApi(dataSend).Result;

                                        JObject parsedJson = JObject.Parse(jsonInvite);

                                        if (parsedJson["data"]["accept_or_decline_profile_plus_admin_invite"] is JValue nullValue && nullValue.Value == null)
                                        {
                                            CommonSQL.UpdateInvitePage(cUidClone, uidPageOK, namePage, "ERR", "2");
                                            SetStatusAccount(num3, text5 + "ERROR: " + uidPageOK);
                                            Helpers.Common.SaveLog("error", $" - Uid: {cUidClone} - https://fb.com/{uidPageOK} - ERR", "invitePage");
                                            Helpers.Common.DelayTime(rd.Next(nudFrom, nudTo + 1));
                                            continue;
                                        }
                                        else if (parsedJson["data"]["accept_or_decline_profile_plus_admin_invite"] is JObject dataObject && dataObject["id"] != null)
                                        {
                                            string uidcheckOK = dataObject["id"].ToString();
                                            if (uidcheckOK != "" && uidcheckOK == uidPageOK)
                                            {
                                                bool checkUpdate = CommonSQL.UpdateInvitePage(cUidClone, uidPageOK, namePage, "OK");
                                                if (checkUpdate)
                                                {
                                                    SetStatusAccount(num3, text5 + "OK AD: " + uidPageOK);
                                                    Helpers.Common.SaveLog("success", $" - Uid: {cUidClone} - https://fb.com/{uidPageOK} - OK", "invitePage");
                                                    Helpers.Common.DelayTime(rd.Next(nudFrom, nudTo + 1));
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                CommonSQL.UpdateInvitePage(cUidClone, uidPageOK, namePage, "ERR", "2");
                                                SetStatusAccount(num3, text5 + "ERROR: " + uidPageOK);
                                                Helpers.Common.SaveLog("error", $" - Uid: {cUidClone} - https://fb.com/{uidPageOK} - ERR", "invitePage");
                                                Helpers.Common.DelayTime(rd.Next(nudFrom, nudTo + 1));
                                                continue;
                                            }
                                        }

                                        SetCountInvite(num3, (Int32.Parse(cTotal) - 1).ToString());

                                        //if (jaccepInvite["data"]["accept_or_decline_profile_plus_admin_invite"] != null  || jaccepInvite["data"]["accept_or_decline_profile_plus_admin_invite"].ToString() != "null")
                                        //{
                                        //    string uidcheckOK = jaccepInvite["data"]["accept_or_decline_profile_plus_admin_invite"]["id"].ToString();
                                        //    if (uidcheckOK != "" && uidcheckOK == uidPageOK)
                                        //    {
                                        //        bool checkUpdate = CommonSQL.UpdateInvitePage(cUidClone, uidPageOK, namePage, "OK");
                                        //        if (checkUpdate)
                                        //        {
                                        //            SetStatusAccount(num3, text5 + "OK AD: " + uidPageOK);
                                        //            SetCountInvite(num3, (Int32.Parse(cTotal) - 1).ToString());
                                        //            Helpers.Common.SaveLog("success", $" - Uid: {cUidClone} - https://fb.com/{uidPageOK} - OK", "invitePage");
                                        //            Helpers.Common.DelayTime(rd.Next(nudFrom, nudTo + 1));
                                        //            continue;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        SetStatusAccount(num3, text5 + "ERROR: " + uidPageOK);
                                        //        SetCountInvite(num3, (Int32.Parse(cTotal) - 1).ToString());
                                        //        Helpers.Common.SaveLog("error", $" - Uid: {cUidClone} - https://fb.com/{uidPageOK} - ERR", "invitePage");
                                        //        Helpers.Common.DelayTime(rd.Next(nudFrom, nudTo + 1));
                                        //        continue;
                                        //    }
                                        //}

                                    }
                                    goto endQuit;
                            endQuit:
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Helpers.Common.ExportError(ex, "Invite Page");
                            SetStatusAccount(num3, text5 + "Error App");
                        }
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
                }
            }
        }

        private void SetRowColor(int indexRow, int typeColor)
        {
            switch (typeColor)
            {
                case 1:
                    dtgvInvite.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                    break;
                case 2:
                    dtgvInvite.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 182);
                    break;
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                btnStop.Enabled = false;
                btnStop.Text = "Đang dừng...";
            }
            catch
            {
            }
        }
        private void rControl(string dt)
        {
            try
            {
                if (dt == "start")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        btnStop.Enabled = true;
                        btnStart.Enabled = false;
                    });
                }
                else if (dt == "stop")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        btnStop.Text = "Dừng";
                        btnStop.Enabled = false;
                        btnStart.Enabled = true;
                    });
                }
            }
            catch
            {
            }
        }
        private void LoadStatusDatagridView(int row, string status)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvInvite, row, "cStatus", status);
        }
        public void SetStatusAccount(int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvInvite, indexRow, "cStatus", value);
        }
        public void SetCountInvite(int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvInvite, indexRow, "cTotalInvite", value);
        }
        public void SetInfoAccount(string id, int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvInvite, indexRow, "cInfo", value);
            CommonSQL.UpdateFieldToAccount(id, "info", value);
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                this.settings.Update("nudThreadRun", this.nudThreadRun.Value);
                this.settings.Update("nudCountRun", this.nudCountRun.Value);
                this.settings.Update("nudFrom", this.nudFrom.Value);
                this.settings.Update("nudTo", this.nudTo.Value);
                bool rbCookie = this.rbCookie.Checked;
                int loginType = 0;
                if(rbCookie)
                {
                    loginType = 1;
                }
                this.settings.Update("loginType", loginType);

                this.settings.Save("");
                MessageBoxHelper.ShowMessageBox("Lưu thành công");
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox("Lỗi!", 1);
            }
        }

        private void fPageInvite_Load(object sender, EventArgs e)
        {
            this.nudThreadRun.Value = this.settings.GetValueInt("nudThreadRun", 3);
            this.nudCountRun.Value = this.settings.GetValueInt("nudCountRun", 2);
            this.nudFrom.Value = this.settings.GetValueInt("nudFrom", 3);
            this.nudTo.Value = this.settings.GetValueInt("nudTo", 5);

            string typeLogin = this.settings.GetValue("loginType", "0");
            if (typeLogin != "0")
            {
                if (typeLogin == "1")
                {
                    this.rbCookie.Checked = true;
                }
            }
            else
            {
                this.rbUidPass.Checked = true;
            }
        }

        private static string loginFacebookRequest(string uid, string pass, string tfa, string userAgent, string proxy = "")
        {
            string cookie = null;
            string rs = "0|Lỗi";
            Meta request = new Meta(cookie, userAgent, proxy);
            try
            {
                while(true)
                {
                    string loadHome = request.Get("https://en-gb.facebook.com/").Result;
                    string actionLogin = "https://en-gb.facebook.com" + Regex.Match(loadHome, "action=\"(.*?)\"").Groups[1].Value.Trim();
                    string Jazoest = Regex.Match(loadHome.Replace("\\", string.Empty), "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value.Trim();
                    string LSD = Regex.Match(loadHome.Replace("\\", string.Empty), "LSD\",(.*?),{\"token\":\"(.*?)\"").Groups[2].Value;
                    string loginPost = request.PostLogin(actionLogin, "jazoest=" + Jazoest + "&lsd=" + LSD + "&login_source=comet_headerless_login&next=&email=" + uid + "&pass=" + pass).Result;
                    string checkUrl = cuakit.Helpers.checkLoginAndCheckpoint(request.ResponseURI);
                    if (!string.IsNullOrEmpty(checkUrl))
                    {
                        rs = "0|" + checkUrl;
                        break;
                    }

                    if (loginPost.Contains("name=\"approvals_code\"") && string.IsNullOrEmpty(checkUrl))
                    {
                        string nh = Regex.Match(loginPost, "name=\"nh\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        string jazoest = Regex.Match(loginPost, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        string fb_dtsg = Regex.Match(loginPost, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value.Trim();
                        //if (string.IsNullOrEmpty(tfa))
                        //{
                        //    rs = "0|Không có 2FA";
                        //    break;
                        //}
                        //noti & get code 2fa
                        //TwoFactorAuthNet.TwoFactorAuth getcode = new TwoFactorAuthNet.TwoFactorAuth();
                        //string code = getcode.GetCode(tfa);

                        string cleanedAccount4 = tfa.Replace(" ", "").Replace("\n", "");
                        string code = Helpers.Common.GetTotp(cleanedAccount4);

                        string loginSubmit = request.PostCheckPoint("https://en-gb.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&no_fido=true&approvals_code={code}&submit%5BContinue%5D=Continue").Result;

                        if (loginSubmit.Contains("data-xui-error"))
                        {
                            rs = "0|Mã 2fa không đúng "+ code;
                            break;
                        }

                        //lưu vị trí
                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://en-gb.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&name_action_selected=save_device&submit%5BContinue%5D=Continue").Result;
                        }

                        //save browser
                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://en-gb.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BContinue%5D=Continue").Result;
                        }

                        //day la toi 
                        if (loginSubmit.Contains("name=\"submit[This was me]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://en-gb.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BThis+was+me%5D=This+was+me").Result;
                        }

                        if (loginSubmit.Contains("name=\"submit[Continue]\""))
                        {
                            loginSubmit = request.PostCheckPoint("https://en-gb.facebook.com/checkpoint/?next", $"jazoest={jazoest}&fb_dtsg={fb_dtsg}&nh={nh}&submit%5BContinue%5D=Continue").Result;
                        }

                        string checkUrl2 = cuakit.Helpers.checkLoginAndCheckpoint(request.ResponseURI);

                        if (!string.IsNullOrEmpty(checkUrl2))
                        {
                            rs = "0|" + checkUrl2;
                            break;
                        }

                        loginSubmit = request.Get("https://www.facebook.com/").Result;
                        if (loginSubmit.Contains("personal_user_id\":\"0\""))
                        {
                            rs = "0|Đăng nhập thất bại!";
                            break;
                        }
                        rs = "1|" + request.GetCookie();
                        break;
                    }
                    else
                    {
                        string loginSubmit = request.Get("https://www.facebook.com/").Result;
                        if (loginSubmit.Contains("personal_user_id\":\"0\""))
                        {
                            rs = "0|Đăng nhập thất bại!";
                            break;
                        }
                        rs = "1|" + request.GetCookie();
                        break;
                    }
                }    
            }catch (Exception ex)
            {
                rs = "0|Error App";
            }

            return rs;
        }

        private void xoáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAccount();
        }

        private void DeleteAccount()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dtgvInvite.RowCount; i++)
            {
                if (Convert.ToBoolean(dtgvInvite.Rows[i].Cells["cChose"].Value))
                {
                    list.Add(dtgvInvite.Rows[i].Cells["cUidClone"].Value.ToString());
                }
            }
            if (list.Count == 0)
            {
                MessageBoxHelper.ShowMessageBox(("Vui lòng chọn tài khoản cần xóa!"));
            }
            else
            {
                if (MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có chắc muốn xoá?") != DialogResult.Yes)
                {
                    return;
                }
                if (CommonSQL.DeleteInvitePageToDatabase(list))
                {
                    for (int j = 0; j < dtgvInvite.RowCount; j++)
                    {
                        if (Convert.ToBoolean(dtgvInvite.Rows[j].Cells["cChose"].Value))
                        {
                            dtgvInvite.Rows.RemoveAt(j--);
                        }
                    }
                    CountAcc();
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox(("Xóa thất bại, vui lòng thử lại sau!"), 2);
                }
            }
        }

        private void chọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("All");
        }

        private void ChoseRowInDatagrid(string modeChose)
        {
            switch (modeChose)
            {
                case "All":
                    {
                        for (int k = 0; k < dtgvInvite.RowCount; k++)
                        {
                            SetCellAccount(k, "cChose", true);
                        }
                        UpdateSelectCountRecord();
                        break;
                    }
                case "UnAll":
                    {
                        for (int j = 0; j < dtgvInvite.RowCount; j++)
                        {
                            SetCellAccount(j, "cChose", false);
                        }
                        UpdateSelectCountRecord();
                        break;
                    }

            }
        }
        public void SetCellAccount(int indexRow, string column, object value, bool isAllowEmptyValue = true)
        {
            if (isAllowEmptyValue || !(value.ToString().Trim() == ""))
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvInvite, indexRow, column, value);
            }
        }

        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("UnAll");
        }

        private void tảiLạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadInvitePage();
        }
    }
}

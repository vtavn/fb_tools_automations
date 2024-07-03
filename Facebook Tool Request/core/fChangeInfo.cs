using Bunifu.Framework.UI;
using cuakit;
using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.core.Enum;
using Facebook_Tool_Request.Helpers;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OtpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Facebook_Tool_Request.core
{
    public partial class fChangeInfo : Form
    {
        private Random rd = new Random();

        private bool isStop = false;

        public static bool isAdd;

        private JSON_Settings settings;

        private JSON_Settings setting_general;

        private JSON_Settings settingg;

        private object k = new object();

        private List<int> lstPossition = new List<int>();

        private List<Thread> lstThread = null;

        private Queue<string> lstUid = new Queue<string>();

        private string pathFolderAvatar = "";

        private string pathFolderCover = "";

        private string pathFolderTieuSu = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\tieusu";

        private string pathFolderThongTinCaNhan = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan";

        private string pathFileHo = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\doiten\\ho.txt";

        private string pathFileTenDem = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\doiten\\tendem.txt";

        private string pathFileTen = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\doiten\\ten.txt";

        private string pathFileMatKhau = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\doimk.txt";

        private List<string> lstHo = new List<string>();

        private List<string> lstTenDem = new List<string>();

        private List<string> lstTen = new List<string>();

        private List<string> lstTenRandom = new List<string>();

        private List<string> lstPassword = new List<string>();

        private List<string> lstNoiLamViec = new List<string>();

        private List<string> lstNoiLamViecTemp = new List<string>();

        private List<string> lstQueQuan = new List<string>();

        private List<string> lstQueQuanTemp = new List<string>();

        private List<string> lstThanhPho = new List<string>();

        private List<string> lstThanhPhoTemp = new List<string>();

        private List<string> lstTruongDH = new List<string>();

        private List<string> lstTruongDHTemp = new List<string>();

        private List<string> lstTruongTHPT = new List<string>();

        private List<string> lstTruongTHPTTemp = new List<string>();

        private List<string> lstMailAdd = new List<string>();

        private List<string> lstMailVerify = new List<string>();

        private string ipx = "";

        private List<string> lstMailLoi = new List<string>();

        private List<string> lstPathFileTieuSu = new List<string>();

        private List<string> lstPathFileTieuSuTemp = new List<string>();

        private List<string> lstPathFolderAvatar = new List<string>();

        private List<string> lstPathFolderAvatarTemp = new List<string>();

        private List<string> lstPathFolderCover = new List<string>();

        private List<string> lstPathFolderCoverTemp = new List<string>();

        private List<string> lstPathImage = new List<string>();

        private List<ShopLike> listShopLike = null;

        private List<string> listProxyShopLike = null;

        private List<MinProxy> listMinproxy = null;
        private List<string> listApiMinproxy = null;

        private object lock_StartProxy = new object();

        private object lock_FinishProxy = new object();

        private object lock1 = new object();

        private Queue<string> lstProxyTinsoft = new Queue<string>();
        private List<TinsoftProxy> listTinsoft = null;
        private List<string> listApiTinsoft = null;


        public fChangeInfo(List<string> listAcc)
        {
            InitializeComponent();
            if (listAcc != null && listAcc.Count > 0)
            {
                for (int i = 0; i < listAcc.Count; i++)
                {
                    dtgvAcc.Rows.Add(true, listAcc[i].Split('|')[0], listAcc[i].Split('|')[1], listAcc[i].Split('|')[2], listAcc[i].Split('|')[3], listAcc[i].Split('|')[4], listAcc[i].Split('|')[5], listAcc[i].Split('|')[6], listAcc[i].Split('|')[7], listAcc[i].Split('|')[8], listAcc[i].Split('|')[9], listAcc[i].Split('|')[10], listAcc[i].Split('|')[11], "");
                }
            }
            CountAcc();
            LoadConfig();
            isAdd = false;
        }
        private void LoadConfig()
        {
            settings = new JSON_Settings("configChange");
            setting_general = new JSON_Settings("configGeneral");
            settingg = new JSON_Settings("configCheckpoint");
        }
        private void CountAcc()
        {
            try
            {
                lblCountAcc.Text = dtgvAcc.RowCount.ToString();
            }
            catch
            {
            }
        }
        private void btnClosed_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnMaxWindow_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Maximized)
            {
                base.WindowState = FormWindowState.Normal;
            }
            else
            {
                base.WindowState = FormWindowState.Maximized;
            }
        }
        private void btnHideWindow_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void dtgvAcc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    try
                    {
                        if (Convert.ToBoolean(dtgvAcc.CurrentRow.Cells["cChose"].Value))
                        {
                            dtgvAcc.CurrentRow.Cells["cChose"].Value = false;
                        }
                        else
                        {
                            dtgvAcc.CurrentRow.Cells["cChose"].Value = true;
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
        private void UpdateSelectCountRecord()
        {
            int count = 0;
            for (int i = 0; i < dtgvAcc.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtgvAcc.Rows[i].Cells["cChose"].Value))
                {
                    count++;
                }
            }
            try
            {
                lblChoosed.Invoke((MethodInvoker)delegate
                {
                    lblChoosed.Text = count.ToString();
                });
            }
            catch
            {
            }
        }

        private void dtgvAcc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dtgvAcc.CurrentRow.Cells["cChose"].Value = !Convert.ToBoolean(dtgvAcc.CurrentRow.Cells["cChose"].Value);
                UpdateSelectCountRecord();
            }
            catch
            {
            }
        }

        private void dtgvAcc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue == 32)
                {
                    for (int i = 0; i < dtgvAcc.SelectedRows.Count; i++)
                    {
                        int index = dtgvAcc.SelectedRows[i].Index;
                        if (Convert.ToBoolean(dtgvAcc.Rows[index].Cells["cChose"].Value))
                        {
                            dtgvAcc.Rows[index].Cells["cChose"].Value = false;
                        }
                        else
                        {
                            dtgvAcc.Rows[index].Cells["cChose"].Value = true;
                        }
                    }
                }
            }
            catch
            {
            }
            UpdateSelectCountRecord();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Helpers.Common.CreateFolder("configschange\\fail");
                Process.Start("configschange\\fail");
            }
            catch
            {
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                btnPause.Enabled = false;
                btnPause.Text = "Đang dừng...";
            }
            catch
            {
            }
        }
        public List<string> CloneList(List<string> lstFrom)
        {
            //Discarded unreachable code: IL_004b, IL_0058
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
                LoadConfig();
                int maxThread = settings.GetValueInt("change_nudThread", 3);
                //process
                goto changeAvt;
            changeAvt:
                if (!settings.GetValueBool("change_ckbDoiAvatar"))
                {
                    goto changeCover;
                }
                pathFolderAvatar = settings.GetValue("change_txtPathAvatar");
                if (pathFolderAvatar == "")
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn đường dẫn folder chứa avatar!", 2);
                    return;
                }
                if (Directory.GetFiles(pathFolderAvatar).Length == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm ảnh vào folder avatar!", 2);
                    return;
                }
                lstPathFolderAvatar = Directory.GetFiles(pathFolderAvatar).ToList();
                lstPathFolderAvatarTemp = CloneList(lstPathFolderAvatar);
                goto changeCover;
            changeCover:
                if (!settings.GetValueBool("change_ckbDoiAnhBia") || settings.GetValueInt("change_typeUpCover") != 0)
                {
                    goto changeBio;
                }
                pathFolderCover = settings.GetValue("change_txtPathCover");
                if (pathFolderCover == "")
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng chọn đường dẫn folder chứa ảnh bìa!", 2);
                    return;
                }
                if (Directory.GetFiles(pathFolderCover).Length == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm ảnh bìa vào folder ảnh bìa!", 2);
                    return;
                }
                lstPathFolderCover = Directory.GetFiles(pathFolderCover).ToList();
                lstPathFolderCoverTemp = CloneList(lstPathFolderCover);
                goto changeBio;
            changeBio:
                if (!settings.GetValueBool("change_ckbThemMoTa"))
                {
                    goto changeInfos;
                }
                if (Directory.GetFiles(pathFolderTieuSu).Length == 0)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm tiểu sử!", 2);
                    return;
                }
                lstPathFileTieuSu = Directory.GetFiles(pathFolderTieuSu).ToList();
                lstPathFileTieuSuTemp = CloneList(lstPathFileTieuSu);
                goto changeInfos;
            changeInfos:
                if (settings.GetValueBool("change_ckbCapNhatThongTin"))
                {
                    string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\NoiLamViec.txt";

                    lstNoiLamViec = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\NoiLamViec.txt").ToList();
                    lstQueQuan = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\QueQuan.txt").ToList();
                    lstThanhPho = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\ThanhPho.txt").ToList();
                    lstTruongDH = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\TruongDH.txt").ToList();
                    lstTruongTHPT = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\configschange\\thongtincanhan\\TruongTHPT.txt").ToList();
                    lstNoiLamViec = Helpers.Common.RemoveEmptyItems(lstNoiLamViec);
                    lstNoiLamViecTemp = Helpers.Common.CloneList(lstNoiLamViec);
                    lstQueQuan = Helpers.Common.RemoveEmptyItems(lstQueQuan);
                    lstQueQuanTemp = Helpers.Common.CloneList(lstQueQuan);
                    lstThanhPho = Helpers.Common.RemoveEmptyItems(lstThanhPho);
                    lstThanhPhoTemp = Helpers.Common.CloneList(lstThanhPho);
                    lstTruongDH = Helpers.Common.RemoveEmptyItems(lstTruongDH);
                    lstTruongDHTemp = Helpers.Common.CloneList(lstTruongDH);
                    lstTruongTHPT = Helpers.Common.RemoveEmptyItems(lstTruongTHPT);
                    lstTruongTHPTTemp = Helpers.Common.CloneList(lstTruongTHPT);
                }
                goto changePass;
            changePass:
                if (!settings.GetValueBool("change_ckbDoiPass") || settings.GetValueInt("change_typeDoiPass") != 0)
                {
                    goto initThread;
                }
                lstPassword = Helpers.Common.RemoveEmptyItems(File.ReadAllLines(pathFileMatKhau).ToList());
                if (lstPassword.Count != 0)
                {
                    goto initThread;
                }
                MessageBoxHelper.ShowMessageBox(("Vui lòng nhập thêm mật khẩu muốn đổi!"), 2);
                goto endProcess;
            //init thread
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
                rControl("start");
                isAdd = true;
                isStop = false;
                int curChangeIp = 0;
                bool isChangeIPSuccess = false;
                int checkDelayChrome = 0;
                lstThread = new List<Thread>();
                new Thread((ThreadStart)delegate
                {
                    try
                    {
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 < dtgvAcc.Rows.Count && !isStop)
                            {
                                if (Convert.ToBoolean(dtgvAcc.Rows[num3].Cells["cChose"].Value))
                                {
                                    if (lstThread.Count < maxThread)
                                    {
                                        if (isStop)
                                        {
                                            goto joinThread;
                                        }
                                        int row = num3++;
                                        if (checkDelayChrome > 0)
                                        {
                                            Helpers.Common.DelayTime(setting_general.GetValueInt("nudDelayOpenChrome", 1));
                                        }
                                        checkDelayChrome++;
                                        Thread thread = new Thread((ThreadStart)delegate
                                        {
                                            try
                                            {
                                                int indexOfPossitionApp = Helpers.Common.GetIndexOfPossitionApp(ref lstPossition);
                                                ExcuteOneThread(row, indexOfPossitionApp);
                                                Helpers.Common.FillIndexPossition(ref lstPossition, indexOfPossitionApp);
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, row, "cChose", false);
                                            }
                                            catch (Exception ex3)
                                            {
                                                Helpers.Common.ExportError(null, ex3);
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
                    rControl("stop");
                    if (settings.GetValueBool("ckbAddMail"))
                    {
                        if (settings.GetValueInt("themMail") == 1 && !settings.GetValueBool("ckbMailDvFb"))
                        {
                            //File.WriteAllLines("configschange\\addmail\\hotmail.txt", lstMailAdd);
                        }
                        Helpers.Common.CreateFolder("configschange\\addmail");
                        File.WriteAllLines("configschange\\addmail\\MailLoi.txt", lstMailLoi);
                    }
                }).Start();
                goto endProcess;
            endProcess:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex);
            }
        }
        private void ExcuteOneThread(object data, int indexPos)
        {
            int num = 0;
            int num2 = 0;
            string text = "";
            int num3 = (int)data;
            Chrome chrome = null;
            string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cId");
            string text2 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cUid");
            string text3 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cPassword");
            string statusDataGridView2 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cCookies");
            string statusDataGridView3 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cFa2");
            string text4 = DatagridviewHelper.GetStatusDataGridView(dtgvAcc, num3, "cUseragent");
            if (text2 == "")
            {
                text2 = Regex.Match(statusDataGridView2, "c_user=(.*?);").Groups[1].Value;
            }
            ShopLike shopLike = null;
            TinsoftProxy tinsoftProxy = null;
            MinProxy minProxy = null;
            string text5 = "";
            int typeProxy = 0;
            string text6 = "";
            while (true)
            {
                if (isStop)
                {
                    LoadStatusDatagridView(num3, text5 + ("Đã dừng!"));
                    num = 1;
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
                    case 2:
                        LoadStatusDatagridView(num3, "Đang lấy proxy Tinsoft...");
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
                                    TinsoftProxy tinsoftProxy2 = tinsoftProxy;
                                    tinsoftProxy2.dangSuDung++;
                                    tinsoftProxy2 = tinsoftProxy;
                                    tinsoftProxy2.daSuDung++;
                                    break;
                                }
                                bool flag7 = true;
                            }
                            if (isStop)
                            {
                                LoadStatusDatagridView(num3, text5 + Language.GetValue("Đã dừng!"));
                                num = 1;
                                break;
                            }
                            bool flag8 = true;
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
                                    flag8 = false;
                                }
                            }
                            if (!flag8)
                            {
                                TinsoftProxy tinsoftProxy2 = tinsoftProxy;
                                tinsoftProxy2.dangSuDung--;
                                tinsoftProxy2 = tinsoftProxy;
                                tinsoftProxy2.daSuDung--;
                                continue;
                            }
                            goto default;
                        }
                    case 3:
                        LoadStatusDatagridView(num3, "Đang lấy Proxy MinProxy ...");
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
                            if (!setting_general.GetValueBool("ckbKhongCheckIP"))
                            {
                                text5 = "(IP: " + text6 + ") ";
                            }
                            if (text4 == "" || text.Split(':').Length == 4)
                            {
                                text4 = Base.useragentDefault;
                            }
                            try
                            {
                                SetStatusAccount(num3, text5 + "Đang mở trình duyệt...");
                                string app = "data:,";
                                Point pointFromIndexPosition = Helpers.Common.GetPointFromIndexPosition(indexPos, setting_general.GetValueInt("cbbColumnChrome", 3), setting_general.GetValueInt("cbbRowChrome", 2));
                                Point sizeChrome = Helpers.Common.GetSizeChrome(setting_general.GetValueInt("cbbColumnChrome", 3), setting_general.GetValueInt("cbbRowChrome", 2));
                                if (text4 == "")
                                {
                                    text4 = Base.useragentDefault;
                                }
                                string text7 = "";
                                if (text2 != "")
                                {
                                    text7 = ConfigHelper.GetPathProfile() + "\\" + text2;
                                    if (!settings.GetValueBool("ckbCreateProfile") && !Directory.Exists(text7))
                                    {
                                        text7 = "";
                                    }
                                }
                                Chrome chrome2 = new Chrome();
                                chrome2.IndexChrome = num3;
                                chrome2.DisableImage = !Convert.ToBoolean((setting_general.GetValue("ckbShowImageInteract") == "") ? "false" : setting_general.GetValue("ckbShowImageInteract"));
                                chrome2.UserAgent = text4;
                                chrome2.ProfilePath = text7;
                                chrome2.Size = sizeChrome;
                                chrome2.Position = pointFromIndexPosition;
                                chrome2.TimeWaitForSearchingElement = 3;
                                chrome2.TimeWaitForLoadingPage = 120;
                                chrome2.Proxy = text;
                                chrome2.TypeProxy = typeProxy;
                                chrome2.DisableSound = true;
                                chrome2.App = app;
                                chrome2.IsUsePortable = setting_general.GetValueBool("ckbUsePortable");
                                chrome2.PathToPortableZip = setting_general.GetValue("txtPathToPortableZip");
                                chrome = chrome2;
                                if (setting_general.GetValue("sizeChrome").Contains("x"))
                                {
                                    chrome.IsUseEmulator = true;
                                    string text8 = setting_general.GetValue("sizeChrome").Substring(0, setting_general.GetValue("sizeChrome").LastIndexOf('x'));
                                    int pixelRatio = Convert.ToInt32(setting_general.GetValue("sizeChrome").Split('x')[2]);
                                    chrome.Size_Emulator = new Point(Convert.ToInt32(text8.Split('x')[0]), Convert.ToInt32(text8.Split('x')[1]));
                                    if (text4 == "")
                                    {
                                        chrome.UserAgent = Base.useragentDefault;
                                    }
                                    chrome.PixelRatio = pixelRatio;
                                }
                                if (isStop)
                                {
                                    SetStatusAccount(num3, text5 + "Đã dừng!");
                                    num = 1;
                                    break;
                                }
                                if (setting_general.GetValueInt("typeBrowser") != 0)
                                {
                                    chrome.LinkToOtherBrowser = setting_general.GetValue("txtLinkToOtherBrowser");
                                }
                                int num5 = 0;
                                while (true)
                                {
                                    if (!chrome.Open())
                                    {
                                        SetStatusAccount(num3, text5 + "Lỗi mở trình duyệt!");
                                        num = 1;
                                        break;
                                    }
                                    chrome.ExecuteScript("document.title=\"proxyauth=" + text + "\"");
                                    chrome.DelayTime(2.0);
                                    if (!setting_general.GetValueBool("ckbKhongCheckIP") && text.Split(':').Length > 1)
                                    {
                                        chrome.GotoURL("https://api.myip.com/");
                                        chrome.DelayTime(1.0);
                                        string pageSource = chrome.GetPageSource();
                                        if (!pageSource.Contains("ip"))
                                        {
                                            chrome.Close();
                                            num5++;
                                            if (num5 < 3)
                                            {
                                                continue;
                                            }
                                            SetStatusAccount(num3, text5 + "Lỗi kết nối proxy!");
                                            num = 1;
                                            break;
                                        }
                                    }
                                    if (!chrome.GetProcess())
                                    {
                                        SetStatusAccount(num3, text5 + "Không check được chrome!");
                                        num = 1;
                                        break;
                                    }
                                    SetStatusAccount(num3, text5 + "Đang đăng nhập...");
                                    bool flag10 = false;
                                    string text9 = "https://www.facebook.com/";
                                    if (settings.GetValueInt("typeBrowserLogin") == 0)
                                    {
                                        text9 = "https://m.facebook.com/";
                                    }
                                    if (!(text7.Trim() != ""))
                                    {
                                        goto loginFacebook;
                                    }
                                    num2 = CommonChrome.CheckLiveCookie(chrome, text9);
                                    if (!chrome.GetURL().Contains("facebook.com/confirm"))
                                    {
                                        switch (num2)
                                        {
                                            case 1:
                                                flag10 = true;
                                                goto loginFacebook;
                                            case -2:
                                                chrome.Status = StatusChromeAccount.ChromeClosed;
                                                goto endQuit;
                                            case -3:
                                                chrome.Status = StatusChromeAccount.NoInternet;
                                                goto endQuit;
                                            case 2:
                                                break;
                                            default:
                                                goto loginFacebook;
                                        }
                                        chrome.Status = StatusChromeAccount.Checkpoint;
                                    }
                                    goto runThread;
                                runThread:
                                    SetStatusAccount(num3, text5 + ("Đăng nhập thành công!"));
                                    if (chrome.GetURL().StartsWith("https://m.facebook.com/si/actor_experience/actor_gateway/nt/"))
                                    {
                                        chrome.ClickWithAction(4, "span");
                                        chrome.DelayTime(2.0);
                                    }
                                    if (chrome.GetURL().Contains("gettingstarted"))
                                    {
                                        for (int j = 0; j < 5; j++)
                                        {
                                            if (chrome.CheckExistElement("#nux-nav-button", 3.0) != 1)
                                            {
                                                break;
                                            }
                                            chrome.Click(1, "nux-nav-button");
                                            chrome.DelayTime(2.0);
                                        }
                                    }
                                    //process in here
                                    if (settings.GetValueBool("change_ckbDoiNgonNgu"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Đổi ngôn ngữ...");
                                        num2 = ChangeLanguage(chrome, settings.GetValue("change_cbbNgonNgu"));
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                        break;
                                                }
                                                goto endQuit;
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbCapNhatThongTin"))
                                    {
                                        num2 = 0;
                                        string text13 = "";
                                        lock (lstNoiLamViecTemp)
                                        {
                                            if (lstNoiLamViecTemp.Count == 0)
                                            {
                                                lstNoiLamViecTemp = Helpers.Common.CloneList(lstNoiLamViec);
                                            }
                                            if (lstNoiLamViecTemp.Count > 0)
                                            {
                                                text13 = lstNoiLamViecTemp[rd.Next(0, lstNoiLamViecTemp.Count)];
                                                lstNoiLamViecTemp.Remove(text13);
                                            }
                                        }
                                        if (text13 != "")
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhật nơi làm việc...");
                                            num2 = ChangeWork(chrome, text13);
                                            switch (num2)
                                            {
                                                case -2:
                                                    num = 2;
                                                    goto endQuit;
                                                case 0:
                                                    switch (CommonChrome.CheckStatusChrome(chrome))
                                                    {
                                                        case -1:
                                                            num = 3;
                                                            break;
                                                        case -2:
                                                            num = 2;
                                                            break;
                                                        case -3:
                                                            num = 1;
                                                            LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                            break;
                                                    }
                                                    goto endQuit;
                                            }
                                        }
                                        string text14 = "";
                                        lock (lstQueQuanTemp)
                                        {
                                            if (lstQueQuanTemp.Count == 0)
                                            {
                                                lstQueQuanTemp = Helpers.Common.CloneList(lstQueQuan);
                                            }
                                            if (lstQueQuanTemp.Count > 0)
                                            {
                                                text14 = lstQueQuanTemp[rd.Next(0, lstQueQuanTemp.Count)];
                                                lstQueQuanTemp.Remove(text14);
                                            }
                                        }
                                        if (text14 != "")
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhật quê quán...");
                                            num2 = ChangeHomeTown(chrome, text14);
                                            switch (num2)
                                            {
                                                case -2:
                                                    num = 2;
                                                    goto endQuit;
                                                case 0:
                                                    switch (CommonChrome.CheckStatusChrome(chrome))
                                                    {
                                                        case -1:
                                                            num = 3;
                                                            break;
                                                        case -2:
                                                            num = 2;
                                                            break;
                                                        case -3:
                                                            num = 1;
                                                            LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                            break;
                                                    }
                                                    goto endQuit;
                                            }
                                        }
                                        string text15 = "";
                                        lock (lstThanhPhoTemp)
                                        {
                                            if (lstThanhPhoTemp.Count == 0)
                                            {
                                                lstThanhPhoTemp = Helpers.Common.CloneList(lstThanhPho);
                                            }
                                            if (lstThanhPhoTemp.Count > 0)
                                            {
                                                text15 = lstThanhPhoTemp[rd.Next(0, lstThanhPhoTemp.Count)];
                                                lstThanhPhoTemp.Remove(text15);
                                            }
                                        }
                                        if (text15 != "")
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhật thành phố...");
                                            num2 = ChangeCity(chrome, text15);
                                            switch (num2)
                                            {
                                                case -2:
                                                    num = 2;
                                                    goto endQuit;
                                                case 0:
                                                    switch (CommonChrome.CheckStatusChrome(chrome))
                                                    {
                                                        case -1:
                                                            num = 3;
                                                            break;
                                                        case -2:
                                                            num = 2;
                                                            break;
                                                        case -3:
                                                            num = 1;
                                                            LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                            break;
                                                    }
                                                    goto endQuit;
                                            }
                                        }
                                        string text16 = "";
                                        lock (lstTruongDHTemp)
                                        {
                                            if (lstTruongDHTemp.Count == 0)
                                            {
                                                lstTruongDHTemp = Helpers.Common.CloneList(lstTruongDH);
                                            }
                                            if (lstTruongDHTemp.Count > 0)
                                            {
                                                text16 = lstTruongDHTemp[rd.Next(0, lstTruongDHTemp.Count)];
                                                lstTruongDHTemp.Remove(text16);
                                            }
                                        }
                                        if (text16 != "")
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhật trường ĐH...");
                                            num2 = ChangeDaiHoc(chrome, text16);
                                            switch (num2)
                                            {
                                                case -2:
                                                    num = 2;
                                                    goto endQuit;
                                                case 0:
                                                    switch (CommonChrome.CheckStatusChrome(chrome))
                                                    {
                                                        case -1:
                                                            num = 3;
                                                            break;
                                                        case -2:
                                                            num = 2;
                                                            break;
                                                        case -3:
                                                            num = 1;
                                                            LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                            break;
                                                    }
                                                    goto endQuit;
                                            }
                                        }
                                        string text17 = "";
                                        lock (lstTruongTHPTTemp)
                                        {
                                            if (lstTruongTHPTTemp.Count == 0)
                                            {
                                                lstTruongTHPTTemp = Helpers.Common.CloneList(lstTruongTHPT);
                                            }
                                            if (lstTruongTHPTTemp.Count > 0)
                                            {
                                                text17 = lstTruongTHPTTemp[rd.Next(0, lstTruongTHPTTemp.Count)];
                                                lstTruongTHPTTemp.Remove(text17);
                                            }
                                        }
                                        if (text17 != "")
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhật trường THPT...");
                                            num2 = ChangePTTH(chrome, text17);
                                            switch (num2)
                                            {
                                                case -2:
                                                    num = 2;
                                                    goto endQuit;
                                                case 0:
                                                    switch (CommonChrome.CheckStatusChrome(chrome))
                                                    {
                                                        case -1:
                                                            num = 3;
                                                            break;
                                                        case -2:
                                                            num = 2;
                                                            break;
                                                        case -3:
                                                            num = 1;
                                                            LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                            break;
                                                    }
                                                    goto endQuit;
                                            }
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbDoiNgaySinh"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Đổi ngày sinh...");
                                        int num6 = settings.GetValueInt("change_nudNgayFrom");
                                        int num7 = settings.GetValueInt("change_nudNgayTo");
                                        int valueInt3 = settings.GetValueInt("change_nudThangFrom");
                                        int valueInt4 = settings.GetValueInt("change_nudThangTo");
                                        int valueInt5 = settings.GetValueInt("change_nudNamFrom");
                                        int valueInt6 = settings.GetValueInt("change_nudNamTo");
                                        if (num6 == 31)
                                        {
                                            num6 = 30;
                                        }
                                        if (num7 == 31)
                                        {
                                            num7 = 30;
                                        }
                                        int num8 = rd.Next(num6, num7 + 1);
                                        int num9 = rd.Next(valueInt3, valueInt4 + 1);
                                        int num10 = rd.Next(valueInt5, valueInt6 + 1);
                                        if (num9 == 2 && num8 > 28)
                                        {
                                            num8 = 28;
                                        }
                                        num2 = ChangeNgaySinh(chrome, num8, num9, num10);
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                        break;
                                                }
                                                goto endQuit;
                                            case 1:
                                                {
                                                    string arg = ((num9 < 10) ? ("0" + num9) : (num9.ToString() ?? ""));
                                                    string arg2 = ((num8 < 10) ? ("0" + num8) : (num8.ToString() ?? ""));
                                                    CommonSQL.UpdateFieldToAccount(statusDataGridView, "birthday", $"{arg}/{arg2}/{num10}");
                                                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cBirthday", $"{arg}/{arg2}/{num10}");
                                                    break;
                                                }
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbDoiAvatar"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Đổi avatar...");
                                        string text10 = "";
                                        lock (lstPathFolderAvatarTemp)
                                        {
                                            if (lstPathFolderAvatarTemp.Count == 0)
                                            {
                                                lstPathFolderAvatarTemp = CloneList(lstPathFolderAvatar);
                                            }
                                            if (!settings.GetValueBool("change_ckbAvatarThuTu"))
                                            {
                                                text10 = lstPathFolderAvatarTemp[rd.Next(rd.Next(0, lstPathFolderAvatarTemp.Count))];
                                                lstPathFolderAvatarTemp.Remove(text10);
                                            }
                                            else
                                            {
                                                text10 = lstPathFolderAvatarTemp[num3];
                                            }
                                        }
                                        num2 = UpAvatar(chrome, text10, rd);
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                        break;
                                                }
                                                goto endQuit;
                                            case 1:
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView, "avatar", "Có");
                                                if (settings.GetValueBool("ckbAutoDeleteFile") && File.Exists(text10))
                                                {
                                                    File.Delete(text10);
                                                }
                                                break;
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbDoiAnhBia"))
                                    {
                                        num2 = 0;
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + " Đổi ảnh bìa...");
                                        string text11 = "";
                                        if (settings.GetValueInt("change_typeUpCover") == 0)
                                        {
                                            lock (lstPathFolderCoverTemp)
                                            {
                                                if (lstPathFolderCoverTemp.Count == 0)
                                                {
                                                    lstPathFolderCoverTemp = CloneList(lstPathFolderCover);
                                                }
                                                if (!settings.GetValueBool("change_ckbCoverThuTu"))
                                                {
                                                    text11 = lstPathFolderCoverTemp[rd.Next(0, lstPathFolderCoverTemp.Count)];
                                                    lstPathFolderCoverTemp.Remove(text11);
                                                }
                                                else
                                                {
                                                    text11 = lstPathFolderCoverTemp[num3];
                                                }
                                            }
                                            num2 = UpCover(chrome, text11, rd);
                                        }
                                        else
                                        {
                                            num2 = UpCoverNgheThuat(chrome, rd);
                                        }
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                        break;
                                                }
                                                goto endQuit;
                                            case 1:
                                                if (settings.GetValueBool("ckbAutoDeleteFile") && text11 != "" && File.Exists(text11))
                                                {
                                                    File.Delete(text11);
                                                }
                                                break;
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbThemMoTa"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Cập nhập tiểu sử...");
                                        string tieuSu = "";
                                        lock (lstPathFileTieuSuTemp)
                                        {
                                            if (lstPathFileTieuSuTemp.Count == 0)
                                            {
                                                lstPathFileTieuSuTemp = CloneList(lstPathFileTieuSu);
                                            }
                                            string text12 = lstPathFileTieuSuTemp[rd.Next(0, lstPathFileTieuSuTemp.Count)];
                                            lstPathFileTieuSuTemp.Remove(text12);
                                            tieuSu = File.ReadAllText(text12);
                                        }
                                        num2 = ChangeTieuSu(chrome, tieuSu);
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Disconnect Internet!");
                                                        break;
                                                }
                                                goto endQuit;
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbDoiPass"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Đổi mật khẩu...");
                                        string text21 = "";
                                        text21 = ((settings.GetValueInt("change_typeDoiPass") != 0) ? ("Cua@" + Helpers.Common.CreateRandomStringNumber(6, rd)) : lstPassword[rd.Next(0, lstPassword.Count)]);
                                        num2 = ((!settings.GetValueBool("change_ckbDoiPassUseLinkHacked")) ? ChangePass(chrome, text3, text21) : ChangePassUseLinkHacked(chrome, text3, text21));
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Đổi mật khẩu thất bại..."));
                                                WriteFile(num3, 1);
                                                num = 1;
                                                goto endQuit;
                                            case 1:
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView, "pass", text21);
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cPassword", text21);
                                                text3 = text21;
                                                break;
                                            case 2:
                                                num = 4;
                                                goto endQuit;
                                        }
                                    }
                                    if(settings.GetValueBool("change_ckbKhienAvt"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Bật khiên avatar...");
                                        if(OpenGuardAvatart(chrome, text2))
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Bật khiên thành công...");
                                            num = 2;
                                            goto endQuit;
                                        }
                                        else
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Bật khiên thất bại...");
                                            WriteFile(num3, 1);
                                            num = 1;
                                            goto endQuit;

                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbGioiTinh"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Đổi giới tính") + "...");
                                        int valueInt2 = settings.GetValueInt("change_typeGioiTinh");
                                        num2 = ChangeGender(chrome, valueInt2, statusDataGridView);
                                        switch (num2)
                                        {
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case 0:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        break;
                                                    case -2:
                                                        num = 2;
                                                        break;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                        break;
                                                }
                                                goto endQuit;
                                        }
                                    }
                                    if (settings.GetValueBool("ckbXoaSdt"))
                                    {
                                        num2 = RemovePhone(chrome, text3);
                                        switch (num2)
                                        {
                                            case 0:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Lỗi xóa SĐT!"));
                                                WriteFile(num3, 2);
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView, "ghiChu", "Lỗi xóa sđt!");
                                                num = 1;
                                                goto endQuit;
                                            case 1:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Xóa SĐT thành công..."));
                                                goto default;
                                            case 2:
                                                num = 4;
                                                goto endQuit;
                                            default:
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        goto endQuit;
                                                    case -2:
                                                        num = 2;
                                                        goto endQuit;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                        goto endQuit;
                                                }
                                                break;
                                        }
                                    }
                                    if (settings.GetValueBool("ckbLogOut"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Đang đăng xuất các thiết bị cũ..."));
                                        if (Logout(chrome))
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Đang đăng xuất các thiết bị cũ!"));
                                        }
                                        else
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Lỗi đăng xuất các thiết bị cũ!"));
                                        }
                                        switch (CommonChrome.CheckStatusChrome(chrome))
                                        {
                                            case -1:
                                                num = 3;
                                                break;
                                            case -2:
                                                num = 2;
                                                break;
                                            case -3:
                                                num = 1;
                                                LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                break;
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckbXoaThietBiTinCay"))
                                    {
                                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Đang xóa thiết bị tin cậy..."));
                                        chrome.GotoURL("https://m.facebook.com/settings/security/two_factor/devices/");
                                        chrome.DelayThaoTacNho();
                                        if (chrome.CheckExistElement("[data-sigil=\"removable-area marea\"] button", 10.0) == 1)
                                        {
                                            chrome.ExecuteScript("var x=document.querySelectorAll('[data-sigil=\"touchable removable-area-button\"]').length;for(var i=1;i<=x;i++){document.querySelectorAll('[data-sigil=\"touchable removable-area-button\"]')[x-i].click()}");
                                            chrome.DelayTime(1.0);
                                        }
                                    }
                                    if (settings.GetValueBool("change_ckb2fa"))
                                    {
                                        if (settings.GetValueInt("change_type2fa") == 0)
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Bật 2fa..."));
                                            statusDataGridView3 = Create2FA(chrome, text3);
                                            if (statusDataGridView3.StartsWith("1|"))
                                            {
                                                statusDataGridView3 = statusDataGridView3.Split('|')[1];
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView, "fa2", statusDataGridView3);
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cFa2", statusDataGridView3);
                                            }
                                        }
                                        else
                                        {
                                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Tắt 2fa..."));
                                            if (Remove2FA(chrome, text3))
                                            {
                                                CommonSQL.UpdateFieldToAccount(statusDataGridView, "fa2", "");
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cFa2", "");
                                            }
                                        }
                                        switch (CommonChrome.CheckStatusChrome(chrome))
                                        {
                                            case -1:
                                                num = 3;
                                                goto endQuit;
                                            case -2:
                                                num = 2;
                                                goto endQuit;
                                            case -3:
                                                num = 1;
                                                LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                goto endQuit;
                                        }
                                    }
                                    if (settings.GetValueBool("ckbAddMail"))
                                    {
                                        num2 = ChangeMail(chrome, text3, num3, text5, statusDataGridView);
                                        switch (num2)
                                        {
                                            case 0:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Lỗi Add mail!"));
                                                WriteFile(num3, 2);
                                                goto default;
                                            case 1:
                                                //CommonSQL.UpdateFieldToAccount(statusDataGridView, "pass", text21);
                                                //DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cPassword", text21);
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Thêm mail thành công..."));
                                                goto default;
                                            case 2:
                                                num = 4;
                                                goto endQuit;
                                            case 3:
                                                if (settings.GetValueInt("themMail") == 1)
                                                {
                                                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Hết mail!"));
                                                }
                                                else
                                                {
                                                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Không lấy được Temp-mail!"));
                                                }
                                                isStop = true;
                                                goto default;
                                            case 4:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Fb bắt nhập pass!"));
                                                goto default;
                                            case 5:
                                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Fb không gửi otp về mail!"));
                                                goto default;
                                            default:
                                                if (num2 != 1 && settings.GetValueBool("ckbCloseChrome"))
                                                {
                                                    num = 1;
                                                    goto endBreak;
                                                }
                                                switch (CommonChrome.CheckStatusChrome(chrome))
                                                {
                                                    case -1:
                                                        num = 3;
                                                        goto endQuit;
                                                    case -2:
                                                        num = 2;
                                                        goto endQuit;
                                                    case -3:
                                                        num = 1;
                                                        LoadStatusDatagridView(num3, text5 + "Mất Kết Nối Mạng!");
                                                        goto endQuit;
                                                }
                                                break;
                                        }
                                    }

                                    break;
                                loginFacebook:
                                    if (!flag10)
                                    {
                                        string text22 = "";
                                        switch (settings.GetValueInt("typeLogin"))
                                        {
                                            case 0:
                                                if (text2.Trim() == "" || text3.Trim() == "")
                                                {
                                                    if (text2.Trim() == "")
                                                    {
                                                        SetStatusAccount(num3, text5 + ("Không tìm thấy UID!"));
                                                    }
                                                    else if (text3.Trim() == "")
                                                    {
                                                        SetStatusAccount(num3, text5 + ("Không tìm thấy Pass!"));
                                                    }
                                                    num = 1;
                                                    goto endBreak;
                                                }
                                                SetStatusAccount(num3, text5 + ("Đăng nhập bằng uid|pass..."));
                                                text22 = ((!settings.GetValueBool("change_ckbGiaiCheckPoint")) ? CommonChrome.LoginFacebookUsingUidPassNew(chrome, text2, text3, statusDataGridView3, text9, setting_general.GetValueInt("tocDoGoVanBan"), setting_general.GetValueBool("ckbDontSaveBrowser")) : CommonChrome.LoginFacebookUsingUidPassNew(chrome, text2, text3, statusDataGridView3, "https://www.facebook.com", setting_general.GetValueInt("tocDoGoVanBan"), setting_general.GetValueBool("ckbDontSaveBrowser")));
                                                try
                                                {
                                                    num2 = Convert.ToInt32(text22);
                                                }
                                                catch
                                                {
                                                    num2 = -1;
                                                }
                                                goto default;
                                            case 1:
                                                if (statusDataGridView2.Trim() == "")
                                                {
                                                    SetStatusAccount(num3, text5 + ("Không tìm thấy Cookie!"));
                                                    num = 1;
                                                    goto endBreak;
                                                }
                                                SetStatusAccount(num3, text5 + ("Đăng nhập bằng cookie..."));
                                                num2 = Convert.ToInt32(CommonChrome.LoginFacebookUsingCookie(chrome, statusDataGridView2, text9));
                                                goto default;
                                            default:
                                                if (chrome.GetURL().Contains("facebook.com/confirm"))
                                                {
                                                    break;
                                                }
                                                if (settings.GetValueInt("typeLogin") != 1)
                                                {
                                                    switch (num2)
                                                    {
                                                        case -2:
                                                            chrome.Status = StatusChromeAccount.ChromeClosed;
                                                            goto default;
                                                        case -1:
                                                            SetStatusAccount(num3, text5 + text22);
                                                            goto default;
                                                        case 0:
                                                            SetStatusAccount(num3, text5 + ("Đăng nhập thất bại!"));
                                                            goto default;
                                                        case 1:
                                                            flag10 = true;
                                                            goto default;
                                                        case 2:
                                                            chrome.Status = StatusChromeAccount.Checkpoint;
                                                            SetInfoAccount(statusDataGridView, num3, ("Checkpoint"));
                                                            break;
                                                        case 3:
                                                            SetStatusAccount(num3, text5 + ("Không có 2fa!"));
                                                            goto default;
                                                        case 4:
                                                            SetStatusAccount(num3, text5 + ("Tài khoản không đúng!"));
                                                            goto default;
                                                        case 5:
                                                            SetStatusAccount(num3, text5 + ("Mật khẩu không đúng!"));
                                                            SetInfoAccount(statusDataGridView, num3, "Changed pass");
                                                            goto default;
                                                        case 6:
                                                            SetStatusAccount(num3, text5 + ("Mã 2fa không đúng!"));
                                                            goto default;
                                                        case 7:
                                                            chrome.Status = StatusChromeAccount.NoInternet;
                                                            goto default;
                                                        default:
                                                            if (!flag10)
                                                            {
                                                                ScreenCaptureError(chrome, text2, 1);
                                                                num = 1;
                                                                goto endBreak;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                }
                                                switch (num2)
                                                {
                                                    case 1:
                                                        flag10 = true;
                                                        goto default;
                                                    case -2:
                                                        chrome.Status = StatusChromeAccount.ChromeClosed;
                                                        goto endQuit;
                                                    case -3:
                                                        chrome.Status = StatusChromeAccount.NoInternet;
                                                        goto endQuit;
                                                    case 2:
                                                        chrome.Status = StatusChromeAccount.Checkpoint;
                                                        SetInfoAccount(statusDataGridView, num3, ("Checkpoint"));
                                                        break;
                                                    case 3:
                                                        SetInfoAccount(statusDataGridView, num3, ("Account Novery"));
                                                        break;
                                                        //if (settings.GetValueBool("change_ckbVerify"))
                                                        //{
                                                        //    flag10 = true;
                                                        //}
                                                        //goto default;
                                                    default:
                                                        if (flag10)
                                                        {
                                                            break;
                                                        }
                                                        SetStatusAccount(num3, text5 + ("Đăng nhập thất bại!"));
                                                        ScreenCaptureError(chrome, text2, 1);
                                                        num = 1;
                                                        goto endQuit;
                                                }
                                                break;
                                        }
                                    }
                                    goto runThread;
                                }
                            endQuit:;
                            }
                            catch (Exception ex)
                            {
                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Lỗi không xác định!"));
                                Helpers.Common.ExportError(chrome, ex);
                            }
                            break;
                        }
                    endBreak:
                        break;
                }
                break;
            }
            switch (num)
            {
                case 1:
                    {
                        StatusChromeAccount status = chrome.Status;
                        StatusChromeAccount statusChromeAccount = status;
                        if (statusChromeAccount == StatusChromeAccount.ChromeClosed || statusChromeAccount == StatusChromeAccount.Checkpoint || statusChromeAccount == StatusChromeAccount.NoInternet)
                        {
                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + GetContentStatusChrome.GetContent(chrome.Status));
                        }
                        break;
                    }
                case 2:
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Không tìm thấy chrome!"));
                    break;
                case 3:
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Checkpoint!");
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cInfo", "Die");
                    CommonSQL.UpdateFieldToAccount(statusDataGridView, "info", "Die");
                    WriteFile(num3, 4);
                    break;
                case 4:
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Sai pass!"));
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cInfo", "Changed pass");
                    CommonSQL.UpdateFieldToAccount(statusDataGridView, "info", "Changed pass");
                    WriteFile(num3, 3);
                    break;
                default:
                    if (CommonChrome.IsCheckpoint(chrome))
                    {
                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + "Checkpoint!");
                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cInfo", "Die");
                        CommonSQL.UpdateFieldToAccount(statusDataGridView, "info", "Die");
                        WriteFile(num3, 4);
                    }
                    else if (!settings.GetValueBool("ckbAddMail") || num2 == 1)
                    {
                        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, num3, "cStatus", text5 + ("Cập nhật thông tin xong!"));
                    }
                    break;
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
        public static int ChangeNgaySinh(Chrome chrome, int ngay, int thang, int nam)
        {
            bool flag = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/editprofile.php?type=basic&edit=birthday");
                chrome.DelayTime(3.0);
                if (chrome.CheckExistElement("#root > div > form > table > tbody > tr:nth-child(2) > td > div", 5.0) != 1)
                {
                    chrome.Select(1, "day", ngay.ToString());
                    chrome.DelayTime(1.0);
                    chrome.Select(1, "month", thang.ToString());
                    chrome.DelayTime(0.5);
                    chrome.Select(1, "year", nam.ToString());
                    chrome.DelayTime(0.5);
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(2.0);
                    if (chrome.CheckExistElement("[name=\"birthday_confirmation\"]", 5.0) == 1)
                    {
                        chrome.Click(4, "[name=\"birthday_confirmation\"]");
                        chrome.DelayTime(1.0);
                    }
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(1.0);
                    if (chrome.CheckExistElement("[data-sigil=\"profile-card-header\"]", 10.0) == 1)
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangePTTH(Chrome chrome, string ptth)
        {
            bool flag = false;
            int num = 0;
            int num2 = 0;
            try
            {
                chrome.GotoURL("https://m.facebook.com/me/about");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("#timelineBody", 10.0) == 1)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('.darkTouch.l').length").ToString());
                }
                int num3 = chrome.GotoURLIfNotExist("https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=education&experience_type=2003");
                if (num3 == -2)
                {
                    return -2;
                }
                chrome.DelayTime(3.0);
                if (chrome.CheckExistElement("[data-sigil=\"edit-hs_school-text textinput\"]", 10.0) == 1)
                {
                    chrome.SendKeys(Base.rd, 4, "[data-sigil=\"edit-hs_school-text textinput\"]", ptth, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeyDown(4, "[data-sigil=\"edit-hs_school-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendEnter(4, "[data-sigil=\"edit-hs_school-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[name=\"graduated\"]");
                    chrome.DelayThaoTacNho();
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(2.0);
                    chrome.GotoURL("https://m.facebook.com/me/about");
                    chrome.DelayTime(2.0);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeDaiHoc(Chrome chrome, string daiHoc)
        {
            bool flag = false;
            int num = 0;
            int num2 = 0;
            try
            {
                chrome.GotoURLIfNotExist("https://m.facebook.com/me/about");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("#timelineBody", 10.0) == 1)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('.darkTouch.l').length").ToString());
                }
                int num3 = chrome.GotoURLIfNotExist("https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=education&experience_type=2004");
                if (num3 == -2)
                {
                    return -2;
                }
                chrome.DelayTime(3.0);
                if (chrome.CheckExistElement("[data-sigil=\"edit-college_school-text textinput\"]", 10.0) == 1)
                {
                    chrome.SendKeys(Base.rd, 4, "[data-sigil=\"edit-college_school-text textinput\"]", daiHoc, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeyDown(4, "[data-sigil=\"edit-college_school-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendEnter(4, "[data-sigil=\"edit-college_school-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[name=\"graduated\"]");
                    chrome.DelayThaoTacNho();
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(2.0);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeCity(Chrome chrome, string city)
        {
            bool flag = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=living");
                chrome.DelayTime(2.0);
                if (chrome.CheckExistElement("[data-sigil=\"edit-current_city-text textinput\"]", 10.0) == 1)
                {
                    chrome.ClearText(4, "[data-sigil=\"edit-current_city-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendKeys(Base.rd, 4, "[data-sigil=\"edit-current_city-text textinput\"]", city, 0.1);
                    chrome.DelayTime(1.0);
                    chrome.SendKeyDown(4, "[data-sigil=\"edit-current_city-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendEnter(4, "[data-sigil=\"edit-current_city-text textinput\"]");
                    chrome.DelayThaoTacNho(1);
                    chrome.Click(4, "[name=\"save\"]");
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeHomeTown(Chrome chrome, string homeTown)
        {
            bool flag = false;
            try
            {
                chrome.GotoURLIfNotExist("https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=living");
                chrome.DelayTime(2.0);
                if (chrome.CheckExistElement("[data-sigil=\"edit-hometown-text textinput\"]", 10.0) == 1)
                {
                    chrome.ClearText(4, "[data-sigil=\"edit-hometown-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendKeys(Base.rd, 4, "[data-sigil=\"edit-hometown-text textinput\"]", homeTown, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeyDown(4, "[data-sigil=\"edit-hometown-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendEnter(4, "[data-sigil=\"edit-hometown-text textinput\"]");
                    chrome.DelayThaoTacNho(1);
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(2.0);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeWork(Chrome chrome, string work)
        {
            bool flag = false;
            int num = 0;
            int num2 = 0;
            try
            {
                chrome.GotoURLIfNotExist("https://m.facebook.com/me/about");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("#timelineBody", 10.0) == 1)
                {
                    num = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('.darkTouch.l').length").ToString());
                }
                int num3 = chrome.GotoURLIfNotExist("https://m.facebook.com/profile/edit/infotab/section/forms/?life_event_surface=mtouch_profile&section=work&experience_type=2002");
                if (num3 == -2)
                {
                    return -2;
                }
                chrome.DelayTime(3.0);
                if (chrome.CheckExistElement("[data-sigil=\"edit-employer-text textinput\"]", 10.0) == 1)
                {
                    chrome.SendKeys(Base.rd, 4, "[data-sigil=\"edit-employer-text textinput\"]", work, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeyDown(4, "[data-sigil=\"edit-employer-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.SendEnter(4, "[data-sigil=\"edit-employer-text textinput\"]");
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[data-sigil=\"junk-text\"]");
                    chrome.DelayTime(0.5);
                    chrome.Click(4, "[data-sigil=\"edit-current touchable\"]");
                    chrome.DelayThaoTacNho();
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(2.0);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeTieuSu(Chrome chrome, string tieuSu, bool isPostWall = true)
        {
            bool flag = false;
            try
            {
                chrome.GotoURLIfNotExist("https://m.facebook.com/profile/intro/edit/public/?refid=17");
                if (chrome.CheckExistElement("#root>div>div>div>div>div>div:nth-child(5)>div> div:nth-child(2)", 10.0) == 1)
                {
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "#root>div>div>div>div>div>div:nth-child(5)>div> div:nth-child(2)");
                    if (chrome.CheckExistElement("[name=\"bio\"]", 10.0) == 1)
                    {
                        chrome.ClearText(4, "[name=\"bio\"]");
                        tieuSu = tieuSu.Replace("\r\n", "\\r\\n");
                        chrome.ExecuteScript("document.querySelector('[name=\"bio\"]').value=\"" + tieuSu + "\"");
                        if (isPostWall)
                        {
                            chrome.Click(4, "[name=\"publish_to_feed\"]");
                            chrome.DelayTime(1.0);
                        }
                        if (chrome.Click(4, "[data-sigil=\"touchable profile-intro-card-confirm-button\"]") != 1)
                        {
                            chrome.ExecuteScript("document.querySelector('[data-sigil=\"touchable profile-intro-card-confirm-button\"]').click()");
                        }
                        chrome.DelayTime(2.0);
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int UpCover(Chrome chrome, string pathImage, Random rd)
        {
            bool flag = false;
            try
            {
                chrome.DelayTime(3.0);
                chrome.GotoURL("https://mbasic.facebook.com/photos/upload/?cover_photo");
                chrome.DelayTime(3.0);
                chrome.SendKeys(2, "file1", pathImage, isClick: false);
                chrome.DelayTime(2.0);
                chrome.Click(3, "/html/body/div/div/div[2]/div/table/tbody/tr/td/div/form/div[2]/input");
                chrome.DelayTime(2.0);
                flag = true;
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int UpCoverNgheThuat(Chrome chrome, Random rd)
        {
            bool flag = false;
            try
            {
                chrome.GotoURLIfNotExist("https://mbasic.facebook.com/photos/change/cover_photo/?photo_type=artwork_photos");
                if (chrome.CheckExistElement("#root > table > tbody > tr > td > div > div > div > span > a", 10.0) == 1)
                {
                    int num = Convert.ToInt32(chrome.ExecuteScript("return document.querySelectorAll('#root > table > tbody > tr > td > div > div > div > span > a').length+''").ToString());
                    if (num > 0)
                    {
                        chrome.ExecuteScript("document.querySelectorAll('#root > table > tbody > tr > td > div > div > div > span > a')[" + rd.Next(0, num) + "].click()");
                        chrome.DelayTime(1.0);
                        if (chrome.CheckExistElement("#root > table > tbody > tr > td > div > div > div > div > form > div > input", 10.0) == 1)
                        {
                            chrome.Click(4, "#root > table > tbody > tr > td > div > div > div > div > form > div > input");
                        }
                        chrome.DelayTime(2.0);
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int UpAvatar(Chrome chrome, string pathImage, Random rd)
        {
            bool flag = false;
            try
            {
                chrome.DelayTime(3.0);
                chrome.GotoURL("https://mbasic.facebook.com/photos/upload/?profile_pic&upload_source=profile_pic_upload&profile_pic_source=tagged_photos_page");
                chrome.DelayTime(3.0);
                chrome.SendKeys(2, "file1", pathImage, isClick: false);
                chrome.DelayTime(2.0);
                chrome.Click(3, "/html/body/div/div/div[2]/div/table/tbody/tr/td/div/form/div[2]/input");
                chrome.DelayTime(2.0);
                flag = true;
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangeLanguage(Chrome chrome, string country_code)
        {
            bool flag = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/language.php");
                int tickCount = Environment.TickCount;
                string text = "";
                while (Environment.TickCount - tickCount < 10000)
                {
                    text = chrome.ExecuteScript("var x='';if(document.documentElement.innerHTML.includes('/a/language.php?l=" + country_code + "')) x=('https://m.facebook.com'+document.documentElement.innerHTML.match(new RegExp('/a/language.php\\\\?l=" + country_code + "(.*?)\"'))[0]).replace('\"','').split('amp;').join(''); else x=''; return x;").ToString();
                    if (text != "")
                    {
                        chrome.GotoURL(text);
                        flag = true;
                        break;
                    }
                    if (chrome.CheckExistElement("[value=\"" + country_code + "\"]") == 1 && chrome.Click(4, "[value=\"" + country_code + "\"]") == 1)
                    {
                        chrome.CheckExistElement("[href=\"/language.php\"]", 5.0);
                        flag = true;
                        break;
                    }
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        public int ChangePass(Chrome chrome, string old_pass, string new_pass)
        {
            int num = 0;
            try
            {
                if (!chrome.CheckIsLive())
                {
                    return -2;
                }
                chrome.GotoURLIfNotExist("https://m.facebook.com/settings/security/password/");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("[name=\"password_old\"]", 10.0) == 1)
                {
                    chrome.SendKeys(Base.rd, 4, "[name=\"password_old\"]", old_pass, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeys(Base.rd, 4, "[name=\"password_new\"]", new_pass, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.SendKeys(Base.rd, 4, "[name=\"password_confirm\"]", new_pass, 0.1);
                    chrome.DelayThaoTacNho();
                    chrome.Click(4, "[name=\"save\"]");
                    chrome.DelayTime(5.0);
                    num = 1;
                    int tickCount = Environment.TickCount;
                    while (true)
                    {
                        if (!chrome.CheckIsLive())
                        {
                            return -2;
                        }
                        if (Environment.TickCount - tickCount > 20000)
                        {
                            break;
                        }
                        if (chrome.CheckExistElement("[name=\"session_invalidation_options\"]") == 1)
                        {
                            chrome.DelayTime(1.0);
                            chrome.ExecuteScript("document.querySelectorAll(\"#m_check_list_aria_label > input\")[1].click()");
                            chrome.DelayTime(1.0);
                            chrome.Click(4, "[name=\"submit_action\"]");
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            if (num == 0)
            {
                Helpers.Common.ExportError(chrome, null, "Doi Pass Fail");
            }
            return num;
        }
        public int ChangePassUseLinkHacked(Chrome chrome, string old_pass, string new_pass)
        {
            int num = 0;
            try
            {
                if (!chrome.CheckIsLive())
                {
                    return -2;
                }
                chrome.GotoURLIfNotExist("https://m.facebook.com/hacked");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("[value=\"someone_accessed\"]", 10.0) == 1)
                {
                    chrome.DelayTime(1.0);
                    chrome.ExecuteScript("document.querySelector('[value=\"someone_accessed\"]').checked=true");
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[type=\"submit\"]");
                    chrome.DelayTime(1.0);
                    if (chrome.CheckExistElement("#checkpointButtonGetStarted-actual-button", 10.0) == 1)
                    {
                        chrome.DelayTime(1.0);
                        chrome.Click(4, "#checkpointButtonGetStarted-actual-button");
                        chrome.DelayTime(3.0);
                        if (chrome.CheckExistElement("#checkpointSubmitButton-actual-button", 60.0) == 1)
                        {
                            chrome.DelayTime(1.0);
                            int num2 = 0;
                            for (int i = 0; i < 10; i++)
                            {
                                if (chrome.CheckExistElement("[name=\"password_new\"]") == 1)
                                {
                                    if (num2 > 0)
                                    {
                                        num = 0;
                                        return num;
                                    }
                                    chrome.DelayTime(1.0);
                                    if (chrome.CheckExistElement("[name=\"password_old\"]") == 1)
                                    {
                                        chrome.SendKeys(2, "password_old", old_pass);
                                        chrome.DelayTime(2.0);
                                    }
                                    chrome.SendKeys(2, "password_new", new_pass);
                                    chrome.DelayTime(2.0);
                                    if (chrome.CheckExistElement("[name=\"password_confirm\"]") == 1)
                                    {
                                        chrome.SendKeys(2, "password_confirm", new_pass);
                                        chrome.DelayTime(2.0);
                                    }
                                    num2++;
                                    num = 1;
                                }
                                string text = chrome.CheckExistElementsv2(0.0, "#checkpointSubmitButton-actual-button", "#checkpointButtonContinue-actual-button");
                                if (text == "")
                                {
                                    if (chrome.GetURL().StartsWith(CommonChrome.GetWebsiteFacebook(chrome, 2) + "/home.php") || chrome.CheckExistElement("[href*=\"/friends/\"]") == 1)
                                    {
                                        return num;
                                    }
                                    continue;
                                }
                                if (text == "-2")
                                {
                                    num = -2;
                                    return num;
                                }
                                chrome.Click(4, text);
                                chrome.DelayTime(1.0);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            if (num == 0)
            {
                Helpers.Common.ExportError(chrome, null, "Doi Pass Fail");
            }
            return num;
        }
        public int ChangeGender(Chrome chrome, int type, string id)
        {
            bool flag = false;
            try
            {
                int num = chrome.GotoURLIfNotExist("https://m.facebook.com/profile/edit/infotab/section/forms/?section=basic-info");
                if (num == -2)
                {
                    return -2;
                }
                if (chrome.CheckExistElement("[data-sigil=\"gender-selector\"]", 30.0) == 1)
                {
                    chrome.ScrollSmooth("document.querySelector('[data-sigil=\"gender-selector\"]')");
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[data-sigil=\"option touchable\"]", type);
                    chrome.DelayTime(1.0);
                    chrome.ScrollSmooth("document.querySelector('[name=\"save\"]')");
                    chrome.DelayTime(1.0);
                    chrome.Click(2, "save");
                    chrome.DelayTime(1.0);
                    for (int i = 0; i < 30; i++)
                    {
                        if (chrome.CheckExistElement("#basic-info") == 1)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    string fieldValue = "";
                    switch (type)
                    {
                        case 0:
                            fieldValue = "female";
                            break;
                        case 1:
                            fieldValue = "male";
                            break;
                    }
                    CommonSQL.UpdateFieldToAccount(id, "gender", fieldValue);
                    flag = true;
                }
            }
            catch
            {
            }
            return flag ? 1 : 0;
        }
        private int RemovePhone(Chrome chrome, string pass)
        {
            int result = 0;
            try
            {
                chrome.GotoURL("https://m.facebook.com/settings/sms/");
                chrome.DelayTime(3.0);
                string cssSelector = chrome.GetCssSelector("a", "href", "/settings/sms/?remove");
                while (true)
                {
                    if (cssSelector != "")
                    {
                        int num = 0;
                        if (num >= 10)
                        {
                            break;
                        }
                        chrome.Click(4, cssSelector);
                        switch (chrome.CheckExistElements(10.0, "[name=\"remove_phone_warning_acknwoledged\"]", "[name=\"contact_point\"]", "[name=\"save_password\"]"))
                        {
                            case 1:
                                chrome.Click(2, "remove_phone_warning_acknwoledged");
                                chrome.DelayTime(1.0);
                                chrome.Click(4, "button");
                                chrome.DelayTime(3.0);
                                if (chrome.CheckExistElement("[name=\"save_password\"]") != 1)
                                {
                                    break;
                                }
                                chrome.SendKeys(2, "save_password", pass);
                                chrome.DelayTime(1.0);
                                chrome.Click(2, "save");
                                chrome.DelayTime(3.0);
                                try
                                {
                                    if (chrome.GetURL().Contains("remove_phone&phone_number"))
                                    {
                                        return 0;
                                    }
                                    if (Convert.ToBoolean(chrome.ExecuteScript("var x='1'; if(document.querySelector('[name=\"save_password\"]')!=null) x=document.querySelector('[name=\"save_password\"]').value; return (x=='')+''")))
                                    {
                                        return 2;
                                    }
                                }
                                catch
                                {
                                }
                                break;
                            case 2:
                                result = 1;
                                goto endProcess;
                            default:
                                chrome.SendKeys(2, "save_password", pass);
                                chrome.DelayTime(1.0);
                                chrome.Click(2, "save");
                                chrome.DelayTime(3.0);
                                try
                                {
                                    if (chrome.GetURL().Contains("remove_phone&phone_number"))
                                    {
                                        return 0;
                                    }
                                    if (Convert.ToBoolean(chrome.ExecuteScript("var x='1'; if(document.querySelector('[name=\"save_password\"]')!=null) x=document.querySelector('[name=\"save_password\"]').value; return (x=='')+''")))
                                    {
                                        return 2;
                                    }
                                }
                                catch
                                {
                                }
                                break;
                        }
                        chrome.GotoURL("https://m.facebook.com/settings/sms/");
                        chrome.DelayTime(1.0);
                        cssSelector = chrome.GetCssSelector("a", "href", "/settings/sms/?remove");
                        if (cssSelector == "")
                        {
                            result = 1;
                            break;
                        }
                        continue;
                    }
                    result = 1;
                    break;
                }
            endProcess:;
            }
            catch
            {
            }
            return result;
        }
        private bool Logout(Chrome chrome)
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
        private bool OpenGuardAvatart(Chrome chrome, string uid)
        {
            bool result = false;
            try
            {
                string cookie = chrome.GetCookieFromChrome();
                string useragent = chrome.GetUseragent();
                string proxy = chrome.Proxy;

                Meta request = new Meta(cookie, useragent, proxy);
                string tempRunOpenGuard = request.GraphMApi("&variables={\"input\":{\"is_shielded\":true,\"actor_id\":\"" + uid + "\",\"session_id\":\"{Var:ssid}\",\"client_mutation_id\":\"{Var:ssid}\"}}&doc_id=1477043292367183").Result;
                return true;
            }
            catch { }

            return result;
        }
        public string Create2FA(Chrome chrome, string pass)
        {
            string text = "";
            string text2 = "";
            try
            {
                int num = 0;
                while (true)
                {
                    chrome.GotoURL("https://m.facebook.com/security/2fac/setup/intro/");
                    chrome.DelayTime(1.0);
                    switch (chrome.CheckExistElements(20.0, "a[data-sigil=\"touchable\"]", "[name=\"pass\"]", "[data-testid=\"tfs_header_button\"]"))
                    {
                        case 0:
                            break;
                        case 3:
                            text = "0|" + ("Đã có 2FA rồi!");
                            break;
                        case 2:
                            chrome.DelayTime(1.0);
                            chrome.SendKeys(Base.rd, 4, "[name=\"pass\"]", pass, 0.1);
                            chrome.DelayThaoTacNho();
                            chrome.Click(4, "[name=\"save\"]");
                            if (chrome.CheckExistElement("[data-testid=\"tfs_header_button\"]", 10.0) == 1)
                            {
                                text = "0|" + ("Đã có 2FA rồi!");
                            }
                            break;
                        default:
                            {
                                string text3 = chrome.ExecuteScript("return document.documentElement.innerHTML.match(new RegExp('https://m.facebook.com/2fac/setup/qrcode/generate(.*?)\"'))[0].replace('\"','').split('amp;').join('');").ToString();
                                if (text3 == "")
                                {
                                    text3 = chrome.ExecuteScript("return document.documentElement.innerHTML.match(new RegExp('https://m.facebook.com/security/2fac/setup/qrcode/generate(.*?)\"'))[0].replace('\"','').split('amp;').join('');").ToString();
                                }
                                if (!(text3 != ""))
                                {
                                    break;
                                }
                                chrome.GotoURL(text3);
                                chrome.DelayTime(1.0);
                                int num2 = chrome.CheckExistElements(20.0, "[name=\"pass\"]", "[data-testid=\"key\"]", "#checkpointSubmitButton", "#captcha_response", "#checkpointBottomBar");
                                if (num2 != 1 && num2 != 2)
                                {
                                    break;
                                }
                                if (num2 == 1)
                                {
                                    chrome.DelayTime(1.0);
                                    chrome.SendKeys(Base.rd, 4, "[name=\"pass\"]", pass, 0.1);
                                    chrome.DelayThaoTacNho();
                                    chrome.Click(4, "[name=\"save\"]");
                                }
                                if (chrome.CheckExistElement("[data-testid=\"key\"]", 20.0) != 1)
                                {
                                    break;
                                }
                                text2 = chrome.ExecuteScript("return document.querySelector('[data-testid=\"key\"]').innerText").ToString().Replace(" ", "");
                                chrome.Click(4, "[name=\"confirmButton\"]");
                                if (chrome.CheckExistElement("[name=\"code\"]", 20.0) != 1)
                                {
                                    break;
                                }
                                chrome.DelayTime(1.0);
                                string totp = Helpers.Common.GetTotp(text2);
                                if (totp == "")
                                {
                                    num++;
                                    if (num == 1)
                                    {
                                        continue;
                                    }
                                }
                                if (totp != "")
                                {
                                    chrome.SendKeys(Base.rd, 4, "[name=\"code\"]", totp, 0.1);
                                    chrome.DelayThaoTacNho();
                                    chrome.Click(1, "submit_code_button");
                                    chrome.DelayTime(2.0);
                                }
                                else
                                {
                                    text = "0|" + ("Không tạo được mã 6 số!");
                                }
                                break;
                            }
                    }
                    break;
                }
            }
            catch
            {
            }
            if (text == "")
            {
                text = "1|" + text2;
            }
            return text;
        }
        public bool Remove2FA(Chrome chrome, string pass)
        {
            bool result = false;
            try
            {
                chrome.GotoURL("https://m.facebook.com/security/2fac/setup/turn_off/");
                chrome.DelayTime(1.0);
                if (chrome.CheckExistElement("[data-testid=\"tfa_setup_dialog_primary_button\"]", 20.0) == 1)
                {
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[data-testid=\"tfa_setup_dialog_primary_button\"]");
                    chrome.DelayTime(2.0);
                    switch (chrome.CheckExistElements(10.0, "[name=\"pass\"]", "a[data-sigil=\"touchable\"]", "#checkpointSubmitButton", "#captcha_response", "#checkpointBottomBar"))
                    {
                        case 1:
                            chrome.DelayTime(1.0);
                            chrome.SendKeys(Base.rd, 4, "[name=\"pass\"]", pass, 0.1);
                            chrome.DelayThaoTacNho();
                            chrome.Click(4, "[name=\"save\"]");
                            if (chrome.CheckExistElement("a[data-sigil=\"touchable\"]", 10.0) == 1)
                            {
                                result = true;
                            }
                            break;
                        case 2:
                            result = true;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            break;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        private int ChangeMail(Chrome chrome, string pass, int indexRow, string statusProxy, string id)
        {
            int result = 0;
            try
            {
                string text = "";
                string text2 = "";
                string imap = "";
                string text3 = "";
                string username = "";
                string text4 = "";
                int num = 0;
                if (settings.GetValueInt("themMail") == 1)
                {
                    if (settings.GetValueBool("ckbMailDvFb"))
                    {
                        //get mail dvfb //return 3; = het mail // text = mail
                        int buyCount = 0;
                        int buyErr = 3;
                        bool buyCountStatus = false;
                        while (buyCount < buyErr && !buyCountStatus)
                        {
                            // Mua mail mới
                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Đang mua mail mới {buyCount}/{buyErr}");
                            string mailnew = Helpers.CommonRequest.buyEmailDongVanFb(settings.GetValue("change_txtKeyDVFB") , settings.GetValue("cbbTypeDv"));
                            if (mailnew.StartsWith("success|"))
                            {
                                buyCountStatus = true;
                                // Lấy thông tin mail mới
                                string[] str = mailnew.Split('|');
                                text = str[1];
                                text2 = str[2];
                                string fullmailpass = text + "|" + text2;
                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Mua thành công: {fullmailpass}");
                            }
                            else
                            {
                                string[] str = mailnew.Split('|');
                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Lỗi: {str[1]}");
                                buyCount++;
                            }
                        }
                        if (buyCount >= buyErr && !buyCountStatus)
                        {
                            return 3;
                        }
                    }
                    if (settings.GetValueBool("ckbMailCua"))
                    {
                        int buyCount2 = 0;
                        int buyErr2 = 2;
                        bool buyCountStatus2 = false;
                        while (buyCount2 < buyErr2 && !buyCountStatus2)
                        {
                            // Mua mail mới
                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Đang lấy mail mới {buyCount2}/{buyErr2}");
                            string mailnew = Helpers.CommonRequest.getMailCuaApi();
                            if (mailnew.StartsWith("success|"))
                            {
                                buyCountStatus2 = true;
                                // Lấy thông tin mail mới
                                string[] str = mailnew.Split('|');
                                text = str[1];
                                text2 = str[2];
                                string fullmailpass = text + "|" + text2;
                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Lấy thành công: {fullmailpass}");
                            }
                            else
                            {
                                string[] str = mailnew.Split('|');
                                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + $"Lỗi: {str[1]}");
                                buyCount2++;
                            }
                        }
                        if (buyCount2 >= buyErr2 && !buyCountStatus2)
                        {
                            return 3;
                        }
                    }

                }
                else
                {
                    text = GetMailFromTempMail(chrome, "https://temp-mail.org/");
                    if (!(text != ""))
                    {
                        return 3;
                    }
                    text2 = "";
                }
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Đang thêm mail..."));
                for (int i = 0; i < 10; i++)
                {
                    chrome.GotoURL("https://www.facebook.com/settings?tab=account");
                    if (CommonChrome.CheckTypeWebFacebookFromUrl(chrome.GetURL()) == 1)
                    {
                        break;
                    }
                }
                chrome.ExecuteScript("function AddMail(email){var spinR = require([\"SiteData\"]).__spin_r; var spinB = require([\"SiteData\"]).__spin_b; var spinT = require([\"SiteData\"]).__spin_t; var jazoest = require([\"SprinkleConfig\"]).jazoest; var fbdtsg = require([\"DTSGInitData\"]).token; var userId = require([\"CurrentUserInitialData\"]).USER_ID; var hsi = require([\"SiteData\"]).hsi; var pass = \"\"; var url = \"https://www.facebook.com/add_contactpoint/dialog/submit/\"; var data = \"jazoest=22134&fb_dtsg=\" + fbdtsg + \"&next=&contactpoint=\" + email + \"&__user=\" + userId + \"&__a=1&__dyn=&__req=1&__be=1&__pc=PHASED%3ADEFAULT&dpr=1&__rev=&__s=&__hsi=\" + hsi + \"&__spin_r=\" + spinR + \"&__spin_b=\" + spinB + \"&__spin_t=\" + spinT; fetch(url, { method: 'POST', body: data, headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(e => e.text()).then(e => {});};AddMail(\"" + text + "\");");
                chrome.DelayTime(2.0);
                string text5 = CommonChrome.RequestGet(chrome, "https://m.facebook.com/ntdelegatescreen/?params=%7B%22entry-point%22%3A%22settings%22%7D&path=%2Fcontacts%2Fmanagement%2F", "https://m.facebook.com/");
                if (!text5.Contains(Helpers.Common.UrlEncode(text.ToLower())))
                {
                    return 0;
                }
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + "Đang lấy otp...");
                string text6 = "";
                // getOtpMailDVFB(text, text2, "facebook") EmailHelper.GetOtpFromMail2(0, username, text2, text, 30, "imap.yandex.com")
                //for (; ; )
                //{
                //    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Đang lấy mã từ hotmail..."));
                //    text6 = CuaGetMail.getOtpMail(text, text2, "security@facebookmail.com");
                //    bool checkmail = text6.ToString() != "";
                //    if (checkmail)
                //    {
                //        break;
                //    }
                //    if (num > 2)
                //    {
                //        DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Fb không gửi otp về mail!"));
                //        lock (k)
                //        {
                //            lstMailLoi.Add(text3);
                //        }
                //        return 5;
                //    }
                //    num++;
                //}

                text6 = (settings.GetValueBool("ckbMailDvFb") ? EmailHelper.GetOtpFromMail(0, text, text2, 30, imap) : EmailHelper.GetOtpFromMail(0, text, text2, 30, imap));
                if (text6.Trim() == "")
                {
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Fb không gửi otp về mail!"));
                    lock (k)
                    {
                        lstMailLoi.Add(text3);
                    }
                    return 5;
                }

                for (int j = 0; j < 3; j++)
                {
                    chrome.GotoURL("https://www.facebook.com/confirmcontact.php?c=" + text6);
                    chrome.DelayTime(2.0);
                    if (chrome.CheckExistElement("[href=\"/help/?ref=404\"]") != 1)
                    {
                        break;
                    }
                }
                if (chrome.CheckExistElement("[name=\"pass\"]") != 1)
                {
                    goto hideMailanDeleteMailOld;
                }
                if (pass != "")
                {
                    chrome.SendKeys(Base.rd, 4, "[name=\"pass\"]", pass, 0.1);
                    chrome.DelayTime(1.0);
                    chrome.Click(4, "[data-testid=\"sec_ac_button\"]");
                    chrome.DelayTime(3.0);
                    try
                    {
                        if (Convert.ToBoolean(chrome.ExecuteScript("(document.querySelector('[name=\"pass\"]').value=='')+''")))
                        {
                            return 2;
                        }
                    }
                    catch
                    {
                    }
                    goto hideMailanDeleteMailOld;
                }
                result = 4;
                goto endProcess;
            hideMailanDeleteMailOld:
                result = 1;
                bool e1 = CommonSQL.UpdateFieldToAccount(id, "email", text);
                bool pe1 = CommonSQL.UpdateFieldToAccount(id, "passmail", text2);
                if (settings.GetValueBool("ckbAnMailMoi"))
                {
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Đang ẩn mail..."));
                    chrome.GotoURL("https://m.facebook.com/settings/email/");
                    chrome.DelayTime(3.0);
                    chrome.ExecuteScript("document.querySelector('#root>div:nth-child(1)>div>div>div.acw.apl>div>div:nth-child(1)>div>div>div:nth-child(2)> a').click()");
                    chrome.DelayTime(2.0);
                    chrome.ExecuteScript("document.getElementsByName('px')[2].click()");
                    chrome.DelayTime(1.0);
                }
                if (settings.GetValueBool("ckbXoaMailCu"))
                {
                    DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Đang xoá mail cũ..."));
                    chrome.DelayTime(2.0);
                    switch (RemoveMail(chrome, pass))
                    {
                        case 0:
                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + "Xóa mail thất bại!");
                            break;
                        case 1:
                            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + "Xóa mail thành công!");
                            break;
                        case 2:
                            result = 4;
                            break;
                    }
                }
            endProcess:;
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(ex, "Add Mail");
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", statusProxy + ("Add mail thất bại!"));
            }
            return result;
        }
        private int RemoveMail(Chrome chrome, string pass, string emailMoiAdd = "")
        {
            int result = 0;
            try
            {
                int num = 0;
                while (num < 2)
                {
                    if (!chrome.GetURL().StartsWith("https://m.facebook.com/settings/email/"))
                    {
                        chrome.GotoURL("https://m.facebook.com/settings/email/");
                        chrome.DelayTime(3.0);
                    }
                    string input = chrome.ExecuteScript("return document.documentElement.innerHTML").ToString();
                    MatchCollection matchCollection = Regex.Matches(input, "/settings/email/\\?remove_email&(.*?)\"");
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 < matchCollection.Count)
                        {
                            string text = matchCollection[num2].Value.Replace("amp;", "").Replace("\"", "");
                            string value = Regex.Match(text, "email=(.*?)&").Groups[1].Value;
                            value = Helpers.Common.UrlDecode(value);
                            if (value != emailMoiAdd && chrome.ExecuteScript("return (document.documentElement.innerText.match(new RegExp('" + value + "', 'g')) || []).length+''").ToString() != "2")
                            {
                                chrome.GotoURL("https://m.facebook.com" + text);
                                if (chrome.CheckExistElement("[name=\"save\"]", 10.0) == 1)
                                {
                                    chrome.DelayTime(3.0);
                                    while (true)
                                    {
                                        int num3 = chrome.CheckExistElements(0.0, "[name=\"pass\"]", "[name=\"save_password\"]", "[href=\"https://www.facebook.com/help/177066345680802\"]");
                                        if (num3 != 0)
                                        {
                                            switch (num3)
                                            {
                                                case 1:
                                                    chrome.SendKeys(2, "pass", pass);
                                                    chrome.DelayTime(1.0);
                                                    chrome.Click(2, "save");
                                                    chrome.DelayTime(3.0);
                                                    try
                                                    {
                                                        if (Convert.ToBoolean(chrome.ExecuteScript("(document.querySelector('[name=\"pass\"]').value=='')+''")))
                                                        {
                                                            result = 2;
                                                            return result;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    if (chrome.CheckExistElement("[href=\"https://www.facebook.com/help/177066345680802\"]") != 1)
                                                    {
                                                        break;
                                                    }
                                                    result = 3;
                                                    goto endProcess;
                                                case 2:
                                                    chrome.SendKeys(2, "save_password", pass);
                                                    chrome.DelayTime(1.0);
                                                    chrome.Click(2, "save");
                                                    chrome.DelayTime(3.0);
                                                    try
                                                    {
                                                        if (Convert.ToBoolean(chrome.ExecuteScript("(document.querySelector('[name=\"save_password\"]').value=='')+''")))
                                                        {
                                                            result = 2;
                                                            return result;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    if (chrome.CheckExistElement("[href=\"https://www.facebook.com/help/177066345680802\"]") != 1)
                                                    {
                                                        break;
                                                    }
                                                    result = 3;
                                                    goto endProcess;
                                                case 3:
                                                    result = 3;
                                                    goto endProcess;
                                            }
                                            break;
                                        }
                                        if (chrome.CheckExistElement("[name=\"save\"]") != 1)
                                        {
                                            break;
                                        }
                                        chrome.Click(2, "save");
                                        chrome.DelayTime(3.0);
                                    }
                                }
                            }
                            num2++;
                            continue;
                        }
                        num++;
                        break;
                    }
                }
            endProcess:;
            }
            catch
            {
            }
            return result;
        }
        private string getOtpMailDVFB(string email, string password, string type)
        {
            string otp = "";
            try
            {
                int getOtpCount = 0;
                int getOtpErr = 4;
                bool getOtpStatus = false;

                while (getOtpCount < getOtpErr && !getOtpStatus)
                {
                    string getOtp = Helpers.CommonRequest.getOtpDongVanFb(email, password, type);
                    if (getOtp.StartsWith("success|"))
                    {
                        string[] str2 = getOtp.Split('|');
                        otp = str2[1];
                        getOtpStatus = true;

                    }
                    else
                    {
                        string[] str2 = getOtp.Split('|');
                        getOtpCount++;
                    }

                }
                if (getOtpCount >= getOtpErr && !getOtpStatus)
                {
                    otp = "";
                }
            }catch {
            
            }
            return otp;
        }
        private string GeneratorEmail(string domain)
        {
            string result = "";
            try
            {
                result = CommonCSharp.CreateRandomString(10) + CommonCSharp.CreateRandomNumber(4) + "@" + domain;
            }
            catch
            {
            }
            return result;
        }
        private string GetMailFromTempMail(Chrome chrome, string url, int timeOut = 30)
        {
            string text = "";
            try
            {
                for (int i = 0; i < timeOut; i++)
                {
                    if (!chrome.GetURL().Contains(url))
                    {
                        chrome.GotoURL(url);
                    }
                    string[] array = chrome.GetCookieFromChrome(Regex.Match(url, "https://(.*?)\\.").Groups[1].Value).Split(';');
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (array[j].Split('=')[0] == "email")
                        {
                            text = WebUtility.UrlDecode(array[j].Split('=')[1]);
                            break;
                        }
                    }
                    if (text != "")
                    {
                        break;
                    }
                    Helpers.Common.DelayTime(1.0);
                }
            }
            catch
            {
            }
            return text;
        }
        private void LoadStatusDatagridView(int row, string status)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, row, "cStatus", status);
        }
        public void SetStatusAccount(int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cStatus", value);
        }
        public void SetInfoAccount(string id, int indexRow, string value)
        {
            DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, "cInfo", value);
            CommonSQL.UpdateFieldToAccount(id, "info", value);
        }
        private void rControl(string dt)
        {
            try
            {
                if (dt == "start")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        btnPause.Enabled = true;
                        btnStart.Enabled = false;
                    });
                }
                else if (dt == "stop")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        btnPause.Text = "Tạm dừng";
                        btnPause.Enabled = false;
                        btnStart.Enabled = true;
                    });
                }
            }
            catch
            {
            }
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fChangeInfoConfig());
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
                    string contents = chrome.ExecuteScript("var markup = document.documentElement.innerHTML;return markup;").ToString();
                    File.WriteAllText(text + "\\" + uid + ".html", contents);
                }
            }
            catch
            {
            }
        }
        private void WriteFile(int row, int type)
        {
            string text = "";
            string text2 = "";
            try
            {
                text = text + DatagridviewHelper.GetStatusDataGridView(dtgvAcc, row, "cUid") + "|" + DatagridviewHelper.GetStatusDataGridView(dtgvAcc, row, "cPassword") + "|" + DatagridviewHelper.GetStatusDataGridView(dtgvAcc, row, "cFa2") + "|" + DatagridviewHelper.GetStatusDataGridView(dtgvAcc, row, "cCookies") + "\r\n";
                switch (type)
                {
                    case 1:
                        text2 = "DoiPassThatBai.txt";
                        break;
                    case 2:
                        text2 = "ThemMailThatBai.txt";
                        break;
                    case 3:
                        text2 = "SaiPass.txt";
                        break;
                    case 4:
                        text2 = "Checkpoint.txt";
                        break;
                    case 5:
                        text2 = "Loi.txt";
                        break;
                }
                lock (lock1)
                {
                    File.AppendAllText("configschange\\fail\\" + text2, text);
                }
            }
            catch
            {
            }
        }

        private void fChangeInfo_Load(object sender, EventArgs e)
        {
            UpdateSelectCountRecord();
        }

        private void bỏChọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("UnAll");
        }
        public void SetCellAccount(int indexRow, string column, object value, bool isAllowEmptyValue = true)
        {
            if (isAllowEmptyValue || !(value.ToString().Trim() == ""))
            {
                DatagridviewHelper.SetStatusDataGridView(dtgvAcc, indexRow, column, value);
            }
        }
        public string GetCellAccount(int indexRow, string column)
        {
            return DatagridviewHelper.GetStatusDataGridView(dtgvAcc, indexRow, column);
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
                        UpdateSelectCountRecord();
                        break;
                    }
                case "UnAll":
                    {
                        for (int j = 0; j < dtgvAcc.RowCount; j++)
                        {
                            SetCellAccount(j, "cChose", false);
                        }
                        UpdateSelectCountRecord();
                        break;
                    }
                case "SelectHighline":
                    {
                        DataGridViewSelectedRowCollection selectedRows = dtgvAcc.SelectedRows;
                        for (int l = 0; l < selectedRows.Count; l++)
                        {
                            SetCellAccount(selectedRows[l].Index, "cChose", true);
                        }
                        UpdateSelectCountRecord();
                        break;
                    }
                case "ToggleCheck":
                    {
                        for (int i = 0; i < dtgvAcc.SelectedRows.Count; i++)
                        {
                            int index = dtgvAcc.SelectedRows[i].Index;
                            SetCellAccount(index, "cChose", !Convert.ToBoolean(GetCellAccount(index, "cChose")));
                        }
                        UpdateSelectCountRecord();
                        break;
                    }
            }
        }

        private void chọnTấtCảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoseRowInDatagrid("All");
        }

        private void chọnBôiĐenToolStripMenuItem_Click(object sender, EventArgs e)
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
            tìnhTrạngToolStripMenuItem.DropDownItems.Clear();
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
                tìnhTrạngToolStripMenuItem.DropDownItems.Add(list[m]);
                tìnhTrạngToolStripMenuItem.DropDownItems[m].Click += SelectGridByInfo;
            }
        }
        private void SelectGridByInfo(object sender, EventArgs e)
        {
            ChooseAccountByValue("cInfo", (sender as ToolStripMenuItem).Text);
        }
        private void ChooseAccountByValue(string column, string value)
        {
            for (int i = 0; i < dtgvAcc.RowCount; i++)
            {
                dtgvAcc.Rows[i].Cells["cChose"].Value = GetCellAccount(i, column).Contains(value);
            }
            UpdateSelectCountRecord();
        }
        private void SelectGridByStatus(object sender, EventArgs e)
        {
            ChooseAccountByValue("cStatus", (sender as ToolStripMenuItem).Text);
        }
    }
}

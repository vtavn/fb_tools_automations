using AutoUpdaterDotNET;
using Facebook_Tool_Request.Common;
using Facebook_Tool_Request.Helpers;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fCaiDatChung : Form
    {
        public fCaiDatChung()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configGeneral", false);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }
        private void LoadCbbLocation()
        {
            Dictionary<string, string> dataSource = this.ShopLikePrxGetListLocation();
            this.cbbLocationShopLikePrx.DataSource = new BindingSource(dataSource, null);
            this.cbbLocationShopLikePrx.ValueMember = "Key";
            this.cbbLocationShopLikePrx.DisplayMember = "Value";

            Dictionary<string, string> dataSource2 = this.TinsoftGetListLocation();
            this.cbbLocationTinsoft.DataSource = new BindingSource(dataSource2, null);
            this.cbbLocationTinsoft.ValueMember = "Key";
            this.cbbLocationTinsoft.DisplayMember = "Value";
        }

        public Dictionary<string, string> ShopLikePrxGetListLocation()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryShoplikePrx = SetupFolder.GetListCountryShopLikePrx();
            for (int i = 0; i < listCountryShoplikePrx.Count; i++)
            {
                string[] array = listCountryShoplikePrx[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            return dictionary;
        }
        private void LoadCbbSizeChrome()
        {
            JSON_Settings json_Settings = new JSON_Settings("configChrome", false);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("default", "Mặc định");
            bool flag = json_Settings.GetValue("sizeChrome", "") != "";
            if (flag)
            {
                Dictionary<string, object> dictionary2 = JSON_Settings.ConvertToDictionary(JObject.Parse(json_Settings.GetValue("sizeChrome", "")));
                foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
                {
                    dictionary.Add(keyValuePair.Value.ToString(), keyValuePair.Key + string.Format(" ({0})", keyValuePair.Value));
                }
            }
            this.cbbSizeChrome.DataSource = new BindingSource(dictionary, null);
            this.cbbSizeChrome.ValueMember = "Key";
            this.cbbSizeChrome.DisplayMember = "Value";
        }

        private void fCaiDatChung_Load(object sender, EventArgs e)
        {
            this.LoadCbbLocation();
            this.LoadCbbSizeChrome();
            this.nudInteractThread.Value = this.settings.GetValueInt("nudInteractThread", 3);
            this.nudHideThread.Value = this.settings.GetValueInt("nudHideThread", 5);
            this.txtScaleChorme.Text = this.settings.GetValue("txtScaleChorme", "0.7");
            this.txbPathProfile.Text = ((this.settings.GetValue("txbPathProfile", "") == "") ? (Application.StartupPath + "\\profiles") : this.settings.GetValue("txbPathProfile", ""));
            bool flag = !Directory.Exists(this.txbPathProfile.Text) || this.txbPathProfile.Text.Trim() == "profiles";
            if (flag)
            {
                this.txbPathProfile.Text = Application.StartupPath + "\\profiles";
            }
            this.settings.Update("txbPathProfile", this.txbPathProfile.Text);
            this.ckbShowImageInteract.Checked = Convert.ToBoolean((this.settings.GetValue("ckbShowImageInteract", "") == "") ? "false" : this.settings.GetValue("ckbShowImageInteract", ""));
            this.ckbAddChromeIntoForm.Checked = this.settings.GetValueBool("ckbAddChromeIntoForm", false);
            this.ckbThunhoPage.Checked = this.settings.GetValueBool("ckbThunhoPage", true);
            this.nudDelayOpenChromeFrom.Value = this.settings.GetValueInt("nudDelayOpenChromeFrom", 1);
            this.nudDelayOpenChromeTo.Value = this.settings.GetValueInt("nudDelayOpenChromeTo", 1);
            this.nudDelayCloseChromeFrom.Value = this.settings.GetValueInt("nudDelayCloseChromeFrom", 0);
            this.nudDelayCloseChromeTo.Value = this.settings.GetValueInt("nudDelayCloseChromeTo", 0);
            this.nudWidthChrome.Value = this.settings.GetValueInt("nudWidthChrome", 450);
            this.nudHeighChrome.Value = this.settings.GetValueInt("nudHeighChrome", 350);
            this.cbbColumnChrome.Text = ((this.settings.GetValue("cbbColumnChrome", "") == "") ? "3" : this.settings.GetValue("cbbColumnChrome", ""));
            this.cbbRowChrome.Text = ((this.settings.GetValue("cbbRowChrome", "") == "") ? "2" : this.settings.GetValue("cbbRowChrome", ""));
            int valueInt = this.settings.GetValueInt("typeBrowser", 0);
            int num = valueInt;
            int num2 = num;
            if (num2 != 0)
            {
                if (num2 == 1)
                {
                    this.rbChromium.Checked = true;
                }
            }
            else
            {
                this.rbChrome.Checked = true;
            }
            this.txtLinkToOtherBrowser.Text = this.settings.GetValue("txtLinkToOtherBrowser", "");
            this.ckbUsePortable.Checked = this.settings.GetValueBool("ckbUsePortable", false);
            this.txtPathToPortableZip.Text = this.settings.GetValue("txtPathToPortableZip", "");
            this.cbbSizeChrome.SelectedValue = this.settings.GetValue("sizeChrome", "default").ToString();
            bool flag2 = this.cbbSizeChrome.SelectedValue == null;
            if (flag2)
            {
                this.cbbSizeChrome.SelectedValue = "default";
            }
            switch (this.settings.GetValueInt("tocDoGoVanBan", 0))
            {
                case 0:
                    this.rbTocDoGoCham.Checked = true;
                    break;
                case 1:
                    this.rbTocDoGoBinhThuong.Checked = true;
                    break;
                case 2:
                    this.rbTocDoGoNhanh.Checked = true;
                    break;
            }
            this.ckbKhongCheckIP.Checked = this.settings.GetValueBool("ckbKhongCheckIP", false);
            this.nudChangeIpCount.Value = this.settings.GetValueInt("ip_nudChangeIpCount", 1);
            this.nudDelayCheckIP.Value = this.settings.GetValueInt("nudDelayCheckIP", 0);

            int valueInt2 = this.settings.GetValueInt("ip_iTypeChangeIp", 0);
            bool typeChangeProxy = valueInt2 == 0;
            if (typeChangeProxy)
            {
                this.rbNone.Checked = true;
            }
            else
            {
                bool rbSL = valueInt2 == 1;
                if (rbSL)
                {
                    this.rbShopLike.Checked = true;
                }
                else
                {
                    bool rbTs = valueInt2 == 2;
                    if (rbTs)
                    {
                        this.rbTinsoft.Checked = true;
                    }
                    else
                    {
                        bool rbMin = valueInt2 == 3;
                        if (rbMin)
                        {
                            this.rbminproxy.Checked = true;
                        }
                    }
                }
            }

            this.txtApiShopLike.Text = this.settings.GetValue("txtApiShopLike", "");
            this.nudLuongPerIPShopLike.Value = this.settings.GetValueInt("nudLuongPerIPShopLike", 1);
            this.cbbLocationShopLikePrx.SelectedValue = (this.settings.GetValue("cbbLocationShopLikePrx", "") == "") ? "0" : this.settings.GetValue("cbbLocationShopLikePrx", "");

            this.txtApiMinproxy.Text = this.settings.GetValue("txtApiMinproxy", "");
            this.nudLuongPerIPMinProxy.Value = this.settings.GetValueInt("nudLuongPerIPMinProxy", 1);

            bool typeApiTs = this.settings.GetValueInt("typeApiTinsoft", 0) == 0;
            if (typeApiTs)
            {
                this.rbApiUser.Checked = true;
            }
            else
            {
                this.rbApiProxy.Checked = true;
            }
            this.txtApiUser.Text = this.settings.GetValue("txtApiUser", "");
            this.txtApiProxy.Text = this.settings.GetValue("txtApiProxy", "");
            this.cbbLocationTinsoft.SelectedValue = ((this.settings.GetValue("cbbLocationTinsoft", "") == "") ? "0" : this.settings.GetValue("cbbLocationTinsoft", ""));
            this.nudLuongPerIPTinsoft.Value = this.settings.GetValueInt("nudLuongPerIPTinsoft", 0);
            this.ckbWaitDoneAllTinsoft.Checked = this.settings.GetValueBool("ckbWaitDoneAllTinsoft", false);

            bool flag20 = this.settings.GetValueInt("typePhanBietMau", 0) == 0;
            if (flag20)
            {
                this.rbPhanBietMauNen.Checked = true;
            }
            else
            {
                this.rbPhanBietMauChu.Checked = true;
            }

            this.CheckedChangedFull();
        }

        private JSON_Settings settings;

        private void CheckedChangedFull()
        {
            this.ckbAddChromeIntoForm_CheckedChanged(null, null);
            this.rbShopLike_CheckedChanged(null, null);
            this.rbTinsoft_CheckedChanged(null, null);
        }

        private void rbShopLike_CheckedChanged(object sender, EventArgs e)
        {
            this.plShopLike.Enabled = this.rbShopLike.Checked;
            this.plShopLike.Visible = this.rbShopLike.Checked;
        }

        private void txtApiShopLike_TextChanged(object sender, EventArgs e)
        {
            List<string> list = this.txtApiShopLike.Lines.ToList<string>();
            list = Helpers.Common.RemoveEmptyItems(list);
            this.label47.Text = string.Format(Language.GetValue("Nhập API KEY ({0}):"), list.Count.ToString());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.settings.Update("nudInteractThread", this.nudInteractThread.Value);
                this.settings.Update("nudHideThread", this.nudHideThread.Value);
                this.settings.Update("txbPathProfile", this.txbPathProfile.Text);
                this.settings.Update("ckbShowImageInteract", this.ckbShowImageInteract.Checked);
                this.settings.Update("ckbAddChromeIntoForm", this.ckbAddChromeIntoForm.Checked);
                this.settings.Update("ckbThunhoPage", this.ckbThunhoPage.Checked);
                this.settings.Update("nudWidthChrome", this.nudWidthChrome.Value);
                this.settings.Update("nudHeighChrome", this.nudHeighChrome.Value);
                this.settings.Update("nudDelayOpenChromeFrom", this.nudDelayOpenChromeFrom.Value);
                this.settings.Update("nudDelayOpenChromeTo", this.nudDelayOpenChromeTo.Value);
                this.settings.Update("nudDelayCloseChromeFrom", this.nudDelayCloseChromeFrom.Value);
                this.settings.Update("nudDelayCloseChromeTo", this.nudDelayCloseChromeTo.Value);
                this.settings.Update("cbbColumnChrome", this.cbbColumnChrome.Text);
                this.settings.Update("cbbRowChrome", this.cbbRowChrome.Text);
                this.settings.Update("txtScaleChorme", this.txtScaleChorme.Text);

                int num = 0;
                
                this.settings.Update("typeBrowser", num);
                this.settings.Update("txtLinkToOtherBrowser", this.txtLinkToOtherBrowser.Text.Trim());
                this.settings.Update("ckbUsePortable", this.ckbUsePortable.Checked);
                this.settings.Update("txtPathToPortableZip", this.txtPathToPortableZip.Text.Trim());
                bool flag = num != 0 && this.txtLinkToOtherBrowser.Text.Trim() == "";
                if (flag)
                {
                    string arg = "";
                    switch (num)
                    {
                        case 1:
                            arg = "Chromium";
                            break;
                        case 2:
                            arg = "Cốc cốc";
                            break;
                        case 3:
                            arg = "Slimjet";
                            break;
                    }
                    MessageBoxHelper.ShowMessageBox(string.Format(Language.GetValue("Vui lòng nhập đường dẫn đến file chạy của trình duyệt {0}!"), arg), 2);
                }
                else
                {
                    bool flag2 = this.cbbSizeChrome.Items.Count > 0;
                    if (flag2)
                    {
                        bool flag3 = this.cbbSizeChrome.SelectedValue.ToString() != "";
                        if (flag3)
                        {
                            this.settings.Update("sizeChrome", this.cbbSizeChrome.SelectedValue.ToString());
                        }
                    }
                    bool checked2 = this.rbTocDoGoCham.Checked;
                    if (checked2)
                    {
                        this.settings.Update("tocDoGoVanBan", 0);
                    }
                    else
                    {
                        bool checked3 = this.rbTocDoGoBinhThuong.Checked;
                        if (checked3)
                        {
                            this.settings.Update("tocDoGoVanBan", 1);
                        }
                        else
                        {
                            this.settings.Update("tocDoGoVanBan", 2);
                        }
                    }
                    this.settings.Update("ckbKhongCheckIP", this.ckbKhongCheckIP.Checked);
                    this.settings.Update("ip_nudChangeIpCount", this.nudChangeIpCount.Value);
                    this.settings.Update("nudDelayCheckIP", this.nudDelayCheckIP.Value);
                    this.settings.Update("nudLuongPerIPShopLike", this.nudLuongPerIPShopLike.Value);

                    //proxy shoplike only
                    int num2 = 0;
                    bool checkedNone = this.rbNone.Checked;
                    if (checkedNone)
                    {
                        num2 = 0;
                    }
                    else
                    {
                        bool rbShoplike = this.rbShopLike.Checked;
                        if (rbShoplike)
                        {
                            num2 = 1; //12
                        }
                        else
                        {
                            bool rbTinsoft = this.rbTinsoft.Checked;
                            if (rbTinsoft)
                            {
                                num2 = 2;
                            }
                            else
                            {
                                bool rbminproxy = this.rbminproxy.Checked;
                                if (rbminproxy)
                                {
                                    num2 = 3;
                                }
                            }
                        }
                    }
                    this.settings.Update("ip_iTypeChangeIp", num2);
                    this.settings.Update("txtApiShopLike", this.txtApiShopLike.Text);
                    this.settings.Update("cbbLocationShopLikePrx", this.cbbLocationShopLikePrx.SelectedValue);

                    //min
                    this.settings.Update("txtApiMinproxy", this.txtApiMinproxy.Text);
                    this.settings.Update("nudLuongPerIPMinProxy", this.nudLuongPerIPMinProxy.Value);

                    //tinsoft
                    bool rbApiUser = this.rbApiUser.Checked;
                    if (rbApiUser)
                    {
                        this.settings.Update("typeApiTinsoft", 0);
                    }
                    else
                    {
                        this.settings.Update("typeApiTinsoft", 1);
                    }
                    this.settings.Update("txtApiUser", this.txtApiUser.Text);
                    this.settings.Update("txtApiProxy", this.txtApiProxy.Text);
                    this.settings.Update("cbbLocationTinsoft", this.cbbLocationTinsoft.SelectedValue);
                    this.settings.Update("nudLuongPerIPTinsoft", this.nudLuongPerIPTinsoft.Value);
                    this.settings.Update("ckbWaitDoneAllTinsoft", this.ckbWaitDoneAllTinsoft.Checked);

                    
                    bool checked21 = this.rbPhanBietMauNen.Checked;
                    if (checked21)
                    {
                        this.settings.Update("typePhanBietMau", 0);
                    }
                    else
                    {
                        this.settings.Update("typePhanBietMau", 1);
                    }

                    //done
                    this.settings.Save("");
                    bool flag4 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Lưu thành công, bạn có muốn đóng cửa sổ?")) == DialogResult.Yes;
                    if (flag4)
                    {
                        base.Close();
                    }
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox("Lỗi!", 1);
            }
        }

        private void txbPathProfile_Click(object sender, EventArgs e)
        {
            bool flag = (e as MouseEventArgs).Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control;
            if (flag)
            {
                Process.Start(this.txbPathProfile.Text.Trim());
            }
        }

        private void ckbKhongCheckIP_CheckedChanged(object sender, EventArgs e)
        {
            bool @checked = this.rbNone.Checked;
            if (@checked)
            {
                this.ckbKhongCheckIP.Checked = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.ShowMessageBox(Language.GetValue("Đang phát triển :)"), 3);
            //bool flag = this.settings.GetValueInt("ip_iTypeChangeIp", 0) == 0;
            //if (flag)
            //{
            //    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn loại đổi IP"), 3);
            //}
            //else
            //{
            //    bool flag2 = Helpers.Common.ChangeIP(this.settings.GetValueInt("ip_iTypeChangeIp", 0), 0, "", "", 0, "");
            //    if (flag2)
            //    {
            //        MessageBoxHelper.ShowMessageBox(Language.GetValue("Đổi IP thành công!"), 1);
            //    }
            //    else
            //    {
            //        MessageBoxHelper.ShowMessageBox(Language.GetValue("Đổi IP thất bại!"), 2);
            //    }
            //}
        }

        private void ckbAddChromeIntoForm_CheckedChanged(object sender, EventArgs e)
        {
            this.plAddChromeVaoFormView.Enabled = this.ckbAddChromeIntoForm.Checked;
            this.plSapXepCuaSoChrome.Enabled = !this.ckbAddChromeIntoForm.Checked;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = MessageBoxHelper.ShowYesNo("Bạn có muốn cập nhật ChromeDriver không?") != DialogResult.Yes;
            if (!flag)
            {
                try
                {
                    Facebook_Tool_Request.Helpers.Common.KillProcess("chromedriver");
                    string text = Facebook_Tool_Request.Helpers.Common.GetVersionChromeEXE();
                    bool flag2 = !(text == "");
                    if (flag2)
                    {
                        string text2 = "";
                        string input = new RequestXNet("", "", "", 0).RequestGet("https://api.nuget.org/v3-flatcontainer/Selenium.WebDriver.ChromeDriver/index.json");
                        IEnumerator object_ = Regex.Matches(input, "\"(\\d+\\.0\\.\\d+\\.\\d+)\"").GetEnumerator();
                        try
                        {
                            while (Facebook_Tool_Request.Helpers.Utils.smethod_014(object_))
                            {
                                Match match = (Match)Facebook_Tool_Request.Helpers.Utils.DB2856BD(object_);
                                bool flag3 = match.Groups[1].Value == text || Facebook_Tool_Request.Helpers.Utils.B5A9BB12(match.Groups[1].Value, (string)((object[])Facebook_Tool_Request.Helpers.Utils.smethod_01(text, new char[]
                                {
                                    '.'
                                }))[0]);
                                if (flag3)
                                {
                                    text2 = match.Groups[1].Value;
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = object_ as IDisposable;
                            bool flag4 = disposable != null;
                            if (flag4)
                            {
                                disposable.Dispose();
                            }
                        }
                        bool flag5 = !(text2 == "");
                        if (flag5)
                        {
                            Facebook_Tool_Request.Helpers.Common.DeleteFile(FileHelper.GetPathToCurrentFolder() + "\\chromedriver.exe");
                            string text3 = Facebook_Tool_Request.Helpers.Utils.E097A431(new string[]
                            {
                                "https://api.nuget.org/v3-flatcontainer/Selenium.WebDriver.ChromeDriver/",
                                text2,
                                "/selenium.webdriver.chromedriver.",
                                text2,
                                ".nupkg"
                            });
                            Facebook_Tool_Request.Helpers.Common.DownloadFile(text3);
                            MessageBoxHelper.ShowMessageBox("Cập nhật ChromeDriver thành công!", 1);
                            return;
                        }
                    }
                }
                catch
                {
                }
                MessageBoxHelper.ShowMessageBox("Cập nhật ChromeDriver thất bại!", 2);

                //bool flag = CommonChrome.GetUserAgentDefault() == "";
                //if (flag)
                //{
                //    MessageBoxHelper.ShowMessageBox("Cập nhật chromedriver!", 3);
                //    bool update = Common.ChromedriverUpdater.ChromeUpdate();
                //    if (update)
                //    {
                //        MessageBoxHelper.ShowMessageBox("Cập nhật hoàn tất!", 1);
                //    }
                //    else
                //    {
                //        MessageBoxHelper.ShowMessageBox("Cập nhật Lỗi!", 2);
                //    }
                //}
                //else
                //{
                //    MessageBoxHelper.ShowMessageBox("Phiên bản chromedriver khả dụng!", 1);
                //}
            }
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("https://api.cua.monster/license/ver.xml");
        }

        private void btnCheckApiUserTinsoft_Click(object sender, EventArgs e)
        {
            string api_user = this.txtApiUser.Text.Trim();
            List<string> listKey = TinsoftProxy.GetListKey(api_user);
            bool flag = listKey.Count > 0;
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(string.Format("Đang có {0} proxy khả dụng!", listKey.Count), 1);
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Không có proxy khả dụng!", 2);
            }
        }

        private void btnCheckApiKeyTinsoft_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            List<string> list2 = this.txtApiProxy.Lines.ToList<string>();
            list2 = Helpers.Common.RemoveEmptyItems(list2);
            foreach (string text in list2)
            {
                bool flag = TinsoftProxy.CheckApiProxy(text);
                if (flag)
                {
                    list.Add(text);
                }
            }
            this.txtApiProxy.Lines = list.ToArray();
            bool flag2 = list.Count > 0;
            if (flag2)
            {
                MessageBoxHelper.ShowMessageBox(string.Format("Đang có {0} proxy khả dụng!", list.Count), 1);
            }
            else
            {
                MessageBoxHelper.ShowMessageBox("Không có proxy khả dụng!", 2);
            }
        }

        private void rbTinsoft_CheckedChanged(object sender, EventArgs e)
        {
            this.plTinsoft.Enabled = this.rbTinsoft.Checked;
            this.plTinsoft.Visible = this.rbTinsoft.Checked;
        }

        private void plTinsoft_Click(object sender, EventArgs e)
        {
            bool flag = (e as MouseEventArgs).Button == MouseButtons.Right && Control.ModifierKeys == Keys.Control;
            if (flag)
            {
                this.ckbWaitDoneAllTinsoft.Visible = true;
            }
        }

        private void txtApiProxy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txtApiProxy.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblCountApiProxy.Text = "(" + list.Count.ToString() + ")";
            }
            catch
            {
            }
        }
        public Dictionary<string, string> TinsoftGetListLocation()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> listCountryTinsoft = SetupFolder.GetListCountryTinsoft();
            for (int i = 0; i < listCountryTinsoft.Count; i++)
            {
                string[] array = listCountryTinsoft[i].Split(new char[]
                {
                    '|'
                });
                dictionary.Add(array[0], array[1]);
            }
            return dictionary;
        }

        private void rbminproxy_CheckedChanged(object sender, EventArgs e)
        {
            this.plMinProxy.Enabled = this.rbminproxy.Checked;
            this.plMinProxy.Visible = this.rbminproxy.Checked;
        }

        private void txtApiMinproxy_TextChanged(object sender, EventArgs e)
        {
            List<string> list = this.txtApiMinproxy.Lines.ToList<string>();
            list = Helpers.Common.RemoveEmptyItems(list);
            this.label17.Text = string.Format(Language.GetValue("Nhập API KEY ({0}):"), list.Count.ToString());
        }

        private void btnChosseProfile_Click(object sender, EventArgs e)
        {
            this.txbPathProfile.Text = Helpers.Common.SelectFolderNew("Chọn thư mục profiles");
        }

        private void ckbThunhoPage_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://googlechromelabs.github.io/chrome-for-testing/#stable");
        }
    }
}

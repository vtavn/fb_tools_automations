using Facebook_Tool_Request.Helpers;
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
    public partial class fImportAccount : Form
    {
        public fImportAccount(string idFile)
        {
            this.InitializeComponent();
            this.Load_cbbThuMuc();
            bool flag = idFile != "" && idFile != "-1" && idFile != "999999";
            if (flag)
            {
                this.cbbThuMuc.SelectedValue = idFile;
            }
            this.cbbDinhDangNhap.SelectedIndex = 0;
            this.lstCbbDinhDang = new List<ComboBox>
            {
                this.cbbDinhDang1,
                this.cbbDinhDang2,
                this.cbbDinhDang3,
                this.cbbDinhDang4,
                this.cbbDinhDang5,
                this.cbbDinhDang6,
                this.cbbDinhDang7,
                this.cbbDinhDang8,
                this.cbbDinhDang9
            };
            fImportAccount.isAddFile = false;
            fImportAccount.isAddAccount = false;
            fImportAccount.idFileAdded = -1;
        }

        private void Load_cbbThuMuc()
        {
            this.indexOld = this.cbbThuMuc.SelectedIndex;
            DataTable allFilesFromDatabase = CommonSQL.GetAllFilesFromDatabase(false);
            bool flag = allFilesFromDatabase.Rows.Count > 0;
            if (flag)
            {
                this.cbbThuMuc.DataSource = allFilesFromDatabase;
                this.cbbThuMuc.ValueMember = "id";
                this.cbbThuMuc.DisplayMember = "name";
                bool flag2 = this.indexOld == -1;
                if (flag2)
                {
                    this.indexOld = 0;
                }
                this.cbbThuMuc.SelectedIndex = this.indexOld;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txbAccount.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                bool flag = list.Count == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập thông tin tài khoản!"), 3);
                    this.txbAccount.Focus();
                }
                else
                {
                    bool flag2 = this.cbbThuMuc.SelectedValue == null;
                    if (flag2)
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn thư mục!"), 3);
                    }
                    else
                    {
                        string idFile = this.cbbThuMuc.SelectedValue.ToString();
                        bool isCheckThongTin = this.ckbCheckThongTin.Checked;
                        int selectedIndex = this.cbbDinhDangNhap.SelectedIndex;
                        bool flag3 = selectedIndex == 6;
                        if (flag3)
                        {
                            bool flag4 = false;
                            for (int i = 0; i < this.lstCbbDinhDang.Count; i++)
                            {
                                bool flag5 = this.lstCbbDinhDang[i].SelectedIndex > -1;
                                if (flag5)
                                {
                                    flag4 = true;
                                    break;
                                }
                            }
                            bool flag6 = !flag4;
                            if (flag6)
                            {
                                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn định dạng tài khoản!"), 3);
                                return;
                            }
                        }
                        int maxThread = 100;
                        bool flag7 = list.Count < 100;
                        if (flag7)
                        {
                            maxThread = list.Count;
                        }
                        this.lblSuccess.Text = "0";
                        this.lblError.Text = "0";
                        this.lblWallDie.Text = "0";
                        this.lblKhongCheckDuoc.Text = "0";
                        this.lblWallLive.Text = "0";
                        this.lstAccount = new List<string>();
                        string[] temp;
                        switch (selectedIndex)
                        {
                            case 0:
                                {
                                    int num = 3;
                                    for (int j = 0; j < list.Count; j++)
                                    {
                                        temp = list[j].Split(new char[]
                                        {
                                    '|'
                                        });
                                        bool flag8 = temp.Count<string>() >= num;
                                        if (flag8)
                                        {
                                            this.lstAccount.Add(string.Concat(new string[]
                                            {
                                        temp[0],
                                        "|",
                                        temp[1],
                                        "|||||",
                                        temp[2],
                                        "|||"
                                            }));
                                        }
                                        else
                                        {
                                            this.IncrementLabel(this.lblError, -1);
                                        }
                                    }
                                    break;
                                }
                            case 1:
                                for (int k = 0; k < list.Count; k++)
                                {
                                    this.lstAccount.Add("|||" + list[k] + "||||||");
                                }
                                break;
                            case 2:
                                {
                                    int num = 2;
                                    for (int l = 0; l < list.Count; l++)
                                    {
                                        temp = list[l].Split(new char[]
                                        {
                                    '|'
                                        });
                                        bool flag9 = temp.Count<string>() >= num;
                                        if (flag9)
                                        {
                                            this.lstAccount.Add(temp[0] + "|" + temp[1] + "||||||||");
                                        }
                                        else
                                        {
                                            this.IncrementLabel(this.lblError, -1);
                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    int num = 4;
                                    for (int m = 0; m < list.Count; m++)
                                    {
                                        temp = list[m].Split(new char[]
                                        {
                                    '|'
                                        });
                                        bool flag10 = temp.Count<string>() >= num;
                                        if (flag10)
                                        {
                                            this.lstAccount.Add(string.Concat(new string[]
                                            {
                                        temp[0],
                                        "|",
                                        temp[1],
                                        "|",
                                        temp[2],
                                        "|",
                                        temp[3],
                                        "||||||"
                                            }));
                                        }
                                        else
                                        {
                                            this.IncrementLabel(this.lblError, -1);
                                        }
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    int num = 6;
                                    for (int n = 0; n < list.Count; n++)
                                    {
                                        temp = list[n].Split(new char[]
                                        {
                                    '|'
                                        });
                                        bool flag11 = temp.Count<string>() >= num;
                                        if (flag11)
                                        {
                                            this.lstAccount.Add(string.Concat(new string[]
                                            {
                                        temp[0],
                                        "|",
                                        temp[1],
                                        "|",
                                        temp[2],
                                        "|",
                                        temp[3],
                                        "|",
                                        temp[4],
                                        "|",
                                        temp[5],
                                        "||||"
                                            }));
                                        }
                                        else
                                        {
                                            this.IncrementLabel(this.lblError, -1);
                                        }
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    int num = 7;
                                    for (int num2 = 0; num2 < list.Count; num2++)
                                    {
                                        temp = list[num2].Split(new char[]
                                        {
                                    '|'
                                        });
                                        bool flag12 = temp.Count<string>() >= num;
                                        if (flag12)
                                        {
                                            this.lstAccount.Add(string.Concat(new string[]
                                            {
                                        temp[0],
                                        "|",
                                        temp[1],
                                        "|",
                                        temp[2],
                                        "|",
                                        temp[3],
                                        "|",
                                        temp[4],
                                        "|",
                                        temp[5],
                                        "|",
                                        temp[6],
                                        "|||"
                                            }));
                                        }
                                        else
                                        {
                                            this.IncrementLabel(this.lblError, -1);
                                        }
                                    }
                                    break;
                                }
                            case 6:
                                for (int num3 = 0; num3 < list.Count; num3++)
                                {
                                    temp = list[num3].Split(new char[]
                                    {
                                    '|'
                                    });
                                    string text = "";
                                    string text2 = "";
                                    string text3 = "";
                                    string text4 = "";
                                    string text5 = "";
                                    string text6 = "";
                                    string text7 = "";
                                    string text8 = "";
                                    string text9 = "";
                                    string text10 = "";
                                    try
                                    {
                                        for (int num4 = 0; num4 < this.lstCbbDinhDang.Count; num4++)
                                        {
                                            switch (this.lstCbbDinhDang[num4].SelectedIndex)
                                            {
                                                case 0:
                                                    text = temp[num4];
                                                    break;
                                                case 1:
                                                    text2 = temp[num4];
                                                    break;
                                                case 2:
                                                    text3 = temp[num4];
                                                    break;
                                                case 3:
                                                    text4 = temp[num4];
                                                    break;
                                                case 4:
                                                    text5 = temp[num4];
                                                    break;
                                                case 5:
                                                    text6 = temp[num4];
                                                    break;
                                                case 6:
                                                    text7 = temp[num4];
                                                    break;
                                                case 7:
                                                    text8 = temp[num4];
                                                    break;
                                                case 8:
                                                    {
                                                        bool flag13 = temp[num4].Trim() == "";
                                                        if (flag13)
                                                        {
                                                            text9 = "";
                                                        }
                                                        else
                                                        {
                                                            text9 = temp[num4] + "*0";
                                                        }
                                                        break;
                                                    }
                                                case 9:
                                                    text10 = temp[num4];
                                                    break;
                                            }
                                        }
                                        this.lstAccount.Add(string.Concat(new string[]
                                        {
                                        text,
                                        "|",
                                        text2,
                                        "|",
                                        text3,
                                        "|",
                                        text4,
                                        "|",
                                        text5,
                                        "|",
                                        text6,
                                        "|",
                                        text7,
                                        "|",
                                        text8,
                                        "|",
                                        text9,
                                        "|",
                                        text10
                                        }));
                                    }
                                    catch
                                    {
                                        this.IncrementLabel(this.lblError, -1);
                                    }
                                }
                                break;
                        }
                        List<string> lstQuery = new List<string>();
                        this.lstThread = new List<Thread>();
                        new Thread(delegate ()
                        {
                            try
                            {
                                this.btnAdd.Invoke(new MethodInvoker(delegate ()
                                {
                                    this.btnAdd.Enabled = false;
                                }));
                                this.UpdateStatus(Language.GetValue("Chuẩn bị thêm tài khoản..."), this.lblStatus);
                                bool ischeckThongTin = isCheckThongTin;
                                if (ischeckThongTin)
                                {
                                    int num5 = 0;
                                    while (num5 < this.lstAccount.Count)
                                    {
                                        bool flag14 = this.lstThread.Count < maxThread;
                                        if (flag14)
                                        {
                                            int stt = num5++;
                                            this.UpdateStatus(string.Format(Language.GetValue("Đang check thông tin {0}/{1}..."), num5, this.lstAccount.Count), this.lblStatus);
                                            Thread thread = new Thread(delegate ()
                                            {
                                                try
                                                {
                                                    string text14 = this.lstAccount[stt];
                                                    bool flag19 = text14.Trim() == "";
                                                    if (!flag19)
                                                    {
                                                        string[] array2 = text14.Split(new char[]
                                                        {
                                                            '|'
                                                        });
                                                        string text15 = array2[0];
                                                        string pass2 = array2[1];
                                                        string token2 = array2[2];
                                                        string text16 = array2[3];
                                                        string email2 = array2[4];
                                                        string passMail2 = array2[5];
                                                        string fa2 = array2[6];
                                                        string useragent2 = array2[7];
                                                        string proxy2 = array2[8];
                                                        string birthday2 = array2[9];
                                                        string name2 = "";
                                                        string friends2 = "";
                                                        string groups2 = "";
                                                        string gender2 = "";
                                                        string text17 = "unknow";
                                                        bool flag20 = text15 == "";
                                                        if (flag20)
                                                        {
                                                            text15 = Regex.Match(text16, "c_user=(.*?);").Groups[1].Value;
                                                        }
                                                        bool flag21 = text15 == "";
                                                        if (flag21)
                                                        {
                                                            text17 = "Die";
                                                        }
                                                        else
                                                        {
                                                            string text18 = CommonRequest.CheckInfoUsingUid(text15);
                                                            bool flag22 = text18.StartsWith("0|");
                                                            if (flag22)
                                                            {
                                                                bool flag23 = CommonRequest.CheckLiveWall(text15).StartsWith("0|");
                                                                if (flag23)
                                                                {
                                                                    text17 = "Die";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bool flag24 = text18.StartsWith("1|");
                                                                if (flag24)
                                                                {
                                                                    temp = text18.Split(new char[]
                                                                    {
                                                                        '|'
                                                                    });
                                                                    name2 = temp[2];
                                                                    gender2 = temp[3].ToLower();
                                                                    birthday2 = temp[4];
                                                                    friends2 = temp[5];
                                                                    groups2 = temp[6];
                                                                    text17 = "Live";
                                                                }
                                                            }
                                                        }
                                                        string text19 = text17;
                                                        string a = text19;
                                                        if (!(a == "Live"))
                                                        {
                                                            if (!(a == "Die"))
                                                            {
                                                                this.IncrementLabel(this.lblKhongCheckDuoc, -1);
                                                            }
                                                            else
                                                            {
                                                                this.IncrementLabel(this.lblWallDie, -1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            this.IncrementLabel(this.lblWallLive, -1);
                                                        }
                                                        lstQuery.Add(CommonSQL.ConvertToSqlInsertAccount(text15, pass2, token2, text16, email2, name2, friends2, groups2, birthday2, gender2, text17, fa2, idFile, passMail2, useragent2, proxy2));
                                                    }
                                                }
                                                catch
                                                {
                                                }
                                            });
                                            this.lstThread.Add(thread);
                                            thread.Start();
                                        }
                                        else
                                        {
                                            for (int num6 = 0; num6 < this.lstThread.Count; num6++)
                                            {
                                                bool flag15 = !this.lstThread[num6].IsAlive;
                                                if (flag15)
                                                {
                                                    this.lstThread.RemoveAt(num6--);
                                                }
                                            }
                                        }
                                    }
                                    for (int num7 = 0; num7 < this.lstThread.Count; num7++)
                                    {
                                        this.lstThread[num7].Join();
                                    }
                                }
                                else
                                {
                                    for (int num8 = 0; num8 < this.lstAccount.Count; num8++)
                                    {
                                        try
                                        {
                                            string text11 = this.lstAccount[num8];
                                            bool flag16 = text11.Trim() == "";
                                            if (flag16)
                                            {
                                                return;
                                            }
                                            string[] array = text11.Split(new char[]
                                            {
                                                '|'
                                            });
                                            string text12 = array[0];
                                            string pass = array[1];
                                            string token = array[2];
                                            string text13 = array[3];
                                            string email = array[4];
                                            string passMail = array[5];
                                            string fa = array[6];
                                            string useragent = array[7];
                                            string proxy = array[8];
                                            string birthday = array[9];
                                            string name = "";
                                            string friends = "";
                                            string groups = "";
                                            string gender = "";
                                            string info = "unknow";
                                            bool flag17 = text12 == "";
                                            if (flag17)
                                            {
                                                text12 = Regex.Match(text13, "c_user=(.*?);").Groups[1].Value;
                                            }
                                            lstQuery.Add(CommonSQL.ConvertToSqlInsertAccount(text12, pass, token, text13, email, name, friends, groups, birthday, gender, info, fa, idFile, passMail, useragent, proxy));
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                this.UpdateStatus(Language.GetValue("Đang thêm tài khoản..."), this.lblStatus);
                                bool flag18 = lstQuery.Count >= 0;
                                if (flag18)
                                {
                                    lstQuery = CommonSQL.ConvertToSqlInsertAccount(lstQuery);
                                    for (int num9 = 0; num9 < lstQuery.Count; num9++)
                                    {
                                        this.IncrementLabel(this.lblSuccess, Connector.Instance.ExecuteNonQuery(lstQuery[num9]));
                                    }
                                }
                                this.UpdateStatus((Convert.ToInt32(this.lblTotal.Text) - Convert.ToInt32(this.lblSuccess.Text)).ToString() ?? "", this.lblError);
                                this.btnAdd.Invoke(new MethodInvoker(delegate ()
                                {
                                    this.btnAdd.Enabled = true;
                                }));
                                MessageBoxHelper.ShowMessageBox(Language.GetValue("Thêm tài khoản thành công!"), 1);
                                this.UpdateStatus(Language.GetValue("Thêm tài khoản thành công!"), this.lblStatus);
                                fImportAccount.isAddAccount = true;
                                fImportAccount.idFileAdded = Convert.ToInt32(idFile);
                            }
                            catch (Exception ex2)
                            {
                                Helpers.Common.ExportError(null, ex2, "AddAccount");
                            }
                        }).Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
                Helpers.Common.ExportError(null, ex, "AddAccount");
            }
        }

        private void UpdateStatus(string content, Label lbl)
        {
            lbl.Invoke(new MethodInvoker(delegate ()
            {
                Application.DoEvents();
                lbl.Text = content;
            }));
        }

        private void TxbAccount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txbAccount.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.lblTotal.Text = list.Count.ToString();
            }
            catch
            {
            }
        }

        private void IncrementLabel(Label lbl, int count = -1)
        {
            bool flag = count == -1;
            if (flag)
            {
                lbl.Invoke(new MethodInvoker(delegate ()
                {
                    Application.DoEvents();
                    lbl.Text = (Convert.ToInt32(lbl.Text) + ((count == -1) ? 1 : count)).ToString();
                }));
            }
            else
            {
                lbl.Invoke(new MethodInvoker(delegate ()
                {
                    Application.DoEvents();
                    lbl.Text = (Convert.ToInt32(lbl.Text) + count).ToString();
                }));
            }
        }

        private void cbbDinhDangNhap_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.plDinhDangNhap.Visible = (this.cbbDinhDangNhap.SelectedIndex == this.cbbDinhDangNhap.Items.Count - 1);
            bool visible = this.plDinhDangNhap.Visible;
            if (visible)
            {
                this.LoadDinhDang();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fAddFolder f = new fAddFolder();
            Helpers.Common.ShowForm(f);
            bool isAdd = fAddFolder.isAdd;
            if (isAdd)
            {
                this.Load_cbbThuMuc();
                this.cbbThuMuc.SelectedIndex = this.cbbThuMuc.Items.Count - 1;
                fImportAccount.isAddFile = true;
            }
        }

        private bool CheckExistDinhDang()
        {
            bool result = false;
            List<int> list = new List<int>();
            for (int i = 0; i < this.lstCbbDinhDang.Count; i++)
            {
                int selectedIndex = this.lstCbbDinhDang[i].SelectedIndex;
                bool flag = selectedIndex != -1;
                if (flag)
                {
                    bool flag2 = list.Contains(selectedIndex);
                    if (flag2)
                    {
                        result = true;
                        break;
                    }
                    list.Add(selectedIndex);
                }
            }
            return result;
        }

        private void cbbDinhDang1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = this.CheckExistDinhDang();
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Tùy chọn này đã tồn tại, vui lòng chọn tùy chọn khác!"), 3);
                (sender as ComboBox).SelectedIndex = -1;
            }
        }

        public static bool isAddAccount = false;

        public static int idFileAdded = -1;

        public static bool isAddFile = false;

        private List<ComboBox> lstCbbDinhDang;

        private int indexOld = 0;

        private List<string> lstAccount = new List<string>();

        private List<Thread> lstThread = null;

        private object objLock = new object();

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ResetDinhDang();
        }
        private void ResetDinhDang()
        {
            try
            {
                for (int i = 0; i < this.lstCbbDinhDang.Count; i++)
                {
                    this.lstCbbDinhDang[i].SelectedIndex = -1;
                }
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.SaveDinhDang();
        }
        private void LoadDinhDang()
        {
            try
            {
                bool flag = File.Exists("settings\\format_paste.txt");
                if (flag)
                {
                    string text = File.ReadAllText("settings\\format_paste.txt");
                    for (int i = 0; i < this.lstCbbDinhDang.Count; i++)
                    {
                        this.lstCbbDinhDang[i].SelectedIndex = Convert.ToInt32(text.Split(new char[]
                        {
                            '|'
                        })[i]);
                    }
                }
            }
            catch
            {
            }
        }
        private void SaveDinhDang()
        {
            try
            {
                string text = "";
                for (int i = 0; i < this.lstCbbDinhDang.Count; i++)
                {
                    text = text + this.lstCbbDinhDang[i].SelectedIndex.ToString() + "|";
                }
                text = text.TrimEnd(new char[]
                {
                    '|'
                });
                File.WriteAllText("settings\\format_paste.txt", text);
                MessageBoxHelper.ShowMessageBox("Lưu định dạng thành công!!", 1);
            }
            catch
            {
            }
        }
    }
}

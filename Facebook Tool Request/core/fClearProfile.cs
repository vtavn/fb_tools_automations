using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fClearProfile : Form
    {
        private JSON_Settings setting_general;
        private List<string> lstProfileRac = new List<string>();

        public fClearProfile()
        {
            InitializeComponent();
            setting_general = new JSON_Settings("configGeneral");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnCancel.Enabled = false;
            int count = 0;
            LoadStatus(string.Format(Language.GetValue("Đang dọn dẹp profile {0}/{1}..."), count, lstProfileRac.Count));
            int iThread = 0;
            int maxThread = 20;
            new Thread((ThreadStart)delegate
            {
                try
                {
                    int num = 0;
                    while (num < lstProfileRac.Count)
                    {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = num++;
                            new Thread((ThreadStart)delegate
                            {
                                try
                                {
                                    string path = setting_general.GetValue("txbPathProfile") + "\\" + lstProfileRac[row];
                                    Directory.Delete(path, recursive: true);
                                    Interlocked.Increment(ref count);
                                    LoadStatus(string.Format(Language.GetValue("Đang dọn dẹp profile {0}/{1}..."), count, lstProfileRac.Count));
                                    Interlocked.Decrement(ref iThread);
                                }
                                catch
                                {
                                }
                            }).Start();
                        }
                        else
                        {
                            Application.DoEvents();
                            Thread.Sleep(200);
                        }
                    }
                    while (iThread > 0)
                    {
                        Helpers.Common.DelayTime(1.0);
                    }
                    Invoke((Action)delegate
                    {
                        Close();
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã dọn dẹp profile xong!"));
                    });
                }
                catch
                {
                }
            }).Start();
        }
        private List<string> GetListUidFromDatabase()
        {
            List<string> list = new List<string>();
            try
            {
                DataTable allAccountFromDatabase = CommonSQL.GetAllAccountFromDatabase();
                for (int i = 0; i < allAccountFromDatabase.Rows.Count; i++)
                {
                    list.Add(allAccountFromDatabase.Rows[i]["uid"].ToString());
                }
            }
            catch
            {
            }
            return list;
        }
        private void LoadStatus(string content)
        {
            Invoke((Action)delegate
            {
                lblStatus.Text = content;
            });
        }
        private void fClearProfile_Load(object sender, EventArgs e)
        {
            new Thread((ThreadStart)delegate
            {
                try
                {
                    List<string> list = Directory.GetDirectories(setting_general.GetValue("txbPathProfile")).ToList();
                    List<string> listUidFromDatabase = GetListUidFromDatabase();
                    string text = "";
                    for (int i = 0; i < list.Count; i++)
                    {
                        text = list[i].Substring(list[i].LastIndexOf("\\") + 1);
                        if (!listUidFromDatabase.Contains(text))
                        {
                            lstProfileRac.Add(text);
                        }
                    }
                    if (lstProfileRac.Count > 0)
                    {
                        Invoke((Action)delegate
                        {
                            lblStatus.Text = string.Format(Language.GetValue("Tìm thấy {0} profile rác!"), lstProfileRac.Count) + "\r\n" + Language.GetValue("Bạn có muốn dọn dẹp?");
                            btnAdd.Visible = true;
                            btnCancel.Visible = true;
                        });
                    }
                    else
                    {
                        Invoke((Action)delegate
                        {
                            Close();
                            MessageBoxHelper.ShowMessageBox(Language.GetValue("Không có profile rác!"));
                        });
                    }
                }
                catch (Exception)
                {
                }
            }).Start();
        }
        
    }
}

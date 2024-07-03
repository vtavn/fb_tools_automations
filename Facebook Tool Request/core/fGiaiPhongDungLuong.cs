using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fGiaiPhongDungLuong : Form
    {
        public fGiaiPhongDungLuong(bool isShutdown = false)
        {
            this.InitializeComponent();
            this.isShutdown = isShutdown;
        }
        private void SetText(Label lbl, string content)
        {
            try
            {
                if (lbl.InvokeRequired)
                {
                    lbl.Invoke((MethodInvoker)delegate
                    {
                        lbl.Text = content;
                    });
                }
                else
                {
                    lbl.Text = content;
                }
            }
            catch
            {
                // Handle the exception or log it if necessary
            }
        }

        private new void Visible(Label lbl)
        {
            try
            {
                if (lbl.InvokeRequired)
                {
                    lbl.Invoke((MethodInvoker)delegate
                    {
                        lbl.Visible = true;
                    });
                }
                else
                {
                    lbl.Visible = true;
                }
            }
            catch
            {
                // Handle the exception or log it if necessary
            }
        }

        private void BoldText(Label lbl, bool isBold = true)
        {
            try
            {
                if (isBold)
                {
                    lbl.Font = new Font("Montserrat", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
                }
                else
                {
                    lbl.Font = new Font("Montserrat", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
            catch
            {
            }
        }

        private List<string> GetListUidFromDatabase()
        {
            List<string> list = new List<string>();
            List<string> result;
            try
            {
                DataTable allAccountFromDatabase = CommonSQL.GetAllAccountFromDatabase(true);
                for (int i = 0; i < allAccountFromDatabase.Rows.Count; i++)
                {
                    list.Add(allAccountFromDatabase.Rows[i]["uid"].ToString());
                }
                result = list;
            }
            catch
            {
                result = list;
            }
            return result;
        }

        private void btnCan()
        {
            this.btnCancel.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Invoke(new Action(this.btnCan));
            this.isStop = true;
            base.Close();
        }

        private void fGiaiPhongDungLuong_Load(object sender, EventArgs e)
        {
            Helpers.Common.KillProcess("chromedriver");
            new Thread(delegate ()
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {

                        BoldText(lblXoaFileRac, true);
                        SetText(lblStatusXoaFileRac, "Đang chạy" + "...");
                        Visible(lblStatusXoaFileRac);
                        Helpers.Common.DeleteFolder("log_capture");
                        bool flag = !this.isStop;
                        if (flag)
                        {
                            SetText(lblStatusXoaFileRac, "Hoàn thành!");
                            BoldText(lblXoaFileRac, false);
                            BoldText(lblDonDepProfileRac, true);
                            SetText(lblStatusDonDepProfileRac, "");
                            Visible(lblStatusDonDepProfileRac);
                            int valueInt = SettingsTool.GetSettings("configGeneral").GetValueInt("nudHideThread", 10);
                            List<string> list = Directory.GetDirectories(SettingsTool.GetSettings("configGeneral").GetValue("txbPathProfile", "")).ToList<string>();
                            List<string> listUidFromDatabase = this.GetListUidFromDatabase();
                            List<string> lstProfileRac = new List<string>();
                            for (int i = 0; i < list.Count; i++)
                            {
                                string text = list[i].Substring(list[i].LastIndexOf("\\") + 1);
                                bool flag2 = !listUidFromDatabase.Contains(text);
                                if (flag2)
                                {
                                    lstProfileRac.Add(text);
                                }
                            }
                            try
                            {
                                int iThread3 = 0;
                                int count3 = 0;
                                int num = 0;
                                while (num < lstProfileRac.Count && !this.isStop)
                                {
                                    bool flag3 = iThread3 < valueInt;
                                    if (flag3)
                                    {
                                        Interlocked.Increment(ref iThread3);
                                        int row2 = num++;
                                        new Thread(delegate ()
                                        {
                                            try
                                            {
                                                //    Helpers.Common.DeleteFolder(SettingsTool.GetSettings("configGeneral").GetValue("txbPathProfile", "") + "\\" + lstProfileRac[row2]);
                                                Interlocked.Increment(ref count3);
                                                SetText(lblStatusDonDepProfileRac, "Đang chạy" + string.Format(" {0}/{1}...", count3, lstProfileRac.Count));
                                                Interlocked.Decrement(ref iThread3);
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
                                while (iThread3 > 0)
                                {
                                    Helpers.Common.DelayTime(1.0);
                                }
                            }
                            catch
                            {
                            }
                            bool flag4 = !this.isStop;
                            if (flag4)
                            {
                                SetText(lblStatusDonDepProfileRac, "Hoàn thành!");
                                BoldText(lblDonDepProfileRac, false);
                                BoldText(lblXoaCacheProfile, true);
                                SetText(lblStatusXoaCacheProfile, "");
                                Visible(lblStatusXoaCacheProfile);
                                int iThread2 = 0;
                                int count2 = 0;
                                List<string> lstUid = this.GetListUidFromDatabase();
                                int num2 = 0;
                                while (num2 < lstUid.Count && !this.isStop)
                                {
                                    bool flag5 = iThread2 < valueInt;
                                    if (flag5)
                                    {
                                        Interlocked.Increment(ref iThread2);
                                        string uid = lstUid[num2++];
                                        new Thread(delegate ()
                                        {
                                            try
                                            {
                                                string text2 = SettingsTool.GetSettings("configGeneral").GetValue("txbPathProfile", "") + "\\" + uid;
                                                bool flag9 = Directory.Exists(text2);
                                                if (flag9)
                                                {
                                                    //Helpers.Common.DeleteFolder(text2 + "\\Default\\Cache");
                                                    //Helpers.Common.DeleteFolder(text2 + "\\Default\\Code Cache");
                                                    //Helpers.Common.DeleteFolder(text2 + "\\OptimizationGuidePredictionModels");
                                                    //Helpers.Common.DeleteFolder(text2 + "\\Default\\optimization_guide_prediction_model_downloads");
                                                    //Helpers.Common.DeleteFolder(text2 + "\\SwReporter");
                                                    //Helpers.Common.DeleteFolder(text2 + "\\pnacl");
                                                }
                                                Interlocked.Increment(ref count2);
                                                SetText(lblStatusXoaCacheProfile, "Đang chạy" + string.Format(" {0}/{1}...", count2, lstUid.Count));
                                                Interlocked.Decrement(ref iThread2);
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
                                while (iThread2 > 0)
                                {
                                    Helpers.Common.DelayTime(1.0);
                                }
                                bool flag6 = !this.isStop;
                                if (flag6)
                                {
                                    SetText(lblStatusXoaCacheProfile, "Hoàn thành!");
                                    BoldText(lblXoaCacheProfile, false);
                                    BoldText(lblXoaTemp, true);
                                    SetText(lblStatusXoaTemp, "");
                                    Visible(lblStatusXoaTemp);
                                    try
                                    {
                                        int iThread = 0;
                                        int count = 0;
                                        List<string> lstTemp = Directory.GetDirectories(Path.GetTempPath()).ToList<string>();
                                        int num3 = 0;
                                        while (num3 < lstTemp.Count && !this.isStop)
                                        {
                                            bool flag7 = iThread < valueInt;
                                            if (flag7)
                                            {
                                                Interlocked.Increment(ref iThread);
                                                int row = num3++;
                                                new Thread(delegate ()
                                                {
                                                    try
                                                    {
                                                        //Helpers.Common.DeleteFolder(lstTemp[row]);
                                                        Interlocked.Increment(ref count);
                                                        SetText(lblStatusXoaTemp, "Đang chạy" + string.Format(" {0}/{1}...", count, lstTemp.Count));
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
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        bool flag8 = this.isShutdown;
                        if (flag8)
                        {
                            Helpers.Common.Shutdown();
                        }
                        MessageBoxHelper.ShowMessageBox("Dọn dẹp thành công!");
                        base.Close();
                    });
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowMessageBox("Error:\n" + ex.ToString(), 1);
                }
            }).Start();
        }

        private bool isShutdown;

        private bool isStop;
    }
}

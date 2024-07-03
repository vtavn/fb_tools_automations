using Facebook_Tool_Request.Helpers;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core.fTools
{
    public partial class fScanLive : Form
    {
        private int lineCount = 0;
        public fScanLive()
        {
            InitializeComponent();

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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fScanLive_Load(object sender, EventArgs e)
        {
            this.statusNote.Text = "";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string idPost = this.idPost.Text.Trim();
            string accessToken = this.accessToken.Text.Trim();
            if (string.IsNullOrEmpty(idPost) && string.IsNullOrEmpty(accessToken))
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng nhập đầy đủ token và idpost!", 3);
                return;
            }
            int iThread = 1;
            Interlocked.Increment(ref iThread);
            new Thread((ThreadStart)delegate
            {
                cControl("start");
                RunThread(idPost, accessToken);
                Interlocked.Decrement(ref iThread);
            }).Start();
        }
        private void ShowStatus(string status)
        {
            if (statusNote.InvokeRequired)
            {
                this.Invoke(new Action<string>(ShowStatus), status);
            }
            else
            {
                this.statusNote.Text = status;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cControl("stop");
        }

        private void cControl(string tt)
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    if (tt == "start")
                    {
                        timer2.Start();
                        lbStatus.Text = "ĐANG CHẠY...";
                        lbStatus.ForeColor = Color.Green;
                        btnStop.Enabled = true; btnStart.Enabled = false;
                        plsettings.Enabled = false;
                        isStop = false;

                    }
                    else if (tt == "stop")
                    {
                        timer2.Stop();
                        lbStatus.Text = "ĐANG NGHỈ!";
                        //this.statusNote.Text = "";
                        lbStatus.ForeColor = Color.Red;
                        btnStop.Enabled = false; btnStart.Enabled = true;
                        plsettings.Enabled = true;
                        isStop = true;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private string convertSecondToTime(int n)
        {
            int hour, minute, second;
            hour = n / 3600;
            minute = n % 3660 / 60;
            second = n % 3600 % 60;
            return $"{hour:D2}:{minute:D2}:{second:D2}";
        }

        int timeRunOK = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            timeRunOK++;
            lbtimerun.Text = convertSecondToTime(timeRunOK);
        }

        bool firstRun = true;
        string apiUrlOld = "";
        bool saveTrue = true;
        bool isStop = false;

        private void RunThread(string idPost, string accessToken)
        {
            ShowStatus("Check Token");

            string rsCheckToken = GraphFacebook($"https://graph.facebook.com/me?access_token={accessToken}");
            if(rsCheckToken == "Error")
            {
                ShowStatus($"Token Die!");
                cControl("stop");
                return;
            }

            JObject jCheckToken = JObject.Parse(rsCheckToken);
            if (jCheckToken["id"].ToString() != null)
            {
                string nameToken = jCheckToken["name"].ToString();
                ShowStatus($"Token name: {nameToken}");
                Thread.Sleep(1000);
                int errorCurrent = 0;

                while (true)
                {   
                    if(isStop)
                    {
                        cControl("stop");
                        ShowStatus($"Đã dừng chạy!");
                        break;
                    }

                    if(errorCurrent == this.errorStop.Value)
                    {
                        ShowStatus($"Lỗi {errorCurrent} lần. Dừng chạy!");
                        cControl("stop");
                        break;
                    }    
                    string apiUrl = $"https://graph.facebook.com/{idPost}?fields=comments&access_token={accessToken}";
                    while (!string.IsNullOrEmpty(apiUrl))
                    {
                        List<string> comments;
                        apiUrl = GetComments(apiUrl, out comments);

                        if(apiUrl == "IdError")
                        {
                            isStop = true;
                            ShowStatus($"ID SAI!");
                            break;
                        }

                        if (this.saveTrue && this.apiUrlOld != apiUrl)
                        {
                            SaveToFile(comments, idPost);
                            ShowStatus($"Lưu thành công!");
                        }
                        else
                        {
                            errorCurrent++;
                            ShowStatus($"Hết data get. Đang nghỉ {this.errorDelay.Text}s");
                            Thread.Sleep(Convert.ToInt32(int.Parse(this.errorDelay.Text) * 1000.0));
                        }

                        if (string.IsNullOrEmpty(apiUrl))
                        {
                            apiUrl = this.apiUrlOld;
                            saveTrue = false;
                            Console.WriteLine($"apiurl check set: {apiUrl}");
                            ShowStatus($"Set OK {apiUrl}");
                        }
                    }
                    Thread.Sleep(Convert.ToInt32(int.Parse(this.errorDelay.Text) * 1000.0));
                }
            }
        }

        private string GetComments(string apiUrl, out List<string> comments)
        {
            ShowStatus($"Get Data!");
            comments = new List<string>();

            Thread.Sleep(Convert.ToInt32(int.Parse(this.errorDelay.Text) * 1000.0));
            string rs = GraphFacebook(apiUrl);

            if (!string.IsNullOrEmpty(apiUrl))
            {
                apiUrlOld = apiUrl;
            }

            string next_url = null;
            if(rs == "Error") { 
                return "IdError";
            }

            JObject jObject = JObject.Parse(rs);
            if(jObject != null)
            {

                if (this.firstRun)
                {
                    foreach (var comment in jObject["comments"]["data"])
                    {
                        comments.Add(comment.ToString());
                    }

                    if (jObject["comments"]["paging"] != null && jObject["comments"]["paging"]["next"] != null)
                    {
                        next_url = jObject["comments"]["paging"]["next"].ToString();
                    }

                    if (!string.IsNullOrEmpty(next_url))
                    {
                        apiUrl = next_url;
                    }
                    else
                    {
                        apiUrl = null;
                    }
                }
                else
                {
                    foreach (var comment in jObject["data"])
                    {
                        comments.Add(comment.ToString());
                    }

                    if (jObject["paging"] != null && jObject["paging"]["next"] != null)
                    {
                        next_url = jObject["paging"]["next"].ToString();
                    }

                    if (!string.IsNullOrEmpty(next_url))
                    {
                        apiUrl = next_url;
                    }
                    else
                    {
                        apiUrl = null;
                    }
                }
            }

            if (!string.IsNullOrEmpty(apiUrl))
            {
                this.firstRun = false;
                this.saveTrue = true;
            }


            return apiUrl;
        }

        private void SaveToFile(List<string> comments, string idFile)
        {
            string dateTimeString = DateTime.Now.ToString("yyyy_MM_dd");
            string fileNameWithDateTime = $"{idFile}-{dateTimeString}.txt";

            string txtPathUid = Path.Combine(Environment.CurrentDirectory, "log", "dataUid\\full", fileNameWithDateTime);

            if (!File.Exists(txtPathUid)) Helpers.Common.CreateFile(txtPathUid);

            using (StreamWriter file = new StreamWriter(txtPathUid, true, System.Text.Encoding.UTF8))
            {
                foreach (var comment in comments)
                {
                    JObject obj = JObject.Parse(comment);

                    string commentId = obj["from"]["id"].ToString();
                    string commentName = obj["from"]["name"].ToString();
                    string commentMessage = obj["message"].ToString().Replace("\n", "\\n");
                    string commentLine = $"{commentId}|{commentName}|{commentMessage}\n";
                    file.Write(commentLine);
                    UpdateRichTextBox(commentLine);
                }
            }

            string txtPathUid2 = Path.Combine(Environment.CurrentDirectory, "log", "dataUid\\uid", fileNameWithDateTime);
            using (StreamWriter file2 = new StreamWriter(txtPathUid2, true, System.Text.Encoding.UTF8))
            {
                foreach (var comment in comments)
                {
                    JObject obj = JObject.Parse(comment);

                    string commentId = obj["from"]["id"].ToString();
                    string commentName = obj["from"]["name"].ToString();
                    string commentMessage = obj["message"].ToString().Replace("\n", "\\n");
                    string commentLine = $"{commentId}\n";
                    file2.Write(commentLine);
                }
            }
        }
        private void UpdateRichTextBox(string text)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText(text);
                    UpdateLabel();
                }));
            }
            else
            {
                txtLog.AppendText(text);
                UpdateLabel();
            }
        }

        private void UpdateLabel()
        {
            if (lbtotalcmt.InvokeRequired)
            {
                lbtotalcmt.Invoke(new Action(() =>
                {
                    lbtotalcmt.Text = txtLog.Lines.Length.ToString();
                }));
            }
            else
            {
                lbtotalcmt.Text = txtLog.Lines.Length.ToString();
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

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.CurrentDirectory + "\\log\\dataUid");
            }
            catch
            {
            }
        }

        private void linkgetid_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://id.traodoisub.com/");
            Process.Start(sInfo);
        }
    }

}

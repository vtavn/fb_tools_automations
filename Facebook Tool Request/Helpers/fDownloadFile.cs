using Facebook_Tool_Request.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    public partial class fDownloadFile : Form
    {
        public fDownloadFile(string fileLink)
        {
            this.InitializeComponent();
            this.fileLink = fileLink;
            this.fileName = Path.GetFileName(this.fileLink);
        }

        private void fDownloadFile_Load(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                bool flag = InternetConnection.IsConnectedToInternet();
                if (flag)
                {
                    Uri uri = new Uri(this.fileLink);
                    Common.DeleteFolder("download");
                    Common.CreateFolder("download");
                    this.StartDownload(uri, "download\\" + this.fileName);
                }
                else
                {
                    MessageBoxHelper.ShowMessageBox("Không có kết nối Internet, vui lòng kiểm tra lại!", 1);
                    base.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox("Error: " + ex.Message, 2);
                base.Close();
            }
        }
        public void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);
            this.CopyAll(source, target);
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            int num = 1;
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                Application.DoEvents();
                fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
                num++;
            }
            DirectoryInfo[] directories = source.GetDirectories();
            foreach (DirectoryInfo directoryInfo in directories)
            {
                DirectoryInfo target2 = target.CreateSubdirectory(directoryInfo.Name);
                this.CopyAll(directoryInfo, target2);
            }
        }
        private void StartDownload(Uri uri, string pathFile)
        {
            new Thread(delegate ()
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += this.client_DownloadProgressChanged;
                webClient.DownloadFileCompleted += this.client_DownloadFileCompleted;
                webClient.DownloadFileAsync(uri, pathFile);
            })
            {
                IsBackground = true
            }.Start();
        }
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(delegate ()
            {
                double num = double.Parse(e.BytesReceived.ToString());
                double num2 = double.Parse(e.TotalBytesToReceive.ToString());
                double d = num / num2 * 100.0;
                this.lblproccess.Text = string.Format("Downloading ({0}%)...", int.Parse(Math.Truncate(d).ToString()));
                this.progressBar1.Value = int.Parse(Math.Truncate(d).ToString());
            }));
        }
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(delegate ()
            {
                this.lblproccess.Text = ("Unzip file...");
            }));
            try
            {
                ZipFile.ExtractToDirectory("./download/" + this.fileName, "./download/");
                Common.DeleteFile("./download/" + this.fileName);
                this.Copy("download", "./");
                bool flag = File.Exists("download\\driver\\win32\\chromedriver.exe");
                if (flag)
                {
                    this.Copy("download\\driver\\win32", "./");
                }
                else
                {
                    this.Copy("download", "./");
                }
                Common.DeleteFolder("download");
                base.BeginInvoke(new MethodInvoker(delegate ()
                {
                    base.Close();
                }));
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox("Error: " + ex.Message, 2);
                base.BeginInvoke(new MethodInvoker(delegate ()
                {
                    base.Close();
                }));
            }
        }
        private string fileLink = "";
        private string fileName = "";

    }
}

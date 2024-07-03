using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fShowProgressBar : Form
    {
        public fShowProgressBar(List<string> lstPathFolder)
        {
            this.InitializeComponent();
            this.lstPathFolder = lstPathFolder;
        }

        private void fShowProgressBar_Load(object sender, EventArgs e)
        {
            try
            {
                new Thread(delegate ()
                {
                    int count = 0;
                    int total = this.lstPathFolder.Count;
                    base.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        this.lblproccess.Text = string.Format(Language.GetValue("Đang copy, vui lòng chờ ({0}/{1})..."), count, total);
                    }));
                    for (int i = 0; i < this.lstPathFolder.Count; i++)
                    {
                        string sourceDirName = this.lstPathFolder[i].Split(new char[]
                        {
                            '|'
                        })[0];
                        string destDirName = this.lstPathFolder[i].Split(new char[]
                        {
                            '|'
                        })[1];
                        bool flag = FileHelper.DirectoryCopy(sourceDirName, destDirName, true);
                        if (flag)
                        {
                            int count2 = count;
                            count = count2 + 1;
                        }
                        double percentage = (double)count * 1.0 / (double)total * 100.0;
                        base.BeginInvoke(new MethodInvoker(delegate ()
                        {
                            this.lblproccess.Text = string.Format(Language.GetValue("Đang copy, vui lòng chờ ({0}/{1})..."), count, total);
                            this.progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
                        }));
                    }
                    base.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        base.Close();
                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox("Error: " + ex.Message, 2);
                base.Close();
            }
        }

        private List<string> lstPathFolder = new List<string>();
    }
}

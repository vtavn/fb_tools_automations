using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fCoppy : Form
    {
        public fCoppy(List<string> lstAcc)
        {
            this.InitializeComponent();
            this.lst = lstAcc;
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
        }

        private void button1_Click(object sender, EventArgs e)
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

        private List<ComboBox> lstCbbDinhDang;

        private List<string> lst = new List<string>();

        private void fCoppy_Load(object sender, EventArgs e)
        {
            this.LoadDinhDang();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }
        private void LoadDinhDang()
        {
            try
            {
                bool flag = File.Exists("settings\\format_copy.txt");
                if (flag)
                {
                    string text = File.ReadAllText("settings\\format_copy.txt");
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
                File.WriteAllText("settings\\format_copy.txt", text);
            }
            catch
            {
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                int num = 0;
                for (int i = this.lstCbbDinhDang.Count - 1; i >= 0; i--)
                {
                    bool flag = this.lstCbbDinhDang[i].SelectedIndex != -1;
                    if (flag)
                    {
                        num = i + 1;
                        break;
                    }
                }
                bool flag2 = num == 0;
                if (flag2)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("vui lòng chọn định dạng cần copy!"), 3);
                }
                else
                {
                    this.SaveDinhDang();
                    for (int j = 0; j < this.lst.Count; j++)
                    {
                        string text = "";
                        string[] array = this.lst[j].Split(new char[]
                        {
                            '|'
                        });
                        for (int k = 0; k < this.lstCbbDinhDang.Count; k++)
                        {
                            bool flag3 = this.lstCbbDinhDang[k].SelectedIndex != -1;
                            if (flag3)
                            {
                                text += array[this.lstCbbDinhDang[k].SelectedIndex];
                            }
                            text += "|";
                        }
                        text = text.TrimEnd(new char[]
                        {
                            '|'
                        });
                        for (int l = text.Split(new char[]
                        {
                            '|'
                        }).Count<string>(); l < num; l++)
                        {
                            text += "|";
                        }
                        list.Add(text);
                    }
                    string text2 = string.Join("\r\n", list);
                    Clipboard.SetText(text2);
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Copy thành công!"), 1);
                    base.Close();
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }
    }
}

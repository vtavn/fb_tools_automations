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

namespace Facebook_Tool_Request.core.fTools
{
    public partial class fTienIchLocTrung : Form
    {
        public fTienIchLocTrung()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnSelectedFile_Click(object sender, EventArgs e)
        {
            this.txtNhapTuFile.Text = Helpers.Common.SelectFile("Chọn File txt", "txt Files (*.txt)|*.txt|");
        }

        private void btnFilters_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> source = new List<string>();
                string text = this.txtNhapTuFile.Text.Trim();
                bool @checked = this.rbNhapTuFile.Checked;
                if (@checked)
                {
                    bool flag = text.EndsWith(".txt");
                    if (flag)
                    {
                        bool flag2 = !File.Exists(text);
                        if (flag2)
                        {
                            MessageBoxHelper.ShowMessageBox(Language.GetValue("File dữ liệu nhập không tồn tại!"), 2);
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("File nhập vào chỉ hỗ trợ đối với File (.txt)!"), 2);
                    }
                }
                bool checked2 = this.rbNhapTuFile.Checked;
                if (checked2)
                {
                    source = File.ReadAllLines(text).ToList<string>();
                }
                else
                {
                    bool checked3 = this.rbTuNhap.Checked;
                    if (checked3)
                    {
                        source = this.txtInput.Lines.ToList<string>();
                    }
                }
                List<string> list = source.Distinct<string>().ToList<string>();
                this.txtOutput.Lines = list.ToArray();
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã lọc xong!"), 1);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }

        private void rbNhapTuFile_CheckedChanged(object sender, EventArgs e)
        {
            this.txtNhapTuFile.Enabled = this.rbNhapTuFile.Checked;
            this.btnSelectedFile.Enabled = this.rbNhapTuFile.Checked;
        }

        private void rbTuNhap_CheckedChanged(object sender, EventArgs e)
        {
            this.txtInput.Enabled = this.rbTuNhap.Checked;
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txtInput.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.rbTuNhap.Text = string.Format(Language.GetValue("Tự nhập ({0})"), list.Count.ToString());
            }
            catch
            {
            }
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.txtOutput.Lines.ToList<string>();
                list = Helpers.Common.RemoveEmptyItems(list);
                this.gunaGroupBox2.Text = string.Format("Output ({0})", list.Count.ToString());
                this.btnSave.Enabled = true;
            }
            catch
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string[] lines = txtOutput.Lines;
            try
            {
                File.WriteAllLines(this.txtNhapTuFile.Text.Trim(), lines);
                MessageBoxHelper.ShowMessageBox("Lưu thành công!", 1);
                txtOutput.Text = "";
                txtNhapTuFile.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }
    }
}

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
    public partial class fNhapDuLieu2 : Form
    {
        private string linkPathFolder = "";

        private Random rd = new Random();

        private bool isAdd = false;

        public fNhapDuLieu2(string linkPathFolder, string title)
        {
            this.InitializeComponent();
            this.linkPathFolder = linkPathFolder;
            this.lblTitle.Text = title;
        }

        private void fNhapDuLieu2_Load(object sender, EventArgs e)
        {
            Helpers.Common.CreateFolder(this.linkPathFolder);
            this.LoadDsFile(this.linkPathFolder);
        }
        private void LoadContentFileFromDtgv(int row)
        {
            this.txtContent.Lines = File.ReadAllLines(DatagridviewHelper.GetStatusDataGridView(this.dtgvDanhSach, row, "cName"));
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.dtgvDanhSach.SelectedRows[0].Cells["cName"].Value.ToString();
                Helpers.Common.CreateFile(text);
                bool flag = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn hủy?")) == DialogResult.Yes;
                if (flag)
                {
                    bool flag2 = this.isAdd;
                    if (flag2)
                    {
                        File.Delete(text);
                    }
                    this.rControl(2);
                    this.LoadDsFile(this.linkPathFolder);
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.dtgvDanhSach.SelectedRows[0].Cells["cName"].Value.ToString();
                Helpers.Common.CreateFile(text);
                bool flag = this.txtContent.Text.Trim() == "";
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng nhập nội dung cần lưu!"), 3);
                }
                else
                {
                    bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn lưu lại?")) == DialogResult.Yes;
                    if (flag2)
                    {
                        File.WriteAllLines(text, this.txtContent.Lines);
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Lưu thành công!"), 1);
                        this.rControl(2);
                    }
                }
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Đã có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }
        private void rControl(int type)
        {
            bool flag = type == 1;
            if (flag)
            {
                this.dtgvDanhSach.Enabled = false;
                this.btnAdd.Enabled = false;
                this.btnSave.Enabled = true;
                this.btnHuy.Enabled = true;
                this.txtContent.ReadOnly = false;
                this.txtContent.Focus();
            }
            else
            {
                this.dtgvDanhSach.Enabled = true;
                this.btnAdd.Enabled = true;
                this.btnSave.Enabled = false;
                this.btnHuy.Enabled = false;
                this.txtContent.ReadOnly = true;
            }
        }
        private void LoadDsFile(string linkPathFolder)
        {
            int num = -1;
            int rowCount = this.dtgvDanhSach.RowCount;
            int num2 = rowCount;
            if (num2 != 0)
            {
                if (num2 != 1)
                {
                    num = this.dtgvDanhSach.SelectedRows[0].Index;
                }
                else
                {
                    num = 0;
                }
            }
            this.dtgvDanhSach.Rows.Clear();
            List<string> list = Directory.GetFiles(linkPathFolder).ToList<string>();
            for (int i = 0; i < list.Count; i++)
            {
                this.dtgvDanhSach.Rows.Add(new object[]
                {
                    i + 1,
                    list[i],
                    Language.GetValue("Chi tiết"),
                    Language.GetValue("Sửa"),
                    Language.GetValue("Xóa")
                });
            }
            bool flag = num == -1 && this.dtgvDanhSach.RowCount > 0;
            if (flag)
            {
                num = 0;
            }
            else
            {
                bool flag2 = num + 1 > this.dtgvDanhSach.RowCount;
                if (flag2)
                {
                    num = this.dtgvDanhSach.RowCount - 1;
                }
            }
            bool flag3 = num == -1;
            if (flag3)
            {
                this.txtContent.Text = "";
            }
            else
            {
                Helpers.Common.ClearSelectedOnDatagridview(this.dtgvDanhSach);
                this.dtgvDanhSach.Rows[num].Selected = true;
                this.LoadContentFileFromDtgv(num);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string text;
                bool flag;
                do
                {
                    text = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_") + Helpers.Common.CreateRandomString(3, this.rd) + ".txt";
                    flag = File.Exists(text);
                }
                while (flag);
                string text2 = this.linkPathFolder + "\\" + text;
                Helpers.Common.CreateFile(text2);
                this.txtContent.Lines = File.ReadAllLines(text2);
                this.dtgvDanhSach.Rows.Add(new object[]
                {
                    this.dtgvDanhSach.RowCount + 1,
                    text2,
                    Language.GetValue("Chi tiết"),
                    Language.GetValue("Sửa"),
                    Language.GetValue("Xóa")
                });
                Helpers.Common.ClearSelectedOnDatagridview(this.dtgvDanhSach);
                this.dtgvDanhSach.Rows[this.dtgvDanhSach.RowCount - 1].Selected = true;
                this.rControl(1);
                this.isAdd = true;
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox("Đã có lỗi xảy ra, vui lòng thử lại sau!", 2);
            }
        }

        private void dtgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            bool flag = dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0;
            if (flag)
            {
                string name = dataGridView.Columns[e.ColumnIndex].Name;
                string a = name;
                if (!(a == "cChiTiet"))
                {
                    if (!(a == "cSua"))
                    {
                        if (a == "cXoa")
                        {
                            bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có muốn xóa?")) == DialogResult.Yes;
                            if (flag2)
                            {
                                File.Delete(DatagridviewHelper.GetStatusDataGridView(this.dtgvDanhSach, e.RowIndex, "cName"));
                                this.LoadDsFile(this.linkPathFolder);
                            }
                        }
                    }
                    else
                    {
                        this.txtContent.Lines = File.ReadAllLines(DatagridviewHelper.GetStatusDataGridView(this.dtgvDanhSach, e.RowIndex, "cName"));
                        this.isAdd = false;
                        this.rControl(1);
                    }
                }
                else
                {
                    this.LoadContentFileFromDtgv(e.RowIndex);
                }
            }
        }

        private void dtgvDanhSach_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            string name = this.dtgvDanhSach.Columns[e.ColumnIndex].Name;
            bool flag = name != "cChiTiet" && name != "cSua" && name != "cXoa";
            if (flag)
            {
                this.dtgvDanhSach.Cursor = Cursors.Default;
            }
            else
            {
                bool flag2 = e.RowIndex < this.dtgvDanhSach.RowCount;
                if (flag2)
                {
                    this.dtgvDanhSach.Cursor = Cursors.Hand;
                }
            }
        }
    }
}

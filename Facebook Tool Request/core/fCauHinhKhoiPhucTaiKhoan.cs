using Facebook_Tool_Request.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.core
{
    public partial class fCauHinhKhoiPhucTaiKhoan : Form
    {
        public static bool isOK = false;
        public static int typeThuMuc = 0;
        public static string idFile = "";
        private int indexOld = 0;

        public fCauHinhKhoiPhucTaiKhoan()
        {
            this.InitializeComponent();
            this.Load_cbbThuMuc();
            fCauHinhKhoiPhucTaiKhoan.isOK = false;
            fCauHinhKhoiPhucTaiKhoan.typeThuMuc = 0;
            fCauHinhKhoiPhucTaiKhoan.idFile = "";
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool @checked = this.rbThuMucKhac.Checked;
            if (@checked)
            {
                bool flag = this.cbbThuMuc.SelectedIndex == -1;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm thư mục để lưu tài khoản!"), 3);
                    return;
                }
                fCauHinhKhoiPhucTaiKhoan.typeThuMuc = 1;
                fCauHinhKhoiPhucTaiKhoan.idFile = this.cbbThuMuc.SelectedValue.ToString();
            }
            fCauHinhKhoiPhucTaiKhoan.isOK = true;
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void fCauHinhKhoiPhucTaiKhoan_Load(object sender, EventArgs e)
        {
            this.rbThuMucKhac_CheckedChanged(null, null);
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
            }
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

        private void rbThuMucKhac_CheckedChanged(object sender, EventArgs e)
        {
            this.plThuMucKhac.Enabled = this.rbThuMucKhac.Checked;
        }
    }
}

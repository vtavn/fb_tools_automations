using Facebook_Tool_Request.core.KichBanPage;
using Emgu.CV.Flann;
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

namespace Facebook_Tool_Request.core.KichBanPage
{
    public partial class fThemKichBanPage : Form
    {
        private int type = 0;
        private string id = "";
        private string idNew = "";
        public fThemKichBanPage(int type, string id = "")
        {
            this.InitializeComponent();
            this.type = type;
            this.id = id;
            bool flag = type == 1;
            if (flag)
            {
                this.btnAdd.Text = "Cập nhật";
                this.txtTen.Text = InteractSQL.GetKichBanById(id).Rows[0]["TenKichBan"].ToString();
                LoadHanhDong();
            }
            if (string.IsNullOrEmpty(id))
            {
                var getKb = InteractSQL.GetKichBanMoi();
                string stt = "0";
                if (getKb != null && getKb.Rows.Count > 0)
                {
                    stt = getKb.Rows[0]["Id_KichBan"].ToString();
                }
                this.idNew = (int.Parse(stt) + 1).ToString();
                this.txtTen.Text = "Kịch bản " + (int.Parse(stt) + 1).ToString();
            }
        }
        private void LoadHanhDong()
        {
            try
            {
                this.dtgvHanhDong.Rows.Clear();
                string idKichBan = this.id.ToString();

                if (!string.IsNullOrEmpty(idKichBan)) { 
                    DataTable allHanhDongByKichBan = InteractSQL.GetAllHanhDongByKichBan(idKichBan);
                    for (int i = 0; i < allHanhDongByKichBan.Rows.Count; i++)
                    {
                        this.dtgvHanhDong.Rows.Add(new object[]
                        {
                            this.dtgvHanhDong.RowCount + 1,
                            allHanhDongByKichBan.Rows[i]["Id_HanhDong"],
                            allHanhDongByKichBan.Rows[i]["TenHanhDong"],
                            allHanhDongByKichBan.Rows[i]["MoTa"]
                        });
                    }
                }
            }
            catch
            {
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = this.txtTen.Text.Trim();
            bool flag = text == "";
            if (flag)
            {
                MessageBoxHelper.ShowMessageBox("Vui lòng nhập tên kịch bản!", 2);
            }
            else
            {
                bool action = this.type == 0;
                if (action)
                {
                    bool add = InteractSQL.InsertKichBan(text);
                    if (add)
                    {
                        //add thành công
                        string id_kichban_moi = InteractSQL.GetKichBanMoi().Rows[0]["Id_KichBan"].ToString();
                        MessageBoxHelper.ShowMessageBox("Thêm thành công kịch bản mới!", 1);

                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
                    }
                }
                else
                {
                    bool update = InteractSQL.UpdateKichBan(this.id, text);
                    if (update)
                    {
                        base.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
                    }
                }
            }
        }

        private void thêmHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ThemHanhDong();
        }

        private void ThemHanhDong()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.idNew) && !string.IsNullOrEmpty(this.id))
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm kịch bản trước!", 3);
                }
                else
                {
                    string text = "";
                    if (!string.IsNullOrEmpty(this.idNew))
                    {
                        text = this.idNew;
                    }
                    else
                    {
                        text = this.id;
                    }

                    int count = InteractSQL.GetAllHanhDongByKichBan(text).Rows.Count;
                    Helpers.Common.ShowForm(new fThemHanhDongPage(text));
                    DataTable allHanhDongByKichBan = InteractSQL.GetAllHanhDongByKichBan(text);
                    int count2 = allHanhDongByKichBan.Rows.Count;
                    bool flag2 = count2 > count;
                    if (flag2)
                    {
                        this.dtgvHanhDong.Rows.Add(new object[]
                        {
                            this.dtgvHanhDong.RowCount + 1,
                            allHanhDongByKichBan.Rows[count2 - 1]["Id_HanhDong"],
                            allHanhDongByKichBan.Rows[count2 - 1]["TenHanhDong"],
                            allHanhDongByKichBan.Rows[count2 - 1]["MoTa"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
            }
        }

        private void nhânBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.NhanBanHanhDong();
        }
        private void NhanBanHanhDong()
        {
            try
            {
                bool flag = this.dtgvHanhDong.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm hành động trước!", 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvHanhDong.SelectedRows[0];
                    string id_HanhDong = dataGridViewRow.Cells["cId_HanhDong"].Value.ToString();
                    string str = dataGridViewRow.Cells["cTenHanhDong"].Value.ToString();
                    string text = str + " - Copy";
                    int num = 2;
                    while (InteractSQL.CheckExistTenHanhDong(text))
                    {
                        text = str + string.Format(" - Copy ({0})", num++);
                    }
                    bool flag2 = InteractSQL.DuplicateHanhDong(id_HanhDong, text);
                    if (flag2)
                    {
                        DataTable hanhDongMoi = InteractSQL.GetHanhDongMoi();
                        this.dtgvHanhDong.Rows.Add(new object[]
                        {
                            this.dtgvHanhDong.RowCount + 1,
                            hanhDongMoi.Rows[0]["Id_HanhDong"],
                            hanhDongMoi.Rows[0]["TenHanhDong"],
                            hanhDongMoi.Rows[0]["MoTa"]
                        });
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
                    }
                }
            }
            catch
            {
            }
        }

        private void xoáHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.XoaHanhDong();
        }
        private void XoaHanhDong()
        {
            try
            {
                bool flag = this.dtgvHanhDong.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm hành động trước!", 3);
                }
                else
                {
                    bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion("Bạn có chắc muốn xóa hoạt động này?") == DialogResult.Yes;
                    if (flag2)
                    {
                        DataGridViewRow dataGridViewRow = this.dtgvHanhDong.SelectedRows[0];
                        bool flag3 = InteractSQL.DeleteHanhDongByIdHanhDong(dataGridViewRow.Cells["cId_HanhDong"].Value.ToString());
                        if (flag3)
                        {
                            int index = dataGridViewRow.Index;
                            for (int i = index; i < this.dtgvHanhDong.Rows.Count - 1; i++)
                            {
                                this.DoiChoDgv(ref this.dtgvHanhDong, i, i + 1);
                            }
                            this.dtgvHanhDong.Rows.RemoveAt(this.dtgvHanhDong.Rows.Count - 1);
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
            }
        }
        public void DoiChoDgv(ref DataGridView dgv, int oldRow, int newRow)
        {
            for (int i = 1; i < dgv.Columns.Count; i++)
            {
                string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(dgv, oldRow, i);
                DatagridviewHelper.SetStatusDataGridView(dgv, oldRow, i, DatagridviewHelper.GetStatusDataGridView(dgv, newRow, i));
                DatagridviewHelper.SetStatusDataGridView(dgv, newRow, i, statusDataGridView);
            }
        }

        private void sửaHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SuaHanhDong();
        }
        private void SuaHanhDong()
        {
            try
            {
                bool flag = this.dtgvHanhDong.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox("Vui lòng thêm hành động trước!", 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvHanhDong.SelectedRows[0];
                    string text = dataGridViewRow.Cells["cId_HanhDong"].Value.ToString();
                    DataTable hanhDongById = InteractSQL.GetHanhDongById(text);
                    string str = "Facebook_Tool_Request.core.KichBanPage.f";
                    object obj = hanhDongById.Rows[0]["TenTuongTac"];
                    string name = str + ((obj != null) ? obj.ToString() : null);
                    Form formByName = Helpers.Common.GetFormByName(name, text);
                    bool flag2 = formByName != null;
                    if (flag2)
                    {
                        Helpers.Common.ShowForm(formByName);
                    }
                    hanhDongById = InteractSQL.GetHanhDongById(text);
                    DatagridviewHelper.SetStatusDataGridView(this.dtgvHanhDong, dataGridViewRow.Index, "cTenHanhDong", hanhDongById.Rows[0]["TenHanhDong"].ToString());
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox("Có lỗi, vui lòng thử lại sau!", 2);
            }
        }

    }
}

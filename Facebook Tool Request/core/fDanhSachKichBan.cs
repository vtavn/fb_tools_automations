using core.KichBan;
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
    public partial class fDanhSachKichBan : Form
    {
        public fDanhSachKichBan(string kickBan)
        {
            this.InitializeComponent();
            this.kichBan = kickBan;
        }

        private void dtgvKichBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = e.RowIndex > -1;
            if (flag)
            {
                this.LoadHanhDong();
            }
        }

        private void LoadHanhDong()
        {
            try
            {
                this.dtgvHanhDong.Rows.Clear();
                bool flag = this.dtgvKichBan.RowCount == 0;
                if (!flag)
                {
                    DataGridViewRow dataGridViewRow = this.dtgvKichBan.SelectedRows[0];
                    string idKichBan = dataGridViewRow.Cells["cId_KichBan"].Value.ToString();
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

        private void thêmKịchBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ThemKichBan();
        }

        private void ThemKichBan()
        {
            try
            {
                string a = "";
                try
                {
                    a = InteractSQL.GetKichBanMoi().Rows[0]["Id_KichBan"].ToString();
                }
                catch
                {
                }
                Helpers.Common.ShowForm(new fThemKichBan(0, ""));
                DataTable kichBanMoi = InteractSQL.GetKichBanMoi();
                string b = "";
                try
                {
                    b = kichBanMoi.Rows[0]["Id_KichBan"].ToString();
                }
                catch
                {
                }
                bool flag = a != b;
                if (flag)
                {
                    this.dtgvKichBan.Rows.Add(new object[]
                    {
                        this.dtgvKichBan.RowCount + 1,
                        kichBanMoi.Rows[0]["Id_KichBan"],
                        kichBanMoi.Rows[0]["TenKichBan"]
                    });
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }
        private void LoadKichBan(string kichBan = "")
        {
            try
            {
                this.dtgvKichBan.Rows.Clear();
                DataTable allKichBan = InteractSQL.GetAllKichBan();
                bool flag = allKichBan.Rows.Count > 0;
                if (flag)
                {
                    for (int i = 0; i < allKichBan.Rows.Count; i++)
                    {
                        DataRow dataRow = allKichBan.Rows[i];
                        this.dtgvKichBan.Rows.Add(new object[]
                        {
                            i + 1,
                            dataRow["Id_KichBan"],
                            dataRow["TenKichBan"]
                        });
                    }
                }
                bool flag2 = kichBan != "";
                if (flag2)
                {
                    for (int j = 0; j < this.dtgvKichBan.RowCount; j++)
                    {
                        bool flag3 = DatagridviewHelper.GetStatusDataGridView(this.dtgvKichBan, j, "cId_KichBan") == kichBan;
                        if (flag3)
                        {
                            this.dtgvKichBan.Rows[j].Selected = true;
                            break;
                        }
                    }
                }
                this.LoadHanhDong();
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
            }
        }

        private void dtgvKichBan_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            Keys keys = keyCode;
            if (keys <= Keys.Delete)
            {
                if (keys != Keys.Insert)
                {
                    if (keys != Keys.Delete)
                    {
                        return;
                    }
                    this.XoaKichBan();
                    return;
                }
            }
            else
            {
                if (keys == Keys.D)
                {
                    bool flag = e.Modifiers == Keys.Control;
                    if (flag)
                    {
                        this.NhanBanKichBan();
                    }
                    return;
                }
                switch (keys)
                {
                    case Keys.F1:
                        break;
                    case Keys.F2:
                        this.SuaKichBan();
                        return;
                    case Keys.F3:
                    case Keys.F4:
                        return;
                    case Keys.F5:
                        this.LoadKichBan("");
                        return;
                    default:
                        return;
                }
            }
            this.ThemKichBan();
        }

        private void XoaKichBan()
        {
            try
            {
                bool flag = this.dtgvKichBan.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm kịch bản trước!"), 3);
                }
                else
                {
                    bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có chắc muốn xóa kịch bản này?")) == DialogResult.Yes;
                    if (flag2)
                    {
                        DataGridViewRow dataGridViewRow = this.dtgvKichBan.SelectedRows[0];
                        bool flag3 = InteractSQL.DeleteKichBan(dataGridViewRow.Cells["cId_KichBan"].Value.ToString());
                        if (flag3)
                        {
                            int index = dataGridViewRow.Index;
                            for (int i = index; i < this.dtgvKichBan.Rows.Count - 1; i++)
                            {
                                this.DoiChoDgv(ref this.dtgvKichBan, i, i + 1);
                            }
                            this.dtgvKichBan.Rows.RemoveAt(this.dtgvKichBan.Rows.Count - 1);
                            this.LoadHanhDong();
                        }
                        else
                        {
                            MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }

        private void SuaKichBan()
        {
            try
            {
                bool flag = this.dtgvKichBan.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm kịch bản trước!"), 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvKichBan.SelectedRows[0];
                    string id = dataGridViewRow.Cells["cId_KichBan"].Value.ToString();
                    Helpers.Common.ShowForm(new fThemKichBan(1, id));
                    string status = InteractSQL.GetKichBanById(id).Rows[0]["TenKichBan"].ToString();
                    DatagridviewHelper.SetStatusDataGridView(this.dtgvKichBan, dataGridViewRow.Index, "cTenKichBan", status);
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }
        private void NhanBanKichBan()
        {
            try
            {
                bool flag = this.dtgvKichBan.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm kịch bản trước!"), 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvKichBan.SelectedRows[0];
                    string id_KichBanCu = dataGridViewRow.Cells["cId_KichBan"].Value.ToString();
                    string str = dataGridViewRow.Cells["cTenKichBan"].Value.ToString();
                    string text = str + " - Copy";
                    int num = 2;
                    while (InteractSQL.CheckExistTenKichBan(text))
                    {
                        text = str + string.Format(" - Copy ({0})", num++);
                    }
                    bool flag2 = InteractSQL.DuplicateKichBan(id_KichBanCu, text);
                    if (flag2)
                    {
                        DataTable kichBanMoi = InteractSQL.GetKichBanMoi();
                        this.dtgvKichBan.Rows.Add(new object[]
                        {
                            this.dtgvKichBan.RowCount + 1,
                            kichBanMoi.Rows[0]["Id_KichBan"],
                            kichBanMoi.Rows[0]["TenKichBan"]
                        });
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                    }
                }
            }
            catch
            {
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

        private void fDanhSachKichBan_Load(object sender, EventArgs e)
        {
            this.LoadKichBan(this.kichBan);
        }

        private string kichBan = "";

        private void sửaTênKịchBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SuaKichBan();
        }

        private void xoáKịchBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.XoaKichBan();
        }

        private void nhânBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.NhanBanKichBan();
        }

        private void dtgvHanhDong_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            Keys keys = keyCode;
            if (keys <= Keys.Delete)
            {
                if (keys != Keys.Insert)
                {
                    if (keys != Keys.Delete)
                    {
                        return;
                    }
                    this.XoaHanhDong();
                    return;
                }
            }
            else
            {
                if (keys == Keys.D)
                {
                    bool flag = e.Modifiers == Keys.Control;
                    if (flag)
                    {
                        this.NhanBanHanhDong();
                    }
                    return;
                }
                switch (keys)
                {
                    case Keys.F1:
                        break;
                    case Keys.F2:
                        this.SuaHanhDong();
                        return;
                    case Keys.F3:
                    case Keys.F4:
                        return;
                    case Keys.F5:
                        this.LoadHanhDong();
                        return;
                    default:
                        return;
                }
            }
            this.ThemHanhDong();
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
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                    }
                }
            }
            catch
            {
            }
        }

        private void XoaHanhDong()
        {
            try
            {
                bool flag = this.dtgvHanhDong.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm hành động trước!"), 3);
                }
                else
                {
                    bool flag2 = MessageBoxHelper.ShowMessageBoxWithQuestion(Language.GetValue("Bạn có chắc muốn xóa hoạt động này?")) == DialogResult.Yes;
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
                            MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(null, ex, "");
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }

        private void ThemHanhDong()
        {
            try
            {
                bool flag = this.dtgvKichBan.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm kịch bản trước!"), 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvKichBan.SelectedRows[0];
                    string text = dataGridViewRow.Cells["cId_KichBan"].Value.ToString();
                    int count = InteractSQL.GetAllHanhDongByKichBan(text).Rows.Count;
                    Helpers.Common.ShowForm(new fThemHanhDong(text));
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
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }

        private void SuaHanhDong()
        {
            try
            {
                bool flag = this.dtgvHanhDong.RowCount == 0;
                if (flag)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thêm hành động trước!"), 3);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = this.dtgvHanhDong.SelectedRows[0];
                    string text = dataGridViewRow.Cells["cId_HanhDong"].Value.ToString();
                    DataTable hanhDongById = InteractSQL.GetHanhDongById(text);
                    string str = "Facebook_Tool_Request.core.fKichBan.f";
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
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi, vui lòng thử lại sau!"), 2);
            }
        }

        private void thêmHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ThemHanhDong();
        }

        private void sửaHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SuaHanhDong();
        }

        private void xoáHànhĐộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.XoaHanhDong();
        }

        private void nhânBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.NhanBanHanhDong();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = this.dtgvHanhDong.SelectedRows[0].Index;
            bool flag = index == 0;
            if (!flag)
            {
                string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(this.dtgvHanhDong, index - 1, "cId_HanhDong");
                string statusDataGridView2 = DatagridviewHelper.GetStatusDataGridView(this.dtgvHanhDong, index, "cId_HanhDong");
                bool flag2 = statusDataGridView + statusDataGridView2 != "";
                if (flag2)
                {
                    bool flag3 = InteractSQL.UpdateThuTuHanhDong(statusDataGridView, statusDataGridView2);
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        this.DoiChoDgv(ref this.dtgvHanhDong, index, index - 1);
                        this.dtgvHanhDong.Rows[index - 1].Selected = true;
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi xảy ra, vui lòng thử lại sau!"), 2);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = this.dtgvHanhDong.SelectedRows[0].Index;
            bool flag = index == this.dtgvHanhDong.RowCount - 1;
            if (!flag)
            {
                string statusDataGridView = DatagridviewHelper.GetStatusDataGridView(this.dtgvHanhDong, index + 1, "cId_HanhDong");
                string statusDataGridView2 = DatagridviewHelper.GetStatusDataGridView(this.dtgvHanhDong, index, "cId_HanhDong");
                bool flag2 = statusDataGridView + statusDataGridView2 != "";
                if (flag2)
                {
                    bool flag3 = InteractSQL.UpdateThuTuHanhDong(statusDataGridView, statusDataGridView2);
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        this.DoiChoDgv(ref this.dtgvHanhDong, index, index + 1);
                        this.dtgvHanhDong.Rows[index + 1].Selected = true;
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi xảy ra, vui lòng thử lại sau!"), 2);
                    }
                }
            }
        }

        private void btnClosed_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

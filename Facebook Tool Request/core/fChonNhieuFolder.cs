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
    public partial class fChonNhieuFolder : Form
    {
        public fChonNhieuFolder(bool isFromBin = false)
        {
            this.InitializeComponent();
            fChonNhieuFolder.isAdd = false;
            this.isFromBin = isFromBin;
        }

        private void fClearProfile_Load(object sender, EventArgs e)
        {
            bool flag = this.isFromBin;
            if (flag)
            {
                bool flag2 = fChonNhieuFolder.lstChooseIdFilesFromBin == null;
                if (flag2)
                {
                    fChonNhieuFolder.lstChooseIdFilesFromBin = new List<string>();
                }
                this.LoadListFiles(fChonNhieuFolder.lstChooseIdFilesFromBin);
            }
            else
            {
                bool flag3 = fChonNhieuFolder.lstChooseIdFiles == null;
                if (flag3)
                {
                    fChonNhieuFolder.lstChooseIdFiles = new List<string>();
                }
                this.LoadListFiles(fChonNhieuFolder.lstChooseIdFiles);
            }
        }

        private void LoadListFiles(List<string> lstIdFile = null)
        {
            try
            {
                bool flag = this.isFromBin;
                DataTable dataTable;
                if (flag)
                {
                    dataTable = CommonSQL.GetAllFilesFromDatabaseForBin(false);
                }
                else
                {
                    dataTable = CommonSQL.GetAllFilesFromDatabase(false);
                }
                bool flag2 = lstIdFile != null && lstIdFile.Count > 0;
                if (flag2)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        bool flag3 = lstIdFile.Contains(dataTable.Rows[i]["id"].ToString());
                        if (flag3)
                        {
                            this.dtgvAcc.Rows.Add(new object[]
                            {
                                true,
                                i + 1,
                                dataTable.Rows[i]["id"],
                                dataTable.Rows[i]["name"]
                            });
                        }
                        else
                        {
                            this.dtgvAcc.Rows.Add(new object[]
                            {
                                false,
                                i + 1,
                                dataTable.Rows[i]["id"],
                                dataTable.Rows[i]["name"]
                            });
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        this.dtgvAcc.Rows.Add(new object[]
                        {
                            false,
                            j + 1,
                            dataTable.Rows[j]["id"],
                            dataTable.Rows[j]["name"]
                        });
                    }
                }
                this.UpdateSelectCountRecord();
                this.UpdateTotalCountRecord();
                bool flag4 = this.CountSelectRow() == this.dtgvAcc.RowCount;
                if (flag4)
                {
                    this.checkBox1.Checked = true;
                }
                else
                {
                    this.checkBox1.Checked = false;
                }
            }
            catch (Exception ex)
            {
                Helpers.Common.ExportError(ex, "LoadListFiles");
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Có lỗi xảy ra, vui lòng thử lại sau!"), 2);
            }
        }

        private void UpdateSelectCountRecord()
        {
            try
            {
                this.lblCountChoose.Text = this.CountSelectRow().ToString();
            }
            catch
            {
            }
        }

        private void DtgvAcc_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = e.ColumnIndex == 0;
            if (flag)
            {
                this.UpdateSelectCountRecord();
                bool flag2 = this.CountSelectRow() == this.dtgvAcc.RowCount;
                if (flag2)
                {
                    this.checkBox1.Checked = true;
                }
                else
                {
                    this.checkBox1.Checked = false;
                }
            }
        }

        private void UpdateTotalCountRecord()
        {
            try
            {
                this.lblCountTotal.Text = this.dtgvAcc.Rows.Count.ToString();
            }
            catch
            {
            }
        }

        private void dtgvAcc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool flag = Convert.ToBoolean(this.dtgvAcc.CurrentRow.Cells["cChose"].Value);
                if (flag)
                {
                    this.dtgvAcc.CurrentRow.Cells["cChose"].Value = false;
                }
                else
                {
                    this.dtgvAcc.CurrentRow.Cells["cChose"].Value = true;
                }
            }
            catch
            {
            }
        }

        private int CountSelectRow()
        {
            int num = 0;
            for (int i = 0; i < this.dtgvAcc.Rows.Count; i++)
            {
                bool flag = Convert.ToBoolean(this.dtgvAcc.Rows[i].Cells["cChose"].Value);
                if (flag)
                {
                    num++;
                }
            }
            return num;
        }

        private void dtgvAcc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = e.ColumnIndex == 0;
            if (flag)
            {
                try
                {
                    bool flag2 = Convert.ToBoolean(this.dtgvAcc.CurrentRow.Cells["cChose"].Value);
                    if (flag2)
                    {
                        this.dtgvAcc.CurrentRow.Cells["cChose"].Value = false;
                    }
                    else
                    {
                        this.dtgvAcc.CurrentRow.Cells["cChose"].Value = true;
                    }
                }
                catch
                {
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            bool @checked = this.checkBox1.Checked;
            if (@checked)
            {
                for (int i = 0; i < this.dtgvAcc.Rows.Count; i++)
                {
                    DatagridviewHelper.SetStatusDataGridView(this.dtgvAcc, i, "cChose", true);
                }
            }
            else
            {
                for (int j = 0; j < this.dtgvAcc.Rows.Count; j++)
                {
                    DatagridviewHelper.SetStatusDataGridView(this.dtgvAcc, j, "cChose", false);
                }
            }
        }

        private bool isFromBin = false;
        public static List<string> lstChooseIdFiles;
        public static List<string> lstChooseIdFilesFromBin;
        public static bool isAdd;

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            fChonNhieuFolder.isAdd = true;
            bool flag = this.isFromBin;
            if (flag)
            {
                fChonNhieuFolder.lstChooseIdFilesFromBin = new List<string>();
                for (int i = 0; i < this.dtgvAcc.Rows.Count; i++)
                {
                    bool flag2 = Convert.ToBoolean(this.dtgvAcc.Rows[i].Cells["cChose"].Value);
                    if (flag2)
                    {
                        fChonNhieuFolder.lstChooseIdFilesFromBin.Add(DatagridviewHelper.GetStatusDataGridView(this.dtgvAcc, i, "cId"));
                    }
                }
                bool flag3 = fChonNhieuFolder.lstChooseIdFilesFromBin.Count == 0;
                if (flag3)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn ít nhất 1 thư mục!"), 2);
                    return;
                }
            }
            else
            {
                fChonNhieuFolder.lstChooseIdFiles = new List<string>();
                for (int j = 0; j < this.dtgvAcc.Rows.Count; j++)
                {
                    bool flag4 = Convert.ToBoolean(this.dtgvAcc.Rows[j].Cells["cChose"].Value);
                    if (flag4)
                    {
                        fChonNhieuFolder.lstChooseIdFiles.Add(DatagridviewHelper.GetStatusDataGridView(this.dtgvAcc, j, "cId"));
                    }
                }
                bool flag5 = fChonNhieuFolder.lstChooseIdFiles.Count == 0;
                if (flag5)
                {
                    MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng chọn ít nhất 1 thư mục!"), 2);
                    return;
                }
            }
            base.Close();
        }
    }
}

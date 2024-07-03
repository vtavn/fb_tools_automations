using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Tool_Request.Helpers
{
    internal class DatagridviewHelper
    {
        public static void SetStatusDataGridViewWithWait(DataGridView dgv, int row, string colName, int timeWait = 0, string status = "Đợi {time} giây...")
        {
            try
            {
                int time_start = Environment.TickCount;
                while ((Environment.TickCount - time_start) / 1000 - timeWait < 0)
                {
                    dgv.Invoke(new MethodInvoker(delegate ()
                    {
                        dgv.Rows[row].Cells[colName].Value = status.Replace("{time}", (timeWait - (Environment.TickCount - time_start) / 1000).ToString());
                    }));
                    Helpers.Common.DelayTime(0.5);
                }
            }
            catch
            {
            }
        }

        public static void SetStatusDataGridViewWithWait(DataGridView dgv, int row, string colName, int timeWait = 0, int timeStart = 0, string status = "Đợi {time} giây...")
        {
            try
            {
                int time_start = Environment.TickCount;
                while ((Environment.TickCount - time_start) / 1000 - timeWait < 0)
                {
                    dgv.Invoke(new MethodInvoker(delegate ()
                    {
                        dgv.Rows[row].Cells[colName].Value = status.Replace("{time}", (timeStart - (Environment.TickCount - time_start) / 1000).ToString());
                    }));
                    Helpers.Common.DelayTime(0.5);
                }
            }
            catch
            {
            }
        }
        public static string GetStatusDataGridView(DataGridView dgv, int row, int col)
        {
            string output = "";
            try
            {
                bool flag = dgv.Rows[row].Cells[col].Value != null;
                if (flag)
                {
                    try
                    {
                        output = dgv.Rows[row].Cells[col].Value.ToString();
                    }
                    catch
                    {
                        dgv.Invoke(new MethodInvoker(delegate ()
                        {
                            output = dgv.Rows[row].Cells[col].Value.ToString();
                        }));
                    }
                }
            }
            catch
            {
            }
            return output;
        }
        public static string GetStatusDataGridView(DataGridView dgv, int row, string colName)
        {
            string output = "";
            try
            {
                bool flag = dgv.Rows[row].Cells[colName].Value != null;
                if (flag)
                {
                    try
                    {
                        output = dgv.Rows[row].Cells[colName].Value.ToString();
                    }
                    catch
                    {
                        dgv.Invoke(new MethodInvoker(delegate ()
                        {
                            output = dgv.Rows[row].Cells[colName].Value.ToString();
                        }));
                    }
                }
            }
            catch
            {
            }
            return output;
        }
        public static void SetStatusDataGridView(DataGridView dgv, int row, int col, object status)
        {
            try
            {
                try
                {
                    dgv.Invoke(new MethodInvoker(delegate ()
                    {
                        dgv.Rows[row].Cells[col].Value = status;
                    }));
                }
                catch
                {
                    dgv.Rows[row].Cells[col].Value = status;
                }
            }
            catch
            {
            }
        }
        public static void SetStatusDataGridView(DataGridView dgv, int row, string colName, object status)
        {
            try
            {
                try
                {
                    dgv.Invoke(new MethodInvoker(delegate ()
                    {
                        dgv.Rows[row].Cells[colName].Value = status;
                    }));
                }
                catch
                {
                    dgv.Rows[row].Cells[colName].Value = status;
                }
            }
            catch
            {
            }
        }
    }

}

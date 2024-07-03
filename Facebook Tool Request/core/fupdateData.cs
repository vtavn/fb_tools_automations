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
    public partial class fupdateData : Form
    {
        private fMain main;

        public fupdateData(fMain main, string mode)
        {
            InitializeComponent();
            this.main = main;
            cbbTypeUpdate.Text = mode;
            cbbTypeProxy.SelectedIndex = 0;
        }

        private void btnClosed_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                if (txbData.Text.Equals("") || txbData.Text.Equals("|"))
                {
                    if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format(Language.GetValue("Chắc chắn xoá dữ liệu của {0} tài khoản?"), main.CountChooseRowInDatagridview())) == DialogResult.Yes)
                    {
                        flag = true;
                    }
                }
                else if (MessageBoxHelper.ShowMessageBoxWithQuestion(string.Format(Language.GetValue("Chắc chắn cập nhập dữ liệu của {0} tài khoản?"), main.CountChooseRowInDatagridview())) == DialogResult.Yes)
                {
                    flag = true;
                }
                if (!flag)
                {
                    return;
                }
                List<string> list = new List<string>();
                string text = txbData.Text;
                for (int i = 0; i < main.dtgvAcc.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(main.dtgvAcc.Rows[i].Cells["cChose"].Value))
                    {
                        list.Add(main.GetCellAccount(i, "cId"));
                    }
                }
                switch (cbbTypeUpdate.Text)
                {
                    case "Token":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "token", text))
                            {
                                break;
                            }
                            for (int num3 = 0; num3 < main.dtgvAcc.Rows.Count; num3++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[num3].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(num3, "cToken", text);
                                }
                            }
                            break;
                        }
                    case "Cookie":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "cookie1", text))
                            {
                                break;
                            }
                            for (int n = 0; n < main.dtgvAcc.Rows.Count; n++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[n].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(n, "cCookies", text);
                                }
                            }
                            break;
                        }
                    case "Password":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "pass", text))
                            {
                                break;
                            }
                            for (int num5 = 0; num5 < main.dtgvAcc.Rows.Count; num5++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[num5].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(num5, "cPassword", text);
                                }
                            }
                            break;
                        }
                    case "2FA":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "fa2", text))
                            {
                                break;
                            }
                            for (int num2 = 0; num2 < main.dtgvAcc.Rows.Count; num2++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[num2].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(num2, "cFa2", text);
                                }
                            }
                            break;
                        }
                    case "Birthday":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "birthday", text))
                            {
                                break;
                            }
                            for (int num = 0; num < main.dtgvAcc.Rows.Count; num++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[num].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(num, "cBirthday", text);
                                }
                            }
                            break;
                        }
                    case "Useragent":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "useragent", text))
                            {
                                break;
                            }
                            for (int k = 0; k < main.dtgvAcc.Rows.Count; k++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[k].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(k, "cUseragent", text);
                                }
                            }
                            break;
                        }
                    case "Proxy":
                        {
                            int selectedIndex = cbbTypeProxy.SelectedIndex;
                            string text2 = ((text == "") ? "" : (text + "*" + selectedIndex));
                            if (!CommonSQL.UpdateFieldToAccount(list, "proxy", text2))
                            {
                                break;
                            }
                            for (int l = 0; l < main.dtgvAcc.Rows.Count; l++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[l].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(l, "cProxy", text2);
                                }
                            }
                            break;
                        }
                    case "Notes":
                        {
                            if (!CommonSQL.UpdateFieldToAccount(list, "ghiChu", text))
                            {
                                break;
                            }
                            for (int num4 = 0; num4 < main.dtgvAcc.Rows.Count; num4++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[num4].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(num4, "cGhiChu", text);
                                }
                            }
                            break;
                        }
                    case "Mail|pass":
                        {
                            if (text.Split('|').Length != 2)
                            {
                                MessageBoxHelper.ShowMessageBox(Language.GetValue("Nhập lại định dạng chuẩn: Mail|Pass Mail!"), 3);
                                return;
                            }
                            if (!CommonSQL.UpdateMultiFieldToAccount(list, "email|passmail", text))
                            {
                                break;
                            }
                            for (int m = 0; m < main.dtgvAcc.Rows.Count; m++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[m].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(m, "cEmail", text.Split('|')[0]);
                                    main.SetCellAccount(m, "cPassMail", text.Split('|')[1]);
                                }
                            }
                            break;
                        }
                    case "PassMail":
                        {
                            if (!CommonSQL.UpdateMultiFieldToAccount(list, "PassMail", text))
                            {
                                break;
                            }
                            for (int j = 0; j < main.dtgvAcc.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(main.dtgvAcc.Rows[j].Cells["cChose"].Value))
                                {
                                    main.SetCellAccount(j, "cPassMail", text);
                                }
                            }
                            break;
                        }
                }
                Close();
            }
            catch
            {
                MessageBoxHelper.ShowMessageBox(Language.GetValue("Vui lòng thử lại sau!"), 2);
            }
        }

        private void cbbTypeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool visible = cbbTypeUpdate.Text == "Proxy";
            label3.Visible = visible;
            cbbTypeProxy.Visible = visible;
        }
    }
}

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
    public partial class fThemHanhDongPage : Form
    {
        private string id_KichBan;

        public fThemHanhDongPage(string id_KichBan)
        {
            this.InitializeComponent();
            this.id_KichBan = id_KichBan;
        }

        private void btnSpamFb_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fSpamCmtPage(this.id_KichBan, 0, ""));
            bool isSave = fSpamCmtPage.isSave;
            if (isSave)
            {
                base.Close();
            }
        }
    }
}

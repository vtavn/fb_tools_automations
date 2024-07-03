using Facebook_Tool_Request.core.fKichBan;
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
    public partial class fThemHanhDong : Form
    {
        public fThemHanhDong(string id_KichBan)
        {
            this.InitializeComponent();
            this.id_KichBan = id_KichBan;
        }

        private string id_KichBan;

        private void btnPostStatus_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDPostStatus(this.id_KichBan, 0, ""));
            bool isSave = fHDPostStatus.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fSpamCommentPage(this.id_KichBan, 0, ""));
            bool isSave = fSpamCommentPage.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDRegPage(this.id_KichBan, 0, ""));
            bool isSave = fHDRegPage.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fLuotNewFeed(this.id_KichBan, 0, ""));
            bool isSave = fLuotNewFeed.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fChangeInfo_HD(this.id_KichBan, 0, ""));
            bool isSave = fChangeInfo_HD.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDKetBanGoiY(this.id_KichBan, 0, ""));
            bool isSave = fHDKetBanGoiY.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDXacNhanKetBan(this.id_KichBan, 0, ""));
            bool isSave = fHDXacNhanKetBan.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDTimKiemGoogle(this.id_KichBan, 0, ""));
            bool isSave = fHDTimKiemGoogle.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDNghiGiaiLao(this.id_KichBan, 0, ""));
            bool isSave = fHDNghiGiaiLao.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fSeedingFb(this.id_KichBan, 0, ""));
            bool isSave = fSeedingFb.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDKetBanUid(this.id_KichBan, 0, ""));
            bool isSave = fHDKetBanUid.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDNhiemVuClone(this.id_KichBan, 0, ""));
            bool isSave = fHDNhiemVuClone.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Helpers.Common.ShowForm(new fHDBuffViewFb(this.id_KichBan, 0, ""));
            bool isSave = fHDBuffViewFb.isSave;
            if (isSave)
            {
                base.Close();
            }
        }

        private void basetup_spamcomment_Click(object sender, EventArgs e)
        {

        }
    }
}

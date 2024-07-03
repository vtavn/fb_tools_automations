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
    public partial class fToolVip : Form
    {
        private JSON_Settings settings;

        public fToolVip()
        {
            this.InitializeComponent();
            this.settings = new JSON_Settings("configAutoTuongTac", false);
        }
    }
}

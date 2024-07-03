using Facebook_Tool_Request.Properties;
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
    public partial class fHuongDanContent : Form
    {
        public fHuongDanContent()
        {
            this.InitializeComponent();
            this.LoadDgv();
        }

        private void LoadDgv()
        {
            Random random = new Random();
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[u]",
                Resources.u
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[d]",
                Resources.d
            });
            this.dgv.Rows.Add(new object[]
           {
                this.dgv.RowCount + 1,
                "{nd1|nd2|nd3}",
                Resources.sp
           });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[t]",
                Resources.h
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[n*]",
                Resources.rn
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[s*]",
                Resources.rs
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[q***]",
                Resources.number
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r1]",
                Resources.r1
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r2]",
                Resources.r2
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r3]",
                Resources.r3
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r4]",
                Resources.r4
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r5]",
                Resources.r5
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r6]",
                Resources.r6
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r7]",
                Resources.r7
            });
            this.dgv.Rows.Add(new object[]
            {
                this.dgv.RowCount + 1,
                "[r8]",
                Resources.r8
            });
        }

    }
}

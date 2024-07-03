namespace Facebook_Tool_Request.core.KichBanPage
{
    partial class fThemKichBanPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fThemKichBanPage));
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgvHanhDong = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cId_HanhDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTenHanhDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTheLoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctmnHanhDong = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.thêmHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sửaHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xoáHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nhânBảnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.gunaGradientPanel1 = new Guna.UI.WinForms.GunaGradientPanel();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.gunaPictureBox1 = new Guna.UI.WinForms.GunaPictureBox();
            this.gunaControlBox1 = new Guna.UI.WinForms.GunaControlBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvHanhDong)).BeginInit();
            this.ctmnHanhDong.SuspendLayout();
            this.gunaGradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.TargetControl = this;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dtgvHanhDong);
            this.panel1.Location = new System.Drawing.Point(8, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 485);
            this.panel1.TabIndex = 46;
            // 
            // dtgvHanhDong
            // 
            this.dtgvHanhDong.AllowUserToAddRows = false;
            this.dtgvHanhDong.AllowUserToDeleteRows = false;
            this.dtgvHanhDong.AllowUserToResizeRows = false;
            this.dtgvHanhDong.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgvHanhDong.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgvHanhDong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvHanhDong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.cId_HanhDong,
            this.cTenHanhDong,
            this.cTheLoai});
            this.dtgvHanhDong.ContextMenuStrip = this.ctmnHanhDong;
            this.dtgvHanhDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvHanhDong.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dtgvHanhDong.Location = new System.Drawing.Point(0, 0);
            this.dtgvHanhDong.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvHanhDong.MultiSelect = false;
            this.dtgvHanhDong.Name = "dtgvHanhDong";
            this.dtgvHanhDong.RowHeadersVisible = false;
            this.dtgvHanhDong.RowHeadersWidth = 62;
            this.dtgvHanhDong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgvHanhDong.Size = new System.Drawing.Size(358, 485);
            this.dtgvHanhDong.TabIndex = 78;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "STT";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 35;
            // 
            // cId_HanhDong
            // 
            this.cId_HanhDong.HeaderText = "Column1";
            this.cId_HanhDong.MinimumWidth = 8;
            this.cId_HanhDong.Name = "cId_HanhDong";
            this.cId_HanhDong.Visible = false;
            this.cId_HanhDong.Width = 150;
            // 
            // cTenHanhDong
            // 
            this.cTenHanhDong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cTenHanhDong.HeaderText = "Tên hành động";
            this.cTenHanhDong.MinimumWidth = 8;
            this.cTenHanhDong.Name = "cTenHanhDong";
            this.cTenHanhDong.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cTheLoai
            // 
            this.cTheLoai.HeaderText = "Loại tương tác";
            this.cTheLoai.MinimumWidth = 8;
            this.cTheLoai.Name = "cTheLoai";
            this.cTheLoai.Width = 175;
            // 
            // ctmnHanhDong
            // 
            this.ctmnHanhDong.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctmnHanhDong.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thêmHànhĐộngToolStripMenuItem,
            this.sửaHànhĐộngToolStripMenuItem,
            this.xoáHànhĐộngToolStripMenuItem,
            this.nhânBảnToolStripMenuItem1});
            this.ctmnHanhDong.Name = "contextMenuStrip2";
            this.ctmnHanhDong.Size = new System.Drawing.Size(181, 114);
            // 
            // thêmHànhĐộngToolStripMenuItem
            // 
            this.thêmHànhĐộngToolStripMenuItem.Name = "thêmHànhĐộngToolStripMenuItem";
            this.thêmHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.thêmHànhĐộngToolStripMenuItem.Text = "Thêm Hành Động";
            this.thêmHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.thêmHànhĐộngToolStripMenuItem_Click);
            // 
            // sửaHànhĐộngToolStripMenuItem
            // 
            this.sửaHànhĐộngToolStripMenuItem.Name = "sửaHànhĐộngToolStripMenuItem";
            this.sửaHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sửaHànhĐộngToolStripMenuItem.Text = "Sửa Hành Động";
            this.sửaHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.sửaHànhĐộngToolStripMenuItem_Click);
            // 
            // xoáHànhĐộngToolStripMenuItem
            // 
            this.xoáHànhĐộngToolStripMenuItem.Name = "xoáHànhĐộngToolStripMenuItem";
            this.xoáHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xoáHànhĐộngToolStripMenuItem.Text = "Xoá Hành Động";
            this.xoáHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.xoáHànhĐộngToolStripMenuItem_Click);
            // 
            // nhânBảnToolStripMenuItem1
            // 
            this.nhânBảnToolStripMenuItem1.Name = "nhânBảnToolStripMenuItem1";
            this.nhânBảnToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.nhânBảnToolStripMenuItem1.Text = "Nhân Bản";
            this.nhânBảnToolStripMenuItem1.Click += new System.EventHandler(this.nhânBảnToolStripMenuItem1_Click);
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.gunaGradientPanel1;
            this.bunifuDragControl1.Vertical = true;
            // 
            // gunaGradientPanel1
            // 
            this.gunaGradientPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaGradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gunaGradientPanel1.BackgroundImage")));
            this.gunaGradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gunaGradientPanel1.Controls.Add(this.txtTen);
            this.gunaGradientPanel1.Controls.Add(this.gunaLabel1);
            this.gunaGradientPanel1.Controls.Add(this.gunaPictureBox1);
            this.gunaGradientPanel1.Controls.Add(this.gunaControlBox1);
            this.gunaGradientPanel1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.gunaGradientPanel1.GradientColor1 = System.Drawing.Color.White;
            this.gunaGradientPanel1.GradientColor2 = System.Drawing.Color.White;
            this.gunaGradientPanel1.GradientColor3 = System.Drawing.Color.White;
            this.gunaGradientPanel1.GradientColor4 = System.Drawing.Color.White;
            this.gunaGradientPanel1.Location = new System.Drawing.Point(1, 1);
            this.gunaGradientPanel1.Name = "gunaGradientPanel1";
            this.gunaGradientPanel1.Size = new System.Drawing.Size(373, 29);
            this.gunaGradientPanel1.TabIndex = 45;
            this.gunaGradientPanel1.Text = "gunaGradientPanel1";
            // 
            // txtTen
            // 
            this.txtTen.Location = new System.Drawing.Point(120, 4);
            this.txtTen.Name = "txtTen";
            this.txtTen.Size = new System.Drawing.Size(181, 22);
            this.txtTen.TabIndex = 0;
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.BackColor = System.Drawing.Color.Transparent;
            this.gunaLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.gunaLabel1.Location = new System.Drawing.Point(37, 6);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(77, 15);
            this.gunaLabel1.TabIndex = 45;
            this.gunaLabel1.Text = "Tên kịch bản:";
            // 
            // gunaPictureBox1
            // 
            this.gunaPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.gunaPictureBox1.BaseColor = System.Drawing.Color.White;
            this.gunaPictureBox1.Image = global::Facebook_Tool_Request.Properties.Resources.cua_logo;
            this.gunaPictureBox1.InitialImage = global::Facebook_Tool_Request.Properties.Resources.cua_logo;
            this.gunaPictureBox1.Location = new System.Drawing.Point(3, -1);
            this.gunaPictureBox1.Name = "gunaPictureBox1";
            this.gunaPictureBox1.Size = new System.Drawing.Size(28, 30);
            this.gunaPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gunaPictureBox1.TabIndex = 1;
            this.gunaPictureBox1.TabStop = false;
            // 
            // gunaControlBox1
            // 
            this.gunaControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaControlBox1.AnimationHoverSpeed = 0.07F;
            this.gunaControlBox1.AnimationSpeed = 0.03F;
            this.gunaControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.gunaControlBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaControlBox1.IconColor = System.Drawing.Color.Black;
            this.gunaControlBox1.IconSize = 15F;
            this.gunaControlBox1.Location = new System.Drawing.Point(328, -2);
            this.gunaControlBox1.Name = "gunaControlBox1";
            this.gunaControlBox1.OnHoverBackColor = System.Drawing.Color.Orange;
            this.gunaControlBox1.OnHoverIconColor = System.Drawing.Color.White;
            this.gunaControlBox1.OnPressedColor = System.Drawing.Color.Black;
            this.gunaControlBox1.Size = new System.Drawing.Size(45, 31);
            this.gunaControlBox1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Montserrat SemiBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(186, 525);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 29);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "Đóng";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAdd.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Montserrat SemiBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(58, 525);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(122, 29);
            this.btnAdd.TabIndex = 48;
            this.btnAdd.Text = "Lưu";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // fThemKichBanPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(375, 563);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gunaGradientPanel1);
            this.Font = new System.Drawing.Font("Montserrat", 8.999999F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fThemKichBanPage";
            this.Text = "Thêm kịch bản page";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvHanhDong)).EndInit();
            this.ctmnHanhDong.ResumeLayout(false);
            this.gunaGradientPanel1.ResumeLayout(false);
            this.gunaGradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI.WinForms.GunaElipse gunaElipse1;
        private Guna.UI.WinForms.GunaGradientPanel gunaGradientPanel1;
        private Guna.UI.WinForms.GunaLabel gunaLabel1;
        private Guna.UI.WinForms.GunaControlBox gunaControlBox1;
        private Guna.UI.WinForms.GunaPictureBox gunaPictureBox1;
        private System.Windows.Forms.Panel panel1;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtTen;
        public System.Windows.Forms.DataGridView dtgvHanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId_HanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTenHanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTheLoai;
        private System.Windows.Forms.ContextMenuStrip ctmnHanhDong;
        private System.Windows.Forms.ToolStripMenuItem thêmHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sửaHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xoáHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nhânBảnToolStripMenuItem1;
    }
}
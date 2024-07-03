namespace Facebook_Tool_Request.core
{
    partial class fDanhSachKichBan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDanhSachKichBan));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtgvKichBan = new System.Windows.Forms.DataGridView();
            this.cStt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cId_KichBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTenKichBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextKichBan = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.thêmKịchBảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sửaTênKịchBảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xoáKịchBảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nhânBảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dtgvHanhDong = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cId_HanhDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTenHanhDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTheLoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextHanhDong = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.thêmHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sửaHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xoáHànhĐộngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nhânBảnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gunaDragControl1 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.gunaGradientPanel1 = new Guna.UI.WinForms.GunaGradientPanel();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.gunaPictureBox1 = new Guna.UI.WinForms.GunaPictureBox();
            this.gunaControlBox1 = new Guna.UI.WinForms.GunaControlBox();
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvKichBan)).BeginInit();
            this.contextKichBan.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvHanhDong)).BeginInit();
            this.contextHanhDong.SuspendLayout();
            this.gunaGradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.dtgvKichBan);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(8, 38);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(302, 586);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Danh sách kịch bản";
            // 
            // dtgvKichBan
            // 
            this.dtgvKichBan.AllowUserToAddRows = false;
            this.dtgvKichBan.AllowUserToDeleteRows = false;
            this.dtgvKichBan.AllowUserToResizeRows = false;
            this.dtgvKichBan.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgvKichBan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtgvKichBan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvKichBan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cStt,
            this.cId_KichBan,
            this.cTenKichBan});
            this.dtgvKichBan.ContextMenuStrip = this.contextKichBan;
            this.dtgvKichBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvKichBan.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dtgvKichBan.Location = new System.Drawing.Point(4, 22);
            this.dtgvKichBan.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvKichBan.MultiSelect = false;
            this.dtgvKichBan.Name = "dtgvKichBan";
            this.dtgvKichBan.RowHeadersVisible = false;
            this.dtgvKichBan.RowHeadersWidth = 62;
            this.dtgvKichBan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgvKichBan.Size = new System.Drawing.Size(294, 559);
            this.dtgvKichBan.TabIndex = 76;
            this.dtgvKichBan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvKichBan_CellClick);
            this.dtgvKichBan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtgvKichBan_KeyDown);
            // 
            // cStt
            // 
            this.cStt.HeaderText = "STT";
            this.cStt.MinimumWidth = 8;
            this.cStt.Name = "cStt";
            this.cStt.Width = 35;
            // 
            // cId_KichBan
            // 
            this.cId_KichBan.HeaderText = "Column1";
            this.cId_KichBan.MinimumWidth = 8;
            this.cId_KichBan.Name = "cId_KichBan";
            this.cId_KichBan.Visible = false;
            this.cId_KichBan.Width = 150;
            // 
            // cTenKichBan
            // 
            this.cTenKichBan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cTenKichBan.HeaderText = "Tên kịch bản";
            this.cTenKichBan.MinimumWidth = 8;
            this.cTenKichBan.Name = "cTenKichBan";
            this.cTenKichBan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextKichBan
            // 
            this.contextKichBan.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextKichBan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thêmKịchBảnToolStripMenuItem,
            this.sửaTênKịchBảnToolStripMenuItem,
            this.xoáKịchBảnToolStripMenuItem,
            this.nhânBảnToolStripMenuItem});
            this.contextKichBan.Name = "contextMenuStrip1";
            this.contextKichBan.Size = new System.Drawing.Size(164, 92);
            // 
            // thêmKịchBảnToolStripMenuItem
            // 
            this.thêmKịchBảnToolStripMenuItem.Name = "thêmKịchBảnToolStripMenuItem";
            this.thêmKịchBảnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.thêmKịchBảnToolStripMenuItem.Text = "Thêm Kịch Bản";
            this.thêmKịchBảnToolStripMenuItem.Click += new System.EventHandler(this.thêmKịchBảnToolStripMenuItem_Click);
            // 
            // sửaTênKịchBảnToolStripMenuItem
            // 
            this.sửaTênKịchBảnToolStripMenuItem.Name = "sửaTênKịchBảnToolStripMenuItem";
            this.sửaTênKịchBảnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.sửaTênKịchBảnToolStripMenuItem.Text = "Sửa Tên Kịch Bản";
            this.sửaTênKịchBảnToolStripMenuItem.Click += new System.EventHandler(this.sửaTênKịchBảnToolStripMenuItem_Click);
            // 
            // xoáKịchBảnToolStripMenuItem
            // 
            this.xoáKịchBảnToolStripMenuItem.Name = "xoáKịchBảnToolStripMenuItem";
            this.xoáKịchBảnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.xoáKịchBảnToolStripMenuItem.Text = "Xoá Kịch Bản";
            this.xoáKịchBảnToolStripMenuItem.Click += new System.EventHandler(this.xoáKịchBảnToolStripMenuItem_Click);
            // 
            // nhânBảnToolStripMenuItem
            // 
            this.nhânBảnToolStripMenuItem.Name = "nhânBảnToolStripMenuItem";
            this.nhânBảnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.nhânBảnToolStripMenuItem.Text = "Nhân Bản";
            this.nhânBảnToolStripMenuItem.Click += new System.EventHandler(this.nhânBảnToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.dtgvHanhDong);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox2.Location = new System.Drawing.Point(314, 38);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(594, 586);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách hành động";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Image = global::Facebook_Tool_Request.Properties.Resources.expand_arrow_24;
            this.button3.Location = new System.Drawing.Point(555, 59);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 35);
            this.button3.TabIndex = 78;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Image = global::Facebook_Tool_Request.Properties.Resources.collapse_arrow_24;
            this.button2.Location = new System.Drawing.Point(555, 21);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 35);
            this.button2.TabIndex = 78;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtgvHanhDong
            // 
            this.dtgvHanhDong.AllowUserToAddRows = false;
            this.dtgvHanhDong.AllowUserToDeleteRows = false;
            this.dtgvHanhDong.AllowUserToResizeRows = false;
            this.dtgvHanhDong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvHanhDong.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
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
            this.dtgvHanhDong.ContextMenuStrip = this.contextHanhDong;
            this.dtgvHanhDong.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dtgvHanhDong.Location = new System.Drawing.Point(5, 22);
            this.dtgvHanhDong.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvHanhDong.MultiSelect = false;
            this.dtgvHanhDong.Name = "dtgvHanhDong";
            this.dtgvHanhDong.RowHeadersVisible = false;
            this.dtgvHanhDong.RowHeadersWidth = 62;
            this.dtgvHanhDong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgvHanhDong.Size = new System.Drawing.Size(546, 559);
            this.dtgvHanhDong.TabIndex = 77;
            this.dtgvHanhDong.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtgvHanhDong_KeyDown);
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
            // contextHanhDong
            // 
            this.contextHanhDong.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextHanhDong.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thêmHànhĐộngToolStripMenuItem,
            this.sửaHànhĐộngToolStripMenuItem,
            this.xoáHànhĐộngToolStripMenuItem,
            this.nhânBảnToolStripMenuItem1});
            this.contextHanhDong.Name = "contextMenuStrip2";
            this.contextHanhDong.Size = new System.Drawing.Size(169, 92);
            // 
            // thêmHànhĐộngToolStripMenuItem
            // 
            this.thêmHànhĐộngToolStripMenuItem.Name = "thêmHànhĐộngToolStripMenuItem";
            this.thêmHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.thêmHànhĐộngToolStripMenuItem.Text = "Thêm Hành Động";
            this.thêmHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.thêmHànhĐộngToolStripMenuItem_Click);
            // 
            // sửaHànhĐộngToolStripMenuItem
            // 
            this.sửaHànhĐộngToolStripMenuItem.Name = "sửaHànhĐộngToolStripMenuItem";
            this.sửaHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.sửaHànhĐộngToolStripMenuItem.Text = "Sửa Hành Động";
            this.sửaHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.sửaHànhĐộngToolStripMenuItem_Click);
            // 
            // xoáHànhĐộngToolStripMenuItem
            // 
            this.xoáHànhĐộngToolStripMenuItem.Name = "xoáHànhĐộngToolStripMenuItem";
            this.xoáHànhĐộngToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.xoáHànhĐộngToolStripMenuItem.Text = "Xoá Hành Động";
            this.xoáHànhĐộngToolStripMenuItem.Click += new System.EventHandler(this.xoáHànhĐộngToolStripMenuItem_Click);
            // 
            // nhânBảnToolStripMenuItem1
            // 
            this.nhânBảnToolStripMenuItem1.Name = "nhânBảnToolStripMenuItem1";
            this.nhânBảnToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.nhânBảnToolStripMenuItem1.Text = "Nhân Bản";
            this.nhânBảnToolStripMenuItem1.Click += new System.EventHandler(this.nhânBảnToolStripMenuItem1_Click);
            // 
            // gunaDragControl1
            // 
            this.gunaDragControl1.TargetControl = this.gunaGradientPanel1;
            // 
            // gunaGradientPanel1
            // 
            this.gunaGradientPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaGradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gunaGradientPanel1.BackgroundImage")));
            this.gunaGradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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
            this.gunaGradientPanel1.Size = new System.Drawing.Size(915, 31);
            this.gunaGradientPanel1.TabIndex = 183;
            this.gunaGradientPanel1.Text = "gunaGradientPanel1";
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel1.Location = new System.Drawing.Point(36, 9);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(133, 15);
            this.gunaLabel1.TabIndex = 2;
            this.gunaLabel1.Text = "DANH SÁCH KỊCH BẢN";
            // 
            // gunaPictureBox1
            // 
            this.gunaPictureBox1.BaseColor = System.Drawing.Color.White;
            this.gunaPictureBox1.Image = global::Facebook_Tool_Request.Properties.Resources.cua_logo;
            this.gunaPictureBox1.InitialImage = global::Facebook_Tool_Request.Properties.Resources.cua_logo;
            this.gunaPictureBox1.Location = new System.Drawing.Point(3, 2);
            this.gunaPictureBox1.Name = "gunaPictureBox1";
            this.gunaPictureBox1.Size = new System.Drawing.Size(28, 29);
            this.gunaPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gunaPictureBox1.TabIndex = 1;
            this.gunaPictureBox1.TabStop = false;
            // 
            // gunaControlBox1
            // 
            this.gunaControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaControlBox1.AnimationHoverSpeed = 0.07F;
            this.gunaControlBox1.AnimationSpeed = 0.03F;
            this.gunaControlBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaControlBox1.IconColor = System.Drawing.Color.Black;
            this.gunaControlBox1.IconSize = 15F;
            this.gunaControlBox1.Location = new System.Drawing.Point(870, -1);
            this.gunaControlBox1.Name = "gunaControlBox1";
            this.gunaControlBox1.OnHoverBackColor = System.Drawing.Color.Orange;
            this.gunaControlBox1.OnHoverIconColor = System.Drawing.Color.White;
            this.gunaControlBox1.OnPressedColor = System.Drawing.Color.Black;
            this.gunaControlBox1.Size = new System.Drawing.Size(45, 31);
            this.gunaControlBox1.TabIndex = 0;
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.TargetControl = this;
            // 
            // fDanhSachKichBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(917, 634);
            this.ControlBox = false;
            this.Controls.Add(this.gunaGradientPanel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fDanhSachKichBan";
            this.Text = "Quản Lý Kịch Bản";
            this.Load += new System.EventHandler(this.fDanhSachKichBan_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvKichBan)).EndInit();
            this.contextKichBan.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvHanhDong)).EndInit();
            this.contextHanhDong.ResumeLayout(false);
            this.gunaGradientPanel1.ResumeLayout(false);
            this.gunaGradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DataGridView dtgvKichBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStt;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId_KichBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTenKichBan;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.DataGridView dtgvHanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cId_HanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTenHanhDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTheLoai;
        private System.Windows.Forms.ContextMenuStrip contextKichBan;
        private System.Windows.Forms.ToolStripMenuItem thêmKịchBảnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sửaTênKịchBảnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xoáKịchBảnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nhânBảnToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextHanhDong;
        private System.Windows.Forms.ToolStripMenuItem thêmHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sửaHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xoáHànhĐộngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nhânBảnToolStripMenuItem1;
        private Guna.UI.WinForms.GunaDragControl gunaDragControl1;
        private Guna.UI.WinForms.GunaElipse gunaElipse1;
        private Guna.UI.WinForms.GunaGradientPanel gunaGradientPanel1;
        private Guna.UI.WinForms.GunaLabel gunaLabel1;
        private Guna.UI.WinForms.GunaPictureBox gunaPictureBox1;
        private Guna.UI.WinForms.GunaControlBox gunaControlBox1;
    }
}
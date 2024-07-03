namespace Facebook_Tool_Request.core.fTools
{
    partial class fTienIchLocTrung
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
            this.bunifuCards1 = new Bunifu.Framework.UI.BunifuCards();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.titleApplication = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.gunaGroupBox1 = new Guna.UI.WinForms.GunaGroupBox();
            this.btnSelectedFile = new MetroFramework.Controls.MetroButton();
            this.txtInput = new System.Windows.Forms.RichTextBox();
            this.txtNhapTuFile = new System.Windows.Forms.TextBox();
            this.rbTuNhap = new System.Windows.Forms.RadioButton();
            this.rbNhapTuFile = new System.Windows.Forms.RadioButton();
            this.gunaGroupBox2 = new Guna.UI.WinForms.GunaGroupBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.btnFilters = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.bunifuCards1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gunaGroupBox1.SuspendLayout();
            this.gunaGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuCards1
            // 
            this.bunifuCards1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuCards1.BackColor = System.Drawing.Color.White;
            this.bunifuCards1.BorderRadius = 5;
            this.bunifuCards1.BottomSahddow = true;
            this.bunifuCards1.color = System.Drawing.Color.Tomato;
            this.bunifuCards1.Controls.Add(this.pnlHeader);
            this.bunifuCards1.LeftSahddow = false;
            this.bunifuCards1.Location = new System.Drawing.Point(0, 0);
            this.bunifuCards1.Name = "bunifuCards1";
            this.bunifuCards1.RightSahddow = true;
            this.bunifuCards1.ShadowDepth = 20;
            this.bunifuCards1.Size = new System.Drawing.Size(1033, 38);
            this.bunifuCards1.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.titleApplication);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Controls.Add(this.btnMinimize);
            this.pnlHeader.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pnlHeader.Location = new System.Drawing.Point(0, 5);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1033, 29);
            this.pnlHeader.TabIndex = 1;
            // 
            // titleApplication
            // 
            this.titleApplication.AutoSize = true;
            this.titleApplication.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.titleApplication.ForeColor = System.Drawing.Color.Black;
            this.titleApplication.Location = new System.Drawing.Point(46, 6);
            this.titleApplication.Name = "titleApplication";
            this.titleApplication.Size = new System.Drawing.Size(122, 16);
            this.titleApplication.TabIndex = 12;
            this.titleApplication.Text = "Lọc Trùng Dữ Liệu";
            this.titleApplication.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Facebook_Tool_Request.Properties.Resources.cua_logo;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(6, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Image = global::Facebook_Tool_Request.Properties.Resources.icons8_close_window_29;
            this.btnMinimize.Location = new System.Drawing.Point(1003, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 29);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.pnlHeader;
            this.bunifuDragControl1.Vertical = true;
            // 
            // gunaGroupBox1
            // 
            this.gunaGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.gunaGroupBox1.BaseColor = System.Drawing.Color.White;
            this.gunaGroupBox1.BorderColor = System.Drawing.Color.Gainsboro;
            this.gunaGroupBox1.BorderSize = 1;
            this.gunaGroupBox1.Controls.Add(this.btnSelectedFile);
            this.gunaGroupBox1.Controls.Add(this.txtInput);
            this.gunaGroupBox1.Controls.Add(this.txtNhapTuFile);
            this.gunaGroupBox1.Controls.Add(this.rbTuNhap);
            this.gunaGroupBox1.Controls.Add(this.rbNhapTuFile);
            this.gunaGroupBox1.LineColor = System.Drawing.Color.Gainsboro;
            this.gunaGroupBox1.Location = new System.Drawing.Point(6, 44);
            this.gunaGroupBox1.Name = "gunaGroupBox1";
            this.gunaGroupBox1.Size = new System.Drawing.Size(504, 373);
            this.gunaGroupBox1.TabIndex = 2;
            this.gunaGroupBox1.Text = "Input";
            this.gunaGroupBox1.TextLocation = new System.Drawing.Point(10, 8);
            // 
            // btnSelectedFile
            // 
            this.btnSelectedFile.Location = new System.Drawing.Point(284, 38);
            this.btnSelectedFile.Name = "btnSelectedFile";
            this.btnSelectedFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectedFile.TabIndex = 55;
            this.btnSelectedFile.Text = "Chọn";
            this.btnSelectedFile.Click += new System.EventHandler(this.btnSelectedFile_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(21, 91);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(474, 275);
            this.txtInput.TabIndex = 54;
            this.txtInput.Text = "";
            this.txtInput.WordWrap = false;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // txtNhapTuFile
            // 
            this.txtNhapTuFile.Location = new System.Drawing.Point(107, 38);
            this.txtNhapTuFile.Name = "txtNhapTuFile";
            this.txtNhapTuFile.Size = new System.Drawing.Size(171, 23);
            this.txtNhapTuFile.TabIndex = 53;
            // 
            // rbTuNhap
            // 
            this.rbTuNhap.AutoSize = true;
            this.rbTuNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbTuNhap.Location = new System.Drawing.Point(6, 66);
            this.rbTuNhap.Name = "rbTuNhap";
            this.rbTuNhap.Size = new System.Drawing.Size(94, 20);
            this.rbTuNhap.TabIndex = 51;
            this.rbTuNhap.Text = "Tự nhập (0)";
            this.rbTuNhap.UseVisualStyleBackColor = true;
            this.rbTuNhap.CheckedChanged += new System.EventHandler(this.rbTuNhap_CheckedChanged);
            // 
            // rbNhapTuFile
            // 
            this.rbNhapTuFile.AutoSize = true;
            this.rbNhapTuFile.Checked = true;
            this.rbNhapTuFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbNhapTuFile.Location = new System.Drawing.Point(6, 39);
            this.rbNhapTuFile.Name = "rbNhapTuFile";
            this.rbNhapTuFile.Size = new System.Drawing.Size(94, 20);
            this.rbNhapTuFile.TabIndex = 52;
            this.rbNhapTuFile.TabStop = true;
            this.rbNhapTuFile.Text = "Nhập từ File";
            this.rbNhapTuFile.UseVisualStyleBackColor = true;
            this.rbNhapTuFile.CheckedChanged += new System.EventHandler(this.rbNhapTuFile_CheckedChanged);
            // 
            // gunaGroupBox2
            // 
            this.gunaGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.gunaGroupBox2.BaseColor = System.Drawing.Color.White;
            this.gunaGroupBox2.BorderColor = System.Drawing.Color.Gainsboro;
            this.gunaGroupBox2.BorderSize = 1;
            this.gunaGroupBox2.Controls.Add(this.txtOutput);
            this.gunaGroupBox2.LineColor = System.Drawing.Color.Gainsboro;
            this.gunaGroupBox2.Location = new System.Drawing.Point(516, 44);
            this.gunaGroupBox2.Name = "gunaGroupBox2";
            this.gunaGroupBox2.Size = new System.Drawing.Size(504, 373);
            this.gunaGroupBox2.TabIndex = 55;
            this.gunaGroupBox2.Text = "Output (0)";
            this.gunaGroupBox2.TextLocation = new System.Drawing.Point(10, 8);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(7, 36);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(488, 328);
            this.txtOutput.TabIndex = 51;
            this.txtOutput.Text = "";
            this.txtOutput.WordWrap = false;
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // btnFilters
            // 
            this.btnFilters.BackColor = System.Drawing.Color.Red;
            this.btnFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilters.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnFilters.ForeColor = System.Drawing.Color.White;
            this.btnFilters.Location = new System.Drawing.Point(364, 423);
            this.btnFilters.Name = "btnFilters";
            this.btnFilters.Size = new System.Drawing.Size(137, 36);
            this.btnFilters.TabIndex = 58;
            this.btnFilters.Text = "Lọc";
            this.btnFilters.UseVisualStyleBackColor = false;
            this.btnFilters.Click += new System.EventHandler(this.btnFilters_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.Enabled = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(507, 423);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(137, 36);
            this.btnSave.TabIndex = 59;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // fTienIchLocTrung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1031, 465);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFilters);
            this.Controls.Add(this.gunaGroupBox2);
            this.Controls.Add(this.gunaGroupBox1);
            this.Controls.Add(this.bunifuCards1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fTienIchLocTrung";
            this.Text = "fTienIchLocTrung";
            this.bunifuCards1.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gunaGroupBox1.ResumeLayout(false);
            this.gunaGroupBox1.PerformLayout();
            this.gunaGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCards bunifuCards1;
        private System.Windows.Forms.Panel pnlHeader;
        private Bunifu.Framework.UI.BunifuCustomLabel titleApplication;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMinimize;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private Guna.UI.WinForms.GunaGroupBox gunaGroupBox1;
        private System.Windows.Forms.RichTextBox txtInput;
        private System.Windows.Forms.TextBox txtNhapTuFile;
        private System.Windows.Forms.RadioButton rbTuNhap;
        private System.Windows.Forms.RadioButton rbNhapTuFile;
        private Guna.UI.WinForms.GunaGroupBox gunaGroupBox2;
        private System.Windows.Forms.RichTextBox txtOutput;
        private MetroFramework.Controls.MetroButton btnSelectedFile;
        private System.Windows.Forms.Button btnFilters;
        private System.Windows.Forms.Button btnSave;
    }
}
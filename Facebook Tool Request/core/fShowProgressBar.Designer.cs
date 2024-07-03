namespace Facebook_Tool_Request.core
{
    partial class fShowProgressBar
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
            this.lblproccess = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblproccess
            // 
            this.lblproccess.AutoSize = true;
            this.lblproccess.Location = new System.Drawing.Point(49, 23);
            this.lblproccess.Name = "lblproccess";
            this.lblproccess.Size = new System.Drawing.Size(188, 16);
            this.lblproccess.TabIndex = 0;
            this.lblproccess.Text = "Đang copy, vui lòng chờ (0/0)...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(38, 51);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(219, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // fShowProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 104);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblproccess);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "fShowProgressBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_progress";
            this.Load += new System.EventHandler(this.fShowProgressBar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblproccess;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
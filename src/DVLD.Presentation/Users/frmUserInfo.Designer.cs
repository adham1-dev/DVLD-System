namespace DVLD.Presentation.Users
{
    partial class frmUserInfo
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
            this.ctrlUC = new DVLD.Presentation.Users.ctrlUserCard();
            this.SuspendLayout();
            // 
            // ctrlUC
            // 
            this.ctrlUC.Location = new System.Drawing.Point(12, 12);
            this.ctrlUC.Name = "ctrlUC";
            this.ctrlUC.Size = new System.Drawing.Size(714, 370);
            this.ctrlUC.TabIndex = 0;
            // 
            // frmUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 409);
            this.Controls.Add(this.ctrlUC);
            this.Name = "frmUserInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Info";
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlUserCard ctrlUC;
    }
}
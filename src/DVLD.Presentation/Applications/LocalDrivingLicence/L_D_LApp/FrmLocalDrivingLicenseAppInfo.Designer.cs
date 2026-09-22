namespace DVLD.Presentation.Applications.L_D_LApp
{
    partial class FrmLocalDrivingLicenseAppInfo
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
            this.ctrlLDL_ApplicationInfo1 = new DVLD.Presentation.Applications.L_D_LApp.ctrlLDL_ApplicationInfo();
            this.SuspendLayout();
            // 
            // ctrlLDL_ApplicationInfo1
            // 
            this.ctrlLDL_ApplicationInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlLDL_ApplicationInfo1.Location = new System.Drawing.Point(12, 12);
            this.ctrlLDL_ApplicationInfo1.Name = "ctrlLDL_ApplicationInfo1";
            this.ctrlLDL_ApplicationInfo1.Size = new System.Drawing.Size(546, 417);
            this.ctrlLDL_ApplicationInfo1.TabIndex = 0;
            // 
            // FrmLocalDrivingLicenseAppInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 439);
            this.Controls.Add(this.ctrlLDL_ApplicationInfo1);
            this.Name = "FrmLocalDrivingLicenseAppInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLocalDrivingLicenseAppInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlLDL_ApplicationInfo ctrlLDL_ApplicationInfo1;
    }
}
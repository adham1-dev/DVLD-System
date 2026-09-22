namespace DVLD.Presentation.Applications.L_D_LApp
{
    partial class FrmAddUpdateNewL_D_LApplication
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTop = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tbPersonInfo = new System.Windows.Forms.TabPage();
            this.btnNext = new System.Windows.Forms.Button();
            this.ctrlPCF = new DVLD.Presentation.People.ctrlPersonCardFilter();
            this.tbAppInfo = new System.Windows.Forms.TabPage();
            this.pnlApp = new System.Windows.Forms.Panel();
            this.cmbLC = new System.Windows.Forms.ComboBox();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblFees = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblL_D_LAppID = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabs.SuspendLayout();
            this.tbPersonInfo.SuspendLayout();
            this.tbAppInfo.SuspendLayout();
            this.pnlApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Cairo SemiBold", 9.980197F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(539, 553);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 39);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Cairo SemiBold", 9.980197F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(642, 553);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 39);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTop
            // 
            this.lblTop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTop.AutoSize = true;
            this.lblTop.Font = new System.Drawing.Font("Cairo", 27.80198F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTop.Location = new System.Drawing.Point(12, 18);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(728, 72);
            this.lblTop.TabIndex = 34;
            this.lblTop.Text = "Add New Local Driving License Applecation";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tbPersonInfo);
            this.tabs.Controls.Add(this.tbAppInfo);
            this.tabs.Location = new System.Drawing.Point(15, 109);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(729, 438);
            this.tabs.TabIndex = 31;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // tbPersonInfo
            // 
            this.tbPersonInfo.Controls.Add(this.btnNext);
            this.tbPersonInfo.Controls.Add(this.ctrlPCF);
            this.tbPersonInfo.Location = new System.Drawing.Point(4, 22);
            this.tbPersonInfo.Name = "tbPersonInfo";
            this.tbPersonInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tbPersonInfo.Size = new System.Drawing.Size(721, 412);
            this.tbPersonInfo.TabIndex = 0;
            this.tbPersonInfo.Text = "Personal Info";
            this.tbPersonInfo.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatAppearance.BorderSize = 2;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Font = new System.Drawing.Font("Cairo SemiBold", 9.980197F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(597, 367);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(115, 39);
            this.btnNext.TabIndex = 31;
            this.btnNext.Text = "Next →";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // ctrlPCF
            // 
            this.ctrlPCF.IsLocked = false;
            this.ctrlPCF.Location = new System.Drawing.Point(6, 6);
            this.ctrlPCF.Name = "ctrlPCF";
            this.ctrlPCF.Size = new System.Drawing.Size(706, 347);
            this.ctrlPCF.TabIndex = 0;
            // 
            // tbAppInfo
            // 
            this.tbAppInfo.Controls.Add(this.pnlApp);
            this.tbAppInfo.Location = new System.Drawing.Point(4, 22);
            this.tbAppInfo.Name = "tbAppInfo";
            this.tbAppInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tbAppInfo.Size = new System.Drawing.Size(721, 412);
            this.tbAppInfo.TabIndex = 1;
            this.tbAppInfo.Text = "Application Info";
            this.tbAppInfo.UseVisualStyleBackColor = true;
            // 
            // pnlApp
            // 
            this.pnlApp.Controls.Add(this.cmbLC);
            this.pnlApp.Controls.Add(this.lblCreatedBy);
            this.pnlApp.Controls.Add(this.lblFees);
            this.pnlApp.Controls.Add(this.lblDate);
            this.pnlApp.Controls.Add(this.label1);
            this.pnlApp.Controls.Add(this.lblL_D_LAppID);
            this.pnlApp.Controls.Add(this.label4);
            this.pnlApp.Controls.Add(this.label3);
            this.pnlApp.Controls.Add(this.label2);
            this.pnlApp.Controls.Add(this.label14);
            this.pnlApp.Enabled = false;
            this.pnlApp.Location = new System.Drawing.Point(6, 6);
            this.pnlApp.Name = "pnlApp";
            this.pnlApp.Size = new System.Drawing.Size(439, 315);
            this.pnlApp.TabIndex = 56;
            // 
            // cmbLC
            // 
            this.cmbLC.FormattingEnabled = true;
            this.cmbLC.Location = new System.Drawing.Point(224, 147);
            this.cmbLC.Name = "cmbLC";
            this.cmbLC.Size = new System.Drawing.Size(182, 21);
            this.cmbLC.TabIndex = 69;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.Location = new System.Drawing.Point(218, 208);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(36, 32);
            this.lblCreatedBy.TabIndex = 68;
            this.lblCreatedBy.Text = "__";
            // 
            // lblFees
            // 
            this.lblFees.AutoSize = true;
            this.lblFees.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFees.Location = new System.Drawing.Point(218, 176);
            this.lblFees.Name = "lblFees";
            this.lblFees.Size = new System.Drawing.Size(38, 32);
            this.lblFees.TabIndex = 67;
            this.lblFees.Text = "0 $";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(218, 112);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(106, 32);
            this.lblDate.TabIndex = 66;
            this.lblDate.Text = "dd/mm/yyyy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 32);
            this.label1.TabIndex = 65;
            this.label1.Text = "Created By:";
            // 
            // lblL_D_LAppID
            // 
            this.lblL_D_LAppID.AutoSize = true;
            this.lblL_D_LAppID.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblL_D_LAppID.Location = new System.Drawing.Point(218, 79);
            this.lblL_D_LAppID.Name = "lblL_D_LAppID";
            this.lblL_D_LAppID.Size = new System.Drawing.Size(36, 32);
            this.lblL_D_LAppID.TabIndex = 64;
            this.lblL_D_LAppID.Text = "__";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(62, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 32);
            this.label4.TabIndex = 61;
            this.label4.Text = "Application Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(85, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 32);
            this.label3.TabIndex = 60;
            this.label3.Text = "License Class:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 32);
            this.label2.TabIndex = 59;
            this.label2.Text = "Application Fees:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Cairo SemiBold", 12.11881F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(43, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(156, 32);
            this.label14.TabIndex = 56;
            this.label14.Text = "L.D.L Application ID:";
            // 
            // FrmAddUpdateNewL_D_LApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 612);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.tabs);
            this.Name = "FrmAddUpdateNewL_D_LApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmAddUpdateNewL_D_LApplication";
            this.tabs.ResumeLayout(false);
            this.tbPersonInfo.ResumeLayout(false);
            this.tbAppInfo.ResumeLayout(false);
            this.pnlApp.ResumeLayout(false);
            this.pnlApp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tbPersonInfo;
        private System.Windows.Forms.Button btnNext;
        private People.ctrlPersonCardFilter ctrlPCF;
        private System.Windows.Forms.TabPage tbAppInfo;
        private System.Windows.Forms.Panel pnlApp;
        private System.Windows.Forms.Label lblL_D_LAppID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbLC;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblFees;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label1;
    }
}
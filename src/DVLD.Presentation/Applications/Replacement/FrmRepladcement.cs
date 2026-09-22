using DVLD.Business.DTOs.Applications;
using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications;
using DVLD.Business.Services.Applications.LocalDrivingLicence;
using DVLD.Presentation.Drivers;
using DVLD.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Presentation.Applications.Replacement
{
    public partial class FrmRepladcement : Form
    {
        LocalLicenseDTO _info = null;
        decimal _DamagedFees = ApplicationTypesService.DTOById(4).AppFees;
        decimal _LostFees = ApplicationTypesService.DTOById(3).AppFees;

        ReplacmentReason _reason = ReplacmentReason.None;




        LocalLicenseService _service = new LocalLicenseService();
        public FrmRepladcement()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                errorProvider1.SetError(txtSearch, "Number only is allowed.");
            }
            else
            {
                errorProvider1.SetError(txtSearch, "");
            }
        }



        private void btnGet_Click(object sender, EventArgs e)
        {

            ctrlDriverLicenseInfo1.LoadPersonBy_LicenseID(int.Parse(txtSearch.Text));

            if (ctrlDriverLicenseInfo1.Tag == null)
                return;

            _info = (LocalLicenseDTO)ctrlDriverLicenseInfo1.Tag;
            lblOldLicenseID.Text = _info.ID.ToString();
            btnIssueReplacement.Enabled = true;
            qlblLicHistory.Enabled = true;

        }






        private void qlblEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FrmLicenseHistory history = new FrmLicenseHistory(_info.L_D_LApp.Application.PersonInfo.NationalNo);
            history.ShowDialog();

        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to Add Replacement for This License?", "Replacement", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            if (_reason == ReplacmentReason.None)
            {
                MessageBox.Show("Please Chosse Rplacment Reason Firest.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UIHelper.Execute(
            action: () =>
            {
                int PersonID = _info.L_D_LApp.Application.PersonInfo.ID;
                int CreatedByID = MainForm.CurrentUser.ID;
                int LocalLicID = _info.ID;
                int LicClassID = _info.L_D_LApp.LicenseClass.ID;


                _service.AddReplacment(_reason, PersonID, CreatedByID, LicClassID,LocalLicID, 
                    out int ReplacedLicID, out int ReplaceApplicationID, out int ReplaceLDL_AppID, out DateTime ApplicationDate);




                lblAppID.Text = ReplaceApplicationID.ToString();
                lblNewLicID.Text = ReplacedLicID.ToString();
                lblAppDate.Text = ApplicationDate.ToShortDateString();

                qlblShowLic.Tag = ReplaceLDL_AppID;
                qlblShowLic.Enabled = true;

                MessageBox.Show("Replacment Added successfully", "Replacment");
            });



        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamagedLicense.Checked)
            {
                lblAppFees.Text = _DamagedFees.ToString();
                _reason = ReplacmentReason.Damaged;

            }
            else if (rbLostLicense.Checked)
            {
                lblAppFees.Text = _LostFees.ToString();
                _reason = ReplacmentReason.Lost;

            }

        }

        private void History_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLicenseHistory history = new FrmLicenseHistory(_info.L_D_LApp.Application.PersonInfo.NationalNo);
            history.ShowDialog();


        }

        private void LicInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FrmDriverLicenseInfo info = new FrmDriverLicenseInfo((int)qlblShowLic.Tag);
            info.ShowDialog();


        }
    }
}

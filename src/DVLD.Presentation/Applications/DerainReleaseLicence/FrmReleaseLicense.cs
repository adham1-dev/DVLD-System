using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications;
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

namespace DVLD.Presentation.Applications.DerainReleaseLicence
{
    public partial class FrmReleaseLicense : Form
    {
        public delegate void ReturnedRowEventHandler(DataRow row, Mode mode);
        public event ReturnedRowEventHandler ReturnedRow;

        LocalLicenseDTO _info = null;
        DetainedLicenseDTO _detainedLicInfo;

        DetainedLicenseService _service;
        public FrmReleaseLicense(DetainedLicenseService s)
        {
            InitializeComponent();
            _service = s;
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

            if (_info.IsDetained == false)
            {
                MessageBox.Show("This License is not Detained.", "Info");

                return;
            }

            UIHelper.Execute(
            action: () =>
            {
                _detainedLicInfo = (DetainedLicenseDTO)_service.GetDTOById(_info.ID);

                lblLicenseID.Text = _detainedLicInfo.LocalLicenseID.ToString();
                lblDetainDate.Text = _detainedLicInfo.DetainDate.ToShortDateString();
                lblDetainID.Text = _detainedLicInfo.ID.ToString();

                decimal AppFees = ApplicationTypesService.DTOById(7).AppFees;
                decimal FineFees = _detainedLicInfo.FineFees;

                lblAppFees.Text = AppFees.ToString();
                lblFineFees.Text = FineFees.ToString();
                lblTotalFees.Text = (AppFees + FineFees).ToString();

                btnRelease.Enabled = true;
            });

        }



        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to Release This License?", "Release", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            if (_info == null)
            {
                MessageBox.Show("There is no Lisence to release.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(_detainedLicInfo == null)
            {
                MessageBox.Show("This Lisence not detained to release it.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            UIHelper.Execute(
            action: () =>
            {

                DetainedLicenseDTO DetainInfo = new DetainedLicenseDTO
                {
                    ID = _detainedLicInfo.ID,
                    ReleasedBy = MainForm.CurrentUser.ID,
                    PersonID = _info.L_D_LApp.Application.PersonInfo.ID,
                    LocalLicenseID =_info.ID, 
                };

                DataRow Row = _service.Release(DetainInfo);

                MessageBox.Show("Licsence Released successfully", "Release");

                lblAppID.Text = Row[7].ToString(); ;
                ctrlDriverLicenseInfo1.UpdateIsDetaine(!(bool)Row[2]);

                ReturnedRow?.Invoke(Row, Mode.AddNew);
                qlblShowLicHistory.Enabled = true;

            });
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FrmLicenseHistory history = new FrmLicenseHistory(_info.L_D_LApp.Application.PersonInfo.NationalNo);
            history.ShowDialog();

        }
    }
}

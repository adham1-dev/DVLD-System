using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Presentation.Applications.DerainReleaseLicence
{
    public partial class FrmDetainLicense : Form
    {
        public delegate void ReturnedRowEventHandler(DataRow row, Mode mode);
        public event ReturnedRowEventHandler ReturnedRow;

        LocalLicenseDTO _info = null;

        DetainedLicenseService _service;
        public FrmDetainLicense(DetainedLicenseService s)
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
            lblLicenseID.Text = _info.ID.ToString();
            btnDetained.Enabled = true;

        }


        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FrmLicenseHistory history = new FrmLicenseHistory(_info.L_D_LApp.Application.PersonInfo.NationalNo);
            history.ShowDialog();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to Detaine This License?", "Detaine", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            if (!decimal.TryParse(txtFineFees.Text, out decimal Fees))
            {
                MessageBox.Show("Add Fine Fees Value before save.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UIHelper.Execute(
            action: () =>
            {
                DetainedLicenseDTO DetainInfo = new DetainedLicenseDTO
                {
                    LocalLicenseID = _info.ID,
                    DetaindByUserID = MainForm.CurrentUser.ID,
                    FineFees = Fees,

                };

                DataRow Row = _service.DetaineLic(DetainInfo);

                lblDetainID.Text = Row[0].ToString();
                lblDetainDate.Text = ((DateTime)Row[4]).ToShortDateString();

                ReturnedRow?.Invoke(Row, Mode.AddNew);
                qlblShowLicHistory.Enabled = true;
                ctrlDriverLicenseInfo1.UpdateIsDetaine(!(bool)Row[2]);

                MessageBox.Show("Licsence Detained successfully", "Detaine");
            });

        }
    }
}

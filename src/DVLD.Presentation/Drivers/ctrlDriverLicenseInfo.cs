using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications.LocalDrivingLicence;
using DVLD.Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Drivers
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public LocalLicenseService _service = new LocalLicenseService();
        LocalLicenseDTO _LicInfo;
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        void _UiFromDTO(LocalLicenseDTO LicInfoDTO)
        {
            if (LicInfoDTO == null)
                return;


            lblClass.Text = LicInfoDTO.L_D_LApp.LicenseClass.ClassName;
            lblLicenseID.Text = LicInfoDTO.ID.ToString();
            lblExpirationDate.Text = LicInfoDTO.ExpirationDate.ToShortDateString();
            lblDriverID.Text = LicInfoDTO.DriverID.ToString();
            lblIsActive.Text = LicInfoDTO.IsActive == true ? "Yes" : "No" ;
            lblNotes.Text = LicInfoDTO.Notes;
            lblIssueReason.Text = LicInfoDTO.IssueReason;
            lblIssueDate.Text = LicInfoDTO.ReleaseDate.ToShortDateString();
            lblIsDetained.Text = LicInfoDTO.IsDetained == true ? "Yes" : "No";




            lblName.Text =        LicInfoDTO.L_D_LApp.Application.PersonInfo.FullName;
            lblNationalNo.Text =  LicInfoDTO.L_D_LApp.Application.PersonInfo.NationalNo;
            lblGender.Text =      LicInfoDTO.L_D_LApp.Application.PersonInfo.Gender == "Male" ? "Male" : "Female";
            lblDateOfBirth.Text = LicInfoDTO.L_D_LApp.Application.PersonInfo.DateOfBirth.ToShortDateString();


            if (File.Exists(LicInfoDTO.L_D_LApp.Application.PersonInfo.ImagePath))
            {
                //Remove old image (close)
                if (pbPersonImage.Image != null) pbPersonImage.Image.Dispose();

                //Add temp image from file (cached)
                using (Image tempImage = Image.FromFile(LicInfoDTO.L_D_LApp.Application.PersonInfo.ImagePath))
                {
                    pbPersonImage.Image = new Bitmap(tempImage);
                }
            }
            else
            {
                pbPersonImage.Image = (LicInfoDTO.L_D_LApp.Application.PersonInfo.Gender == "Male") ? Resources.man : Resources.woman;
            }



        }

        public void UpdateIsDetaine(bool isDetaine)
        {
            lblIsDetained.Text = isDetaine == true ? "Yes" : "No";
        }
        public void LoadPersonBy_LDLApp_ID(int L_D_LAppID)
        {
            Tag = null;
            if (L_D_LAppID <= 0)
                return;

            _LicInfo = (LocalLicenseDTO)_service.GetDTOBy_LDLApp_Id(L_D_LAppID);
            Tag = _LicInfo;

            if (_LicInfo == null)
            {
                MessageBox.Show("This License dose not exist", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_LicInfo.ID > 0)
            {
                _UiFromDTO(_LicInfo);
            }

        }

        public void LoadPersonBy_LicenseID(int LicenseID)
        {
            Tag = null;
            if (LicenseID <= 0)
                return;

            _LicInfo = (LocalLicenseDTO)_service.GetDTOById(LicenseID);
            Tag = _LicInfo;

            if (_LicInfo == null)
            {
                MessageBox.Show("This License dose not exist", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_LicInfo.ID > 0)
            {
                _UiFromDTO(_LicInfo);
            }

        }



    }
}

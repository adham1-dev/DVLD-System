using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Services;
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

namespace DVLD.Presentation.Applications.L_D_LApp
{
    public partial class ctrlLDL_ApplicationInfo : UserControl
    {
        L_D_LAppDTO _D_LAppDTO;
        public ctrlLDL_ApplicationInfo()
        {
            InitializeComponent();
        }

        void _UiFromDTO (L_D_LAppDTO DTO)
        {
            //LDL App
            lblLDL_ID.Text = DTO.ID.ToString();
            lblLicenseClass.Text = DTO.LicenseClass.ClassName.ToString();
            lblPassedTests.Text = DTO.PassedTests.ToString() + "/3";

            if(DTO.PassedTests == 3) qlblShowLicenseInfo.Enabled = true;
            else qlblShowLicenseInfo.Enabled = false;

            //General App
            lblID.Text = DTO.Application.ID.ToString();

            if (DTO.Application.ID > 0) qlblViewPersonInfo.Enabled = true;
            else
            {
                qlblViewPersonInfo.Enabled = false;
                return;
            }

            lblStatus.Text = DTO.Application.ApplicationStatus.ToString();
            lblFees.Text = DTO.Application.AppType.AppFees.ToString();
            lblType.Text = DTO.Application.AppType.AppTitle.ToString();
            lblApplicant.Text = DTO.Application.PersonInfo.FullName;
            lblDate.Text = DTO.Application.Date.ToString();
            lblCreatedBy.Text = DTO.Application.CreatedBy.UserName;
        }


        public void UpdatePassedTest(int num)
        {
            lblPassedTests.Text = num + "/3";
        }

        public void LoadAppInfo (L_D_LAppDTO DTO)
        {
            if (DTO.Application.ID > 0 && DTO != null)
            {
                _D_LAppDTO = DTO;
                _UiFromDTO(DTO);
            }
            else
                MessageBox.Show("this App dosn't have enough enformation to display");
        }

        void UpdatePersonInfo (PersonDTO DTO)
        {
            _D_LAppDTO.Application.PersonInfo = DTO;
            _UiFromDTO(_D_LAppDTO);

        }

        private void qlblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonInfo personInfo = new frmPersonInfo(_D_LAppDTO.Application.PersonInfo.ID, new PeopleService());
            personInfo.PersonDTOBack += UpdatePersonInfo;
            personInfo.ShowDialog();
        }
    }
}

using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications.L_D_LApp;
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
    public partial class FrmAddUpdateNewL_D_LApplication : Form
    {
        public delegate void LDL_AppReturnedRowHandler(DataRow row, Mode mode);
        public event LDL_AppReturnedRowHandler LDL_AppReturnedRow;
        public delegate void LDL_AppDTOReturnedHandler(L_D_LAppDTO userDTO);
        public event LDL_AppDTOReturnedHandler LDL_AppDTOReturned;


        L_D_LAppDTO _AppInfo = new L_D_LAppDTO();
        L_D_LAppService _service;

        public FrmAddUpdateNewL_D_LApplication(L_D_LAppService s)
        {
            InitializeComponent();
            _service = s;
            cmbLC.DataSource = LicenseClassesService.AllLicenseClass;
            cmbLC.DisplayMember = "ClassName";
            cmbLC.ValueMember = "LicenseClassID";
            lblFees.Text = ApplicationTypesService.DTOById(1).AppFees.ToString();
            lblCreatedBy.Text = MainForm.CurrentUser.UserName.ToString();
        }

        public FrmAddUpdateNewL_D_LApplication(L_D_LAppDTO L_D_LAppInfo, L_D_LAppService s)
        {
            InitializeComponent();
            _service = s;
            _AppInfo = L_D_LAppInfo;
            lblTop.Text = "Update Person";
            cmbLC.DataSource = LicenseClassesService.AllLicenseClass;
            cmbLC.DisplayMember = "ClassName";
            cmbLC.ValueMember = "LicenseClassID";

            ctrlPCF.LoadStaticPerson(L_D_LAppInfo.Application.PersonInfo);

            lblL_D_LAppID.Text = L_D_LAppInfo.ID.ToString();
            lblDate.Text = L_D_LAppInfo.Application.Date.ToString();
            cmbLC.SelectedValue = L_D_LAppInfo.LicenseClass.ID;
            lblFees.Text = L_D_LAppInfo.Application.AppType.AppFees.ToString();
            lblCreatedBy.Text = L_D_LAppInfo.Application.CreatedBy.UserName;
        }




        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPCF.Tag == null)
            {
                MessageBox.Show("You must get person First", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabs.SelectedTab = tbAppInfo;

        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedTab != tbAppInfo)
                return;

            if (ctrlPCF.Tag == null)
            {
                pnlApp.Enabled = false;
                MessageBox.Show("You must get person First", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabs.SelectedTab = tbPersonInfo;
                return;
            }

            PersonDTO pesonInfo = (PersonDTO)ctrlPCF.Tag;
            if (pesonInfo.ID > 0)
            {
                if (ctrlPCF.IsLocked == false)
                {
                    if (MessageBox.Show($"Are you sure you want to make New Local Driving License for {pesonInfo.FullName}?", "Save", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        ctrlPCF.IsLocked = true;
                        _AppInfo.Application.PersonInfo = pesonInfo;
                        pnlApp.Enabled = true;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        tabs.SelectedTab = tbPersonInfo;
                    }
                }
                else if (ctrlPCF.IsLocked == true)
                {
                    if (int.TryParse(lblL_D_LAppID.Text, out int UserID) && UserID > 0)
                    {
                        pnlApp.Enabled = true;
                        btnSave.Enabled = true;
                    }

                }
            }
            else
            {
                MessageBox.Show("not valid person", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to {lblTop.Text} for {_AppInfo.Application.PersonInfo.FullName}?", "Save", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            _AppInfo.Application.PersonInfo = (PersonDTO)ctrlPCF.Tag;
            _AppInfo.LicenseClass.ID = cmbLC.SelectedIndex;
            _AppInfo.LicenseClass.ClassName = cmbLC.Text;
            _AppInfo.Application.CreatedBy = MainForm.CurrentUser;


            UpdateAdd LDL_App = new UpdateAdd(_AppInfo, _service);

            DataRow LDL_AppRow;

            if (LDL_App.currentMode == Mode.AddNew)
            {
                LDL_AppRow = LDL_App.Save();
                if (_AppInfo.ID != default)
                {
                    LDL_AppReturnedRow?.Invoke(LDL_AppRow, Mode.AddNew);
                    lblTop.Text = "Update Local Driving License Application";
                    lblL_D_LAppID.Text = _AppInfo.ID.ToString();
                    lblDate.Text = LDL_AppRow["ApplicationDate"].ToString();
                    LDL_AppDTOReturned?.Invoke(_AppInfo);
                }
            }
            else if (LDL_App.currentMode == Mode.Updated)
            {
                LDL_AppRow = LDL_App.Save();
                LDL_AppReturnedRow?.Invoke(LDL_AppRow, Mode.Updated);
                LDL_AppDTOReturned?.Invoke(_AppInfo);
            }

        }
    }
}

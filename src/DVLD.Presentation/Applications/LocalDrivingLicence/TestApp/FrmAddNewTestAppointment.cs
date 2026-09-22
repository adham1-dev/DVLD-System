using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.TestApp;
using DVLD.Business.Services.Applications.TestApp;
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
using static System.Windows.Forms.AxHost;

namespace DVLD.Presentation.Applications.TestApp
{
    public partial class FrmAddNewTestAppointment : Form
    {
        public delegate void TestAppReturnedRowHandler(DataRow row, Mode mode);
        public event TestAppReturnedRowHandler TestAppReturnedRow;
        public delegate void TestAppDTOReturnedHandler(TestAppointmentDTO userDTO);
        public event TestAppDTOReturnedHandler TestAppDTOReturned;



        TestAppointmentDTO _testAppInfo;
        TestAppointmentService _service;
        public FrmAddNewTestAppointment( int TrialNumber ,TestAppointmentDTO Info , TestAppointmentService s)
        {
            InitializeComponent();
            if(Info.RetakeApp == null)
            {
                groupBox1.Enabled = false;
                lblTop.Text = "Schedule Test";
            }
            else
            {

                lblTop.Text = "Schedule Retake Test";
            }

            gb.Text = Info.TestType.TestTitle.ToString();


            _testAppInfo = Info;
            _service = s;


            lblLDL_ID.Text = _testAppInfo.L_D_LApp.ID.ToString();
            lblClass.Text = _testAppInfo.L_D_LApp.LicenseClass.ClassName;
            lblName.Text = _testAppInfo.L_D_LApp.Application.PersonInfo.FullName;
            lblTrial.Text = TrialNumber.ToString();
            dtDate.Value = _testAppInfo.L_D_LApp.Application.Date;
            lblFees.Text = _testAppInfo.TestType?.TestFees.ToString();
            

            lblR_AppFees.Text = _testAppInfo.RetakeApp?.AppType?.AppFees.ToString() ?? "0";
            lblR_AppID.Text = _testAppInfo.RetakeApp?.ID.ToString() ?? "__";

            lblTotalFees.Text = _testAppInfo.PaidFees.ToString();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to save test?", "Save", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            _testAppInfo.ID = default;
            _testAppInfo.TestDate = dtDate.Value;

            UpdateAdd Test = new UpdateAdd(_testAppInfo, _service);

            DataRow testAppRow;

            if (Test.currentMode == Mode.AddNew)
            {
                testAppRow = Test.Save();
                if (_testAppInfo.ID != default)
                {
                    TestAppReturnedRow?.Invoke(testAppRow, Mode.AddNew);
                    TestAppDTOReturned?.Invoke(_testAppInfo);
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

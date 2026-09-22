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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DVLD.Presentation.Applications.TestApp
{
    public partial class FrmUpdateTestAppointment : Form
    {
        public delegate void TestAppReturnedRowHandler(DataRow row, Mode mode);
        public event TestAppReturnedRowHandler TestAppReturnedRow;
        public delegate void TestAppDTOReturnedHandler(TestAppointmentDTO userDTO);
        public event TestAppDTOReturnedHandler TestAppDTOReturned;



        TestAppointmentDTO _testAppInfo;
        TestAppointmentService _service;
        public FrmUpdateTestAppointment(TestAppointmentDTO Info, TestAppointmentService s)
        {
            InitializeComponent();


            _testAppInfo = Info;
            _service = s;


            lblLDL_ID.Text = _testAppInfo.L_D_LApp.ID.ToString();
            lblClass.Text = _testAppInfo.L_D_LApp.LicenseClass.ClassName;
            lblName.Text = _testAppInfo.L_D_LApp.Application.PersonInfo.FullName;
            dtDate.Value = _testAppInfo.TestDate.Date;
            lblFees.Text = _testAppInfo.TestType?.TestFees.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to Update this test?", "Save", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            _testAppInfo.TestDate = dtDate.Value;

            UpdateAdd Test = new UpdateAdd(_testAppInfo, _service);

            DataRow testAppRow;

            if (Test.currentMode == Mode.Updated)
            {
                testAppRow = Test.Save();
                if (_testAppInfo.ID != default)
                {
                    TestAppReturnedRow?.Invoke(testAppRow, Mode.Updated);
                    TestAppDTOReturned?.Invoke(_testAppInfo);
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }

        }
    }
}

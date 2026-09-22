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

namespace DVLD.Presentation.Applications.TestApp
{
    public partial class FrmTakeTest : Form
    {
        public delegate void TestAppReturnedRowHandler(DataRow row, Mode mode);
        public event TestAppReturnedRowHandler TestAppReturnedRow;
        public delegate void TestAppDTOReturnedHandler(TestAppointmentDTO userDTO);
        public event TestAppDTOReturnedHandler TestAppDTOReturned;

        TestAppointmentDTO _testAppInfo;
        TestAppointmentService _service;

        public FrmTakeTest( TestAppointmentDTO Info, TestAppointmentService s)
        {
            InitializeComponent();

            gb.Text = Info.TestType.TestTitle.ToString();


            _testAppInfo = Info;
            _service = s;


            lblLDL_ID.Text = _testAppInfo.L_D_LApp.ID.ToString();
            lblClass.Text = _testAppInfo.L_D_LApp.LicenseClass.ClassName;
            lblName.Text = _testAppInfo.L_D_LApp.Application.PersonInfo.FullName;
            dtDate.Value = _testAppInfo.L_D_LApp.Application.Date;
            lblFees.Text = _testAppInfo.TestType?.TestFees.ToString();


        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rbPass.Checked == rbFail.Checked)
            {
                MessageBox.Show("Choose a result for this test first before saving.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to save Result?", "Save", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;


            _testAppInfo.TestResult = rbPass.Checked;


            UpdateAdd Test = new UpdateAdd(_testAppInfo, _service);

            DataRow testAppRow;

            if (Test.currentMode == Mode.Updated)
            {
                testAppRow = Test.Save();
                if (_testAppInfo.ID != default)
                {
                    TestAppReturnedRow?.Invoke(testAppRow, Mode.Updated);
                    if (_testAppInfo.TestResult == true)
                    {
                        _testAppInfo.L_D_LApp.PassedTests++;
                        TestAppDTOReturned?.Invoke(_testAppInfo);
                    }
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

using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.TestApp;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications.L_D_LApp;
using DVLD.Business.Services.Applications.TestApp;
using DVLD.Presentation.Applications.L_D_LApp;
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
    public partial class FrmManageTestAppointment : Form
    {
        L_D_LAppService _LD_Service;
        private TestAppointmentService _service;
        DataView view ;

        UIHelper _Helper;
        TestAppointmentDTO _testAppInfo = new TestAppointmentDTO();
        void AddUpateRowInView(DataRow row, Mode mode)
        {
            if (row == null) return;

            if (mode == Mode.AddNew)
            {
                string firstColumnName = view.Table.Columns[0].ColumnName;
                view.Sort = firstColumnName + " ASC";
                view.Table.Rows.Add(row.ItemArray);
                this.BindingContext[view].Position = view.Count - 1;
                lblRowsCount.Text = dgv.RowCount.ToString();
            }
            else if (mode == Mode.Updated)
            {
                DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
                DataRow currentRow = currentRowView.Row;

                foreach (DataColumn col in currentRow.Table.Columns) col.ReadOnly = false;
                currentRow.ItemArray = row.ItemArray;
                currentRow.AcceptChanges();
            }
        }
        void _uiRefresh()
        {
            dgv.DataSource = view;
            lblRowsCount.Text = dgv.RowCount.ToString();
        }

        public FrmManageTestAppointment(int LDapp_ID,int TestTypeID, L_D_LAppService s)
        {
            InitializeComponent();
            _LD_Service = s;
            _service = new TestAppointmentService(_LD_Service);
            _Helper = new UIHelper(_service);

            _testAppInfo.L_D_LApp = (L_D_LAppDTO)_LD_Service.GetDTOById(LDapp_ID);
            _testAppInfo.TestType = TestTypesService.DTOById(TestTypeID);

            lblTop.Text = _testAppInfo.TestType.TestTitle + " Appointments";

            view = new DataView(_service.GetAppointmentView(_testAppInfo.L_D_LApp.ID, _testAppInfo.TestType.ID));

            ctrlLDL_ApplicationInfo1.LoadAppInfo(_testAppInfo.L_D_LApp);
            //compoBox fill by filter
            _uiRefresh();

        }

        private void btnAddApp_Click(object sender, EventArgs e)
        {
            UIHelper.Execute(
            action: () =>
            {
                _service.Validate(_testAppInfo);
                FrmAddNewTestAppointment Add = new FrmAddNewTestAppointment(int.Parse(lblRowsCount.Text), _testAppInfo, _service);
                Add.TestAppReturnedRow += AddUpateRowInView;
                Add.ShowDialog();
            });

        }


        private void dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv.ClearSelection();

                dgv.Rows[e.RowIndex].Selected = true;

                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    bool IsLocked = (bool)dgv.CurrentRow.Cells["Is Locked"].Value;
                    mniEdit.Enabled = !IsLocked;
                    mniTakeTest.Enabled = !IsLocked;
                }
            }
        }

        private void mniEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int id = (int)dgv.CurrentRow.Cells[0].Value;

                    FrmUpdateTestAppointment Edit = new FrmUpdateTestAppointment((TestAppointmentDTO)_service.GetDTOById(id), _service);
                    Edit.TestAppReturnedRow += AddUpateRowInView;
                    Edit.ShowDialog();

                }
            }

        }


        void PassedTest (TestAppointmentDTO DTO)
        {
            ctrlLDL_ApplicationInfo1.UpdatePassedTest(DTO.L_D_LApp.PassedTests);
        }


        private void mniTakeTest_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int id = (int)dgv.CurrentRow.Cells[0].Value;

                    FrmTakeTest Take = new FrmTakeTest((TestAppointmentDTO)_service.GetDTOById(id), _service);
                    Take.TestAppDTOReturned += PassedTest;
                    Take.TestAppReturnedRow += AddUpateRowInView;
                    Take.ShowDialog();
                }
            }

        }
    }
}

using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Services;
using DVLD.Business.Services.Applications.L_D_LApp;
using DVLD.Presentation.Applications.LocalDrivingLicence;
using DVLD.Presentation.Applications.TestApp;
using DVLD.Presentation.Drivers;
using DVLD.Presentation.Generic;
using DVLD.Presentation.Users;
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

namespace DVLD.Presentation.Applications.L_D_LApp
{

    public enum AppFilter { L_D_LAppID, DrivingClass, NationalNo, FullName, ApplicationDate, PassedTests, Status }

    public partial class FrmManageLocalLicenseApplications : Form
    {
        L_D_LAppService _service = new L_D_LAppService();
        UIHelper _Helper;
        DataView view;
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
        void _uiRefresh(int targetID = -1)
        {
            _Helper.FillDGV(dgv, view, cbFilter.Text, txtSearch.Text);
            lblRowsCount.Text = dgv.RowCount.ToString();
            if (targetID != -1)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if ((int)row.Cells["L_D_LAppID"].Value == targetID)
                    {
                        dgv.ClearSelection();
                        row.Selected = true;
                        dgv.CurrentCell = row.Cells[0];
                        row.Selected = true;
                        dgv.Focus();
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        public FrmManageLocalLicenseApplications()
        {
            InitializeComponent();
            _Helper = new UIHelper(_service);
            view = new DataView(_service.GetView());
            //compoBox fill by filter
            cbFilter.DataSource = Enum.GetValues(typeof(AppFilter));
            _uiRefresh();

        }

        private void btnAddApp_Click(object sender, EventArgs e)
        {
            FrmAddUpdateNewL_D_LApplication add = new FrmAddUpdateNewL_D_LApplication(_service);
            add.LDL_AppReturnedRow += AddUpateRowInView;
            add.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _uiRefresh();
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int Id = (int)dgv.CurrentRow.Cells[0].Value;

                    FrmAddUpdateNewL_D_LApplication EditInfo = new FrmAddUpdateNewL_D_LApplication((L_D_LAppDTO)_Helper.GetDTO(Id), _service);
                    EditInfo.LDL_AppReturnedRow += AddUpateRowInView;
                    EditInfo.ShowDialog();
                }
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                int Id = (int)dgv.CurrentRow.Cells[0].Value;

                if (MessageBox.Show("Are you sure do you want to delete this Applecation ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_Helper.Delete(Id))
                    {
                        DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
                        currentRowView.Row.Delete();
                        lblRowsCount.Text = dgv.RowCount.ToString();

                        MessageBox.Show("Applecation deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("Falid to delete this Applecation");
                    }
                }
            }

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int Id = (int)dgv.CurrentRow.Cells[0].Value;
                    if (MessageBox.Show("Are you sure do you want to Cancel this Applecation ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AddUpateRowInView(_service.CancelApplecation(Id), Mode.Updated);
                        MessageBox.Show("Applecation Canceld successfully");

                    }
                    else
                    {
                        MessageBox.Show("Falid to Cancel this Applecation");
                    }

                }
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int Id = (int)dgv.CurrentRow.Cells[0].Value;
                    FrmLocalDrivingLicenseAppInfo info = new FrmLocalDrivingLicenseAppInfo((L_D_LAppDTO)_service.GetDTOById(Id));
                    info.ShowDialog();
                }
            }
        }

        void EnableTest (byte PassedTests)
        {
            mniVisionTest.Enabled = PassedTests == 0;
            mniWrittenTest.Enabled = PassedTests == 1;
            mniStreetTest.Enabled = PassedTests == 2;
        }


        private void cmOperations_Opening(object sender, CancelEventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    string Status = dgv.CurrentRow.Cells["Status"].Value.ToString();
                    if(Status == "Issued")
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                        showLicenseToolStripMenuItem.Enabled = true;
                        return;
                    }
                    else
                    {
                        if (!byte.TryParse(dgv.CurrentRow.Cells["PassedTests"].Value?.ToString(), out byte PassedTests))
                            return;
                        if (PassedTests == 3)
                        {
                            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                            showLicenseToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                            showLicenseToolStripMenuItem.Enabled = false;
                        }
                        EnableTest(PassedTests);
                    }
                }
            }

        }


        void ScheduleTest(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    byte PassedTests = (byte)dgv.CurrentRow.Cells["PassedTests"].Value;

                    int Id = (int)dgv.CurrentRow.Cells[0].Value;
                    FrmManageTestAppointment Test = new FrmManageTestAppointment(Id, PassedTests + 1, _service);
                    Test.ShowDialog();

                    view = new DataView(_service.GetView());
                    _uiRefresh(Id);

                }
            }

        }



        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    byte PassedTests = (byte)dgv.CurrentRow.Cells["PassedTests"].Value;

                    int Id = (int)dgv.CurrentRow.Cells[0].Value;
                    FrmIssueDrivingLicence issue = new FrmIssueDrivingLicence(Id, _service);
                    issue.ShowDialog();

                    view = new DataView(_service.GetView());
                    _uiRefresh(Id);

                }
            }

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int Id = (int)dgv.CurrentRow.Cells["L_D_LAppID"].Value;
                    FrmDriverLicenseInfo info = new FrmDriverLicenseInfo(Id);
                    info.ShowDialog();
                }
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    string NoNum = dgv.CurrentRow.Cells["NationalNo"].Value.ToString();
                    FrmLicenseHistory info = new FrmLicenseHistory(NoNum);
                    info.ShowDialog();
                }
            }

        }
    }
}

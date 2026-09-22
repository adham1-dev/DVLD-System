using DVLD.Business.DTOs;
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

namespace DVLD.Presentation.Users
{
    public enum UsersFilter { UserID, UserName, IsActive }

    public partial class FrmManageUsers : Form
    {
        UsersService _service = new UsersService();
        UIHelper _userUI;
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
        void _uiRefresh()
        {
            _userUI.FillDGV(dgv, view, cbFilter.Text, txtSearch.Text);
            lblRowsCount.Text = dgv.RowCount.ToString();
        }
        public FrmManageUsers()
        {
            InitializeComponent();
            _userUI = new UIHelper(_service);
            view = new DataView(_service.GetView());
            //compoBox fill by filter
            cbFilter.DataSource = Enum.GetValues(typeof(UsersFilter));
            _uiRefresh();
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _uiRefresh();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            FrmAddUpdateNewUser AddUpdate = new FrmAddUpdateNewUser(_service);
            AddUpdate.UserReturnedRow += AddUpateRowInView;
            AddUpdate.ShowDialog();
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                int UserId = (int)dgv.CurrentRow.Cells["UserID"].Value;

                if (MessageBox.Show("Are you sure do you want to delete this user ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_userUI.Delete(UserId))
                    {
                        DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
                        currentRowView.Row.Delete();
                        lblRowsCount.Text = dgv.RowCount.ToString();

                        MessageBox.Show("User deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("Falid to delete this User");
                    }
                }
            }

        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddUser_Click(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int userId = (int)dgv.CurrentRow.Cells["UserID"].Value;

                    FrmAddUpdateNewUser EditInfo = new FrmAddUpdateNewUser((UserDTO)_userUI.GetDTO(userId), _service);
                    EditInfo.UserReturnedRow += AddUpateRowInView;
                    EditInfo.ShowDialog();
                }
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int userId = (int)dgv.CurrentRow.Cells["UserID"].Value;

                    frmUserInfo Info = new frmUserInfo(userId, _service);
                    Info.ShowDialog();
                }
            }

        }
    }
}

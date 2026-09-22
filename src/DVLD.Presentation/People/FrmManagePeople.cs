using DVLD.Business.DTOs;
using DVLD.Business.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Generic
{
    public enum PeopleFilter { PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, Gender, DateOfBirth, Nationality, Phone, Email }

    public partial class FrmManagePeople : Form
    {
        PeopleService _service = new PeopleService();
        UIHelper _personUI;
        DataView view;
        void AddUpateRowInView(DataRow row, Mode mode)
        {
            if (row == null) return;

            if(mode == Mode.AddNew)
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
            _personUI.FillDGV(dgv, view, cbFilter.Text, txtSearch.Text);
            lblRowsCount.Text = dgv.RowCount.ToString();
        }
        public FrmManagePeople()
        {
            InitializeComponent();
            _personUI = new UIHelper(_service);
            view = new DataView(_service.GetView());
            //compoBox fill by filter
            cbFilter.DataSource = Enum.GetValues(typeof(PeopleFilter));
            _uiRefresh();
        }




        

        //reftesh
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _uiRefresh();
        }



        //add update
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            FrmAddOrUpdateNewPerson addUpdate = new FrmAddOrUpdateNewPerson(_service);
            addUpdate.PersonReturnedRow += AddUpateRowInView;
            addUpdate.ShowDialog();
        }
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddPerson_Click(sender, e);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int personId = (int)dgv.CurrentRow.Cells["PersonID"].Value;

                    FrmAddOrUpdateNewPerson EditInfo = new FrmAddOrUpdateNewPerson((PersonDTO)_personUI.GetDTO(personId), _service);
                    EditInfo.PersonReturnedRow += AddUpateRowInView;
                    EditInfo.ShowDialog();
                }
            }

        }





        //update from tool strip
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                if (dgv.CurrentRow != null)
                {
                    int personId = (int)dgv.CurrentRow.Cells["PersonID"].Value;

                    frmPersonInfo personInfo = new frmPersonInfo(personId, _service);
                    personInfo.PersonRowBack += AddUpateRowInView;
                    personInfo.ShowDialog();

                }
            }
        }

        //delete
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                int personId = (int)dgv.CurrentRow.Cells["PersonID"].Value;

                if (MessageBox.Show("Are you sure do you want to delete this person ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_personUI.Delete(personId))
                    {
                        DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
                        currentRowView.Row.Delete();
                        lblRowsCount.Text = dgv.RowCount.ToString();

                        MessageBox.Show("Person deleted successfully");
                    }
                    else
                    {
                        MessageBox.Show("Falid to delete this person");
                    }
                }
            }
        }


        //select whole row
        private void dgvPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv.ClearSelection();

                dgv.Rows[e.RowIndex].Selected = true;

                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

    }
}

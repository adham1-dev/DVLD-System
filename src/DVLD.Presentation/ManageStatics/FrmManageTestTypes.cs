using DVLD.Business.DTOs;
using DVLD.Business.Services;
using DVLD.Presentation.Generic;
using DVLD.Presentation.ManageStatics.UpdateStatics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.ManageStatics
{
    public partial class FrmManageTestTypes : Form
    {
        TestTypesService _service = new TestTypesService();

        public FrmManageTestTypes()
        {
            InitializeComponent();
            dgv.DataSource = _service.GetView();
        }
        void UpateRowInView(DataRow row)
        {
            if (row == null) return;

            DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
            DataRow currentRow = currentRowView.Row;

            foreach (DataColumn col in currentRow.Table.Columns) col.ReadOnly = false;
            currentRow.ItemArray = row.ItemArray;
            currentRow.AcceptChanges();
        }

        TestTypeDTO CurrentRow2DTO ()
        {
            DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
            DataRow currentRow = currentRowView.Row;

            return new TestTypeDTO
            {
                ID = (int)currentRow["TestTypeID"],
                TestTitle = (string)currentRow["TestTitle"],
                Description = (string)currentRow["Description"],
                TestFees = (decimal)currentRow["TestFees"],
            };
        }


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUpdateTestTypes edit = new FrmUpdateTestTypes(CurrentRow2DTO(), _service);
            edit.ReturndRow += UpateRowInView;
            edit.ShowDialog();
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
    }
}

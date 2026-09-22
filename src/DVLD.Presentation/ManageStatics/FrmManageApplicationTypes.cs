using DVLD.Business.DTOs;
using DVLD.Business.Services;
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
    public partial class FrmManageApplicationTypes : Form
    {
        ApplicationTypesService _service = new ApplicationTypesService();
        public FrmManageApplicationTypes()
        {
            InitializeComponent();
            dgv.DataSource = _service.GetView();
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
        void UpateRowInView(DataRow row)
        {
            if (row == null) return;

            DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
            DataRow currentRow = currentRowView.Row;

            foreach (DataColumn col in currentRow.Table.Columns) col.ReadOnly = false;
            currentRow.ItemArray = row.ItemArray;
            currentRow.AcceptChanges();
        }

        ApplicationTypeDTO CurrentRow2DTO()
        {
            DataRowView currentRowView = (DataRowView)dgv.CurrentRow.DataBoundItem;
            DataRow currentRow = currentRowView.Row;

            return new ApplicationTypeDTO
            {
                ID = (int)currentRow["ApplicationTypeID"],
                AppTitle = (string)currentRow["AppTitle"],
                AppFees = (decimal)currentRow["AppFees"],
            };
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUpdateApplicationTypes edit = new FrmUpdateApplicationTypes(CurrentRow2DTO(), _service);
            edit.ReturndRow += UpateRowInView;
            edit.ShowDialog();
        }
    }
}

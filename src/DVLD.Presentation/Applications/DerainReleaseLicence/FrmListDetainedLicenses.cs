using DVLD.Business.Services;
using DVLD.Business.Services.Applications;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD.Presentation.Applications.DerainReleaseLicence
{
    public enum PeopleFilter { DetainedLicenseID, LocalLicenseID, IsReleased, FineFees, ReleaseDate, NationalNo, FullName, ReleaseAppID }

    public partial class FrmListDetainedLicenses : Form
    {

        DetainedLicenseService _service = new DetainedLicenseService();
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
        void _uiRefresh()
        {
            _Helper.FillDGV(dgv, view, cbFilter.Text, txtSearch.Text);
            lblRowsCount.Text = dgv.RowCount.ToString();
        }
        public FrmListDetainedLicenses()
        {
            InitializeComponent();
            _Helper = new UIHelper(_service);
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

        private void dgvPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv.ClearSelection();

                dgv.Rows[e.RowIndex].Selected = true;

                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            FrmDetainLicense detain = new FrmDetainLicense(_service);
            detain.ReturnedRow += AddUpateRowInView;
            detain.ShowDialog();
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            FrmReleaseLicense release = new FrmReleaseLicense(_service);
            release.ReturnedRow += AddUpateRowInView;
            release.ShowDialog();

        }
    }
}

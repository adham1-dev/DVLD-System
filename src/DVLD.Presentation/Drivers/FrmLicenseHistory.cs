using DVLD.Business.Services.Applications.LocalDrivingLicence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Drivers
{
    public partial class FrmLicenseHistory : Form
    {
        LocalLicenseService _service = new LocalLicenseService();
        public FrmLicenseHistory(String NoNum)
        {
            InitializeComponent();
            if(NoNum != null)
            {
                ctrlPersonCard1.LoadPersonByNoNum(NoNum);
                dgv.DataSource = _service.GetViewByPersonID(ctrlPersonCard1._person.ID);
                lblRowsCount.Text = dgv.RowCount.ToString();
            }
            else
            {
                MessageBox.Show("No person selected to get.", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

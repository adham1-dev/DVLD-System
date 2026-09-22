using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Services.Applications.L_D_LApp;
using DVLD.Business.Services.Applications.LocalDrivingLicence;
using DVLD.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Applications.LocalDrivingLicence
{
    public partial class FrmIssueDrivingLicence : Form
    {
        LocalLicenseService _service = new LocalLicenseService();
        L_D_LAppService _LD_Service;
        L_D_LAppDTO _LD_Info;

        public FrmIssueDrivingLicence(int LDapp_ID, L_D_LAppService s)
        {
            _LD_Service = s;
            InitializeComponent();
            _LD_Info = (L_D_LAppDTO)_LD_Service.GetDTOById(LDapp_ID);
            ctrlLDL_ApplicationInfo1.LoadAppInfo(_LD_Info);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LocalLicenseDTO DTO = new LocalLicenseDTO
            {
                L_D_LApp = _LD_Info,
                Notes = txtNotes.Text,
            };

            UIHelper.Execute(
            action: () =>
            {
                int IssuiedLicenseID = _service.AddNew(DTO);
                MessageBox.Show("License Issued successfully whit license ID = " + IssuiedLicenseID, "Succeeded");
            });

        }
    }
}

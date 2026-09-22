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
    public partial class FrmDriverLicenseInfo : Form
    {
        public FrmDriverLicenseInfo(int L_D_LAppID)
        {
            InitializeComponent();
            ctrlDriverLicenseInfo1.LoadPersonBy_LDLApp_ID(L_D_LAppID);
        }
    }
}

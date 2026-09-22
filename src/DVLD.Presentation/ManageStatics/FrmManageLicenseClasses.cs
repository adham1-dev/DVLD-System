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

namespace DVLD.Presentation.ManageStatics
{
    public partial class FrmManageLicenseClasses : Form
    {
        LicenseClassesService _service = new LicenseClassesService();


        public FrmManageLicenseClasses()
        {
            InitializeComponent();
            dgv.DataSource = _service.GetView();
        }
    }
}

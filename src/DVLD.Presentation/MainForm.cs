using DVLD.Business.DTOs;
using DVLD.Business.Services;
using DVLD.Presentation.Applications.DerainReleaseLicence;
using DVLD.Presentation.Applications.L_D_LApp;
using DVLD.Presentation.Applications.Replacement;
using DVLD.Presentation.Generic;
using DVLD.Presentation.ManageStatics;
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

namespace DVLD.Presentation
{
    public partial class MainForm : Form
    {
        ApplicationTypesService applicationTypesService = new ApplicationTypesService();
        LicenseClassesService licenseClasses = new LicenseClassesService();
        CountryService countryService = new CountryService();
        TestTypesService testTypesService = new TestTypesService();

        static public UserDTO CurrentUser;


        public MainForm(UserDTO currentUser)
        {

            CurrentUser = currentUser;
            InitializeComponent();

        }


        private void ToggleControls(Control parent, bool isEnabled)
        {
            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Enabled = isEnabled;

                if (ctrl.HasChildren)
                {
                    ToggleControls(ctrl, isEnabled);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmManagePeople managePeople = new FrmManagePeople();
            managePeople.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmManageUsers manageUsers = new FrmManageUsers();
            manageUsers.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmManageTestTypes test = new FrmManageTestTypes();
            test.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmManageApplicationTypes app = new FrmManageApplicationTypes();
            app.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmManageLicenseClasses lc = new FrmManageLicenseClasses();
            lc.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmManageLocalLicenseApplications mlc = new FrmManageLocalLicenseApplications();
            mlc.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FrmListDetainedLicenses detaine = new FrmListDetainedLicenses();
            detaine.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrmRepladcement repladcement = new FrmRepladcement();
            repladcement.ShowDialog();
        }

    }
}

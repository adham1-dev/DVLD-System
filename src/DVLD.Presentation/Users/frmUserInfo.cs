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

namespace DVLD.Presentation.Users
{
    public partial class frmUserInfo : Form
    {
        public frmUserInfo(int Id)
        {
            InitializeComponent();
            ctrlUC.LoadUserData(Id);
        }
        public frmUserInfo(int Id , UsersService service)
        {
            InitializeComponent();
            ctrlUC._service = service;
            ctrlUC.LoadUserData(Id);
        }
    }
}

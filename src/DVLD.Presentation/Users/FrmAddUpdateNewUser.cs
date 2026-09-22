using DVLD.Business.DTOs;
using DVLD.Business.Services;
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

namespace DVLD.Presentation.Users
{
    public partial class FrmAddUpdateNewUser : Form
    {
        public delegate void UserReturnedRowHandler (DataRow row , Mode mode);
        public event UserReturnedRowHandler UserReturnedRow;
        public delegate void UserDTOReturnedHandler(UserDTO userDTO);
        public event UserDTOReturnedHandler UserDTOReturned;


        UserDTO _UserInfo = new UserDTO();
        UsersService _service ;
        public FrmAddUpdateNewUser(UsersService s)
        {
            InitializeComponent();
            _service = s;
        }

        public FrmAddUpdateNewUser(UserDTO userInfo, UsersService s)
        {
            InitializeComponent();
            _service = s;
            _UserInfo = userInfo;
            lblTop.Text = "Update Person";
            ctrlPCF.LoadStaticPerson(userInfo.person);

            lblUserID.Text = userInfo.ID.ToString();
            txtUserName.Text = userInfo.UserName;
            txtPassword.Text = userInfo.Password;
            txtPasswordConfirm.Text = userInfo.Password;
            cbActive.Checked = userInfo.isActive;
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPCF.Tag == null)
            {
                MessageBox.Show("You must get person First", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabs.SelectedTab = tbLoginInfo;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtPasswordConfirm.Text) 
            {
                MessageBox.Show("Password dosen't match", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _UserInfo.UserName = txtUserName.Text;
            _UserInfo.Password = txtPassword.Text;
            _UserInfo.isActive = cbActive.Checked;
                
            UpdateAdd user = new UpdateAdd(_UserInfo, _service);

            DataRow userRow;

            if (user.currentMode == Mode.AddNew)
            {
                userRow = user.Save();
                if(_UserInfo.ID != default)
                {
                    UserReturnedRow?.Invoke(userRow, Mode.AddNew);
                    lblTop.Text = "Update Person";
                    lblUserID.Text = _UserInfo.ID.ToString();
                    UserDTOReturned?.Invoke(_UserInfo);
                }
            }
            else if (user.currentMode == Mode.Updated)
            {
                userRow = user.Save();
                UserReturnedRow?.Invoke(userRow, Mode.Updated);
                UserDTOReturned?.Invoke(_UserInfo);
            }

        }

        private void txtPasswordConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                epPassConfirm.SetError(txtPasswordConfirm, "Passowrd dosn't match");
            }
            else
            {
                epPassConfirm.SetError(txtPasswordConfirm, "");
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedTab != tbLoginInfo)
                return;

            if (ctrlPCF.Tag == null)
            {
                pnlUser.Enabled = false;
                MessageBox.Show("You must get person First", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabs.SelectedTab = tbPersonInfo;
                return;
            }

            PersonDTO pesonInfo = (PersonDTO)ctrlPCF.Tag;
            if (pesonInfo.ID > 0)
            {
                if (ctrlPCF.IsLocked == false )
                {
                    if (MessageBox.Show($"Are you sure you want to make\n{pesonInfo.FullName} \nUser in system?", "Save", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        ctrlPCF.IsLocked = true;
                        _UserInfo.person = pesonInfo;
                        pnlUser.Enabled = true;
                        tabs.SelectedTab = tbLoginInfo;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        tabs.SelectedTab = tbPersonInfo;
                    }
                }
                else if (ctrlPCF.IsLocked == true)
                {
                    if(int.TryParse(lblUserID.Text, out int UserID) && UserID > 0)
                    {
                        pnlUser.Enabled = true;
                        btnSave.Enabled = true;
                    }

                }
            }
            else
            {
                MessageBox.Show("not valid person", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

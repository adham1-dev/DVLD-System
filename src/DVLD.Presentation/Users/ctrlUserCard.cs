using DVLD.Business.DTOs;
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
    public partial class ctrlUserCard : UserControl
    {
        public UsersService _service = null;
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        public void LoadUserData(int Id)
        {
            if (_service == null)
                _service = new UsersService();

            if (Id <= 0) return;
            UserDTO userDTO = (UserDTO)_service.GetDTOById(Id);
            PC.LoadPerson(userDTO.person);
            lblUserID.Text = userDTO.ID.ToString();
            lblUserName.Text = userDTO.UserName;
            lblIsActive.Text = userDTO.isActive == true ? "Yes" : "No";
        }
    }
}

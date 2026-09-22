using DVLD.Business.DTOs;
using DVLD.Business.Services;
using DVLD.Presentation.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.People
{
    public partial class ctrlPersonCardFilter : UserControl
    {
        
        string [] _oldSearchValue = new string[2];
        PeopleService _service = new PeopleService();
        enum Filter { PersonID, NationalNo }
        public ctrlPersonCardFilter()
        {
            InitializeComponent();
            cbFilter.DataSource = Enum.GetValues(typeof(Filter));
            PC.service = _service;
            PC.PersonDTOBack += UpdateTag;
        }

        public bool IsLocked { get { return !gbFilter.Enabled; } set { gbFilter.Enabled = !value; } }

        public void LoadStaticPerson (PersonDTO personDTO)
        {
            IsLocked = true;
            gbFilter.Enabled = false;
            PC.LoadPerson(personDTO);
            Tag = personDTO;
        }


        bool IsOldValue(string newValue, int index)
        {
            if(newValue == _oldSearchValue[index])
            {
                return true;
            }
            else
            {
                _oldSearchValue[index] = newValue;
                return false;
            }
        }
        private void btnGet_Click(object sender, EventArgs e)
        {
            if(IsOldValue(txtSearch.Text , 0) && IsOldValue(cbFilter.Text,1))
                return;

            Tag = null;

            UIHelper.Execute
            (
                action: () =>
                {
                    Tag = _service.GetDTOBy(cbFilter.Text, txtSearch.Text);
                    PC.LoadPerson((PersonDTO)Tag);
                }
            );
        }

        void UpdateTag(PersonDTO personDTO)
        {
            Tag = personDTO;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddOrUpdateNewPerson Add = new FrmAddOrUpdateNewPerson(_service);
            Add.PersonDTOReturned += PC.LoadPerson;
            Add.ShowDialog();
        }
    }
}

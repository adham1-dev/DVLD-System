using DVLD.Business.DTOs;
using DVLD.Business.Services;
using DVLD.Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Generic
{
    public partial class ctrlPersonCard : UserControl
    {
        public delegate void DataRowBackHandler(DataRow row, Mode mode);
        public event DataRowBackHandler DataRowBack;

        public delegate void PerosnDTOBackHandler(PersonDTO DTO);
        public event PerosnDTOBackHandler PersonDTOBack;

        private PeopleService _service;
        public PeopleService service { get { return _service; } set { _service = value; qlblEditPerson.Enabled = true; } }
        public PersonDTO _person;
        public DataRow personRow { get; private set; }

        public ctrlPersonCard()
        {
            InitializeComponent();
            personRow = null;
            _service = null;
        }


        void _UiFromDTO(PersonDTO personDTO)
        {
            if(personDTO == null) 
                return;

            if(personDTO.ID <= 0)
                qlblEditPerson.Enabled = false;



            lblID.Text = personDTO.ID.ToString();
            lblName.Text = personDTO.FullName;
            lblNationalNo.Text = personDTO.NationalNo;
            lblGender.Text = personDTO.Gender == "Male" ? "Male" : "Female";
            lblEmail.Text = personDTO.Email ?? "";
            lblAddress.Text = personDTO.Address;
            lblDateOfBirth.Text = personDTO.DateOfBirth.ToShortDateString();
            lblPhone.Text = personDTO.Phone;
            lblCountry.Text = personDTO.Country;

            //Wrong code: it's open All files you select and never close those images
            //if (personDTO.ImagePath != null && File.Exists(personDTO.ImagePath))
            //    pbPersonImage.Image = Image.FromFile(personDTO.ImagePath);

            if (File.Exists(personDTO.ImagePath))
            {
                //Remove old image (close)
                if (pbPersonImage.Image != null) pbPersonImage.Image.Dispose();

                //Add temp image from file (cached)
                using (Image tempImage = Image.FromFile(personDTO.ImagePath))
                {
                    pbPersonImage.Image = new Bitmap(tempImage);
                }
            }
            else
            {
                pbPersonImage.Image = (personDTO.Gender == "Male") ? Resources.man : Resources.woman;
            }
        


        }

        public void LoadPerson(PersonDTO person)
        {
            _person = person ?? default;
            if(person.ID > 0)
            {
                qlblEditPerson.Enabled = true;
                _UiFromDTO(_person);
            }
            else
            {
                qlblEditPerson.Enabled = false;
            }
        }

        public void LoadPersonByID(int ID)
        {
            if(_service == null)
                service = new PeopleService();

            _person = (PersonDTO)service.GetDTOById(ID);
            if(_person == null)
            {
                qlblEditPerson.Enabled = false;
                return;
            }

            if (_person.ID > 0 )
            {
                qlblEditPerson.Enabled = true;
                _UiFromDTO(_person);
            }
            else
            {
                qlblEditPerson.Enabled = false;
            }

        }

        public void LoadPersonByNoNum(string NoNum)
        {
            if(_service == null)
                service = new PeopleService();

            _person = (PersonDTO)service.GetDTOBy("NationalNo", NoNum);
            if(_person == null)
            {
                qlblEditPerson.Enabled = false;
                return;
            }

            if (_person.ID > 0 )
            {
                qlblEditPerson.Enabled = true;
                _UiFromDTO(_person);
            }
            else
            {
                qlblEditPerson.Enabled = false;
            }

        }


        void EditedDataRow (DataRow row , Mode mode)
        {
            DataRowBack?.Invoke(row, Mode.Updated);
            personRow = row;
        }


        void EditedDTO (PersonDTO DTO)
        {
            PersonDTOBack?.Invoke(DTO);
        }


        private void qlblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmAddOrUpdateNewPerson EditInfo = new FrmAddOrUpdateNewPerson(_person , service);
            EditInfo.PersonDTOReturned += LoadPerson;
            EditInfo.PersonReturnedRow += EditedDataRow;
            EditInfo.PersonDTOReturned += EditedDTO;
            EditInfo.ShowDialog();
        }
    }
}

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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD.Presentation.Generic
{
    public partial class FrmAddOrUpdateNewPerson : Form
    {
        //signature with return type
        public delegate void PersonDTOReturnedEventHandler(PersonDTO row);
        //list of func pointers
        public event PersonDTOReturnedEventHandler PersonDTOReturned;

        //signature with return type
        public delegate void PersonReturnedRowEventHandler(DataRow row , Mode mode);
        //list of func pointers
        public event PersonReturnedRowEventHandler PersonReturnedRow;


        PersonDTO _personInfo = new PersonDTO();
        //DataRow _newPerson = null;
        CountryService countryService = new CountryService();
        PeopleService _service;

        public FrmAddOrUpdateNewPerson(PeopleService s)
        {
            InitializeComponent();
            _service = s;
            cmbCountry.DataSource = CountryService.AllCountries;
            cmbCountry.DisplayMember = "CountryName";
            cmbCountry.ValueMember = "CountryID";
        }
        public FrmAddOrUpdateNewPerson(PersonDTO personDTO , PeopleService s)
        {
            InitializeComponent();
            _service = s;
            cmbCountry.DataSource = CountryService.AllCountries;
            cmbCountry.DisplayMember = "CountryName";
            cmbCountry.ValueMember = "CountryID";

            _personInfo.FillBy(personDTO ?? throw new Exception("Value Cant be null")) ;
            _FillFormWithData(_personInfo);

        }



        void _FillFormWithData(PersonDTO personInfo)
        {
            //Fill Form with data
            if (personInfo.ID != default)
            {
                lblTop.Text = "Update Person";
                lblID.Text = _personInfo.ID.ToString();
            }
            txtFirstName.Text = personInfo.FirstName;
            txtSecondName.Text = personInfo.SecondName;
            txtThirdName.Text = personInfo.ThirdName ?? "";
            txtLastName.Text = personInfo.LastName;
            txtNationalNo.Text = personInfo.NationalNo;
            if (personInfo.Gender == "Male")
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            txtEmail.Text = personInfo.Email ?? "";
            txtAddress.Text = personInfo.Address;
            dtpDateOfBirth.Value = personInfo.DateOfBirth;
            txtPhone.Text = personInfo.Phone;
            cmbCountry.Text = personInfo.Country;


            //if (File.Exists(personInfo.ImagePath))
            //{
            //    pbPersonImage.Image = Image.FromFile(_personInfo.ImagePath);
            //}

            if (File.Exists(personInfo.ImagePath))
            {
                //Remove old image (close)
                if (pbPersonImage.Image != null) pbPersonImage.Image.Dispose();

                //Add temp image from file (cached)
                using (Image tempImage = Image.FromFile(personInfo.ImagePath))
                {
                    pbPersonImage.Image = new Bitmap(tempImage);
                }
            }
            else
            {
                pbPersonImage.Image = (personInfo.Gender == "Male") ? Resources.man : Resources.woman;
                personInfo.ImagePath = null;
            }


        }


        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = _personInfo.ImagePath == null ? Resources.woman : pbPersonImage.Image;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = _personInfo.ImagePath == null ? Resources.man : pbPersonImage.Image;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            _personInfo = new PersonDTO
            {
                ID = _personInfo.ID,
                FirstName = txtFirstName.Text,
                SecondName = txtSecondName.Text,
                ThirdName = string.IsNullOrWhiteSpace(txtThirdName.Text) ? null : txtThirdName.Text,
                LastName = txtLastName.Text,
                NationalNo = txtNationalNo.Text,
                Gender = rbMale.Checked ? "Male" : "Female",
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text,
                Address = txtAddress.Text,
                DateOfBirth = dtpDateOfBirth.Value,
                Phone = txtPhone.Text,
                Country = cmbCountry.Text,
                ImagePath = _personInfo.ImagePath == default ? null : _personInfo.ImagePath,
            };
            UpdateAdd person = new UpdateAdd(_personInfo, _service);

            DataRow PersonRow;

            if (person.currentMode == Mode.AddNew)
            {
                PersonRow = person.Save();
                PersonReturnedRow?.Invoke(PersonRow, Mode.AddNew);
                lblTop.Text = "Update Person";
                lblID.Text = _personInfo.ID.ToString();
                PersonDTOReturned?.Invoke(_personInfo);
            }
            else if (person.currentMode == Mode.Updated)
            {
                PersonRow = person.Save();
                PersonReturnedRow?.Invoke(PersonRow, Mode.Updated);
                PersonDTOReturned?.Invoke(_personInfo);
            }
                
        }

        private void qlblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (fdSetImage.ShowDialog() == DialogResult.OK)
            {
                _personInfo.ImagePath = fdSetImage.FileName;

                //Close old image
                if (pbPersonImage.Image != null) pbPersonImage.Image.Dispose();

                //Open new image
                pbPersonImage.Image = Image.FromFile(_personInfo.ImagePath);
            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAddOrUpdateNewPerson_Load(object sender, EventArgs e)
        {

        }
    }
}

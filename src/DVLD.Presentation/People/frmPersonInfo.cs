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

namespace DVLD.Presentation.Generic
{
    public partial class frmPersonInfo : Form
    {
        public delegate void DataRowBackHandler(DataRow row, Mode mode);
        public event DataRowBackHandler PersonRowBack;

        public delegate void PerosnDTOBackHandler(PersonDTO DTO);
        public event PerosnDTOBackHandler PersonDTOBack;

        public frmPersonInfo(int id , PeopleService s)
        {
            InitializeComponent();
            crdPersonInfo.service = s;
            crdPersonInfo.LoadPersonByID(id);
            crdPersonInfo.DataRowBack += GetPersonRow;
            crdPersonInfo.PersonDTOBack += GetPersonDTO;
        }
        void GetPersonRow(DataRow PersonRow , Mode mode)
        {
            PersonRowBack?.Invoke(PersonRow , mode);
        }
        void GetPersonDTO(PersonDTO DTO)
        {
            PersonDTOBack?.Invoke(DTO);
        }
    }
}

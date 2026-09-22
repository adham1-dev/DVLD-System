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

namespace DVLD.Presentation.ManageStatics.UpdateStatics
{
    public partial class FrmUpdateTestTypes : Form
    {
        public delegate void ReturndRowHandler(DataRow row);
        public event ReturndRowHandler ReturndRow;


        TestTypesService _service;
        public FrmUpdateTestTypes(TestTypeDTO DTO , TestTypesService s)
        {
            InitializeComponent();
            _service = s;
            lblID.Text = DTO.ID.ToString();
            txtTitle.Text = DTO.TestTitle.ToString();
            txtDescription.Text = DTO.Description.ToString();
            txtFees.Text = DTO.TestFees.ToString();
        }


        TestTypeDTO UI2DTO()
        {
            return new TestTypeDTO
            {
                ID = int.Parse(lblID.Text),
                TestTitle = txtTitle.Text,
                Description = txtDescription.Text,
                TestFees = decimal.Parse(txtFees.Text),
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UIHelper.Execute
            (
                action: () =>
                {
                    DataRow updatedRow = _service.Update(UI2DTO());
                    if (updatedRow != null)
                    {
                        ReturndRow?.Invoke(updatedRow);
                        MessageBox.Show("Test Updated successfuly");
                    }
                }
            );
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

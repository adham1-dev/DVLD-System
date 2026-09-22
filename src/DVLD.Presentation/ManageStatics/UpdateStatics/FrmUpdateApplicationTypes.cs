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
    public partial class FrmUpdateApplicationTypes : Form
    {
        public delegate void ReturndRowHandler(DataRow row);
        public event ReturndRowHandler ReturndRow;


        ApplicationTypesService _service;

        public FrmUpdateApplicationTypes(ApplicationTypeDTO DTO , ApplicationTypesService s)
        {
            InitializeComponent();
            _service = s;
            lblID.Text = DTO.ID.ToString();
            txtTitle.Text = DTO.AppTitle;
            txtFees.Text = DTO.AppFees.ToString();
        }

        ApplicationTypeDTO UI2DTO()
        {
            return new ApplicationTypeDTO
            {
                ID = int.Parse(lblID.Text),
                AppTitle = txtTitle.Text,
                AppFees = decimal.Parse(txtFees.Text),
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

        private void FrmUpdateApplicationTypes_Load(object sender, EventArgs e)
        {

        }
    }
}

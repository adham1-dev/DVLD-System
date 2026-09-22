using DVLD.Business.Generic;
using DVLD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs.Applications.GeneralApp
{
    public class ApplicationDTO : BaseDTO
    {
        public UserDTO CreatedBy { get; set; }
        public PersonDTO PersonInfo { get; set; }
        public ApplicationTypeDTO AppType { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime Date { get; set; }

    }
}

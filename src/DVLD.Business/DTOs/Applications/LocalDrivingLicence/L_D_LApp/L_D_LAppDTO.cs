using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs.Applications.L_D_LApp
{
    public class L_D_LAppDTO : BaseDTO
    {
        public LicenseClassDTO LicenseClass { get; set; } = new LicenseClassDTO();
        public ApplicationDTO Application { get; set; } = new ApplicationDTO();
        public byte PassedTests { get; set; }

    }
}

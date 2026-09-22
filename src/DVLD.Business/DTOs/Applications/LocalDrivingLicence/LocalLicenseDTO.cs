using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs.Applications.LocalDrivingLicence
{
    public class LocalLicenseDTO : BaseDTO
    {
        public L_D_LAppDTO L_D_LApp { get; set; }

        public DateTime ReleaseDate { get; set; }
        public string IssueReason { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; }
        public int DriverID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsDetained { get; set; }

    }
}

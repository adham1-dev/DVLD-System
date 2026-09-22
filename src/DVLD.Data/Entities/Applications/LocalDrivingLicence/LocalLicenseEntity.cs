using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities.Applications.LocalDrivingLicence
{
    public class LocalLicenseEntity
    {
        public int LocalLicenseID { get; set; }
        public L_D_LAppEntity L_D_LApp { get; set; }

        public DateTime ReleaseDate { get; set; }
        public string IssueReason { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; }
        public int DriverID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsDetained { get; set; }
    }
}

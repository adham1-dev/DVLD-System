using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities.Applications
{
    public class DetainedLicenseEntity
    {
        public int DetainedLicenseID { get; set; }
        public int LocalLicenseID { get; set; }
        public int? ReleaseAppID { get; set; }
        public int ReleasedBy { get; set; }
        public int PersonID { get; set; }

        public int DetaindByUserID { get; set; }
        public string DetainNote { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public bool IsReleased { get; set; }
    }

}

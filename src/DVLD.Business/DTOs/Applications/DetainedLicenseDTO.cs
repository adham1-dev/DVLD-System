using DVLD.Business.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs.Applications
{
    public class DetainedLicenseDTO : BaseDTO
    {
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

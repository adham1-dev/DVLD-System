using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs.Applications.TestApp
{
    public class TestAppointmentDTO : BaseDTO
    {
        public ApplicationDTO RetakeApp { get; set; }
        public TestTypeDTO TestType { get; set; } = new TestTypeDTO();
        public L_D_LAppDTO L_D_LApp { get; set; } = new L_D_LAppDTO();
        public bool? TestResult { get; set; }
        public DateTime TestDate { get; set; }
        public decimal PaidFees => (RetakeApp?.AppType?.AppFees ?? 0) + (TestType?.TestFees ?? 0);

    }
}

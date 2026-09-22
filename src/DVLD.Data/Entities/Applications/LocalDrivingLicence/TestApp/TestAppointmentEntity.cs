using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities
{
    public class TestAppointmentEntity
    {
        public int TestAppointmentID { get; set; }
        public int? RetakeAppID { get; set; } 
        public int L_D_LAppID { get; set; }
        public int BaseAppID { get; set; }
        public int TestTypeID { get; set; } 
        public bool? TestResult { get; set; } = null;
        public DateTime TestDate { get; set; }
        public int PersonID { get; set; }
        public int CreatedByID { get; set; }
        public decimal PaidFees { get; set; }
    }
}


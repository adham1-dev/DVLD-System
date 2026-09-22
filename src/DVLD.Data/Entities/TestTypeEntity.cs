using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities
{
    public class TestTypeEntity
    {
        public int TestTypeID { get; set; }
        public string TestTitle { get; set; }
        public string Description { get; set; }
        public decimal TestFees { get; set; }
    }
}

using DVLD.Business.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs
{
    public class TestTypeDTO : BaseDTO
    {
        public string TestTitle { get; set; }
        public string Description { get; set; }
        public decimal TestFees { get; set; }

    }
}

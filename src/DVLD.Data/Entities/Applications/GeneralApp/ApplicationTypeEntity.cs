using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities
{
    public class ApplicationTypeEntity
    {
        public int ApplicationTypeID { get; set; }
        public string AppTitle { get; set; }
        public decimal AppFees { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities
{
    public class L_D_LAppEntity
    {
        public int L_D_LAppID { get; set; }
        public LicenseClassEntity LicenseClass { get; set; }
        public ApplicationEntity Application { get; set; }
        public byte PassedTests { get; set; }

    }
}

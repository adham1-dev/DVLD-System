using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Entities
{
    public class ApplicationEntity
    {
        public int ApplicationID { get; set; }
        public UserEntity CreatedBy { get; set; }
        public PersonEntity PersonInfo { get; set; }
        public ApplicationTypeEntity AppType { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime Date { get; set; }
    }
}

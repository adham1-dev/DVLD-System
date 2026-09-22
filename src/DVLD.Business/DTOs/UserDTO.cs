using DVLD.Business.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs
{
    public class UserDTO : BaseDTO
    {
        public PersonDTO person {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }

    }
}

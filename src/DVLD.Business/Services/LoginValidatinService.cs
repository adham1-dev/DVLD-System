using DVLD.Business.Generic;
using DVLD.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services
{
    public class LoginValidatinService
    {
        LoginValidationRepository _repo;

        public void ValidateLoginInfo (in string username, in string password, out int ID)
        {
            if (string.IsNullOrEmpty(username)) 
            { 
                throw new Exception("Username is required"); 
            }
            if(string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is required"); 
            }

            string hashedPass = ServiceHelper.ComputeHash(password);

            _repo = new LoginValidationRepository(in username, in hashedPass, out ID);

        }
    }
}

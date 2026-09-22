using DVLD.Business.Generic;
using DVLD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs
{
    public class PersonDTO : BaseDTO
    {
        public string FullName => string.Join(" ", 
            new[] { FirstName, SecondName, ThirdName, LastName }.
            Where(namePart => !string.IsNullOrWhiteSpace(namePart)));
                        
        public string   FirstName { get; set; }
        public string   SecondName { get; set; }
        public string   ThirdName { get; set; }
        public string   LastName { get; set; }
                        
        public string   NationalNo { get; set; }
        public string   Gender { get; set; }
        public string   Email { get; set; }
        public string   Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int age => DateTime.Now.Year - DateOfBirth.Year;
        public string Phone { get; set; }
        public string Country { get; set; }

        public string ImagePath { get; set; }

        public void FillBy(PersonDTO personDTO)
        {
            this.ID = personDTO.ID;
            this.FirstName = personDTO.FirstName;
            this.SecondName = personDTO.SecondName;
            this.ThirdName = personDTO.ThirdName;
            this.LastName = personDTO.LastName;
            this.NationalNo = personDTO.NationalNo;
            this.Gender = personDTO.Gender;
            this.Email = personDTO.Email;
            this.Address = personDTO.Address;
            this.DateOfBirth = personDTO.DateOfBirth;
            this.Phone = personDTO.Phone;
            this.Country = personDTO.Country;
            this.ImagePath = personDTO.ImagePath;
        }


    }
}

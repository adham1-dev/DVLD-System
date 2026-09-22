using DVLD.Business.DTOs;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using DVLD.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services
{
    public class PeopleService : BaseService,IUpdateAddService 
    {

        private PersonRepository _people;
        private CountryService _countryService = new CountryService();


        public PeopleService() : base(new PersonRepository())
        {
            //convert base repo to spicial repo
            _people = (PersonRepository)_baseRepo;
        }


        // you can copy this and modify for any class as it about poring mapping
        public override BaseDTO GetDTOById(int id)
        {
            PersonEntity person = _people.GetEntity(id);

            if (person == null)
                return null;

            return PersonEntity2DTO(person);
        }

        static public PersonDTO PersonEntity2DTO (PersonEntity person)
        {
            return new PersonDTO
            {
                ID = person.PersonID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                Gender = person.Gender == 0 ? "Male" : "FeMale",
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                Phone = person.Phone,
                Country = person.NativeCountry?.CountryName,
                ImagePath = person.ImagePath,

            };

        }

        public PersonDTO GetDTOBy(string ColName ,string value)
        {
            PersonEntity person = null;
            if(ColName == "PersonID")
            {
                if (int.TryParse(value, out int num))
                    person = _people.GetEntity(num);
                else
                    throw new Exception("ID must be numbers only");
            }
            else if(ColName == "NationalNo")
               person = _people.GetEntityByColName(ColName, value);

            if (person == null)
                throw new Exception ("This person not founded");

            return new PersonDTO
            {
                ID = person.PersonID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                Gender = person.Gender == 0 ? "Male" : "FeMale",
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                Phone = person.Phone,
                Country = person.NativeCountry.CountryName,
                ImagePath = person.ImagePath,

            };
        }



        //interface later for DI
        PersonEntity DTO2Entinty(PersonDTO person)
        {
            return new PersonEntity
            {
                PersonID = person.ID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                Gender = person.Gender == "Male" ? (byte)0 : (byte)1,
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                Phone = person.Phone,
                NativeCountry = _countryService.GetCountryEntityByName(person.Country),
                ImagePath = ServiceHelper.SaveImageToProjectFolder(person.ImagePath),
            };
        }

        public DataRow AddNew(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'Add New' function shouldn't include an ID field. \nit should be auto-generated.");

            PersonDTO person = (PersonDTO)DTO;

            return _people.Add(DTO2Entinty(person));
        }

        public DataRow Update(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");
            PersonDTO person = (PersonDTO)DTO;

            PersonEntity entity = DTO2Entinty(person);
            if (entity.PersonID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            return _people.Update(entity);
        }



    }
}

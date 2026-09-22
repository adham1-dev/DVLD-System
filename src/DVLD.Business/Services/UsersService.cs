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
    public class UsersService: BaseService, IUpdateAddService
    {

        UserRepository _repo;
        PeopleService _personService = new PeopleService();
        public UsersService() :base(new UserRepository())
        {
            _repo = (UserRepository)_baseRepo;
        }

        // you can copy this and modify for any class as it about poring mapping
        public override BaseDTO GetDTOById(int id)
        {
            UserEntity user = _repo.GetEntity(id);

            if (user == null)
                return null;

            return new UserDTO
            {
                ID = user.UserID,
                person = new PersonDTO
                {
                    ID = user.Person.PersonID,
                    FirstName = user.Person.FirstName,
                    SecondName = user.Person.SecondName,
                    ThirdName = user.Person.ThirdName,
                    LastName = user.Person.LastName,
                    NationalNo = user.Person.NationalNo,
                    Gender = user.Person.Gender == 0 ? "Male" : "FeMale",
                    Email = user.Person.Email,
                    Address = user.Person.Address,
                    DateOfBirth = user.Person.DateOfBirth,
                    Phone = user.Person.Phone,
                    Country = user.Person.NativeCountry.CountryName,
                    ImagePath = user.Person.ImagePath,
                },
                UserName = user.UserName,
                Password = user.Password,
                isActive = user.IsActive,
            };
        }

        //interface later for DI
        UserEntity DTO2Entinty(UserDTO user)
        {
            return new UserEntity
            {
                UserID = user.ID,
                Person = new PersonEntity
                {
                    PersonID = user.person.ID
                },
                UserName =user.UserName,
                Password = user.Password,
                IsActive = user.isActive,
               
            };
        }
        public DataRow AddNew(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'Add New' function shouldn't include an ID field. \nit should be auto-generated.");

            UserDTO person = (UserDTO)DTO;

            person.Password = ServiceHelper.ComputeHash(person.Password);

            return _repo.AddRecord(DTO2Entinty(person));
        }

        public DataRow Update(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");
            UserDTO person = (UserDTO)DTO;

            UserEntity entity = DTO2Entinty(person);
            if (entity.UserID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            return _repo.UpdateRecord(entity);
        }



    }
}

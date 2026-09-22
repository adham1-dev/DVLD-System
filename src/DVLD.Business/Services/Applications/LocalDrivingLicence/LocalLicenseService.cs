using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using DVLD.Data.Entities.Applications.LocalDrivingLicence;
using DVLD.Data.Repositories;
using DVLD.Data.Repositories.Applications.LocalDrivingLicense;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD.Data.Repositories.Applications.LocalDrivingLicense.LocalLicenseRepository;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Business.Services.Applications.LocalDrivingLicence
{
    public enum ReplacmentReason { None , Damaged , Lost}

    public class LocalLicenseService : BaseService
    {


        private LocalLicenseRepository _repo;




        public LocalLicenseService() : base(new LocalLicenseRepository())
        {
            //convert base repo to spicial repo
            _repo = (LocalLicenseRepository)_baseRepo;
        }

        public DataTable GetViewByPersonID(int PersonID)
        {
            return _baseRepo.GetAllRecords("PersonID",PersonID);
        }

        // you can copy this and modify for any class as it about poring mapping
        public override BaseDTO GetDTOById(int id)
        {
            LocalLicenseEntity entity = _repo.GetEntity(id);

            if (entity == null)
                return null;

            return Entity2DTO(entity);
        }

        public LocalLicenseDTO GetDTOBy_LDLApp_Id(int id)
        {
            LocalLicenseEntity entity = _repo.GetEntityBy_LDLApp_ID(id);

            if (entity == null)
                return null;

            return Entity2DTO(entity);
        }

        public LocalLicenseDTO Entity2DTO(LocalLicenseEntity entity)
        {
            return new LocalLicenseDTO
            {

                ID = entity.LocalLicenseID,
                L_D_LApp = new L_D_LAppDTO
                {
                    ID = entity.L_D_LApp.L_D_LAppID,
                    LicenseClass = new LicenseClassDTO
                    {
                        ID = entity.L_D_LApp.LicenseClass.LicenseClassID,
                        ClassName = entity.L_D_LApp.LicenseClass.ClassName,
                    },
                    Application = new ApplicationDTO
                    {
                        PersonInfo = PeopleService.PersonEntity2DTO(entity.L_D_LApp.Application.PersonInfo),
                    },
                },

                ReleaseDate = entity.ReleaseDate,
                IssueReason = entity.IssueReason,
                Notes = entity.Notes,
                IsActive = entity.IsActive,
                DriverID = entity.DriverID,
                ExpirationDate = entity.ExpirationDate,
                IsDetained = entity.IsDetained,

            };

        }


        //interface later for DI
        LocalLicenseEntity DTO2Entinty(LocalLicenseDTO DTO)
        {
            return new LocalLicenseEntity
            {
                LocalLicenseID = DTO.ID,
                L_D_LApp = new L_D_LAppEntity
                {
                    L_D_LAppID = DTO.L_D_LApp.ID,
                    LicenseClass =new LicenseClassEntity
                    {
                        LicenseClassID = DTO.L_D_LApp.LicenseClass.ID,
                        ValidityLength = DTO.L_D_LApp.LicenseClass.ValidityLength,
                    },
                    Application = new ApplicationEntity
                    {
                        ApplicationID = DTO.L_D_LApp.Application.ID,
                        PersonInfo = new PersonEntity
                        {
                            PersonID = DTO.L_D_LApp.Application.PersonInfo.ID,
                        },
                        CreatedBy = new UserEntity
                        {
                            UserID =DTO.L_D_LApp.Application.CreatedBy.ID,
                        }
                        
                    },
                },
                Notes = DTO.Notes,
            };
        }

        public int AddNew(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'Add New' function shouldn't include an ID field. \nit should be auto-generated.");

            LocalLicenseDTO localLicense = (LocalLicenseDTO)DTO;

            return _repo.AddFirestTimeLic(DTO2Entinty(localLicense) );
        }

        public void AddReplacment(ReplacmentReason reason, in int PersonID, in int CreatedByID, in int LicClassID, in int LocalLicID
            , out int ReplacedLicID, out int RAppID, out int RLDL_AppID, out DateTime Date)
        {
            switch (reason)
            {
                case ReplacmentReason.Damaged:
                    _repo.AddReplacementLic(LocalLicenseRepository.Reason.Damaged, in PersonID, in CreatedByID, in LicClassID, in LocalLicID
                    , out ReplacedLicID, out RAppID, out RLDL_AppID, out Date);
                    return;

                case ReplacmentReason.Lost:
                    _repo.AddReplacementLic(LocalLicenseRepository.Reason.Lost, in PersonID, in CreatedByID, in LicClassID, in LocalLicID
                    , out ReplacedLicID, out RAppID, out RLDL_AppID, out Date);
                    return;

            }
            ReplacedLicID = default;
            RAppID = default;
            RLDL_AppID = default;
            Date = default;
        }


        /*
        public DataRow Update(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");
            PersonDTO person = (PersonDTO)DTO;

            PersonEntity entity = DTO2Entinty(person);
            if (entity.PersonID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            return _repo.UpdateRecord(entity);
        }
        */


    }
}

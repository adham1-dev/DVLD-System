using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications;
using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.LocalDrivingLicence;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using DVLD.Data.Entities.Applications;
using DVLD.Data.Entities.Applications.LocalDrivingLicence;
using DVLD.Data.Repositories.Applications;
using DVLD.Data.Repositories.Applications.LocalDrivingLicense;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services.Applications
{
    public class DetainedLicenseService : BaseService
    {

        private DetainedLicenseRepository _repo;




        public DetainedLicenseService() : base(new DetainedLicenseRepository())
        {
            //convert base repo to spicial repo
            _repo = (DetainedLicenseRepository)_baseRepo;
        }




        // you can copy this and modify for any class as it about poring mapping
        public override BaseDTO GetDTOById(int id)
        {
            DetainedLicenseEntity entity = _repo.GetEntity(id);

            if (entity == null)
                throw new Exception("This person is not Detained");

            return Entity2DTO(entity);
        }



        public DetainedLicenseDTO Entity2DTO(DetainedLicenseEntity entity)
        {
            return new DetainedLicenseDTO
            {

                ID = entity.DetainedLicenseID,
                LocalLicenseID    = entity.LocalLicenseID,
                ReleaseAppID = entity.ReleaseAppID,
                ReleasedBy = entity.ReleasedBy,
                PersonID = entity.PersonID,
                DetaindByUserID = entity.DetaindByUserID,
                DetainNote = entity.DetainNote,
                ReleaseDate = entity.ReleaseDate,
                DetainDate = entity.DetainDate,
                FineFees = entity.FineFees,
                IsReleased = entity.IsReleased,
            };

        }

        //interface later for DI
        DetainedLicenseEntity DTO2Entinty(DetainedLicenseDTO DTO)
        {
            return new DetainedLicenseEntity
            {
                FineFees = DTO.FineFees,
                LocalLicenseID = DTO.LocalLicenseID,
                DetaindByUserID = DTO.DetaindByUserID,
                DetainedLicenseID = DTO.ID,
                PersonID = DTO.PersonID,
                ReleasedBy = DTO.ReleasedBy,

            };
        }

        public DataRow DetaineLic(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'DetaineLic' function shouldn't include an ID field. \nit should be auto-generated.");

            DetainedLicenseDTO person = (DetainedLicenseDTO)DTO;

            return _repo.DetaineLicense(DTO2Entinty(person));
        }

        public DataRow Release(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");

            DetainedLicenseDTO detained = (DetainedLicenseDTO)DTO;
            DetainedLicenseEntity entity = DTO2Entinty(detained);

            if (entity.DetainedLicenseID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            return _repo.ReleaseLicense(entity);
        }












    }

}

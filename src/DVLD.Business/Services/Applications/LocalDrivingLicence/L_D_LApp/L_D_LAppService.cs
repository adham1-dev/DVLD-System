using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.Generic;
using DVLD.Data.Entities;
using DVLD.Data.Repositories;
using DVLD.Data.Repositories.Applications.L_D_LApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services.Applications.L_D_LApp
{
    public class L_D_LAppService : BaseService, IUpdateAddService
    {
        private L_D_LAppRepository _repo;
        private LicenseClassesService _LCservice = new LicenseClassesService();
        private ApplicationTypesService _ATservice = new ApplicationTypesService();


        public L_D_LAppService() : base(new L_D_LAppRepository())
        {
            //convert base repo to spicial repo
            _repo = (L_D_LAppRepository)_baseRepo;
        }





        public override BaseDTO GetDTOById(int id)
        {
            L_D_LAppEntity entity = _repo.GetEntity(id);

            if (entity == null)
                return null;

            return new L_D_LAppDTO
            {
                ID = id,
                LicenseClass = LicenseClassesService.LicenseClassDTOById(entity.LicenseClass.LicenseClassID),
                Application = new ApplicationDTO
                {
                    ID = entity.Application.ApplicationID,
                    PersonInfo = PeopleService.PersonEntity2DTO(entity.Application.PersonInfo),
                    CreatedBy = new UserDTO
                    {
                        ID = entity.Application.CreatedBy.UserID,
                        UserName = entity.Application.CreatedBy.UserName,
                    },
                    ApplicationStatus = entity.Application.ApplicationStatus,
                    Date = entity.Application.Date,
                    AppType = new ApplicationTypeDTO(entity.Application.AppType.ApplicationTypeID),
                },
                PassedTests = entity.PassedTests,
            };
        }



        L_D_LAppEntity DTO2Entinty(L_D_LAppDTO DTO)
        {
            return new L_D_LAppEntity
            {
                L_D_LAppID = DTO.ID,
                Application = new ApplicationEntity
                {
                    CreatedBy = new UserEntity
                    {
                        UserID = DTO.Application.CreatedBy.ID,
                    },
                    PersonInfo = new PersonEntity
                    {
                        PersonID = DTO.Application.PersonInfo.ID,
                    },
                },

                LicenseClass = _LCservice.LicenseClassEntityByName(DTO.LicenseClass.ClassName),

            };
        }



        public DataRow AddNew(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'Add New' function shouldn't include an ID field. \nit should be auto-generated.");

            L_D_LAppDTO L_D_LApp = (L_D_LAppDTO)DTO;

            int personAge = L_D_LApp.Application.PersonInfo.age;
            int classMinAllowedAge = LicenseClassesService.GetMinAllowedAge(L_D_LApp.LicenseClass.ClassName);

            if (personAge >= classMinAllowedAge)
                return _repo.AddRecord(DTO2Entinty(L_D_LApp));
            else
                throw new Exception($"person Age is not saticfied for this Licence class\nperson age: {personAge}\nClass minmum allowed age: {classMinAllowedAge}");

        }

        public DataRow Update(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");
            L_D_LAppDTO L_D_LApp = (L_D_LAppDTO)DTO;

            L_D_LAppEntity entity = DTO2Entinty(L_D_LApp);
            if (entity.L_D_LAppID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            int personAge = L_D_LApp.Application.PersonInfo.age;
            int classMinAllowedAge = LicenseClassesService.GetMinAllowedAge(L_D_LApp.LicenseClass.ClassName);

            if (personAge >= classMinAllowedAge)
                return _repo.UpdateRecord(entity);
            else
                throw new Exception($"person Age is not saticfied for this Licence class\nperson age: {personAge}\nClass minmum allowed age: {classMinAllowedAge}");



            
        }

        public DataRow CancelApplecation(int ID)
        {
            if (ID <= 0)
                throw new Exception("There is no valid id to 'Cancel'");

            return _repo.CancelApplecation(ID);
        }



    }
}

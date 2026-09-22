using DVLD.Business.DTOs;
using DVLD.Business.DTOs.Applications.GeneralApp;
using DVLD.Business.DTOs.Applications.L_D_LApp;
using DVLD.Business.DTOs.Applications.TestApp;
using DVLD.Business.Generic;
using DVLD.Business.Services.Applications.L_D_LApp;
using DVLD.Data.Entities;
using DVLD.Data.Repositories.Applications.TestApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services.Applications.TestApp
{
    public class TestAppointmentService : BaseService, IUpdateAddService
    {


        private TestAppointmentRepository _repo;
        L_D_LAppService _LD_Service;
        TestTypesService TestTypes_service = new TestTypesService();

        public TestAppointmentService(L_D_LAppService s) : base(new TestAppointmentRepository())
        {
            //convert base repo to spicial repo
            _repo = (TestAppointmentRepository)_baseRepo;
            _LD_Service = s;
        }


        public DataTable GetAppointmentView(int LD_AppID , int TestTypeID)
        {
            try
            {
                return _repo.GetAllRecords( LD_AppID , TestTypeID);
            }
            catch(Exception ex) { throw new Exception("Cant get Appointment view:\n" + ex.Message); }
        }





        // you can copy this and modify for any class as it about poring mapping
        public override BaseDTO GetDTOById(int id)
        {
            TestAppointmentEntity entity = _repo.GetEntity(id);

            if (entity == null)
                return null;

            return Entity2DTO(entity);
        }

        TestAppointmentDTO Entity2DTO(TestAppointmentEntity entity)
        {
            return new TestAppointmentDTO
            {
                ID = entity.TestAppointmentID,
                L_D_LApp = (L_D_LAppDTO)_LD_Service.GetDTOById(entity.L_D_LAppID),
                RetakeApp = new ApplicationDTO
                {
                    ID = entity.RetakeAppID ?? default,
                    AppType = new ApplicationTypeDTO(7),
                },
                TestDate = entity.TestDate,
                TestResult = entity.TestResult,
                TestType = TestTypesService.DTOById(entity.TestTypeID),
            };
        }



        public void Validate(TestAppointmentDTO DTO)
        {
            TestAppointmentEntity entity = new TestAppointmentEntity
            {
                L_D_LAppID = DTO.L_D_LApp.ID,
                TestTypeID =DTO.TestType.ID,
                CreatedByID =DTO.L_D_LApp.Application.CreatedBy.ID,
                PersonID =DTO.L_D_LApp.Application.PersonInfo.ID,
                BaseAppID = DTO.L_D_LApp.Application.ID,
            };
            int? RetakeID = _repo.GetRetakeIDAndVaildation(entity);
            if (RetakeID == null)
                DTO.RetakeApp = null;
            else
                DTO.RetakeApp = new ApplicationDTO
                {
                    ID = RetakeID.Value,
                    AppType = new ApplicationTypeDTO(7),
                };
        }


        //interface later for DI
        TestAppointmentEntity DTO2Entinty(TestAppointmentDTO DTO)
        {
            return new TestAppointmentEntity
            {
                TestAppointmentID = DTO.ID,
                TestTypeID = DTO.TestType.ID,
                TestDate = DTO.TestDate,
                L_D_LAppID = DTO.L_D_LApp.ID,
                RetakeAppID = DTO.RetakeApp?.ID ,
                TestResult = DTO.TestResult,
                BaseAppID = DTO.L_D_LApp.Application.ID,
                CreatedByID = DTO.L_D_LApp.Application.CreatedBy.ID,
                PersonID = DTO.L_D_LApp.Application.PersonInfo.ID,
                PaidFees = DTO.PaidFees,
            };
        }

        public DataRow AddNew(BaseDTO DTO)
        {

            if (DTO.ID > 0)
                throw new Exception("The 'Add New' function shouldn't include an ID field. \nit should be auto-generated.");

            TestAppointmentDTO TestApp = (TestAppointmentDTO)DTO;

            return _repo.AddRecord(DTO2Entinty(TestApp));
        }

        public DataRow Update(BaseDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");
            TestAppointmentDTO person = (TestAppointmentDTO)DTO;

            TestAppointmentEntity entity = DTO2Entinty(person);
            if (entity.TestAppointmentID <= 0)
                throw new Exception("Error while updating (mapping is not correct)");

            return _repo.UpdateRecord(entity);
        }

    }
}

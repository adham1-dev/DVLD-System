using DVLD.Business.DTOs;
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
    public class TestTypesService
    {
        private TestTypeRepository _repo = new TestTypeRepository();
        static public DataTable TestTypesTypes { private set; get; }



        public TestTypesService()
        {
            TestTypesTypes = TestTypesTypes ?? _repo.GetAllRecords();

        }


        static public TestTypeDTO DTOById(int TestTypeID)
        {

            DataRow[] rows = TestTypesTypes.Select($"TestTypeID = '{TestTypeID}' ");

            if (rows.Length > 0)
            {
                return new TestTypeDTO
                {
                    ID = TestTypeID,
                    TestTitle = rows[0]["TestTitle"].ToString(),
                    Description = rows[0]["Description"].ToString(),
                    TestFees = (decimal)rows[0]["TestFees"],
                };
            }
            else
                throw new Exception("This app type Name dose not exist");
        }



        public DataTable GetView()
        {
            return TestTypesTypes;
        }

        public DataRow Update(TestTypeDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");

            TestTypeEntity entity =
                new TestTypeEntity
                {
                    TestTypeID = DTO.ID,
                    Description = DTO.Description,
                    TestFees = DTO.TestFees,
                    TestTitle = DTO.TestTitle,
                };

            return _repo.UpdateRecord(entity);
        }

    }
}

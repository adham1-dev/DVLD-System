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
    public class ApplicationTypesService
    {
        private ApplicationTypeRepository _repo = new ApplicationTypeRepository();
        static public DataTable AllAppTypes { private set; get; }



        public ApplicationTypesService()
        {
            AllAppTypes = AllAppTypes ?? _repo.GetAllRecords(); 

        }



        public ApplicationTypeEntity ApplicationTypeEntityByName(string typeName)
        {
            DataRow[] rows = AllAppTypes.Select($"AppTitle = '{typeName}' ");

            if (rows.Length > 0)
            {
                return new ApplicationTypeEntity
                {
                    ApplicationTypeID = (int)rows[0]["ApplicationTypeID"],
                    AppFees = (decimal)rows[0]["AppFees"],
                    AppTitle = typeName,
                };
            }
            else
                throw new Exception("This app type Name dose not exist");
        }

        static public ApplicationTypeDTO DTOById(int ApplicationTypeID)
        {
            DataRow[] rows = AllAppTypes.Select($"ApplicationTypeID = '{ApplicationTypeID}' ");

            if (rows.Length > 0)
            {
                return new ApplicationTypeDTO
                {
                    ID = ApplicationTypeID,
                    AppFees = (decimal)rows[0]["AppFees"],
                    AppTitle = rows[0]["AppTitle"].ToString(),
                };
            }
            else
                throw new Exception("This app type Name dose not exist");
        }





        public DataTable GetView()
        {
            return _repo.GetAllRecords();
        }


        public DataRow Update(ApplicationTypeDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");

            ApplicationTypeEntity entity =
                new ApplicationTypeEntity
                {
                    ApplicationTypeID = DTO.ID,
                    AppFees = DTO.AppFees,
                    AppTitle = DTO.AppTitle,
                };

            return _repo.UpdateRecord(entity);
        }


    }
}

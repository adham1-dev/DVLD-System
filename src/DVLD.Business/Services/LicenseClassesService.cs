using DVLD.Business.DTOs;
using DVLD.Data.Entities;
using DVLD.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Services
{
    public class LicenseClassesService
    {
        private LicenseClassRepository _repo = new LicenseClassRepository();
        static public DataTable AllLicenseClass { private set; get; }

        public LicenseClassesService()
        {
            AllLicenseClass = AllLicenseClass ?? _repo.GetAllRecords();

        }

        public LicenseClassEntity LicenseClassEntityByName(string className)
        {
            DataRow[] rows = AllLicenseClass.Select($"ClassName = '{className}' ");

            if (rows.Length > 0)
            {
                return new LicenseClassEntity
                {
                    LicenseClassID = (int)rows[0]["LicenseClassID"],
                    ClassName = className,
                    ClassDescription = rows[0]["ClassDescription"].ToString(),
                    MinimumAllowedAge = (byte)rows[0]["MinimumAllowedAge"],
                    ValidityLength = (byte)rows[0]["ValidityLength"],
                    ClassFees = (decimal)rows[0]["ClassFees"],
                };
            }
            else
                throw new Exception("This License Class Name dose not exist");
        }

        static public LicenseClassDTO LicenseClassDTOById(int LicenseClassID)
        {
            DataRow[] rows = AllLicenseClass.Select($"LicenseClassID = '{LicenseClassID}' ");

            if (rows.Length > 0)
            {
                return new LicenseClassDTO
                {
                    ID = LicenseClassID,
                    ClassName = rows[0]["ClassName"].ToString(),
                    ClassDescription = rows[0]["ClassDescription"].ToString(),
                    MinimumAllowedAge = (byte)rows[0]["MinimumAllowedAge"],
                    ValidityLength = (byte)rows[0]["ValidityLength"],
                    ClassFees = (decimal)rows[0]["ClassFees"],
                };
            }
            else
                throw new Exception("This License Class ID dose not exist");
        }

        static public byte GetMinAllowedAge(string className)
        {
            DataRow[] rows = AllLicenseClass.Select($"ClassName = '{className}' ");

            if (rows.Length > 0)
            {
                return (byte)rows[0]["MinimumAllowedAge"];
            }
            else
                throw new Exception("This License Class Name dose not exist");

        }

        public DataTable GetView()
        {
            return _repo.GetAllRecords();
        }

        public DataRow Update(LicenseClassDTO DTO)
        {
            if (DTO.ID <= 0)
                throw new Exception("The 'Update' Function have to include ID field to work");

            LicenseClassEntity entity =
                new LicenseClassEntity
                {
                    LicenseClassID = DTO.ID,
                    ClassName = DTO.ClassName,
                    ClassDescription = DTO.ClassDescription,
                    MinimumAllowedAge = DTO.MinimumAllowedAge,
                    ValidityLength = DTO.ValidityLength,
                    ClassFees = DTO.ClassFees,
                };

            return _repo.UpdateRecord(entity);
        }

    }
}

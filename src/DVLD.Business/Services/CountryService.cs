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
    public class CountryService
    {
        private CountryRepository _repo;
        static public DataTable AllCountries { private set; get; }
        public CountryService() 
        {
            _repo = new CountryRepository();
            AllCountries = _repo.GetAllRecords(); 
        }

        public string GetCountryNameById(int countryId)
        {
            DataRow[] rows = AllCountries.Select("id = " + countryId);

            if (rows.Length > 0)
                return rows[0]["CountryName"].ToString();
            else
                throw new Exception("Tihs Country ID dose not exist");
        }

        public CountryEntity GetCountryEntityById(int countryId)
        {
            DataRow[] rows = AllCountries.Select("CountryID = " + countryId);

            if (rows.Length > 0)
            {
                return new CountryEntity { 
                    CountryID = countryId,
                    CountryName = rows[0]["CountryName"].ToString(),
                };
            }
            else
                throw new Exception("Tihs Country ID dose not exist");
        }
        public CountryEntity GetCountryEntityByName(string countryName)
        {
            DataRow[] rows = AllCountries.Select($"CountryName = '{countryName}' " );

            if (rows.Length > 0)
            {
                return new CountryEntity { 
                    CountryID = (int)rows[0]["CountryID"],
                    CountryName = countryName,
                };
            }
            else
                throw new Exception("This Country Name dose not exist");
        }
    }
}

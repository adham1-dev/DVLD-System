using DVLD.Data.Entities;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Repositories
{
    public class CountryRepository : BaseRepository<CountryEntity>
    {
        public CountryRepository() : base("Countries", "CountryID") { }

        public override CountryEntity MapToEntity(SqlDataReader rdr)
        {
            return new CountryEntity
            {
                CountryID = (int)rdr["CountryID"],
                CountryName = rdr["CountryName"].ToString(),
            };
        }

    }
}

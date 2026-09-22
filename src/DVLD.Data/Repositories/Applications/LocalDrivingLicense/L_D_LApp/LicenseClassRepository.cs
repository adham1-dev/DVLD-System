using DVLD.Data.Entities;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Repositories
{
    public class LicenseClassRepository : BaseRepository<LicenseClassEntity>
    {
        public LicenseClassRepository() : base("LicenseClasses", "LicenseClassID"){
            _getAllSql = $"SELECT * FROM {_tableName} ";

        }

        //G: Handel combosition mapping maually pls
        public override LicenseClassEntity MapToEntity(SqlDataReader rdr)
        {
            return new LicenseClassEntity
            {
                LicenseClassID = (int)rdr["LicenseClassID"],
                ClassName = rdr["ClassName"].ToString(),
                ClassDescription = rdr["ClassDescription"] == DBNull.Value ? null : rdr["ClassDescription"].ToString(),
                MinimumAllowedAge = (byte)rdr["MinimumAllowedAge"],
                ValidityLength = (byte)rdr["ValidityLength"],
                ClassFees = (decimal)rdr["ClassFees"],
            };
        }

        //G: Remove FK-IDs from paramiter
        public DataRow UpdateRecord(LicenseClassEntity entity)
        {
            try
            {
                string sql = $@"UPDATE {_tableName} 
                    SET ClassName = @ClassName, ClassDescription = @ClassDescription, MinimumAllowedAge = @MinimumAllowedAge, ValidityLength = @ValidityLength, ClassFees = @ClassFees 
                    WHERE LicenseClassID = @LicenseClassID;
                    {_getAllSql}  WHERE PersonID = @PersonID;";

                SqlParameter[] parameters = {
                    new SqlParameter("@LicenseClassID", entity.LicenseClassID),
                    new SqlParameter("@ClassName", entity.ClassName),
                    new SqlParameter("@ClassDescription", (object)entity.ClassDescription ?? DBNull.Value),
                    new SqlParameter("@MinimumAllowedAge", entity.MinimumAllowedAge),
                    new SqlParameter("@ValidityLength", entity.ValidityLength),
                    new SqlParameter("@ClassFees", entity.ClassFees)
                };

                DataRow row = DataHelper.ExecuteQuery(sql, parameters).Rows[0];
                return row;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Update error in {_tableName} table: " + ex.Message);
            }

        }
    }

}

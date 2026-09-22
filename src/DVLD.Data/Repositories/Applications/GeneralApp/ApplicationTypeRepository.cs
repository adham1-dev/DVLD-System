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

    public class ApplicationTypeRepository : BaseRepository<ApplicationTypeEntity>
    {
        public ApplicationTypeRepository() : base("ApplicationTypes", "ApplicationTypeID")
        {            
        }

        //G: Handel combosition mapping maually pls
        public override ApplicationTypeEntity MapToEntity(SqlDataReader rdr)
        {
            return new ApplicationTypeEntity
            {
                ApplicationTypeID = (int)rdr["ApplicationTypeID"],
                AppTitle = rdr["AppTitle"].ToString(),
                AppFees = (decimal)rdr["AppFees"],
            };
        }


        //G: Remove FK-IDs from paramiter
        public DataRow UpdateRecord(ApplicationTypeEntity entity)
        {
            try
            {
                string sql = $@"UPDATE {_tableName} 
                        SET AppTitle = @AppTitle, AppFees = @AppFees 
                        WHERE ApplicationTypeID = @ApplicationTypeID;
                        {_getAllSql}  WHERE ApplicationTypeID = @ApplicationTypeID;";

                SqlParameter[] parameters = {
                    new SqlParameter("@ApplicationTypeID", entity.ApplicationTypeID),
                    new SqlParameter("@AppTitle", entity.AppTitle),
                    new SqlParameter("@AppFees", entity.AppFees)
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

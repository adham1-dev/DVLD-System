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
    public class TestTypeRepository : BaseRepository<TestTypeEntity>
    {
        public TestTypeRepository() : base("TestTypes", "TestTypeID"){
            _getAllSql = $"SELECT * FROM {_tableName} ";


        }


        //G: Handel combosition mapping maually pls
        public override TestTypeEntity MapToEntity(SqlDataReader rdr)
        {
            return new TestTypeEntity
            {
                TestTypeID = (int)rdr["TestTypeID"],
                TestTitle = rdr["TestTitle"].ToString(),
                Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString(),
                TestFees = (decimal)rdr["TestFees"],
            };
        }

        //G: Remove FK-IDs from paramiter
        public DataRow UpdateRecord(TestTypeEntity entity)
        {
            try
            {
                string sql = $@"UPDATE {_tableName} 
                        SET TestTitle = @TestTitle,
                        Description = @Description,
                        TestFees = @TestFees 
                        WHERE TestTypeID = @TestTypeID;
                        {_getAllSql}  WHERE TestTypeID = @TestTypeID;";

                SqlParameter[] parameters = {
                    new SqlParameter("@TestTypeID", entity.TestTypeID),
                    new SqlParameter("@TestTitle", entity.TestTitle),
                    new SqlParameter("@Description", (object)entity.Description ?? DBNull.Value),
                    new SqlParameter("@TestFees", entity.TestFees)
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

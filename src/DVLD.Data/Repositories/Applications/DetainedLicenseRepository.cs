using DVLD.Data.Entities.Applications;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Repositories.Applications
{
    public class DetainedLicenseRepository : BaseRepository<DetainedLicenseEntity>
    {
        public DetainedLicenseRepository() : base("DetainedLicenses", "DetainedLicenseID")
        {            //for Entity & mapping
            _getRecSql = @"select * from DetainedLicenses where LocalLicenseID = @ID order by DetainedLicenseID desc;";
            //View for UI
            _getAllSql = @"select * from DetainedLicensesView ";
        }
        //G: Handel combosition mapping maually pls

        public override DetainedLicenseEntity MapToEntity(SqlDataReader rdr)
        {
            return new DetainedLicenseEntity
            {
                DetainedLicenseID = (int)rdr["DetainedLicenseID"],
                LocalLicenseID = (int)rdr["LocalLicenseID"],
                ReleaseAppID = rdr["ReleaseAppID"] == DBNull.Value ? null : (int?)rdr["ReleaseAppID"],
                DetaindByUserID = (int)rdr["DetaindByUserID"],
                ReleaseDate = rdr["ReleaseDate"] == DBNull.Value ? null : (DateTime?)rdr["ReleaseDate"],
                DetainDate = (DateTime)rdr["DetainDate"],
                FineFees = (decimal)rdr["FineFees"],
                IsReleased = (bool)rdr["IsReleased"],
            };
        }

        bool IsLicDetained(int LicID , SqlConnection conn)
        {
            string sql = $"SELECT top(1) 1 FROM {_tableName} WHERE  LocalLicenseID = @ID And IsReleased = 0";

            bool IsExist = false;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ID", LicID);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                        IsExist = true;
                    else
                        IsExist = false;
                }

            }

            return IsExist;

        }

        public DataRow DetaineLicense(DetainedLicenseEntity entity)
        {
            try
            {
                string sql = $@"
                            update LocalLicense SET IsDetained = 1 where LocalLicenseID = @LocalLicenseID;
                            INSERT INTO {_tableName} (LocalLicenseID, DetaindByUserID, FineFees) 
                            VALUES (@LocalLicenseID, @DetaindByUserID, @FineFees); 
                            {_getAllSql}  WHERE DetainedLicenseID = (SELECT SCOPE_IDENTITY()); ";

                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {
                    conn.Open();

                    if (IsLicDetained(entity.LocalLicenseID, conn))
                        throw new Exception("This License aleady is detained");

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@LocalLicenseID", entity.LocalLicenseID);   
                        cmd.Parameters.AddWithValue("@DetaindByUserID", entity.DetaindByUserID);
                        cmd.Parameters.AddWithValue("@FineFees", entity.FineFees);

                        DataTable dt = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt.Rows.Count > 0 ? dt.Rows[0] : throw new Exception("Filed to get Derained License Data");
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new Exception($"Add error in {_tableName} table: " + ex.Message);
            }

        }

        public DataRow ReleaseLicense(DetainedLicenseEntity entity)
        {
            try
            {
                string CreateApp = $@"INSERT INTO Application (CreatedByUserID, PersonID, ApplicationTypeID) 
                                        VALUES (@CreatedByUserID, @PersonID, 5); ";

                string ReleaseLic = $@"update LocalLicense SET IsDetained = 0 where LocalLicenseID = @LocalLicenseID;
                                        UPDATE {_tableName} 
                                        SET ReleaseAppID = (SELECT SCOPE_IDENTITY()), ReleaseDate = getdate(), IsReleased = 1
                                        WHERE DetainedLicenseID = @DetainedLicenseID;
                                        {_getAllSql}  WHERE DetainedLicenseID = @DetainedLicenseID;";

                string sql = CreateApp + "\n" + ReleaseLic;


                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {
                    conn.Open();


                    if (!IsLicDetained(entity.LocalLicenseID, conn))
                        throw new Exception("This License is not detained to Release it.");

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@LocalLicenseID", entity.LocalLicenseID);
                        cmd.Parameters.AddWithValue("@DetainedLicenseID", entity.DetainedLicenseID);
                        cmd.Parameters.AddWithValue("@CreatedByUserID", entity.ReleasedBy);
                        cmd.Parameters.AddWithValue("@PersonID", entity.PersonID);

                        DataTable dt = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt.Rows.Count > 0 ? dt.Rows[0] : throw new Exception("Filed to get Derained License Data");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Add error in {_tableName} table: " + ex.Message);
            }


        }

    }
}

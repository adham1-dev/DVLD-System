using DVLD.Data.Entities;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Repositories.Applications.L_D_LApp
{
    public class L_D_LAppRepository : BaseRepository<L_D_LAppEntity>
    {
        public L_D_LAppRepository() : base("L_D_LApp", "L_D_LAppID")
        {            //for Entity & mapping
            _getRecSql = @"SELECT * FROM L_D_LAppView where L_D_LAppID = @ID; ";
            //View for UI
            _getAllSql = @"SELECT
                            L_D_LAppID, 
                            ClassName as DrivingClass, 
                            NationalNo, 
                            FirstName + ' ' + SecondName + ' ' + ISNULL(ThirdName + ' ', '') + LastName AS FullName, 
                            Date as ApplicationDate, 
                            PassedTests, 
                            ApplicationStatus as Status
                            FROM L_D_LAppView ";
        }


        public override DataTable GetAllRecords(string FilterBy = null, object Value = null)
        {
            string sql = @"SELECT
                            L_D_LAppID, 
                            ClassName as DrivingClass, 
                            NationalNo, 
                            FirstName + ' ' + SecondName + ' ' + ISNULL(ThirdName + ' ', '') + LastName AS FullName, 
                            Date as ApplicationDate, 
                            PassedTests, 
                            ApplicationStatus as Status
                            FROM L_D_LAppView Where PassedTests is not null; ";

            return DataHelper.ExecuteQuery(sql);
        }

        public DataRow CancelApplecation(int id)
        {
            string sql = $@"UPDATE Application
                            SET ApplicationStatus = 'Cancel'
                            WHERE  ApplicationID = (select top 1 ApplicationID from L_D_LApp where L_D_LAppID = @L_D_LAppID);
                            {_getAllSql}  WHERE L_D_LAppID = @L_D_LAppID";


            DataTable dt = DataHelper.ExecuteQuery(sql, new SqlParameter[] { new SqlParameter("@L_D_LAppID", id) });

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }






        //G: Handel combosition mapping maually pls


        public override L_D_LAppEntity MapToEntity(SqlDataReader rdr)
        {
            return new L_D_LAppEntity
            {
                L_D_LAppID = Convert.ToInt32(rdr["L_D_LAppID"]),
                PassedTests = Convert.ToByte(rdr["PassedTests"]),
                Application = new ApplicationEntity
                {
                    ApplicationID = Convert.ToInt32(rdr["ApplicationID"]),
                    CreatedBy = new UserEntity
                    {
                        UserID = Convert.ToInt32(rdr["UserID"]),
                        UserName = rdr["UserName"].ToString(),
                    },
                    PersonInfo = new PersonEntity
                    {
                        PersonID = Convert.ToInt32(rdr["PersonID"]),
                        NativeCountry = new CountryEntity
                        {
                            CountryID = Convert.ToInt32(rdr["CountryID"]),
                            CountryName = rdr["CountryName"].ToString()
                        },
                        NationalNo = rdr["NationalNo"].ToString(),
                        FirstName = rdr["FirstName"].ToString(),
                        SecondName = rdr["SecondName"].ToString(),
                        ThirdName = rdr["ThirdName"] == DBNull.Value ? null : rdr["ThirdName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(rdr["DateOfBirth"]),
                        Gender = Convert.ToByte(rdr["Gender"]),
                        Address = rdr["Address"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Email = rdr["Email"] == DBNull.Value ? null : rdr["Email"].ToString(),
                        ImagePath = rdr["ImagePath"] == DBNull.Value ? null : rdr["ImagePath"].ToString(),
                    },
                    ApplicationStatus = rdr["ApplicationStatus"].ToString(),
                    Date = Convert.ToDateTime(rdr["Date"]),
                    AppType = new ApplicationTypeEntity
                    {
                        ApplicationTypeID = Convert.ToInt32(rdr["ApplicationTypeID"]),
                        AppTitle = rdr["AppTitle"].ToString(),
                        AppFees = Convert.ToDecimal(rdr["AppFees"]),
                    },
                },
                LicenseClass = new LicenseClassEntity
                {
                    LicenseClassID = Convert.ToInt32(rdr["LicenseClassID"]),
                    ClassName = rdr["ClassName"].ToString(),
                    MinimumAllowedAge = Convert.ToByte(rdr["MinimumAllowedAge"]),
                    ClassFees = Convert.ToDecimal(rdr["ClassFees"]),
                },
            };
        }
        void Validate(SqlConnection conn, L_D_LAppEntity entity)
        {
            string validateSql = @"SELECT TOP 1 A.ApplicationStatus
                                        FROM Application A
                                        JOIN L_D_LApp L ON A.ApplicationID = L.ApplicationID
                                        WHERE A.PersonID = @PersonID 
                                        AND L.LicenseClassID = @LicenseClassID ; ";

            using (SqlCommand Vcmd = new SqlCommand(validateSql, conn))
            {
                Vcmd.Parameters.AddWithValue("@PersonID", entity.Application.PersonInfo.PersonID);
                Vcmd.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClass.LicenseClassID);

                object result = Vcmd.ExecuteScalar();

                if (result?.ToString().ToLower() == "new")
                    throw new Exception($"this Preson already have an active application on ({entity.LicenseClass.ClassName}).");
                if ( result != null && !(result?.ToString().ToLower()  == "cancel"))
                    throw new Exception($"this Preson already have a completed application for ({entity.LicenseClass.ClassName}).");
            }

        }



        public DataRow AddRecord(L_D_LAppEntity entity)
        {
            try
            {
                string insertApp = $@"INSERT INTO Application (CreatedByUserID, PersonID, ApplicationTypeID, ApplicationStatus) 
                        VALUES (@CreatedByUserID, @PersonID, 1, 'New'); ";

                string insertL_D_LApp = $@"INSERT INTO {_tableName} (LicenseClassID, ApplicationID) 
                        VALUES (@LicenseClassID, (SELECT SCOPE_IDENTITY())); 
                        {_getAllSql}  WHERE L_D_LAppID = (SELECT SCOPE_IDENTITY()); ";

                string sql = insertApp + "\n" + insertL_D_LApp;

                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {
                    conn.Open();

                    Validate(conn,entity);
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CreatedByUserID", entity.Application.CreatedBy.UserID);
                        cmd.Parameters.AddWithValue("@PersonID", entity.Application.PersonInfo.PersonID);
                        cmd.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClass.LicenseClassID); 

                        DataTable dt = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Add error in {_tableName} table: " + ex.Message);
            }
        }

        public DataRow UpdateRecord(L_D_LAppEntity entity)
        {
            try
            {
                string sql = $@"UPDATE {_tableName} 
                        SET LicenseClassID = @LicenseClassID
                        WHERE L_D_LAppID = @L_D_LAppID;
                        {_getAllSql}  WHERE L_D_LAppID = @L_D_LAppID;";


                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {
                    conn.Open();

                    Validate(conn, entity);
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@L_D_LAppID", entity.L_D_LAppID);
                        cmd.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClass.LicenseClassID);

                        DataTable dt = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Update error in {_tableName} table: " + ex.Message);
            }

        }
    }
}

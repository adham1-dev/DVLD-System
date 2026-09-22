using DVLD.Data.Entities;
using DVLD.Data.Generic;
using DVLD.Data.Repositories.Applications.L_D_LApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Repositories.Applications.TestApp
{
    public class TestAppointmentRepository : BaseRepository<TestAppointmentEntity>
    {
        public TestAppointmentRepository() : base("TestAppointment", "TestAppointmentID")
        {            //for Entity & mapping
            _getRecSql = @"select * from TestAppointment where TestAppointmentID = @ID; ";
            //View for UI
            _getAllSql = @"SELECT        
                            TestAppointmentID AS [Appointment ID], 
                            TestDate AS [Appointment Date], 
                            PaidFees AS [Paid Fees], 
                            IsLocked AS [Is Locked]
                        FROM  AppointmentsView ";
        }
        //G: Handel combosition mapping maually pls

        public virtual DataTable GetAllRecords(int LD_AppID , int TestTypeID)
        {

            string sql = _getAllSql + $" where L_D_LAppID = @LD_AppID And TestTypeID = @TestTypeID;";
            SqlParameter[] parameter = {
                 new SqlParameter("@LD_AppID", LD_AppID),
                 new SqlParameter("@TestTypeID", TestTypeID),
            };
            return DataHelper.ExecuteQuery(sql, parameter);
        }



        public override TestAppointmentEntity MapToEntity(SqlDataReader rdr)
        {
            return new TestAppointmentEntity
            {
                TestAppointmentID = (int)rdr["TestAppointmentID"],
                RetakeAppID = rdr["RetakeAppID"] == DBNull.Value ? null : (int?)rdr["RetakeAppID"],
                TestTypeID = (int)rdr["TestTypeID"],
                L_D_LAppID = (int)rdr["L_D_LAppID"],
                TestResult = rdr["TestResult"] == DBNull.Value ? null : (bool?)rdr["TestResult"],
                TestDate = (DateTime)rdr["TestDate"],
            };
        }


        public int? GetRetakeIDAndVaildation( TestAppointmentEntity entity)
        {
            int? Result = null;
                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {
                    conn.Open();

                    if (IsValidAppStatus(conn, entity))
                    {

                        string validateSql = @"SELECT TOP 1 TestResult
                                            FROM TestAppointment 
                                            WHERE L_D_LAppID = @L_D_LAppID 
                                            AND TestTypeID = @TestTypeID 
                                            ORDER BY TestAppointmentID DESC;";

                        using (SqlCommand Vcmd = new SqlCommand(validateSql, conn))
                        {
                            Vcmd.Parameters.AddWithValue("@L_D_LAppID", entity.L_D_LAppID);
                            Vcmd.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);

                            object result = Vcmd.ExecuteScalar();

                            if (result == null)
                            {
                            //Dont make Retake test
                                Result=  null;
                            }
                            else
                            {

                                if (result == DBNull.Value)
                                    throw new Exception("Person Already have an active appointment for this test, You cannot add new appointment");
                                else if ((bool)result == true)
                                    throw new Exception("Person Already passed this test, You cannot add new appointment");
                                else
                                {
                                //make Retake test
                                    Result = InsertRetakeApplication(conn, entity);

                                }


                            }
                        }

                    }
                }

            return Result;
        }

        bool IsValidAppStatus(SqlConnection conn, TestAppointmentEntity entity)
        {
            string validateSql = @"SELECT TOP 1 ApplicationStatus
                                        FROM Application 
                                        WHERE ApplicationID = @ApplicationID ;";

            using (SqlCommand Vcmd = new SqlCommand(validateSql, conn))
            {
                Vcmd.Parameters.AddWithValue("@ApplicationID", entity.BaseAppID);

                object result = Vcmd.ExecuteScalar();

                if (result == null)
                    throw new Exception("There is no application have this application id");
                else
                {

                    if (result == DBNull.Value)
                        throw new Exception("application status is UnKnowned!!");
                    else if (result.ToString().ToLower() == "cancel")
                        throw new Exception("This application status is canceld make anthor application");
                    else
                        return true;

                }
            }

        }

        int? InsertRetakeApplication(SqlConnection conn, TestAppointmentEntity entity)
        {
            //Need Retake
            string insertApp = $@"INSERT INTO Application (CreatedByUserID, PersonID, ApplicationTypeID) 
                                                    VALUES (@CreatedByUserID, @PersonID, 7); 
                                                    SELECT SCOPE_IDENTITY();";
            using (SqlCommand cmd = new SqlCommand(insertApp, conn))
            {

                cmd.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByID);
                cmd.Parameters.AddWithValue("@PersonID", entity.PersonID);

                object result = cmd.ExecuteScalar();

                return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : (int?)null;
            }

        }



        public DataRow AddRecord(TestAppointmentEntity entity)
        {
            try
            {

                string sql = $@"INSERT INTO {_tableName} (RetakeAppID, TestTypeID, L_D_LAppID, TestDate, PaidFees) 
                    VALUES ((SELECT SCOPE_IDENTITY()), @TestTypeID, @L_D_LAppID, @TestDate, @PaidFees); 
                    {_getAllSql}  WHERE TestAppointmentID = (SELECT SCOPE_IDENTITY()); ";


                using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@RetakeAppID", (object)entity.RetakeAppID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);
                        cmd.Parameters.AddWithValue("@L_D_LAppID", entity.L_D_LAppID);
                        cmd.Parameters.AddWithValue("@TestDate", entity.TestDate);
                        cmd.Parameters.AddWithValue("@PaidFees", entity.PaidFees);

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
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

        //G: Remove FK-IDs from paramiter
        public DataRow UpdateRecord(TestAppointmentEntity entity)
        {
            try
            {
                string sql = $@"UPDATE {_tableName} 
                        SET  
                            TestResult = @TestResult, 
                            TestDate = @TestDate 
                        WHERE TestAppointmentID = @TestAppointmentID;
                        {_getAllSql}  WHERE TestAppointmentID = @TestAppointmentID;";

                var parameters = new List<SqlParameter> {
                    new SqlParameter("@TestAppointmentID", entity.TestAppointmentID),
                    new SqlParameter("@TestResult", (object)entity.TestResult ?? DBNull.Value),
                    new SqlParameter("@TestDate", entity.TestDate),
                };



                if (entity.TestResult == true)
                {
                    sql += @" UPDATE L_D_LApp
                                SET
                                    PassedTests = @PassedTests
                                 Where L_D_LAppID = @L_D_LAppID;";

                    parameters.Add(new SqlParameter("@PassedTests", entity.TestTypeID));
                    parameters.Add(new SqlParameter("@L_D_LAppID", entity.L_D_LAppID));

                    if (entity.TestTypeID == 3)
                    {
                        sql += "Update Application SET ApplicationStatus = 'Completed' where ApplicationID = @ApplicationID;";
                        parameters.Add(new SqlParameter("@ApplicationID", entity.BaseAppID));
                    }

                }

                DataRow row = DataHelper.ExecuteQuery(sql, parameters.ToArray()).Rows[0];
                return row;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Update error in {_tableName} table: " + ex.Message);
            }

        }
    }
}

using DVLD.Data.Entities;
using DVLD.Data.Entities.Applications.LocalDrivingLicence;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Data.Repositories.Applications.LocalDrivingLicense
{
    public class LocalLicenseRepository : BaseRepository<LocalLicenseEntity>
    {
        public LocalLicenseRepository() : base("LocalLicense", "LocalLicenseID")
        {
            //for Entity & mapping
            _getRecSql = @"update LocalLicense SET IsActive = CASE WHEN GETDATE() > ExpirationDate or IsActive = 0 THEN 0 ELSE 1 END WHERE LocalLicenseID = @ID;
                            SELECT top 1 * FROM LicenseInfoView Where LocalLicenseID = @ID";
            //View for UI
            _getAllSql = @"update LocalLicense SET IsActive = CASE WHEN GETDATE() > ExpirationDate or IsActive = 0 THEN 0 ELSE 1 END;
                        SELECT 
                           [ApplicationID]
                          ,[ClassName]
                          ,[ReleaseDate]
                          ,[ExpirationDate]
                          ,[IsDetained]
                          ,[IssueReason]
                           ,IsActive
                          FROM LocalLicensesHistoryView ";
        }
        //G: Handel combosition mapping maually pls

        public override LocalLicenseEntity MapToEntity(SqlDataReader rdr)
        {
            return new LocalLicenseEntity
            {
                LocalLicenseID = (int)rdr["LocalLicenseID"],
                L_D_LApp = new L_D_LAppEntity
                {
                    L_D_LAppID = (int)rdr["L_D_LAppID"],
                    LicenseClass = new LicenseClassEntity
                    {
                        LicenseClassID = (int)rdr["LicenseClassID"],
                        
                        ClassName = rdr["ClassName"].ToString(),
                    },
                    Application = new ApplicationEntity
                    {
                        PersonInfo = new PersonEntity
                        {
                            PersonID = (int)rdr["PersonID"],
                            FirstName = rdr["FirstName"].ToString(),
                            SecondName = rdr["SecondName"].ToString(),
                            ThirdName = rdr["ThirdName"] == DBNull.Value ? null : rdr["ThirdName"].ToString(),
                            LastName = rdr["LastName"].ToString(),

                            NationalNo = rdr["NationalNo"].ToString(),
                            Gender = (byte)rdr["Gender"],
                            DateOfBirth = (DateTime)rdr["DateOfBirth"],
                            ImagePath = rdr["ImagePath"] == DBNull.Value ? null : rdr["ImagePath"].ToString(),

                        },
                    },

                },

                ReleaseDate = (DateTime)rdr["ReleaseDate"],
                IssueReason = rdr["IssueReason"].ToString(),
                Notes = rdr["Notes"] == DBNull.Value ? null : rdr["Notes"].ToString(),

                IsActive = (bool)rdr["IsActive"],
                DriverID = (int)rdr["DriverID"],
                ExpirationDate = (DateTime)rdr["ExpirationDate"],
                IsDetained = (bool)rdr["IsDetained"],
            };
        }


        int GetDriverID(int PersonID, SqlConnection conn)
        {
            //IF ELSE -> we deal with diffrent Lic classes | same person can have many lic class
            string sql = @" 
                            DECLARE @ActualDriverID INT;

                            IF EXISTS(SELECT 1 FROM Drivers WHERE PersonID = @PersonID)
                            BEGIN
                                SELECT @ActualDriverID = DriverID FROM Drivers WHERE PersonID = @PersonID;
                            END
                            ELSE
                            BEGIN
                                INSERT INTO Drivers(PersonID) 
                                VALUES(@PersonID);
                                SET @ActualDriverID = SCOPE_IDENTITY();
                            END

                            SELECT @ActualDriverID;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : throw new Exception("Faild to Get Driver ID");
            }
        }

        bool IsPassedTests(int L_D_LAppID, SqlConnection conn)
        {
            string sql = "SELECT PassedTests FROM L_D_LApp Where [L_D_LAppID] = @L_D_LAppID";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@L_D_LAppID", L_D_LAppID);
                object result = cmd.ExecuteScalar();


                if (result is byte)
                    return (byte)result == 3;
                else
                    throw new Exception("This LDL_ID dosn't exist.");
            }
        }

        bool IsHaveThisLicClass(int PersonID, int LicenseClassID, SqlConnection conn)
        {
            string sql = @"SELECT TOP (1) 1
                            FROM LocalLicensesHistoryView 
                            where PersonID = @PersonID and LicenseClassID = @LicenseClassID;";

            bool IsHave = false;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PersonID", PersonID);
                cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                        IsHave = true;
                    else
                        IsHave = false;
                }

            }

            return IsHave;

        }

        bool IsActive(int LicID, SqlConnection conn)
        {
            string sql = "SELECT top 1 IsActive FROM LocalLicense WHERE LocalLicenseID = @LocalLicenseID";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@LocalLicenseID", LicID);
                object result = cmd.ExecuteScalar();


                if (result is bool)
                    return (bool)result;
                else
                    throw new Exception("This License dosn't exist.");
            }
        }


        void AddReplacmentApp(Reason reason, int CreatedByUserID, int PersonID, int LicClassID, SqlConnection conn
            , out DateTime Date, out int RAppID, out int RLDL_AppID)
        {
            string sql = $@"
                        INSERT INTO Application (CreatedByUserID, PersonID, ApplicationTypeID) 
                                    VALUES (@CreatedByUserID, @PersonID, {(int)reason}); 
                                    SELECT TOP (1) [ApplicationID],[Date] FROM [Application] Where [ApplicationID] = (SELECT SCOPE_IDENTITY());";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                cmd.Parameters.AddWithValue("@PersonID", PersonID);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        RAppID = Convert.ToInt32(rdr["ApplicationID"]);
                        Date = Convert.ToDateTime(rdr["Date"]);
                    }
                    else
                        throw new Exception("Faild to add Replacement Application");
                }
            }


            string insertL_D_LApp = $@"INSERT INTO L_D_LApp (LicenseClassID, ApplicationID, PassedTests , IssueReason) 
                        VALUES (@LicenseClassID, @RApp , null , '{reason.ToString()}'); 
                        SELECT SCOPE_IDENTITY(); ";

            using (SqlCommand cmd = new SqlCommand(insertL_D_LApp, conn))
            {
                cmd.Parameters.AddWithValue("@RApp", RAppID);
                cmd.Parameters.AddWithValue("@LicenseClassID", LicClassID);
                object result = cmd.ExecuteScalar();

                if (result == null)
                    throw new Exception("Faild to Get License ID");
                else
                {
                    RLDL_AppID = Convert.ToInt32(result);
                }
            }



        }

        //It works form all classes 
        public int AddFirestTimeLic(LocalLicenseEntity entity)
        {

            using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
            {
                conn.Open();
                int L_D_LAppID = entity.L_D_LApp.L_D_LAppID;
                int PersonID = entity.L_D_LApp.Application.PersonInfo.PersonID;

                if (IsHaveThisLicClass(PersonID, entity.L_D_LApp.LicenseClass.LicenseClassID, conn))
                    throw new Exception("This Person alrady have this licence.");

                if (!IsPassedTests(L_D_LAppID, conn))
                    throw new Exception("This Person Dosn't Pass all tests yet.");


                string sql = $@" UPDATE [Application] SET [ApplicationStatus] = 'Issued' where [ApplicationID] = @ApplicationID;
                        INSERT INTO {_tableName} (DriverID, L_D_LAppID, ValidityLength, Notes) 
                        VALUES (@DriverID, @L_D_LAppID, @ValidityLength, @Notes); 
                        SELECT SCOPE_IDENTITY();";

                int DriverID = GetDriverID(PersonID, conn);
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    cmd.Parameters.AddWithValue("@ApplicationID", entity.L_D_LApp.Application.ApplicationID);
                    cmd.Parameters.AddWithValue("@L_D_LAppID", entity.L_D_LApp.L_D_LAppID);
                    cmd.Parameters.AddWithValue("@ValidityLength", entity.L_D_LApp.LicenseClass.ValidityLength);
                    cmd.Parameters.AddWithValue("@Notes", (object)entity.Notes ?? DBNull.Value);

                    object result = cmd.ExecuteScalar();
                    return (result != null) ? Convert.ToInt32(result) : throw new Exception("Faild to Get License ID");
                }

            }


        }

        public enum Reason { Lost = 3, Damaged = 4 }

        public void AddReplacementLic(Reason reason, in int PersonID, in int CreatedByID, in int LicClassID, in int LocalLicID
            , out int ReplacedLicID, out int RAppID, out int RLDL_AppID, out DateTime Date)
        {

            using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
            {
                conn.Open();

                if(!IsActive(LocalLicID, conn))
                    throw new Exception("This License is not active to replace it.");

                AddReplacmentApp(reason, CreatedByID, PersonID, LicClassID, conn
                    , out Date, out RAppID, out RLDL_AppID);

                //Add Lic
                string sql = @"update LocalLicense SET IsActive = 0 where LocalLicenseID = @LocalLicenseID;

                                INSERT INTO LocalLicense (DriverID , ValidityLength, L_D_LAppID)
                                    select DriverID , ValidityLength,@RL_D_LAppID From LocalLicense Where LocalLicenseID = @LocalLicenseID;

                                SELECT SCOPE_IDENTITY();";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@LocalLicenseID", LocalLicID);
                    cmd.Parameters.AddWithValue("@RL_D_LAppID", RLDL_AppID);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                        throw new Exception("Faild to Get License ID");
                    else
                    {
                        ReplacedLicID = Convert.ToInt32(result);
                    }

                }

            }

        }

        public LocalLicenseEntity GetEntityBy_LDLApp_ID(int L_D_LAppID)
        {
            string sql = @"update LocalLicense SET IsActive = CASE WHEN GETDATE() > ExpirationDate THEN 0 ELSE 1 END WHERE LocalLicenseID = @ID;
                            SELECT * FROM LicenseInfoView Where L_D_LAppID = @ID ";

            LocalLicenseEntity Entity = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ID", L_D_LAppID);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                        Entity = MapToEntity(rdr);
                }
            }

            return Entity;
        }

    }
}

using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD.Data.Repositories.Applications.LocalDrivingLicense.LocalLicenseRepository;

namespace DVLD.Data.Repositories
{
    public class LoginValidationRepository
    {
        public LoginValidationRepository(in string  username, in string password , out int ID)
        {
            string sql = $"SELECT top(1) [UserID], [IsActive]  FROM Users WHERE [UserName] = @UserName and [Password] = @Password";



            using (SqlConnection conn = new SqlConnection(DataHelper._connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (!Convert.ToBoolean(rdr["IsActive"]))
                            throw new Exception("This user dose not active in the system.");

                        ID = Convert.ToInt32(rdr["UserID"]);
                    }
                    else
                        throw new Exception("Wrong Username Or Password.");
                }
            }

        }
    }
}

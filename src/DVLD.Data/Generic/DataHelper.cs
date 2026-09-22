using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Generic
{
    public static class DataHelper
    {
        public static string _connectionString = "Server=.;Database=DVLD;Integrated Security=True;TrustServerCertificate=True;";
        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }

        }

        public static object ExecuteScalar(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }

        }

        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                    return dt;
                }
            }
        }

        //for seeking search by indixing in DB
        public static SqlDbType GetSqlDbType(object value)
        {

            if (value is string) 
                return SqlDbType.NVarChar;

            if (value is int) 
                return SqlDbType.Int;

            if (value is DateTime) 
                return SqlDbType.DateTime;

            if (value is bool) 
                return SqlDbType.Bit;

            if (value is byte) 
                return SqlDbType.TinyInt;

            if (value is decimal || value is double) 
                return SqlDbType.Decimal;

            if (value is Guid) 
                return SqlDbType.UniqueIdentifier;

            return SqlDbType.Variant; // نوع عام في حال لم يتطابق شيء
        }

    }
}

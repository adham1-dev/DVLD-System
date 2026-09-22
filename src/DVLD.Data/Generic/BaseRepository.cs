using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DVLD.Data.Generic
{
    public abstract class BaseRepository <T> : IBaseRepository where T : class
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected readonly string _primaryKey;
        protected string _getAllSql = null;
        protected string _getRecSql = null;
        protected BaseRepository(string tableName , string primaryKey)
        {
            _connectionString = DataHelper._connectionString;
            _tableName = tableName;
            _primaryKey = primaryKey;
        }

        public abstract T MapToEntity(SqlDataReader rdr);
        public virtual DataTable GetAllRecords(string FilterBy = null , object Value = null)
        {
            if (_getAllSql == null)
                _getAllSql = $"SELECT * FROM {_tableName} ;";

            if(FilterBy != null && Value != null)
            {
                string sql = _getAllSql + $" where [{FilterBy}] = @value";
                SqlParameter parameter = new SqlParameter("@value", Value);
                parameter.SqlDbType = DataHelper.GetSqlDbType(Value);
                return DataHelper.ExecuteQuery(sql, new SqlParameter[] { parameter });
            }

            return DataHelper.ExecuteQuery(_getAllSql);
        }


        public DataRow GetDataRow (int ID)
        {
            if (_getAllSql == null)
                _getAllSql = $"SELECT * FROM {_tableName} ";

            string sql = _getAllSql + $" WHERE {_primaryKey} = @ID";
                SqlParameter parameter = new SqlParameter("@ID", ID);
                parameter.SqlDbType = DataHelper.GetSqlDbType(ID);

            DataRow row = DataHelper.ExecuteQuery(sql, new SqlParameter[] { parameter }).Rows[0];

            return row;

        }

        public T GetEntity (int ID)
        {
            if (_getRecSql == null)
                _getRecSql = $"SELECT * FROM {_tableName} WHERE {_primaryKey} = @ID";
            T Entity = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(_getRecSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                            Entity = MapToEntity(rdr);
                    }
                }

            return Entity;
        }

        public bool IsExist(int ID)
        {
            string sql = $"SELECT top(1) 1 FROM {_tableName} WHERE {_primaryKey} = @ID";

            bool IsExist = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                conn.Open();

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

        public bool Delete(int ID)
        {
            string sql = $"DELETE FROM {_tableName} WHERE {_primaryKey} = @ID";
            SqlParameter[] parameters = { new SqlParameter("@ID" , ID) }; 
            return 0 < DataHelper.ExecuteNonQuery(sql, parameters);
        }

    }
}

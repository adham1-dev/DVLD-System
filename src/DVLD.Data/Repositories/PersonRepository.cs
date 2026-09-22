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
    public class PersonRepository : BaseRepository<PersonEntity>
    {
        public PersonRepository() : base("People", "PersonID") {
            //for Entity & mapping
            _getRecSql = @"SELECT   Countries.*, People.* FROM  People 
                            INNER JOIN Countries ON People.CountryID = Countries.CountryID Where PersonID = @ID";
            //View for UI
            _getAllSql = @"select * from ManagePeopleView ";
        }

        public DataRow Add(PersonEntity entity)
        {
            string sql = $@"INSERT INTO {_tableName} (CountryID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, ImagePath) 
                        VALUES (@CountryID, @NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @ImagePath);
                        {_getAllSql}  WHERE PersonID = (SELECT SCOPE_IDENTITY()) ;";

            SqlParameter[] parameters = {
                new SqlParameter("@CountryID", entity.NativeCountry.CountryID),
                new SqlParameter("@NationalNo", entity.NationalNo),
                new SqlParameter("@FirstName", entity.FirstName),
                new SqlParameter("@SecondName", entity.SecondName),
                new SqlParameter("@ThirdName", (object)entity.ThirdName ?? DBNull.Value),
                new SqlParameter("@LastName", entity.LastName),
                new SqlParameter("@DateOfBirth", entity.DateOfBirth),
                new SqlParameter("@Gender", entity.Gender),
                new SqlParameter("@Address", entity.Address),
                new SqlParameter("@Phone", entity.Phone),
                new SqlParameter("@Email", (object)entity.Email ?? DBNull.Value),
                new SqlParameter("@ImagePath", (object)entity.ImagePath ?? DBNull.Value)
            };


            DataRow row = DataHelper.ExecuteQuery(sql, parameters).Rows[0];

            return row;

        }

        public DataRow Update(PersonEntity entity)
        {
            if(entity.PersonID <=0 ) throw new Exception("Non valid id cant update");

            string sql = $@"UPDATE {_tableName} 
                        SET CountryID = @CountryID, NationalNo = @NationalNo, FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address, Phone = @Phone, Email = @Email, ImagePath = @ImagePath 
                        WHERE PersonID = @PersonID;
                        {_getAllSql}  WHERE PersonID = @PersonID;";

            SqlParameter[] parameters = {
                new SqlParameter("@PersonID", entity.PersonID),
                new SqlParameter("@CountryID", entity.NativeCountry.CountryID),
                new SqlParameter("@NationalNo", entity.NationalNo),
                new SqlParameter("@FirstName", entity.FirstName),
                new SqlParameter("@SecondName", entity.SecondName),
                new SqlParameter("@ThirdName", (object)entity.ThirdName ?? DBNull.Value),
                new SqlParameter("@LastName", entity.LastName),
                new SqlParameter("@DateOfBirth", entity.DateOfBirth),
                new SqlParameter("@Gender", entity.Gender),
                new SqlParameter("@Address", entity.Address),
                new SqlParameter("@Phone", entity.Phone),
                new SqlParameter("@Email", (object)entity.Email ?? DBNull.Value),
                new SqlParameter("@ImagePath", (object)entity.ImagePath ?? DBNull.Value)
            };

            DataRow row = DataHelper.ExecuteQuery(sql, parameters).Rows[0];
            return row;
        }

        public PersonEntity GetEntityByColName(string ColName ,string value)
        {
            
            string   sql = $@"SELECT   Countries.*, People.* FROM  People
                            INNER JOIN Countries ON People.CountryID = Countries.CountryID 
                            WHERE {ColName} = @value";

            PersonEntity Entity = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@value", value);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                        Entity = MapToEntity(rdr);
                }
            }

            return Entity;
        }

        public string GetImagePath (int ID)
        {

            string sql = $"SELECT top 1 ImagePath FROM {_tableName} WHERE {_primaryKey} = @ID";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                        return rdr["ImagePath"] == DBNull.Value ? null : rdr["ImagePath"].ToString();
                    else
                        return null;
                }
            }

        }

        public override PersonEntity MapToEntity(SqlDataReader rdr)
        {
            return new PersonEntity
            {
                PersonID = (int)rdr["PersonID"],

                NativeCountry = new CountryEntity
                {
                    CountryID = (int)rdr["CountryID"],
                    CountryName = rdr["CountryName"].ToString()
                },

                NationalNo = rdr["NationalNo"].ToString(),
                FirstName = rdr["FirstName"].ToString(),
                SecondName = rdr["SecondName"].ToString(),
                ThirdName = rdr["ThirdName"] == DBNull.Value ? null : rdr["ThirdName"].ToString(),
                LastName = rdr["LastName"].ToString(),
                DateOfBirth = (DateTime)rdr["DateOfBirth"],
                Gender = (byte)rdr["Gender"],
                Address = rdr["Address"].ToString(),
                Phone = rdr["Phone"].ToString(),
                Email = rdr["Email"] == DBNull.Value ? null : rdr["Email"].ToString(),
                ImagePath = rdr["ImagePath"] == DBNull.Value ? null : rdr["ImagePath"].ToString(),
            };
        }

    }
}

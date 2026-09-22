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
    public class UserRepository : BaseRepository<UserEntity>
    {
        public UserRepository() : base("Users", "UserID")
        {            //for Entity & mapping
            _getRecSql = @"select Users.*,People.* ,Countries.CountryName
                            from Users 
                            join People on Users.PersonID = People.PersonID 
                            join Countries on People.CountryID = Countries.CountryID
                            WHERE UserID = @ID;";
            //View for UI
            _getAllSql = @"select * from ManageUsersView ";
        }
        // Handel combosition mapping maually pls

        public override UserEntity MapToEntity(SqlDataReader rdr)
        {
            return new UserEntity
            {
                UserID = (int)rdr["UserID"],
                Person = new PersonEntity
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

                },
                UserName = rdr["UserName"].ToString(),
                Password = rdr["Password"].ToString(),
                IsActive = (bool)rdr["IsActive"],
            };
        }

        public DataRow AddRecord(UserEntity entity)
        {
            try
            {
                string sql = $@"INSERT INTO {_tableName} (PersonID, UserName, Password, IsActive) 
                            VALUES (@PersonID, @UserName, @Password, @IsActive); 
                            {_getAllSql}  WHERE UserID = (SELECT SCOPE_IDENTITY());" ;

                SqlParameter[] parameters = {
                    new SqlParameter("@PersonID", entity.Person.PersonID),
                    new SqlParameter("@UserName", entity.UserName),
                    new SqlParameter("@Password", entity.Password),
                    new SqlParameter("@IsActive", entity.IsActive)
                };


                DataRow row = DataHelper.ExecuteQuery(sql, parameters).Rows[0];
                return row;
            }
            catch (SqlException ex) 
            {
                if(ex.Number == SqlExeptionCodes.UQ)
                {
                    if (ex.Message.Contains("UQ_PersonID"))
                    {
                        throw new Exception("This Person already is a user ! ");
                    }
                    else if (ex.Message.Contains("UQ_UserName"))
                    {
                        throw new Exception("This User name is already taken, please Choose anthor one");
                    }
                }
                throw new Exception("Add error: " + ex.Message);
            }

        }

        public DataRow UpdateRecord(UserEntity entity)
        {

            try
            {
                string sql = $@"UPDATE {_tableName} 
                            SET UserName = @UserName, Password = @Password, IsActive = @IsActive 
                            WHERE UserID = @UserID;
                            {_getAllSql}  WHERE UserID = @UserID;";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", entity.UserID),

                    new SqlParameter("@UserName", entity.UserName),
                    new SqlParameter("@Password", entity.Password),
                    new SqlParameter("@IsActive", entity.IsActive)
                };

                DataRow row = DataHelper.ExecuteQuery(sql, parameters).Rows[0];
                return row;
            }
            catch (SqlException ex) 
            {
                if (ex.Number == SqlExeptionCodes.UQ)
                {
                    if (ex.Message.Contains("UQ_UserName"))
                    {
                        throw new Exception("This User name is already taken, please Choose anthor one");
                    }
                }
                throw new Exception("Update error: " + ex.Message);
            }

        }
    }
}

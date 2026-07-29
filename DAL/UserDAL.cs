using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class UserDAL : DBServices
    {
        public UserDAL(IConfiguration config) : base(config) { }

       
        public int Register(User u)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_Register", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", u.Username);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
                    cmd.Parameters.AddWithValue("@FullName", (object)u.FullName ?? DBNull.Value);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }

        }

        public User GetByUsername(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetByUsername", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        user = new User
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            PasswordHash = reader["PasswordHash"] == DBNull.Value ? null : reader["PasswordHash"].ToString(),
                            FullName = reader["FullName"] as string,
                            Role = reader["Role"].ToString(),
                            IsLocked = (bool)reader["IsLocked"],
                            CanShare = (bool)reader["CanShare"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            LastLoginAt = reader["LastLoginAt"] == DBNull.Value ? null : (DateTime?)reader["LastLoginAt"]
                        };
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public User GetById(int userId)
        {
            User user = null;
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        user = new User
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            PasswordHash = reader["PasswordHash"] == DBNull.Value ? null : reader["PasswordHash"].ToString(),
                            FullName = reader["FullName"] as string,
                            Role = reader["Role"].ToString(),
                            IsLocked = (bool)reader["IsLocked"],
                            CanShare = (bool)reader["CanShare"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            LastLoginAt = reader["LastLoginAt"] == DBNull.Value ? null : (DateTime?)reader["LastLoginAt"]
                        };
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }


        }

        public List<User> GetAll()
        {
            List<User> list = new List<User>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        list.Add(new User
                        {
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            FullName = reader["FullName"] as string,
                            Role = reader["Role"].ToString(),
                            IsLocked = (bool)reader["IsLocked"],
                            CanShare = (bool)reader["CanShare"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            LastLoginAt = reader["LastLoginAt"] == DBNull.Value ? null : (DateTime?)reader["LastLoginAt"]
                        });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }


        }



        public int UpdateProfile(int userId, string email, string fullName)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_UpdateProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FullName", (object)fullName ?? DBNull.Value);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }


        public void UpdatePassword(int userId, string newPasswordHash)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_UpdatePassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public int UpdateLastLogin(int userId)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_UpdateLastLogin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public int SetLocked(int userId, bool isLocked)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_SetLocked", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@IsLocked", isLocked);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public int SetCanShare(int userId, bool canShare)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_SetCanShare", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CanShare", canShare);

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }

        }


    }
}
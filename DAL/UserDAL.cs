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
            using (SqlConnection con = new SqlConnection(_connectionString))
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

        public User GetByUsername(string username)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetByUsername", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    user = MapReaderToUser(reader);
            }
            return user;
        }

        public User GetById(int userId)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    user = MapReaderToUser(reader);
            }
            return user;
        }

        public List<User> GetAll()
        {
            List<User> list = new List<User>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapReaderToUserSummary(reader));
            }
            return list;
        }

        private User MapReaderToUserSummary(SqlDataReader reader)
        {
            return new User
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
            };
        }

        public int UpdateProfile(int userId, string email, string fullName)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
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


        public void UpdatePassword(int userId, string newPasswordHash)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_UpdatePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int UpdateLastLogin(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_UpdateLastLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int SetLocked(int userId, bool isLocked)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_SetLocked", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@IsLocked", isLocked);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int SetCanShare(int userId, bool canShare)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Users_FP_RM_SetCanShare", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CanShare", canShare);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private User MapReaderToUser(SqlDataReader reader)
        {
            return new User
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
    }
}
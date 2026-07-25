using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class LoginHistoryDAL : DBServices
    {
        public LoginHistoryDAL(IConfiguration config) : base(config) { }

        public void RecordLogin(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginHistory_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<LoginHistoryEntry> GetForRange(DateTime fromUtc, DateTime toUtc)
        {
            List<LoginHistoryEntry> list = new List<LoginHistoryEntry>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginHistory_FP_RM_GetForRange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromUtc", fromUtc);
                cmd.Parameters.AddWithValue("@ToUtc", toUtc);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new LoginHistoryEntry { LogId = (int)reader["LogId"], UserId = (int)reader["UserId"], Username = reader["Username"].ToString(), LoginAt = (DateTime)reader["LoginAt"] });
            }
            return list;
        }
    }
}
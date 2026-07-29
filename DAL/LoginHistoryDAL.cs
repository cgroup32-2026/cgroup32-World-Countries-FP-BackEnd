using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class LoginHistoryDAL : DBServices
    {
        public LoginHistoryDAL(IConfiguration config) : base(config) { }

        public void Insert(int userId)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginHistory_FP_RM_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }

        }

        public List<LoginHistoryEntry> Get(DateTime fromUtc, DateTime toUtc)
        {
            List<LoginHistoryEntry> list = new List<LoginHistoryEntry>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_LoginHistory_FP_RM_GetForRange", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromUtc", fromUtc);
                    cmd.Parameters.AddWithValue("@ToUtc", toUtc);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        list.Add(new LoginHistoryEntry
                        {
                            LogId = (int)reader["LogId"],
                            UserId = (int)reader["UserId"],
                            Username = reader["Username"].ToString(),
                            LoginAt = (DateTime)reader["LoginAt"]
                        });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }
    }
}
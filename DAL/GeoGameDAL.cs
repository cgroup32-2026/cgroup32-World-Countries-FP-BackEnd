using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class GeoGameDAL : DBServices
    {
        public GeoGameDAL(IConfiguration config) : base(config) { }

        public List<GeoGameMode> GetAllModes()
        {
            List<GeoGameMode> list = new List<GeoGameMode>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GeoGameModes_FP_RM_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new GeoGameMode { ModeId = (int)reader["ModeId"], ModeCode = reader["ModeCode"].ToString(), Label = reader["Label"].ToString() });
            }
            return list;
        }

        public void InsertAttempt(int userId, string modeCode, int score, int rounds)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GeoGameAttempts_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ModeCode", modeCode);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.Parameters.AddWithValue("@Rounds", rounds);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<GeoGameLeaderboardEntry> GetLeaderboard(string modeCode, int top = 10)
        {
            List<GeoGameLeaderboardEntry> list = new List<GeoGameLeaderboardEntry>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GeoGameAttempts_FP_RM_GetLeaderboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModeCode", modeCode);
                cmd.Parameters.AddWithValue("@Top", top);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new GeoGameLeaderboardEntry { Username = reader["Username"].ToString(), Score = (int)reader["Score"], Rounds = (int)reader["Rounds"], PlayedAt = (DateTime)reader["PlayedAt"] });
            }
            return list;
        }

        public List<GeoGameTotalLeaderboardEntry> GetTotalLeaderboard(int top = 10)
        {
            List<GeoGameTotalLeaderboardEntry> list = new List<GeoGameTotalLeaderboardEntry>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GeoGameAttempts_FP_RM_GetTotalLeaderboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Top", top);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new GeoGameTotalLeaderboardEntry { Username = reader["Username"].ToString(), TotalScore = (int)reader["TotalScore"] });
            }
            return list;
        }

        public List<GeoGameAttempt> GetMyAttempts(int userId)
        {
            List<GeoGameAttempt> list = new List<GeoGameAttempt>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GeoGameAttempts_FP_RM_GetMine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new GeoGameAttempt { AttemptId = (int)reader["AttemptId"], ModeCode = reader["ModeCode"].ToString(), Label = reader["Label"].ToString(), Score = (int)reader["Score"], Rounds = (int)reader["Rounds"], PlayedAt = (DateTime)reader["PlayedAt"] });
            }
            return list;
        }
    }
}
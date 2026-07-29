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
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GeoGameModes_FP_RM_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        list.Add(new GeoGameMode
                        {
                            ModeId = (int)reader["ModeId"],
                            ModeCode = reader["ModeCode"].ToString(),
                            Label = reader["Label"].ToString()
                        });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public void InsertAttempt(int userId, string modeCode, int score, int rounds)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }
        }

        public List<GeoGameLeaderboardEntry> GetLeaderboard(string modeCode, int top = 10)
        {
            List<GeoGameLeaderboardEntry> list = new List<GeoGameLeaderboardEntry>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public List<GeoGameTotalLeaderboardEntry> GetTotalLeaderboard(int top = 10)
        {
            List<GeoGameTotalLeaderboardEntry> list = new List<GeoGameTotalLeaderboardEntry>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }


        }

        public List<GeoGameAttempt> GetMyAttempts(int userId)
        {

            List<GeoGameAttempt> list = new List<GeoGameAttempt>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }


        }
    }
}
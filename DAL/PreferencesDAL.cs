using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class PreferencesDAL : DBServices
    {
        public PreferencesDAL(IConfiguration config) : base(config) { }

        public List<Continent> GetContinentsForUser(int userId)
        {
            List<Continent> list = new List<Continent>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserPreferredContinents_FP_RM_GetForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new Continent { ContinentId = (int)reader["ContinentId"], Name = reader["Name"].ToString() });
            }
            return list;
        }

        public void ClearContinentsForUser(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserPreferredContinents_FP_RM_ClearForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddContinentForUser(int userId, int continentId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserPreferredContinents_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ContinentId", continentId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<UserLanguagePreference> GetLanguagesForUser(int userId)
        {
            List<UserLanguagePreference> list = new List<UserLanguagePreference>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserLanguages_FP_RM_GetForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new UserLanguagePreference
                    {
                        LanguageId = (int)reader["LanguageId"],
                        Code = reader["Code"].ToString(),
                        Name = reader["Name"].ToString(),
                        Level = reader["Level"].ToString()
                    });
            }
            return list;
        }

        public void ClearLanguagesForUser(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserLanguages_FP_RM_ClearForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddLanguageForUser(int userId, int languageId, string level)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserLanguages_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@LanguageId", languageId);
                cmd.Parameters.AddWithValue("@Level", level);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
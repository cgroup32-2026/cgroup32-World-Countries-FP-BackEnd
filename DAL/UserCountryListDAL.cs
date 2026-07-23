using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class UserCountryListDAL : DBServices
    {
        public UserCountryListDAL(IConfiguration config) : base(config) { }

        public List<UserCountryListEntry> GetForUser(int userId, string listType = null)
        {
            List<UserCountryListEntry> list = new List<UserCountryListEntry>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserCountryLists_FP_RM_GetForUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListType", (object)listType ?? DBNull.Value);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new UserCountryListEntry
                    {
                        ListEntryId = (int)reader["ListEntryId"],
                        CountryId = (int)reader["CountryId"],
                        NameCommon = reader["NameCommon"].ToString(),
                        FlagUrl = reader["FlagUrl"] as string,
                        Region = reader["Region"] as string,
                        ListType = reader["ListType"].ToString(),
                        AddedAt = (DateTime)reader["AddedAt"]
                    });
                }
            }
            return list;
        }

        public void Add(int userId, int countryId, string listType)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserCountryLists_FP_RM_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@ListType", listType);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Remove(int userId, int countryId, string listType)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UserCountryLists_FP_RM_Remove", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@ListType", listType);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
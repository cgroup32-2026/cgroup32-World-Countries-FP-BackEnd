using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class UserCountryListDAL : DBServices
    {
        public UserCountryListDAL(IConfiguration config) : base(config) { }

        public List<UserCountryListEntry> Get(int userId, string listType = null)
        {
            List<UserCountryListEntry> list = new List<UserCountryListEntry>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public void Add(int userId, int countryId, string listType)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
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

            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public void Remove(int userId, int countryId, string listType)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to delete data from the database.", ex);
            }

        }
    }
}
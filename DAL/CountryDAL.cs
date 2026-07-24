using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;
using System.Diagnostics.Metrics;

namespace CountriesProject.DAL
{
    public class CountryDAL : DBServices
    {
        public CountryDAL(IConfiguration config) : base(config) { }

        public List<Country> GetAll()
        {
            List<Country> list = new List<Country>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapReaderToCountry(reader));
            }
            return list;
        }

        public Country GetById(int countryId)
        {
            Country country = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    country = new Country
                    {
                        CountryId = (int)reader["CountryId"],
                        CcaCode3 = reader["CcaCode3"].ToString(),
                        NameCommon = reader["NameCommon"].ToString(),
                        NameOfficial = reader["NameOfficial"] as string,
                        Region = reader["Region"] as string,
                        Subregion = reader["Subregion"] as string,
                        Capital = reader["Capital"] as string,
                        Population = reader["Population"] as long?,
                        AreaKm2 = reader["AreaKm2"] as double?,
                        FlagUrl = reader["FlagUrl"] as string,
                        MapUrl = reader["MapUrl"] as string,
                        Latitude = reader["Latitude"] as double?,
                        Longitude = reader["Longitude"] as double?,
                        CurrencyCodes = ParseCsv(reader["CurrencyCodes"]),
                        LanguageCodes = ParseCsv(reader["LanguageCodes"])
                    };
                }
            }
            return country;

        }

        public Country GetByCcaCode3(string ccaCode3)
        {
            Country country = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_GetByCcaCode3", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CcaCode3", ccaCode3);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    country = MapReaderToCountry(reader);
            }
            return country;
        }

        public int Insert(Country c)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CcaCode3", c.CcaCode3);
                cmd.Parameters.AddWithValue("@NameCommon", c.NameCommon);
                cmd.Parameters.AddWithValue("@NameOfficial", (object)c.NameOfficial ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Region", (object)c.Region ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Subregion", (object)c.Subregion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Capital", (object)c.Capital ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Population", (object)c.Population ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AreaKm2", (object)c.AreaKm2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FlagUrl", (object)c.FlagUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MapUrl", (object)c.MapUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Latitude", (object)c.Latitude ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", (object)c.Longitude ?? DBNull.Value);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int Update(Country c)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", c.CountryId);
                cmd.Parameters.AddWithValue("@NameCommon", c.NameCommon);
                cmd.Parameters.AddWithValue("@NameOfficial", (object)c.NameOfficial ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Region", (object)c.Region ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Subregion", (object)c.Subregion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Capital", (object)c.Capital ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Population", (object)c.Population ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AreaKm2", (object)c.AreaKm2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FlagUrl", (object)c.FlagUrl ?? DBNull.Value);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(int countryId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public void ClearCurrencyLinks(int countryId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CountryCurrencies_FP_RM_ClearForCountry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddCurrencyLink(int countryId, string currencyCode)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CountryCurrencies_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ClearLanguageLinks(int countryId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CountryLanguages_FP_RM_ClearForCountry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddLanguageLink(int countryId, int languageId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CountryLanguages_FP_RM_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@LanguageId", languageId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Country MapReaderToCountry(SqlDataReader reader)
        {
            return new Country
            {
                CountryId = (int)reader["CountryId"],
                CcaCode3 = reader["CcaCode3"].ToString(),
                NameCommon = reader["NameCommon"].ToString(),
                NameOfficial = reader["NameOfficial"] as string,
                Region = reader["Region"] as string,
                Subregion = reader["Subregion"] as string,
                Capital = reader["Capital"] as string,
                Population = reader["Population"] as long?,
                AreaKm2 = reader["AreaKm2"] as double?,
                FlagUrl = reader["FlagUrl"] as string,
                MapUrl = reader["MapUrl"] as string,
                Latitude = reader["Latitude"] as double?,
                Longitude = reader["Longitude"] as double?
            };
        }


        public List<CountryWithDetails> GetAllWithDetails()
        {
            List<CountryWithDetails> list = new List<CountryWithDetails>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Countries_FP_RM_GetAllWithDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new CountryWithDetails
                    {
                        CountryId = (int)reader["CountryId"],
                        CcaCode3 = reader["CcaCode3"].ToString(),
                        NameCommon = reader["NameCommon"].ToString(),
                        NameOfficial = reader["NameOfficial"] as string,
                        Region = reader["Region"] as string,
                        Subregion = reader["Subregion"] as string,
                        Capital = reader["Capital"] as string,
                        Population = reader["Population"] as long?,
                        AreaKm2 = reader["AreaKm2"] as double?,
                        FlagUrl = reader["FlagUrl"] as string,
                        MapUrl = reader["MapUrl"] as string,
                        Latitude = reader["Latitude"] as double?,
                        Longitude = reader["Longitude"] as double?,
                        CurrencyCodes = ParseCsv(reader["CurrencyCodes"]),
                        LanguageCodes = ParseCsv(reader["LanguageCodes"])
                    });
                }
            }
            return list;
        }

        private List<string> ParseCsv(object value)
        {
            if (value == DBNull.Value || value == null) return new List<string>();
            return value.ToString().Split(',').ToList();
        }
    }
}
using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class GeoGameLandmarkDAL : DBServices
    {
        public GeoGameLandmarkDAL(IConfiguration config) : base(config) { }

        public void DeleteAll()
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GeoGameLandmarks_FP_RM_DeleteAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to delete data from the database.", ex);
            }

        }

        public void Insert(int countryId, string title, string imageUrl, double lat, double lng)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GeoGameLandmarks_FP_RM_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryId", countryId);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    cmd.Parameters.AddWithValue("@Latitude", lat);
                    cmd.Parameters.AddWithValue("@Longitude", lng);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public List<GeoGameLandmark> GetAll()
        {
            List<GeoGameLandmark> list = new List<GeoGameLandmark>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_GeoGameLandmarks_FP_RM_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new GeoGameLandmark
                        {
                            LandmarkId = (int)reader["LandmarkId"],
                            CountryId = (int)reader["CountryId"],
                            CountryName = reader["CountryName"].ToString(),
                            Region = reader["Region"] as string,
                            Title = reader["Title"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            Latitude = (double)reader["Latitude"],
                            Longitude = (double)reader["Longitude"],
                            AreaKm2 = reader["AreaKm2"] as double?
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
    }
}
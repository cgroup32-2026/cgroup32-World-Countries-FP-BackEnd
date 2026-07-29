using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class ShareDAL : DBServices
    {
        public ShareDAL(IConfiguration config) : base(config) { }

        //used 5 times so i made a mapper for it
        private Share MapToShareObject(SqlDataReader reader)
        {
            return new Share
            {
                ShareId = (int)reader["ShareId"],
                UserId = (int)reader["UserId"],
                Username = reader["Username"].ToString(),
                CountryId = (int)reader["CountryId"],
                CountryName = reader["CountryName"].ToString(),
                Content = reader["Content"].ToString(),
                Rating = Convert.ToInt32(reader["Rating"]),
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTime?)reader["UpdatedAt"]
            };
        }

        public List<Share> GetAll()
        {
            List<Share> list = new List<Share>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToShareObject(reader));
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public List<Share> GetByCountry(int countryId)
        {
            List<Share> list = new List<Share>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_GetByCountry", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryId", countryId);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToShareObject(reader));
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }


        public List<Share> GetByUser(int userId)
        {
            List<Share> list = new List<Share>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_GetByUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapToShareObject(reader));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public Share GetById(int shareId)
        {
            Share share = null;
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_GetById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShareId", shareId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read()) share = MapToShareObject(reader);
                }
                return share;
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }

        public int Insert(int userId, int countryId, string content, int rating)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CountryId", countryId);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public void Update(int shareId, string content, int rating)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShareId", shareId);
                    cmd.Parameters.AddWithValue("@Content", content);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }


        }

        public void Delete(int shareId)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShareId", shareId);
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
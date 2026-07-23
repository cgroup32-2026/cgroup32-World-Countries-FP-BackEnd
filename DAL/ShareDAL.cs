using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class ShareDAL : DBServices
    {
        public ShareDAL(IConfiguration config) : base(config) { }

    

        public List<Share> GetAll() => RunListQuery("sp_Shares_FP_RM_GetAll", null);

        public List<Share> GetByCountry(int countryId) =>
            RunListQuery("sp_Shares_FP_RM_GetByCountry", cmd => cmd.Parameters.AddWithValue("@CountryId", countryId));

        public List<Share> GetByUser(int userId) =>
            RunListQuery("sp_Shares_FP_RM_GetByUser", cmd => cmd.Parameters.AddWithValue("@UserId", userId));

        public Share GetById(int shareId)
        {
            Share share = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShareId", shareId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) share = MapReaderToShare(reader);
            }
            return share;
        }

        public int Insert(int userId, int countryId, string content, int rating)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
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

        public void Update(int shareId, string content, int rating)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
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

        public void Delete(int shareId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Shares_FP_RM_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShareId", shareId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //  private helper, the 3 "get many shares" methods above are the same
        // only difference which params they use, so this avoids repeating the read loop 3 times.
        private List<Share> RunListQuery(string spName, Action<SqlCommand> addParams)
        {
            List<Share> list = new List<Share>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(spName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                addParams?.Invoke(cmd);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapReaderToShare(reader));
            }
            return list;
        }

        private Share MapReaderToShare(SqlDataReader reader)
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
    }
}
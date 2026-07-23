using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class ContinentDAL : DBServices
    {
        public ContinentDAL(IConfiguration config) : base(config) { }

        public List<Continent> GetAll()
        {
            List<Continent> list = new List<Continent>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Continents_FP_RM_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new Continent { ContinentId = (int)reader["ContinentId"], Name = reader["Name"].ToString() });
            }
            return list;
        }
    }
}
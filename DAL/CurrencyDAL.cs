using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class CurrencyDAL : DBServices
    {
        public CurrencyDAL(IConfiguration config) : base(config) { }

        public void InsertIfNotExists(string code, string name, string symbol)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Currencies_FP_RM_InsertIfNotExists", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CurrencyCode", code);
                cmd.Parameters.AddWithValue("@CurrencyName", name);
                cmd.Parameters.AddWithValue("@Symbol", (object)symbol ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
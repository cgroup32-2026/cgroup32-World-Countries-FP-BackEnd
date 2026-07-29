using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class CurrencyDAL : DBServices
    {
        public CurrencyDAL(IConfiguration config) : base(config) { }
        
        public void Insert(string code, string name, string symbol)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
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
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }

        }
    }
}
using System.Data;
using Microsoft.Data.SqlClient;
using CountriesProject.DAL.Models;

namespace CountriesProject.DAL
{
    public class AdminDAL : DBServices
    {
        public AdminDAL(IConfiguration config) : base(config) { }

        public AdminUsageStats GetUsageStats()
        {
            AdminUsageStats stats = null;
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Admin_FP_RM_GetUsageStats", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())

                            stats = new AdminUsageStats
                            {
                                TotalUsers = (int)reader["TotalUsers"],
                                LoginsToday = (int)reader["LoginsToday"],
                                TotalCountriesImported = (int)reader["TotalCountriesImported"],
                                TotalSavedCountries = (int)reader["TotalSavedCountries"],
                                TotalShares = (int)reader["TotalShares"],
                                TotalQuizAttempts = (int)reader["TotalQuizAttempts"]
                            };
                    }      
                }
                return stats;
            }
            catch (Exception ex)
            {

                throw new Exception("Was not able to fetch data from the database.", ex);
            }

        }
    }
}
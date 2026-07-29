using System.Data;
using CountriesProject.DAL.Models;
using Microsoft.Data.SqlClient;

namespace CountriesProject.DAL
{
    public class LanguageDAL : DBServices
    {
        public LanguageDAL(IConfiguration config) : base(config) { }

        public int Insert(string code, string name)
        {
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Languages_FP_RM_InsertIfNotExists", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    return (int)reader["LanguageId"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Was not able to add data to the database.", ex);
            }
        }

        public List<Language> GetAll()
        {
            List<Language> list = new List<Language>();
            try
            {
                using (SqlConnection con = GetDBSConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Languages_FP_RM_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        list.Add(new Language { LanguageId = (int)reader["LanguageId"], Code = reader["Code"].ToString(), Name = reader["Name"].ToString() });
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
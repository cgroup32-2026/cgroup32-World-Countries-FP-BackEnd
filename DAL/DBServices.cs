using Microsoft.Data.SqlClient;

namespace CountriesProject.DAL
{
    public class DBServices
    {
        protected readonly string _connectionString;

        public DBServices(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("myProjDB");
        }
    }
}
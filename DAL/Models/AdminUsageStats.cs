namespace CountriesProject.DAL.Models
{
    public class AdminUsageStats
    {
        public int TotalUsers { get; set; }
        public int LoginsToday { get; set; }
        public int TotalCountriesImported { get; set; }
        public int TotalSavedCountries { get; set; }
        public int TotalShares { get; set; }
        public int TotalQuizAttempts { get; set; }
    }
}
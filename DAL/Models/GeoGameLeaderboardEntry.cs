namespace CountriesProject.DAL.Models
{
    public class GeoGameLeaderboardEntry {
        public string Username { get; set; }
        public int Score { get; set; } 
        public int Rounds { get; set; } 
        public DateTime PlayedAt { get; set; } 
    }
}

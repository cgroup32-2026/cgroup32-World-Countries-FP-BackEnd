namespace CountriesProject.DAL.Models
{
    public class LeaderboardEntry
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public int TimeTakenSeconds { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
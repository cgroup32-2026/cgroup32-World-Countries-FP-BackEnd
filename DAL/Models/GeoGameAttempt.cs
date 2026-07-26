namespace CountriesProject.DAL.Models
{
    public class GeoGameAttempt { 
        public int AttemptId { get; set; } 
        public string ModeCode { get; set; } 
        public string Label { get; set; }
        public int Score { get; set; }
        public int Rounds { get; set; } 
        public DateTime PlayedAt { get; set; } 
    }
}

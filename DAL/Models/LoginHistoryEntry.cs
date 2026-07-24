namespace CountriesProject.DAL.Models
{
    public class LoginHistoryEntry { 
        public int LogId { get; set; } 
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime LoginAt { get; set; } 
    }
}

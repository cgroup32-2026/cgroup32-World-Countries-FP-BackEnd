namespace CountriesProject.DAL.Models
{
    public class Share
    {
        public int ShareId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
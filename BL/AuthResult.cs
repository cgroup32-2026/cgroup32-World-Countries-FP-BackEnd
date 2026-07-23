using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class AuthResult
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
namespace CountriesProject.DAL.Models
{
    public class CountryWithDetails : Country
    {
        public List<string> CurrencyCodes { get; set; }
        public List<string> LanguageCodes { get; set; }
    }
}
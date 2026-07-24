namespace CountriesProject.DAL.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CcaCode3 { get; set; }
        public string NameCommon { get; set; }
        public string NameOfficial { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string Capital { get; set; }
        public long? Population { get; set; }
        public double? AreaKm2 { get; set; }
        public string FlagUrl { get; set; }
        public string MapUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public List<string>? CurrencyCodes { get; set; }
        public List<string>? LanguageCodes { get; set; }
    }
}
namespace CountriesProject.BL
{
    public class CountrySearchParams
    {
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? LanguageCode { get; set; }
        public string? CurrencyCode { get; set; }
        public long? MinPopulation { get; set; }
        public long? MaxPopulation { get; set; }
        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
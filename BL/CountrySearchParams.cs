namespace CountriesProject.BL
{
    //helper class I can use to filter countires easie instead of using  complicated stored procedures.
    //this represents what would the user choose or type in the site. each field is also represneted
    //by a field in the country tab in the site. 
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
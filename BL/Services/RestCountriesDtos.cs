using System.Text.Json.Serialization;
using CountriesProject.DAL.Models;


namespace CountriesProject.BL.Services
{
    public class CountryDevDto
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("alpha3Code")] public string Alpha3Code { get; set; }
        [JsonPropertyName("capital")] public string Capital { get; set; }
        [JsonPropertyName("region")] public string Region { get; set; }
        [JsonPropertyName("subregion")] public string Subregion { get; set; }
        [JsonPropertyName("population")] public long? Population { get; set; }
        [JsonPropertyName("area")] public double? Area { get; set; }
        [JsonPropertyName("latlng")] public List<double> Latlng { get; set; }
        [JsonPropertyName("flags")] public CountryDevFlags Flags { get; set; }
        [JsonPropertyName("maps")] public CountryDevMaps Maps { get; set; }
        [JsonPropertyName("currencies")] public List<CountryDevCurrency> Currencies { get; set; }
        [JsonPropertyName("languages")] public List<CountryDevLanguage> Languages { get; set; }
    }

    public class CountryDevFlags
    {
        [JsonPropertyName("png")] public string Png { get; set; }
    }

    public class CountryDevMaps
    {
        [JsonPropertyName("googleMaps")] public string GoogleMaps { get; set; }
    }

    public class CountryDevCurrency
    {
        [JsonPropertyName("code")] public string Code { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("symbol")] public string Symbol { get; set; }
    }

    public class CountryDevLanguage
    {
        [JsonPropertyName("iso639_1")] public string Iso6391 { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    
    public class CountryImportResult
    {
            public CountriesProject.DAL.Models.Country Country { get; set; }
            public List<CountryDevCurrency> Currencies { get; set; }
            public List<CountryDevLanguage> Languages { get; set; }
     }
    
}
using System.Net.Http.Json;
using System.Text.Json;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL.Services
{
    public class RestCountriesService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public RestCountriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CountryImportResult>> FetchAllCountriesAsync()
        {
            List<CountryDevDto> dtos = await _httpClient.GetFromJsonAsync<List<CountryDevDto>>(
                "countries?full=true", _jsonOptions);

            List<CountryImportResult> results = new List<CountryImportResult>();
            if (dtos == null) return results;

            foreach (var dto in dtos)
            {
                results.Add(new CountryImportResult
                {
                    Country = MapDtoToCountry(dto),
                    Currencies = dto.Currencies ?? new List<CountryDevCurrency>(),
                    Languages = dto.Languages ?? new List<CountryDevLanguage>()
                });
            }
            return results;
        }

        private Country MapDtoToCountry(CountryDevDto dto)
        {
            return new Country
            {
                CcaCode3 = dto.Alpha3Code,
                NameCommon = dto.Name,
                NameOfficial = null,
                Region = dto.Region,
                Subregion = dto.Subregion,
                Capital = dto.Capital,
                Population = dto.Population,
                AreaKm2 = dto.Area,
                FlagUrl = dto.Flags?.Png,
                MapUrl = dto.Maps?.GoogleMaps,
                Latitude = dto.Latlng != null && dto.Latlng.Count > 0 ? dto.Latlng[0] : (double?)null,
                Longitude = dto.Latlng != null && dto.Latlng.Count > 1 ? dto.Latlng[1] : (double?)null
            };
        }
    }
}
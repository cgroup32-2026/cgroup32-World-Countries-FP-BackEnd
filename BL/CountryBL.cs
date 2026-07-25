using CountriesProject.DAL;
using CountriesProject.DAL.Models;
using CountriesProject.BL.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace CountriesProject.BL
{
    public class CountryBL
    {
        private readonly CountryDAL _countryDAL;
        private readonly CurrencyDAL _currencyDAL;
        private readonly LanguageDAL _languageDAL;
        private readonly RestCountriesService _restCountriesService;
        private readonly LandmarksService _landmarksService;

        private static readonly Regex CcaCode3Regex = new Regex(@"^[A-Za-z]{3}$", RegexOptions.Compiled);


        public CountryBL(CountryDAL countryDAL, CurrencyDAL currencyDAL, LanguageDAL languageDAL, 
            RestCountriesService restCountriesService, LandmarksService landmarksService)
        {
            _countryDAL = countryDAL;
            _currencyDAL = currencyDAL;
            _languageDAL = languageDAL;
            _restCountriesService = restCountriesService;
            _landmarksService = landmarksService;
        }

        public List<Country> GetAll() => _countryDAL.GetAll();

        public Country GetById(int id) => _countryDAL.GetById(id);


        public Country Create(Country country)
        {
            if (string.IsNullOrWhiteSpace(country.CcaCode3) || !CcaCode3Regex.IsMatch(country.CcaCode3))
                throw new Exception("CcaCode3 must be exactly 3 letters (e.g. USA, FRA)");
            if (string.IsNullOrWhiteSpace(country.NameCommon))
                throw new Exception("NameCommon is required");

            int newId = _countryDAL.Insert(country);
            return _countryDAL.GetById(newId);
        }

        public void Update(Country country)
        {
            if (string.IsNullOrWhiteSpace(country.CcaCode3) || !CcaCode3Regex.IsMatch(country.CcaCode3))
                throw new Exception("CcaCode3 must be exactly 3 letters (e.g. USA, FRA)");
            if (string.IsNullOrWhiteSpace(country.NameCommon))
                throw new Exception("NameCommon is required");
            if (_countryDAL.GetById(country.CountryId) == null)
                throw new Exception("Country not found");
            _countryDAL.Update(country);
        }

     
        public void Delete(int id)
        {
            if (_countryDAL.GetById(id) == null)
                throw new Exception("Country not found");
            _countryDAL.Delete(id);
        }

        public async Task<int> ImportFromRestCountriesAsync()
        {
            List<CountryImportResult> fetched = await _restCountriesService.FetchAllCountriesAsync();
            int importedCount = 0;

            foreach (var result in fetched)
            {
                var country = result.Country;
                if (string.IsNullOrEmpty(country.CcaCode3)) continue;

                int countryId;
                Country existing = _countryDAL.GetByCcaCode3(country.CcaCode3);
                if (existing == null)
                {
                    countryId = _countryDAL.Insert(country);
                    importedCount++;
                }
                else
                {
                    countryId = existing.CountryId;
                    country.CountryId = countryId;
                    _countryDAL.Update(country);
                }

                _countryDAL.ClearCurrencyLinks(countryId);
                _countryDAL.ClearLanguageLinks(countryId);

                foreach (var currency in result.Currencies)
                {
                    if (string.IsNullOrEmpty(currency.Code)) continue;
                    _currencyDAL.InsertIfNotExists(currency.Code, currency.Name, currency.Symbol);
                    _countryDAL.AddCurrencyLink(countryId, currency.Code);
                }

                foreach (var language in result.Languages)
                {
                    if (string.IsNullOrEmpty(language.Iso6391)) continue;
                    int languageId = _languageDAL.GetOrCreate(language.Iso6391, language.Name);
                    _countryDAL.AddLanguageLink(countryId, languageId);
                }
            }
            return importedCount;
        }

        private List<string> ParseCsv(object value)
        {
            if (value == DBNull.Value || value == null) return new List<string>();
            return value.ToString().Split(',').ToList();
        }


        public List<CountryWithDetails> Search(CountrySearchParams p)
        {
            IEnumerable<CountryWithDetails> results = _countryDAL.GetAllWithDetails();

            if (!string.IsNullOrWhiteSpace(p.Name))
                results = results.Where(c => c.NameCommon != null &&
                    c.NameCommon.Contains(p.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(p.Region))
                results = results.Where(c => c.Region != null &&
                    c.Region.Equals(p.Region, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(p.LanguageCode))
                results = results.Where(c => c.LanguageCodes.Any(l => l.Equals(p.LanguageCode, StringComparison.OrdinalIgnoreCase)));

            if (!string.IsNullOrWhiteSpace(p.CurrencyCode))
                results = results.Where(c => c.CurrencyCodes.Any(cur => cur.Equals(p.CurrencyCode, StringComparison.OrdinalIgnoreCase)));

            if (p.MinPopulation.HasValue)
                results = results.Where(c => c.Population.HasValue && c.Population.Value >= p.MinPopulation.Value);

            if (p.MaxPopulation.HasValue)
                results = results.Where(c => c.Population.HasValue && c.Population.Value <= p.MaxPopulation.Value);

            if (p.MinArea.HasValue)
                results = results.Where(c => c.AreaKm2.HasValue && c.AreaKm2.Value >= p.MinArea.Value);

            if (p.MaxArea.HasValue)
                results = results.Where(c => c.AreaKm2.HasValue && c.AreaKm2.Value <= p.MaxArea.Value);

            results = p.SortBy?.ToLower() switch
            {
                "population" => p.SortDescending ? results.OrderByDescending(c => c.Population) : results.OrderBy(c => c.Population),
                "area" => p.SortDescending ? results.OrderByDescending(c => c.AreaKm2) : results.OrderBy(c => c.AreaKm2),
                "region" => p.SortDescending ? results.OrderByDescending(c => c.Region) : results.OrderBy(c => c.Region),
                _ => p.SortDescending ? results.OrderByDescending(c => c.NameCommon) : results.OrderBy(c => c.NameCommon),
            };

            return results.ToList();
        }

        public async Task<List<LandmarkResult>> GetLandmarks(int countryId)
        {
            Country country = _countryDAL.GetById(countryId);
            if (country == null) throw new Exception("Country not found");
            if (country.Latitude == null || country.Longitude == null) return new List<LandmarkResult>();
            return await _landmarksService.GetLandmarksNear(country.Latitude.Value, country.Longitude.Value);
        }
    }
}
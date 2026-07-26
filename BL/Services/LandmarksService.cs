using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CountriesProject.BL.Services
{
    public class LandmarksService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private static readonly string[] NonLandmarkKeywords = { "war", "battle", "attack", "revolution", "coup", "invasion", "campaign", "uprising", "massacre", "siege", "operation", "offensive" };
        private static readonly Regex YearInTitleOrExtract = new Regex(@"\(\d{4}\)|\b(19|20)\d{2}\b", RegexOptions.Compiled);

        public LandmarksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LandmarkResult>> GetLandmarksNear(double latitude, double longitude, string excludeTitle)
        {
            string url = $"w/api.php?action=query&generator=geosearch&ggscoord={latitude}|{longitude}&ggsradius=10000&ggslimit=15&prop=pageimages|extracts&exintro=true&explaintext=true&exchars=300&piprop=thumbnail&pithumbsize=400&format=json";

            var response = await _httpClient.GetFromJsonAsync<WikipediaQueryResponse>(url, _jsonOptions);
            var results = new List<LandmarkResult>();
            if (response?.Query?.Pages == null) return results;

            foreach (var page in response.Query.Pages.Values)
            {
                if (page.Thumbnail == null) continue;
                if (string.IsNullOrWhiteSpace(page.Extract) || page.Extract.Length < 80) continue; // filters out geography-stub articles
                if (!string.IsNullOrEmpty(excludeTitle) && page.Title.Contains(excludeTitle, StringComparison.OrdinalIgnoreCase)) continue; // filters out the country's own article
                if (NonLandmarkKeywords.Any(kw => page.Title.Contains(kw, StringComparison.OrdinalIgnoreCase))) continue;

                results.Add(new LandmarkResult
                {
                    Title = page.Title,
                    Description = page.Extract,
                    ImageUrl = page.Thumbnail.Source,
                    WikipediaUrl = $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(page.Title.Replace(" ", "_"))}"
                });

                if (results.Count >= 6) break; // stop once we have enough good ones
            }
            return results;
        }

        public async Task<(double lat, double lng)?> GeocodeCityName(string cityName)
        {
            string url = $"w/api.php?action=query&generator=search&gsrsearch={Uri.EscapeDataString(cityName)}&gsrlimit=1&prop=coordinates&format=json";
            var response = await _httpClient.GetFromJsonAsync<WikipediaCoordResponse>(url, _jsonOptions);
            var page = response?.Query?.Pages?.Values.FirstOrDefault();
            var coord = page?.Coordinates?.FirstOrDefault();
            return coord != null ? (coord.Lat, coord.Lon) : null;
        }

        public async Task<List<(string title, string imageUrl, double lat, double lng)>> GetGameQualityLandmarksNear(double latitude, double longitude, string excludeTitle)
        {
            string url = $"w/api.php?action=query&generator=geosearch&ggscoord={latitude}|{longitude}&ggsradius=10000&ggslimit=20&prop=pageimages|extracts|coordinates&exintro=true&explaintext=true&exchars=300&piprop=thumbnail&pithumbsize=400&format=json";

            var response = await _httpClient.GetFromJsonAsync<WikipediaQueryResponse>(url, _jsonOptions);
            var results = new List<(string, string, double, double)>();
            if (response?.Query?.Pages == null) return results;

            foreach (var page in response.Query.Pages.Values)
            {
                if (page.Thumbnail == null) continue;
                if (page.Coordinates == null || page.Coordinates.Count == 0) continue;
                if (string.IsNullOrWhiteSpace(page.Extract) || page.Extract.Length < 80) continue;
                if (!string.IsNullOrEmpty(excludeTitle) && page.Title.Contains(excludeTitle, StringComparison.OrdinalIgnoreCase)) continue;
                if (NonLandmarkKeywords.Any(kw => page.Title.Contains(kw, StringComparison.OrdinalIgnoreCase))) continue;
                if (YearInTitleOrExtract.IsMatch(page.Title)) continue; // game mode only — stricter than the detail-page filter

                var coord = page.Coordinates.First();
                results.Add((page.Title, page.Thumbnail.Source, coord.Lat, coord.Lon));

                if (results.Count >= 3) break; // cap per country — a large, deep pool matters more than an exhaustive one per place
            }
            return results;
        }
    }
}
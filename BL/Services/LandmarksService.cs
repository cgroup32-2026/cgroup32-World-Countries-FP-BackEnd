using System.Net.Http.Json;
using System.Text.Json;

namespace CountriesProject.BL.Services
{
    public class LandmarksService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public LandmarksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LandmarkResult>> GetLandmarksNear(double latitude, double longitude)
        {
            string url = $"w/api.php?action=query&generator=geosearch&ggscoord={latitude}|{longitude}&ggsradius=10000&ggslimit=6&prop=pageimages|extracts&exintro=true&explaintext=true&exchars=200&piprop=thumbnail&pithumbsize=400&format=json";

            var response = await _httpClient.GetFromJsonAsync<WikipediaQueryResponse>(url, _jsonOptions);
            var results = new List<LandmarkResult>();
            if (response?.Query?.Pages == null) return results;

            foreach (var page in response.Query.Pages.Values)
            {
                if (page.Thumbnail == null) continue; 
                results.Add(new LandmarkResult
                {
                    Title = page.Title,
                    Description = page.Extract,
                    ImageUrl = page.Thumbnail.Source,
                    WikipediaUrl = $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(page.Title.Replace(" ", "_"))}"
                });
            }
            return results;
        }
    }
}
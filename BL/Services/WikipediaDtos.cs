using System.Text.Json.Serialization;

namespace CountriesProject.BL.Services
{
    public class WikipediaQueryResponse
    {
        [JsonPropertyName("query")] public WikipediaQuery Query { get; set; }
    }

    public class WikipediaQuery
    {
        [JsonPropertyName("pages")] public Dictionary<string, WikipediaPage> Pages { get; set; }
    }

    public class WikipediaPage
    {
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("extract")] public string Extract { get; set; }
        [JsonPropertyName("thumbnail")] public WikipediaThumbnail Thumbnail { get; set; }
    }

    public class WikipediaThumbnail
    {
        [JsonPropertyName("source")] public string Source { get; set; }
    }

    public class LandmarkResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string WikipediaUrl { get; set; }
    }
}
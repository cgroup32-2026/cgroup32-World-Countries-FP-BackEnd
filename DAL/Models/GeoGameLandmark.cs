public class GeoGameLandmark
{
    public int LandmarkId { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public string Region { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? AreaKm2 { get; set; }
}
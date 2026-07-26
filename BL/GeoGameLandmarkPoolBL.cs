using CountriesProject.DAL;
using CountriesProject.DAL.Models;
using CountriesProject.BL.Services;

namespace CountriesProject.BL
{
    public class GeoGameLandmarkPoolBL
    {
        private readonly CountryDAL _countryDAL;
        private readonly LandmarksService _landmarksService;
        private readonly GeoGameLandmarkDAL _landmarkDAL;

        public GeoGameLandmarkPoolBL(CountryDAL countryDAL, LandmarksService landmarksService, GeoGameLandmarkDAL landmarkDAL)
        {
            _countryDAL = countryDAL; _landmarksService = landmarksService; _landmarkDAL = landmarkDAL;
        }

        public async Task<int> BuildPool()
        {
            _landmarkDAL.DeleteAll();
            var countries = _countryDAL.GetAll();
            int inserted = 0;
            int failed = 0;

            foreach (var country in countries)
            {
                if (country.Latitude == null || country.Longitude == null) continue;

                try
                {
                    double lat = country.Latitude.Value, lng = country.Longitude.Value;
                    if (!string.IsNullOrWhiteSpace(country.Capital))
                    {
                        await Task.Delay(1000); // before the geocode call
                        var capitalCoords = await _landmarksService.GeocodeCityName(country.Capital);
                        if (capitalCoords != null) { lat = capitalCoords.Value.lat; lng = capitalCoords.Value.lng; }
                    }

                    await Task.Delay(1000); // before the landmark search call

                    var landmarks = await _landmarksService.GetGameQualityLandmarksNear(lat, lng, country.NameCommon);
                    foreach (var (title, imageUrl, landmarkLat, landmarkLng) in landmarks)
                    {
                        _landmarkDAL.Insert(country.CountryId, title, imageUrl, landmarkLat, landmarkLng);
                        inserted++;
                    }
                }
                catch (Exception)
                {
                    failed++;
                    await Task.Delay(3000); // extra cooldown specifically after a failure, in case we're being throttled harder than usual
                }
            }

            Console.WriteLine($"Landmark pool build complete: {inserted} landmarks inserted, {failed} countries failed/skipped.");
            return inserted;
        }
    }
}
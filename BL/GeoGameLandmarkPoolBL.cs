using CountriesProject.DAL;
using CountriesProject.DAL.Models;
using CountriesProject.BL.Services;

namespace CountriesProject.BL
{
    //this class is mainly for building the pool using an admin button in the website. its meant to be used once only.
    //(was already used)
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

        public async Task<int> BuildPoolForMissingCountries()
        {
            var countries = _countryDAL.GetAll();
            var alreadyCovered = _landmarkDAL.GetAll().Select(l => l.CountryId).ToHashSet();
            var missing = countries.Where(c => !alreadyCovered.Contains(c.CountryId)).ToList();

            int inserted = 0, failed = 0, consecutiveFailures = 0;

            foreach (var country in missing)
            {
                if (country.Latitude == null || country.Longitude == null) continue;

                try
                {
                    double lat = country.Latitude.Value, lng = country.Longitude.Value;
                    if (!string.IsNullOrWhiteSpace(country.Capital))
                    {
                        await Task.Delay(1500);
                        var capitalCoords = await _landmarksService.GeocodeCityName(country.Capital);
                        if (capitalCoords != null) { lat = capitalCoords.Value.lat; lng = capitalCoords.Value.lng; }
                    }

                    await Task.Delay(1500);
                    var landmarks = await _landmarksService.GetGameQualityLandmarksNear(lat, lng, country.NameCommon);
                    foreach (var (title, imageUrl, landmarkLat, landmarkLng) in landmarks)
                    {
                        _landmarkDAL.Insert(country.CountryId, title, imageUrl, landmarkLat, landmarkLng);
                        inserted++;
                    }
                    consecutiveFailures = 0;
                }
                catch (Exception)
                {
                    failed++;
                    consecutiveFailures++;
                    int backoffSeconds = Math.Min(120, 10 * (int)Math.Pow(2, consecutiveFailures - 1));
                    await Task.Delay(TimeSpan.FromSeconds(backoffSeconds));
                }
            }

            Console.WriteLine($"Gap-fill complete: {inserted} new landmarks, {failed} still failing.");
            return inserted;
        }
    }
}
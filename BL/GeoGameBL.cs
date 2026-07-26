using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class GeoGameBL
    {
        private readonly GeoGameDAL _geoGameDAL;

        public GeoGameBL(GeoGameDAL geoGameDAL)
        {
            _geoGameDAL = geoGameDAL;
        }

        public List<GeoGameMode> GetAllModes() => _geoGameDAL.GetAllModes();

        public void SubmitAttempt(int userId, string modeCode, int score, int rounds)
        {
            if (score < 0) throw new Exception("Score cannot be negative");
            if (rounds < 1) throw new Exception("Rounds must be at least 1");
            _geoGameDAL.InsertAttempt(userId, modeCode, score, rounds);
        }

        public List<GeoGameLeaderboardEntry> GetLeaderboard(string modeCode) => _geoGameDAL.GetLeaderboard(modeCode);
        public List<GeoGameTotalLeaderboardEntry> GetTotalLeaderboard() => _geoGameDAL.GetTotalLeaderboard();
        public List<GeoGameAttempt> GetMyAttempts(int userId) => _geoGameDAL.GetMyAttempts(userId);
    }
}
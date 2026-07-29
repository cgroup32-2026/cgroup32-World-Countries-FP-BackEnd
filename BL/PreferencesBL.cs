using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class PreferencesBL
    {
        private static readonly string[] ValidLevels = { "Beginner", "Intermediate", "Advanced", "Native" };

        private readonly PreferencesDAL _preferencesDAL;
        private readonly ContinentDAL _continentDAL;
        private readonly LanguageDAL _languageDAL;

        public PreferencesBL(PreferencesDAL preferencesDAL, ContinentDAL continentDAL, LanguageDAL languageDAL)
        {
            _preferencesDAL = preferencesDAL;
            _continentDAL = continentDAL;
            _languageDAL = languageDAL;
        }

        public List<Continent> GetAllContinents() => _continentDAL.GetAll();
        public List<Language> GetAllLanguages() => _languageDAL.GetAll();

        public List<Continent> GetUserContinents(int userId) => _preferencesDAL.GetContinents(userId);
        public List<UserLanguagePreference> GetUserLanguages(int userId) => _preferencesDAL.GetLanguages(userId);

        public void SetUserContinents(int userId, List<int> continentIds)
        {
            _preferencesDAL.ClearContinents(userId);
            foreach (int continentId in continentIds.Distinct())
                _preferencesDAL.AddContinent(userId, continentId);
        }

        public void SetUserLanguages(int userId, List<(int LanguageId, string Level)> languages)
        {
            foreach (var lang in languages)
            {
                if (!ValidLevels.Contains(lang.Level, StringComparer.OrdinalIgnoreCase))
                    throw new Exception($"Invalid level '{lang.Level}'. Must be one of: {string.Join(", ", ValidLevels)}");
            }

            _preferencesDAL.ClearLanguages(userId);
            foreach (var lang in languages)
                _preferencesDAL.AddLanguage(userId, lang.LanguageId, lang.Level);
        }
    }
}
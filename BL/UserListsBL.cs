using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class UserListsBL
    {
        private static readonly string[] ValidListTypes = { "Visited", "WantToVisit" };

        private readonly UserCountryListDAL _listDAL;
        private readonly CountryDAL _countryDAL;

        public UserListsBL(UserCountryListDAL listDAL, CountryDAL countryDAL)
        {
            _listDAL = listDAL;
            _countryDAL = countryDAL;
        }

        private void ValidateListType(string listType)
        {
            if (!ValidListTypes.Contains(listType))
                throw new Exception($"wrong list type '{listType}'. must be one of : {string.Join(", ", ValidListTypes)}");
        }

        public List<UserCountryListEntry> GetList(int userId, string listType = null)
        {
            if (listType != null) ValidateListType(listType);
            return _listDAL.GetForUser(userId, listType);
        }

        public void AddToList(int userId, int countryId, string listType)
        {
            ValidateListType(listType);
            if (_countryDAL.GetById(countryId) == null)
                throw new Exception("country not found");
            _listDAL.Add(userId, countryId, listType);
        }

        public void RemoveFromList(int userId, int countryId, string listType)
        {

            ValidateListType(listType);
            _listDAL.Remove(userId, countryId, listType);
        }

        public void MoveToList(int userId, int countryId, string fromListType, string toListType)
        {
            ValidateListType(fromListType);
            ValidateListType(toListType);

            var currentEntries = _listDAL.GetForUser(userId, fromListType);
            bool existsInSource = currentEntries.Any(e => e.CountryId == countryId);
            if (!existsInSource)
                throw new Exception($"country is not currently in your '{fromListType}' list, nothing to move.");


            _listDAL.Remove(userId, countryId, fromListType);
            _listDAL.Add(userId, countryId, toListType);
        }
    }
}
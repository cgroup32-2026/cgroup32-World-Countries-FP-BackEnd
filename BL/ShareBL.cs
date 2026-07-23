using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class ShareBL
    {
        private readonly ShareDAL _shareDAL;
        private readonly CountryDAL _countryDAL;
        private readonly UserDAL _userDAL;

        public ShareBL(ShareDAL shareDAL, CountryDAL countryDAL, UserDAL userDAL)
        {
            _shareDAL = shareDAL;
            _countryDAL = countryDAL;
            _userDAL = userDAL;
        }

        public List<Share> GetAll() => _shareDAL.GetAll();
        public List<Share> GetByCountry(int countryId) => _shareDAL.GetByCountry(countryId);
        public List<Share> GetByUser(int userId) => _shareDAL.GetByUser(userId);

        public Share Create(int userId, int countryId, string content, int rating)
        {
            User user = _userDAL.GetById(userId);
            if (user == null) throw new Exception("user not not found");
            if (!user.CanShare) throw new Exception("your account is not permitted to share content");
            if (string.IsNullOrWhiteSpace(content)) throw new Exception("share content can not be empty");
            if (content.Length > 1000) throw new Exception("share content can not exceed 1000 characters");
            if (rating < 1 || rating > 5) throw new Exception("rating must be between 1 and 5");
            if (_countryDAL.GetById(countryId) == null) throw new Exception("country is not found");

            int newId = _shareDAL.Insert(userId, countryId, content.Trim(), rating);
            return _shareDAL.GetById(newId);
        }

        public void Update(int shareId, int requestingUserId, string content, int rating)
        {
            Share existing = _shareDAL.GetById(shareId);
            if (existing == null) throw new Exception("share not found");
            if (existing.UserId != requestingUserId) throw new Exception("you can only edit your own shares");
            if (string.IsNullOrWhiteSpace(content)) throw new Exception("share content can not be empty");
            if (content.Length > 1000) throw new Exception("share content cannot exceed 1000 characters");
            if (rating < 1 || rating > 5) throw new Exception("rating must be between 1 and 5");

            _shareDAL.Update(shareId, content.Trim(), rating);
        }
        public void Delete(int shareId, int requestingUserId)
        {
            Share existing = _shareDAL.GetById(shareId);
            if (existing == null) throw new Exception("share not found");
            if (existing.UserId != requestingUserId) throw new Exception("you can only delete your own shares");

            _shareDAL.Delete(shareId);
        }
    }
}
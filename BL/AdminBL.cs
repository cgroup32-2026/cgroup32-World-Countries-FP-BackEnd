using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class AdminBL
    {
        private readonly UserDAL _userDAL;
        private readonly AdminDAL _adminDAL;

        public AdminBL(UserDAL userDAL, AdminDAL adminDAL)
        {
            _userDAL = userDAL;
            _adminDAL = adminDAL;
        }

        public List<User> GetAllUsers() => _userDAL.GetAll();

        public void SetUserLocked(int userId, bool isLocked) => _userDAL.SetLocked(userId, isLocked);

        public void SetUserCanShare(int userId, bool canShare) => _userDAL.SetCanShare(userId, canShare);

        public AdminUsageStats GetUsageStats() => _adminDAL.GetUsageStats();
    }
}
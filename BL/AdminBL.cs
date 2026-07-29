using CountriesProject.DAL;
using CountriesProject.DAL.Models;

namespace CountriesProject.BL
{
    public class AdminBL
    {
        private readonly UserDAL _userDAL;
        private readonly AdminDAL _adminDAL;
        private readonly LoginHistoryDAL _loginHistoryDAL;


        public AdminBL(UserDAL userDAL, AdminDAL adminDAL, LoginHistoryDAL loginHistoryDAL)
        {
            _userDAL = userDAL;
            _adminDAL = adminDAL;
            _loginHistoryDAL = loginHistoryDAL;
        }

        public List<User> GetAllUsers() => _userDAL.GetAll();

        public void SetUserLocked(int targetUserId, bool isLocked, int requestingAdminId)
        {
            if (isLocked && targetUserId == requestingAdminId)
                throw new Exception("you cannot lock your own account");
            _userDAL.SetLocked(targetUserId, isLocked);
        }
        public void SetUserCanShare(int userId, bool canShare) => _userDAL.SetCanShare(userId, canShare);

        public AdminUsageStats GetUsageStats() => _adminDAL.GetUsageStats();

        public List<LoginHistoryEntry> GetLoginHistory(DateTime fromUtc, DateTime toUtc) => _loginHistoryDAL.Get(fromUtc, toUtc);
    }
}
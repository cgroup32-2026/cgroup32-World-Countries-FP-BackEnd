using System.Text.RegularExpressions;
using CountriesProject.DAL;
using CountriesProject.DAL.Models;
using CountriesProject.BL.Services;

namespace CountriesProject.BL
{
    public class AuthBL
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9_]{3,20}$", RegexOptions.Compiled);
        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled);
        
        private readonly LoginHistoryDAL _loginHistoryDAL;
        private readonly UserDAL _userDAL;
        private readonly JwtService _jwtService;

        public AuthBL(UserDAL userDAL, JwtService jwtService, LoginHistoryDAL loginHistoryDAL)
        {
            _userDAL = userDAL;
            _jwtService = jwtService;
            _loginHistoryDAL = loginHistoryDAL;
        }

        public AuthResult Register(string username, string email, string password, string fullName)
        {
            if (string.IsNullOrWhiteSpace(username) || !UsernameRegex.IsMatch(username))
                throw new Exception("Username must be 3-20 characters: letters, numbers, underscore only");
            if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
                throw new Exception("Invalid email format");
            if (string.IsNullOrWhiteSpace(password) || !PasswordRegex.IsMatch(password))
                throw new Exception("Password must be at least 8 characters, with an uppercase letter, a lowercase letter, and a digit");

            if (_userDAL.GetByUsername(username) != null)
                throw new Exception("Username already exists");


            // im using encryption for the password, alongside JWT tokens.
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            User newUser = new User { Username = username, Email = email, PasswordHash = passwordHash, FullName = fullName };

            int newId = _userDAL.Register(newUser);
            User createdUser = _userDAL.GetById(newId);
            _loginHistoryDAL.Insert(createdUser.UserId);
            string token = _jwtService.GenerateToken(createdUser);
            createdUser.PasswordHash = null;

            return new AuthResult { User = createdUser, Token = token };
        }

        public AuthResult Login(string username, string password)
        {
            User user = _userDAL.GetByUsername(username);
            if (user == null) throw new Exception("Invalid username or password");
            if (user.IsLocked) throw new Exception("This account is locked");
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new Exception("Invalid username or password");

            _userDAL.UpdateLastLogin(user.UserId);
            _loginHistoryDAL.Insert(user.UserId);
            string token = _jwtService.GenerateToken(user);
            user.PasswordHash = null;

            return new AuthResult { User = user, Token = token };
        }

        public User GetById(int userId)
        {
            User user = _userDAL.GetById(userId);
            if (user != null) user.PasswordHash = null;
            return user;
        }

        public User UpdateProfile(int userId, string email, string fullName)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
                throw new Exception("Invalid email format");

            _userDAL.UpdateProfile(userId, email, fullName);
            return GetById(userId);
        }

        public void ChangePassword(int userId, string currentPassword, string newPassword)
        {
            User user = _userDAL.GetById(userId);
            if (user == null) throw new Exception("user not found");
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                throw new Exception("current password is incorrect");
            if (string.IsNullOrWhiteSpace(newPassword) || !PasswordRegex.IsMatch(newPassword))
                throw new Exception("new password must be at least 8 characters, with an uppercase letter, a lowercase letter, and a digit");

            string newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userDAL.UpdatePassword(userId, newHash);
        }
    }
}
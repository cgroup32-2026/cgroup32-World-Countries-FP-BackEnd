using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthBL _authBL;

        public AuthController(AuthBL authBL)
        {
            _authBL = authBL;
        }

        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string? FullName{ get; set; }
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UpdateProfileRequest
        {
            public string Email { get; set; }
            public string FullName { get; set; }
        }

        public class ChangePasswordRequest
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }


        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            try { return Ok(_authBL.Register(request.Username, request.Email, request.Password, request.FullName)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try { return Ok(_authBL.Login(request.Username, request.Password)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPut("me/password")]
        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                _authBL.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }


        // this is the ptrotected endpoint,  for the JWT pipeline 
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var user = _authBL.GetById(int.Parse(userIdClaim));
            return user == null ? NotFound() : Ok(user);
        }

        
        [Authorize]
        [HttpPut("me")]
        public IActionResult UpdateProfile(UpdateProfileRequest request)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var user = _authBL.UpdateProfile(userId, request.Email, request.FullName);
                return Ok(user);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
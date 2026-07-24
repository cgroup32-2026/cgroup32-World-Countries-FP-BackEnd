using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CountriesProject.BL;
using System.Security.Claims;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminBL _adminBL;

        public AdminController(AdminBL adminBL)
        {
            _adminBL = adminBL;
        }

        public class SetLockedRequest { public bool IsLocked { get; set; } }
        public class SetCanShareRequest { public bool CanShare { get; set; } }

        [HttpGet("users")]
        public IActionResult GetUsers() => Ok(_adminBL.GetAllUsers());

        [HttpPut("users/{userId}/lock")]
        public IActionResult SetLocked(int userId, SetLockedRequest request)
        {
            try
            {
                int requestingAdminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                _adminBL.SetUserLocked(userId, request.IsLocked, requestingAdminId);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }


        [HttpPut("users/{userId}/sharing")]
        public IActionResult SetCanShare(int userId, SetCanShareRequest request)
        {
            _adminBL.SetUserCanShare(userId, request.CanShare);
            return Ok();
        }

        [HttpGet("stats")]
        public IActionResult GetStats() => Ok(_adminBL.GetUsageStats());


        [HttpGet("login-history")]
        public IActionResult GetLoginHistory([FromQuery] DateTime? date) => Ok(_adminBL.GetLoginHistory(date));
    }
}
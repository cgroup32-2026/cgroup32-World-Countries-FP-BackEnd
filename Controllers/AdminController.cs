using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CountriesProject.BL;

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
            _adminBL.SetUserLocked(userId, request.IsLocked);
            return Ok();
        }

        [HttpPut("users/{userId}/sharing")]
        public IActionResult SetCanShare(int userId, SetCanShareRequest request)
        {
            _adminBL.SetUserCanShare(userId, request.CanShare);
            return Ok();
        }

        [HttpGet("stats")]
        public IActionResult GetStats() => Ok(_adminBL.GetUsageStats());
    }
}
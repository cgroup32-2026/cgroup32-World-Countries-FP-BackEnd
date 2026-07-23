using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharesController : ControllerBase
    {
        private readonly ShareBL _shareBL;

        public SharesController(ShareBL shareBL)
        {
            _shareBL = shareBL;
        }
        public class CreateShareRequest
        {
            public int CountryId { get; set; }
            public string Content { get; set; }
            public int Rating { get; set; }
        }

        public class UpdateShareRequest
        {
            public string Content { get; set; }
            public int Rating { get; set; }
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // public: anyone can view shares, logged in or not
        [HttpGet]
        public IActionResult GetAll() => Ok(_shareBL.GetAll());

        [HttpGet("country/{countryId}")]
        public IActionResult GetByCountry(int countryId) => Ok(_shareBL.GetByCountry(countryId));

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMine() => Ok(_shareBL.GetByUser(GetCurrentUserId()));

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateShareRequest request)
        {
            try { return Ok(_shareBL.Create(GetCurrentUserId(), request.CountryId, request.Content, request.Rating)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPut("{shareId}")]
        public IActionResult Update(int shareId, UpdateShareRequest request)
        {
            try
            {
                _shareBL.Update(shareId, GetCurrentUserId(), request.Content, request.Rating);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpDelete("{shareId}")]
        public IActionResult Delete(int shareId)
        {
            try
            {
                _shareBL.Delete(shareId, GetCurrentUserId());
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
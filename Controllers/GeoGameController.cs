using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;
using CountriesProject.DAL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoGameController : ControllerBase
    {
        private readonly GeoGameBL _geoGameBL;

        public GeoGameController(GeoGameBL geoGameBL)
        {
            _geoGameBL = geoGameBL;
        }

        public class SubmitAttemptRequest
        {
            public string ModeCode { get; set; }
            public int Score { get; set; }
            public int Rounds { get; set; }
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet("modes")]
        public IActionResult GetModes() => Ok(_geoGameBL.GetAllModes());

        [Authorize]
        [HttpPost("attempts")]
        public IActionResult SubmitAttempt(SubmitAttemptRequest request)
        {
            try
            {
                _geoGameBL.SubmitAttempt(GetCurrentUserId(), request.ModeCode, request.Score, request.Rounds);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("leaderboard/{modeCode}")]
        public IActionResult GetLeaderboard(string modeCode) => Ok(_geoGameBL.GetLeaderboard(modeCode));

        [HttpGet("leaderboard-total")]
        public IActionResult GetTotalLeaderboard() => Ok(_geoGameBL.GetTotalLeaderboard());

        [Authorize]
        [HttpGet("me/attempts")]
        public IActionResult GetMyAttempts() => Ok(_geoGameBL.GetMyAttempts(GetCurrentUserId()));



        [HttpGet("landmarks")]
        public IActionResult GetLandmarkPool([FromServices] GeoGameLandmarkDAL landmarkDAL) => Ok(landmarkDAL.GetAll());
    }
}
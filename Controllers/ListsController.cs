using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListsController : ControllerBase
    {
        private readonly UserListsBL _listsBL;

        public ListsController(UserListsBL listsBL)
        {
            _listsBL = listsBL;
        }

        public class AddToListRequest
        {
            public int CountryId { get; set; }
            public string ListType { get; set; }
        }

        public class MoveRequest
        {
            public int CountryId { get; set; }
            public string FromListType { get; set; }
            public string ToListType { get; set; }
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet("me")]
        public IActionResult GetMyList([FromQuery] string? listType = null)
        {
            try { return Ok(_listsBL.GetList(GetCurrentUserId(), listType)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("me")]
        public IActionResult AddToMyList(AddToListRequest request)
        {
            try
            {
                _listsBL.AddToList(GetCurrentUserId(), request.CountryId, request.ListType);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("me/{countryId}/{listType}")]
        public IActionResult RemoveFromMyList(int countryId, string listType)
        {
            try
            {
                _listsBL.RemoveFromList(GetCurrentUserId(), countryId, listType);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("me/move")]
        public IActionResult MoveInMyList(MoveRequest request)
        {
            try
            {
                _listsBL.MoveToList(GetCurrentUserId(), request.CountryId, request.FromListType, request.ToListType);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
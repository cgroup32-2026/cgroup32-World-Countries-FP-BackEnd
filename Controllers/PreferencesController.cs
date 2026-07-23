using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CountriesProject.BL;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly PreferencesBL _preferencesBL;

        public PreferencesController(PreferencesBL preferencesBL)
        {
            _preferencesBL = preferencesBL;
        }

        public class SetContinentsRequest
        {
            public List<int> ContinentIds { get; set; }
        }

        public class LanguageInput
        {
            public int LanguageId { get; set; }
            public string Level { get; set; }
        }

        public class SetLanguagesRequest
        {
            public List<LanguageInput> Languages { get; set; }
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // this is for public users :  populates dropdowns/checkboxes on  preferences 
        [HttpGet("continents")]
        public IActionResult GetAllContinents() => Ok(_preferencesBL.GetAllContinents());

        [HttpGet("languages")]
        public IActionResult GetAllLanguages() => Ok(_preferencesBL.GetAllLanguages());

        // protected , for logged in only
        [Authorize]
        [HttpGet("me/continents")]
        public IActionResult GetMyContinents() => Ok(_preferencesBL.GetUserContinents(GetCurrentUserId()));

        [Authorize]
        [HttpPut("me/continents")]
        public IActionResult SetMyContinents(SetContinentsRequest request)
        {
            _preferencesBL.SetUserContinents(GetCurrentUserId(), request.ContinentIds);
            return Ok();
        }

        [Authorize]
        [HttpGet("me/languages")]
        public IActionResult GetMyLanguages() => Ok(_preferencesBL.GetUserLanguages(GetCurrentUserId()));

        [Authorize]
        [HttpPut("me/languages")]
        public IActionResult SetMyLanguages(SetLanguagesRequest request)
        {
            try
            {
                var languages = request.Languages.Select(l => (l.LanguageId, l.Level)).ToList();
                _preferencesBL.SetUserLanguages(GetCurrentUserId(), languages);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
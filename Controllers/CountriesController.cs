using Microsoft.AspNetCore.Mvc;
using CountriesProject.BL;
using CountriesProject.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly CountryBL _countryBL;

        public CountriesController(CountryBL countryBL)
        {
            _countryBL = countryBL;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_countryBL.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var country = _countryBL.GetById(id);
            return country == null ? NotFound() : Ok(country);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Country country)
        {
            try { return Ok(_countryBL.Create(country)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Country country)
        {
            country.CountryId = id;
            try { _countryBL.Update(country); return Ok(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try { _countryBL.Delete(id); return Ok(); }
            catch (Exception ex) { return NotFound(new { message = ex.Message }); }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            int count = await _countryBL.ImportFromRestCountriesAsync();
            return Ok(new { importedCount = count });
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] CountrySearchParams searchParams)
        {
            return Ok(_countryBL.Search(searchParams));
        }
    }
}
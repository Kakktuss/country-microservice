using System;
using System.Threading.Tasks;
using CountryApplication.Dtos.Request;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CountryApi.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/country")]
    public class CountryController : Controller
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [Authorize("country:read_all")]
        public async Task<IActionResult> Get()
        {
            var result = await _countryService.RetrieveAsync();

            if (result.IsFailed &&
                result.Errors.Exists(e => e.HasMetadata("errCode", e => (string) e == "errCountriesNotFound")))
                return NotFound();

            return Ok(result.ValueOrDefault);
        }

        [HttpGet("{countryUuid}")]
        [Authorize("country:read")]
        public async Task<IActionResult> Get(Guid countryUuid)
        {
            var result = await _countryService.RetrieveByUuidAsync(countryUuid);

            if (result.IsFailed &&
                result.Errors.Exists(e => e.HasMetadata("errCode", e => (string) e == "errCountryNotFound")))
                return NotFound();

            return Ok(result.ValueOrDefault);
        }

        [HttpPost]
        [Authorize("country:create")]
        public async Task<IActionResult> Create([FromForm] string name, [FromForm] string code)
        {
            var result = await _countryService.CreateAsync(new CreateCountryDto(name, code));

            if (result.IsFailed &&
                (result.Errors.Exists(e =>
                     e.HasMetadata("errCode", e => (string) e == "errCountryAlreadyExistsByName")) ||
                 result.Errors.Exists(e =>
                     e.HasMetadata("errCode", e => (string) e == "errCountryAlreadyExistsByName"))))
                return Conflict("The country already exists");

            if (result.IsFailed &&
                result.Errors.Exists(e => e.HasMetadata("errCode", e => (string) e == "errDbSaveFail")))
                return BadRequest("An error happened while trying to save the country into the database");

            return Ok();
        }

        [HttpDelete("{countryUuid}")]
        [Authorize("country:delete")]
        public async Task<IActionResult> Delete(Guid countryUuid)
        {
            var result = await _countryService.RemoveAsync(new RemoveCountryDto(countryUuid));

            if (result.IsFailed &&
                result.Errors.Exists(e => e.HasMetadata("errCode", e => (string) e == "errCountryNotFound")))
                return NotFound();

            return Ok();
        }
    }
}
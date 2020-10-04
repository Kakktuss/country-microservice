using System;
using System.Threading.Tasks;
using CountryApplication.Dtos.Request;
using CountryApplication.Services;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCountryDto createCountryDto)
        {
            var result = await _countryService.CreateAsync(createCountryDto);

            if (result.IsFailed &&
                (result.Errors.Exists(e => e.HasMetadata("errCode", "errCountryAlreadyExistsByName")) ||
                 result.Errors.Exists(e => e.HasMetadata("errCode", "errCountryAlreadyExistsByName"))))
                return Conflict("The country already exists");

            if (result.IsFailed && result.Errors.Exists(e => e.HasMetadata("errCode", "errDbSaveFail")))
                return BadRequest("An error happened while trying to save the country into the database");

            return Ok();
        }

        [HttpDelete("{countryUuid}")]
        public async Task<IActionResult> Delete(Guid countryUuid)
        {
            var result = await _countryService.DeleteAsync(new DeleteCountryDto(countryUuid));

            if (result.IsFailed && result.Errors.Exists(e => e.HasMetadata("errCode", "errCountryNotFound")))
                return NotFound();

            return Ok();
        }
    }
}
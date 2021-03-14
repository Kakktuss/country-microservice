using System.Threading.Tasks;
using CountryApplication.Dtos.Request;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Models;
using FluentResults;

namespace CountryApplication.Services
{
    public interface ICountryService
    {
        public Task<Result<Country>> CreateAsync(CreateCountryDto createCountryDto);
        
        public Task<Result<Country>> RemoveAsync(RemoveCountryDto createCountryDto);
    }
}
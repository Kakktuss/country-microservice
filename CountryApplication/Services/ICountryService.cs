using System.Threading.Tasks;
using CountryApplication.Dtos.Request;
using FluentResults;

namespace CountryApplication.Services
{
    public interface ICountryService
    {
        public Task<Result> CreateAsync(CreateCountryDto createCountryDto);
    }
}
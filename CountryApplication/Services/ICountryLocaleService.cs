using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Models;
using FluentResults;

namespace CountryApplication.Services
{
    public interface ICountryLocaleService
    {

        public Task<Result<Country>> AssignLocale(AssignLocaleDto assignLocaleDto);

        public Task<Result<Country>> DeassignLocale(DeassignLocaleDto deassignLocaleDto);

    }
}
using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Models;
using FluentResults;

namespace CountryApplication.Services
{
    public interface ICountryLocaleService
    {

        public Task<Result<Country>> AssignLocale(AssignCountryLocaleDto assignCountryLocaleDto);

        public Task<Result<Country>> DeassignLocale(UnassignCountryLocaleDto unassignCountryLocaleDto);

    }
}
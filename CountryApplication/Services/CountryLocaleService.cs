using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using FluentResults;

namespace CountryApplication.Services
{
    public class CountryLocaleService : ICountryLocaleService
    {

        private readonly ICountryRepository _countryRepository;

        private readonly ILocaleRepository _localeRepository;
        
        public CountryLocaleService(ICountryRepository countryRepository,
            ILocaleRepository localeRepository)
        {
            _countryRepository = countryRepository;

            _localeRepository = localeRepository;
        }
        
        public Task<Result<Country>> AssignLocale(AssignLocaleDto assignLocaleDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Country>> DeassignLocale(DeassignLocaleDto deassignLocaleDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
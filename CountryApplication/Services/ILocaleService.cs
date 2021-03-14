using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Locale;
using CountryApplication.Models;
using FluentResults;

namespace CountryApplication.Services
{
    public interface ILocaleService
    {
        public Task<Result<Locale>> CreateAsync(CreateLocaleDto createLocaleDto);

        public Task<Result<Locale>> RemoveAsync(RemoveLocaleDto removeLocaleDto);
    }
}
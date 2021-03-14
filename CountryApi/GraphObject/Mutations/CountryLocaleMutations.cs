using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Models;
using CountryApplication.Services;
using FluentResults;
using HotChocolate;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class CountryLocaleMutations
    {

        [GraphQLName("assignLocale")]
        public async Task<Country> AssignLocale(AssignLocaleDto input, [Service] ICountryLocaleService countryLocaleService)
        {
            var country = await countryLocaleService.AssignLocale(input);

            return country.ValueOrDefault;
        }
        
        [GraphQLName("deassignLocale")]
        public async Task<Country> DeassignLocale(DeassignLocaleDto input, [Service] ICountryLocaleService countryLocaleService)
        {
            var country = await countryLocaleService.DeassignLocale(input);

            return country.ValueOrDefault;
        }
        
    }
}
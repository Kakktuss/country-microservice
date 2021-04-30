using System.Threading.Tasks;
using CountryApi.GraphObject.InputTypes.Country.Locale;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.Models;
using CountryApplication.Services;
using FluentResults;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class CountryLocaleMutations
    {

        [GraphQLName("assignCountryLocale")]
        [Authorize(Policy = "country:locale:assign")]
        public async Task<Country> AssignCountryLocale([GraphQLType(typeof(AssignCountryLocaleInputType))] AssignCountryLocaleDto input, [Service] ICountryLocaleService countryLocaleService)
        {
            var country = await countryLocaleService.AssignLocale(input);

            return country.ValueOrDefault;
        }
        
        [GraphQLName("unassignCountryLocale")]
        [Authorize(Policy = "country:locale:unassign")]
        public async Task<Country> UnassignLocale([GraphQLType(typeof(UnassignCountryLocaleInputType))]UnassignCountryLocaleDto input, [Service] ICountryLocaleService countryLocaleService)
        {
            var country = await countryLocaleService.DeassignLocale(input);

            return country.ValueOrDefault;
        }
        
    }
}
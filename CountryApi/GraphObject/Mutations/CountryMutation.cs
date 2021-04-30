using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using CountryApplication.Services;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class CountryMutations
    {

        [GraphQLName("createCountry")]
        [Authorize(Policy = "country:create")]
        public async Task<Country> CreateCountry([GraphQLType(typeof(CreateCountryDto))] CreateCountryDto input, [Service] ICountryService countryService)
        {

            var result = await countryService.CreateAsync(input);

            return result.ValueOrDefault;

        }
        
        [GraphQLName("removeCountry")]
        [Authorize(Policy = "country:remove")]
        public async Task<Country> RemoveCountry([GraphQLType(typeof(RemoveCountryDto))] RemoveCountryDto input, [Service] ICountryService countryService)
        {

            var result = await countryService.RemoveAsync(input);

            return result.ValueOrDefault;

        }
        
    }
}
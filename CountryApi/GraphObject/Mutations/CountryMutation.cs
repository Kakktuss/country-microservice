using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Country;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using CountryApplication.Services;
using HotChocolate;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class CountryMutations
    {

        [GraphQLName("createCountry")]
        public async Task<Country> CreateCountry(CreateCountryDto input, [Service] ICountryService countryService)
        {

            var result = await countryService.CreateAsync(input);

            return result.ValueOrDefault;

        }
        
        [GraphQLName("removeCountry")]
        public async Task<Country> RemoveCountry(RemoveCountryDto input, [Service] ICountryService countryService)
        {

            var result = await countryService.RemoveAsync(input);

            return result.ValueOrDefault;

        }
        
    }
}
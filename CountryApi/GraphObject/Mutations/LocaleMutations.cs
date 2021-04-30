using System.Threading.Tasks;
using CountryApi.GraphObject.InputTypes.Country;
using CountryApi.GraphObject.Types;
using CountryApplication.Dtos.Request.Locale;
using CountryApplication.Models;
using CountryApplication.Services;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class LocaleMutations
    {

        [GraphQLName("createLocale")]
        [Authorize("locale:create")]
        public async Task<Locale> CreateLocale([GraphQLType(typeof(CreateLocaleInputType))] CreateLocaleDto input, [Service] ILocaleService localeService)
        {
            var locale = await localeService.CreateAsync(input);

            return locale.ValueOrDefault;
        }

        [GraphQLName("removeLocale")]
        [Authorize("locale:remove")]
        public async Task<Locale> RemoveLocale([GraphQLType(typeof(RemoveLocaleInputType))] RemoveLocaleDto input, [Service] ILocaleService localeService)
        {
            var locale = await localeService.RemoveAsync(input);

            return locale.ValueOrDefault;
        }
                
    }
}
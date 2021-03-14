using System.Threading.Tasks;
using CountryApplication.Dtos.Request.Locale;
using CountryApplication.Models;
using CountryApplication.Services;
using HotChocolate;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class LocaleMutations
    {

        [GraphQLName("createLocale")]
        public async Task<Locale> CreateLocale(CreateLocaleDto input, [Service] ILocaleService localeService)
        {
            var locale = await localeService.CreateAsync(input);

            return locale.ValueOrDefault;
        }

        [GraphQLName("removeLocale")]
        public async Task<Locale> RemoveLocale(RemoveLocaleDto input, [Service] ILocaleService localeService)
        {
            var locale = await localeService.RemoveAsync(input);

            return locale.ValueOrDefault;
        }
                
    }
}
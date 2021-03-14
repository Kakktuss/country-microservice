using System;
using System.Linq;
using System.Threading.Tasks;
using CountryApi.GraphObject.Types;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace CountryApi.GraphObject.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class LocaleQueries
    {

        [GraphQLName("locales")]
        // Authorize for admins
        public IQueryable<Locale> GetLocales([Service] ILocaleRepository localeRepository)
        {
            return localeRepository.GetLocales().AsNoTracking();
        }

        [GraphQLName("locale")]
        public Task<Locale> GetLocaleByUuid(Guid uuid, [Service] ILocaleRepository localeRepository)
        {

            return localeRepository.FindByUuidAsync(uuid);

        }

    }
}
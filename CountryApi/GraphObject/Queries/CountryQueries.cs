using System;
using System.Linq;
using System.Threading.Tasks;
using CountryApi.GraphObject.Types;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace CountryApi.GraphObject.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class CountryQueries
    {
        [GraphQLName("countries")]
        [GraphQLType(typeof(ListType<CountryObjectType>))]
        // Authorize for admins
        public IQueryable<Country> GetCountries([Service] ICountryRepository countryRepository)
        {
            return countryRepository.GetCountries()
                .AsNoTracking();
        }

        [GraphQLName("country")]
        [GraphQLType(typeof(CountryObjectType))]
        public Task<Country> GetCountryByUuid(Guid uuid, [Service] ICountryRepository countryRepository)
        {
            return countryRepository
                .FindByUuidAsync(uuid);
        }
    }
}
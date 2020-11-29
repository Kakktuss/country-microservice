using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.Models;
using CountryApplication.ViewModels;

namespace CountryApplication.DapperDataAccess.Repositories
{
    public interface ICountryRepository
    {
        public Task<IEnumerable<CountryViewModel>> GetCountriesAsync();

        public Task<CountryViewModel> FindCountryByUuidAsync(Guid uuid);
    }
}
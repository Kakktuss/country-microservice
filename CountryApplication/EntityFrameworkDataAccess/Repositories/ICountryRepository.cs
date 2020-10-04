using System;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.Models;

namespace CountryApplication.EntityFrameworkDataAccess.Repositories
{
    public interface ICountryRepository : IRepository
    {
        void Add(Country country);

        void Remove(Country country);

        Task<Country> FindByIdAsync(int id);

        Task<Country> FindByUuidAsync(Guid uuid);

        Task<bool> ExistsByCountryCodeAsync(string code);

        Task<bool> ExistsByCountryNameAsync(string name);

        Task<bool> ExistsByUuidAsync(Guid uuid);
    }
}
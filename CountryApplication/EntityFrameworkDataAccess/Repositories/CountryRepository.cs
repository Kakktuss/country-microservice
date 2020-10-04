using System;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CountryApplication.EntityFrameworkDataAccess.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly CountryContext _context;

        private readonly DbSet<Country> _countries;

        public CountryRepository(CountryContext context)
        {
            _context = context;

            _countries = context.Set<Country>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Country country)
        {
            _countries.Add(country);
        }

        public void Remove(Country country)
        {
            _countries.Remove(country);
        }

        public Task<Country> FindByIdAsync(int id)
        {
            return _countries.FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<Country> FindByUuidAsync(Guid uuid)
        {
            return _countries.FirstOrDefaultAsync(e => e.Uuid == uuid);
        }

        public Task<bool> ExistsByUuidAsync(Guid uuid)
        {
            return _countries.AnyAsync(e => e.Uuid == uuid);
        }

        public Task<bool> ExistsByCountryCodeAsync(string code)
        {
            return _countries.AnyAsync(e => e.Code == code);
        }

        public Task<bool> ExistsByCountryNameAsync(string name)
        {
            return _countries.AnyAsync(e => e.Name == name);
        }
    }
}
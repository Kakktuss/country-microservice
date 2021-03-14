using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CountryApplication.EntityFrameworkDataAccess.Repositories
{
    public class LocaleRepository : ILocaleRepository
    {
        private CountryContext _context;

        private readonly DbSet<Locale> _locales;

        public LocaleRepository(CountryContext context)
        {
            _context = context;

            _locales = context.Set<Locale>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Locale locale)
        {
            _locales.Add(locale);
        }

        public void Remove(Locale locale)
        {
            _locales.Remove(locale);
        }

        public IQueryable<Locale> GetLocales() => _locales;

        public Task<Locale> FindByUuidAsync(Guid uuid)
        {
            return _locales.FirstOrDefaultAsync(e => e.Uuid == uuid);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            return _locales.AnyAsync(e => e.Name == name);
        }
    }
}
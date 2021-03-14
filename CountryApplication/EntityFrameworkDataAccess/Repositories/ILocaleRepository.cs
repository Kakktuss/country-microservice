using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.Models;

namespace CountryApplication.EntityFrameworkDataAccess.Repositories
{
    public interface ILocaleRepository: IRepository
    {
        void Add(Locale locale);

        void Remove(Locale locale);

        IQueryable<Locale> GetLocales();

        Task<Locale> FindByUuidAsync(Guid uuid);
        
        Task<bool> ExistsByNameAsync(string name);
    }
}
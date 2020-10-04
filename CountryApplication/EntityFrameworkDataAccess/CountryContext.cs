using System.Threading;
using System.Threading.Tasks;
using BuildingBlock.DataAccess.Abstractions;
using CountryApplication.EntityFrameworkDataAccess.Configurations;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CountryApplication.EntityFrameworkDataAccess
{
    public class CountryContext : DbContext, IUnitOfWork
    {
        public CountryContext(DbContextOptions<CountryContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await SaveChangesAsync(cancellationToken);

            return result != 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryEntityTypeConfiguration());
        }
    }
}
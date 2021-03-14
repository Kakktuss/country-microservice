using Autofac;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using Microsoft.Extensions.Configuration;

namespace CountryApplication
{
    public class RepositoryAutofacModule : Module
    {
        private readonly string _connectionString;

        public RepositoryAutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // EF Core Configuration
            builder.RegisterType<CountryRepository>()
                .As<ICountryRepository>()
                .AsSelf();

            builder.RegisterType<LocaleRepository>()
                .As<ILocaleRepository>()
                .AsSelf();
            
            // Dapper Configuration
            builder.Register(c => new DapperDataAccess.Repositories.CountryRepository(_connectionString))
                .As<DapperDataAccess.Repositories.ICountryRepository>()
                .AsSelf();
        }
    }
}
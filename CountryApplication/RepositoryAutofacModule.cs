using Autofac;
using CountryApplication.EntityFrameworkDataAccess.Repositories;

namespace CountryApplication
{
    public class RepositoryAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // EF Core Configuration
            builder.RegisterType<CountryRepository>()
                .As<ICountryRepository>()
                .AsSelf();

            // Dapper Configuration
            builder.RegisterType<DapperDataAccess.Repositories.CountryRepository>()
                .As<DapperDataAccess.Repositories.ICountryRepository>()
                .AsSelf();
        }
    }
}
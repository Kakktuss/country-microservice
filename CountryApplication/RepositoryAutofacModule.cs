using Autofac;
using CountryApplication.EntityFrameworkDataAccess.Repositories;

namespace CountryApplication
{
    public class RepositoryAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterType<CountryRe>()

            builder.RegisterType<CountryRepository>()
                .As<ICountryRepository>()
                .AsSelf();
        }
    }
}
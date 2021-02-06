using Autofac;
using CountryApplication.Services;

namespace CountryApplication
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CountryService>()
                .As<ICountryService>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
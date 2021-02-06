using System.Threading;
using System.Threading.Tasks;
using CountryApplication.EntityFrameworkDataAccess;
using Microsoft.Extensions.Hosting;

namespace CountryApi.HostedServices
{
    public class EntityFrameworkInitializerHostedService : IHostedService
    {
        private readonly CountryContext _countryContext;
        
        public EntityFrameworkInitializerHostedService(CountryContext countryContext)
        {
            _countryContext = countryContext;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var model = _countryContext.Countries;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
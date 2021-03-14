using System.Threading.Tasks;
using BuildingBlock.Bus.Abstractions.Stan.Events;
using CountryApplication.Dtos.Request.Locale;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.IntegrationEvents.Events.Locale;
using CountryApplication.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CountryApplication.Services
{
    public class LocaleService : ILocaleService
    {
        private readonly ILocaleRepository _localeRepository;

        private readonly IStanIntegrationEventBus _stanIntegrationEventBus;

        private readonly ILogger<LocaleService> _logger;
        
        public LocaleService(ILocaleRepository localeRepository,
            IStanIntegrationEventBus stanIntegrationEventBus,
            ILogger<LocaleService> logger)
        {
            _localeRepository = localeRepository;

            _stanIntegrationEventBus = stanIntegrationEventBus;

            _logger = logger;
        }

        /// <summary>
        /// Create a new locale depending the input
        /// </summary>
        /// <param name="createLocaleDto"></param>
        /// <returns></returns>
        public async Task<Result<Locale>> CreateAsync(CreateLocaleDto createLocaleDto)
        {
            _logger.LogTrace("[LocaleService:CreateAsync] Starting processing the command.");

            if (await _localeRepository.ExistsByNameAsync(createLocaleDto.Name))
            {
                return Result.Fail(new Error("The request locale already exists"));
            }

            var locale = new Locale(createLocaleDto.Name);
            
            _localeRepository.Add(locale);
            
            var result = await _localeRepository.UnitOfWork.SaveEntitiesAsync();
                
            if (!result)
            {
                _logger.LogInformation(
                    "[LocaleService:CreateAsync] Error: An error happened while trying to create the locale");

                return Result.Fail(new Error("An error happened while trying to create the locale")
                    .WithMetadata("errCode", "errDbSaveFail"));
            }

            var subject = "locale";
            
            // Publish an event to notice locale was created
            _stanIntegrationEventBus.Publish(subject, "created", 
                new LocaleCreatedIntegrationEvent(locale.Uuid, locale.Name));
            
            return Result.Ok(locale);
        }

        public async Task<Result<Locale>> RemoveAsync(RemoveLocaleDto removeLocaleDto)
        {
            
            _logger.LogTrace("[LocaleService:DeleteAsync] Starting processing the command.");

            var locale = await _localeRepository.FindByUuidAsync(removeLocaleDto.Uuid);

            if (locale == null)
            {
                return Result.Fail(new Error("The requested locale   does not exists"));
            }
            
            _localeRepository.Remove(locale);

            var result = await _localeRepository.UnitOfWork.SaveEntitiesAsync();

            if (!result)
            {
                _logger.LogInformation(
                    "[LocaleService:CreateAsync] Error: An error happened while trying to create the locale");

                return Result.Fail(new Error("An error happened while trying to create the locale")
                    .WithMetadata("errCode", "errDbSaveFail"));
            }

            var subject = "locale";
            
            // Publish an event to notice locale was created
            _stanIntegrationEventBus.Publish(subject, "deleted", 
                new LocaleDeletedIntegrationEvent(locale.Uuid));
            
            return Result.Ok(locale);
        }
    }
}
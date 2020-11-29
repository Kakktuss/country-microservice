using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlock.Bus.Abstractions.Stan.Events;
using CountryApplication.Dtos.Request;
using CountryApplication.EntityFrameworkDataAccess.Repositories;
using CountryApplication.IntegrationEvents.Events;
using CountryApplication.Models;
using CountryApplication.ViewModels;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CountryApplication.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        private readonly DapperDataAccess.Repositories.ICountryRepository _dapperCountryRepository;

        private readonly ILogger<CountryService> _logger;

        private readonly IStanIntegrationEventBus _stanIntegrationEventBus;

        public CountryService(ICountryRepository countryRepository,
            DapperDataAccess.Repositories.ICountryRepository dapperCountryRepository,
            IStanIntegrationEventBus stanIntegrationEventBus,
            ILogger<CountryService> logger)
        {
            _countryRepository = countryRepository;

            _dapperCountryRepository = dapperCountryRepository;

            _stanIntegrationEventBus = stanIntegrationEventBus;

            _logger = logger;
        }

        public async Task<Result<IEnumerable<CountryViewModel>>> RetrieveAsync()
        {
            _logger.LogTrace("[CountryService:RetrieveAsync] Starting processing the command");

            var countries = await _dapperCountryRepository.GetCountriesAsync();

            if (!countries.Any())
                return Result.Fail(new Error("There is no country found")
                    .WithMetadata("errCode", "errCountriesNotFound"));

            _logger.LogTrace("[CountryService:RetrieveAsync] Command processed successfully");


            return Result.Ok(countries);
        }

        public async Task<Result<CountryViewModel>> RetrieveByUuidAsync(Guid uuid)
        {
            _logger.LogTrace("[CountryService:RetrieveByUuidAsync] Starting processing the command");

            var country = await _dapperCountryRepository.FindCountryByUuidAsync(uuid);

            if (country == null)
                return Result.Fail(new Error($"There is no country with uuid {uuid} found")
                    .WithMetadata("errCode", "errCountryNotFound"));

            _logger.LogTrace("[CountryService:RetrieveByUuidAsync] Command processed successfully");

            return Result.Ok(country);
        }

        public async Task<Result> CreateAsync(CreateCountryDto createCountryDto)
        {
            _logger.LogTrace("[CountryService:CreateAsync] Starting processing the command");

            if (await _countryRepository.ExistsByCountryNameAsync(createCountryDto.Name))
            {
                _logger.LogInformation(
                    $"[CountryService:CreateAsync] Error: The country with name {createCountryDto.Name} already exists");

                return Result.Fail(new Error($"The country with name {createCountryDto.Name} already exists")
                    .WithMetadata("errCode", "errCountryAlreadyExistsByName"));
            }

            if (await _countryRepository.ExistsByCountryCodeAsync(createCountryDto.Code))
            {
                _logger.LogInformation(
                    $"[CountryService:CreateAsync] Error: The country with name {createCountryDto.Name} already exists");

                return Result.Fail(new Error($"The country with code {createCountryDto.Code} already exists")
                    .WithMetadata("errCode", "errCountryAlreadyExistsByCode"));
            }

            var country = new Country(createCountryDto.Name, createCountryDto.Code);

            _countryRepository.Add(country);

            var result = await _countryRepository.UnitOfWork.SaveEntitiesAsync();

            if (!result)
            {
                _logger.LogInformation(
                    "[CountryService:CreateAsync] Error: An error happened while trying to create the country");

                return Result.Fail(new Error("An error happened while trying to create the country")
                    .WithMetadata("errCode", "errDbSaveFail"));
            }

            var subject = "country";

            _stanIntegrationEventBus.Publish(subject, "created",
                new CountryCreatedIntegrationEvent(country.Uuid, country.Name, country.Code));

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(DeleteCountryDto deleteCountryDto)
        {
            _logger.LogTrace("[CountryService:RemoveAsync] Starting processing the command");

            if (!await _countryRepository.ExistsByUuidAsync(deleteCountryDto.Uuid))
                return Result.Fail(new Error($"The country with uuid {deleteCountryDto.Uuid} does not exists")
                    .WithMetadata("errCode", "errCountryNotFound"));

            var country = await _countryRepository.FindByUuidAsync(deleteCountryDto.Uuid);

            if (country == null)
                return Result.Fail(new Error(""));

            _countryRepository.Remove(country);

            try
            {
                var result = await _countryRepository.UnitOfWork.SaveEntitiesAsync();

                if (!result)
                {
                    _logger.LogInformation(
                        "[CountryService:CreateAsync] Error: An error happened while trying to remove the country");

                    return Result.Fail(new Error("An error happened while trying to remove the country")
                        .WithMetadata("errCode", "errDbSaveFail"));
                }
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(
                    "[CountryService:CreateAsync] Error: An error happened while trying to remove the country", e);

                return Result.Fail(new Error("An error happened while trying to save the country")
                    .WithMetadata("errCode", "errDbSaveFail"));
            }

            var subject = "country";

            _stanIntegrationEventBus.Publish(subject, "deleted", new CountryDeletedIntegrationEvent(country.Uuid));

            return Result.Ok();
        }
    }
}
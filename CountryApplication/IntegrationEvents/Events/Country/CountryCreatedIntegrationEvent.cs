using System;
using BuildingBlock.Bus.Events;

namespace CountryApplication.IntegrationEvents.Events.Country
{
    public class CountryCreatedIntegrationEvent : IntegrationEvent
    {
        public CountryCreatedIntegrationEvent(Guid countryUuid,
            string countryName,
            string countryCode)
        {
            CountryUuid = countryUuid;

            CountryName = countryName;

            CountryCode = countryCode;
        }

        public Guid CountryUuid { get; }

        public string CountryName { get; }

        public string CountryCode { get; }
    }
}
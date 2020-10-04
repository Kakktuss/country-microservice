using System;
using BuildingBlock.Bus.Events;

namespace CountryApplication.IntegrationEvents.Events
{
    public class CountryDeletedIntegrationEvent : IntegrationEvent
    {
        public CountryDeletedIntegrationEvent(Guid countryUuid)
        {
            CountryUuid = countryUuid;
        }

        public Guid CountryUuid { get; }
    }
}
using System;
using BuildingBlock.Bus.Abstractions.Events;
using BuildingBlock.Bus.Events;

namespace CountryApplication.IntegrationEvents.Events.Locale
{
    public class LocaleDeletedIntegrationEvent : IntegrationEvent
    {
        public LocaleDeletedIntegrationEvent(Guid localeUuid)
        {
            LocaleUuid = localeUuid;
        }

        public Guid LocaleUuid { get; }
    }
}
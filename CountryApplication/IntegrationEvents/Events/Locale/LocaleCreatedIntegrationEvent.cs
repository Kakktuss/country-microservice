using System;
using BuildingBlock.Bus.Events;

namespace CountryApplication.IntegrationEvents.Events.Locale
{
    public class LocaleCreatedIntegrationEvent : IntegrationEvent
    {

        public LocaleCreatedIntegrationEvent(Guid localeUuid,
            string localeName)
        {
            LocaleUuid = localeUuid;

            LocaleName = localeName;
        }
        
        public Guid LocaleUuid { get; }
        
        public string LocaleName { get; }
        
    }
}
using System;

namespace CountryApplication.Dtos.Request.Country
{
    public class DeassignLocaleDto
    {
        public Guid CountryUuid { get; set; }
        
        public Guid LocaleUuid { get; set; }
    }
}
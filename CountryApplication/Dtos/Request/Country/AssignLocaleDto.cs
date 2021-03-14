using System;

namespace CountryApplication.Dtos.Request.Country
{
    public class AssignLocaleDto
    {
        public Guid CountryUuid { get; set; }
        
        public Guid LocaleUuid { get; set; }
    }
}
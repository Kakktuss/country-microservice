using System;

namespace CountryApplication.Dtos.Request.Country
{
    public class RemoveCountryDto
    {
        public RemoveCountryDto(Guid uuid)
        {
            Uuid = uuid;
        }

        public Guid Uuid { get; }
    }
}
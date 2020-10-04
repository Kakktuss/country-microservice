using System;

namespace CountryApplication.Dtos.Request
{
    public class DeleteCountryDto
    {
        public DeleteCountryDto(Guid uuid)
        {
            Uuid = uuid;
        }

        public Guid Uuid { get; }
    }
}
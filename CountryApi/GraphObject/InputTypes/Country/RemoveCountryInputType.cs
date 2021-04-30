using CountryApplication.Dtos.Request.Country;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country
{
    public class RemoveCountryInputType : InputObjectType<RemoveCountryDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveCountryDto> descriptor)
        {
            descriptor.Name("RemoveCountryInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
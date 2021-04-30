using CountryApplication.Dtos.Request.Country;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country
{
    public class CreateCountryInputType : InputObjectType<CreateCountryDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCountryDto> descriptor)
        {
            descriptor.Name("CreateCountryInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
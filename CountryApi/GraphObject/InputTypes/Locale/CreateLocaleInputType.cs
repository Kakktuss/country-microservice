using CountryApplication.Dtos.Request.Locale;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country
{
    public class CreateLocaleInputType : InputObjectType<CreateLocaleDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateLocaleDto> descriptor)
        {
            descriptor.Name("CreateLocaleInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
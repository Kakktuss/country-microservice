using CountryApplication.Dtos.Request.Locale;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country
{
    public class RemoveLocaleInputType : InputObjectType<RemoveLocaleDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveLocaleDto> descriptor)
        {
            descriptor.Name("RemoveLocaleInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
using CountryApplication.Dtos.Request.Country;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country.Locale
{
    public class UnassignCountryLocaleInputType : InputObjectType<UnassignCountryLocaleDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UnassignCountryLocaleDto> descriptor)
        {
            descriptor.Name("UnassignCountryLocaleInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
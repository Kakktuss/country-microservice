using CountryApplication.Dtos.Request.Country;
using HotChocolate.Types;

namespace CountryApi.GraphObject.InputTypes.Country.Locale
{
    public class AssignCountryLocaleInputType : InputObjectType<AssignCountryLocaleDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AssignCountryLocaleDto> descriptor)
        {
            descriptor.Name("AssignCountryLocaleInput");
            
            descriptor.BindFieldsImplicitly();
        }
    }
}
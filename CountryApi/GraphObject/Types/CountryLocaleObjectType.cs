using CountryApplication.Models;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Types
{
    public class CountryLocaleObjectType : ObjectType<CountryLocale>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryLocale> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            // Ignore local country id & local locale id
            descriptor.Ignore(e => e.CountryId);

            descriptor.Ignore(e => e.LocaleId);
        }
    }
}
using CountryApplication.Models;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Types
{
    public class LocaleObjectType : ObjectType<Locale>
    {
        protected override void Configure(IObjectTypeDescriptor<Locale> descriptor)
        {
            descriptor.BindFieldsImplicitly();

                        
            descriptor.Ignore(e => e.Id);
            
            // Ignore local country id & local locale id
            descriptor.Ignore(e => e.DomainEvents);

            descriptor.Ignore(e => e.IsTransient());
        }
    }
}
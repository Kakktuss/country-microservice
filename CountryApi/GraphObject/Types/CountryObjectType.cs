using CountryApplication.Models;
using HotChocolate.Types;

namespace CountryApi.GraphObject.Types
{
    public class CountryObjectType : ObjectType<Country>
    {
        protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            // Ignore domain events and local id
            descriptor.Ignore(e => e.DomainEvents);
            
            descriptor.Ignore(e => e.IsTransient());

            descriptor.Ignore(e => e.Id);
        }
    }
}
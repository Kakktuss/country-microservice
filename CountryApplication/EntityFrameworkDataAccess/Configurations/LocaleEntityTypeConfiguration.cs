using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryApplication.EntityFrameworkDataAccess.Configurations
{
    public class LocaleEntityTypeConfiguration : IEntityTypeConfiguration<Locale>
    {
        public void Configure(EntityTypeBuilder<Locale> builder)
        {
            builder.ToTable("Locales");

            // Tell efcore to delegate the generation of the id to the database
            builder.Property(e => e.Id)
                .UseIdentityColumn()
                .Metadata
                .SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            
            builder.Property(e => e.Uuid);
            
            builder.Property(e => e.Name);
        }
    }
}
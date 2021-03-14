using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryApplication.EntityFrameworkDataAccess.Configurations
{
    public class CountryLocaleEntityTypeConfiguration : IEntityTypeConfiguration<CountryLocale>
    {
        public void Configure(EntityTypeBuilder<CountryLocale> builder)
        {
            builder.ToTable("CountryLocales");

            builder.HasKey(e => new {e.CountryId, e.LocaleId});
            
            builder.HasOne(e => e.Country)
                .WithMany(e => e.Locales)
                .HasForeignKey(e => e.CountryId);
            
            builder.HasOne(e => e.Locale)
                .WithMany(e => e.Countries)
                .HasForeignKey(e => e.LocaleId);
        }
    }
}
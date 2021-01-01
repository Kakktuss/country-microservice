using CountryApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CountryApplication.EntityFrameworkDataAccess.Configurations
{
    public class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            // Tell efcore to delegate the generation of the id to the database
            builder.Property(e => e.Id)
                .UseIdentityColumn()
                .Metadata
                .SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            // Setup the index UIX_Countries_Uuid on the Uuid
            builder.HasIndex(e => e.Uuid)
                .HasDatabaseName("UIX_Countries_Uuid")
                .IsUnique();

            // Setup the index UIX_Countries_Name on the Name
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("UIX_Countries_Name")
                .IsUnique();

            // Setup the index UIX_Countries_Code on the Uuid
            builder.HasIndex(e => e.Code)
                .HasDatabaseName("UIX_Countries_Code")
                .IsUnique();
        }
    }
}
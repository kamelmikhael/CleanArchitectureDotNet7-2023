using Domain.Entities.Lookups;
using Domain.Entities.StaticLookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public partial class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(TableNames.Countries, SchemaNames.LK, t => t.IsTemporal());

        builder.HasKey(e => e.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(RequestType.MaxNameLength);
        builder.Property(p => p.NameAr).IsRequired().HasMaxLength(RequestType.MaxNameArLength);
        builder.Property(p => p.Alpha3Code).HasMaxLength(3);
        builder.HasData(Country.GCC);

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Country> entity);
}

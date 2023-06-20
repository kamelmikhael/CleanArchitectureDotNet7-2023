using Domain.Entities.StaticLookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public partial class RequestTypeConfiguration : IEntityTypeConfiguration<RequestType>
{
    public void Configure(EntityTypeBuilder<RequestType> builder)
    {
        builder.ToTable(TableNames.RequestTypes, SchemaNames.LK, t => t.IsTemporal());

        builder.HasKey(e => e.Code);
        builder.Property(p => p.Code).HasColumnOrder(1).IsRequired().ValueGeneratedNever();
        builder.Property(p => p.Name).HasColumnOrder(2).IsRequired().HasMaxLength(RequestType.MaxNameLength);
        builder.Property(p => p.NameAr).HasColumnOrder(3).IsRequired().HasMaxLength(RequestType.MaxNameArLength);

        builder.HasData(RequestType.All);

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<RequestType> entity);
}

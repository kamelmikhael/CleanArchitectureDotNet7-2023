using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public partial class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable(TableNames.Authors, SchemaNames.ATH, t => t.IsTemporal());

        builder.Property(x => x.Name)
            .HasMaxLength(Author.MaxNameLength)
            .IsRequired();

        builder.HasMany<Book>()
            .WithOne()
            .HasForeignKey(x => x.AuthorId);

        builder.HasQueryFilter(x => x.IsDeleted == false);

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Author> entity);
}

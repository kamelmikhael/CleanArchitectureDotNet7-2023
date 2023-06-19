using Domain.Entities;
using Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Infrastructure.Configurations;

public partial class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> entity)
    {
        entity.ToTable($"{nameof(Book)}s");

        entity.Property(x => x.Title)
            .HasMaxLength(Book.MaxTitleLength)
            .IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(Book.MaxDescriptionLength)
            .IsRequired();

        entity.Property( x => x.PublishedOn)
            .HasColumnType("date")
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        entity.Property(x => x.PublishedTime)
            .HasColumnType("time")
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();

        // entity.OwnsMany(x => x.Translations).ToJson();

        var jsonSerializationOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
        };

        entity.Property(p => p.Translations)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonSerializationOptions),
                v => JsonSerializer.Deserialize<List<BookTranslation>>(v, jsonSerializationOptions)!);

        entity.HasQueryFilter(x => x.IsDeleted == false);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Book> entity);
}


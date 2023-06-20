using Domain.Entities;
using Domain.Enums;
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
        entity.ToTable(TableNames.Books, SchemaNames.BK, t => t.IsTemporal());

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
                translationValue => JsonSerializer.Serialize(translationValue, jsonSerializationOptions),
                dbValue => JsonSerializer.Deserialize<List<BookTranslation>>(dbValue, jsonSerializationOptions)!);

        entity.Property(p => p.Type)
            .HasConversion(
                enumValue => enumValue.ToString(),
                dbValue => Enum.Parse<BookType>(dbValue))
            .HasMaxLength(Book.MaxTypeLength);

        entity.HasQueryFilter(x => x.IsDeleted == false);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Book> entity);
}


using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Infrastructure.Configurations;

public partial class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(TableNames.Books, SchemaNames.BK, t => t.IsTemporal());

        //builder.Property(x => x.Title)
        //    .HasMaxLength(Book.MaxTitleLength)
        //    .IsRequired();
        builder.Property(x => x.Title)
            .HasConversion(
                bookTitle => bookTitle.Value,
                dbValue => BookTitle.Create(dbValue).Value)
            .HasMaxLength(BookTitle.MaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(Book.MaxDescriptionLength)
            .IsRequired();

        builder.Property( x => x.PublishedOn)
            .HasColumnType("date")
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        builder.Property(x => x.PublishedTime)
            .HasColumnType("time")
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();

        // entity.OwnsMany(x => x.Translations).ToJson();

        var jsonSerializationOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
        };

        builder.Property(p => p.Translations)
            .HasConversion(
                translationValue => JsonSerializer.Serialize(translationValue, jsonSerializationOptions),
                dbValue => JsonSerializer.Deserialize<List<BookTranslation>>(dbValue, jsonSerializationOptions)!);

        builder.Property(p => p.Type)
            .HasConversion(
                enumValue => enumValue.ToString(),
                dbValue => Enum.Parse<BookType>(dbValue))
            .HasMaxLength(Book.MaxTypeLength);

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId);

        builder.HasQueryFilter(x => x.IsDeleted == false);

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Book> entity);
}

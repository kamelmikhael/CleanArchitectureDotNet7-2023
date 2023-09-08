using Domain.Common;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class BookTitle : ValueObject
{
    public const int MaxLength = 50;
    public const int MinLength = 2;

    public BookTitle(string value)
    {
        // Validate(value);

        Value = value;
    }

    public string Value { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static explicit operator string(BookTitle bookTitle) => bookTitle.Value;

    public static AppResult<BookTitle> Create(string value)
    {
        return AppResult.Ensure(
            value,
            (e => !string.IsNullOrWhiteSpace(e), DomainErrors.BookTitle.Empty),
            (e => e.Length <= MaxLength, DomainErrors.BookTitle.MaxLength),
            (e => e.Length >= MinLength, DomainErrors.BookTitle.MinLength))
            .Map(e => new BookTitle(e));

        #region Validation
        //if (string.IsNullOrWhiteSpace(value))
        //    return AppResult.Failure<BookTitle>(DomainErrors.BookTitle.Empty);

        //if (value.Length > MaxLength)
        //    return AppResult.Failure<BookTitle>(DomainErrors.BookTitle.MaxLength);

        //if (value.Length < MinLength)
        //    return AppResult.Failure<BookTitle>(DomainErrors.BookTitle.MaxLength);
        #endregion

        //return new BookTitle(value);
    }

    private static void Validate(string value)
    {
        if (value.Length > MaxLength) 
            throw new BookTitleExceedMaxLengthDomainException(
                $"{nameof(value)} length is greater than {MaxLength}");
    }
}

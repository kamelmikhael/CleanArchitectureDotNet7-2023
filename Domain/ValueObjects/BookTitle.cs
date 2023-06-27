using Domain.Common;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class BookTitle : ValueObject
{
    public const int MaxLength = 50;

    private BookTitle(string value)
    {
        // Validate(value);

        Value = value;
    }

    public string Value { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static AppResult<BookTitle> Create(string value)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(value))
            return AppResult.Failure<BookTitle>(DomainErrors.BookTitle.Empty);

        if (value.Length > MaxLength)
            return AppResult.Failure<BookTitle>(DomainErrors.BookTitle.MaxLength);
        #endregion

        return new BookTitle(value);
    }

    private static void Validate(string value)
    {
        if (value.Length > MaxLength) 
            throw new BookTitleExceedMaxLengthDomainException(
                $"{nameof(value)} length is greater than {MaxLength}");
    }
}

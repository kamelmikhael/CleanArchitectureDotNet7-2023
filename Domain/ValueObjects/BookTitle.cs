using Domain.Common;
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
        if(string.IsNullOrWhiteSpace(value))
            return AppResult.Failure<BookTitle>(new AppError(
                $"{nameof(BookTitle)}.Empty",
                $"{nameof(BookTitle)} is empty"));

        if (value.Length > MaxLength)
            return AppResult.Failure<BookTitle>(new AppError(
                $"{nameof(BookTitle)}.MaxLength",
                $"{nameof(BookTitle)} must be less than or equal {MaxLength}"));

        return new BookTitle(value);
    }

    //private static void Validate(string value)
    //{
    //    if (value.Length > MaxLength) throw new ArgumentException($"{nameof(value)} length is greater than {MaxLength}");
    //}
}

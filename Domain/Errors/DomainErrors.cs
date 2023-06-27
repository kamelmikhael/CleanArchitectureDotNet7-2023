using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Record
    {
        public static AppError NotFound(string entityName, object id) => new(
            $"{entityName}.NotFound",
            $"{entityName} with [{id}] not found.");

        public static AppError NotFound(object id) => new(
            "Record.NotFound",
            $"Record with [{id}] not found.");

        public static AppError NotFound() => new(
            "Resource.NotFound",
            $"Resource is not found or you have no permission to access.");
    }

    public static class Book
    {
        public static readonly AppError TitleIsAlreadyUsed = new(
            $"{nameof(Book)}.{nameof(TitleIsAlreadyUsed)}",
            $"Title is already used with another book.");
    }

    public static class BookTitle
    {
        public static readonly AppError Empty = new AppError(
                $"{nameof(BookTitle)}.Empty",
                $"{nameof(BookTitle)} is empty");

        public static readonly AppError MaxLength = new AppError(
                $"{nameof(BookTitle)}.MaxLength",
                $"{nameof(BookTitle)} must be less than or equal {MaxLength}");
    }
}

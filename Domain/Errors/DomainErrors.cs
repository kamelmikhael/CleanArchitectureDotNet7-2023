using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Record
    {
        public static AppError NotFound(string entityName, object id) => new AppError(
            "Record.NotFound",
            $"{entityName} with [{id}] not found.");
    }

    public static class Book
    {
        public static AppError TitleIsAlreadyUsed = new AppError(
            $"{nameof(Book)}.{nameof(TitleIsAlreadyUsed)}",
            $"Title is already used with another book.");
    }
}

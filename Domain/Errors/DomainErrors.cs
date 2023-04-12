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
        public static AppError TitleIsAlreadyUsed = new(
            $"{nameof(Book)}.{nameof(TitleIsAlreadyUsed)}",
            $"Title is already used with another book.");
    }
}

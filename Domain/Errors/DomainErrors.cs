using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Record
    {
        public static AppError NotFound(string entityName, object id) => new(
            $"{entityName}.NotFound",
            $"{entityName} with Id = [{id}] was not found.");

        public static AppError NotFound(object id) => new(
            "Record.NotFound",
            $"Record with Id = [{id}] was not found.");

        public static AppError NotFound() => new(
            "Resource.NotFound",
            $"Resource was not found or you have no permission to access.");
    }

    public static class Book
    {
        public static readonly AppError TitleAlreadyInUse = new(
            $"{nameof(Book)}.{nameof(TitleAlreadyInUse)}",
            $"Title is already used with another book.");
    }

    public static class BookTitle
    {
        public static readonly AppError Empty = new AppError(
                $"{nameof(BookTitle)}.Empty",
                $"{nameof(BookTitle)} is empty");

        public static readonly AppError MaxLength = new AppError(
                $"{nameof(BookTitle)}.MaxLength",
                $"{nameof(BookTitle)} must be less than or equal {ValueObjects.BookTitle.MaxLength}");

        public static readonly AppError MinLength = new AppError(
                $"{nameof(BookTitle)}.MinLength",
                $"{nameof(BookTitle)} must be greater than or equal {ValueObjects.BookTitle.MinLength}");
    }

    public static class Author
    {
        public static readonly AppError NameAlreadyExist = new AppError(
                $"{nameof(Author)}.NameAlreadyExist",
                $"{nameof(Author)} name is already exist");
    }

    public static class Account
    {
        public static readonly AppError InvalidCredentials = new AppError(
                $"{nameof(Account)}.InvalidCredentials",
                $"Please enter your correct email/password");
    }
}

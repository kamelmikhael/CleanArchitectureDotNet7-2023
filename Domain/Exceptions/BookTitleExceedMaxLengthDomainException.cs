namespace Domain.Exceptions;

public sealed class BookTitleExceedMaxLengthDomainException : DomainException
{
    public BookTitleExceedMaxLengthDomainException(string message) 
        : base(message)
    { }
}

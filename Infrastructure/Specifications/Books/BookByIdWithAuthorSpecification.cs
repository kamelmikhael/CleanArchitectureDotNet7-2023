using Domain.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Specifications.Books;

public class BookByIdWithAuthorSpecification : Specification<Book>
{
    public BookByIdWithAuthorSpecification(Guid bookId) 
        : base(book => book.Id == bookId)
    {
        // AddInclude(book => book.Author);

        // IsSplitQuery = true;

        // IsPaged = true;
        // PageIndex = 1;
        // PageSize = 5;
    }
}

using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Specifications.Books;

public class BookGetAllWithPaginationSpecification : Specification<Book, Guid>
{
    public BookGetAllWithPaginationSpecification(
        string keyword, 
        int pageIndex, 
        int pageSize) : base(book => string.IsNullOrEmpty(keyword) || ((string)book.Title).Contains(keyword))
    {
        WithPaging(pageIndex, pageSize);
    }
}

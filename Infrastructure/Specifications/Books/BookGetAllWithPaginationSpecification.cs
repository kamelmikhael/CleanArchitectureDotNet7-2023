using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Specifications.Books;

public class BookGetAllWithPaginationSpecification : Specification<Book, Guid>
{
    public BookGetAllWithPaginationSpecification(
        string keyword, 
        int pageIndex, 
        int pageSize) : base(book => string.IsNullOrEmpty(keyword) || book.Title == BookTitle.Create(keyword).Value)
    {
        WithPaging(pageIndex, pageSize);
    }
}

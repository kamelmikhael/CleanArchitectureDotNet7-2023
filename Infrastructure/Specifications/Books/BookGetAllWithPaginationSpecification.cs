using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Specifications.Books;

public class BookGetAllWithPaginationSpecification : Specification<Book>
{
    public BookGetAllWithPaginationSpecification(
        string keyword, 
        int pageIndex, 
        int pageSize) : base(book => string.IsNullOrEmpty(keyword) || book.Title == BookTitle.Create(keyword).Value)
    {
        IsPagedResult = true;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

using Domain.Entities;

namespace Infrastructure.Specifications.Books;

public class BookGetAllWithPaginationSpecification : Specification<Book>
{
    public BookGetAllWithPaginationSpecification(
        string keyword, 
        int pageIndex, 
        int pageSize) : base(book => book.Title.Contains(keyword))
    {
        IsPagedResult = true;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}

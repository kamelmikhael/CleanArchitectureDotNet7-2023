namespace Application.Common;

public class PagedRequestDto
{
    private const int MinPageSize = 10;
    private const int MinPageNumber = 0;

    private int _pageNumber = MinPageNumber;

    public int PageIndex
    {
        get => _pageNumber;
        set => _pageNumber = (value < 0) ? MinPageNumber : value;
    }


    private int _pageSize = MinPageSize;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = (value <= 0) ? MinPageSize : value;
        }
    }
}
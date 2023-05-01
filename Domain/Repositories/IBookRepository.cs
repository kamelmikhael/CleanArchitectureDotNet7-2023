using Domain.Entities;

namespace Domain.Repositories;

public interface IBookRepository
{
    #region Read Methods
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Book>> GetAllWithPagingAsync(string keyword, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    #endregion

    #region Write Methods
    void Add(Book entity);

    void Delete(Book entity);
    #endregion

    #region Business Methods
    Task<bool> IsBookTitleUniqueAsync(string title, CancellationToken cancellationToken = default);
    #endregion
}

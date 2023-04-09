using Domain.Entities;

namespace Domain.Repositories;

public interface IBookRepository
{
    #region Read Methods
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Book>> GetAllWithPagingAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    #endregion

    #region Write Methods
    void Add(Book entity);

    void Delete(Book entity);
    #endregion
}

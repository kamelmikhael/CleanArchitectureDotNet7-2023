using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BookRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Read Methods
    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Book>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Book>> GetAllWithPagingAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
        => await _dbContext.Set<Book>()
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
        => await _dbContext.Set<Book>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    #endregion

    #region Write Methods
    public void Add(Book entity)
        => _dbContext.Set<Book>().Add(entity);

    public void Delete(Book entity)
        => _dbContext.Set<Book>().Remove(entity);
    #endregion
}

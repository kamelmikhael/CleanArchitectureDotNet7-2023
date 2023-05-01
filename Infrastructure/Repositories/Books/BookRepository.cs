using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Contexts;
using Infrastructure.Specifications;
using Infrastructure.Specifications.Books;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Books;

public sealed class BookRepository : IBookRepository
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
        string keyword,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
        => await ApplySpecification(new BookGetAllWithPaginationSpecification(keyword, page, pageSize))
            .ToListAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await ApplySpecification(new BookByIdWithAuthorSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> CountAsync(
        CancellationToken cancellationToken = default)
        => await _dbContext.Set<Book>().CountAsync(cancellationToken);
    #endregion

    #region Write Methods
    public void Add(Book entity)
        => _dbContext.Set<Book>().Add(entity);

    public void Delete(Book entity)
        => _dbContext.Set<Book>().Remove(entity);
    #endregion

    #region Business Methods
    public async Task<bool> IsBookTitleUniqueAsync(
        string title,
        CancellationToken cancellationToken = default)
        => await _dbContext.Set<Book>().AnyAsync(x => x.Title != title);
    #endregion

    private IQueryable<Book> ApplySpecification(Specification<Book> specification)
        => SpecificationEvaluator.GetQuery(_dbContext.Set<Book>(), specification);
}

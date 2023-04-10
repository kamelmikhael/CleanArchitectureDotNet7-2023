using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.Books;

public class CachedBookRepository : IBookRepository
{
    private readonly BookRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedBookRepository(
        BookRepository decorated,
        IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public void Add(Book entity)
    {
        _decorated.Add(entity);
    }

    public void Delete(Book entity)
    {
        _decorated.Delete(entity);
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _decorated.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAllWithPagingAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetAllWithPagingAsync(page, pageSize, cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"book-{id}";

        return await _memoryCache.GetOrCreateAsync(
            key,
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return await _decorated.GetByIdAsync(id, cancellationToken);
            });
    }

    public async Task<bool> IsBookTitleUniqueAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _decorated.IsBookTitleUniqueAsync(title, cancellationToken);
    }
}

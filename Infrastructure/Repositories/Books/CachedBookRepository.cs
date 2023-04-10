using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Infrastructure.Repositories.Books;

public class CachedBookRepository : IBookRepository
{
    private readonly BookRepository _decorated;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;

    public CachedBookRepository(
        BookRepository decorated,
        IMemoryCache memoryCache,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
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

        //return await _memoryCache.GetOrCreateAsync(
        //    key,
        //    async entry =>
        //    {
        //        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
        //        return await _decorated.GetByIdAsync(id, cancellationToken);
        //    });

        string? cahchedBook = await _distributedCache.GetStringAsync(key, cancellationToken);

        Book? book;

        if (string.IsNullOrEmpty(cahchedBook)) 
        {
            book = await _decorated.GetByIdAsync(id, cancellationToken);
            
            if (book is not null)
            {
                await _distributedCache.SetStringAsync(
                key,
                JsonSerializer.Serialize(book),
                cancellationToken);
            }

            return book;
        }

        book = JsonSerializer.Deserialize<Book>(cahchedBook);

        return book;
    }

    public async Task<bool> IsBookTitleUniqueAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _decorated.IsBookTitleUniqueAsync(title, cancellationToken);
    }
}

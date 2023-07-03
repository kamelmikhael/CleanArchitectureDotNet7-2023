using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

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
        => _decorated.Add(entity);

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => _decorated.CountAsync(cancellationToken);

    public void Update(Book entity)
        => _decorated.Update(entity);

    public void Delete(Book entity)
        => _decorated.Delete(entity);

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _decorated.GetAllAsync(cancellationToken);

    public async Task<(IEnumerable<Book>, int)> GetAllWithPagingAsync(
        string keyword, 
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
        => await _decorated.GetAllWithPagingAsync(keyword, page, pageSize, cancellationToken);

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

        // return await GetDistributedCachedBook(id, key, cancellationToken);
    }

    private async Task<Book?> GetDistributedCachedBook(Guid id, string key, CancellationToken cancellationToken)
    {
        string? cahchedBook = await _distributedCache.GetStringAsync(key, cancellationToken);

        Book? book;

        if (string.IsNullOrEmpty(cahchedBook))
        {
            book = await _decorated.GetByIdAsync(id, cancellationToken);

            if (book is not null)
            {
                await _distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(book),
                cancellationToken);
            }

            return book;
        }

        book = JsonConvert.DeserializeObject<Book>(
            cahchedBook,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver(),
            });

        return book;
    }

    public async Task<bool> IsBookTitleUniqueAsync(BookTitle title, CancellationToken cancellationToken = default)
        => await _decorated.IsBookTitleUniqueAsync(title, cancellationToken);
}

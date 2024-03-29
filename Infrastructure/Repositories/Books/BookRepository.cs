﻿using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
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

    public async Task<(IEnumerable<Book>, int)> GetAllWithPagingAsync(
        string keyword,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
        => (
            await ApplySpecification(new BookGetAllWithPaginationSpecification(keyword, page, pageSize)).ToListAsync(cancellationToken),
            await ApplySpecification(new BookGetAllWithPaginationSpecification(keyword, 0, int.MaxValue)).CountAsync(cancellationToken)
        );

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

    public void Update(Book entity)
        => _dbContext.Set<Book>().Update(entity);

    public void Delete(Book entity)
        => _dbContext.Set<Book>().Remove(entity);
    #endregion

    #region Business Methods
    public async Task<bool> IsBookTitleUniqueAsync(
        BookTitle title,
        CancellationToken cancellationToken = default)
        => !await _dbContext.Set<Book>().AnyAsync(x => x.Title == title, cancellationToken);
    #endregion

    private IQueryable<Book> ApplySpecification(Specification<Book, Guid> specification)
        => SpecificationEvaluator.GetQuery(_dbContext.Set<Book>(), specification);
}

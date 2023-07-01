using Application.Features.BookFeatures.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.BookFeatures.Commands;

public class BookCreateCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;

    public BookCreateCommandHandlerTests()
    {
        _bookRepositoryMock = new();
        _unitOfWorkMock = new();
        _mapperMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenTitleIsNotUnique()
    {
        // Setup
        _bookRepositoryMock.Setup(
            x => x.IsBookTitleUniqueAsync(
                It.IsAny<BookTitle>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Arrange
        var command = new BookCreateCommand(
            "Book 1",
            "Description",
            BookType.Story,
            DateOnly.FromDateTime(DateTime.UtcNow),
            new() { });

        var handler = new BookCreateCommandHandler(
            _unitOfWorkMock.Object,
            _bookRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        AppResult<Guid> result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Book.TitleAlreadyInUse);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenTitleIsUnique()
    {
        // Setup
        _bookRepositoryMock.Setup(
            x => x.IsBookTitleUniqueAsync(
                It.IsAny<BookTitle>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mapperMock.Setup(
            x => x.Map<Book>(
                It.IsAny<BookCreateCommand>()))
            .Returns(Book.Create(
                Guid.NewGuid(),
                BookTitle.Create("Title").Value,
                "Description",
                BookType.Story,
                DateOnly.FromDateTime(DateTime.UtcNow),
                true).Value);

        // Arrange
        var command = new BookCreateCommand(
            "Book 1",
            "Description",
            BookType.Story,
            DateOnly.FromDateTime(DateTime.UtcNow),
            new() { });

        var handler = new BookCreateCommandHandler(
            _unitOfWorkMock.Object,
            _bookRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        AppResult<Guid> result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_CallAddOnRepoistory_WhenTitleIsUnique()
    {
        // Setup
        _bookRepositoryMock.Setup(
            x => x.IsBookTitleUniqueAsync(
                It.IsAny<BookTitle>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mapperMock.Setup(
            x => x.Map<Book>(
                It.IsAny<BookCreateCommand>()))
            .Returns(Book.Create(
                Guid.NewGuid(),
                BookTitle.Create("Title").Value,
                "Description",
                BookType.Story,
                DateOnly.FromDateTime(DateTime.UtcNow),
                true).Value);

        // Arrange
        var command = new BookCreateCommand(
            "Book 1",
            "Description",
            BookType.Story,
            DateOnly.FromDateTime(DateTime.UtcNow),
            new() { });

        var handler = new BookCreateCommandHandler(
            _unitOfWorkMock.Object,
            _bookRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        AppResult<Guid> result = await handler.Handle(command, default);

        // Assert
        _bookRepositoryMock.Verify(
            x => x.Add(It.Is<Book>(b => b.Id == result.Value)),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_Should_NotCallSaveChangesOnUnitOfWork_WhenTitleIsNotUnique()
    {
        // Setup
        _bookRepositoryMock.Setup(
            x => x.IsBookTitleUniqueAsync(
                It.IsAny<BookTitle>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Arrange
        var command = new BookCreateCommand(
            "Book 1",
            "Description",
            BookType.Story,
            DateOnly.FromDateTime(DateTime.UtcNow),
            new() { });

        var handler = new BookCreateCommandHandler(
            _unitOfWorkMock.Object,
            _bookRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        await handler.Handle(command, default);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}

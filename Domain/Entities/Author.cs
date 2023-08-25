using Domain.Common;
using Domain.Extensions;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public sealed class Author : FullAuditedEntity
{
    public const int MaxNameLength = 100;

    protected Author() { }

    private Author(int id, string name, DateTime? dateOfBirth)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
    }

    public string Name { get; private set; }

    public DateTime? DateOfBirth { get; private set; }

    public Author ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    private void SetName([NotNull] string name)
    {
        // Validate name
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: Author.MaxNameLength
        );
    }

    public static AppResult<Author> Create(
        int id,
        string name,
        DateTime? dateOfBirth)
    {
        var author = new Author(id, name, dateOfBirth);

        return author;
    }
}

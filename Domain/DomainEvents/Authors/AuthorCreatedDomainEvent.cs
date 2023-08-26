using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents.Authors;

public sealed record AuthorCreatedDomainEvent(
    Guid Id,
    int AuthorId) : DomainEvent(Id);

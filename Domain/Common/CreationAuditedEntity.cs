using Domain.Abstractions;
using System.Security.Principal;

namespace Domain.Common;

//
// Summary:
//     This class can be used to simplify implementing Abp.Domain.Entities.Auditing.ICreationAudited.
//
// Type parameters:
//   TPrimaryKey:
//     Type of the primary key of the entity
[Serializable]
public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
{
    //
    // Summary:
    //     Creation time of this entity.
    public virtual DateTime CreationTime { get; set; }

    //
    // Summary:
    //     Creator of this entity.
    public virtual Guid? CreatorUserId { get; set; }

    //
    // Summary:
    //     Constructor.
    protected CreationAuditedEntity()
    {
        CreationTime = DateTime.UtcNow;
    }
}


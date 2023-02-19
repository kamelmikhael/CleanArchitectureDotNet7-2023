using Domain.Abstractions;

namespace Domain.Common;

//
// Summary:
//     A shortcut of Abp.Domain.Entities.Auditing.AuditedEntity`1 for most used primary
//     key type (System.Int32).
[Serializable]
public abstract class AuditedEntity : AuditedEntity<int>, IBaseEntity, IBaseEntity<int>
{ }

//
// Summary:
//     This class can be used to simplify implementing Abp.Domain.Entities.Auditing.IAudited.
//
// Type parameters:
//   TPrimaryKey:
//     Type of the primary key of the entity
[Serializable]
public abstract class AuditedEntity<TPrimaryKey>
    : CreationAuditedEntity<TPrimaryKey>, IAudited, ICreationAudited, IModificationAudited
{
    //
    // Summary:
    //     Last modification date of this entity.
    public virtual DateTime? LastModificationTime { get; set; }

    //
    // Summary:
    //     Last modifier user of this entity.
    public virtual Guid? LastModifierUserId { get; set; }
}

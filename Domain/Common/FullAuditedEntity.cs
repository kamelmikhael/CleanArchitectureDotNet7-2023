using Domain.Abstractions;

namespace Domain.Common;

//
// Summary:
//     A shortcut of Abp.Domain.Entities.Auditing.FullAuditedEntity`1 for most used
//     primary key type (System.Int32).
[Serializable]
public abstract class FullAuditedEntity : FullAuditedEntity<int>, IBaseEntity, IBaseEntity<int>
{
}


//
// Summary:
//     Implements Abp.Domain.Entities.Auditing.IFullAudited to be a base class for full-audited
//     entities.
//
// Type parameters:
//   TPrimaryKey:
//     Type of the primary key of the entity
[Serializable]
public abstract class FullAuditedEntity<TPrimaryKey>
    : AuditedEntity<TPrimaryKey>, IFullAudited, IAudited, ICreationAudited, IModificationAudited, IDeletionAudited
{
    //
    // Summary:
    //     Is this entity Deleted?
    public virtual bool IsDeleted { get; set; }

    //
    // Summary:
    //     Which user deleted this entity?
    public virtual Guid? DeleterUserId { get; set; }

    //
    // Summary:
    //     Deletion time of this entity.
    public virtual DateTime? DeletionTime { get; set; }
}

using Domain.Abstractions;

namespace Application.Common;

//
// Summary:
//     Implements Abp.Domain.Entities.Auditing.IFullAudited to be a base class for full-audited
//     entities.
//
// Type parameters:
//   TPrimaryKey:
//     Type of the primary key of the entity
[Serializable]
public abstract class FullAuditedEntityDto<TPrimaryKey>
    : AuditedEntityDto<TPrimaryKey>, IFullAudited, IAudited, ICreationAudited, IModificationAudited, IDeletionAudited
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
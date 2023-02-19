using Domain.Abstractions;

namespace Application.Common;

//
// Summary:
//     This class can be used to simplify implementing Abp.Domain.Entities.Auditing.IAudited.
//
// Type parameters:
//   TPrimaryKey:
//     Type of the primary key of the entity
[Serializable]
public abstract class AuditedEntityDto<TPrimaryKey>
    : CreationAuditedEntityDto<TPrimaryKey>, IAudited, ICreationAudited, IModificationAudited
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
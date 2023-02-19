namespace Domain.Abstractions;

//
// Summary:
//     This interface is implemented by entities that is wanted to store creation information
//     (who and when created). Creation time and creator user are automatically set
//     when saving Abp.Domain.Entities.Entity to database.
public interface ICreationAudited
{
    /// <summary>
    /// Creation time of this entity.
    /// </summary>        
    DateTime CreationTime { get; set; }

    /// <summary>
    /// Id of the creator user of this entity.
    /// </summary>
    Guid? CreatorUserId { get; set; }
}

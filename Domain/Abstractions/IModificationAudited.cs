namespace Domain.Abstractions;

//
// Summary:
//     This interface is implemented by entities that is wanted to store modification
//     information (who and when modified lastly). Properties are automatically set
//     when updating the Abp.Domain.Entities.IEntity.
public interface IModificationAudited
{

    //
    // Summary:
    //     The last modified time for this entity.
    DateTime? LastModificationTime { get; set; }

    /// <summary>
    /// Last modifier user for this entity.
    /// </summary>        
    Guid? LastModifierUserId { get; set; }
}
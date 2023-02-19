namespace Domain.Abstractions;

//
// Summary:
//     This interface is implemented by entities which wanted to store deletion information
//     (who and when deleted).
public interface IDeletionAudited
{
    //
    // Summary:
    //     Deletion time of this entity.
    DateTime? DeletionTime { get; set; }

    //
    // Summary:
    //     Which user deleted this entity?
    Guid? DeleterUserId { get; set; }

    //
    // Summary:
    //     Used to mark an Entity as 'Deleted'.
    bool IsDeleted { get; set; }
}
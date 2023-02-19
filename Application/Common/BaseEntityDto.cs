using Domain.Abstractions;

namespace Application.Common;

public class BaseEntityDto : BaseEntityDto<int>
{ }

public class BaseEntityDto<TPrimaryKey> : IBaseEntity<TPrimaryKey>
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>        
    public virtual TPrimaryKey Id { get; set; }
}

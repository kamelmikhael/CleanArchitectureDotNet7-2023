using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

public class BaseEntity : BaseEntity<int>
{ }

public class BaseEntity<TPrimaryKey> : IBaseEntity<TPrimaryKey>
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>        
    public virtual TPrimaryKey Id { get; set; }
}

using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

public abstract class AggregateRoot : AggregateRoot<int>
{ }

public abstract class AggregateRoot<TPrimaryKey> : BaseEntity<TPrimaryKey>
{ }

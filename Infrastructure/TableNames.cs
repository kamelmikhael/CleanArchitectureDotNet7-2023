using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure;

internal static class SchemaNames
{
    public const string BK = nameof(BK);

    public const string LK = nameof(LK);
}

public static class TableNames
{
    public static readonly string Books = nameof(Books);
    public static readonly string RequestTypes = nameof(RequestTypes);
    public static readonly string Countries = nameof(Countries);
}

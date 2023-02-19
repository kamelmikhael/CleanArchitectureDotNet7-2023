using Domain.Entities;
using Domain.Enums;
using System.Text.Json;

namespace Infrastructure.Auditing;

public class AuditEntry
{
    public string TableName { get; set; }

    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();

    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

    public AuditType AuditType { get; set; }

    public List<string> ChangedColumns { get; } = new List<string>();

    public AuditLog ToAudit() => new()
    {
        AuditType = AuditType,
        Type = AuditType.ToString(),
        TableName = TableName,
        PrimaryKey = JsonSerializer.Serialize(KeyValues),
        OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
        NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
        AffectedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns),
    };
}

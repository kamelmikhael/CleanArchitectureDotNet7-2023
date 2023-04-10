using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class AuditLog : CreationAuditedEntity<Guid>
{
    /// <summary>
    /// Audit Type
    /// </summary>
    public AuditType? AuditType { get; set; }

    /// <summary>
    /// Type of Audit
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Table Name
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// Table Primary Key
    /// </summary>
    public string? PrimaryKey { get; set; }

    /// <summary>
    /// Old Values
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// New Values
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// Affected Columns
    /// </summary>
    public string? AffectedColumns { get; set; }
}

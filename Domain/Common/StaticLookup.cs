namespace Domain.Common;

public interface IStaticLookup<TPrimaryKey>
{
    TPrimaryKey Code { get; init; }
}

[Serializable]
public abstract class StaticLookup<TPrimaryKey> : IStaticLookup<TPrimaryKey>
{
    public virtual required TPrimaryKey Code { get; init; }
    public required string Name { get; set; }
    public string NameAr { get; set; }
    public override int GetHashCode() => HashCode.Combine(Code);

    public static implicit operator TPrimaryKey(StaticLookup<TPrimaryKey> instance) => instance.Code;
}



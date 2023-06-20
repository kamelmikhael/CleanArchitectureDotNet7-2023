namespace Domain.Common;

public class LookupBase : LookupBase<int>
{
    public static implicit operator int(LookupBase lookup) => lookup.Id;
}

public class LookupBase<TKey> : BaseEntity<TKey>
{
    public string Name { get; set; }
    public string NameAr { get; set; }

    public static implicit operator TKey(LookupBase<TKey> lookup) => lookup.Id;
}

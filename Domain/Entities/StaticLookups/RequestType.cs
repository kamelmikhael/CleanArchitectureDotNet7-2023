using Domain.Common;

namespace Domain.Entities.StaticLookups;

public class RequestType : StaticLookup<short>
{
    public const int MaxNameLength = 50;
    public const int MaxNameArLength = 50;

    public static readonly RequestType New = new()
    {
        Code = 1,
        Name = "New",
        NameAr = "جديد"
    };

    public static readonly RequestType Old = new()
    {
        Code = 2,
        Name = "Old",
        NameAr = "قدبم"
    };

    public static readonly IReadOnlyCollection<RequestType> All = new List<RequestType>
    {
        New,
        Old,
    };
}

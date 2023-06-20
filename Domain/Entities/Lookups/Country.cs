using Domain.Common;

namespace Domain.Entities.Lookups;

public class Country : LookupBase<int>
{
    public const int MaxNameLength = 50;
    public const int MaxNameArLength = 50;

    public int CallingCode { get; set; }
    public string Alpha3Code { get; set; }


    public static readonly int SaudiArabiaId = 1;

    public static readonly Country SaudiArabia = new()
    {
        Id = Country.SaudiArabiaId,
        Name = "Saudi Arabia",
        NameAr = "المملكة العربية السعودية",
        CallingCode = 966,
        Alpha3Code = "SAU"
    };

    public static readonly Country ArabEmirates = new()
    {
        Id = 12,
        Name = "Arab Emirates",
        NameAr = "الامارات  العربية المتحدة",
        CallingCode = 971,
        Alpha3Code = "ARE",
    };

    public static readonly Country Oman = new()
    {
        Id = 6,
        Name = "Oman",
        NameAr = "عمان",
        CallingCode = 968,
        Alpha3Code = "OMN",
    };

    public static readonly Country Kuwait = new()
    {
        Id = 9,
        Name = "Kuwait",
        NameAr = "الكويت",
        CallingCode = 965,
        Alpha3Code = "KWT",
    };

    public static readonly Country Bahrain = new()
    {
        Id = 3,
        Name = "Bahrain",
        NameAr = "البحرين",
        CallingCode = 973,
        Alpha3Code = "BHR",
    };

    public static readonly Country Qatar = new()
    {
        Id = 8,
        Name = "Qatar",
        NameAr = "قطر",
        CallingCode = 974,
        Alpha3Code = "QAT",
    };

    public static readonly IReadOnlyCollection<Country> GCC = new List<Country>
    {
        SaudiArabia,
        ArabEmirates,
        Oman,
        Kuwait,
        Bahrain,
        Qatar,
    };
}

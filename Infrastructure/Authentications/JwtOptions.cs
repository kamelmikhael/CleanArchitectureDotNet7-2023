namespace Infrastructure.Authentications;

public class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecertKey { get; init; }
}

namespace Infrastructure.Authentications;

public class JwtOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecertKey { get; init; } = string.Empty;
}

namespace Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken();
}

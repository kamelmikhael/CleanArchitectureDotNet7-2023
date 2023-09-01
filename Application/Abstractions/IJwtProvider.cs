using Domain.Entities;

namespace Application.Abstractions;

public interface IJwtProvider
{
    Task<string> Generate(User user);
}

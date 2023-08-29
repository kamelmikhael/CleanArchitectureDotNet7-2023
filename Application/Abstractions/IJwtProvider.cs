using Domain.Entities;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}

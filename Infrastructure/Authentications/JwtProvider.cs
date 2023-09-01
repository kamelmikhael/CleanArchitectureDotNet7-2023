using Application.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentications;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IPermissionService _permissionService;

    public JwtProvider(
        IOptions<JwtOptions> jwtOptions,
        IPermissionService permissionService)
    {
        _jwtOptions = jwtOptions.Value;
        _permissionService = permissionService;
    }

    public async Task<string> Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Name),
        };

        var permissions = await _permissionService.GetPermissionsAsync(user.Id);

        foreach (var permission in permissions)
        {
            claims.Add(new(CustomClaimNames.Permissions, permission));
        }

        var signingCredentinals = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecertKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1), // 1 hour life-time 
            signingCredentinals);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

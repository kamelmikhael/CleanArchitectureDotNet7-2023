//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.JsonWebTokens;

//namespace Infrastructure.Authentications;

//public class PermissionAuthorizationHandler
//    : AuthorizationHandler<PermissionRequirement>
//{
//    private readonly IServiceScopeFactory _serviceScopeFactory;

//    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
//    {
//        _serviceScopeFactory = serviceScopeFactory;
//    }

//    protected override Task HandleRequirementAsync(
//        AuthorizationHandlerContext context,
//        PermissionRequirement requirement)
//    {
//        //var userId = context.User.Claims.FirstOrDefault(
//        //    x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

//        //if (!long.TryParse(userId, out long parsetUserId)) { return; }

//        //using IServiceScope scope = _serviceScopeFactory.CreateScope();

//        //Approach 1 to get permissions from DB
//        //IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
//        //var permissions = await permissionService.GetPermissionsAsync(parsetUserId);
        
//        //Approach 2 to get permissions from Claims
//        var permissions = context.User
//            .Claims
//            .Where(x => x.Type == CustomClaimNames.Permissions)
//            .Select(x => x.Value)
//            .ToHashSet();

//        if (permissions.Contains(requirement.Permission))
//        {
//            context.Succeed(requirement);
//        }

//        return Task.CompletedTask;
//    }
//}

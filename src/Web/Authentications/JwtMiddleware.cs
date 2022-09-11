using Microsoft.AspNetCore.Http;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Stores;
using Tekoding.KoIdentity.Examples.API.Authentications;

namespace Tekoding.KoIdentity.Web.Authentications;

public class JwtMiddleware
{
    private RequestDelegate Next { get; }

    public JwtMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task Invoke(HttpContext context, IUserStore userStore, IUserRoleStore userRoleStore)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userSelectionResult = await userStore.FindByIdAsync(JwtUtils.ValidateToken(token));
        var user = userSelectionResult.Payload as User;
        context.Items["User"] = user;

        if (user is not null)
        {
            var rolesSelectionResult =
                await userRoleStore.GetRolesByUserId(((userSelectionResult.Payload as User)!).Id);
            context.Items["UserRoles"] = rolesSelectionResult.Payload as List<Role>;
        }

        await Next(context);
    }
}
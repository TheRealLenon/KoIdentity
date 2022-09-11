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

    public async Task Invoke(HttpContext context, IUserStore userStore)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var selectionResult = await userStore.FindByIdAsync(JwtUtils.ValidateToken(token));

        context.Items["User"] = selectionResult.Payload as User;

        await Next(context);
    }
}
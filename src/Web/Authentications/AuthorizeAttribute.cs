using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Examples.API.Authentications;

namespace Tekoding.KoIdentity.Web.Authentications;

/// <summary>
/// The authorization Attribute for non anonymous access.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private string[] RoleNames { get; }

    public AuthorizeAttribute(params string[] roleNames)
    {
        RoleNames = roleNames;
    }

    /// <inheritdoc />
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User;
        var userRoles = context.HttpContext.Items["UserRoles"] as List<Role>;

        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        if (user is null || (RoleNames.Length > 0 &&
                             !RoleNames.Any(x => userRoles is not null && userRoles.Any(y => y.Name == x))))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
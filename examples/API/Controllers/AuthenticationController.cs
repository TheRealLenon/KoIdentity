using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Core;
using Tekoding.KoIdentity.Web.Authentications;

namespace Tekoding.KoIdentity.Examples.API.Controllers;

/// <summary>
/// Provides authentication related endpoints.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private DatabaseContext DbContext { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="AuthenticationController"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="DatabaseContext"/> used to access the database.</param>
    public AuthenticationController(DatabaseContext dbContext)
    {
        DbContext = dbContext;
    }
    
    /// <summary>
    /// Generates a new login token.
    /// </summary>
    /// <param name="username">The username of the user to login.</param>
    /// <param name="password">The password of the user to login.</param>
    /// <returns>The JWT-Token assigned to the user.</returns>
    /// 
    /// <response code="201">Returns the JWT-Token assigned to the user.</response>
    /// <response code="400">Returns an information, that the password could not be verified.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Authentication
    ///     {
    ///        "Username": "TonyAvenger",
    ///        "Password": "HulkIsStronger"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(string username, string password)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound();
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return BadRequest();
        }

        return Ok(JwtUtils.GenerateJwtToken(user.Id));
    }

}
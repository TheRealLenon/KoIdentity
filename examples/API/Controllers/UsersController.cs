using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Models.Dtos;
using Tekoding.KoIdentity.Core.Stores;

namespace Tekoding.KoIdentity.Examples.API.Controllers;

// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
//[ProducesResponseType(StatusCodes.Status404NotFound)]

/// <summary>
/// Provides user related endpoints.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserStore UserStore { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="UsersController"/>.
    /// </summary>
    /// <param name="userStore">The store used to manage users in a persistence store.</param>
    public UsersController(IUserStore userStore)
    {
        UserStore = userStore;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="creationDto">The data transfer object of the user to create.</param>
    /// <returns>The unique identifier of the newly created user.</returns>
    /// 
    /// <response code="201">Returns the unique identifier of the newly created user.</response>
    /// <response code="500">Returns an information, that the creation failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Users
    ///     {
    ///        "Username": "TonyAvenger",
    ///        "Password": "HulkIsStronger"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(UserCreationDto creationDto)
    {
        var user = new User
        {
            Username = creationDto.Username,
            Password = creationDto.Password
        };
        
        var operationResult = await UserStore.CreateAsync(user);

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(Post), user.Id);
    }
}
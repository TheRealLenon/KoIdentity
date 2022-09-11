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
    
    /// <summary>
    /// Deletes a single user.
    /// </summary>
    /// <returns>The user to delete.</returns>
    /// <response code="200">Returns an status indicating that the removal of the user with the provided <paramref name="id"/> succeeded.</response>
    /// <response code="404">
    /// Returns a information, that the deletion failed, because no user with the <paramref name="id"/> was found.
    /// </response>
    /// <response code="500">Returns an information, that the deletion failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /Users/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpDelete("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var selectionResult = await UserStore.FindByIdAsync(id);

        if (!selectionResult.State || selectionResult.Payload is not User user)
        {
            return NotFound(id);
        }

        var deletionResult = await UserStore.DeleteAsync(user);

        if (!deletionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(id);
    }
    
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    /// <response code="200">Returns an array of all existing users.</response>
    /// <response code="500">Returns an information, that the selection failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /Users
    ///
    /// </remarks>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var operationResult = await UserStore.GetAllAsync();

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(operationResult.Payload as IEnumerable<User>);
    }
    
    /// <summary>
    /// Gets a single user.
    /// </summary>
    /// <returns>The user to be looked for.</returns>
    /// <response code="200">Returns an user, with the assigned <paramref name="id"/>.</response>
    /// <response code="500">Returns an information, that the selection failed due to an internal server error.</response>
    /// <response code="404">
    /// Returns an information, that the selection failed, because no user with the <paramref name="id"/> was found.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /Users/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(User))]
    public async Task<IActionResult> Get(Guid id)
    {
        var operationResult = await UserStore.FindByIdAsync(id);

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (operationResult.Payload == null || operationResult.Payload.GetType() != typeof(User))
        {
            return NotFound(id);
        }

        return Ok(operationResult.Payload as User);
    }
}
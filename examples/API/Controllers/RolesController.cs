using System.Net.Mime;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Models.Dtos;
using Tekoding.KoIdentity.Core.Stores;

namespace Tekoding.KoIdentity.Examples.API.Controllers;

/// <summary>
/// Provides role related endpoints.
/// </summary>
[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private IRoleStore RoleStore { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="RolesController"/>.
    /// </summary>
    /// <param name="roleStore">The store used to manage roles in a persistence store.</param>
    public RolesController(IRoleStore roleStore)
    {
        RoleStore = roleStore;
    }

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="roleName">The name of the desired role.</param>
    /// <returns>The unique identifier of the newly created role.</returns>
    /// 
    /// <response code="201">Returns the unique identifier of the newly created role.</response>
    /// <response code="500">Returns an information, that the creation failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Roles
    ///     {
    ///        "Name": "Support"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(string roleName)
    {
        var role = new Role
        {
            Name = roleName
        };
        
        var operationResult = await RoleStore.CreateAsync(role);

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(Post), role.Id);
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>A list of all roles.</returns>
    /// <response code="200">Returns an array of all existing roles.</response>
    /// <response code="500">Returns an information, that the selection failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /Roles
    ///
    /// </remarks>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var operationResult = await RoleStore.GetAllAsync();

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(operationResult.Payload as IEnumerable<Role>);
    }

    /// <summary>
    /// Gets a single role.
    /// </summary>
    /// <returns>The role to be looked for.</returns>
    /// <response code="200">Returns an role, with the assigned <paramref name="id"/>.</response>
    /// <response code="500">Returns an information, that the selection failed due to an internal server error.</response>
    /// <response code="404">
    /// Returns an information, that the selection failed, because no role with the <paramref name="id"/> was found.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /Roles/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Role))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Role))]
    public async Task<IActionResult> Get(Guid id)
    {
        var operationResult = await RoleStore.FindByIdAsync(id);

        if (!operationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (operationResult.Payload == null || operationResult.Payload.GetType() != typeof(Role))
        {
            return NotFound(id);
        }

        return Ok(operationResult.Payload as Role);
    }

    /// <summary>
    /// Deletes a single role.
    /// </summary>
    /// <returns>The role to delete.</returns>
    /// <response code="200">Returns an status indicating that the removal of the role with the provided <paramref name="id"/> succeeded.</response>
    /// <response code="404">
    /// Returns a information, that the deletion failed, because no role with the <paramref name="id"/> was found.
    /// </response>
    /// <response code="500">Returns an information, that the deletion failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /Roles/e802511d-7e23-4a19-72cf-08da30926879
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
        var selectionResult = await RoleStore.FindByIdAsync(id);

        if (!selectionResult.State || selectionResult.Payload is not Role role)
        {
            return NotFound(id);
        }

        var deletionResult = await RoleStore.DeleteAsync(role);

        if (!deletionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(id);
    }
}
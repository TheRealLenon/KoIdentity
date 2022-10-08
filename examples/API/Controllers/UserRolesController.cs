// KoIdentity Copyright (C) 2022 Tekoding. All Rights Reserved.
// 
// Created: 2022.05.14
// 
// Authors: TheRealLenon
// 
// Licensed under the MIT License. See LICENSE.md in the project root for license
// information.
// 
// KoIdentity is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the MIT
// License for more details.

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Stores;

namespace Tekoding.KoIdentity.Examples.API.Controllers;

/// <summary>
/// Provides user role related endpoints.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserRolesController: ControllerBase
{
    private IUserRoleStore UserRoleStore { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="UserRolesController"/>.
    /// </summary>
    /// <param name="userRoleStore">The manager used to manage user roles in a persistence store.</param>
    public UserRolesController(IUserRoleStore userRoleStore)
    {
        UserRoleStore = userRoleStore;
    }
    

    /// <summary>
    /// Creates a new user role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user the role should be assigned to.</param>
    /// <param name="roleId">The unique identifier of the role the user should be assigned to.</param>
    /// <returns>The unique identifier of the newly created user role.</returns>
    ///
    /// <response code="201">Returns the unique identifier of the newly created user role.</response>
    /// <response code="500">Returns an information, that the creation failed.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /UserRole
    ///     {
    ///        "UserId": "e802511d-7e23-4a19-72cf-08da30926879",
    ///        "RoleId": "e802511d-7e23-4a19-72cf-08da30926879",
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(Guid userId, Guid roleId)
    {
        var userRole = new UserRole<User>
        {
            UserId = userId,
            RoleId = roleId
        };

        var creationResult = await UserRoleStore.CreateAsync(userRole);

        if (!creationResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(Post), userRole.Id);
    }

    /// <summary>
    /// Gets all user roles.
    /// </summary>
    /// <returns>A list of all user roles.</returns>
    /// <response code="200">Returns an array of all existing user roles.</response>
    /// <response code="500">Returns an information, that the selection failed.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /UserRoles
    ///
    /// </remarks>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserRole<User>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var selectionResult = await UserRoleStore.GetAllAsync();

        if (!selectionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        return Ok(selectionResult.Payload as IEnumerable<UserRole<User>>);
    }
    
    /// <summary>
    /// Gets a single user role.
    /// </summary>
    /// <returns>The user role to be looked for.</returns>
    /// <response code="200">Returns a user role, with the assigned <paramref name="id"/>.</response>
    /// <response code="500">Returns an information, that the selection failed.</response>
    /// <response code="404">
    /// Returns an information, that the selection failed, because no user role with the provided <paramref name="id"/>
    /// was found.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /UserRoles/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRole<User>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UserRole<User>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var selectionResult = await UserRoleStore.FindByIdAsync(id);

        if (!selectionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        if (selectionResult.Payload == null || selectionResult.Payload.GetType() != typeof(UserRole<User>))
        {
            return NotFound(id);
        }

        return Ok(selectionResult.Payload as UserRole<User>);
    }
    
    /// <summary>
    /// Gets a list of roles assigned to the provided user.
    /// </summary>
    /// <returns>The list of roles the user has currently assigned.</returns>
    /// <response code="200">Returns an list of roles the user with the provided <paramref name="id"/> is currently assigned to.</response>
    /// <response code="500">Returns an information, that the selection failed.</response>
    /// <response code="404">
    /// Returns an information, that the selection failed, because the user with the provided <paramref name="id"/> does
    /// not have any roles assigned to.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /UserRoles/Roles/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpGet("Roles/{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Role>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    public async Task<IActionResult> GetRoles(Guid id)
    {
        var selectionResult = await UserRoleStore.GetRolesByUserId(id);

        if (!selectionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        if (selectionResult.Payload == null || selectionResult.Payload.GetType() != typeof(List<Role>))
        {
            return NotFound(id);
        }
        
        return Ok(selectionResult.Payload);
    }
    
    /// <summary>
    /// Gets a list of users assigned to the provided role.
    /// </summary>
    /// <returns>The list of users the role has currently assigned.</returns>
    /// <response code="200">Returns an list of users the role with the provided <paramref name="id"/> is currently assigned to.</response>
    /// <response code="500">Returns an information, that the selection failed.</response>
    /// <response code="404">
    /// Returns an information, that the selection failed, because the role with the provided <paramref name="id"/> does
    /// not have any users assigned to.
    /// </response>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /UserRoles/Users/e802511d-7e23-4a19-72cf-08da30926879
    ///
    /// </remarks>
    [HttpGet("Users/{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    public async Task<IActionResult> GetUsers(Guid id)
    {
        var selectionResult = await UserRoleStore.GetUsersByRoleId(id);

        if (!selectionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        if (selectionResult.Payload == null || selectionResult.Payload.GetType() != typeof(List<User>))
        {
            return NotFound(id);
        }
        
        return Ok(selectionResult.Payload);
    }

    /// <summary>
    /// Deletes a single user role.
    /// </summary>
    /// <returns>The unique identifier of the user role, which was deleted.</returns>
    /// <response code="200"> Returns the unique identifier of the deleted user role. </response>
    /// <response code="404">
    /// Returns an information, that the deletion failed, because no user role with the provided <paramref name="id"/>
    /// was found.
    /// </response>
    /// <response code="500">Returns an information, that the deletion failed due to an internal server error.</response>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /UserRoles/e802511d-7e23-4a19-72cf-08da30926879
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
        var selectionResult = await UserRoleStore.FindByIdAsync(id);

        if (!selectionResult.State || selectionResult.Payload is not UserRole<User> userRole)
        {
            return NotFound(id);
        }

        var deletionResult = await UserRoleStore.DeleteAsync(userRole);

        if (!deletionResult.State)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(id);
    }
}
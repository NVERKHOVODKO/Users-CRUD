using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.DTO;
using TestApplication.Services;


namespace TestApplication.Controllers;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="request">Request to create a user.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users/create
    ///     {
    ///         "name": "John Doe",
    ///         "email": "johndoe@example.com",
    ///         "age": 30
    ///     }
    ///
    /// </remarks>
    /// <returns>Result of user creation.</returns>
    /// <response code="200">User created successfully.</response>
    /// <response code="400">User with the same email already exists or the age is less than zero.</response>
    /// <response code="500">An error occurred while creating the user.</response>
    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation($"CreateUserRequest(Name: {request.Name}; Email: {request.Email}; Age: {request.Age})");
        try
        {
            if (!await _userService.IsEmailUniqueAsync(request.Email))
            {
                return BadRequest("Email isn't unique");
            }
            if (request.Age < 0)
            {
                return BadRequest("Age must be positive number");
            }
            await _userService.CreateUserAsync(request);
            return Ok("User created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Can't create user: {ex.Message}");
            return StatusCode(500, $"Can't create user: {ex.Message}");
        }
    }

    /// <summary>
    /// Add a role to a user.
    /// </summary>
    /// <param name="request">Request to add a role to a user.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users/addRole
    ///     {
    ///         "userId": "00000000-0000-0000-0000-000000000000",
    ///         "roleId": "00000000-0000-0000-0000-000000000000"
    ///     }
    /// </remarks>
    /// <returns>Result of adding the role to the user.</returns>
    /// <response code="200">Role added to the user successfully.</response>
    /// <response code="500">An error occurred while adding the role to the user.</response>
    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("addRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddUserRoleRequest request)
    {
        try
        {
            await _userService.AddRoleToUserAsync(request);
            return Ok("Role added to user successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Can't add role{request.RoleId} to user{request.UserId}: {ex.Message}");
            return StatusCode(500, $"Can't add role{request.RoleId} to user{request.UserId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Get a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users/getUser?id=00000000-0000-0000-0000-000000000000
    /// </remarks>
    /// <returns>The user with the specified ID.</returns>
    /// <response code="200">User found and returned successfully.</response>
    /// <response code="404">User with the specified ID not found.</response>
    [Authorize]
    [HttpGet("getUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Get a paginated list of users.
    /// </summary>
    /// <param name="pageNumber">The page number of the results (1-based).</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <remarks>
    /// </remarks>
    /// <returns>The user with the specified ID.</returns>
    /// <response code="200">List of users retrieved successfully.</response>
    /// <response code="400">Invalid page number or page size provided.</response>
    /// <response code="404">No users found.</response>
    [Authorize]
    [HttpGet("getUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
    {
        if (pageSize < 1 || pageNumber < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }

        var users = await _userService.GetUsers(pageNumber, pageSize);

        if (users.Any())
        {
            return Ok(users);
        }
    
        return NotFound("Users not found");
    }


    /// <summary>
    /// Filter and sort users with pagination.
    /// </summary>
    /// <param name="request">The filter and sort criteria.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users/filterSortUsers
    ///     {
    ///         "pageNumber": 1,
    ///         "pageSize": 10,
    ///         "filters": [
    ///             {
    ///                 "param": "age",
    ///                 "min": 20,
    ///                 "max": 30
    ///             },
    ///             {
    ///                 "param": "name",
    ///                 "min": 3,
    ///                 "max": 20
    ///             }
    ///         ],
    ///         "sortField": "name",
    ///         "sortDirection": "Ascending"
    ///     }
    /// </remarks>
    /// <returns>A paginated list of filtered and sorted users.</returns>
    /// <response code="200">Filtered and sorted users retrieved successfully.</response>
    /// <response code="400">Invalid page number or page size provided.</response>
    /// <response code="500">Error occurred while filtering/sorting users.</response>
    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("filterSortUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FilterSortUsers(FilterSortUserRequest request)
    {
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }
        try
        {
            var filteredUsers = await _userService.GetFilteredAndSortedUsers(request);
            return Ok(filteredUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error filtering/sorting users: {ex.Message}");
            return StatusCode(500, $"Error filtering/sorting users: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Filter and sort roles with pagination.
    /// </summary>
    /// <param name="request">The filter and sort criteria.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users/filterSortRoles
    ///     {
    ///         "pageNumber": 1,
    ///         "pageSize": 10,
    ///         "selectedRoles": ["Admin", "Support"],
    ///         "sortField": "role",
    ///         "sortDirection": "Ascending"
    ///     }
    /// </remarks>
    /// <returns>A paginated list of filtered and sorted roles.</returns>
    /// <response code="200">Filtered and sorted roles retrieved successfully.</response>
    /// <response code="400">Invalid page number or page size provided.</response>
    /// <response code="500">Error occurred while filtering/sorting roles.</response>
    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("filterSortRoles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FilterSortUsersRoles(FilterSortRolesRequest request)
    {
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }
        try
        {
            var filteredRoles = await _userService.GetFilteredAndSortedRoles(request);
            return Ok(filteredRoles);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error filtering/sorting roles: {ex.Message}");
            return StatusCode(500, $"Error filtering/sorting roles: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Edit an existing user.
    /// </summary>
    /// <param name="request">The user details to be edited.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/users/editUser
    ///     {
    ///         "id": "12345678-1234-1234-1234-123456789012",
    ///         "name": "Updated User",
    ///         "email": "updated@example.com",
    ///         "age": 35
    ///     }
    /// </remarks>
    /// <returns>A message indicating the result of the user edit.</returns>
    /// <response code="200">User edited successfully.</response>
    /// <response code="400">Invalid user details or non-unique email provided.</response>
    /// <response code="500">Error occurred while editing the user.</response>
    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPut("editUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditUser(EditUserRequest request)
    {
        if (request.Email == null || request.Name == null || request.Age == null || request.Id == null)
        {
            return BadRequest("Fill in all details");
        }
        if (request.Age < 0)
        {
            return BadRequest("Age must be positive");
        }
        try
        {
            if (!await _userService.IsEmailUniqueForUserAsync(request.Id, request.Email))
            {
                return BadRequest("Email isn't unique");
            }
            await _userService.EditUserAsync(request);
            _logger.LogInformation($"User with Id {request.Id} has been updated.");
            return Ok($"User with Id {request.Id} has been updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error editing user with Id {request.Id}: {ex.Message}");
            return StatusCode(500, $"User with Id {request.Id} hasn't been updated.");
        }
    }
    
    /// <summary>
    /// Delete a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/users/deleteUser?id=12345678-1234-1234-1234-123456789012
    /// </remarks>
    /// <returns>A message indicating the result of the user deletion.</returns>
    /// <response code="200">User deleted successfully.</response>
    /// <response code="400">Invalid ID provided.</response>
    /// <response code="500">Error occurred while deleting the user.</response>
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("deleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (id == null)
        {
            return BadRequest("Id must be not null");
        }
        try
        {
            await _userService.DeleteUserAsync(id);
            _logger.LogInformation($"User({id}) has been deleted.");
            return Ok($"User({id}) has been deleted.");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError($"User({id}) hasn't been deleted.");
            return StatusCode(StatusCodes.Status500InternalServerError, $"User({id}) hasn't been deleted.");
        }
    }
}
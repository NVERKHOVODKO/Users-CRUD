using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.DTO;
using TestApplication.Services;


namespace TestApplication.Controllers;

[Authorize]
[ApiController]
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
    
    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPost("create")]
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

    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("addRole")]
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

    [Authorize]
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsers();
        if (users.Any())
        {
            return Ok(users);
        }
        return NotFound("Users not found");
    }

    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("filterSort")]
    public async Task<IActionResult> FilterSortUsers(FilterSortRequest request)
    {
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
    
    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPut("editUser")]
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
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("deleteUser")]
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
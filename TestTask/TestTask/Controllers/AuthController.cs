using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Services;

namespace TestApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Generate an authentication token.
    /// </summary>
    /// <param name="email">The email of the user to log in.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users/login?email=johndoe@example.com
    /// </remarks>
    /// <returns>An authentication token if the login is successful.</returns>
    /// <response code="200">Authentication token generated successfully.</response>
    /// <response code="404">User with the provided email not found.</response>
    [HttpGet("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login(string email)
    {
        try
        {
            var token = await _authService.GenerateTokenAsync(email);
            return Ok(token);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound($"not found {e.Message}");
        }
    }
}
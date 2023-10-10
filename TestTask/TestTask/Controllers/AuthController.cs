using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.DTO;
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
    
    [HttpGet("login")]
    public async Task<IActionResult> Login(string email)
    {
        try
        {
            var token = await _authService.GenerateTokenAsync(email);
            return Ok(token);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("not found");
        }
    }
}
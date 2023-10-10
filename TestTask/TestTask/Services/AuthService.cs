using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestApplication.Controllers;
using TestApplication.Data;

namespace TestApplication.Services;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

    }
    
    public async Task<bool> IsUserExists(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user != null;
    }

    public async Task<string> GenerateTokenAsync(string email)
    {
        if (await IsUserExists(email) == false)
        {
            throw new KeyNotFoundException();
        }

        var roles = await GetUserRoles(email);

        var claims = new List<Claim>();

        if (roles != null && roles.Any())
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); 
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);
        
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    } 
    
    public async Task<List<string>> GetUserRoles(string email)
    {
        var user = await _context.Users
            .Include(u => u.UserRoleModels)
            .ThenInclude(ur => ur.RoleModel)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            var roles = user.UserRoleModels
                .Select(ur => ur.RoleModel.Role)
                .ToList();
        
            return roles;
        }

        throw new KeyNotFoundException();
    }
}
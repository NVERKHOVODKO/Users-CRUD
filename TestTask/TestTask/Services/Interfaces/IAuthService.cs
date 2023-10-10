namespace TestApplication.Services;

public interface IAuthService
{
    public Task<string> GenerateTokenAsync(string email);
}
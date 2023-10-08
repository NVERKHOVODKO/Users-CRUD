using TestApplication.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserRequest request);
    public Task AddRoleToUserAsync(AddUserRoleRequest roleRequest);
    public Task<UserModel> GetUser(Guid id);
    public Task<List<UserModel>> GetUsers();
    public Task DeleteUserAsync(Guid userId);
    public Task EditUserAsync(EditUserRequest request);
    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<bool> IsEmailUniqueForUserAsync(Guid userId, string email);
    public Task<List<UserModel>> GetFilteredAndSortedUsers(FilterSortRequest request);
    public Task DeleteUserRoleAsync(Guid userId, Guid roleId);
}
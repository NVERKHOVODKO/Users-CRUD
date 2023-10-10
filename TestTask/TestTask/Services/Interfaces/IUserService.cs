using TestApplication.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public interface IUserService
{
    public Task CreateUserAsync(CreateUserRequest request);
    public Task AddRoleToUserAsync(AddUserRoleRequest roleRequest);
    public Task<UserGetResponse> GetUser(Guid userId);
    public Task<List<UserGetResponse>> GetUsers();
    public Task DeleteUserAsync(Guid userId);
    public Task EditUserAsync(EditUserRequest request);
    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<bool> IsEmailUniqueForUserAsync(Guid userId, string email);
    public Task<List<UserModel>> GetFilteredAndSortedUsers(FilterSortRequest request);
}
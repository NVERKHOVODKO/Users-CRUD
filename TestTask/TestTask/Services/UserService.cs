using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestApplication.Controllers;
using TestApplication.Data;
using TestApplication.DTO;
using TestApplication.Models;
using TestTask.Enums;

namespace TestApplication.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly ILogger<UserController> _logger;

        public UserService(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task CreateUserAsync(CreateUserRequest request)
        {
            var user = new UserModel(Guid.NewGuid(), request.Name, request.Email, request.Age);
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User created (Name: {request.Name}; Email: {request.Email}; Age: {request.Age})");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"The user was not created (Name: {request.Name}; Email: {request.Email}; Age: {request.Age})");
                throw new Exception($"Error creating user: {ex.Message}");
            }
        }
        
        public async Task EditUserAsync(EditUserRequest request)
        {
            var userToUpdate = await _context.Users.FindAsync(request.Id);

            if (userToUpdate != null)
            {
                userToUpdate.Name = request.Name;
                userToUpdate.Email = request.Email;
                userToUpdate.Age = request.Age;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User with Id {request.Id} has been updated.");
            }
            else
            {
                _logger.LogInformation($"User with Id {request.Id} was not found.");
            }
        }
        
        public async Task AddRoleToUserAsync(AddUserRoleRequest roleRequest)
        {
            var userRole = new UserRoleModel
            {
                UserId = roleRequest.UserId,
                RoleId = roleRequest.RoleId
            };
            try
            {
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding role to user: {ex.Message}");
            }
        }
        
        public async Task DeleteUserAsync(Guid userId)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    var userRolesToDelete = _context.UserRoles.Where(ur => ur.UserId == userId);
                    _context.UserRoles.RemoveRange(userRolesToDelete);
                    _context.Users.Remove(userToDelete);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"User with Id {userId} has been deleted.");
                }
                else
                {
                    _logger.LogInformation($"User with Id {userId} was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with Id {userId}: {ex.Message}");
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }

        
        public async Task DeleteUserRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                var userRoleToDelete = _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);
                if (userRoleToDelete != null)
                {
                    _context.UserRoles.Remove(userRoleToDelete);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Role with Id {roleId} has been removed from user with Id {userId}.");
                }
                else
                {
                    _logger.LogInformation($"Role with Id {roleId} was not found for user with Id {userId}.");
                }
            }
            else
            {
                _logger.LogInformation($"User with Id {userId} was not found.");
            }
        }


        public async Task<UserModel> GetUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleModels)
                .ThenInclude(ur => ur.RoleModel) // Включаем связанные объекты RoleModel
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return user;
        }
        
        public async Task<List<UserModel>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRoleModels)
                .ThenInclude(ur => ur.RoleModel)
                .ToListAsync();
            
            return users;
        }
        
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var userWithSameEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return userWithSameEmail == null;
        }
        
        public async Task<bool> IsEmailUniqueForUserAsync(Guid userId, string email)
        {
            var userWithSameEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Id != userId);
            return userWithSameEmail == null;
        }
        
        
        public async Task<List<UserModel>> GetFilteredAndSortedUsers(FilterSortRequest request)
        {
            IQueryable<UserModel> query = _context.Users
                .Include(u => u.UserRoleModels)
                .ThenInclude(ur => ur.RoleModel);


            foreach (var param in request.Filters)
            {
                if (!string.IsNullOrWhiteSpace(param.Param) && param.Min >= 0 && param.Max >= param.Min)
                {
                    switch (param.Param.ToLower())
                    {
                        case "age":
                            query = query.Where(u => u.Age >= param.Min && u.Age <= param.Max);
                            break;
                        case "name":
                            query = query.Where(u => u.Name.Length >= param.Min && u.Name.Length <= param.Max);
                            break;
                        case "email":
                            query = query.Where(u => u.Email.Length >= param.Min && u.Email.Length <= param.Max);
                            break;
                    }
                }
            }
            
            switch (request.SortField.ToLower())
            {
                case "age":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(u => u.Age)
                        : query.OrderByDescending(u => u.Age);
                    break;
                case "name":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(u => u.Name)
                        : query.OrderByDescending(u => u.Name);
                    break;
                case "email":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                    break;
            }

            var users = await query.ToListAsync();
    
            return users;
        }
    }
}
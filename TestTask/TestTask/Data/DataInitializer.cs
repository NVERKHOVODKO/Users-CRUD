using TestApplication.Models;

namespace TestApplication.Data
{
    public class DataInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            var roles = new List<RoleModel>
            {
                new RoleModel { Id = Guid.NewGuid(), Role = "User" },
                new RoleModel { Id = Guid.NewGuid(), Role = "Admin" },
                new RoleModel { Id = Guid.NewGuid(), Role = "Support" },
                new RoleModel { Id = Guid.NewGuid(), Role = "SuperAdmin" }
            };
            
            if (!context.Roles.Any())
            {
                Console.WriteLine("Roles");
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            var users = new List<UserModel>
            {
                new UserModel { Id = Guid.NewGuid(), Name = "name1", Email = "email1@gmail.com", Age = 14 },
                new UserModel { Id = Guid.NewGuid(), Name = "name2", Email = "email2@gmail.com", Age = 34 },
                new UserModel { Id = Guid.NewGuid(), Name = "name3", Email = "email3@gmail.com", Age = 33 },
            };
            
            if (!context.Users.Any())
            {
                Console.WriteLine("Users");
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            
            if (!context.UserRoles.Any())
            {
                var userRoles = new List<UserRoleModel>
                {
                    new UserRoleModel { Id = Guid.NewGuid(), UserId = users[0].Id, RoleId = roles[0].Id },
                    new UserRoleModel { Id = Guid.NewGuid(), UserId = users[1].Id, RoleId = roles[1].Id },
                    new UserRoleModel { Id = Guid.NewGuid(), UserId = users[1].Id, RoleId = roles[3].Id },
                    new UserRoleModel { Id = Guid.NewGuid(), UserId = users[2].Id, RoleId = roles[1].Id },
                    new UserRoleModel { Id = Guid.NewGuid(), UserId = users[2].Id, RoleId = roles[2].Id },

                };
                Console.WriteLine("UserRoles");
                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }
        }
    }
}

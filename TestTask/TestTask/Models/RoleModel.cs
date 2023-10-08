namespace TestApplication.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public string Role { get; set; }
    public List<UserRoleModel> UserRoleModels { get; set; }
}

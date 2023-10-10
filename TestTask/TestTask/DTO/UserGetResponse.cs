using TestApplication.Models;

namespace TestApplication.DTO;

public class UserGetResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public List<RoleModel> Roles { get; set; }
}
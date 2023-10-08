namespace TestApplication.DTO;

public class AddUserRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
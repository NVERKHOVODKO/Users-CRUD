namespace TestApplication.DTO;

public class DeleteUserRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
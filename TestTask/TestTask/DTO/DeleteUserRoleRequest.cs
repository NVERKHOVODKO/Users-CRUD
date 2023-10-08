namespace TestApplication.DTO;

public class DeleteUserRoleRequest
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
}
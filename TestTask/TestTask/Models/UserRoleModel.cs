using System.ComponentModel.DataAnnotations;

namespace TestApplication.Models;

public class UserRoleModel
{
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public virtual UserModel UserModel { get; set; }
    [Required]
    public Guid RoleId { get; set; }
    public virtual RoleModel RoleModel { get; set; }
}
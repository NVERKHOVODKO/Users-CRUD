using System.ComponentModel.DataAnnotations;

namespace TestApplication.Models;

public class UserModel
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
    [Required]
    public int Age { get; set; }
    public List<UserRoleModel> UserRoleModels { get; set; }

    public UserModel(Guid id, string name, string email, int age)
    {
        Id = id;
        Name = name;
        Email = email;
        Age = age;
    }
    
    public UserModel()
    {
        
    }
}
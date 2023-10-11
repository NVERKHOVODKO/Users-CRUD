using TestApplication.Models;

namespace TestApplication.DTO
{
    /// <summary>
    /// Response model for retrieving a user.
    /// </summary>
    public class UserGetResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the list of roles associated with the user.
        /// </summary>
        public List<RoleModel> Roles { get; set; }
    }
}
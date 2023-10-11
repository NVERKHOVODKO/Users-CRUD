namespace TestApplication.DTO
{
    /// <summary>
    /// Request model for creating a new user.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int Age { get; set; }
    }
}
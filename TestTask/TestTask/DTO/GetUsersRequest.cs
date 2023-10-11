namespace TestApplication.DTO
{
    /// <summary>
    /// Request model for retrieving a list of users with pagination.
    /// </summary>
    public class GetUsersRequest
    {
        /// <summary>
        /// Gets or sets the number of users to include in each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number to retrieve.
        /// </summary>
        public int PageNumber { get; set; }
    }
}
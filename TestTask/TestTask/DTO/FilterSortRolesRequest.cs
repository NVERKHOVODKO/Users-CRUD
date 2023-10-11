using TestApplication.Models;
using TestTask.Enums;
using System.Collections.Generic;

namespace TestApplication.DTO
{
    /// <summary>
    /// Request model for filtering and sorting roles.
    /// </summary>
    public class FilterSortRolesRequest
    {
        /// <summary>
        /// Gets or sets the list of selected roles for filtering.
        /// </summary>
        public List<string> SelectedRoles { get; set; }

        /// <summary>
        /// Gets or sets the field to use for sorting.
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Gets or sets the sort direction (ascending or descending).
        /// </summary>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the page size for pagination.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        public int PageNumber { get; set; }

        public FilterSortRolesRequest(List<string> selectedRoles, string sortField, SortDirection sortDirection, int pageSize, int pageNumber)
        {
            SelectedRoles = selectedRoles;
            SortField = sortField;
            SortDirection = sortDirection;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
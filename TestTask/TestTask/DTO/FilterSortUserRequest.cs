using System.Collections.Generic;
using TestTask.Enums;

namespace TestApplication.DTO
{
    /// <summary>
    /// Request model for filtering and sorting users.
    /// </summary>
    public class FilterSortUserRequest
    {
        /// <summary>
        /// Gets or sets the list of filter parameters for filtering users.
        /// </summary>
        public List<FilterParams> Filters { get; set; }

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

        public FilterSortUserRequest(List<FilterParams> filters, string sortField, SortDirection sortDirection, int pageSize, int pageNumber)
        {
            Filters = filters;
            SortField = sortField;
            SortDirection = sortDirection;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
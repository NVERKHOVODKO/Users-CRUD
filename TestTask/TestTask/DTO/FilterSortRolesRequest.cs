using TestApplication.Models;
using TestTask.Enums;

namespace TestApplication.DTO;

public class FilterSortRolesRequest
{
    public List<string> SelectedRoles { get; set; }
    public string SortField { get; set; }
    public SortDirection SortDirection { get; set; }
    public int PageSize { get; set; }
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
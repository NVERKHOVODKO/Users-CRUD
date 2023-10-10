using TestApplication.Models;
using TestTask.Enums;

namespace TestApplication.DTO;

public class FilterSortRolesRequest
{
    public List<string> SelectedRoles { get; set; }
    public string SortField { get; set; }
    public SortDirection SortDirection { get; set; }
}
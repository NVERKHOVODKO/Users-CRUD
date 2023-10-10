using TestTask.Enums;

namespace TestApplication.DTO;

public class FilterSortUserRequest
{
    public List<FilterParams> Filters { get; set; }
    public string SortField { get; set; }
    public SortDirection SortDirection { get; set; }
}
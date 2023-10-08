using TestTask.Enums;

namespace TestApplication.DTO;

public class FilterSortRequest
{
    public List<FilterParams> Filters { get; set; }
    public string SortField { get; set; }
    public SortDirection SortDirection { get; set; }
}
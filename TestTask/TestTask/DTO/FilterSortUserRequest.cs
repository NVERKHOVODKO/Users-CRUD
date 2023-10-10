using TestTask.Enums;

namespace TestApplication.DTO;

public class FilterSortUserRequest
{
    public List<FilterParams> Filters { get; set; }
    public string SortField { get; set; }
    public SortDirection SortDirection { get; set; }
    public int PageSize { get; set; }
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
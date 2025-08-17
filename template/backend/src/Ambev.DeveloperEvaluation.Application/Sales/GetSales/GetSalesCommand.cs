using MediatR;

public class GetSalesCommand : IRequest<GetSalesResultPaginated>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public bool? IsCancelled { get; set; }
    public string? OrderBy { get; set; } = "Date";
    public string? SortDirection { get; set; } = "asc";
}

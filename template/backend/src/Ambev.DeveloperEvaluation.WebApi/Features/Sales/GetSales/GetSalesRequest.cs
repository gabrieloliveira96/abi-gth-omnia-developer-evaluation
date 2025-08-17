using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Common;

public class GetSalesRequest : IRequest<PaginatedResponse<GetSalesResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public DateTime? MinDate { get; set; } 
    public DateTime? MaxDate { get; set; }
    public bool? IsCancelled { get; set; }
    public string? OrderBy { get; set; } = "Date";
    public bool Desc { get; set; } = false;
}

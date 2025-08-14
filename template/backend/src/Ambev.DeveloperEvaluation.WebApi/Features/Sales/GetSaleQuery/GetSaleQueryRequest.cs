using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Common;

public class GetSaleQueryRequest : IRequest<PaginatedResponse<GetSaleQueryResponse>>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? CustomerName { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}

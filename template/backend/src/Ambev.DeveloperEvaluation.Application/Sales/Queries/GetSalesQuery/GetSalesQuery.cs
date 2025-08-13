using MediatR;

public class GetSalesQuery : IRequest<List<SaleResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; } = "date";
    public string? OrderDirection { get; set; } = "asc";
}

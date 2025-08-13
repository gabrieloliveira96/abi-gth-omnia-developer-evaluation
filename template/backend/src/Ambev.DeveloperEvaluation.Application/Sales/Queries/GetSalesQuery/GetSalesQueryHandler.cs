using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, List<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<List<SaleResponse>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var query = _saleRepository.Query();

        // Ordenação dinâmica
        query = request.OrderBy?.ToLower() switch
        {
            "date" or _ => request.OrderDirection == "desc"
                ? query.OrderByDescending(s => s.Date)
                : query.OrderBy(s => s.Date)
        };

        // Paginação
        var skip = (request.PageNumber - 1) * request.PageSize;
        var sales = await query
            .Include(s => s.Items) 
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
            
            return sales.Select(s => new SaleResponse
            {
                Id = s.Id,
                Date = s.Date,
                CustomerName = s.CustomerName,
                BranchName = s.BranchName,
                TotalAmount = s.TotalAmount,
                Canceled = s.IsCancelled,
                Items = s.Items.Select(i => new SaleItemResponse
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
        }).ToList();

    }
}

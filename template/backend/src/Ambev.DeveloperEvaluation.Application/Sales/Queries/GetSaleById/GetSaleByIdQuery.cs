using MediatR;

public record GetSaleByIdQuery(Guid Id) : IRequest<SaleDetailsResponse>;

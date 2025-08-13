using MediatR;

public record CreateSaleCommand(CreateSaleRequest Sale) : IRequest<Guid>;

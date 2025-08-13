using MediatR;

public record CancelSaleCommand(Guid Id) : IRequest<bool>;

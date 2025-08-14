using MediatR;

public record CancelSaleCommand : IRequest<bool>
{
    public Guid Id { get; }
    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}

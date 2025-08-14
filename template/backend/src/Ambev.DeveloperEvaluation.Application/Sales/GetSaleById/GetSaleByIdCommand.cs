using MediatR;

public record GetSaleByIdCommand : IRequest<GetSaleByIdResult?>
{

    public Guid Id { get; }
    
    public GetSaleByIdCommand(Guid id)
    {
        Id = id;
    }
}

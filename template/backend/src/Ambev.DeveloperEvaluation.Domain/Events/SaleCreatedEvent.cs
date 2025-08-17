using MediatR;

public class SaleCreatedEvent : INotification
{
    public Sale Sale { get; }
    public DateTime OccurredOn { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
        OccurredOn = DateTime.UtcNow;
    }
}
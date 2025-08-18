using MediatR;

public class SaleCreatedEvent : INotification
{
    public Sale Sale { get; }
    public IReadOnlyCollection<SaleItem> Items { get; }
    public DateTime OccurredOn { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
        Items = sale.Items.ToList();
        OccurredOn = DateTime.UtcNow;
    }
}
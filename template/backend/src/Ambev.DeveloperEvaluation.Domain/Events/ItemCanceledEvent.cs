using MediatR;

public class ItemCanceledEvent :INotification
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }
    public string Reason { get; }
    public DateTime OccurredOn { get; }


    public ItemCanceledEvent(Guid saleId, Guid itemId, string reason)
    {
        SaleId = saleId;
        ItemId = itemId;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}

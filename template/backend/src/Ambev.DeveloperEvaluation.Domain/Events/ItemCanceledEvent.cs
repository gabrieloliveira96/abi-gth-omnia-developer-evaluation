public class ItemCanceledEvent
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }
    public string Reason { get; }

    public ItemCanceledEvent(Guid saleId, Guid itemId, string reason)
    {
        SaleId = saleId;
        ItemId = itemId;
        Reason = reason;
    }
}

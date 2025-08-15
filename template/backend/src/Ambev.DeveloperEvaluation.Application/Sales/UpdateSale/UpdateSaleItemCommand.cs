public class UpdateSaleItemCommand
{
    public Guid? Id { get; set; } = null;
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

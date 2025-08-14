public class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount => (UnitPrice * Quantity) - Discount;
    public Sale Sale { get; set; } = null!;

    private SaleItem() { }

    public SaleItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = CalculateDiscount(quantity, unitPrice);
    }

    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity < 4)
            return 0;

        if (quantity >= 4 && quantity < 10)
            return quantity * unitPrice * 0.10m;

        if (quantity >= 10 && quantity <= 20)
            return quantity * unitPrice * 0.20m;

        throw new InvalidOperationException("Desconto inválido para a quantidade fornecida.");
    }
}

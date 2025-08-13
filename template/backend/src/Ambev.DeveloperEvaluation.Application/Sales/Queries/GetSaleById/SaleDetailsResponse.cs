public class SaleDetailsResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string BranchId { get; set; }
    public string BranchName { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<SaleItemDetailsResponse> Items { get; set; }
}

public class SaleItemDetailsResponse
{
     public int ProductId { get; set; }
    public   string ProductName { get; set; }
    public      int    Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
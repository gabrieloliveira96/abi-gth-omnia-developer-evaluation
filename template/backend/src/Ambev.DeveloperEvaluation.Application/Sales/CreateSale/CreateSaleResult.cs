
public class CreateSaleResult
{
      public Guid Id { get; set; }
      public DateTime Date { get; set; }
      public string CustomerId { get; set; }
      public string CustomerName { get; set; }
      public string BranchId { get; set; }
      public string BranchName { get; set; }
      public List<CreateSaleItemResult> Items { get; set; }
}

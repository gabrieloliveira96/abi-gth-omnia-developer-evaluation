
using Ambev.DeveloperEvaluation.Domain.Common;

public class Sale : BaseEntity
{
    public DateTime Date { get; private set; }
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public string BranchId { get; private set; }
    public string BranchName { get; private set; }
    public decimal TotalAmount => Items.Sum(i => i.TotalAmount);
    public bool IsCancelled { get; private set; }
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();


    private Sale() { }

    public Sale(DateTime date, string customerId, string customerName, string branchId, string branchName)
    {
        Id = Guid.NewGuid();
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        IsCancelled = false;
    }

    public void AddItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new InvalidOperationException("The sale cannot contain more than 20 units of the same product");

        var item = new SaleItem(productId, productName, quantity, unitPrice);
        _items.Add(item);
    }

    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Item already cancelled.");
        IsCancelled = true;
    }
    public void Update(DateTime date, string customerId, string customerName, string branchId, string branchName)
    {
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
    }
}

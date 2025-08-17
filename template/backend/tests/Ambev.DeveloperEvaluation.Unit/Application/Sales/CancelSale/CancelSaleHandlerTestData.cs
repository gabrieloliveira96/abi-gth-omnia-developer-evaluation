
public static class CancelSaleHandlerTestData
{
    public static CancelSaleCommand GenerateValidCommand()
    {
        return new CancelSaleCommand(Guid.NewGuid());
    }

    public static CancelSaleCommand GenerateInvalidCommand()
    {
        return new CancelSaleCommand(Guid.Empty);
    }

    public static Sale GenerateValidSale(Guid? id = null)
    {
        var sale = new Sale(
            DateTime.UtcNow,
            "CUST-123",
            "Cliente Teste",
            "BR-001",
            "Filial Teste");

        if (id.HasValue)
            typeof(Sale).GetProperty("Id")!.SetValue(sale, id.Value);
        else
            typeof(Sale).GetProperty("Id")!.SetValue(sale, Guid.NewGuid());

        sale.AddItem(1, "Produto A", 1, 10);
        sale.AddItem(2, "Produto B", 2, 20);

        return sale;
    }

    public static Sale GenerateAlreadyCancelledSale(Guid id)
    {
        var sale = GenerateValidSale(id);
        sale.Cancel();
        return sale;
    }
}

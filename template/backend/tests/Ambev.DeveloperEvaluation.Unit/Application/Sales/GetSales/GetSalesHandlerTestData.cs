public static class GetSalesHandlerTestData
{
    public static GetSalesCommand GenerateDefaultCommand()
    {
        return new GetSalesCommand
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Date",
            SortDirection = "asc"
        };
    }

    public static GetSalesCommand GenerateFilteredCommand()
    {
        return new GetSalesCommand
        {
            CustomerName = "Cliente 1",
            BranchName = "Filial A",
            MinDate = DateTime.UtcNow.AddDays(-10),
            MaxDate = DateTime.UtcNow.AddDays(1),
            PageNumber = 1,
            PageSize = 5,
            OrderBy = "CustomerName",
            SortDirection = "desc"
        };
    }

    public static List<Sale> GenerateSalesList(int count)
    {
        var sales = new List<Sale>();
        for (int i = 0; i < count; i++)
        {
            var sale = new Sale(
                DateTime.UtcNow.AddDays(-i),
                $"CUST{i:D3}",
                $"Cliente {i}",
                $"BR{i:D3}",
                $"Filial {i}");

            typeof(Sale).GetProperty("Id")!.SetValue(sale, Guid.NewGuid());
            sales.Add(sale);
        }

        return sales;
    }
}

using Bogus;

public static class UpdateSaleHandlerTestData
{
    private static readonly Faker<UpdateSaleCommand> faker = new Faker<UpdateSaleCommand>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.CustomerId, f => Guid.NewGuid().ToString())
        .RuleFor(c => c.CustomerName, f => f.Name.FullName())
        .RuleFor(c => c.BranchId, f => Guid.NewGuid().ToString())
        .RuleFor(c => c.BranchName, f => f.Company.CompanyName())
        .RuleFor(c => c.Date, f => f.Date.Recent())
        .RuleFor(c => c.Items, f => new List<UpdateSaleItemCommand>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = 1,
                ProductName = "Produto A",
                Quantity = 2,
                UnitPrice = 10
            }
        });

    public static UpdateSaleCommand GenerateValidCommand() => faker.Generate();
}

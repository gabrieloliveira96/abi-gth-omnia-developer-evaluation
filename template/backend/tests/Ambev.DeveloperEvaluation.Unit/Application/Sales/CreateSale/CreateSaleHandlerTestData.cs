using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> faker = new Faker<CreateSaleCommand>()
        .RuleFor(c => c.CustomerId, f => Guid.NewGuid().ToString())
        .RuleFor(c => c.CustomerName, f => f.Name.FullName())
        .RuleFor(c => c.BranchId, f => Guid.NewGuid().ToString())
        .RuleFor(c => c.BranchName, f => f.Company.CompanyName())
        .RuleFor(c => c.Date, f => f.Date.Recent())
        .RuleFor(c => c.Items, f => new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = 1,
                ProductName = "Produto A",
                Quantity = 2,
                UnitPrice = 10
            },
            new CreateSaleItemCommand
            {
                ProductId = 2,
                ProductName = "Produto B",
                Quantity = 1,
                UnitPrice = 15
            }
        });

    public static CreateSaleCommand GenerateValidCommand() => faker.Generate();
}

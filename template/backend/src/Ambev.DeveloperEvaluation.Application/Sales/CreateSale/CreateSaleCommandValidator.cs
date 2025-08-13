using FluentValidation;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Sale.CustomerId).NotEmpty();
        RuleFor(x => x.Sale.BranchId).NotEmpty();
        RuleFor(x => x.Sale.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Sale.Items).NotEmpty();

        RuleForEach(x => x.Sale.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId).GreaterThan(0);
            items.RuleFor(i => i.ProductName).NotEmpty();
            items.RuleFor(i => i.Quantity).InclusiveBetween(1, 20);
            items.RuleFor(i => i.UnitPrice).GreaterThan(0);
        });
    }
}

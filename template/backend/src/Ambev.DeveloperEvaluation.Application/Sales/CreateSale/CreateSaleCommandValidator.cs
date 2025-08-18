using FluentValidation;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(s => s.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.");

        RuleFor(s => s.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(s => s.BranchName)
            .NotEmpty().WithMessage("Branch name is required.");

        RuleFor(s => s.Date)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(s => s.Items)
            .NotEmpty().WithMessage("The sale must contain at least one item.");

        RuleForEach(s => s.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");

            items.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            items.RuleFor(i => i.Quantity)
                .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        });
    }
}

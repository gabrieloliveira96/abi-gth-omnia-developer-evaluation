using FluentValidation;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

        RuleFor(s => s.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

        RuleFor(s => s.BranchId)
            .NotEmpty().WithMessage("O ID da filial é obrigatório.");

        RuleFor(s => s.BranchName)
            .NotEmpty().WithMessage("O nome da filial é obrigatório.");

        RuleFor(s => s.Date)
            .NotEmpty().WithMessage("A data da venda é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data da venda não pode ser futura.");

        RuleFor(s => s.Items)
            .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

        RuleForEach(s => s.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");

            items.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.");

            items.RuleFor(i => i.Quantity)
                .InclusiveBetween(1, 20).WithMessage("A quantidade deve ser entre 1 e 20.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
        });
    }
}

using FluentValidation;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Sale.CustomerId)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

        RuleFor(x => x.Sale.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

        RuleFor(x => x.Sale.BranchId)
            .NotEmpty().WithMessage("O ID da filial é obrigatório.");

        RuleFor(x => x.Sale.BranchName)
            .NotEmpty().WithMessage("O nome da filial é obrigatório.");

        RuleFor(x => x.Sale.Date)
            .NotEmpty().WithMessage("A data da venda é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data da venda não pode ser futura.");

        RuleFor(x => x.Sale.Items)
            .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

        RuleForEach(x => x.Sale.Items).ChildRules(items =>
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

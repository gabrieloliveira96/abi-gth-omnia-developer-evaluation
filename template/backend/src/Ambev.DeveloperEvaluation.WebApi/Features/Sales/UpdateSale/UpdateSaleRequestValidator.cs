using FluentValidation;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID da venda é obrigatório.");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("O nome da filial é obrigatório.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");

            items.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.");

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
        });
    }
}

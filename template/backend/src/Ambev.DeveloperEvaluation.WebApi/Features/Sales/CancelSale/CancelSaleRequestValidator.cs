using FluentValidation;

public class DeleteSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    public DeleteSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}

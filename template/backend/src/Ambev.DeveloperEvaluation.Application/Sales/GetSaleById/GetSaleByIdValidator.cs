using FluentValidation;

public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdCommand>
{

    public GetSaleByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}

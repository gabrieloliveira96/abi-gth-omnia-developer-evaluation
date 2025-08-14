using FluentValidation;

public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdRequest>
{
    public GetSaleByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}

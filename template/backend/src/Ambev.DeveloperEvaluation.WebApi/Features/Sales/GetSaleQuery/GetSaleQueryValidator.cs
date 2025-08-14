using FluentValidation;

public class GetSaleQueryValidator : AbstractValidator<GetSaleQueryRequest>
{
    public GetSaleQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Size).GreaterThan(0).LessThanOrEqualTo(100);
    }
}

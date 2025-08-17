using FluentValidation;

public class GetSaleValidator : AbstractValidator<GetSalesRequest>
{
    public GetSaleValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

        RuleFor(x => x.MaxDate)
            .GreaterThanOrEqualTo(x => x.MinDate)
            .When(x => x.MinDate.HasValue && x.MaxDate.HasValue);

        RuleFor(x => x.OrderBy)
            .Must(BeAValidProperty)
            .When(x => !string.IsNullOrWhiteSpace(x.OrderBy))
            .WithMessage("Invalid field for ordering.");
    }

    private bool BeAValidProperty(string? field)
    {
        var validFields = new[]
        {
            "Date", "CustomerName", "BranchName", "TotalAmount"
        };

        return validFields.Contains(field!);
    }
}

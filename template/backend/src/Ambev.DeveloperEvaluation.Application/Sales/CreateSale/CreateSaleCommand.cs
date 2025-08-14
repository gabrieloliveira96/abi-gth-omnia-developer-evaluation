using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    public DateTime Date { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string BranchId { get; set; }
    public string BranchName { get; set; }
    public List<CreateSaleItemResult> Items { get; set; }

    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

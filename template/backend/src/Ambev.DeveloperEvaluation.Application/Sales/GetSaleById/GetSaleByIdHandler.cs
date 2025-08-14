using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdCommand, GetSaleByIdResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;


    public GetSaleByIdQueryHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }


    public async Task<GetSaleByIdResult?> Handle(GetSaleByIdCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetSaleByIdValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errorMessages);
        }

        var sale = await _saleRepository.GetByIdAsync(command.Id,cancellationToken);

        if (sale == null)
            return null;

        return _mapper.Map<GetSaleByIdResult>(sale);
    }
}

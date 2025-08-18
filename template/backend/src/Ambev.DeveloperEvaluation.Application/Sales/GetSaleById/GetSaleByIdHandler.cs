using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdCommand, GetSaleByIdResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleByIdHandler> _logger;


    public GetSaleByIdHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSaleByIdHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<GetSaleByIdResult?> Handle(GetSaleByIdCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var validator = new GetSaleByIdValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errorMessages);
            }

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");
            return _mapper.Map<GetSaleByIdResult>(sale);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }
    }
}

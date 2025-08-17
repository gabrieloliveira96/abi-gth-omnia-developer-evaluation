using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMapper _mapper;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogInformation("validationResult: {errorMessages}",errorMessages);
            throw new ValidationException(errorMessages);
        }

        var sale = _mapper.Map<Sale>(command);

        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }
        sale.AddDomainEvent(new SaleCreatedEvent(sale));

        var saleCreate = await _saleRepository.CreateAsync(sale);

        var result = _mapper.Map<CreateSaleResult>(saleCreate);
        
        return result; ;  
    }
}

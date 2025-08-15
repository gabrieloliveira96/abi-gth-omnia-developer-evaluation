using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        ILogger<UpdateSaleHandler> logger,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errorMessages);
        }

        var sale = await _saleRepository.GetByIdAsync(command.Id);
        if (sale == null)
            throw new InvalidOperationException("Sale not found");
        
        if(sale.IsCancelled)
            throw new InvalidOperationException("Sale is canceled");

        // Atualiza os dados principais da venda
        sale.Update(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);

        // Itens existentes
        var existingItems = sale.Items.ToList();

        // Processa os itens recebidos
        foreach (var itemCmd in command.Items)
        {
            if (itemCmd.Id.HasValue)
            {
                // Atualiza item existente
                var existingItem = existingItems.FirstOrDefault(x => x.Id == itemCmd.Id.Value);
                if (existingItem != null)
                {
                    existingItem.Update(itemCmd.ProductId, itemCmd.ProductName, itemCmd.Quantity, itemCmd.UnitPrice);
                }
            }
            else
            {
                // Adiciona novo item
                sale.AddItem(itemCmd.ProductId, itemCmd.ProductName, itemCmd.Quantity, itemCmd.UnitPrice);
            }
        }

        // Cancela itens que não estão mais na request
        var idsFromRequest = command.Items.Where(i => i.Id.HasValue).Select(i => i.Id.Value).ToHashSet();
        var toCancel = existingItems.Where(x => !idsFromRequest.Contains(x.Id) && !x.IsCancelled).ToList();

        foreach (var item in toCancel)
        {
            if(!item.IsCancelled)
                item.Cancel();

            var itemCanceledEvent = new ItemCanceledEvent(sale.Id, item.Id, "Item removed during update");
            _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}, ItemId: {ItemId}",
                nameof(ItemCanceledEvent), sale.Id, item.Id);
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale);

        var saleUpdatedEvent = new SaleUpdatedEvent(updatedSale);
        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleUpdatedEvent), sale.Id);

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}

using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events;

public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedEvent>
{
    private readonly ILogger<SaleUpdatedEventHandler> _logger;
    private readonly IEventStore _eventStore;


    public SaleUpdatedEventHandler(ILogger<SaleUpdatedEventHandler> logger, IEventStore eventStore)
    {
        _logger = logger;
        _eventStore = eventStore;
    }

    public async Task Handle(SaleUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var sale = notification.Sale;

            await _eventStore.SaveAsync(notification, cancellationToken);

            _logger.LogInformation("SaleUpdatedEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
                sale.Id, sale.CustomerName, sale.TotalAmount);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }
    }
}

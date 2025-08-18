using MediatR;
using Microsoft.Extensions.Logging;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly IEventStore _eventStore;


    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger, IEventStore eventStore)
    {
        _logger = logger;
        _eventStore = eventStore;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var sale = notification.Sale;

            await _eventStore.SaveAsync(notification, cancellationToken);

            _logger.LogInformation("SaleCreatedEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
                sale.Id, sale.CustomerName, sale.TotalAmount);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }
    }
}

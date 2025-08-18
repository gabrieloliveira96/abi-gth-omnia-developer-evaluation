using MediatR;
using Microsoft.Extensions.Logging;

public class ItemCanceledEventHandler : INotificationHandler<ItemCanceledEvent>
{
    private readonly ILogger<ItemCanceledEvent> _logger;
    private readonly IEventStore _eventStore;

    public ItemCanceledEventHandler(ILogger<ItemCanceledEvent> logger, IEventStore eventStore)
    {
        _logger = logger;
        _eventStore = eventStore;
    }

    public async Task Handle(ItemCanceledEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var saleId = notification.SaleId;
            var itemId = notification.ItemId;
            var reason = notification.Reason;

            await _eventStore.SaveAsync(notification, cancellationToken);

            _logger.LogInformation("ItemCanceledEvent handled: SaleId={saleId},ItemId={itemId}, reason={reason}",
                saleId, itemId, reason);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }

    }
}

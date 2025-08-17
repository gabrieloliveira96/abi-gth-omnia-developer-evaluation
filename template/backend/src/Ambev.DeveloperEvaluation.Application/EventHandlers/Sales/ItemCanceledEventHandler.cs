using MediatR;
using Microsoft.Extensions.Logging;

public class ItemCanceledEventHandler : INotificationHandler<ItemCanceledEvent>
{
    private readonly ILogger<ItemCanceledEvent> _logger;

    public ItemCanceledEventHandler(ILogger<ItemCanceledEvent> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCanceledEvent notification, CancellationToken cancellationToken)
    {
        var saleId = notification.SaleId;
        var itemId = notification.ItemId;
        var reason = notification.Reason;

        _logger.LogInformation("ItemCanceledEvent handled: SaleId={saleId},ItemId={itemId}, reason={reason}",
            saleId, itemId, reason);

        return Task.CompletedTask;
    }
}

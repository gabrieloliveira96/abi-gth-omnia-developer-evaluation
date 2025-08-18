using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class SaleCanceledEventHandler : INotificationHandler<SaleCanceledEvent>
{
    private readonly ILogger<SaleCanceledEventHandler> _logger;
    private readonly IEventStore _eventStore;

    public SaleCanceledEventHandler(ILogger<SaleCanceledEventHandler> logger, IEventStore eventStore)
    {
        _logger = logger;
        _eventStore = eventStore;
    }

    public async Task Handle(SaleCanceledEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;

        await _eventStore.SaveAsync(notification, cancellationToken);

        _logger.LogInformation("SaleCanceledEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
            sale.Id, sale.CustomerName, sale.TotalAmount);
    }
}

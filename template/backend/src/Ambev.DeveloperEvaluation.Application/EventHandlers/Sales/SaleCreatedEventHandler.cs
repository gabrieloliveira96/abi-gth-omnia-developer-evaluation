using MediatR;
using Microsoft.Extensions.Logging;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;

        _logger.LogInformation("SaleCreatedEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
            sale.Id, sale.CustomerName, sale.TotalAmount);

        return Task.CompletedTask;
    }
}

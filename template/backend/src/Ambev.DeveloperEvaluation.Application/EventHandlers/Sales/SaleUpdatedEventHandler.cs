using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events;

public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedEvent>
{
    private readonly ILogger<SaleUpdatedEventHandler> _logger;

    public SaleUpdatedEventHandler(ILogger<SaleUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;

        _logger.LogInformation("SaleUpdatedEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
            sale.Id, sale.CustomerName, sale.TotalAmount);

        return Task.CompletedTask;
    }
}

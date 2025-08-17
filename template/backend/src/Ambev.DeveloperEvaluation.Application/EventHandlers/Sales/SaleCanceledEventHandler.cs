using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class SaleCanceledEventHandler : INotificationHandler<SaleCanceledEvent>
{
    private readonly ILogger<SaleCanceledEventHandler> _logger;

    public SaleCanceledEventHandler(ILogger<SaleCanceledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCanceledEvent notification, CancellationToken cancellationToken)
    {
        var sale = notification.Sale;

        _logger.LogInformation("SaleCanceledEvent handled: SaleId={SaleId}, Customer={Customer}, Total={Total}",
            sale.Id, sale.CustomerName, sale.TotalAmount);

        return Task.CompletedTask;
    }
}

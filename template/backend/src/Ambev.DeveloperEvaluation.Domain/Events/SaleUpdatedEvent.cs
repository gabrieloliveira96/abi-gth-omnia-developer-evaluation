using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleUpdatedEvent: INotification
    {
        public Sale Sale { get; }
        public IReadOnlyCollection<SaleItem> Items { get; }
        public DateTime OccurredOn { get; }


        public SaleUpdatedEvent(Sale sale)
        {
            Sale = sale;
            Items = sale.Items.ToList();
            OccurredOn = DateTime.UtcNow;
        }
    }
}

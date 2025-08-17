using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleUpdatedEvent: INotification
    {
        public Sale Sale { get; }
        public DateTime OccurredOn { get; }


        public SaleUpdatedEvent(Sale sale)
        {
            Sale = sale;
            OccurredOn = DateTime.UtcNow;
        }
    }
}

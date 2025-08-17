using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCanceledEvent : INotification
    {
        public Sale Sale { get; }
        public DateTime OccurredOn { get; }


        public SaleCanceledEvent(Sale sale)
        {
            Sale = sale;
            OccurredOn = DateTime.UtcNow;   
        }
    }
}

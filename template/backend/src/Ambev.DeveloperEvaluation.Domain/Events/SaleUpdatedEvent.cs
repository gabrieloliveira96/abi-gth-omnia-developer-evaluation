namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleUpdatedEvent
    {
        public Sale Sale { get; }

        public SaleUpdatedEvent(Sale sale)
        {
            Sale = sale;
        }
    }
}

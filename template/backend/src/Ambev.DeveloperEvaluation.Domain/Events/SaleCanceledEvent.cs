namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCanceledEvent
    {
        public Sale Sale { get; }

        public SaleCanceledEvent(Sale sale)
        {
            Sale = sale;
        }
    }
}

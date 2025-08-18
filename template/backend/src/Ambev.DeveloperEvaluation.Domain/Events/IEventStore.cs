    public interface IEventStore
    {
        Task SaveAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
    }
public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale,CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale sale,CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(Guid id,CancellationToken cancellationToken = default);
    IQueryable<Sale> Query(CancellationToken cancellationToken);
}

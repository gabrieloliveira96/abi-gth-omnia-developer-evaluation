public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    Task<Sale?> GetByIdAsync(Guid id);
    IQueryable<Sale> Query();

}

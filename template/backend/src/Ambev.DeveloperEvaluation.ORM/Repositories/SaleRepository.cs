using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public IQueryable<Sale> Query()
    {
        return _context.Sales.AsNoTracking();
    }

}

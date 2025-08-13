using Ambev.DeveloperEvaluation.ORM;

public class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;

    public UnitOfWork(DefaultContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}

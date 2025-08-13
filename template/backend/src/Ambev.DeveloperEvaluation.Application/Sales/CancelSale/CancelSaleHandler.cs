using MediatR;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id);

        if (sale == null || sale.IsCancelled)
            return false;

        sale.Cancel();

        await _unitOfWork.CommitAsync();

        // Aqui poderíamos logar/publishar o evento SaleCancelled
        return true;
    }
}

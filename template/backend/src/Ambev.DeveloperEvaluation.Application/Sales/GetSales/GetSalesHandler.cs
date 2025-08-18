
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResultPaginated>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSalesHandler> _logger;
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetSalesResultPaginated> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var query = _saleRepository.Query(cancellationToken);

            if (!string.IsNullOrEmpty(command.CustomerName))
                query = query.Where(x => x.CustomerName.ToLower().Contains(command.CustomerName.ToLower()));

            if (!string.IsNullOrEmpty(command.BranchName))
                query = query.Where(x => x.BranchName.ToLower().Contains(command.BranchName.ToLower()));

            if (command.MinDate.HasValue)
                query = query.Where(x => x.Date >= command.MinDate.Value.Date);

            if (command.MaxDate.HasValue)
                query = query.Where(x => x.Date <= command.MaxDate.Value.Date);

            if (command.IsCancelled.HasValue)
                query = query.Where(x => x.IsCancelled == command.IsCancelled.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var orderBy = !string.IsNullOrEmpty(command.OrderBy) ? command.OrderBy : "Date";
            var direction = command.SortDirection?.ToLower() == "desc" ? "descending" : "ascending";
            query = query.OrderBy($"{orderBy} {direction}");

            var sales = await query
                .Skip((command.PageNumber - 1) * command.PageSize)
                .Take(command.PageSize)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<GetSalesResult>>(sales);

            return new GetSalesResultPaginated
            {
                Data = mapped,
                TotalCount = totalCount
            };
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }
    }
}

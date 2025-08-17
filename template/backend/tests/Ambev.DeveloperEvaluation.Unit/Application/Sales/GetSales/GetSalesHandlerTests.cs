using AutoMapper;
using FluentAssertions;
using MockQueryable;
using NSubstitute;
using Xunit;

public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;

    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSalesHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given no filters When handling GetSales Then returns all paginated results")]
    public async Task Handle_NoFilters_ReturnsPaginatedResults()
    {
        var command = GetSalesHandlerTestData.GenerateDefaultCommand();

        var sales = GetSalesHandlerTestData.GenerateSalesList(15);

        var queryableMock = sales.BuildMock().AsQueryable();

        _saleRepository.Query(Arg.Any<CancellationToken>()).Returns(queryableMock);

        _mapper.Map<List<GetSalesResult>>(Arg.Any<List<Sale>>())
            .Returns(callInfo =>
            {
                var inputSales = callInfo.Arg<List<Sale>>();
                return inputSales.Select(s => new GetSalesResult
                {
                    Id = s.Id,
                    Date = s.Date,
                    CustomerId = s.CustomerId,
                    CustomerName = s.CustomerName,
                    BranchId = s.BranchId,
                    BranchName = s.BranchName,
                    IsCancelled = s.IsCancelled,
                    TotalAmount = s.TotalAmount
                }).ToList();
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(command.PageSize);
        result.TotalCount.Should().Be(15);
    }

    [Fact(DisplayName = "Given filters When handling GetSales Then returns filtered results")]
    public async Task Handle_WithFilters_ReturnsFilteredResults()
    {
        var command = GetSalesHandlerTestData.GenerateFilteredCommand();

        var sales = Enumerable.Range(1, 10).Select(i =>
            new Sale(DateTime.UtcNow.AddDays(-1), $"C{i}", "Cliente 1", $"B{i}", "Filial A")
        ).ToList();

        var queryable = sales.BuildMock().AsQueryable();

        _saleRepository.Query(Arg.Any<CancellationToken>()).Returns(queryable);

        _mapper.Map<List<GetSalesResult>>(Arg.Any<List<Sale>>())
            .Returns(callInfo =>
            {
                var inputSales = callInfo.Arg<List<Sale>>();
                return inputSales.Select(s => new GetSalesResult
                {
                    Id = s.Id,
                    Date = s.Date,
                    CustomerId = s.CustomerId,
                    CustomerName = s.CustomerName,
                    BranchId = s.BranchId,
                    BranchName = s.BranchName,
                    IsCancelled = s.IsCancelled,
                    TotalAmount = s.TotalAmount
                }).ToList();
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Data.Should().OnlyContain(x =>
            x.CustomerName == "Cliente 1" &&
            x.BranchName == "Filial A");
    }
}

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class GetSaleByIdHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleByIdQueryHandler _handler;

    public GetSaleByIdHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleByIdQueryHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid sale ID When getting sale Then returns mapped result")]
    public async Task Handle_ValidRequest_ReturnsSale()
    {
        
        var command = GetSaleByIdTestData.ValidCommand();
        var sale = new Sale(DateTime.UtcNow, "C123", "Cliente", "B001", "Filial");
        typeof(Sale).GetProperty("Id")!.SetValue(sale, command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);

        var expectedResult = new GetSaleByIdResult { Id = sale.Id };
        _mapper.Map<GetSaleByIdResult>(sale).Returns(expectedResult);

        var result = await _handler.Handle(command, default);

        result.Should().NotBeNull();
        result!.Id.Should().Be(command.Id);
    }

    [Fact(DisplayName = "Given non-existent sale ID When getting sale Then returns null")]
    public async Task Handle_SaleNotFound_ReturnsNull()
    {
        
        var command = GetSaleByIdTestData.ValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var result = await _handler.Handle(command, default);

        result.Should().BeNull();
    }

    [Fact(DisplayName = "Given invalid sale ID When getting sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        
        var command = GetSaleByIdTestData.InvalidCommand();

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Sale ID is required*");
    }
}

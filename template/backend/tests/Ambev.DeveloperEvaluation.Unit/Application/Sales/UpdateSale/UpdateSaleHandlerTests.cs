using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _handler = new UpdateSaleHandler(_saleRepository, _logger, _mapper);
    }

    [Fact(DisplayName = "Given valid update command When sale is updated Then returns updated result")]
    public async Task Handle_ValidRequest_ReturnsUpdatedResult()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        sale.GetType().GetProperty(nameof(Sale.Id))!.SetValue(sale, command.Id);

        sale.AddItem(1, "Produto A", 2, 10);

        _saleRepository.GetByIdAsync(command.Id).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(new UpdateSaleResult { Id = sale.Id });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        sale.DomainEvents.Should().ContainSingle(e => e is SaleUpdatedEvent);
    }

    [Fact(DisplayName = "Given non-existent sale When updating Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id).Returns((Sale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale not found");
    }

    [Fact(DisplayName = "Given canceled sale When updating Then throws exception")]
    public async Task Handle_SaleIsCancelled_ThrowsException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        sale.GetType().GetProperty(nameof(Sale.Id))!.SetValue(sale, command.Id);

        sale.Cancel();

        _saleRepository.GetByIdAsync(command.Id).Returns(sale);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale is canceled");
    }

    [Fact(DisplayName = "Given invalid update command When handling Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = new UpdateSaleCommand();

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }


}

using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _handler = new CancelSaleHandler(_saleRepository, _logger);
    }

    [Fact(DisplayName = "Given valid sale ID When cancelling sale Then cancels and returns true")]
    public async Task Handle_ValidRequest_CancelsSaleAndItems()
    {
        var sale = new Sale(DateTime.UtcNow, "C123", "Cliente", "B001", "Filial");
        sale.AddItem(1, "Produto A", 1, 10);
        sale.AddItem(2, "Produto B", 2, 20);

        var saleId = Guid.NewGuid();
        typeof(Sale).GetProperty(nameof(Sale.Id))!.SetValue(sale, saleId);

        _saleRepository.GetByIdAsync(saleId).Returns(sale);

        var command = new CancelSaleCommand(saleId);

        var result = await _handler.Handle(command, default);

        result.Should().BeTrue();
        sale.IsCancelled.Should().BeTrue();
        sale.Items.Should().OnlyContain(i => i.IsCancelled);

        sale.DomainEvents.Should().ContainSingle(e => e is SaleCanceledEvent);
        sale.Items.SelectMany(i => i.DomainEvents).OfType<ItemCanceledEvent>().Should().HaveCount(2);

        await _saleRepository.Received(1).UpdateAsync(sale);
    }

    [Fact(DisplayName = "Given invalid sale ID When cancelling Then returns false")]
    public async Task Handle_SaleNotFound_ReturnsFalse()
    {
        var command = new CancelSaleCommand(Guid.NewGuid());
        _saleRepository.GetByIdAsync(command.Id).Returns((Sale?)null);

        var result = await _handler.Handle(command, default);

        result.Should().BeFalse();
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given already canceled sale When cancelling again Then returns false")]
    public async Task Handle_SaleAlreadyCancelled_ReturnsFalse()
    {
        var sale = new Sale(DateTime.UtcNow, "C123", "Cliente", "B001", "Filial");
        var saleId = Guid.NewGuid();
        typeof(Sale).GetProperty(nameof(Sale.Id))!.SetValue(sale, saleId);
        sale.Cancel();

        _saleRepository.GetByIdAsync(saleId).Returns(sale);

        var command = new CancelSaleCommand(saleId);

        var result = await _handler.Handle(command, default);

        result.Should().BeFalse();
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given invalid command When handling Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var invalidCommand = new CancelSaleCommand(Guid.Empty);

        Func<Task> act = async () => await _handler.Handle(invalidCommand, default);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Sale ID is required*");
    }
}

using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleItemHandler> _logger;
    private readonly CancelSaleItemHandler _handler;

    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _logger = Substitute.For<ILogger<CancelSaleItemHandler>>();
        _handler = new CancelSaleItemHandler(_saleRepository, _logger);
    }

    [Fact(DisplayName = "Given valid command When cancelling item Then returns true and emits event")]
    public async Task Handle_ValidRequest_CancelsItemAndEmitsEvent()
    {
    
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        var sale = new Sale(DateTime.UtcNow, "C123", "Cliente", "B001", "Filial");
        typeof(Sale).GetProperty("Id")!.SetValue(sale, command.SaleId);

        sale.AddItem(10, "Produto A", 2, 50);
        var item = sale.Items.First(i => i.ProductId == 10);

        typeof(SaleItem).GetProperty("Id")!.SetValue(item, command.ItemId);

        _saleRepository.GetByIdAsync(command.SaleId).Returns(sale);

        var result = await _handler.Handle(command, default);

        result.Should().BeTrue();
        item.IsCancelled.Should().BeTrue();

        item.DomainEvents.Should().ContainSingle(e => e is ItemCanceledEvent);
        var domainEvent = item.DomainEvents.OfType<ItemCanceledEvent>().First();
        domainEvent.Reason.Should().Be("Item canceled");

        await _saleRepository.Received(1).UpdateAsync(sale);
    }

    [Fact(DisplayName = "Given invalid command When handling Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
    
        var command = CancelSaleItemHandlerTestData.GenerateInvalidCommand();

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Sale ID is required*");
    }

    [Fact(DisplayName = "Given non-existent sale When cancelling item Then throws InvalidOperationException")]
    public async Task Handle_SaleNotFound_ThrowsInvalidOperationException()
    {
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.SaleId).Returns((Sale?)null);

        var act = async () => await _handler.Handle(command, default);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale not found");

        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given non-existent item in sale When cancelling Then throws InvalidOperationException")]
    public async Task Handle_ItemNotFound_ThrowsInvalidOperationException()
    {
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        var sale = new Sale(DateTime.UtcNow, "C123", "Cliente", "B001", "Filial");
        typeof(Sale).GetProperty("Id")!.SetValue(sale, command.SaleId);

        _saleRepository.GetByIdAsync(command.SaleId).Returns(sale);

        var act = async () => await _handler.Handle(command, default);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Item not found");

        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
    }

}

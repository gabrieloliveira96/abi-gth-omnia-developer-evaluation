using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;
    

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, Substitute.For<ILogger<CreateSaleHandler>>());
    }

    [Fact(DisplayName = "Given valid sale When creating Then returns success result")]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        var result = new CreateSaleResult { Id = sale.Id, TotalAmount = sale.TotalAmount };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(sale);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);
        response.TotalAmount.Should().Be(35);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given invalid command When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new CreateSaleCommand();

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given valid sale When handling Then adds all items")]
    public async Task Handle_ValidRequest_AddsItemsToSale()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });

        await _handler.Handle(command, CancellationToken.None);

        sale.Items.Should().HaveSameCount(command.Items);
    }

    [Fact(DisplayName = "Given valid sale When handling Then adds SaleCreatedEvent")]
    public async Task Handle_ValidRequest_AddsSaleCreatedDomainEvent()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(command.Date, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });
        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(sale);

        await _handler.Handle(command, CancellationToken.None);

        sale.DomainEvents.Should().ContainSingle(e => e is SaleCreatedEvent);
    }
}

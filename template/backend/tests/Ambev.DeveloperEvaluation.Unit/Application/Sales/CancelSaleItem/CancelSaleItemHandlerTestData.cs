
public static class CancelSaleItemHandlerTestData
{
    public static CancelSaleItemCommand GenerateValidCommand()
    {
        return new CancelSaleItemCommand(
            saleId: Guid.NewGuid(),
            itemId: Guid.NewGuid()
        );
    }

    public static CancelSaleItemCommand GenerateInvalidCommand()
    {
        return new CancelSaleItemCommand(
            saleId: Guid.Empty,
            itemId: Guid.Empty
        );
    }
}

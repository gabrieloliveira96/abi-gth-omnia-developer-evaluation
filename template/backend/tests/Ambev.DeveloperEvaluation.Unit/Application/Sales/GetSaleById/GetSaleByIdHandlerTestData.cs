public static class GetSaleByIdTestData
{
    public static GetSaleByIdCommand ValidCommand()
    {
        return new GetSaleByIdCommand(Guid.NewGuid());
    }

    public static GetSaleByIdCommand InvalidCommand()
    {
        return new GetSaleByIdCommand(Guid.Empty);
    }
}

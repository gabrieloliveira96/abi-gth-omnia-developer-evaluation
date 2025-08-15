using AutoMapper;

public class CancelSaleItemProfile : Profile
{
    public CancelSaleItemProfile()
    {
        CreateMap<CancelSaleItemRequest, CancelSaleItemCommand>()
            .ConstructUsing(x => new CancelSaleItemCommand(x.SaleId, x.ItemId));
    }
}

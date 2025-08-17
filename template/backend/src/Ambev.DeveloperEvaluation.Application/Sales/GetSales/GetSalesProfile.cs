using AutoMapper;

public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSalesResult>();
        CreateMap<SaleItem, GetSalesItemResult>();
    }
}
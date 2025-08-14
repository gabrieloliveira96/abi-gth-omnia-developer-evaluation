using AutoMapper;

public class GetSaleByIdProfile : Profile
{
    public GetSaleByIdProfile()
    {
        CreateMap<Sale, GetSaleByIdResult>();
    }
}
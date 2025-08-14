using AutoMapper;

public class GetSaleQueryProfile : Profile
{
    public GetSaleQueryProfile()
    {
        CreateMap<Sale, GetSaleQueryResponse>();
    }
}

using AutoMapper;

public class GetSaleByIdProfile : Profile
{
    public GetSaleByIdProfile()
    {
        CreateMap<Guid, GetSaleByIdCommand>()
            .ConstructUsing(id => new GetSaleByIdCommand(id));

        CreateMap<GetSaleByIdResult, GetSaleByIdResponse>();
        CreateMap<GetSaleItemByIdResult, GetSaleItemByIdResponse>();
    }
}
using AutoMapper;

public class UpdateSaleProfile : Profile
{

    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<UpdateSaleItemRequest, UpdateSaleItem>();

    }
}

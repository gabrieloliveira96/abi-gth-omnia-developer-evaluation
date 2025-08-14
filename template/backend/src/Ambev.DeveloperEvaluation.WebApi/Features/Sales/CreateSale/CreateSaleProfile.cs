using AutoMapper;

public class CreateSaleProfile : Profile
{

    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleItemRequest, CreateSaleItem>();
    }
}

using AutoMapper;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());
        CreateMap<Sale, UpdateSaleResult>();
        
    }
}

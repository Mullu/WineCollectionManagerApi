using AutoMapper;
using WineCollectionManagerApi.Models;

namespace WineCollectionManagerApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WineBottleCreateModel, WineBottleModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<WineMakerCreateModel, WineMakerModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}

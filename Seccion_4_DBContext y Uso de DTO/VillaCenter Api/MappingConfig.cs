using AutoMapper;
using VillaCenter_Api.Models;
using VillaCenter_Api.Models.DTO;

namespace VillaCenter_Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa,VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

        }

    }
}

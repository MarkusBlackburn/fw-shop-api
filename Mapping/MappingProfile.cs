using AutoMapper;
using fw_shop_api.DTOs;
using fw_shop_api.Models.Domain;

namespace fw_shop_api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegisterDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
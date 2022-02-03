using AutoMapper;
using JobsityChatAPI.DTO.Users;
using JobsityChatAPI.Models;

namespace JobsityChatAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LocalUser, GetUserDto>();
            CreateMap<GetUserDto, LocalUser>();
            CreateMap<AddUserDto, LocalUser>();
        }
    }
}

using AutoMapper;
using MeFit_BE.Models.Domain.User;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.User;

namespace MeFit_BE.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<UserReadDTO, User>().ReverseMap();
            CreateMap<UserWriteDTO, User>().ReverseMap();
            CreateMap<UserEditDTO, User>().ReverseMap();
        }
    }
}

using AutoMapper;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.User;
using System.Linq;

namespace MeFit_BE.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDTO>()
                .ForMember(dto => dto.Goals, opt => opt
                           .MapFrom(user => user.Goals
                                    .Select(g => g.Id).ToList())) 
                .ReverseMap();

            CreateMap<User, UserAdminReadDTO>().ReverseMap();
            CreateMap<User, UserWriteDTO>().ReverseMap();
            CreateMap<User, UserEditDTO>().ReverseMap();
        }
    }
}

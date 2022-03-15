using AutoMapper;
using MeFit_BE.Models.DTO.Profile;

namespace MeFit_BE
{
    public class ProfileProfile: Profile
    {
        public ProfileProfile()
        {
            CreateMap<Models.Domain.UserDomain.Profile, ProfileReadDTO>().ReverseMap();
            CreateMap<Models.Domain.UserDomain.Profile, ProfileWriteDTO>().ReverseMap();
            CreateMap<Models.Domain.UserDomain.Profile, ProfileEditDTO>().ReverseMap();
        }
    }
}


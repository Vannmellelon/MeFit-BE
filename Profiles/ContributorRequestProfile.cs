using AutoMapper;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO.ContributorRequest;

namespace MeFit_BE.Profiles
{
    public class ContributorRequestProfile : AutoMapper.Profile
    {
        public ContributorRequestProfile()
        {
            CreateMap<ContributorRequest, ContributorRequestReadDTO>()
                  .ForMember(crDTO => crDTO.RequestingUser, opt => opt
                           .MapFrom(cr => cr.RequestingUserId));
        }
    }
}

using AutoMapper;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Goal;
using System;
using System.Linq;

namespace MeFit_BE.Profiles
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            
            CreateMap<Goal, GoalReadDTO>()
                .ForMember(g => g.SubGoals, opt => opt
                           .MapFrom(g => g.SubGoals
                           .Select(s => s.Id).ToList()))
                .ReverseMap();
            
            CreateMap<GoalWriteDTO, Goal>().ReverseMap();
            CreateMap<GoalEditDTO, Goal>().ReverseMap();
        }
    }
}

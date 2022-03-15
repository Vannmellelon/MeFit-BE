using AutoMapper;
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
                .ForMember(dto => dto.WorkoutPrograms, opt => opt
                           .MapFrom(goal => goal.WorkoutPrograms
                                            .Select(wp => wp.Id).ToList()))
                .ReverseMap();
            CreateMap<GoalWriteDTO, Goal>().ReverseMap();
            CreateMap<GoalEditDTO, Goal>().ReverseMap();
        }
    }
}

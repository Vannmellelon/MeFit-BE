using AutoMapper;
using MeFit_BE.Models.Domain.Workout;
using MeFit_BE.Models.DTO.Goal;
using System;
using System.Linq;

namespace MeFit_BE.Profiles
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalReadDTO>().ReverseMap();
            CreateMap<GoalWriteDTO, Goal>().ReverseMap();
            CreateMap<GoalEditDTO, Goal>().ReverseMap();
        }
    }
}

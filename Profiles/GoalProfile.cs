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
            CreateMap<Goal, GoalReadDTO>()
                // turning related program into int (id)
                .ForMember(gdto => gdto.Program, opt => opt
                .MapFrom(g => g.ProgramId))
                // turning related subgoals into int array
                .ForMember(gdto => gdto.SubGoals, opt => opt
                .MapFrom(g => g.SubGoals.Select(sg => sg.Id).ToArray()));
                //.ReverseMap();
            CreateMap<GoalWriteDTO, Goal>().ReverseMap();
            CreateMap<GoalEditDTO, Goal>().ReverseMap();
        }
    }
}

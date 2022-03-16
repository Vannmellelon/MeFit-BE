using AutoMapper;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.WorkoutProgram;
using System.Linq;

namespace MeFit_BE.Profiles
{
    public class WorkoutProgramProfile : Profile
    {
        public WorkoutProgramProfile()
        {
            CreateMap<WorkoutProgram, WorkoutProgramReadDTO>()
                .ForMember(wp => wp.Workouts, opt => opt
                           .MapFrom(wp => wp.Workouts
                           .Select(w => w.Id).ToList()))
                .ReverseMap();
            CreateMap<WorkoutProgramEditDTO, WorkoutProgram>().ReverseMap();
            CreateMap<WorkoutProgramWriteDTO, WorkoutProgram>().ReverseMap();
        }
    }
}

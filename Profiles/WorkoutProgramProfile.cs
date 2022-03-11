using AutoMapper;
using MeFit_BE.Models.Domain.Workout;
using MeFit_BE.Models.DTO.WorkoutProgram;

namespace MeFit_BE.Profiles
{
    public class WorkoutProgramProfile : Profile
    {
        public WorkoutProgramProfile()
        {
            CreateMap<WorkoutProgramReadDTO, WorkoutProgram>().ReverseMap();
            CreateMap<WorkoutProgramEditDTO, WorkoutProgram>().ReverseMap();
            CreateMap<WorkoutProgramWriteDTO, WorkoutProgram>().ReverseMap();
        }
    }
}

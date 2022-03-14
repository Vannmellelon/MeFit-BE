using AutoMapper;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Workout;

namespace MeFit_BE.Profiles
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile() 
        {
            CreateMap<WorkoutEditDTO, Workout>().ReverseMap();
            CreateMap<WorkoutReadDTO, Workout>().ReverseMap();
            CreateMap<WorkoutWriteDTO, Workout>().ReverseMap();
        }
    }
}

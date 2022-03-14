using AutoMapper;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Exercise;

namespace MeFit_BE.Profiles
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            CreateMap<ExerciseReadDTO, Exercise>().ReverseMap();
            CreateMap<ExerciseEditDTO, Exercise>().ReverseMap();
            CreateMap<ExerciseWriteDTO, Exercise>().ReverseMap();
        }
    }
}

using AutoMapper;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Workout;
using System.Linq;

namespace MeFit_BE.Profiles
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile() 
        {
            CreateMap<Workout, WorkoutEditDTO>().ReverseMap();

            CreateMap<Workout, WorkoutReadDTO>()
                .ForMember(wdto => wdto.SetIds, opt => opt
                    .MapFrom(w => w.Sets
                        .Select(s => s.Id)
                            .ToList()))
                .ReverseMap();

            CreateMap<Workout, WorkoutWriteDTO>().ReverseMap();
        }
    }
}

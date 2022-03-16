﻿using AutoMapper;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.DTO.SubGoal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Profiles
{
    public class SubGoalProfile : Profile
    {
        public SubGoalProfile()
        {
            CreateMap<SubGoal, SubGoalReadDTO>().ReverseMap();
            CreateMap<SubGoalWriteDTO, SubGoal>().ReverseMap();
            CreateMap<SubGoalEditDTO, SubGoal>().ReverseMap();
        }
    }
}

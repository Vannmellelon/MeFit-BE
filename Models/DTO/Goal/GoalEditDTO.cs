﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.DTO.Goal
{
    public class GoalEditDTO
    {
        public DateTime EndDate { get; set; }

        public bool Achieved { get; set; }
    }
}

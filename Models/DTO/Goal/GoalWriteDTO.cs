﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.DTO.Goal
{
    public class GoalWriteDTO
    {
        public DateTime EndData { get; set; }

        public bool Achieved { get; set; }

        public int UserId { get; set; }
    }
}

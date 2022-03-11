using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.DTO.SubGoal
{
    public class SubGoalWriteDTO
    {
        public bool Achieved { get; set; }

        public int Workout { get; set; }
    }
}

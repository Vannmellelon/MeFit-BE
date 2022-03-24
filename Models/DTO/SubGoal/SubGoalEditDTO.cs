using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.DTO.SubGoal
{
    public class SubGoalEditDTO
    {
        public bool Achieved { get; set; }

        public int WorkoutId { get; set; }

        public int GoalId { get; set; }
    }
}

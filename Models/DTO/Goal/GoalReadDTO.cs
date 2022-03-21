using System;
using System.Collections.Generic;
using MeFit_BE.Models.DTO.SubGoal;

namespace MeFit_BE.Models.DTO.Goal
{
    public class GoalReadDTO
    {
        public int Id { get; set; }

        public DateTime EndData { get; set; }

        public bool Achieved { get; set; }

        public ICollection<SubGoalReadDTO> SubGoals { get; set; } 
    }
}

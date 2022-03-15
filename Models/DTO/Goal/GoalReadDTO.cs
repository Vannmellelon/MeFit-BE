using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.DTO.Goal
{
    public class GoalReadDTO
    {
        public int Id { get; set; }

        public DateTime EndData { get; set; }

        public bool Achieved { get; set; }

        public List<int> WorkoutPrograms { get; set; } 
    }
}

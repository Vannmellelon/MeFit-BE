using System.Collections.Generic;

namespace MeFit_BE.Models.DTO.WorkoutProgram
{
    public class WorkoutProgramReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public List<int> Workouts { get; set; }
    }
}

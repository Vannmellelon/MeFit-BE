using System.Collections.Generic;

namespace MeFit_BE.Models.DTO.Workout
{
    public class WorkoutWriteDTO
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Difficulty { get; set; }

        public int ContributorId { get; set; }
    }
}

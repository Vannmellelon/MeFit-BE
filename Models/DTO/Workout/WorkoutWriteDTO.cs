using System.Collections.Generic;

namespace MeFit_BE.Models.DTO.Workout
{
    public class WorkoutWriteDTO
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int ContributorId { get; set; }

        //public List<int> SetIds { get; set; }
    }
}

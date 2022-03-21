using System.Collections.Generic;
using MeFit_BE.Models.Domain.WorkoutDomain;

namespace MeFit_BE.Models.DTO.Workout
{
    public class WorkoutReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ContributorId { get; set; }

        public string Category { get; set; }

        public string Difficulty { get; set; }

        public List<int> Sets { get; set; }
    }
}

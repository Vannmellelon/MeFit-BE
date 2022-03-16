using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("Workout")]
    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; }

        public ICollection<Set> Sets { get; set; }
      
        // FK to contributor
        public int ContributorId { get; set; }

        public User ContributedBy { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.UserDomain;


namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("WorkoutProgram")]
    public class WorkoutProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int ContributorId { get; set; }

        public User ContributedBy { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;


namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("WorkoutProgram")]
    public class WorkoutProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Difficulty Difficulty { get; set; }

        public int? ContributorId { get; set; }

        [ForeignKey("ContributorId")]
        public User Contributor { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;


namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("WorkoutProgram")]
    public class WorkoutProgram
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Category { get; set; }

        [StringLength(15)]
        public string Difficulty { get; set; }

        public int? ContributorId { get; set; }

        [ForeignKey("ContributorId")]
        public User Contributor { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}

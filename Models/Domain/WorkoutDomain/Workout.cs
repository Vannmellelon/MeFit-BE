using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("Workout")]
    public class Workout
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Category { get; set; }

        [StringLength(15)]
        public string Difficulty { get; set; }

        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; }

        public ICollection<Set> Sets { get; set; }
      
        public int? ContributorId { get; set; }

        [ForeignKey("ContributorId")]
        public User Contributor { get; set; }
    }
}

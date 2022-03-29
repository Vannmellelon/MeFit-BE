using MeFit_BE.Models.Domain.UserDomain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("Set")]
    public class Set
    {
        public int Id { get; set; }

        [Range(1, 100)]
        public int ExerciseRepetitions { get; set; }

        [ForeignKey("WorkoutId")]
        public Workout Workout { get; set; }

        public int? WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }

        public int? ExerciseId { get; set; }
    }
}

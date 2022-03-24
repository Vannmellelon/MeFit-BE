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

        public Workout Workout { get; set; }

        public int? WorkoutId { get; set; }

        public Exercise Exercise { get; set; }

        public int? ExerciseId { get; set; }

    }
}

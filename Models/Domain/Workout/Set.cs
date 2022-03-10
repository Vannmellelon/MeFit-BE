using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.Workout;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Set")]
    public class Set
    {
        public int Id { get; set; }

        public string ExerciseRepetitions { get; set; }

        public int? ExcersiseId { get; set; }

        public Exercise Exercise { get; set; } 

    }
}
